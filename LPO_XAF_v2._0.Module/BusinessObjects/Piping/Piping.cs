//-----------------------------------------------------------------------
// <copyright file="F:\Users\dlandry\source\repos\LPO_XAF_v2.0\LPO_XAF_v2._0.Module\BusinessObjects\Piping\Piping.cs" company="David W. Landry III">
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


        ClientPipeSpec pipeSpec;
        NominalPipeSize nPS;
        Project.Project project;
        PipingSchedule schedule;
        //MetallurgyMaterial metallurgy;
        string lineNumber;
        double outerDiameter;
        double wallThickness;

        [Association("Project-PipingLines")]
        public Project.Project Project { get => project; set => SetPropertyValue(nameof(Project), ref project, value); }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string LineNumber { get => lineNumber; set => SetPropertyValue(nameof(LineNumber), ref lineNumber, value); }

        [DisplayName("NPS")]
        [ToolTip("Nominal Pipe Size in inches")]
        [LookupEditorMode(LookupEditorMode.AllItems)]
        public NominalPipeSize NPS { get => nPS; set => SetPropertyValue(nameof(NPS), ref nPS, value); }

        //public MetallurgyMaterial Metallurgy { get => metallurgy; set => SetPropertyValue(nameof(Metallurgy), ref metallurgy, value); }

        [DataSourceProperty("AvailableSchedules")]
        public PipingSchedule Schedule { get => schedule; set => SetPropertyValue(nameof(Schedule), ref schedule, value); }

        [DataSourceCriteria("Client.Oid = '@This.Project.Client.Oid'")]
        public ClientPipeSpec PipeSpec { get => pipeSpec; set => SetPropertyValue(nameof(PipeSpec), ref pipeSpec, value); }

        [Association("Line-Instruments")]
        [DataSourceCriteria("Project.Oid = '@This.Project.Oid'")]
        public XPCollection<Instrument.Instrument> Instruments { get { return GetCollection<Instrument.Instrument>(nameof(Instruments)); } }

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
        [ModelDefault("DisplayFormat", "F5")]
        [DisplayName("Outer Diameter (in)")]
        public double OuterDiameter
        {
            get
            {
                outerDiameter = NPS == null ? 0 : NPS.OuterDiameterInInches;
                return outerDiameter;
            }
        }
        [ModelDefault("DisplayFormat", "F5")]
        [DisplayName("Wall Thickness (in)")]
        public double WallThickness
        {
            get
            {
                try
                {
                    wallThickness = Session.FindObject<PipeWallThickness>(CriteriaOperator.Parse("NPS=? and Schedule=?", NPS, Schedule)).WallThickness;
                }
                catch (Exception)
                {
                    wallThickness = 0;
                }
                return wallThickness;
            }
        }
        [ModelDefault("DisplayFormat", "F5")]
        [DisplayName("Inner Diameter (in)")]
        public double InnerDiameter => outerDiameter - 2 * wallThickness;
        [ModelDefault("DisplayFormat", "F5")]
        [DisplayName("Inner Area (in^2)")]
        public double InnerAreaInSquareInches => Math.PI * Math.Pow(InnerDiameter / 2, 2);
        [ModelDefault("DisplayFormat", "F5")]
        [DisplayName("Metal Area (in^2)")]
        public double MetalAreaInSquareInches => Math.PI * Math.Pow(outerDiameter / 2, 2) - InnerAreaInSquareInches;
        [ModelDefault("DisplayFormat", "F5")]
        [DisplayName("Wt of CS Pipe per Foot (lbs)")]
        public double WeightOfCarbonSteelPipePerFootInPounds => 10.6802 * wallThickness * (outerDiameter - wallThickness);
        [ModelDefault("DisplayFormat", "F5")]
        [DisplayName("Wt of Water per Foot (lbs)")]
        public double WeightOfWaterPerFootInPounds => 0.3405 * Math.Pow(InnerDiameter, 2);
        [ModelDefault("DisplayFormat", "F5")]
        [DisplayName("Wt of Ferritic SS Pipe per Foot (lbs)")]
        public double WeightOfFerriticSSPipePerFootInPounds => 0.95 * WeightOfCarbonSteelPipePerFootInPounds;
        [ModelDefault("DisplayFormat", "F5")]
        [DisplayName("Wt of Austentic SS Pipe per Foot (lbs)")]
        public double WeightOfAustenticSSPipePerFootInPounds => 1.02 * WeightOfCarbonSteelPipePerFootInPounds;
        [ModelDefault("DisplayFormat", "F5")]
        [DisplayName("Inner Surface Area per Foot (ft^2)")]
        public double InnerSurfaceAreaPerFootInFeetSquared => 0.2618 * InnerDiameter;
        [ModelDefault("DisplayFormat", "F5")]
        [DisplayName("Moment of Inertia (in^4)")]
        public double MomentOfInertiaInInches4 => 0.0491 * (Math.Pow(outerDiameter, 4) - Math.Pow(InnerDiameter, 4));
        [ModelDefault("DisplayFormat", "F5")]
        [DisplayName("Elastic Section Modulus (in^3)")]
        public double ElasticSectionModulusInInches3 => 0.0982 * (Math.Pow(outerDiameter, 4) - Math.Pow(InnerDiameter, 4)) / outerDiameter;
        [ModelDefault("DisplayFormat", "F5")]
        [DisplayName("Plastic Section Modulus (in^3)")]
        public double PlasticSectionModulusInInches3 => Math.Pow(outerDiameter, 3) - Math.Pow(InnerDiameter, 3) / 6;
        [ModelDefault("DisplayFormat", "F5")]
        [DisplayName("Radius of Gyration (in)")]
        public double RadiusOfGyrationInInches => 0.25 * Math.Pow(Math.Pow(outerDiameter, 2) - Math.Pow(InnerDiameter, 2), 0.5);
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

        [DisplayName("NPS")]
        [ModelDefault("AllowEdit", "False")]
        public NominalPipeSize NPS { get => nPS; set => SetPropertyValue(nameof(NPS), ref nPS, value); }

        [ModelDefault("AllowEdit", "False")]
        public PipingSchedule Schedule { get => schedule; set => SetPropertyValue(nameof(Schedule), ref schedule, value); }

        [ModelDefault("AllowEdit", "False")]
        public double WallThickness { get => wallThickness; set => SetPropertyValue(nameof(WallThickness), ref wallThickness, value); }

    }


}