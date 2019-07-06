using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using LPO_XAF_v2._0.Module.BusinessObjects.Project;

namespace LPO_XAF_v2._0.Module.BusinessObjects.Piping
{
    //public class PipingMetallurgy : BaseObject
    //{
    //    
    //    public PipingMetallurgy(Session session) : base(session) { }
    //    
    //}

    [DefaultClassOptions,CreatableItem(false), NavigationItem("Piping")]
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
    [RuleCombinationOfPropertiesIsUnique("UniqueLineNumberPerProjectRule",DefaultContexts.Save, "Project, LineNumber", 
        "This Line Number is already used in this project.  Please try a different Line Number.")]
    public class Line : BaseObject
    {
        
        public Line(Session session) : base(session) { }


        ClientPipeSpec pipeSpec;
        NominalPipeSize nPS;
        Project.Project project;
        PipingSchedule schedule;
        Metallurgy metallurgy;
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

        public Metallurgy Metallurgy { get => metallurgy; set => SetPropertyValue(nameof(Metallurgy), ref metallurgy, value); }

        [DataSourceProperty("AvailableSchedules")]
        public PipingSchedule Schedule{ get => schedule; set => SetPropertyValue(nameof(Schedule), ref schedule, value);}

        [DataSourceCriteria("Client.Oid = '@This.Project.Client.Oid'")]
        public ClientPipeSpec PipeSpec { get => pipeSpec; set => SetPropertyValue(nameof(PipeSpec), ref pipeSpec, value); }

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

                    return new XPCollection<PipingSchedule>(Session, new InOperator("Oid",list));//, new InOperator("Name",);
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
        public double InnerAreaInSquareInches =>  Math.PI * Math.Pow(InnerDiameter / 2,2);
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
        public double RadiusOfGyrationInInches => 0.25 *Math.Pow(Math.Pow(outerDiameter,2) - Math.Pow(InnerDiameter,2),0.5);
    }

    public class Metallurgy : BaseObject
    {
        
        public Metallurgy(Session session) : base(session) { }


        string description;
        string alias;
        string name;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Name { get => name; set => SetPropertyValue(nameof(Name), ref name, value); }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Alias { get => alias; set => SetPropertyValue(nameof(Alias), ref alias, value); }

        [Size(300)]
        public string Description { get => description; set => SetPropertyValue(nameof(Description), ref description, value); }

    }

    [DefaultClassOptions, CreatableItem(false), NavigationItem("Piping")]
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

    [DefaultClassOptions, CreatableItem(false), NavigationItem("Piping")]
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
    public class ClientPipeSpec : BaseObject
    {
        public ClientPipeSpec(Session session) : base(session) { }

        FileData file;
        string specNumber;
        Client client;

        [Association("Client-PipeSpecs")]
        public Client Client { get => client; set => SetPropertyValue(nameof(Client), ref client, value); }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string SpecNumber { get => specNumber; set => SetPropertyValue(nameof(SpecNumber), ref specNumber, value); }

        public FileData File { get => file; set => SetPropertyValue(nameof(File), ref file, value); }


    }

    [RuleCombinationOfPropertiesIsUnique("UniqueSizeAndScheduleRule", DefaultContexts.Save, "NPS, Schedule", "Combination of NPS and Schedule must be unique.")]
    [XafDefaultProperty("WallThickness")]
    [DefaultClassOptions, CreatableItem(false), NavigationItem("Piping")]
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