using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.FormEngine;
using CMS.Localization;
using CMS.PortalEngine;
using CMS.SiteProvider;
using DancingGoat.Controllers.Widgets;
using DancingGoat.Infrastructure;
using DancingGoat.Models.Widgets;
using DancingGoat.Models.Widgets.MegaMenu;
using Kentico.PageBuilder.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
[assembly: RegisterWidget("DancingGoat.HomePage.MegaMenuWidget", typeof(MegaMenuWidgetController), "{$dancinggoatmvc.widget.MegaMenu.name$}", Description = "{$dancinggoatmvc.widget.MegaMenu.description$}", IconClass = "icon-picture")]
namespace DancingGoat.Controllers.Widgets
{
    public class MegaMenuWidgetController : WidgetController<MegaMenuWidgetProperties>
    {
        // GET: MegaMenuWidget
        private readonly IOutputCacheDependencies outputCacheDependencies;

        public MegaMenuWidgetController(IOutputCacheDependencies outputCacheDependencies)
        {
            this.outputCacheDependencies = outputCacheDependencies;
        }

        public MegaMenuWidgetController(IComponentPropertiesRetriever<MegaMenuWidgetProperties> propertiesRetriever, ICurrentPageRetriever currentPageRetriever, IOutputCacheDependencies outputCacheDependencies)
            : base(propertiesRetriever, currentPageRetriever)
        {
            this.outputCacheDependencies = outputCacheDependencies;
        }


        public ActionResult Index()
        {
            var properties = GetProperties();
            var types = DocumentTypeHelper.GetDocumentTypeClasses()
                    .WhereEquals("ClassIsContentOnly", true)
                    .TypedResult
                    .ToList();
            var pagetypes = new List<SelectListItem>();

            pagetypes.Add(new SelectListItem()
            {
                Text = "Select Page Type",
                Value = "0",
                Selected = (0 == 0)
            });

            foreach (var type in types)
            {
                pagetypes.Add(new SelectListItem()
                {
                    Text = type.ClassDisplayName,
                    Value = type.ClassName,
                    Selected = (type.ClassName == properties.PageType)
                });
            }
            var siteList = new List<SelectListItem>();
            var sites = SiteInfoProvider.GetSites().TypedResult.ToList();
            foreach (var site in sites)
            {
                siteList.Add(new SelectListItem()
                {
                    Text = site.DisplayName,
                    Value = site.DisplayName,
                    Selected = (site.DisplayName == properties.Site)
                });
                properties.Site = site.DisplayName;
            }

           
            var treenodedata = GetParticularTreeNodeData(properties.PageType, string.Empty, properties.Site, "100", string.Empty, string.Empty, properties.OrderBy, properties.MaximumNestingLevel, true);



            var menus = treenodedata.Where(x => x.NodeLevel == 1).Select(x => new MenuItemViewModel {
                NodeDisplayName = (x.DocumentName == "/" ? "Home" : x.DocumentName),
                NodeHasChild = x.NodeHasChildren,
                NodeID = x.NodeID,
                ParentNodeID = x.NodeParentID,
                RedirectionURL = x.RelativeURL,
                NodeLevel = x.NodeLevel,
                NodeOrder = x.NodeOrder                

            }).ToList();

            if(menus.Count==0 && treenodedata.Count > 0)
            {
                menus = treenodedata.Select(x => new MenuItemViewModel
                {
                    NodeDisplayName = (x.DocumentName == "/" ? "Home" : x.DocumentName),
                    NodeHasChild = x.NodeHasChildren,
                    NodeID = x.NodeID,
                    ParentNodeID = x.NodeParentID,
                    RedirectionURL = x.RelativeURL,
                    NodeLevel = x.NodeLevel,
                    NodeOrder = x.NodeOrder

                }).ToList();

            }

            foreach (var item in menus)
            {
                item.ChildMenu = GetChildMenuItem(item, treenodedata, CMS.Helpers.ValidationHelper.GetInteger(properties.MaximumNestingLevel,1));
            }
            return PartialView("Widgets/_MegaMenuWidget", new MegaMenuWidgetPropertiesViewModel
            {
                ContetAfter = properties.ContetAfter,
                ContentBefore = properties.ContentBefore,
                MaximumNestingLevel = properties.MaximumNestingLevel,
                PageType = properties.PageType,
                Site = properties.Site,
                Menus = menus,
                AvailableTypes = pagetypes,
                AvailableSite = siteList,
                Visible=properties.Visible
            });
        }

        public List<MenuItemViewModel> GetChildMenuItem(MenuItemViewModel parentMenu, List<TreeNode> treeNodes, int maxNodeLevel)
        {
            var childNodes = new List<MenuItemViewModel>();
            if (maxNodeLevel > parentMenu.NodeLevel)
            {
                parentMenu.ChildMenu = treeNodes.Where(x => x.NodeLevel == parentMenu.NodeLevel + 1 && x.NodeParentID==parentMenu.NodeID).Select(x => new MenuItemViewModel
                {
                    NodeDisplayName = (x.DocumentName == "/" ? "Home" : x.DocumentName),
                    NodeHasChild = x.NodeHasChildren,
                    NodeID = x.NodeID,
                    ParentNodeID = x.NodeParentID,
                    RedirectionURL = x.RelativeURL,
                    NodeLevel = x.NodeLevel,
                    NodeOrder = x.NodeOrder
                }).ToList();

                if (maxNodeLevel > parentMenu.NodeLevel + 1)
                {
                    foreach (var item in parentMenu.ChildMenu)
                    {
                        item.ChildMenu = GetChildMenuItem(item, treeNodes, maxNodeLevel);
                    }
                }
                childNodes = parentMenu.ChildMenu;
            }

            return childNodes;
        }

        public List<TreeNode> GetParticularTreeNodeData(string className, string selectedPath, string siteName, string count, string columns,string whereCondition,
                                                        string orderBy, string maxNestingLevel, bool selectOnlyPublish)
        {
            className = className != "0" ? className : "";
            List<TreeNode> treeNodeList = new List<TreeNode>();
            if (DocumentTypeHelper.GetDocumentTypeClasses().WhereEquals("ClassName", "CMS.Root").TypedResult.Count<DataClassInfo>() >= 1)
            {
                TreeProvider tree = new TreeProvider();
                count = count == null ? "0" : count;
                int maxRelativeLevel = int.Parse(maxNestingLevel == null ? "-1" : maxNestingLevel);
                columns = columns == null ? "" : columns;
                whereCondition = whereCondition == null ? "" : whereCondition;
                orderBy = orderBy == null ? "" : orderBy;
                string cultureCode = LocalizationContext.CurrentCulture.CultureCode == null ? "en-Us" : LocalizationContext.CurrentCulture.CultureCode;
                string siteNameFromProperties = siteName == null ? SiteContext.CurrentSiteName : siteName;
                //selectedPath = !string.IsNullOrEmpty(selectedPath) ? selectedPath+ "/%" : "/%";
                selectedPath += "/%";
                treeNodeList = DocumentHelper.GetDocuments(siteNameFromProperties, selectedPath, cultureCode, false, className, whereCondition, orderBy, maxRelativeLevel, selectOnlyPublish, int.Parse(count), columns, tree).ToList<TreeNode>();
            }
            return treeNodeList;
        }
    }
}