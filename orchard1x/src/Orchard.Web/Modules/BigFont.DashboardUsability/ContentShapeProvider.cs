using Orchard.ContentManagement;
using Orchard.DisplayManagement.Descriptors;
using Orchard.DisplayManagement.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BigFont.DashboardUsability
{
    public class ContentShapeProvider : IShapeTableProvider
    {
        public void Discover(ShapeTableBuilder builder)
        {            
            builder.Describe("Content")
                .OnCreating(creating =>
                {
                    // do stuff to the shape                  
                })
                .OnCreated(created =>
                {
                    // do stuff to the shape                  
                })
                .OnDisplaying(displaying =>
                {
                    // do stuff to the shape
                })
                .OnDisplayed(displayed =>
                {
                    // do stuff to the shape
                });
        }
    }
}