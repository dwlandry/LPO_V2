//-----------------------------------------------------------------------
// <copyright file="F:\Users\dlandry\Source\Repos\LPO_V2\LPO_XAF_v2._0.Module.Win\WinModule.cs" company="David W. Landry III">
//     Author: _**David Landry**_
//     *Copyright (c) David W. Landry III. All rights reserved.*
// </copyright>
//-----------------------------------------------------------------------
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Updating;
using DevExpress.Persistent.BaseImpl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace LPO_XAF_v2._0.Module.Win
{
    [ToolboxItemFilter("Xaf.Platform.Win")]
    // For more typical usage scenarios, be sure to check out https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.ModuleBase.
    public sealed partial class LPO_XAF_v20WindowsFormsModule : ModuleBase
    {
        //private void Application_CreateCustomModelDifferenceStore(Object sender, CreateCustomModelDifferenceStoreEventArgs e) {
        //    e.Store = new ModelDifferenceDbStore((XafApplication)sender, typeof(ModelDifference), true, "Win");
        //    e.Handled = true;
        //}
        private void Application_CreateCustomUserModelDifferenceStore(Object sender, CreateCustomModelDifferenceStoreEventArgs e)
        {
            e.Store = new ModelDifferenceDbStore((XafApplication)sender, typeof(ModelDifference), false, "Win");
            e.Handled = true;
        }
        public LPO_XAF_v20WindowsFormsModule()
        {
            InitializeComponent();
        }
        public override IEnumerable<ModuleUpdater> GetModuleUpdaters(IObjectSpace objectSpace, Version versionFromDB)
        {
            return ModuleUpdater.EmptyModuleUpdaters;
        }
        public override void Setup(XafApplication application)
        {
            base.Setup(application);
            //application.CreateCustomModelDifferenceStore += Application_CreateCustomModelDifferenceStore;
            application.CreateCustomUserModelDifferenceStore += Application_CreateCustomUserModelDifferenceStore;
            // Manage various aspects of the application UI and behavior at the module level.
        }
    }
}
