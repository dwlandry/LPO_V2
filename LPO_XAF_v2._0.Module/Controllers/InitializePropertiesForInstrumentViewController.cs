﻿using System;
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
    public partial class InitializePropertiesForInstrumentViewController : ViewController
    {
        private NewObjectViewController controller;
        public InitializePropertiesForInstrumentViewController()
        {
            InitializeComponent();
            TargetObjectType = typeof(Instrument);
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
                Instrument instrument = e.CreatedObject as Instrument;
                if (instrument != null)
                {
                    PID pid = ((NestedFrame)Frame).ViewItem.CurrentObject as PID;
                    if (pid != null)
                    {
                        Project project = (Project)((UnitOfWork)instrument.Session).GetObjectByKey(typeof(Project), pid.Project.Oid);
                        instrument.Project = project;
                        instrument.Pid = (PID)((UnitOfWork)instrument.Session).GetObjectByKey(typeof(PID), pid.Oid);
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
