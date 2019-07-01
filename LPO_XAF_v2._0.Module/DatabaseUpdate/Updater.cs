//-----------------------------------------------------------------------
// <copyright file="F:\Users\dlandry\source\repos\LPO_XAF_v2.0\LPO_XAF_v2._0.Module\DatabaseUpdate\Updater.cs" company="David W. Landry III">
//     Author: _**David Landry**_
//     *Copyright (c) David W. Landry III. All rights reserved.*
// </copyright>
//-----------------------------------------------------------------------
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Updating;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using LPO_XAF_v2._0.Module.BusinessObjects.Instrument;
using System;
using System.Linq;

namespace LPO_XAF_v2._0.Module.DatabaseUpdate
{
    // For more typical usage scenarios, be sure to check out https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.Updating.ModuleUpdater
    public class Updater : ModuleUpdater
    {
        public Updater(IObjectSpace objectSpace, Version currentDBVersion) :
            base(objectSpace, currentDBVersion)
        {
        }
        public override void UpdateDatabaseAfterUpdateSchema()
        {
            base.UpdateDatabaseAfterUpdateSchema();
            //string name = "MyName";
            //DomainObject1 theObject = ObjectSpace.FindObject<DomainObject1>(CriteriaOperator.Parse("Name=?", name));
            //if(theObject == null) {
            //    theObject = ObjectSpace.CreateObject<DomainObject1>();
            //    theObject.Name = name;
            //}
            CreateDefaultRole();
            SupplyInitialDataForInstrumentMeasurementCategories();
            SupplyInitialDataForInstrumentDeviceCategories();
        }
        public override void UpdateDatabaseBeforeUpdateSchema()
        {
            base.UpdateDatabaseBeforeUpdateSchema();
            //if(CurrentDBVersion < new Version("1.1.0.0") && CurrentDBVersion > new Version("0.0.0.0")) {
            //    RenameColumn("DomainObject1Table", "OldColumnName", "NewColumnName");
            //}
        }
        private PermissionPolicyRole CreateDefaultRole()
        {
            PermissionPolicyRole defaultRole = ObjectSpace.FindObject<PermissionPolicyRole>(new BinaryOperator("Name", "Default"));
            if (defaultRole == null)
            {
                defaultRole = ObjectSpace.CreateObject<PermissionPolicyRole>();
                defaultRole.Name = "Default";

                defaultRole.AddObjectPermission<PermissionPolicyUser>(SecurityOperations.Read, "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
                defaultRole.AddNavigationPermission(@"Application/NavigationItems/Items/Default/Items/MyDetails", SecurityPermissionState.Allow);
                defaultRole.AddMemberPermission<PermissionPolicyUser>(SecurityOperations.Write, "ChangePasswordOnFirstLogon", "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
                defaultRole.AddMemberPermission<PermissionPolicyUser>(SecurityOperations.Write, "StoredPassword", "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
                defaultRole.AddTypePermissionsRecursively<PermissionPolicyRole>(SecurityOperations.Read, SecurityPermissionState.Deny);
                defaultRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                defaultRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                defaultRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.Create, SecurityPermissionState.Allow);
                defaultRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.Create, SecurityPermissionState.Allow);
            }
            return defaultRole;
        }

        private void SupplyInitialDataForInstrumentMeasurementCategories()
        {
            if (!InstrumentMeasurementCategoryExists("Analysis")) ObjectSpace.CreateObject<InstrumentMeasurementCategory>().Name = "Analysis";
            if (!InstrumentMeasurementCategoryExists("Flow")) ObjectSpace.CreateObject<InstrumentMeasurementCategory>().Name = "Flow";
            if (!InstrumentMeasurementCategoryExists("Level")) ObjectSpace.CreateObject<InstrumentMeasurementCategory>().Name = "Level";
            if (!InstrumentMeasurementCategoryExists("Temperature")) ObjectSpace.CreateObject<InstrumentMeasurementCategory>().Name = "Temperature";
            if (!InstrumentMeasurementCategoryExists("Pressure")) ObjectSpace.CreateObject<InstrumentMeasurementCategory>().Name = "Pressure";
            if (!InstrumentMeasurementCategoryExists("Misc")) ObjectSpace.CreateObject<InstrumentMeasurementCategory>().Name = "Misc";
            if (!InstrumentMeasurementCategoryExists("Hand")) ObjectSpace.CreateObject<InstrumentMeasurementCategory>().Name = "Hand";
            //if (!InstrumentMeasurementCategoryExists("Valve")) ObjectSpace.CreateObject<InstrumentMeasurementCategory>().Name = "Valve";
            ObjectSpace.CommitChanges();
        }
        private bool InstrumentMeasurementCategoryExists(string categoryName)
        {
            InstrumentMeasurementCategory category = ObjectSpace.FindObject<InstrumentMeasurementCategory>(
                CriteriaOperator.Parse("Name=?", categoryName));
            return category != null;
        }

        private void SupplyInitialDataForInstrumentDeviceCategories()
        {
            if (!InstrumentDeviceCategoryExists("Switch")) ObjectSpace.CreateObject<InstrumentDeviceCategory>().Name = "Switch";
            if (!InstrumentDeviceCategoryExists("Gauge")) ObjectSpace.CreateObject<InstrumentDeviceCategory>().Name = "Gauge";
            if (!InstrumentDeviceCategoryExists("Element")) ObjectSpace.CreateObject<InstrumentDeviceCategory>().Name = "Element";
            if (!InstrumentDeviceCategoryExists("Transmitter")) ObjectSpace.CreateObject<InstrumentDeviceCategory>().Name = "Transmitter";
            if (!InstrumentDeviceCategoryExists("Valve")) ObjectSpace.CreateObject<InstrumentDeviceCategory>().Name = "Valve";
            if (!InstrumentDeviceCategoryExists("Controller")) ObjectSpace.CreateObject<InstrumentDeviceCategory>().Name = "Controller";
            if (!InstrumentDeviceCategoryExists("Indicator")) ObjectSpace.CreateObject<InstrumentDeviceCategory>().Name = "Indicator";
            if (!InstrumentDeviceCategoryExists("Transducer")) ObjectSpace.CreateObject<InstrumentDeviceCategory>().Name = "Transducer";
            if (!InstrumentDeviceCategoryExists("Signal Conditioner")) ObjectSpace.CreateObject<InstrumentDeviceCategory>().Name = "Signal Conditioner";
        }
        private bool InstrumentDeviceCategoryExists(string categoryName)
        {
            InstrumentDeviceCategory category = ObjectSpace.FindObject<InstrumentDeviceCategory>(
                CriteriaOperator.Parse("Name=?", categoryName));
            return category != null;
        }
    }
}
