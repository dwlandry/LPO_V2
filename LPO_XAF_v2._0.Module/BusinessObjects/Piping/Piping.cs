using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
//using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using LPO_XAF_v2._0.Module.BusinessObjects.Project;
//using System.ComponentModel.DataAnnotations;

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


        [Association("Project-PipingLines")]
        public Project.Project Project { get => project; set => SetPropertyValue(nameof(Project), ref project, value); }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string LineNumber { get => lineNumber; set => SetPropertyValue(nameof(LineNumber), ref lineNumber, value); }

        [DisplayName("NPS")]
        public NominalPipeSize NPS { get => nPS; set => SetPropertyValue(nameof(NPS), ref nPS, value); }

        public Metallurgy Metallurgy { get => metallurgy; set => SetPropertyValue(nameof(Metallurgy), ref metallurgy, value); }

        public PipingSchedule Schedule { get => schedule; set => SetPropertyValue(nameof(Schedule), ref schedule, value); }

        [DataSourceCriteria("Client.Oid = '@This.Project.Client.Oid'")]
        public ClientPipeSpec PipeSpec { get => pipeSpec; set => SetPropertyValue(nameof(PipeSpec), ref pipeSpec, value); }

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