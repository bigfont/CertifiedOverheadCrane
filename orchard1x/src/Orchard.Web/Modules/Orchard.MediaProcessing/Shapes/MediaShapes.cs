﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Hosting;
using Orchard.ContentManagement;
using Orchard.DisplayManagement;
using Orchard.Environment;
using Orchard.FileSystems.Media;
using Orchard.Forms.Services;
using Orchard.Logging;
using Orchard.MediaProcessing.Descriptors.Filter;
using Orchard.MediaProcessing.Media;
using Orchard.MediaProcessing.Models;
using Orchard.MediaProcessing.Services;
using Orchard.Tokens;
using Orchard.Utility.Extensions;

namespace Orchard.MediaProcessing.Shapes {
    
    public class MediaShapes : IDependency {
        private readonly Work<IStorageProvider> _storageProvider;
        private readonly Work<IImageProcessingFileNameProvider> _fileNameProvider;
        private readonly Work<IImageProfileService> _profileService;
        private readonly Work<IImageProcessingManager> _processingManager;
        private readonly Work<IOrchardServices> _services;
        private readonly Work<ITokenizer> _tokenizer;

        public MediaShapes(
            Work<IStorageProvider> storageProvider, 
            Work<IImageProcessingFileNameProvider> fileNameProvider, 
            Work<IImageProfileService> profileService, 
            Work<IImageProcessingManager> processingManager, 
            Work<IOrchardServices> services,
            Work<ITokenizer> tokenizer) {
            _storageProvider = storageProvider;
            _fileNameProvider = fileNameProvider;
            _profileService = profileService;
            _processingManager = processingManager;
            _services = services;
            _tokenizer = tokenizer;
            Logger = NullLogger.Instance;
        }

        public ILogger Logger { get; set; }

        [Shape]
        public void ResizeMediaUrl(dynamic Shape, dynamic Display, TextWriter Output, ContentItem ContentItem, string Path, int Width, int Height, string Mode, string Alignment, string PadColor) {
            var state = new Dictionary<string, string> {
                {"Width", Width.ToString(CultureInfo.InvariantCulture)},
                {"Height", Height.ToString(CultureInfo.InvariantCulture)},
                {"Mode", Mode},
                {"Alignment", Alignment},
                {"PadColor", PadColor},
            };

            var filter = new FilterRecord {
                Category = "Transform",
                Type = "Resize",
                State = FormParametersHelper.ToString(state)
            };

            var profile = "Transform_Resize"
                + "_w_" + Convert.ToString(Width) 
                + "_h_" + Convert.ToString(Height) 
                + "_m_" + Convert.ToString(Mode)
                + "_a_" + Convert.ToString(Alignment) 
                + "_c_" + Convert.ToString(PadColor);

            MediaUrl(Shape, Display, Output, profile, Path, ContentItem, filter);
        }

