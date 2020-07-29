using Kentico.Components.Web.Mvc.FormComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DancingGoat.Models.Widgets
{
    public class MegaMenuViewModel
    {
        public string DisplayName { get; set; }

        public string Icon { get; set; }

        public string Caption { get; set; }

        public string Page { get; set; }

        public string URL { get; set; }

        public string PageType { get; set; } = "";

        
        public bool Visible { get; set; } = true;

        
        public bool SelectOnlyPublished { get; set; } = false;

        
        public IList<PathSelectorItem> Path { get; set; }

        
        public string MaximumNestingLevel { get; set; } = "-1";

       
        public string OrderBy { get; set; } = "";

        
        public string ContentBefore { get; set; } = "";

        
        public string ContetAfter { get; set; } = "";
    }
}