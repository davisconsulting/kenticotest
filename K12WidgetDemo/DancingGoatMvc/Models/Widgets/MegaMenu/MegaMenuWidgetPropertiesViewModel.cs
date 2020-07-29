using CMS.DocumentEngine;
using Kentico.Components.Web.Mvc.FormComponents;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace DancingGoat.Models.Widgets.MegaMenu
{
    public class MegaMenuWidgetPropertiesViewModel
    {
        public List<SelectListItem> AvailableSite { get; set; }
        public string PageType { get; set; }
        public List<SelectListItem> AvailableTypes { get; set; }
        public string Site { get; set; }
        public bool Visible { get; set; } = true;
        public bool SelectOnlyPublished { get; set; } = false;
        public List<PathSelectorItem> Path { get; set; }
        public string MaximumNestingLevel { get; set; } = "-1";
        public string OrderBy { get; set; } = "";
        public string ContentBefore { get; set; } = "";
        public string ContetAfter { get; set; } = "";
        public List<MenuItemViewModel> Menus { get; set; }
    }

    public class MenuItemViewModel {
    
        public string NodeDisplayName { get; set; }
        public int NodeID { get; set; }
        public int ParentNodeID { get; set; }
        public string RedirectionURL { get; set; }
        public bool NodeHasChild { get; set; }
        public int? NodeLevel { get; set; }
        public int NodeOrder { get; set; }
        public int? ParentNodeLevel { get; set; }
        public List<MenuItemViewModel> ChildMenu { get; set; }
    }



  }