﻿//-----------------------------------------------------------------------
// <copyright file="F:\Users\dlandry\source\repos\LPO_XAF_v2.0\LPO_XAF_v2._0.Module\BusinessObjects\Project\Project.cs" company="David W. Landry III">
//     Author: _**David Landry**_
//     *Copyright (c) David W. Landry III. All rights reserved.*
// </copyright>
//-----------------------------------------------------------------------
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using System.Linq;

namespace LPO_XAF_v2._0.Module.BusinessObjects.Project
{
    [DefaultClassOptions, NavigationItem("Projects")]
    [ImageName("BO_Handshake"), CreatableItem(false), DefaultProperty("DisplayName")]
    [DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class Project : BaseObject
    {
        public Project(Session session) : base(session) { }
        public override void AfterConstruction() => base.AfterConstruction();

        string clientProjectNumber;
        byte[] scope;
        ProjectStatus status;
        Client client;
        string projectDescription;
        string projectNumber;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string ProjectNumber { get => projectNumber; set => SetPropertyValue(nameof(ProjectNumber), ref projectNumber, value); }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string ClientProjectNumber { get => clientProjectNumber; set => SetPropertyValue(nameof(ClientProjectNumber), ref clientProjectNumber, value); }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string ProjectDescription { get => projectDescription; set => SetPropertyValue(nameof(ProjectDescription), ref projectDescription, value); }

        [Association("Client-Projects")]
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
    }

    [DefaultClassOptions, NavigationItem("Projects")]
    [ImageName("BO_Customer"), CreatableItem(false)]
    public class Client : BaseObject
    {
        public Client(Session session) : base(session) { }
        public override void AfterConstruction() => base.AfterConstruction();

        string name;

        [Size(50)]
        public string Name { get => name; set => SetPropertyValue(nameof(Name), ref name, value); }
        [Association("Client-Projects")]
        public XPCollection<Project> Projects => GetCollection<Project>(nameof(Projects));
    }

    public enum ProjectStatus
    {
        Bid = 0,
        Open = 1,
        Closed = 2
    }

}