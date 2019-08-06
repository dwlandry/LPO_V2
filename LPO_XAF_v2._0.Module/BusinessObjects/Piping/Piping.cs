//-----------------------------------------------------------------------
// <copyright file="F:\Users\dlandry\Source\Repos\LPO_XAF_v2._0.Module\BusinessObjects\Piping\Piping.cs" company="David W. Landry III">
//     Author: _**David Landry**_
//     *Copyright (c) David W. Landry III. All rights reserved.*
// </copyright>
//-----------------------------------------------------------------------
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using LPO_XAF_v2._0.Module.BusinessObjects.Project;
using System;
using System.Linq;

namespace LPO_XAF_v2._0.Module.BusinessObjects.Piping
{
    [XafDefaultProperty("NominalSizeInInches")]
    public class NominalPipeSize : BaseObject
    {

        public NominalPipeSize(Session session) : base(session) { }

        double outerDiameterInInches;
        double nominalSizeInInches;

        public double NominalSizeInInches { get => nominalSizeInInches; set => SetPropertyValue(nameof(NominalSizeInInches), ref nominalSizeInInches, value); }

        public double OuterDiameterInInches { get => outerDiameterInInches; set => SetPropertyValue(nameof(OuterDiameterInInches), ref outerDiameterInInches, value); }
    }

    [XafDefaultProperty("LineNumber")]
    [DefaultClassOptions, CreatableItem(false), NavigationItem("Piping"), ImageName("Pipe")]
    [RuleCombinationOfPropertiesIsUnique("UniqueLineNumberPerProjectRule", DefaultContexts.Save, "Project, LineNumber",
        "This Line Number is already used in this project.  Please try a different Line Number.")]

    public class Line : BaseObject
    {

        public Line(Session session) : base(session) { }


        string remarks;
        string prefix_Unit;
        string leakTestMedium;
        bool pWHTRequired;
        bool nDERequired;
        string leakTestPressure;
        string tracing;
        string designPressure;
        string operatingPressure;
        string designTemp;
        string operatingTemp;
        string to;
        string from;
        string flowingMedium;
        string insulationType;
        string insulationThickness;
        string number;
        string serviceCode;
        string processDescription;
        ClientPipeSpec pipeSpec;
        NominalPipeSize nPS;
        Project.Project project;
        PipingSchedule schedule;
        //MetallurgyMaterial metallurgy;
        string lineNumber;
        //double outerDiameter;
        //double wallThickness;

        [Association("Project-PipingLines")]
        public Project.Project Project { get => project; set => SetPropertyValue(nameof(Project), ref project, value); }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string LineNumber { get => lineNumber; set => SetPropertyValue(nameof(LineNumber), ref lineNumber, value); }

        [DisplayName("Prefix/Unit")]
        [Size(20)]
        public string Prefix_Unit { get => prefix_Unit; set => SetPropertyValue(nameof(Prefix_Unit), ref prefix_Unit, value); }

        [Size(10)]
        public string ServiceCode { get => serviceCode; set => SetPropertyValue(nameof(ServiceCode), ref serviceCode, value); }

        [Size(20)]
        public string Number { get => number; set => SetPropertyValue(nameof(Number), ref number, value); }

        [DevExpress.Xpo.DisplayName("Size")]
        [ToolTip("Nominal Pipe Size in inches")]
        [LookupEditorMode(LookupEditorMode.AllItems)]
        public NominalPipeSize NPS { get => nPS; set => SetPropertyValue(nameof(NPS), ref nPS, value); }

        [DataSourceCriteria("Client.Oid = '@This.Project.Client.Oid'")]
        public ClientPipeSpec PipeSpec { get => pipeSpec; set => SetPropertyValue(nameof(PipeSpec), ref pipeSpec, value); }

        //public MetallurgyMaterial Metallurgy { get => metallurgy; set => SetPropertyValue(nameof(Metallurgy), ref metallurgy, value); }

