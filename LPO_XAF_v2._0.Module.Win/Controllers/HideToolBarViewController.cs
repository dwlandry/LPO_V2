//-----------------------------------------------------------------------
// <copyright file="F:\Users\dlandry\source\repos\LPO_XAF_v2.0\LPO_XAF_v2._0.Module.Win\Controllers\HideToolBarViewController.cs" company="David W. Landry III">
//     Author: _**David Landry**_
//     *Copyright (c) David W. Landry III. All rights reserved.*
// </copyright>
//-----------------------------------------------------------------------
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Win.Templates;
using DevExpress.XtraBars;
using LPO_XAF_v2._0.Module.BusinessObjects.Instrument;
using System;
using System.Linq;

namespace LPO_XAF_v2._0.Module.Win.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class HideToolBarViewController : ViewController
    {
        public HideToolBarViewController()
        {
            InitializeComponent();
            //RegisterActions(components);
            //TargetViewType = ViewType.ListView;
            //TargetViewNesting = Nesting.Nested;
            //TargetObjectType = typeof(VendorDocument);

        }
        protected override void OnActivated()
        {
            base.OnActivated();
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
            //NestedFrameTemplate template = Frame.Template as NestedFrameTemplate;
            //if (template != null)
            //    SetNestedToolbarVisibility(template, false);
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
        //private void SetNestedToolbarVisibility(NestedFrameTemplate template, bool visible)
        //{
        //    foreach (Bar bar in template.BarManager.Bars)
        //        if (bar.BarName == "ListView Toolbar")
        //        {
        //            bar.Visible = visible;
        //            break;
        //        }
        //}
    }
}
