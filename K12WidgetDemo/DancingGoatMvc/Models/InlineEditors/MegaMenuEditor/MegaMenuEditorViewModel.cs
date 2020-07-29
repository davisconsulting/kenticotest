using Kentico.Components.Web.Mvc.FormComponents;
using Kentico.Forms.Web.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DancingGoat.Models.InlineEditors.MegaMenuEditor
{
    public class MegaMenuEditorViewModel : InlineEditorViewModel
    {
        public string PageType { get; set; } = "";

        public string Site { get; set; } = "";

        [EditingComponent("Kentico.CheckBox", Label = "Visible", Order = 0, Tooltip = "Indicates if the widget should be displayed.")]
        public bool Visible { get; set; } = true;

        [EditingComponent("Kentico.TextInput", Label = "Maximum nesting level", Order = 6, Tooltip = "Specifies the maximum number of content tree sub-levels from which the content is to be loaded. This number is relative, i.e. it is counted from the beginning of the path specified for the content of the web part. Entering -1 causes all sub-levels to be included.")]
        [Range(-1, 100, ErrorMessage = "Please enter valid number")]
        public string MaximumNestingLevel { get; set; } = "-1";

        [EditingComponent("Kentico.TextInput", Label = "Order by", Order = 7, Tooltip = "Sets the value of the ORDER BY clause in the SELECT statement used to retrieve the content. You can specify only the columns common to all of the selected page types.")]
        public string OrderBy { get; set; } = "";

        [EditingComponent("Kentico.TextArea", Label = "Content before", Order = 12, Tooltip = "HTML content placed before the widget. Can be used to display a header or add some encapsulating code, such as <div> or <table> elements to achieve the required layout.")]
        public string ContentBefore { get; set; } = "";

        [EditingComponent("Kentico.TextArea", Label = "Content after", Order = 13, Tooltip = "HTML content placed after the widget. Can be used to display a footer or close the tags contained in the ContentBefore value, such as </div> or </table> elements.")]
        public string ContetAfter { get; set; } = "";
    }
}