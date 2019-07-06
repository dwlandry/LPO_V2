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
            CreatePipingStandards();
            CreatePipingSchedules();
            CreatePipeWallThickness();
            CreateMetallurgyGeneralTypes();
            CreateMetallurgyMaterials();
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
            if (!NominalPipeSizeExists(14)) AddNominalPipeSize(14);
            if (!NominalPipeSizeExists(16)) AddNominalPipeSize(16);
            if (!NominalPipeSizeExists(18)) AddNominalPipeSize(18);
            if (!NominalPipeSizeExists(20)) AddNominalPipeSize(20);
            if (!NominalPipeSizeExists(22)) AddNominalPipeSize(22);
            if (!NominalPipeSizeExists(24)) AddNominalPipeSize(24);
            if (!NominalPipeSizeExists(26)) AddNominalPipeSize(26);
            if (!NominalPipeSizeExists(28)) AddNominalPipeSize(28);
            if (!NominalPipeSizeExists(30)) AddNominalPipeSize(30);
            if (!NominalPipeSizeExists(32)) AddNominalPipeSize(32);
            if (!NominalPipeSizeExists(34)) AddNominalPipeSize(34);
            if (!NominalPipeSizeExists(36)) AddNominalPipeSize(36);
            if (!NominalPipeSizeExists(38)) AddNominalPipeSize(38);
            if (!NominalPipeSizeExists(40)) AddNominalPipeSize(40);
            if (!NominalPipeSizeExists(42)) AddNominalPipeSize(42);
            if (!NominalPipeSizeExists(44)) AddNominalPipeSize(44);
            if (!NominalPipeSizeExists(46)) AddNominalPipeSize(46);
            if (!NominalPipeSizeExists(48)) AddNominalPipeSize(48);
            if (!NominalPipeSizeExists(54)) AddNominalPipeSize(54);
            if (!NominalPipeSizeExists(60)) AddNominalPipeSize(60);
            if (!NominalPipeSizeExists(66)) AddNominalPipeSize(66);
            if (!NominalPipeSizeExists(72)) AddNominalPipeSize(72);
            ObjectSpace.CommitChanges();

        }
        private bool NominalPipeSizeExists(double nominalPipeSize)
        {
            NominalPipeSize size = ObjectSpace.FindObject<NominalPipeSize>(
                CriteriaOperator.Parse("NominalSizeInInches=?", nominalPipeSize));
            return size != null;
        }
        private void AddNominalPipeSize(double nominalSizeInInches, double outerDiameter)
        {
            NominalPipeSize pipe = ObjectSpace.CreateObject<NominalPipeSize>();
            pipe.NominalSizeInInches = nominalSizeInInches;
            pipe.OuterDiameterInInches = outerDiameter;
        }
        private void AddNominalPipeSize(double nominalSizeInInches)
        {
            NominalPipeSize pipe = ObjectSpace.CreateObject<NominalPipeSize>();
            pipe.NominalSizeInInches = nominalSizeInInches;
            pipe.OuterDiameterInInches = nominalSizeInInches;
        }

        private void CreatePipingStandards()
        {
            if (!PipingStandardExists("ASME/ANSI N36.10")) ObjectSpace.CreateObject<PipingStandard>().Name = "ASME/ANSI N36.10";
            if (!PipingStandardExists("ASME/ANSI B36.10")) ObjectSpace.CreateObject<PipingStandard>().Name = "ASME/ANSI B36.10";
            if (!PipingStandardExists("ASME/ANSI B36.19")) ObjectSpace.CreateObject<PipingStandard>().Name = "ASME/ANSI B36.19";
            ObjectSpace.CommitChanges();
        }
        private bool PipingStandardExists(string standardName)
        {
            PipingStandard standard = ObjectSpace.FindObject<PipingStandard>(
                CriteriaOperator.Parse("Name=?", standardName));
            return standard != null;
        }

        private void CreatePipingSchedules()
        {
            if (!PipingScheduleExists("Std")) AddPipingSchedule("Std", "ASME/ANSI N36.10");
            if (!PipingScheduleExists("XS")) AddPipingSchedule("XS", "ASME/ANSI N36.10");
            if (!PipingScheduleExists("XXS")) AddPipingSchedule("XXS", "ASME/ANSI N36.10");
            if (!PipingScheduleExists("5")) AddPipingSchedule("5", "ASME/ANSI B36.10");
            if (!PipingScheduleExists("10")) AddPipingSchedule("10", "ASME/ANSI B36.10");
            if (!PipingScheduleExists("20")) AddPipingSchedule("20", "ASME/ANSI B36.10");
            if (!PipingScheduleExists("30")) AddPipingSchedule("30", "ASME/ANSI B36.10");
            if (!PipingScheduleExists("40")) AddPipingSchedule("40", "ASME/ANSI B36.10");
            if (!PipingScheduleExists("60")) AddPipingSchedule("60", "ASME/ANSI B36.10");
            if (!PipingScheduleExists("80")) AddPipingSchedule("80", "ASME/ANSI B36.10");
            if (!PipingScheduleExists("100")) AddPipingSchedule("100", "ASME/ANSI B36.10");
            if (!PipingScheduleExists("120")) AddPipingSchedule("120", "ASME/ANSI B36.10");
            if (!PipingScheduleExists("140")) AddPipingSchedule("140", "ASME/ANSI B36.10");
            if (!PipingScheduleExists("160")) AddPipingSchedule("160", "ASME/ANSI B36.10");
            if (!PipingScheduleExists("5S")) AddPipingSchedule("5S", "ASME/ANSI B36.19");
            if (!PipingScheduleExists("10S")) AddPipingSchedule("10S", "ASME/ANSI B36.19");
            if (!PipingScheduleExists("40S")) AddPipingSchedule("40S", "ASME/ANSI B36.19");
            if (!PipingScheduleExists("80S")) AddPipingSchedule("80S", "ASME/ANSI B36.19");
            ObjectSpace.CommitChanges();
        }
        private bool PipingScheduleExists(string scheduleName)
        {
            PipingSchedule value = ObjectSpace.FindObject<PipingSchedule>(
                CriteriaOperator.Parse("Name=?", scheduleName));
            return value != null;
        }
        private void AddPipingSchedule(string name, string standardName)
        {
            PipingSchedule schedule = ObjectSpace.CreateObject<PipingSchedule>();
            schedule.Name = name;

            PipingStandard standard = ObjectSpace.FindObject<PipingStandard>(
                CriteriaOperator.Parse("Name=?", standardName));

            schedule.PipingStandard = standard;
        }

        private void CreatePipeWallThickness()
        {
            if (!PipeWallThicknessExists(0.125, "5")) AddPipeWallThickness(0.125, "5", 0.035);
            if (!PipeWallThicknessExists(0.125, "10")) AddPipeWallThickness(0.125, "10", 0.049);
            if (!PipeWallThicknessExists(0.125, "10S")) AddPipeWallThickness(0.125, "10S", 0.049);
            if (!PipeWallThicknessExists(0.125, "40")) AddPipeWallThickness(0.125, "40", 0.068);
            if (!PipeWallThicknessExists(0.125, "Std")) AddPipeWallThickness(0.125, "Std", 0.068);
            if (!PipeWallThicknessExists(0.125, "40S")) AddPipeWallThickness(0.125, "40S", 0.068);
            if (!PipeWallThicknessExists(0.125, "80")) AddPipeWallThickness(0.125, "80", 0.095);
            if (!PipeWallThicknessExists(0.125, "XS")) AddPipeWallThickness(0.125, "XS", 0.095);
            if (!PipeWallThicknessExists(0.125, "80S")) AddPipeWallThickness(0.125, "80S", 0.095);
            if (!PipeWallThicknessExists(0.25, "5")) AddPipeWallThickness(0.25, "5", 0.049);
            if (!PipeWallThicknessExists(0.25, "10")) AddPipeWallThickness(0.25, "10", 0.065);
            if (!PipeWallThicknessExists(0.25, "10S")) AddPipeWallThickness(0.25, "10S", 0.065);
            if (!PipeWallThicknessExists(0.25, "40")) AddPipeWallThickness(0.25, "40", 0.088);
            if (!PipeWallThicknessExists(0.25, "Std")) AddPipeWallThickness(0.25, "Std", 0.088);
            if (!PipeWallThicknessExists(0.25, "40S")) AddPipeWallThickness(0.25, "40S", 0.088);
            if (!PipeWallThicknessExists(0.25, "80")) AddPipeWallThickness(0.25, "80", 0.119);
            if (!PipeWallThicknessExists(0.25, "XS")) AddPipeWallThickness(0.25, "XS", 0.119);
            if (!PipeWallThicknessExists(0.25, "80S")) AddPipeWallThickness(0.25, "80S", 0.119);
            if (!PipeWallThicknessExists(0.375, "5")) AddPipeWallThickness(0.375, "5", 0.049);
            if (!PipeWallThicknessExists(0.375, "10")) AddPipeWallThickness(0.375, "10", 0.065);
            if (!PipeWallThicknessExists(0.375, "10S")) AddPipeWallThickness(0.375, "10S", 0.065);
            if (!PipeWallThicknessExists(0.375, "40")) AddPipeWallThickness(0.375, "40", 0.091);
            if (!PipeWallThicknessExists(0.375, "Std")) AddPipeWallThickness(0.375, "Std", 0.091);
            if (!PipeWallThicknessExists(0.375, "40S")) AddPipeWallThickness(0.375, "40S", 0.091);
            if (!PipeWallThicknessExists(0.375, "80")) AddPipeWallThickness(0.375, "80", 0.126);
            if (!PipeWallThicknessExists(0.375, "XS")) AddPipeWallThickness(0.375, "XS", 0.126);
            if (!PipeWallThicknessExists(0.375, "80S")) AddPipeWallThickness(0.375, "80S", 0.126);
            if (!PipeWallThicknessExists(0.5, "5")) AddPipeWallThickness(0.5, "5", 0.065);
            if (!PipeWallThicknessExists(0.5, "5S")) AddPipeWallThickness(0.5, "5S", 0.065);
            if (!PipeWallThicknessExists(0.5, "10")) AddPipeWallThickness(0.5, "10", 0.083);
            if (!PipeWallThicknessExists(0.5, "10S")) AddPipeWallThickness(0.5, "10S", 0.083);
            if (!PipeWallThicknessExists(0.5, "40")) AddPipeWallThickness(0.5, "40", 0.109);
            if (!PipeWallThicknessExists(0.5, "Std")) AddPipeWallThickness(0.5, "Std", 0.109);
            if (!PipeWallThicknessExists(0.5, "40S")) AddPipeWallThickness(0.5, "40S", 0.109);
            if (!PipeWallThicknessExists(0.5, "80")) AddPipeWallThickness(0.5, "80", 0.147);
            if (!PipeWallThicknessExists(0.5, "XS")) AddPipeWallThickness(0.5, "XS", 0.147);
            if (!PipeWallThicknessExists(0.5, "80S")) AddPipeWallThickness(0.5, "80S", 0.147);
            if (!PipeWallThicknessExists(0.5, "160")) AddPipeWallThickness(0.5, "160", 0.187);
            if (!PipeWallThicknessExists(0.5, "XXS")) AddPipeWallThickness(0.5, "XXS", 0.294);
            if (!PipeWallThicknessExists(0.75, "5")) AddPipeWallThickness(0.75, "5", 0.065);
            if (!PipeWallThicknessExists(0.75, "5S")) AddPipeWallThickness(0.75, "5S", 0.065);
            if (!PipeWallThicknessExists(0.75, "10")) AddPipeWallThickness(0.75, "10", 0.083);
            if (!PipeWallThicknessExists(0.75, "10S")) AddPipeWallThickness(0.75, "10S", 0.083);
            if (!PipeWallThicknessExists(0.75, "40")) AddPipeWallThickness(0.75, "40", 0.113);
            if (!PipeWallThicknessExists(0.75, "Std")) AddPipeWallThickness(0.75, "Std", 0.113);
            if (!PipeWallThicknessExists(0.75, "40S")) AddPipeWallThickness(0.75, "40S", 0.113);
            if (!PipeWallThicknessExists(0.75, "80")) AddPipeWallThickness(0.75, "80", 0.154);
            if (!PipeWallThicknessExists(0.75, "XS")) AddPipeWallThickness(0.75, "XS", 0.154);
            if (!PipeWallThicknessExists(0.75, "80S")) AddPipeWallThickness(0.75, "80S", 0.154);
            if (!PipeWallThicknessExists(0.75, "160")) AddPipeWallThickness(0.75, "160", 0.218);
            if (!PipeWallThicknessExists(0.75, "XXS")) AddPipeWallThickness(0.75, "XXS", 0.308);
            if (!PipeWallThicknessExists(1, "5")) AddPipeWallThickness(1, "5", 0.065);
            if (!PipeWallThicknessExists(1, "5S")) AddPipeWallThickness(1, "5S", 0.065);
            if (!PipeWallThicknessExists(1, "10")) AddPipeWallThickness(1, "10", 0.109);
            if (!PipeWallThicknessExists(1, "10S")) AddPipeWallThickness(1, "10S", 0.109);
            if (!PipeWallThicknessExists(1, "40")) AddPipeWallThickness(1, "40", 0.133);
            if (!PipeWallThicknessExists(1, "Std")) AddPipeWallThickness(1, "Std", 0.133);
            if (!PipeWallThicknessExists(1, "40S")) AddPipeWallThickness(1, "40S", 0.133);
            if (!PipeWallThicknessExists(1, "80")) AddPipeWallThickness(1, "80", 0.179);
            if (!PipeWallThicknessExists(1, "XS")) AddPipeWallThickness(1, "XS", 0.179);
            if (!PipeWallThicknessExists(1, "80S")) AddPipeWallThickness(1, "80S", 0.179);
            if (!PipeWallThicknessExists(1, "160")) AddPipeWallThickness(1, "160", 0.25);
            if (!PipeWallThicknessExists(1, "XXS")) AddPipeWallThickness(1, "XXS", 0.358);
            if (!PipeWallThicknessExists(1.5, "5")) AddPipeWallThickness(1.5, "5", 0.065);
            if (!PipeWallThicknessExists(1.5, "5S")) AddPipeWallThickness(1.5, "5S", 0.065);
            if (!PipeWallThicknessExists(1.5, "10")) AddPipeWallThickness(1.5, "10", 0.109);
            if (!PipeWallThicknessExists(1.5, "10S")) AddPipeWallThickness(1.5, "10S", 0.109);
            if (!PipeWallThicknessExists(1.5, "40")) AddPipeWallThickness(1.5, "40", 0.145);
            if (!PipeWallThicknessExists(1.5, "Std")) AddPipeWallThickness(1.5, "Std", 0.145);
            if (!PipeWallThicknessExists(1.5, "40S")) AddPipeWallThickness(1.5, "40S", 0.145);
            if (!PipeWallThicknessExists(1.5, "80")) AddPipeWallThickness(1.5, "80", 0.2);
            if (!PipeWallThicknessExists(1.5, "XS")) AddPipeWallThickness(1.5, "XS", 0.2);
            if (!PipeWallThicknessExists(1.5, "80S")) AddPipeWallThickness(1.5, "80S", 0.2);
            if (!PipeWallThicknessExists(1.5, "160")) AddPipeWallThickness(1.5, "160", 0.281);
            if (!PipeWallThicknessExists(1.5, "XXS")) AddPipeWallThickness(1.5, "XXS", 0.4);
            if (!PipeWallThicknessExists(2, "5")) AddPipeWallThickness(2, "5", 0.065);
            if (!PipeWallThicknessExists(2, "5S")) AddPipeWallThickness(2, "5S", 0.065);
            if (!PipeWallThicknessExists(2, "10")) AddPipeWallThickness(2, "10", 0.109);
            if (!PipeWallThicknessExists(2, "10S")) AddPipeWallThickness(2, "10S", 0.109);
            if (!PipeWallThicknessExists(2, "40")) AddPipeWallThickness(2, "40", 0.154);
            if (!PipeWallThicknessExists(2, "Std")) AddPipeWallThickness(2, "Std", 0.154);
            if (!PipeWallThicknessExists(2, "40S")) AddPipeWallThickness(2, "40S", 0.154);
            if (!PipeWallThicknessExists(2, "80")) AddPipeWallThickness(2, "80", 0.218);
            if (!PipeWallThicknessExists(2, "XS")) AddPipeWallThickness(2, "XS", 0.218);
            if (!PipeWallThicknessExists(2, "80S")) AddPipeWallThickness(2, "80S", 0.218);
            if (!PipeWallThicknessExists(2, "160")) AddPipeWallThickness(2, "160", 0.343);
            if (!PipeWallThicknessExists(2, "XXS")) AddPipeWallThickness(2, "XXS", 0.436);
            if (!PipeWallThicknessExists(2.5, "5")) AddPipeWallThickness(2.5, "5", 0.083);
            if (!PipeWallThicknessExists(2.5, "5S")) AddPipeWallThickness(2.5, "5S", 0.083);
            if (!PipeWallThicknessExists(2.5, "10")) AddPipeWallThickness(2.5, "10", 0.12);
            if (!PipeWallThicknessExists(2.5, "10S")) AddPipeWallThickness(2.5, "10S", 0.12);
            if (!PipeWallThicknessExists(2.5, "40")) AddPipeWallThickness(2.5, "40", 0.203);
            if (!PipeWallThicknessExists(2.5, "Std")) AddPipeWallThickness(2.5, "Std", 0.203);
            if (!PipeWallThicknessExists(2.5, "40S")) AddPipeWallThickness(2.5, "40S", 0.203);
            if (!PipeWallThicknessExists(2.5, "80")) AddPipeWallThickness(2.5, "80", 0.276);
            if (!PipeWallThicknessExists(2.5, "XS")) AddPipeWallThickness(2.5, "XS", 0.276);
            if (!PipeWallThicknessExists(2.5, "80S")) AddPipeWallThickness(2.5, "80S", 0.276);
            if (!PipeWallThicknessExists(2.5, "160")) AddPipeWallThickness(2.5, "160", 0.375);
            if (!PipeWallThicknessExists(2.5, "XXS")) AddPipeWallThickness(2.5, "XXS", 0.552);
            if (!PipeWallThicknessExists(3, "5")) AddPipeWallThickness(3, "5", 0.083);
            if (!PipeWallThicknessExists(3, "5S")) AddPipeWallThickness(3, "5S", 0.083);
            if (!PipeWallThicknessExists(3, "10")) AddPipeWallThickness(3, "10", 0.12);
            if (!PipeWallThicknessExists(3, "10S")) AddPipeWallThickness(3, "10S", 0.12);
            if (!PipeWallThicknessExists(3, "40")) AddPipeWallThickness(3, "40", 0.216);
            if (!PipeWallThicknessExists(3, "Std")) AddPipeWallThickness(3, "Std", 0.216);
            if (!PipeWallThicknessExists(3, "40S")) AddPipeWallThickness(3, "40S", 0.216);
            if (!PipeWallThicknessExists(3, "80")) AddPipeWallThickness(3, "80", 0.3);
            if (!PipeWallThicknessExists(3, "XS")) AddPipeWallThickness(3, "XS", 0.3);
            if (!PipeWallThicknessExists(3, "80S")) AddPipeWallThickness(3, "80S", 0.3);
            if (!PipeWallThicknessExists(3, "160")) AddPipeWallThickness(3, "160", 0.437);
            if (!PipeWallThicknessExists(3, "XXS")) AddPipeWallThickness(3, "XXS", 0.6);
            if (!PipeWallThicknessExists(4, "5")) AddPipeWallThickness(4, "5", 0.083);
            if (!PipeWallThicknessExists(4, "5S")) AddPipeWallThickness(4, "5S", 0.083);
            if (!PipeWallThicknessExists(4, "10")) AddPipeWallThickness(4, "10", 0.12);
            if (!PipeWallThicknessExists(4, "10S")) AddPipeWallThickness(4, "10S", 0.12);
            if (!PipeWallThicknessExists(4, "40")) AddPipeWallThickness(4, "40", 0.237);
            if (!PipeWallThicknessExists(4, "Std")) AddPipeWallThickness(4, "Std", 0.237);
            if (!PipeWallThicknessExists(4, "40S")) AddPipeWallThickness(4, "40S", 0.237);
            if (!PipeWallThicknessExists(4, "80")) AddPipeWallThickness(4, "80", 0.337);
            if (!PipeWallThicknessExists(4, "XS")) AddPipeWallThickness(4, "XS", 0.337);
            if (!PipeWallThicknessExists(4, "80S")) AddPipeWallThickness(4, "80S", 0.337);
            if (!PipeWallThicknessExists(4, "120")) AddPipeWallThickness(4, "120", 0.437);
            if (!PipeWallThicknessExists(4, "160")) AddPipeWallThickness(4, "160", 0.531);
            if (!PipeWallThicknessExists(4, "XXS")) AddPipeWallThickness(4, "XXS", 0.674);
            if (!PipeWallThicknessExists(4.5, "Std")) AddPipeWallThickness(4.5, "Std", 0.247);
            if (!PipeWallThicknessExists(4.5, "40S")) AddPipeWallThickness(4.5, "40S", 0.247);
            if (!PipeWallThicknessExists(4.5, "XS")) AddPipeWallThickness(4.5, "XS", 0.355);
            if (!PipeWallThicknessExists(4.5, "80S")) AddPipeWallThickness(4.5, "80S", 0.355);
            if (!PipeWallThicknessExists(4.5, "XXS")) AddPipeWallThickness(4.5, "XXS", 0.71);
            if (!PipeWallThicknessExists(5, "5")) AddPipeWallThickness(5, "5", 0.109);
            if (!PipeWallThicknessExists(5, "5S")) AddPipeWallThickness(5, "5S", 0.109);
            if (!PipeWallThicknessExists(5, "10")) AddPipeWallThickness(5, "10", 0.134);
            if (!PipeWallThicknessExists(5, "10S")) AddPipeWallThickness(5, "10S", 0.134);
            if (!PipeWallThicknessExists(5, "40")) AddPipeWallThickness(5, "40", 0.258);
            if (!PipeWallThicknessExists(5, "Std")) AddPipeWallThickness(5, "Std", 0.258);
            if (!PipeWallThicknessExists(5, "40S")) AddPipeWallThickness(5, "40S", 0.258);
            if (!PipeWallThicknessExists(5, "80")) AddPipeWallThickness(5, "80", 0.375);
            if (!PipeWallThicknessExists(5, "XS")) AddPipeWallThickness(5, "XS", 0.375);
            if (!PipeWallThicknessExists(5, "80S")) AddPipeWallThickness(5, "80S", 0.375);
            if (!PipeWallThicknessExists(5, "120")) AddPipeWallThickness(5, "120", 0.5);
            if (!PipeWallThicknessExists(5, "160")) AddPipeWallThickness(5, "160", 0.625);
            if (!PipeWallThicknessExists(5, "XXS")) AddPipeWallThickness(5, "XXS", 0.75);
            if (!PipeWallThicknessExists(6, "5")) AddPipeWallThickness(6, "5", 0.109);
            if (!PipeWallThicknessExists(6, "5S")) AddPipeWallThickness(6, "5S", 0.109);
            if (!PipeWallThicknessExists(6, "10")) AddPipeWallThickness(6, "10", 0.134);
            if (!PipeWallThicknessExists(6, "10S")) AddPipeWallThickness(6, "10S", 0.134);
            if (!PipeWallThicknessExists(6, "40")) AddPipeWallThickness(6, "40", 0.28);
            if (!PipeWallThicknessExists(6, "Std")) AddPipeWallThickness(6, "Std", 0.28);
            if (!PipeWallThicknessExists(6, "40S")) AddPipeWallThickness(6, "40S", 0.28);
            if (!PipeWallThicknessExists(6, "80")) AddPipeWallThickness(6, "80", 0.432);
            if (!PipeWallThicknessExists(6, "XS")) AddPipeWallThickness(6, "XS", 0.432);
            if (!PipeWallThicknessExists(6, "80S")) AddPipeWallThickness(6, "80S", 0.432);
            if (!PipeWallThicknessExists(6, "120")) AddPipeWallThickness(6, "120", 0.562);
            if (!PipeWallThicknessExists(6, "160")) AddPipeWallThickness(6, "160", 0.718);
            if (!PipeWallThicknessExists(6, "XXS")) AddPipeWallThickness(6, "XXS", 0.864);
            if (!PipeWallThicknessExists(7, "Std")) AddPipeWallThickness(7, "Std", 0.301);
            if (!PipeWallThicknessExists(7, "40S")) AddPipeWallThickness(7, "40S", 0.301);
            if (!PipeWallThicknessExists(7, "XS")) AddPipeWallThickness(7, "XS", 0.5);
            if (!PipeWallThicknessExists(7, "80S")) AddPipeWallThickness(7, "80S", 0.5);
            if (!PipeWallThicknessExists(7, "XXS")) AddPipeWallThickness(7, "XXS", 0.875);
            if (!PipeWallThicknessExists(8, "5")) AddPipeWallThickness(8, "5", 0.109);
            if (!PipeWallThicknessExists(8, "5S")) AddPipeWallThickness(8, "5S", 0.109);
            if (!PipeWallThicknessExists(8, "10")) AddPipeWallThickness(8, "10", 0.148);
            if (!PipeWallThicknessExists(8, "10S")) AddPipeWallThickness(8, "10S", 0.148);
            if (!PipeWallThicknessExists(8, "20")) AddPipeWallThickness(8, "20", 0.25);
            if (!PipeWallThicknessExists(8, "30")) AddPipeWallThickness(8, "30", 0.277);
            if (!PipeWallThicknessExists(8, "40")) AddPipeWallThickness(8, "40", 0.322);
            if (!PipeWallThicknessExists(8, "Std")) AddPipeWallThickness(8, "Std", 0.322);
            if (!PipeWallThicknessExists(8, "40S")) AddPipeWallThickness(8, "40S", 0.322);
            if (!PipeWallThicknessExists(8, "60")) AddPipeWallThickness(8, "60", 0.406);
            if (!PipeWallThicknessExists(8, "80")) AddPipeWallThickness(8, "80", 0.5);
            if (!PipeWallThicknessExists(8, "XS")) AddPipeWallThickness(8, "XS", 0.5);
            if (!PipeWallThicknessExists(8, "80S")) AddPipeWallThickness(8, "80S", 0.5);
            if (!PipeWallThicknessExists(8, "100")) AddPipeWallThickness(8, "100", 0.593);
            if (!PipeWallThicknessExists(8, "120")) AddPipeWallThickness(8, "120", 0.718);
            if (!PipeWallThicknessExists(8, "140")) AddPipeWallThickness(8, "140", 0.812);
            if (!PipeWallThicknessExists(8, "XXS")) AddPipeWallThickness(8, "XXS", 0.875);
            if (!PipeWallThicknessExists(8, "160")) AddPipeWallThickness(8, "160", 0.906);
            if (!PipeWallThicknessExists(9, "Std")) AddPipeWallThickness(9, "Std", 0.342);
            if (!PipeWallThicknessExists(9, "40S")) AddPipeWallThickness(9, "40S", 0.342);
            if (!PipeWallThicknessExists(9, "XS")) AddPipeWallThickness(9, "XS", 0.5);
            if (!PipeWallThicknessExists(9, "80S")) AddPipeWallThickness(9, "80S", 0.5);
            if (!PipeWallThicknessExists(10, "5")) AddPipeWallThickness(10, "5", 0.134);
            if (!PipeWallThicknessExists(10, "5S")) AddPipeWallThickness(10, "5S", 0.134);
            if (!PipeWallThicknessExists(10, "10")) AddPipeWallThickness(10, "10", 0.165);
            if (!PipeWallThicknessExists(10, "10S")) AddPipeWallThickness(10, "10S", 0.165);
            if (!PipeWallThicknessExists(10, "20")) AddPipeWallThickness(10, "20", 0.25);
            if (!PipeWallThicknessExists(10, "30")) AddPipeWallThickness(10, "30", 0.307);
            if (!PipeWallThicknessExists(10, "40")) AddPipeWallThickness(10, "40", 0.365);
            if (!PipeWallThicknessExists(10, "Std")) AddPipeWallThickness(10, "Std", 0.365);
            if (!PipeWallThicknessExists(10, "40S")) AddPipeWallThickness(10, "40S", 0.365);
            if (!PipeWallThicknessExists(10, "60")) AddPipeWallThickness(10, "60", 0.5);
            if (!PipeWallThicknessExists(10, "XS")) AddPipeWallThickness(10, "XS", 0.5);
            if (!PipeWallThicknessExists(10, "80S")) AddPipeWallThickness(10, "80S", 0.5);
            if (!PipeWallThicknessExists(10, "80")) AddPipeWallThickness(10, "80", 0.593);
            if (!PipeWallThicknessExists(10, "100")) AddPipeWallThickness(10, "100", 0.718);
            if (!PipeWallThicknessExists(10, "120")) AddPipeWallThickness(10, "120", 0.843);
            if (!PipeWallThicknessExists(10, "140")) AddPipeWallThickness(10, "140", 1);
            if (!PipeWallThicknessExists(10, "160")) AddPipeWallThickness(10, "160", 1.125);
            if (!PipeWallThicknessExists(12, "5")) AddPipeWallThickness(12, "5", 0.156);
            if (!PipeWallThicknessExists(12, "5S")) AddPipeWallThickness(12, "5S", 0.156);
            if (!PipeWallThicknessExists(12, "10")) AddPipeWallThickness(12, "10", 0.18);
            if (!PipeWallThicknessExists(12, "10S")) AddPipeWallThickness(12, "10S", 0.18);
            if (!PipeWallThicknessExists(12, "20")) AddPipeWallThickness(12, "20", 0.25);
            if (!PipeWallThicknessExists(12, "30")) AddPipeWallThickness(12, "30", 0.33);
            if (!PipeWallThicknessExists(12, "Std")) AddPipeWallThickness(12, "Std", 0.375);
            if (!PipeWallThicknessExists(12, "40S")) AddPipeWallThickness(12, "40S", 0.375);
            if (!PipeWallThicknessExists(12, "40")) AddPipeWallThickness(12, "40", 0.406);
            if (!PipeWallThicknessExists(12, "XS")) AddPipeWallThickness(12, "XS", 0.5);
            if (!PipeWallThicknessExists(12, "80S")) AddPipeWallThickness(12, "80S", 0.5);
            if (!PipeWallThicknessExists(12, "60")) AddPipeWallThickness(12, "60", 0.562);
            if (!PipeWallThicknessExists(12, "80")) AddPipeWallThickness(12, "80", 0.687);
            if (!PipeWallThicknessExists(12, "100")) AddPipeWallThickness(12, "100", 0.843);
            if (!PipeWallThicknessExists(12, "120")) AddPipeWallThickness(12, "120", 1);
            if (!PipeWallThicknessExists(12, "140")) AddPipeWallThickness(12, "140", 1.125);
            if (!PipeWallThicknessExists(12, "160")) AddPipeWallThickness(12, "160", 1.312);
            if (!PipeWallThicknessExists(11, "Std")) AddPipeWallThickness(11, "Std", 0.375);
            if (!PipeWallThicknessExists(11, "40S")) AddPipeWallThickness(11, "40S", 0.375);
            if (!PipeWallThicknessExists(11, "XS")) AddPipeWallThickness(11, "XS", 0.5);
            if (!PipeWallThicknessExists(11, "80S")) AddPipeWallThickness(11, "80S", 0.5);
            if (!PipeWallThicknessExists(14, "5S")) AddPipeWallThickness(14, "5S", 0.156);
            if (!PipeWallThicknessExists(14, "10S")) AddPipeWallThickness(14, "10S", 0.188);
            if (!PipeWallThicknessExists(14, "10")) AddPipeWallThickness(14, "10", 0.25);
            if (!PipeWallThicknessExists(14, "20")) AddPipeWallThickness(14, "20", 0.312);
            if (!PipeWallThicknessExists(14, "30")) AddPipeWallThickness(14, "30", 0.375);
            if (!PipeWallThicknessExists(14, "Std")) AddPipeWallThickness(14, "Std", 0.375);
            if (!PipeWallThicknessExists(14, "40")) AddPipeWallThickness(14, "40", 0.437);
            if (!PipeWallThicknessExists(14, "XS")) AddPipeWallThickness(14, "XS", 0.5);
            if (!PipeWallThicknessExists(14, "60")) AddPipeWallThickness(14, "60", 0.593);
            if (!PipeWallThicknessExists(14, "80")) AddPipeWallThickness(14, "80", 0.75);
            if (!PipeWallThicknessExists(14, "100")) AddPipeWallThickness(14, "100", 0.937);
            if (!PipeWallThicknessExists(14, "120")) AddPipeWallThickness(14, "120", 1.093);
            if (!PipeWallThicknessExists(14, "140")) AddPipeWallThickness(14, "140", 1.25);
            if (!PipeWallThicknessExists(14, "160")) AddPipeWallThickness(14, "160", 1.406);
            if (!PipeWallThicknessExists(16, "5S")) AddPipeWallThickness(16, "5S", 0.165);
            if (!PipeWallThicknessExists(16, "10S")) AddPipeWallThickness(16, "10S", 0.188);
            if (!PipeWallThicknessExists(16, "10")) AddPipeWallThickness(16, "10", 0.25);
            if (!PipeWallThicknessExists(16, "20")) AddPipeWallThickness(16, "20", 0.312);
            if (!PipeWallThicknessExists(16, "30")) AddPipeWallThickness(16, "30", 0.375);
            if (!PipeWallThicknessExists(16, "Std")) AddPipeWallThickness(16, "Std", 0.375);
            if (!PipeWallThicknessExists(16, "40")) AddPipeWallThickness(16, "40", 0.5);
            if (!PipeWallThicknessExists(16, "XS")) AddPipeWallThickness(16, "XS", 0.5);
            if (!PipeWallThicknessExists(16, "60")) AddPipeWallThickness(16, "60", 0.656);
            if (!PipeWallThicknessExists(16, "80")) AddPipeWallThickness(16, "80", 0.843);
            if (!PipeWallThicknessExists(16, "100")) AddPipeWallThickness(16, "100", 1.031);
            if (!PipeWallThicknessExists(16, "120")) AddPipeWallThickness(16, "120", 1.218);
            if (!PipeWallThicknessExists(16, "140")) AddPipeWallThickness(16, "140", 1.437);
            if (!PipeWallThicknessExists(16, "160")) AddPipeWallThickness(16, "160", 1.593);
            if (!PipeWallThicknessExists(18, "5S")) AddPipeWallThickness(18, "5S", 0.165);
            if (!PipeWallThicknessExists(18, "10S")) AddPipeWallThickness(18, "10S", 0.188);
            if (!PipeWallThicknessExists(18, "10")) AddPipeWallThickness(18, "10", 0.25);
            if (!PipeWallThicknessExists(18, "20")) AddPipeWallThickness(18, "20", 0.312);
            if (!PipeWallThicknessExists(18, "Std")) AddPipeWallThickness(18, "Std", 0.375);
            if (!PipeWallThicknessExists(18, "30")) AddPipeWallThickness(18, "30", 0.437);
            if (!PipeWallThicknessExists(18, "XS")) AddPipeWallThickness(18, "XS", 0.5);
            if (!PipeWallThicknessExists(18, "40")) AddPipeWallThickness(18, "40", 0.562);
            if (!PipeWallThicknessExists(18, "60")) AddPipeWallThickness(18, "60", 0.75);
            if (!PipeWallThicknessExists(18, "80")) AddPipeWallThickness(18, "80", 0.937);
            if (!PipeWallThicknessExists(18, "100")) AddPipeWallThickness(18, "100", 1.156);
            if (!PipeWallThicknessExists(18, "120")) AddPipeWallThickness(18, "120", 1.375);
            if (!PipeWallThicknessExists(18, "140")) AddPipeWallThickness(18, "140", 1.562);
            if (!PipeWallThicknessExists(18, "160")) AddPipeWallThickness(18, "160", 1.781);
            if (!PipeWallThicknessExists(20, "5S")) AddPipeWallThickness(20, "5S", 0.188);
            if (!PipeWallThicknessExists(20, "10S")) AddPipeWallThickness(20, "10S", 0.218);
            if (!PipeWallThicknessExists(20, "10")) AddPipeWallThickness(20, "10", 0.25);
            if (!PipeWallThicknessExists(20, "20")) AddPipeWallThickness(20, "20", 0.375);
            if (!PipeWallThicknessExists(20, "Std")) AddPipeWallThickness(20, "Std", 0.375);
            if (!PipeWallThicknessExists(20, "30")) AddPipeWallThickness(20, "30", 0.5);
            if (!PipeWallThicknessExists(20, "XS")) AddPipeWallThickness(20, "XS", 0.5);
            if (!PipeWallThicknessExists(20, "40")) AddPipeWallThickness(20, "40", 0.593);
            if (!PipeWallThicknessExists(20, "60")) AddPipeWallThickness(20, "60", 0.812);
            if (!PipeWallThicknessExists(20, "80")) AddPipeWallThickness(20, "80", 1.031);
            if (!PipeWallThicknessExists(20, "100")) AddPipeWallThickness(20, "100", 1.281);
            if (!PipeWallThicknessExists(20, "120")) AddPipeWallThickness(20, "120", 1.5);
            if (!PipeWallThicknessExists(20, "140")) AddPipeWallThickness(20, "140", 1.75);
            if (!PipeWallThicknessExists(20, "160")) AddPipeWallThickness(20, "160", 1.968);
            if (!PipeWallThicknessExists(22, "5S")) AddPipeWallThickness(22, "5S", 0.188);
            if (!PipeWallThicknessExists(22, "10S")) AddPipeWallThickness(22, "10S", 0.218);
            if (!PipeWallThicknessExists(22, "10")) AddPipeWallThickness(22, "10", 0.25);
            if (!PipeWallThicknessExists(22, "20")) AddPipeWallThickness(22, "20", 0.375);
            if (!PipeWallThicknessExists(22, "Std")) AddPipeWallThickness(22, "Std", 0.375);
            if (!PipeWallThicknessExists(22, "30")) AddPipeWallThickness(22, "30", 0.5);
            if (!PipeWallThicknessExists(22, "XS")) AddPipeWallThickness(22, "XS", 0.5);
            if (!PipeWallThicknessExists(22, "60")) AddPipeWallThickness(22, "60", 0.875);
            if (!PipeWallThicknessExists(22, "80")) AddPipeWallThickness(22, "80", 1.125);
            if (!PipeWallThicknessExists(22, "100")) AddPipeWallThickness(22, "100", 1.375);
            if (!PipeWallThicknessExists(22, "120")) AddPipeWallThickness(22, "120", 1.625);
            if (!PipeWallThicknessExists(22, "140")) AddPipeWallThickness(22, "140", 1.875);
            if (!PipeWallThicknessExists(22, "160")) AddPipeWallThickness(22, "160", 2.125);
            if (!PipeWallThicknessExists(24, "5S")) AddPipeWallThickness(24, "5S", 0.218);
            if (!PipeWallThicknessExists(24, "10")) AddPipeWallThickness(24, "10", 0.25);
            if (!PipeWallThicknessExists(24, "10S")) AddPipeWallThickness(24, "10S", 0.25);
            if (!PipeWallThicknessExists(24, "20")) AddPipeWallThickness(24, "20", 0.375);
            if (!PipeWallThicknessExists(24, "Std")) AddPipeWallThickness(24, "Std", 0.375);
            if (!PipeWallThicknessExists(24, "XS")) AddPipeWallThickness(24, "XS", 0.5);
            if (!PipeWallThicknessExists(24, "30")) AddPipeWallThickness(24, "30", 0.562);
            if (!PipeWallThicknessExists(24, "40")) AddPipeWallThickness(24, "40", 0.687);
            if (!PipeWallThicknessExists(24, "60")) AddPipeWallThickness(24, "60", 0.968);
            if (!PipeWallThicknessExists(24, "80")) AddPipeWallThickness(24, "80", 1.218);
            if (!PipeWallThicknessExists(24, "100")) AddPipeWallThickness(24, "100", 1.531);
            if (!PipeWallThicknessExists(24, "120")) AddPipeWallThickness(24, "120", 1.812);
            if (!PipeWallThicknessExists(24, "140")) AddPipeWallThickness(24, "140", 2.062);
            if (!PipeWallThicknessExists(24, "160")) AddPipeWallThickness(24, "160", 2.343);
            if (!PipeWallThicknessExists(26, "10")) AddPipeWallThickness(26, "10", 0.312);
            if (!PipeWallThicknessExists(26, "Std")) AddPipeWallThickness(26, "Std", 0.375);
            if (!PipeWallThicknessExists(26, "20")) AddPipeWallThickness(26, "20", 0.5);
            if (!PipeWallThicknessExists(26, "XS")) AddPipeWallThickness(26, "XS", 0.5);
            if (!PipeWallThicknessExists(28, "10")) AddPipeWallThickness(28, "10", 0.312);
            if (!PipeWallThicknessExists(28, "Std")) AddPipeWallThickness(28, "Std", 0.375);
            if (!PipeWallThicknessExists(28, "20")) AddPipeWallThickness(28, "20", 0.5);
            if (!PipeWallThicknessExists(28, "XS")) AddPipeWallThickness(28, "XS", 0.5);
            if (!PipeWallThicknessExists(28, "30")) AddPipeWallThickness(28, "30", 0.625);
            if (!PipeWallThicknessExists(30, "5S")) AddPipeWallThickness(30, "5S", 0.25);
            if (!PipeWallThicknessExists(30, "10")) AddPipeWallThickness(30, "10", 0.312);
            if (!PipeWallThicknessExists(30, "10S")) AddPipeWallThickness(30, "10S", 0.312);
            if (!PipeWallThicknessExists(30, "Std")) AddPipeWallThickness(30, "Std", 0.375);
            if (!PipeWallThicknessExists(30, "20")) AddPipeWallThickness(30, "20", 0.5);
            if (!PipeWallThicknessExists(30, "XS")) AddPipeWallThickness(30, "XS", 0.5);
            if (!PipeWallThicknessExists(30, "30")) AddPipeWallThickness(30, "30", 0.625);
            if (!PipeWallThicknessExists(30, "40")) AddPipeWallThickness(30, "40", 0.75);
            if (!PipeWallThicknessExists(32, "10")) AddPipeWallThickness(32, "10", 0.312);
            if (!PipeWallThicknessExists(32, "Std")) AddPipeWallThickness(32, "Std", 0.375);
            if (!PipeWallThicknessExists(32, "20")) AddPipeWallThickness(32, "20", 0.5);
            if (!PipeWallThicknessExists(32, "XS")) AddPipeWallThickness(32, "XS", 0.5);
            if (!PipeWallThicknessExists(32, "30")) AddPipeWallThickness(32, "30", 0.625);
            if (!PipeWallThicknessExists(32, "40")) AddPipeWallThickness(32, "40", 0.688);
            if (!PipeWallThicknessExists(34, "10")) AddPipeWallThickness(34, "10", 0.312);
            if (!PipeWallThicknessExists(34, "Std")) AddPipeWallThickness(34, "Std", 0.375);
            if (!PipeWallThicknessExists(34, "20")) AddPipeWallThickness(34, "20", 0.5);
            if (!PipeWallThicknessExists(34, "XS")) AddPipeWallThickness(34, "XS", 0.5);
            if (!PipeWallThicknessExists(34, "30")) AddPipeWallThickness(34, "30", 0.625);
            if (!PipeWallThicknessExists(34, "40")) AddPipeWallThickness(34, "40", 0.688);
            if (!PipeWallThicknessExists(36, "10")) AddPipeWallThickness(36, "10", 0.312);
            if (!PipeWallThicknessExists(36, "Std")) AddPipeWallThickness(36, "Std", 0.375);
            if (!PipeWallThicknessExists(36, "20")) AddPipeWallThickness(36, "20", 0.5);
            if (!PipeWallThicknessExists(36, "XS")) AddPipeWallThickness(36, "XS", 0.5);
            if (!PipeWallThicknessExists(36, "30")) AddPipeWallThickness(36, "30", 0.625);
            if (!PipeWallThicknessExists(36, "40")) AddPipeWallThickness(36, "40", 0.75);
            if (!PipeWallThicknessExists(42, "Std")) AddPipeWallThickness(42, "Std", 0.375);
            if (!PipeWallThicknessExists(42, "20")) AddPipeWallThickness(42, "20", 0.5);
            if (!PipeWallThicknessExists(42, "XS")) AddPipeWallThickness(42, "XS", 0.5);
            if (!PipeWallThicknessExists(42, "30")) AddPipeWallThickness(42, "30", 0.625);
            if (!PipeWallThicknessExists(42, "40")) AddPipeWallThickness(42, "40", 0.75);
            if (!PipeWallThicknessExists(48, "Std")) AddPipeWallThickness(48, "Std", 0.375);
            if (!PipeWallThicknessExists(48, "XS")) AddPipeWallThickness(48, "XS", 0.5);
            ObjectSpace.CommitChanges();
        }
        private bool PipeWallThicknessExists(double nominalPipeSizeInInches, string schedule)
        {
            PipeWallThickness value = ObjectSpace.FindObject<PipeWallThickness>(
                CriteriaOperator.Parse("NPS.NominalSizeInInches=? and Schedule.Name=?", nominalPipeSizeInInches, schedule));
            return value != null;
        }
        private void AddPipeWallThickness(double nominalPipeSizeInInches, string schedule, double wallThickness)
        {
            PipeWallThickness thickness = ObjectSpace.CreateObject<PipeWallThickness>();

            NominalPipeSize nps = ObjectSpace.FindObject<NominalPipeSize>(
                CriteriaOperator.Parse("NominalSizeInInches=?", nominalPipeSizeInInches));
            
            PipingSchedule pipingSchedule = ObjectSpace.FindObject<PipingSchedule>(
                CriteriaOperator.Parse("Name=?", schedule));

            thickness.WallThickness = wallThickness;
            thickness.NPS = nps;
            thickness.Schedule = pipingSchedule;
        }

        private void CreateMetallurgyGeneralTypes()
        {
            if (!MetallurgyGeneralTypeExists("Carbon Steel")) ObjectSpace.CreateObject<MetallurgyGeneralType>().Name = "Carbon Steel";
            if (!MetallurgyGeneralTypeExists("Alloy Steel")) ObjectSpace.CreateObject<MetallurgyGeneralType>().Name = "Alloy Steel";
            if (!MetallurgyGeneralTypeExists("Stainless Steel")) ObjectSpace.CreateObject<MetallurgyGeneralType>().Name = "Stainless Steel";
            if (!MetallurgyGeneralTypeExists("PVC")) ObjectSpace.CreateObject<MetallurgyGeneralType>().Name = "PVC";
            if (!MetallurgyGeneralTypeExists("Bronze")) ObjectSpace.CreateObject<MetallurgyGeneralType>().Name = "Bronze";
            if (!MetallurgyGeneralTypeExists("Galvanized")) ObjectSpace.CreateObject<MetallurgyGeneralType>().Name = "Galvanized";
            ObjectSpace.CommitChanges();
        }
        private bool MetallurgyGeneralTypeExists(string name)
        {
            MetallurgyGeneralType value = ObjectSpace.FindObject<MetallurgyGeneralType>(
                new BinaryOperator("Name", name));
            return value != null;
        }

        private void CreateMetallurgyMaterials()
        {
            if (!MetallurgyMaterialExists("Carbon Steel")) AddMetallurgyMaterial("Carbon Steel", "Carbon Steel", "A181-Gr.1 A181-Gr.2", "", "A234-WPA WPB WPC", "");
            if (!MetallurgyMaterialExists("Carbon Steel - Moderate, High Temp Service")) AddMetallurgyMaterial("Carbon Steel", "Carbon Steel - Moderate, High Temp Service", "A105-Gr.1 A105-Gr.2", "A216-WCA WCB WCC", "", "A106-Gr.B/API 5 LB");
            if (!MetallurgyMaterialExists("Carbon Steel - Cold Temp Service")) AddMetallurgyMaterial("Carbon Steel", "Carbon Steel - Cold Temp Service", "A350-LF1 A350-LF2", "A352-LCB, LCC", "A420-WPL6", "A33-Gr.6/A 671");
            if (!MetallurgyMaterialExists("Carbon - 1/2 Moly")) AddMetallurgyMaterial("Alloy Steel", "Carbon - 1/2 Moly", "", "", "", "");
            if (!MetallurgyMaterialExists("Carbon - 1/2 Moly - High Temp Service")) AddMetallurgyMaterial("Alloy Steel", "Carbon - 1/2 Moly - High Temp Service", "A182-F1", "A217-WC1", "A234-WP1", "");
            if (!MetallurgyMaterialExists("Carbon - 1/2 Moly - Cold Temp Service")) AddMetallurgyMaterial("Alloy Steel", "Carbon - 1/2 Moly - Cold Temp Service", "", "A352-LC1", "", "");
            if (!MetallurgyMaterialExists("1/2Cr-1/2Mo")) AddMetallurgyMaterial("Alloy Steel", "1/2Cr-1/2Mo", "A182-F2", "", "", "");
            if (!MetallurgyMaterialExists("1/2Cr-1/2Mo-1")) AddMetallurgyMaterial("Alloy Steel", "1/2Cr", "", "A217-WC4", "", "");
            if (!MetallurgyMaterialExists("3/4Cr-1 Mo-3/4NI")) AddMetallurgyMaterial("Alloy Steel", "3/4Cr-1 Mo-3/4NI", "", "A217-WC5", "", "");
            if (!MetallurgyMaterialExists("1Cr-1/2Mo")) AddMetallurgyMaterial("Alloy Steel", "1Cr-1/2Mo", "A182-F12", "", "A234-WP12", "");
            if (!MetallurgyMaterialExists("1Cr-1 Mo-Vd")) AddMetallurgyMaterial("Alloy Steel", "1Cr-1 Mo-Vd", "A404-F24", "A389-C24", "", "");
            if (!MetallurgyMaterialExists("1-1/4Cr-1/2Mo")) AddMetallurgyMaterial("Alloy Steel", "1-1/4Cr-1/2Mo", "A182-F11", "A217-WC6", "A234-WP11", "");
            if (!MetallurgyMaterialExists("1-1/4Cr-1/2Mo-Vd")) AddMetallurgyMaterial("Alloy Steel", "1-1/4Cr-1/2Mo-Vd", "", "A389-C23", "", "");
            if (!MetallurgyMaterialExists("2-1/4Cr-1/2Mo-Vd")) AddMetallurgyMaterial("Alloy Steel", "2-1/4Cr-1/2Mo-Vd", "A182-F22", "A217-WC9", "A234-WP22", "");
            if (!MetallurgyMaterialExists("3Cr-1 Mo")) AddMetallurgyMaterial("Alloy Steel", "3Cr-1 Mo", "A182-F21", "", "", "");
            if (!MetallurgyMaterialExists("5Cr-1/2Mo")) AddMetallurgyMaterial("Alloy Steel", "5Cr-1/2Mo", "A182-F5", "", "A234-WP5", "");
            if (!MetallurgyMaterialExists("5Cr-1/2Mo-Si")) AddMetallurgyMaterial("Alloy Steel", "5Cr-1/2Mo-Si", "", "A217-Gr.C5", "", "");
            if (!MetallurgyMaterialExists("7Cr-1/2Mo")) AddMetallurgyMaterial("Alloy Steel", "7Cr-1/2Mo", "A182-F7", "", "A234-WP7", "");
            if (!MetallurgyMaterialExists("9Cr-1Mo")) AddMetallurgyMaterial("Alloy Steel", "9Cr-1Mo", "A182-F9", "A217-Gr.C12", "A234-WP9", "");
            if (!MetallurgyMaterialExists("13Cr")) AddMetallurgyMaterial("Alloy Steel", "13Cr", "A182-F6", "A351-CA15", "", "");
            if (!MetallurgyMaterialExists("304 SS - Standard")) AddMetallurgyMaterial("Stainless Steel", "304 SS - Standard", "A182-F304", "A351-Gr.CF8 CF8a", "A403-WP304", "A312-TP304");
            if (!MetallurgyMaterialExists("304 SS - Low Carbon")) AddMetallurgyMaterial("Stainless Steel", "304 SS - Low Carbon", "A182-F304-L", "A351-Gr.CF3 CF3a", "A403-WP304-L", "A312-Tp304L");
            if (!MetallurgyMaterialExists("304 SS - High Temp Service")) AddMetallurgyMaterial("Stainless Steel", "304 SS - High Temp Service", "A182-F304-H", "A351.Gr.CF10", "A403-WP304-H", "A312-TP304H");
            if (!MetallurgyMaterialExists("309 SS")) AddMetallurgyMaterial("Stainless Steel", "309 SS", "", "", "A403-WP309", "");
            if (!MetallurgyMaterialExists("310 SS")) AddMetallurgyMaterial("Stainless Steel", "310 SS", "A182-F310", "", "A403-WP310", "A312-TP310");
            if (!MetallurgyMaterialExists("316 SS - Standard")) AddMetallurgyMaterial("Stainless Steel", "316 SS - Standard", "A182-F316", "A351-Gr.CF8M", "A403-WP316", "A312-TP316");
            if (!MetallurgyMaterialExists("316 SS - Low Carbon")) AddMetallurgyMaterial("Stainless Steel", "316 SS - Low Carbon", "A182-F316-L", "A351-Gr.CF3M", "A403-WP316-L", "A312-TP316L");
            if (!MetallurgyMaterialExists("316 SS - High Temp Service")) AddMetallurgyMaterial("Stainless Steel", "316 SS - High Temp Service", "A182-F316-H", "A351.Gr.CF10", "A403-WP316-H", "A312-TP316H");
            if (!MetallurgyMaterialExists("317 SS")) AddMetallurgyMaterial("Stainless Steel", "317 SS", "", "A403-WP317", "", "");
            if (!MetallurgyMaterialExists("321 SS - Standard")) AddMetallurgyMaterial("Stainless Steel", "321 SS - Standard", "A182-F321", "", "A403-WP321", "");
            if (!MetallurgyMaterialExists("321 SS - High Temp Service")) AddMetallurgyMaterial("Stainless Steel", "321 SS - High Temp Service", "A182-F321-H", "", "A403-WP321-H", "");
            if (!MetallurgyMaterialExists("347 SS - Standard")) AddMetallurgyMaterial("Stainless Steel", "347 SS - Standard", "A182-F347", "", "A403-WP347", "");
            if (!MetallurgyMaterialExists("347 SS - High Temp Service")) AddMetallurgyMaterial("Stainless Steel", "347 SS - High Temp Service", "A182-F347-H", "A351-Gr.CF8C", "A403-WP347-H", "");
            if (!MetallurgyMaterialExists("348 SS - Standard")) AddMetallurgyMaterial("Stainless Steel", "348 SS - Standard", "A182-F348", "", "A403-WP348", "");
            if (!MetallurgyMaterialExists("348 SS - High Temp Service")) AddMetallurgyMaterial("Stainless Steel", "348 SS - High Temp Service", "A182-F348-H", "", "", "");
            if (!MetallurgyMaterialExists("Duplex SS")) AddMetallurgyMaterial("Stainless Steel", "Duplex SS", "", "", "", "");
            if (!MetallurgyMaterialExists("Hastelloy C276")) AddMetallurgyMaterial("Alloy Steel", "Hastelloy C276", "", "", "", "");
            if (!MetallurgyMaterialExists("PVC")) AddMetallurgyMaterial("PVC", "PVC", "", "", "", "");
            if (!MetallurgyMaterialExists("Bronze")) AddMetallurgyMaterial("Alloy Steel", "Bronze", "", "", "", "");
            if (!MetallurgyMaterialExists("Galvanized")) AddMetallurgyMaterial("Alloy", "Galvanized", "", "", "", "");
            if (!MetallurgyMaterialExists("20 Ni-8 Cr")) AddMetallurgyMaterial("Alloy", "20 Ni-8 Cr", "A182-F10", "", "", "");
            if (!MetallurgyMaterialExists("2 Nickel Alloy Steel - Low Temp Service")) AddMetallurgyMaterial("Alloy Steel", "2 Nickel Alloy Steel - Low Temp Service", "", "A352-LC2", "", "");
            ObjectSpace.CommitChanges();

        }
        private bool MetallurgyMaterialExists(string name)
        {
            MetallurgyMaterial value = ObjectSpace.FindObject<MetallurgyMaterial>(new BinaryOperator("Name", name));
            return value != null;
        }
        private void AddMetallurgyMaterial(string materialType, string materialName, string forgings, string castings, string wroughtFittings, string pipe)
        {
            MetallurgyMaterial item = ObjectSpace.CreateObject<MetallurgyMaterial>();
            item.Name = materialName;
            item.Type = ObjectSpace.FindObject<MetallurgyGeneralType>(new BinaryOperator("Name", materialType));
            item.Forgings = forgings;
            item.Castings = castings;
            item.WroughtFittings = wroughtFittings;
            item.Pipe = pipe;
        }
    }
}
