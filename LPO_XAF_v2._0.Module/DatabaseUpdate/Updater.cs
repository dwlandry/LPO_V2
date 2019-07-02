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
using LPO_XAF_v2._0.Module.BusinessObjects.Piping;
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
            SupplyNominalPipeSizeData();
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
            ObjectSpace.CommitChanges();
        }
        private bool InstrumentDeviceCategoryExists(string categoryName)
        {
            InstrumentDeviceCategory category = ObjectSpace.FindObject<InstrumentDeviceCategory>(
                CriteriaOperator.Parse("Name=?", categoryName));
            return category != null;
        }

        private void SupplyNominalPipeSizeData()
        {
            if (!NominalPipeSizeExists(0.125)) AddNominalPipeSize(0.125, 0.405);
            if (!NominalPipeSizeExists(0.25)) AddNominalPipeSize(0.25, 0.54);
            if (!NominalPipeSizeExists(0.375)) AddNominalPipeSize(0.375, 0.675);
            if (!NominalPipeSizeExists(0.5)) AddNominalPipeSize(0.5, 0.84);
            if (!NominalPipeSizeExists(0.75)) AddNominalPipeSize(0.75, 1.05);
            if (!NominalPipeSizeExists(1)) AddNominalPipeSize(1, 1.315);
            if (!NominalPipeSizeExists(1.25)) AddNominalPipeSize(1.25, 1.66);
            if (!NominalPipeSizeExists(1.5)) AddNominalPipeSize(1.5, 1.9);
            if (!NominalPipeSizeExists(2)) AddNominalPipeSize(2, 2.375);
            if (!NominalPipeSizeExists(2.5)) AddNominalPipeSize(2.5, 2.875);
            if (!NominalPipeSizeExists(3)) AddNominalPipeSize(3, 3.5);
            if (!NominalPipeSizeExists(3.5)) AddNominalPipeSize(3.5, 4);
            if (!NominalPipeSizeExists(4)) AddNominalPipeSize(4, 4.5);
            if (!NominalPipeSizeExists(4.5)) AddNominalPipeSize(4.5, 5);
            if (!NominalPipeSizeExists(5)) AddNominalPipeSize(5, 5.563);
            if (!NominalPipeSizeExists(6)) AddNominalPipeSize(6, 6.625);
            if (!NominalPipeSizeExists(7)) AddNominalPipeSize(7, 7.625);
            if (!NominalPipeSizeExists(8)) AddNominalPipeSize(8, 8.625);
            if (!NominalPipeSizeExists(9)) AddNominalPipeSize(9, 9.625);
            if (!NominalPipeSizeExists(10)) AddNominalPipeSize(10, 10.75);
            if (!NominalPipeSizeExists(11)) AddNominalPipeSize(11, 11.75);
            if (!NominalPipeSizeExists(12)) AddNominalPipeSize(12, 12.75);
            if (!NominalPipeSizeExists(14)) AddNominalPipeSize(14, 14);
            if (!NominalPipeSizeExists(16)) AddNominalPipeSize(16, 16);
            if (!NominalPipeSizeExists(18)) AddNominalPipeSize(18, 18);
            if (!NominalPipeSizeExists(20)) AddNominalPipeSize(20, 20);
            if (!NominalPipeSizeExists(22)) AddNominalPipeSize(22, 22);
            if (!NominalPipeSizeExists(24)) AddNominalPipeSize(24, 24);
            if (!NominalPipeSizeExists(26)) AddNominalPipeSize(26, 26);
            if (!NominalPipeSizeExists(28)) AddNominalPipeSize(28, 28);
            if (!NominalPipeSizeExists(30)) AddNominalPipeSize(30, 30);
            if (!NominalPipeSizeExists(32)) AddNominalPipeSize(32, 32);
            if (!NominalPipeSizeExists(34)) AddNominalPipeSize(34, 34);
            if (!NominalPipeSizeExists(36)) AddNominalPipeSize(36, 36);
            if (!NominalPipeSizeExists(42)) AddNominalPipeSize(42, 42);
            if (!NominalPipeSizeExists(48)) AddNominalPipeSize(48, 48);
            ObjectSpace.CommitChanges();

        }
        private bool NominalPipeSizeExists(double nominalPipeSize)
        {
            NominalPipeSize size = ObjectSpace.FindObject<NominalPipeSize>(
                CriteriaOperator.Parse("NominalSizeInInches=?", nominalPipeSize));
            return size != null;
        }
        private void AddNominalPipeSize(double size, double od)
        {
            NominalPipeSize pipe = ObjectSpace.CreateObject<NominalPipeSize>();
            pipe.NominalSizeInInches = size;
            pipe.OuterDiameterInInches = od;
        }
    }
}
