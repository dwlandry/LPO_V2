﻿//-----------------------------------------------------------------------
// <copyright file="F:\Users\dlandry\source\repos\LPO_XAF_v2.0\LPO_XAF_v2._0.Module\Controllers\InitializePropertiesForIProjectQuoteViewController.cs" company="David W. Landry III">
//     Author: _**David Landry**_
//     *Copyright (c) David W. Landry III. All rights reserved.*
// </copyright>
//-----------------------------------------------------------------------
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Xpo;
using LPO_XAF_v2._0.Module.BusinessObjects.Instrument;
using LPO_XAF_v2._0.Module.BusinessObjects.Procurement;
using LPO_XAF_v2._0.Module.BusinessObjects.Project;
using System;
using System.Linq;

namespace LPO_XAF_v2._0.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class InitializePropertiesForIProjectQuoteViewController : ViewController
    {
        private NewObjectViewController controller;
        public InitializePropertiesForIProjectQuoteViewController()
        {
            InitializeComponent();
            TargetObjectType = typeof(IProjectQuote);
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
                IProjectQuote projectQuote = e.CreatedObject as IProjectQuote;
                if (projectQuote != null)
                {
                    Instrument instrument = ((NestedFrame)Frame).ViewItem.CurrentObject as Instrument;
                    if (instrument != null)
                    {
                        Project project = (Project)((UnitOfWork)((Quote)projectQuote).Session).GetObjectByKey(typeof(Project), instrument.Project.Oid);
                        projectQuote.Project = project;
                    }
                    //else
                    //{
                    //    Project project = ((NestedFrame)Frame).ViewItem.CurrentObject as Project;
                    //    if (project != null) projectQuote.Project = project;
                    //}
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
