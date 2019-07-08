//-----------------------------------------------------------------------
// <copyright file="F:\Users\dlandry\source\repos\LPO_XAF_v2.0\LPO_XAF_v2._0.Module\Controllers\InitializePropertiesForPipeSpecViewController.cs" company="David W. Landry III">
//     Author: _**David Landry**_
//     *Copyright (c) David W. Landry III. All rights reserved.*
// </copyright>
//-----------------------------------------------------------------------
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Xpo;
using LPO_XAF_v2._0.Module.BusinessObjects.Piping;
using LPO_XAF_v2._0.Module.BusinessObjects.Project;
using System;
using System.Linq;

namespace LPO_XAF_v2._0.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class InitializePropertiesForPipeSpecViewController : ViewController
    {
        private NewObjectViewController controller;
        public InitializePropertiesForPipeSpecViewController()
        {
            InitializeComponent();
            TargetObjectType = typeof(ClientPipeSpec);
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
                ClientPipeSpec pipeSpec = e.CreatedObject as ClientPipeSpec;
                if (pipeSpec != null)
                {
                    Line line = ((NestedFrame)Frame).ViewItem.CurrentObject as Line;
                    if (line != null)
                    {
                        Client client = (Client)((UnitOfWork)pipeSpec.Session).GetObjectByKey(typeof(Client), line.Project.Client.Oid);
                        pipeSpec.Client = client;
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
