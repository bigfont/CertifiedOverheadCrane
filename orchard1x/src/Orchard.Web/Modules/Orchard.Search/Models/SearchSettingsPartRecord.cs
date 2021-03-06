using Orchard.ContentManagement.Records;

namespace Orchard.Search.Models {
    public class SearchSettingsPartRecord : ContentPartRecord {
        public SearchSettingsPartRecord() {
            FilterCulture = false;
            SearchedFields = "body, title";
        }
        
        public virtual bool FilterCulture { get; set; }
        public virtual string SearchedFields { get; set; }
        public virtual string SearchIndex { get; set; }
    }
}