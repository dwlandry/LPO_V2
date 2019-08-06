//-----------------------------------------------------------------------
// <copyright file="F:\Users\dlandry\Source\Repos\LPO_XAF_v2._0.Module\BusinessObjects\Project\Project.cs" company="David W. Landry III">
//     Author: _**David Landry**_
//     *Copyright (c) David W. Landry III. All rights reserved.*
// </copyright>
//-----------------------------------------------------------------------
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using LPO_XAF_v2._0.Module.BusinessObjects.Mechanical;
using LPO_XAF_v2._0.Module.BusinessObjects.Products;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;

namespace LPO_XAF_v2._0.Module.BusinessObjects.Project
{
    [DefaultClassOptions, NavigationItem("Projects")]
    [ImageName("Project"), CreatableItem(false), DefaultProperty("DisplayName")]
    [DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    [Appearance("Closed or Lost Bid", TargetItems = "*;Status",
        Criteria = "Status = 2 | Status = 3", Enabled = false)]
    [Appearance("Lost Bid", TargetItems = "ProjectNumber, ProjectDescription, DisplayName, ClientProjectNumber, Client",
        Criteria = "Status = 3", FontColor = "DarkMagenta", FontStyle = FontStyle.Strikeout)]
    [Appearance("Closed", TargetItems = "ProjectNumber, ProjectDescription, DisplayName, ClientProjectNumber, Client",
        Criteria = "Status = 2", FontColor = "ForestGreen", FontStyle = FontStyle.Strikeout)]
    [Appearance("Bid", TargetItems = "ProjectNumber, ProjectDescription, DisplayName, ClientProjectNumber, Client",
        Criteria = "Status = 0", FontColor = "DarkMagenta")]

    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class Project : BaseObject
    {
        public Project(Session session) : base(session) { }
        public override void AfterConstruction()
        {
            Status = ProjectStatus.Active;
            base.AfterConstruction();
        }

        byte[] projectOverview;
        string projectFolder;
        string clientProjectNumber;
        byte[] scope;
        ProjectStatus status;
        Client client;
        string projectDescription;
        string projectNumber;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        [RuleUniqueValue("RuleUniqueField for Project.ProjectNumber", DefaultContexts.Save, "The Project Number entered already exists. Please enter a unique Project Number.")]
        [RuleRequiredField("RuleRequiredField for Project.ProjectNumber", DefaultContexts.Save, "A Project Number must be specified.")]
        public string ProjectNumber { get => projectNumber; set => SetPropertyValue(nameof(ProjectNumber), ref projectNumber, value); }

        [Size(200)]
        [EditorAlias("HyperLinkStringPropertyEditor")]
        public string ProjectFolder
        {
            get => projectFolder;
            set => SetPropertyValue(nameof(ProjectFolder), ref projectFolder, value);
        }
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string ClientProjectNumber { get => clientProjectNumber; set => SetPropertyValue(nameof(ClientProjectNumber), ref clientProjectNumber, value); }

        [Size(200)]
        public string ProjectDescription { get => projectDescription; set => SetPropertyValue(nameof(ProjectDescription), ref projectDescription, value); }

        [Association("Client-Projects")]
        [RuleRequiredField("RuleRequiredField for Project.Client", DefaultContexts.Save, "A Client must be specified.")]
        public Client Client { get => client; set => SetPropertyValue(nameof(Client), ref client, value); }

        public ProjectStatus Status { get => status; set => SetPropertyValue(nameof(Status), ref status, value); }


        [Size(SizeAttribute.Unlimited)]
        [EditorAlias(EditorAliases.RichTextPropertyEditor)]
        public byte[] ProjectOverview
        {
            get => projectOverview;
            set => SetPropertyValue(nameof(ProjectOverview), ref projectOverview, value);
        }

        [Size(SizeAttribute.Unlimited)]
        [EditorAlias(EditorAliases.RichTextPropertyEditor)]
        public byte[] ProjectScope { get => scope; set => SetPropertyValue(nameof(ProjectScope), ref scope, value); }

        [Association("Project-Instruments"), Aggregated]
        public XPCollection<Instrument.Instrument> Instruments => GetCollection<Instrument.Instrument>(nameof(Instruments));

        [DevExpress.Xpo.DisplayName("Project")]
        public string DisplayName
        {
            get
            {
                string result = string.Empty;
                if (client != null)
                {
                    //return $"('{client.Name ?? "null client"}') '{projectNumber ?? "XXXX"}' - '{projectDescription ?? ""}'";
                    result = $"({client.Name}) {projectNumber} - {projectDescription}";
                }
                return result;
            }
        }

        [Association("Project-PlanDrawings"), Aggregated]
        public XPCollection<Instrument.PlanDrawing> PlanDrawings { get { return GetCollection<Instrument.PlanDrawing>(nameof(PlanDrawings)); } }

        [Association("Project-PIDs"), Aggregated]
        public XPCollection<Instrument.PID> PIDs { get { return GetCollection<Instrument.PID>(nameof(PIDs)); } }

        [Association("Project-LoopDrawings"), Aggregated]
        public XPCollection<Instrument.LoopDrawing> LoopDrawings => GetCollection<Instrument.LoopDrawing>(nameof(LoopDrawings));

        [Association("Project-WiringDrawings"), Aggregated]
        public XPCollection<Instrument.WiringDrawing> WiringDrawings { get { return GetCollection<Instrument.WiringDrawing>(nameof(WiringDrawings)); } }

        [Association("Project-LayoutDrawings"), Aggregated]
        public XPCollection<Instrument.LayoutDrawing> LayoutDrawings { get { return GetCollection<Instrument.LayoutDrawing>(nameof(LayoutDrawings)); } }

        [DataSourceCriteria("Project.Oid = '@This.Oid'")]
        [Association("Project-AreaClassificationDrawings")]
        public XPCollection<Instrument.AreaClassificationDrawing> AreaClassificationDrawings { get { return GetCollection<Instrument.AreaClassificationDrawing>(nameof(AreaClassificationDrawings)); } }

        [DataSourceCriteria("Project.Oid = '@This.Oid'")]
        [Association("Project-InstallationDetails")]
        public XPCollection<Instrument.ProjectInstallationDetail> InstallationDetails { get { return GetCollection<Instrument.ProjectInstallationDetail>(nameof(InstallationDetails)); } }

        [Association("Project-InstrumentSpecCheckPackages")]
        [VisibleInListView(false), VisibleInDetailView(false)]
        public XPCollection<Instrument.InstrumentSpecCheckPackage> InstrumentSpecCheckPackages => GetCollection<Instrument.InstrumentSpecCheckPackage>(nameof(InstrumentSpecCheckPackages));

        [Association("Project-JunctionBoxes"), Aggregated]
        [VisibleInListView(false)]
        public XPCollection<Instrument.JunctionBox> JunctionBoxes { get { return GetCollection<Instrument.JunctionBox>(nameof(JunctionBoxes)); } }

        [Association("Project-ControlSystems"), Aggregated]
        [VisibleInListView(false)]
        public XPCollection<Instrument.ControlSystem> ControlSystems { get { return GetCollection<Instrument.ControlSystem>(nameof(ControlSystems)); } }

        [Association("Project-InstrumentQuotes")]
        public XPCollection<Instrument.InstrumentQuote> InstrumentQuotes { get { return GetCollection<Instrument.InstrumentQuote>(nameof(InstrumentQuotes)); } }

        [Association("Project-PipingLines"), Aggregated]
        [DataSourceCriteria("Project.Oid = '@This.Oid'")]
        public XPCollection<Piping.Line> PipingLines { get { return GetCollection<Piping.Line>(nameof(PipingLines)); } }

        [Association("Project-PipingTiePoints"), Aggregated]
        [DataSourceCriteria("Project.Oid = '@This.Oid'")]
        public XPCollection<Piping.PipingTiePoint> TiePoints { get { return GetCollection<Piping.PipingTiePoint>(nameof(TiePoints)); } }

        [Association("Project-Equipment"), Aggregated]
        [DataSourceCriteria("Project.Oid = '@This.Oid'")]
        public XPCollection<Equipment> Equipment { get { return GetCollection<Equipment>(nameof(Equipment)); } }

        [Association("Project-Team"), Aggregated]
        [DataSourceCriteria("Project.Oid = '@This.Oid'")]
        public XPCollection<ProjectTeamMember> Team { get { return GetCollection<ProjectTeamMember>(nameof(Team)); } }
    }

    [DefaultClassOptions, NavigationItem("Projects")]
    [ImageName("BO_Customer"), CreatableItem(false)]
    public class Client : BaseObject
    {
        public Client(Session session) : base(session) { }
        public override void AfterConstruction() => base.AfterConstruction();

        FileData aMLDocument;
        byte[] sitePPERequirements;
        string name;

        [Size(50)]
        public string Name { get => name; set => SetPropertyValue(nameof(Name), ref name, value); }
        [Association("Client-Projects")]
        public XPCollection<Project> Projects => GetCollection<Project>(nameof(Projects));

        [Association("Client-Standards")]
        public XPCollection<ClientStandard> Standards { get { return GetCollection<ClientStandard>(nameof(Standards)); } }

        [DevExpress.Xpo.DisplayName("Site PPE Requirements")]
        [EditorAlias(EditorAliases.RichTextPropertyEditor)]
        [DetailViewLayout("Site Requirements")]
        public byte[] SitePPERequirements { get => sitePPERequirements; set => SetPropertyValue(nameof(SitePPERequirements), ref sitePPERequirements, value); }

        [Association("Client-Contacts"), Aggregated]
        public XPCollection<ClientContact> Contacts { get { return GetCollection<ClientContact>(nameof(Contacts)); } }
        [DevExpress.Xpo.DisplayName("AML Document")]
        public FileData AMLDocument { get => aMLDocument; set => SetPropertyValue(nameof(AMLDocument), ref aMLDocument, value); }
        [Association("Client-ApprovedInstrumentManufacturers"), Aggregated]
        public XPCollection<ApprovedInstrumentManufacturer> ApprovedInstrumentManufacturers { get { return GetCollection<ApprovedInstrumentManufacturer>(nameof(ApprovedInstrumentManufacturers)); } }

        [Association("Client-PipeSpecs")]
        [DataSourceCriteria("ClientPipeSpec.Oid = '@This.Oid'"), Aggregated]
        public XPCollection<Piping.ClientPipeSpec> PipeSpecs { get { return GetCollection<Piping.ClientPipeSpec>(nameof(PipeSpecs)); } }

    }

    public class ClientContact : Person
    {
        public ClientContact(Session session) : base(session) { }

        string notes;
        Client client;
        string title;


        [Association("Client-Contacts")]
        public Client Client { get => client; set => SetPropertyValue(nameof(Client), ref client, value); }
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Title { get => title; set => SetPropertyValue(nameof(Title), ref title, value); }

        [Size(SizeAttribute.Unlimited)]
        public string Notes { get => notes; set => SetPropertyValue(nameof(Notes), ref notes, value); }
    }
    public enum ProjectStatus
    {
        Bid = 0,
        Active = 1,
        Closed = 2,
        LostBid = 3

    }

    [RuleCombinationOfPropertiesIsUnique("UniqueApprovedInstrumentManufacturer", DefaultContexts.Save, "Client, Manufacturer, InstrumentType")]
    [DefaultListViewOptions(allowEdit: true, newItemRowPosition: NewItemRowPosition.Top)]
    public class ApprovedInstrumentManufacturer : BaseObject, IApprovedManufacturer
    {

        public ApprovedInstrumentManufacturer(Session session) : base(session) { }


        bool isPreferred;
        string comments;
        Instrument.InstrumentType instrumentType;
        Manufacturer manufacturer;
        Client client;

        [Association("Client-ApprovedInstrumentManufacturers")]
        public Client Client { get => client; set => SetPropertyValue(nameof(Client), ref client, value); }

        public Manufacturer Manufacturer { get => manufacturer; set => SetPropertyValue(nameof(Manufacturer), ref manufacturer, value); }

        public Instrument.InstrumentType InstrumentType { get => instrumentType; set => SetPropertyValue(nameof(InstrumentType), ref instrumentType, value); }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Comments { get => comments; set => SetPropertyValue(nameof(Comments), ref comments, value); }

        public bool IsPreferred { get => isPreferred; set => SetPropertyValue(nameof(IsPreferred), ref isPreferred, value); }

        //[Association("ApprovedInstrumentManufacturer-PreferredVendors")]
        //public XPCollection<Vendor> PreferredVendors { get { return GetCollection<Vendor>(nameof(PreferredVendors)); } }

    }
    public interface IApprovedManufacturer
    {
        Client Client { get; set; }
        Manufacturer Manufacturer { get; set; }
    }
}