        [Shape]
        public void MediaUrl(dynamic Shape, dynamic Display, TextWriter Output, string Profile, string Path, ContentItem ContentItem, FilterRecord CustomFilter) {
            try {
                Shape.IgnoreShapeTracer = true;
                var services = new Lazy<IOrchardServices>(() => _services.Value);
                var storageProvider = new Lazy<IStorageProvider>(() => _storageProvider.Value);

                // try to load the processed filename from cache
                var filePath = _fileNameProvider.Value.GetFileName(Profile, Path);
                bool process = false;

                // if the filename is not cached, process it
                if (string.IsNullOrEmpty(filePath)) {
                    Logger.Debug("FilePath is null, processing required, profile {0} for image {1}", Profile, Path); 

                    process = true;
                }
            
                    // the processd file doesn't exist anymore, process it
                else if (!storageProvider.Value.FileExists(filePath)) {
                    Logger.Debug("Processed file no longer exists, processing required, profile {0} for image {1}", Profile, Path); 

                    process = true;
                }

                // if the original file is more recent, process it
                else {
                    DateTime pathLastUpdated;
                    if (TryGetImageLastUpdated(Path, out pathLastUpdated)) {
                        var filePathLastUpdated = storageProvider.Value.GetFile(filePath).GetLastUpdated();
                            
                        if (pathLastUpdated > filePathLastUpdated)
                        {
                            Logger.Debug("Original file more recent, processing required, profile {0} for image {1}", Profile, Path);

                            process = true;
                        }
                    }
                }

                // todo: regenerate the file if the profile is newer, by deleting the associated filename cache entries.
                if (process) {
                    Logger.Debug("Processing profile {0} for image {1}", Profile, Path);
                    
                    ImageProfilePart profilePart;

                    if (CustomFilter == null) {
                        profilePart = _profileService.Value.GetImageProfileByName(Profile);
                        if (profilePart == null)
                            return;
                    }
                    else {
                        profilePart = services.Value.ContentManager.New<ImageProfilePart>("ImageProfile");
                        profilePart.Filters.Add(CustomFilter);
                    }

                    using (var image = GetImage(Path)) {
                        
                        var filterContext = new FilterContext { Media = image, FilePath = storageProvider.Value.Combine(Profile, CreateDefaultFileName(Path)) };

                        var tokens = new Dictionary<string, object>();
                        // if a content item is provided, use it while tokenizing
                        if (ContentItem != null) {
                            tokens.Add("Content", ContentItem);
                        }

                        foreach (var filter in profilePart.Filters.OrderBy(f => f.Position)) {
                            var descriptor = _processingManager.Value.DescribeFilters().SelectMany(x => x.Descriptors).FirstOrDefault(x => x.Category == filter.Category && x.Type == filter.Type);
                            if (descriptor == null)
                                continue;

                            var tokenized = _tokenizer.Value.Replace(filter.State, tokens);
                            filterContext.State = FormParametersHelper.ToDynamic(tokenized);
                            descriptor.Filter(filterContext);
                        }

                        _fileNameProvider.Value.UpdateFileName(Profile, Path, filterContext.FilePath);

                        if (!filterContext.Saved) {
                            storageProvider.Value.TryCreateFolder(profilePart.Name);
                            var newFile = storageProvider.Value.OpenOrCreate(filterContext.FilePath);
                            using (var imageStream = newFile.OpenWrite()) {
                                using (var sw = new BinaryWriter(imageStream)) {
                                    if (filterContext.Media.CanSeek) {
                                        filterContext.Media.Seek(0, SeekOrigin.Begin);
                                    }
                                    using (var sr = new BinaryReader(filterContext.Media)) {
                                        int count;
                                        var buffer = new byte[8192];
                                        while ((count = sr.Read(buffer, 0, buffer.Length)) != 0) {
                                            sw.Write(buffer, 0, count);
                                        }
                                    }
                                }
                            }
                        }

                        filterContext.Media.Dispose();
                        filePath = filterContext.FilePath;
                    }
                }

                // generate a timestamped url to force client caches to update if the file has changed
                var publicUrl = storageProvider.Value.GetPublicUrl(filePath);
                var timestamp = storageProvider.Value.GetFile(storageProvider.Value.GetLocalPath(filePath)).GetLastUpdated().Ticks;
                Output.Write(publicUrl + "?v=" + timestamp.ToString(CultureInfo.InvariantCulture));
            }
            catch (Exception ex) {
                Logger.Error(ex, "An error occured while rendering shape {0} for image {1}", Profile, Path);
            }
        }

        private enum ImagePathType
        {
            StorageProvider,
            AbsoluteUrl,
            AppRelative,
            Invalid
        }

        private ImagePathType GetImagePathType(string path) {
            // /OrchardLocal/images/my-image.jpg
            if (Uri.IsWellFormedUriString(path, UriKind.Relative)) {
                return ImagePathType.StorageProvider;
            }

            // http://blob.storage-provider.net/my-image.jpg
            if (Uri.IsWellFormedUriString(path, UriKind.Absolute)) {
                return ImagePathType.AbsoluteUrl;
            }

            // ~/Media/Default/images/my-image.jpg
            if (VirtualPathUtility.IsAppRelative(path)) {
                return ImagePathType.AppRelative;
            }

            return ImagePathType.Invalid;
        }

        // TODO: Update this method once the storage provider has been updated
        private Stream GetImage(string path) {
            var storageProvider = new Lazy<IStorageProvider>(() => _storageProvider.Value);
            var services = new Lazy<IOrchardServices>(() => _services.Value);
            var webClient = new Lazy<WebClient>(() => new WebClient());

            var request = services.Value.WorkContext.HttpContext.Request;

            switch (GetImagePathType(path)) {
                case ImagePathType.StorageProvider:
                    path = storageProvider.Value.GetLocalPath(path);

                    // images/my-image.jpg
                    var file = storageProvider.Value.GetFile(path);
                    return file.OpenRead();

                case ImagePathType.AbsoluteUrl:
                    return webClient.Value.OpenRead(new Uri(path));

                case ImagePathType.AppRelative:
                    return webClient.Value.OpenRead(new Uri(request.Url, VirtualPathUtility.ToAbsolute(path)));

                default:
                    throw new ArgumentException("invalid path");
            }
        }

        private bool TryGetImageLastUpdated(string path, out DateTime lastUpdated) {
            var imagePathType = GetImagePathType(path);
            switch (imagePathType)
            {
                case ImagePathType.StorageProvider:
                    path = _storageProvider.Value.GetLocalPath(path);

                    var file = _storageProvider.Value.GetFile(path);
                    lastUpdated = file.GetLastUpdated();

                    return true;

                case ImagePathType.AppRelative:
                    var serverPath = HostingEnvironment.MapPath(path);
                    lastUpdated = File.GetLastWriteTime(serverPath);

                    return true;

                default:
                    Logger.Warning("Cannot get last updated time for {0}, {1}", imagePathType, path);

                    lastUpdated = DateTime.MinValue;
                    return false;
            }
        }

        private static string CreateDefaultFileName(string path) {
            var extention = Path.GetExtension(path);
            var newPath = Path.ChangeExtension(path, "");
            newPath = newPath.Replace(@"/", "_");
            return newPath.ToSafeName() + extention;
        }
    }
}