        [DataSourceProperty("AvailableSchedules")]
        public PipingSchedule Schedule { get => schedule; set => SetPropertyValue(nameof(Schedule), ref schedule, value); }

        [Size(10)]
        public string InsulationThickness { get => insulationThickness; set => SetPropertyValue(nameof(InsulationThickness), ref insulationThickness, value); }

        [Size(10)]
        public string InsulationType { get => insulationType; set => SetPropertyValue(nameof(InsulationType), ref insulationType, value); }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string FlowingMedium { get => flowingMedium; set => SetPropertyValue(nameof(FlowingMedium), ref flowingMedium, value); }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string From { get => from; set => SetPropertyValue(nameof(From), ref from, value); }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string To { get => to; set => SetPropertyValue(nameof(To), ref to, value); }

        [Size(10)]
        public string OperatingTemp { get => operatingTemp; set => SetPropertyValue(nameof(OperatingTemp), ref operatingTemp, value); }

        [Size(10)]
        public string DesignTemp { get => designTemp; set => SetPropertyValue(nameof(DesignTemp), ref designTemp, value); }

        [Size(10)]
        public string OperatingPressure { get => operatingPressure; set => SetPropertyValue(nameof(OperatingPressure), ref operatingPressure, value); }

        [Size(10)]
        public string DesignPressure { get => designPressure; set => SetPropertyValue(nameof(DesignPressure), ref designPressure, value); }

        [Size(10)]
        public string LeakTestPressure { get => leakTestPressure; set => SetPropertyValue(nameof(LeakTestPressure), ref leakTestPressure, value); }

        [ToolTip("Typically H2O. For specific situations, another fluid might be required.  Also, pneumatic media may be required, but this is potentially more dangerous than hydrostatic tests and extreme care should be exercised.")]
        [Size(20)]
        public string LeakTestMedium { get => leakTestMedium; set => SetPropertyValue(nameof(LeakTestMedium), ref leakTestMedium, value); }

        [ToolTip("Non-Destructive Examination")]
        [CaptionsForBoolValues("YES", "NO")]
        [DisplayName("NDE")]
        public bool NDERequired { get => nDERequired; set => SetPropertyValue(nameof(NDERequired), ref nDERequired, value); }

        [ToolTip("Post-Weld Heat Treatment")]
        [CaptionsForBoolValues("YES", "NO")]
        [DisplayName("PWHT")]
        public bool PWHTRequired { get => pWHTRequired; set => SetPropertyValue(nameof(PWHTRequired), ref pWHTRequired, value); }

        [Size(20)]
        public string Tracing { get => tracing; set => SetPropertyValue(nameof(Tracing), ref tracing, value); }

        [Size(200)]
        public string Remarks { get => remarks; set => SetPropertyValue(nameof(Remarks), ref remarks, value); }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string ProcessDescription { get => processDescription; set => SetPropertyValue(nameof(ProcessDescription), ref processDescription, value); }

        [Association("Line-Instruments")]
        [DataSourceCriteria("Project.Oid = '@This.Project.Oid'")]
        public XPCollection<Instrument.Instrument> Instruments { get { return GetCollection<Instrument.Instrument>(nameof(Instruments)); } }

        [Association("PIDs-Lines")]
        [DataSourceCriteria("Project.Oid = '@This.Project.Oid'")]
        public XPCollection<Instrument.PID> PIDs { get { return GetCollection<Instrument.PID>(nameof(PIDs)); } }

