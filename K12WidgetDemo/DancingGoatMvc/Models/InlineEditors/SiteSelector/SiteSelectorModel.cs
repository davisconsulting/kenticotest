using System.Collections.Generic;
using System.Web.Mvc;

namespace DancingGoat.Models.InlineEditors.SiteSelector
{
    public class SiteSelectorModel
    {
        public string PropertyName { get; set; }
        public string Value { get; set; }
        public List<SelectListItem> Types { get; set; }
    }
}