using Orchard.UI.Resources;

namespace BigFont.DealerDashboard {
    public class ResourceManifest : IResourceManifestProvider {
        public void BuildManifests(ResourceManifestBuilder builder) {
            var manifest = builder.Add();
            manifest.DefineStyle("Temp").SetUrl("temp.css");
        }
    }
}