        [VisibleInDetailView(false), VisibleInListView(false)]
        public XPCollection<PipingSchedule> AvailableSchedules
        {
            get
            {
                if (NPS != null)
                {
                    XPQuery<PipingSchedule> schedules = new XPQuery<PipingSchedule>(Session);
                    XPQuery<PipeWallThickness> thickness = new XPQuery<PipeWallThickness>(Session);
                    var list = from s in schedules
                               join t in thickness on s.Oid equals t.Schedule.Oid
                               where t.NPS.Oid == NPS.Oid
                               select s;

                    return new XPCollection<PipingSchedule>(Session, new InOperator("Oid", list));//, new InOperator("Name",);
                }
                return null;
            }
        }
        //[ModelDefault("DisplayFormat", "F5")]
        //[DisplayName("Outer Diameter (in)")]
        //public double OuterDiameter
        //{
        //    get
        //    {
        //        outerDiameter = NPS == null ? 0 : NPS.OuterDiameterInInches;
        //        return outerDiameter;
        //    }
        //}
        //[ModelDefault("DisplayFormat", "F5")]
        //[DisplayName("Wall Thickness (in)")]
        //public double WallThickness
        //{
        //    get
        //    {
        //        try
        //        {
        //            wallThickness = Session.FindObject<PipeWallThickness>(CriteriaOperator.Parse("NPS=? and Schedule=?", NPS, Schedule)).WallThickness;
        //        }
        //        catch (Exception)
        //        {
        //            wallThickness = 0;
        //        }
        //        return wallThickness;
        //    }
        //}
        //[ModelDefault("DisplayFormat", "F5")]
        //[DisplayName("Inner Diameter (in)")]
        //public double InnerDiameter => outerDiameter - 2 * wallThickness;
        //[ModelDefault("DisplayFormat", "F5")]
        //[DisplayName("Inner Area (in^2)")]
        //public double InnerAreaInSquareInches => Math.PI * Math.Pow(InnerDiameter / 2, 2);
        //[ModelDefault("DisplayFormat", "F5")]
        //[DisplayName("Metal Area (in^2)")]
        //public double MetalAreaInSquareInches => Math.PI * Math.Pow(outerDiameter / 2, 2) - InnerAreaInSquareInches;
        //[ModelDefault("DisplayFormat", "F5")]
        //[DisplayName("Wt of CS Pipe per Foot (lbs)")]
        //public double WeightOfCarbonSteelPipePerFootInPounds => 10.6802 * wallThickness * (outerDiameter - wallThickness);
        //[ModelDefault("DisplayFormat", "F5")]
        //[DisplayName("Wt of Water per Foot (lbs)")]
        //public double WeightOfWaterPerFootInPounds => 0.3405 * Math.Pow(InnerDiameter, 2);
        //[ModelDefault("DisplayFormat", "F5")]
        //[DisplayName("Wt of Ferritic SS Pipe per Foot (lbs)")]
        //public double WeightOfFerriticSSPipePerFootInPounds => 0.95 * WeightOfCarbonSteelPipePerFootInPounds;
        //[ModelDefault("DisplayFormat", "F5")]
        //[DisplayName("Wt of Austentic SS Pipe per Foot (lbs)")]
        //public double WeightOfAustenticSSPipePerFootInPounds => 1.02 * WeightOfCarbonSteelPipePerFootInPounds;
        //[ModelDefault("DisplayFormat", "F5")]
        //[DisplayName("Inner Surface Area per Foot (ft^2)")]
        //public double InnerSurfaceAreaPerFootInFeetSquared => 0.2618 * InnerDiameter;
        //[ModelDefault("DisplayFormat", "F5")]
        //[DisplayName("Moment of Inertia (in^4)")]
        //public double MomentOfInertiaInInches4 => 0.0491 * (Math.Pow(outerDiameter, 4) - Math.Pow(InnerDiameter, 4));
        //[ModelDefault("DisplayFormat", "F5")]
        //[DisplayName("Elastic Section Modulus (in^3)")]
        //public double ElasticSectionModulusInInches3 => 0.0982 * (Math.Pow(outerDiameter, 4) - Math.Pow(InnerDiameter, 4)) / outerDiameter;
        //[ModelDefault("DisplayFormat", "F5")]
        //[DisplayName("Plastic Section Modulus (in^3)")]
        //public double PlasticSectionModulusInInches3 => Math.Pow(outerDiameter, 3) - Math.Pow(InnerDiameter, 3) / 6;
        //[ModelDefault("DisplayFormat", "F5")]
        //[DisplayName("Radius of Gyration (in)")]
        //public double RadiusOfGyrationInInches => 0.25 * Math.Pow(Math.Pow(outerDiameter, 2) - Math.Pow(InnerDiameter, 2), 0.5);
    }

