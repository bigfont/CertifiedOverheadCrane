using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using BigFont.CraneProduct.Models;

namespace BigFont.CraneProduct.Handlers
{
    public class CraneProductHandler : ContentHandler
    {
        public CraneProductHandler(IRepository<CraneProductRecord> repository)
        {
            Filters.Add(StorageFilter.For(repository));
        }
    }
}