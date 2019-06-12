//-----------------------------------------------------------------------
// <copyright file="F:\Users\dlandry\source\repos\LPO_XAF_v2.0\LPO_XAF_v2._0.Module\BusinessObjects\Project\Project.cs" company="David W. Landry III">
//     Author: _**David Landry**_
//     *Copyright (c) David W. Landry III. All rights reserved.*
// </copyright>
//-----------------------------------------------------------------------
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;

namespace LPO_XAF_v2._0.Module.BusinessObjects.Project
{
    [DefaultClassOptions, NavigationItem("Projects")]
    [ImageName("BO_Project"), CreatableItem(false), DefaultProperty("DisplayName")]
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

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        [EditorAlias("HyperLinkStringPropertyEditor")]
        public string ProjectFolder
        {
            get => projectFolder;
            set => SetPropertyValue(nameof(ProjectFolder), ref projectFolder, value);
        }
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string ClientProjectNumber { get => clientProjectNumber; set => SetPropertyValue(nameof(ClientProjectNumber), ref clientProjectNumber, value); }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string ProjectDescription { get => projectDescription; set => SetPropertyValue(nameof(ProjectDescription), ref projectDescription, value); }

        [Association("Client-Projects")]
        [RuleRequiredField("RuleRequiredField for Project.Client", DefaultContexts.Save, "A Client must be specified.")]
        public Client Client { get => client; set => SetPropertyValue(nameof(Client), ref client, value); }

        public ProjectStatus Status { get => status; set => SetPropertyValue(nameof(Status), ref status, value); }

        [Size(SizeAttribute.Unlimited)]
        [EditorAlias(EditorAliases.RichTextPropertyEditor)]
        public byte[] ProjectScope { get => scope; set => SetPropertyValue(nameof(ProjectScope), ref scope, value); }

        [Association("Project-Instruments"), DevExpress.Xpo.Aggregated]
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

    }

    [DefaultClassOptions, NavigationItem("Projects")]
    [ImageName("BO_Customer"), CreatableItem(false)]
    public class Client : BaseObject
    {
        public Client(Session session) : base(session) { }
        public override void AfterConstruction() => base.AfterConstruction();

        byte[] sitePPERequirements;
        string name;

        [Size(50)]
        public string Name { get => name; set => SetPropertyValue(nameof(Name), ref name, value); }
        [Association("Client-Projects")]
        public XPCollection<Project> Projects => GetCollection<Project>(nameof(Projects));

        [DevExpress.Xpo.DisplayName("Site PPE Requirements")]
        [EditorAlias(EditorAliases.RichTextPropertyEditor)]
        public byte[] SitePPERequirements { get => sitePPERequirements; set => SetPropertyValue(nameof(SitePPERequirements), ref sitePPERequirements, value); }

    }

    public enum ProjectStatus
    {
        Bid = 0,
        Active = 1,
        Closed = 2,
        LostBid = 3

    }

}