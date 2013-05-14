using Orchard.UI.Resources;

namespace BigFont.OpenDashboard {
    public class ResourceManifest : IResourceManifestProvider {
        public void BuildManifests(ResourceManifestBuilder builder) {
            var manifest = builder.Add();
            manifest.DefineStyle("OpenDashboard").SetUrl("open-dashboard.css");
        }
    }
}