    [DefaultClassOptions, CreatableItem(false), NavigationItem("Piping"), ImageName("Pipe")]
    public class MetallurgyMaterial : BaseObject
    {

        public MetallurgyMaterial(Session session) : base(session) { }


        string pipe;
        string wroughtFittings;
        string castings;
        string forgings;
        MetallurgyGeneralType type;
        string description;
        string alias;
        string name;


        [Association("MetallurgyGeneralType-Materials")]
        public MetallurgyGeneralType Type { get => type; set => SetPropertyValue(nameof(Type), ref type, value); }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        [RuleUniqueValue]
        public string Name { get => name; set => SetPropertyValue(nameof(Name), ref name, value); }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Forgings { get => forgings; set => SetPropertyValue(nameof(Forgings), ref forgings, value); }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Castings { get => castings; set => SetPropertyValue(nameof(Castings), ref castings, value); }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string WroughtFittings { get => wroughtFittings; set => SetPropertyValue(nameof(WroughtFittings), ref wroughtFittings, value); }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Pipe { get => pipe; set => SetPropertyValue(nameof(Pipe), ref pipe, value); }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Alias { get => alias; set => SetPropertyValue(nameof(Alias), ref alias, value); }

        [Size(300)]
        public string Description { get => description; set => SetPropertyValue(nameof(Description), ref description, value); }

    }

    public class MetallurgyGeneralType : BaseObject
    {
        public MetallurgyGeneralType(Session session) : base(session) { }

        string name;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        [RuleUniqueValue]
        public string Name { get => name; set => SetPropertyValue(nameof(Name), ref name, value); }
        [Association("MetallurgyGeneralType-Materials")]
        public XPCollection<MetallurgyMaterial> Materials { get { return GetCollection<MetallurgyMaterial>(nameof(Materials)); } }
    }

    public class PipingStandard : BaseObject
    {

        public PipingStandard(Session session) : base(session) { }

        string description;
        string name;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        [RuleUniqueValue]
        [ModelDefault("AllowEdit", "False")]
        public string Name { get => name; set => SetPropertyValue(nameof(Name), ref name, value); }

        [Size(SizeAttribute.Unlimited)]
        //[ReadOnly(true)]
        public string Description { get => description; set => SetPropertyValue(nameof(Description), ref description, value); }

    }

    public class PipingSchedule : BaseObject
    {

        public PipingSchedule(Session session) : base(session) { }


        string name;
        PipingStandard pipingStandard;

        [Size(20), RuleUniqueValue]
        [ModelDefault("AllowEdit", "False")]
        public string Name { get => name; set => SetPropertyValue(nameof(Name), ref name, value); }
        [ModelDefault("AllowEdit", "False")]
        public PipingStandard PipingStandard { get => pipingStandard; set => SetPropertyValue(nameof(PipingStandard), ref pipingStandard, value); }


    }


    [RuleCombinationOfPropertiesIsUnique("UniqueSpecNumberRule", DefaultContexts.Save, "Client, SpecNumber", "This Spec Number already exists for the Client.")]
    [XafDefaultProperty("SpecNumber")]
    [DefaultClassOptions, CreatableItem(false), NavigationItem("Piping"), ImageName("Pipe")]
    public class ClientPipeSpec : BaseObject
    {
        public ClientPipeSpec(Session session) : base(session) { }

        string title;
        DateTime revisionDate;
        string revision;
        FileData file;
        string specNumber;
        Client client;

        [Association("Client-PipeSpecs")]
        public Client Client { get => client; set => SetPropertyValue(nameof(Client), ref client, value); }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string SpecNumber { get => specNumber; set => SetPropertyValue(nameof(SpecNumber), ref specNumber, value); }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Revision { get => revision; set => SetPropertyValue(nameof(Revision), ref revision, value); }

