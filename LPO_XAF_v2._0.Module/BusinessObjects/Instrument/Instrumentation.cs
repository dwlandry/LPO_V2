﻿//-----------------------------------------------------------------------
// <copyright file="F:\Users\dlandry\source\repos\LPO_XAF_v2.0\LPO_XAF_v2._0.Module\BusinessObjects\Instrument\Instrumentation.cs" company="David W. Landry III">
//     Author: _**David Landry**_
//     *Copyright (c) David W. Landry III. All rights reserved.*
// </copyright>
//-----------------------------------------------------------------------
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Linq;

namespace LPO_XAF_v2._0.Module.BusinessObjects.Instrument
{
    [DefaultClassOptions]
    [CreatableItem(false), NavigationItem("Instrumentation")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewAndDetailView)]
    public class Instrument : BaseObject
    {
        public Instrument(Session session) : base(session) { }
        public override void AfterConstruction()
        {
            base.AfterConstruction();

        }


        byte[] notes;
        LoopDrawing loopDrawing;
        PID pid;
        PlanDrawing planDrawing;
        InstrumentIOType ioType;
        string location;
        string optionalSuffix;
        string number;
        string letters;
        string optionalPrefix;
        string modelNumber;
        Manufacturer manufacturer;
        InstrumentType instrumentType;
        bool groupD;
        bool groupC;
        bool groupB;
        bool groupA;
        AreaClass_Division div;
        AreaClass_Class @class;
        byte[] specSheet;
        string serviceDescription;
        string tagNumber;
        Project.Project project;

        [Association("Project-Instruments")]
        [RuleRequiredField("RuleRequiredField for Instrument.Project", DefaultContexts.Save, "A Project must be specified.")]
        public Project.Project Project { get => project; set => SetPropertyValue(nameof(Project), ref project, value); }
        [RuleRequiredField("RuleRequiredField for Instrument.TagNumber", DefaultContexts.Save, "A Tag Number must be specified.")]
        [Size(15), DetailViewLayout(LayoutColumnPosition.Left, "Tag Number")]
        public string TagNumber { get => tagNumber; set => SetPropertyValue(nameof(TagNumber), ref tagNumber, value); }
        [Size(5), DetailViewLayout(LayoutColumnPosition.Left, "Tag Number")]
        [DisplayName("Prefix")]
        public string OptionalPrefix { get => optionalPrefix; set => SetPropertyValue(nameof(OptionalPrefix), ref optionalPrefix, value); }
        [Size(10), DetailViewLayout(LayoutColumnPosition.Left, "Tag Number")]
        public string Letters { get => letters; set => SetPropertyValue(nameof(Letters), ref letters, value); }
        [Size(10), DetailViewLayout(LayoutColumnPosition.Left, "Tag Number")]
        public string Number { get => number; set => SetPropertyValue(nameof(Number), ref number, value); }
        [Size(5), DetailViewLayout(LayoutColumnPosition.Left, "Tag Number")]
        [DisplayName("Suffix")]
        public string OptionalSuffix { get => optionalSuffix; set => SetPropertyValue(nameof(OptionalSuffix), ref optionalSuffix, value); }
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string ServiceDescription { get => serviceDescription; set => SetPropertyValue(nameof(ServiceDescription), ref serviceDescription, value); }
        [EditorAlias(EditorAliases.SpreadsheetPropertyEditor)]
        [DetailViewLayoutAttribute(LayoutColumnPosition.Right)]
        public byte[] SpecSheet { get => specSheet; set => SetPropertyValue(nameof(SpecSheet), ref specSheet, value); }
        [DetailViewLayoutAttribute(LayoutColumnPosition.Left, "Area Classification")]
        public AreaClass_Class Class { get => @class; set => SetPropertyValue(nameof(Class), ref @class, value); }
        [DetailViewLayoutAttribute(LayoutColumnPosition.Left, "Area Classification")]
        public AreaClass_Division Div { get => div; set => SetPropertyValue(nameof(Div), ref div, value); }
        [DetailViewLayoutAttribute(LayoutColumnPosition.Left, "Area Classification")]
        public bool GroupA { get => groupA; set => SetPropertyValue(nameof(GroupA), ref groupA, value); }
        [DetailViewLayoutAttribute(LayoutColumnPosition.Left, "Area Classification")]
        public bool GroupB { get => groupB; set => SetPropertyValue(nameof(GroupB), ref groupB, value); }
        [DetailViewLayoutAttribute(LayoutColumnPosition.Left, "Area Classification")]
        public bool GroupC { get => groupC; set => SetPropertyValue(nameof(GroupC), ref groupC, value); }
        [DetailViewLayoutAttribute(LayoutColumnPosition.Left, "Area Classification")]
        public bool GroupD { get => groupD; set => SetPropertyValue(nameof(GroupD), ref groupD, value); }
        [DetailViewLayout(LayoutColumnPosition.Left)]
        public InstrumentType InstrumentType { get => instrumentType; set => SetPropertyValue(nameof(InstrumentType), ref instrumentType, value); }
        [DetailViewLayout(LayoutColumnPosition.Left)]
        public Manufacturer Manufacturer { get => manufacturer; set => SetPropertyValue(nameof(Manufacturer), ref manufacturer, value); }
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        [DetailViewLayout(LayoutColumnPosition.Left)]
        public string ModelNumber { get => modelNumber; set => SetPropertyValue(nameof(ModelNumber), ref modelNumber, value); }
        [Size(50), DetailViewLayout(LayoutColumnPosition.Left)]
        public string Location { get => location; set => SetPropertyValue(nameof(Location), ref location, value); }
        [DisplayName("IO Type"), DetailViewLayout(LayoutColumnPosition.Left)]
        public InstrumentIOType IoType { get => ioType; set => SetPropertyValue(nameof(IoType), ref ioType, value); }

        [EditorAlias(EditorAliases.RichTextPropertyEditor)]
        [Size(SizeAttribute.Unlimited)]
        [VisibleInListView(false), DetailViewLayout("Notes")]
        public byte[] Notes { get => notes; set => SetPropertyValue(nameof(Notes), ref notes, value); }

        [Association("PlanDrawing-Instruments"), DetailViewLayout("Drawings")]
        [DataSourceCriteria("Project.Oid = '@This.Project.Oid'")]
        public PlanDrawing PlanDrawing { get => planDrawing; set => SetPropertyValue(nameof(PlanDrawing), ref planDrawing, value); }

        [Association("PID-Instruments"), DetailViewLayout("Drawings")]
        [DataSourceCriteria("Project.Oid = '@This.Project.Oid'")]
        [DisplayName("P&ID")]
        public PID Pid { get => pid; set => SetPropertyValue(nameof(Pid), ref pid, value); }

        [Association("LoopDrawing-Instruments"), DetailViewLayout("Drawings")]
        public LoopDrawing LoopDrawing { get => loopDrawing; set => SetPropertyValue(nameof(LoopDrawing), ref loopDrawing, value); }

        [VisibleInDetailView(false), VisibleInListView(false)]
        [Association("Instruments-InstrumentSpecCheckPackages")]
        public XPCollection<InstrumentSpecCheckPackage> InstrumentSpecCheckPackages { get { return GetCollection<InstrumentSpecCheckPackage>(nameof(InstrumentSpecCheckPackages)); } }
    }

    public enum AreaClass_Class
    {
        ClassI = 1,
        ClassII = 2,
        ClassIII = 3
    }

    public enum AreaClass_Division
    {
        Div1 = 1,
        Div2 = 2
    }

    public class InstrumentType : BaseObject
    {
        public InstrumentType(Session session) : base(session) { }

        string name;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Name { get => name; set => SetPropertyValue(nameof(Name), ref name, value); }
    }

    public class Manufacturer : BaseObject
    {
        public Manufacturer(Session session) : base(session) { }
        string name;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Name { get => name; set => SetPropertyValue(nameof(Name), ref name, value); }
    }

    //public class InstrumentLocation : BaseObject{
    //    public InstrumentLocation(Session session) : base(session) { }

    //    string name;

    //    [Size(50)]
    //    public string Name { get => name; set => SetPropertyValue(nameof(Name), ref name, value); }
    //}

    public class InstrumentIOType : XPObject
    {

        public InstrumentIOType(Session session) : base(session) { }
        public InstrumentIOType(string name, string description)
        {
            this.name = name;
            this.description = description;
        }

        string description;
        string name;

        [Size(20)]
        [RuleUniqueValue("RuleUniqueField for InstrumentIOType.Name", DefaultContexts.Save, "The value entered already exists. Please enter a unique value.")]
        [RuleRequiredField("RuleRequiredField for InstrumentIOType.Name", DefaultContexts.Save, "A Name must be specified.")]
        public string Name { get => name; set => SetPropertyValue(nameof(Name), ref name, value); }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Description { get => description; set => SetPropertyValue(nameof(Description), ref description, value); }
    }

    [DefaultClassOptions, CreatableItem(false), NavigationItem("Instrumentation")]
    public class JunctionBox : BaseObject
    {
        public JunctionBox(Session session) : base(session) { }

        LayoutDrawing layoutDrawing;
        PlanDrawing planDrawing;
        Project.Project project;
        string name;


        [Association("Project-JunctionBoxes")]
        [RuleRequiredField("RuleRequiredField for JunctionBox.Project", DefaultContexts.Save, "A Project must be selected.")]
        public Project.Project Project { get => project; set => SetPropertyValue(nameof(Project), ref project, value); }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        [RuleRequiredField("RuleRequiredField for JunctionBox.Name", DefaultContexts.Save, "A Name must be specified.")]
        public string Name { get => name; set => SetPropertyValue(nameof(Name), ref name, value); }


        [Association("PlanDrawing-JunctionBoxes"), DetailViewLayout("Drawings")]
        [DataSourceCriteria("Project.Oid = '@This.Project.Oid'")]
        public PlanDrawing PlanDrawing { get => planDrawing; set => SetPropertyValue(nameof(PlanDrawing), ref planDrawing, value); }

        [Association("JunctionBox-WiringDiagrams"), DetailViewLayout("Drawings")]
        [DataSourceCriteria("Project.Oid = '@This.Project.Oid'")]
        public XPCollection<WiringDrawing> WiringDiagrams { get { return GetCollection<WiringDrawing>(nameof(WiringDiagrams)); } }

        // one-to-one relationship
        [DataSourceCriteria("Project.Oid = '@This.Project.Oid'")]
        public LayoutDrawing LayoutDrawing
        {
            get => layoutDrawing;
            set
            {
                if (layoutDrawing == value) return;
                LayoutDrawing prevLayoutDrawing = layoutDrawing;
                layoutDrawing = value;
                if (IsLoading) return;
                if (prevLayoutDrawing != null && prevLayoutDrawing.JunctionBox == this)
                    prevLayoutDrawing.JunctionBox = null;
                if (layoutDrawing != null)
                    layoutDrawing.JunctionBox = this;
                OnChanged("Address");
            }
        }
    }
}