using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using LPO_XAF_v2._0.Module.BusinessObjects.Instrument;
using LPO_XAF_v2._0.Module.BusinessObjects.Project;

namespace LPO_XAF_v2._0.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class InitializeProjectForIProjectDrawingViewController : ViewController
    {
        private NewObjectViewController controller;
        public InitializeProjectForIProjectDrawingViewController()
        {
            InitializeComponent();
            TargetObjectType = typeof(IProjectDrawing);
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
            controller = Frame.GetController<NewObjectViewController>();
            if (controller != null)
                controller.ObjectCreated += Controller_ObjectCreated;
        }

        private void Controller_ObjectCreated(object sender, ObjectCreatedEventArgs e)
        {
            NestedFrame nestedFrame = Frame as NestedFrame;
            if (nestedFrame != null)
            {
                IProjectDrawing projectDrawing = e.CreatedObject as IProjectDrawing;
                if (projectDrawing != null)
                {
                    Instrument instrument = ((NestedFrame)Frame).ViewItem.CurrentObject as Instrument;
                    if (instrument != null)
                    {
                        Project project = (Project)((UnitOfWork)((Drawing)projectDrawing).Session).GetObjectByKey(typeof(Project), instrument.Project.Oid);
                        projectDrawing.Project = project;
                    }
                }
            }
        }

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            if (controller != null)
                controller.ObjectCreated -= Controller_ObjectCreated;
            base.OnDeactivated();
        }
    }
}