        public DateTime RevisionDate { get => revisionDate; set => SetPropertyValue(nameof(RevisionDate), ref revisionDate, value); }

        [Size(200)]
        public string Title { get => title; set => SetPropertyValue(nameof(Title), ref title, value); }

        public FileData File { get => file; set => SetPropertyValue(nameof(File), ref file, value); }


    }

    [RuleCombinationOfPropertiesIsUnique("UniqueSizeAndScheduleRule", DefaultContexts.Save, "NPS, Schedule", "Combination of NPS and Schedule must be unique.")]
    [XafDefaultProperty("WallThickness")]
    public class PipeWallThickness : BaseObject
    {

        public PipeWallThickness(Session session) : base(session) { }


        double wallThickness;
        PipingSchedule schedule;
        NominalPipeSize nPS;

        [DevExpress.Xpo.DisplayName("NPS")]
        [ModelDefault("AllowEdit", "False")]
        public NominalPipeSize NPS { get => nPS; set => SetPropertyValue(nameof(NPS), ref nPS, value); }

        [ModelDefault("AllowEdit", "False")]
        public PipingSchedule Schedule { get => schedule; set => SetPropertyValue(nameof(Schedule), ref schedule, value); }

        [ModelDefault("AllowEdit", "False")]
        public double WallThickness { get => wallThickness; set => SetPropertyValue(nameof(WallThickness), ref wallThickness, value); }

    }

    [RuleCombinationOfPropertiesIsUnique("UniqueProjectAndTiePointNumberRule", DefaultContexts.Save, "Project, Number", "The Tie Point Number entered already exists for the project.")]
    public class PipingTiePoint : BaseObject
    {

        public PipingTiePoint(Session session) : base(session) { }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            this.Qty = TieInQuantity.One;
        }

        string service;
        TieInQuantity qty;
        bool tagHung;
        string note;
        bool hotTap;
        string tieInMethod;
        Line newLine;
        string description;
        BusinessObjects.Instrument.PID pID;
        Line existingLine;
        string number;
        Project.Project project;

        [Association("Project-PipingTiePoints")]
        public Project.Project Project { get => project; set => SetPropertyValue(nameof(Project), ref project, value); }

        [Size(20)]
        public string Number { get => number; set => SetPropertyValue(nameof(Number), ref number, value); }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Service { get => service; set => SetPropertyValue(nameof(Service), ref service, value); }

        [DataSourceCriteria("Project.Oid = '@This.Project.Oid'")]
        public Line ExistingLine { get => existingLine; set => SetPropertyValue(nameof(Line), ref existingLine, value); }

        [DataSourceCriteria("Project.Oid = '@This.Project.Oid'")]
        public Line NewLine { get => newLine; set => SetPropertyValue(nameof(NewLine), ref newLine, value); }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string TieInMethod { get => tieInMethod; set => SetPropertyValue(nameof(Type), ref tieInMethod, value); }

        public TieInQuantity Qty { get => qty; set => SetPropertyValue(nameof(Qty), ref qty, value); }

        [CaptionsForBoolValues("Yes", "No")]
        public bool HotTap { get => hotTap; set => SetPropertyValue(nameof(HotTap), ref hotTap, value); }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Note { get => note; set => SetPropertyValue(nameof(Note), ref note, value); }

        [CaptionsForBoolValues("Yes", "No")]
        public bool TagHung { get => tagHung; set => SetPropertyValue(nameof(TagHung), ref tagHung, value); }

        [Association("PID-TiePoints")]
        [DataSourceCriteria("Project.Oid = '@This.Project.Oid'")]
        public Instrument.PID PID { get => pID; set => SetPropertyValue(nameof(PID), ref pID, value); }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Description { get => description; set => SetPropertyValue(nameof(Description), ref description, value); }
    }

    public enum TieInQuantity
    {
        One = 1,
        Two = 2
    }


}