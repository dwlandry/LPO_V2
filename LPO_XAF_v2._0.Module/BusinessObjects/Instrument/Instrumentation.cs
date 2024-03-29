﻿//-----------------------------------------------------------------------
// <copyright file="D:\Users\dlandry\Source\Repos\LPO_V2\LPO_XAF_v2._0.Module\BusinessObjects\Instrument\Instrumentation.cs" company="David W. Landry III">
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
using LPO_XAF_v2._0.Module.BusinessObjects.Piping;
using LPO_XAF_v2._0.Module.BusinessObjects.Procurement;
using LPO_XAF_v2._0.Module.BusinessObjects.Products;
using System;
using System.Linq;

namespace LPO_XAF_v2._0.Module.BusinessObjects.Instrument
{
    [DefaultClassOptions]
    [CreatableItem(false), NavigationItem("Instrumentation")]
    [ImageName("Instrument")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewAndDetailView)]
    public class Instrument : _MyBaseObject.BaseObjectWithCreatedAndLastModified
    {
        public Instrument(Session session) : base(session) { }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            //createdBy = GetCurrentUser();
            //createdOn = DateTime.Now;
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            //lastModifiedBy = GetCurrentUser();
            //lastModifiedOn = DateTime.Now;
        }

        //[Persistent("CreatedBy")]
        //PermissionPolicyUser createdBy;
        //[Persistent("CreatedOn")]
        //DateTime createdOn;
        //[Persistent("LastModifiedBy")]
        //PermissionPolicyUser lastModifiedBy;
        //[Persistent("LastModifiedOn")]
        //DateTime lastModifiedOn;
        string comments;
        InstrumentStatus status;
        byte[] imageAsByteArrayDelayedProperty;
        byte[] imageAsByteArray;
        Line lineNumber;
        string specSheetNumber;
        private XPCollection<AuditDataItemPersistent> auditTrail;
        bool requiresSpecSheet;
        AreaClassificationDrawing areaClassificationDrawing;
        TracingDetail tracingDetail;
        TubingDetail tubingDetail;
        MountingDetail mountingDetail;
        ElectricalDetail electricalDetail;
        ProcessConnectionDetail processConnectionDetail;
        ResponsibleEngineeringCompany responsibleCompany;
        ControlSystem controlSystem;
        string equipmentNumber;
        Physical_Instrument physicalOrSoftTag;
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

        //[PersistentAlias("createdBy")]
        //public PermissionPolicyUser CreatedBy { get => createdBy; protected set => SetPropertyValue(nameof(CreatedBy), ref createdBy, value); }
        //[PersistentAlias("createdOn")]
        //public DateTime CreatedOn { get => createdOn; protected set => SetPropertyValue(nameof(CreatedOn), ref createdOn, value); }
        //[PersistentAlias("lastModifiedBy")]
        //public PermissionPolicyUser LastModifiedBy { get => lastModifiedBy; protected set => SetPropertyValue(nameof(LastModifiedBy), ref lastModifiedBy, value); }
        //[PersistentAlias("lastModifiedOn")]
        //public DateTime LastModifiedOn { get => lastModifiedOn; protected set => SetPropertyValue(nameof(LastModifiedOn), ref lastModifiedOn, value); }


        [VisibleInListView(true)]
        [ImageEditor(ListViewImageEditorMode = ImageEditorMode.PictureEdit,
            DetailViewImageEditorMode = ImageEditorMode.PictureEdit,
            ListViewImageEditorCustomHeight = 40)]
        public byte[] ImageAsByteArray
        {
            get => imageAsByteArray;
            set => SetPropertyValue(nameof(ImageAsByteArray), ref imageAsByteArray, value);
        }

        [Delayed(), VisibleInListView(true)]
        [ImageEditor(ListViewImageEditorMode = ImageEditorMode.PopupPictureEdit, DetailViewImageEditorMode = ImageEditorMode.DropDownPictureEdit)]
        public byte[] ImageAsByteArrayDelayedProperty
        {
            get => imageAsByteArrayDelayedProperty;
            set => SetPropertyValue(nameof(ImageAsByteArrayDelayedProperty), ref imageAsByteArrayDelayedProperty, value);
        }
        [Association("Project-Instruments"), VisibleInListView(false)]
        [RuleRequiredField("RuleRequiredField for Instrument.Project", DefaultContexts.Save, "A Project must be specified.")]
        public Project.Project Project { get => project; set => SetPropertyValue(nameof(Project), ref project, value); }
        [RuleRequiredField("RuleRequiredField for Instrument.TagNumber", DefaultContexts.Save, "A Tag Number must be specified.")]
        [Size(60), DetailViewLayout(LayoutColumnPosition.Left, "Tag Number")]
        public string TagNumber { get => tagNumber; set => SetPropertyValue(nameof(TagNumber), ref tagNumber, value); }
        [Size(15), DetailViewLayout(LayoutColumnPosition.Left, "Tag Number")]
        [DisplayName("Prefix")]
        public string OptionalPrefix { get => optionalPrefix; set => SetPropertyValue(nameof(OptionalPrefix), ref optionalPrefix, value); }
        [Size(15), DetailViewLayout(LayoutColumnPosition.Left, "Tag Number")]
        public string Letters { get => letters; set => SetPropertyValue(nameof(Letters), ref letters, value); }
        [Size(15), DetailViewLayout(LayoutColumnPosition.Left, "Tag Number")]
        public string Number { get => number; set => SetPropertyValue(nameof(Number), ref number, value); }
        [Size(15), DetailViewLayout(LayoutColumnPosition.Left, "Tag Number")]
        [DisplayName("Suffix")]
        public string OptionalSuffix { get => optionalSuffix; set => SetPropertyValue(nameof(OptionalSuffix), ref optionalSuffix, value); }
        [DisplayName("Physical/Soft Tag")]
        public Physical_Instrument PhysicalOrSoftTag { get => physicalOrSoftTag; set => SetPropertyValue(nameof(PhysicalOrSoftTag), ref physicalOrSoftTag, value); }
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string ServiceDescription { get => serviceDescription; set => SetPropertyValue(nameof(ServiceDescription), ref serviceDescription, value); }

        public InstrumentStatus Status { get => status; set => SetPropertyValue(nameof(Status), ref status, value); }

        [Association("Line-Instruments")]
        [DataSourceCriteria("Project.Oid = '@This.Project.Oid'")]
        [VisibleInListView(false)]
        public Line LineNumber { get => lineNumber; set => SetPropertyValue(nameof(LineNumber), ref lineNumber, value); }

        [VisibleInListView(false)]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string EquipmentNumber { get => equipmentNumber; set => SetPropertyValue(nameof(EquipmentNumber), ref equipmentNumber, value); }


        [Association("ControlSystem-Instruments"), VisibleInListView(false)]
        [DataSourceCriteria("Project.Oid = '@This.Project.Oid'")]
        public ControlSystem ControlSystem { get => controlSystem; set => SetPropertyValue(nameof(ControlSystem), ref controlSystem, value); }

        [EditorAlias(EditorAliases.SpreadsheetPropertyEditor)]
        [DetailViewLayoutAttribute(LayoutColumnPosition.Right), VisibleInListView(false)]
        public byte[] SpecSheet { get => specSheet; set => SetPropertyValue(nameof(SpecSheet), ref specSheet, value); }
        [DetailViewLayoutAttribute(LayoutColumnPosition.Left, "Area Classification"), VisibleInListView(false)]
        public AreaClass_Class Class { get => @class; set => SetPropertyValue(nameof(Class), ref @class, value); }
        [DetailViewLayoutAttribute(LayoutColumnPosition.Left, "Area Classification"), VisibleInListView(false)]
        public AreaClass_Division Div { get => div; set => SetPropertyValue(nameof(Div), ref div, value); }
        [DetailViewLayoutAttribute(LayoutColumnPosition.Left, "Area Classification"), VisibleInListView(false)]
        public bool GroupA { get => groupA; set => SetPropertyValue(nameof(GroupA), ref groupA, value); }
        [DetailViewLayoutAttribute(LayoutColumnPosition.Left, "Area Classification"), VisibleInListView(false)]
        public bool GroupB { get => groupB; set => SetPropertyValue(nameof(GroupB), ref groupB, value); }
        [DetailViewLayoutAttribute(LayoutColumnPosition.Left, "Area Classification"), VisibleInListView(false)]
        public bool GroupC { get => groupC; set => SetPropertyValue(nameof(GroupC), ref groupC, value); }
        [DetailViewLayoutAttribute(LayoutColumnPosition.Left, "Area Classification"), VisibleInListView(false)]
        public bool GroupD { get => groupD; set => SetPropertyValue(nameof(GroupD), ref groupD, value); }
        [DetailViewLayout(LayoutColumnPosition.Left)]
        public InstrumentType InstrumentType { get => instrumentType; set => SetPropertyValue(nameof(InstrumentType), ref instrumentType, value); }
        [DetailViewLayout(LayoutColumnPosition.Left)]
        public Manufacturer Manufacturer { get => manufacturer; set => SetPropertyValue(nameof(Manufacturer), ref manufacturer, value); }
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        [DetailViewLayout(LayoutColumnPosition.Left)]
        public string ModelNumber { get => modelNumber; set => SetPropertyValue(nameof(ModelNumber), ref modelNumber, value); }
        [Size(50), DetailViewLayout(LayoutColumnPosition.Left), VisibleInListView(false)]
        public string Location { get => location; set => SetPropertyValue(nameof(Location), ref location, value); }
        [DisplayName("IO Type"), DetailViewLayout(LayoutColumnPosition.Left), VisibleInListView(false)]
        public InstrumentIOType IoType { get => ioType; set => SetPropertyValue(nameof(IoType), ref ioType, value); }
        [DataSourceCriteria("Project.Oid = '@This.Project.Oid'"), VisibleInListView(false)]
        public ResponsibleEngineeringCompany ResponsibleCompany { get => responsibleCompany; set => SetPropertyValue(nameof(ResponsibleCompany), ref responsibleCompany, value); }
        [Size(300)]
        public string Comments { get => comments; set => SetPropertyValue(nameof(Comments), ref comments, value); }
        [VisibleInListView(false)]
        [CaptionsForBoolValues("YES", "NO")]
        [Nullable(true)]
        public bool RequiresSpecSheet { get => requiresSpecSheet; set => SetPropertyValue(nameof(RequiresSpecSheet), ref requiresSpecSheet, value); }
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string SpecSheetNumber { get => specSheetNumber; set => SetPropertyValue(nameof(SpecSheetNumber), ref specSheetNumber, value); }
        public XPCollection<AuditDataItemPersistent> AuditTrail
        {
            get
            {
                if (auditTrail == null)
                    auditTrail = AuditedObjectWeakReference.GetAuditTrail(Session, this);
                return auditTrail;
            }
        }
        [EditorAlias(EditorAliases.RichTextPropertyEditor)]
        [Size(SizeAttribute.Unlimited)]
        [VisibleInListView(false), DetailViewLayout("Notes")]
        public byte[] Notes { get => notes; set => SetPropertyValue(nameof(Notes), ref notes, value); }

        [Association("PlanDrawing-Instruments"), DetailViewLayout("Drawings"), VisibleInListView(false)]
        [DataSourceCriteria("Project.Oid = '@This.Project.Oid'")]
        public PlanDrawing PlanDrawing { get => planDrawing; set => SetPropertyValue(nameof(PlanDrawing), ref planDrawing, value); }

        [Association("PID-Instruments"), DetailViewLayout("Drawings"), VisibleInListView(false)]
        [DataSourceCriteria("Project.Oid = '@This.Project.Oid'")]
        [DisplayName("P&ID")]
        public PID Pid { get => pid; set => SetPropertyValue(nameof(Pid), ref pid, value); }

        [Association("LoopDrawing-Instruments"), DetailViewLayout("Drawings"), VisibleInListView(false)]
        [DataSourceCriteria("Project.Oid = '@This.Project.Oid'")]
        public LoopDrawing LoopDrawing { get => loopDrawing; set => SetPropertyValue(nameof(LoopDrawing), ref loopDrawing, value); }


        [Association("AreaClassificationDrawing-Instruments"), DetailViewLayout("Drawings"), VisibleInListView(false)]
        [DataSourceCriteria("Project.Oid = '@This.Project.Oid'")]
        public AreaClassificationDrawing AreaClassificationDrawing { get => areaClassificationDrawing; set => SetPropertyValue(nameof(AreaClassificationDrawing), ref areaClassificationDrawing, value); }

        [Association("ProcessConnectionDetail-Instruments"), DetailViewLayout("Installation Details"), VisibleInListView(false)]
        [DataSourceCriteria("Project.Oid = '@This.Project.Oid'")]
        public ProcessConnectionDetail ProcessConnectionDetail { get => processConnectionDetail; set => SetPropertyValue(nameof(ProcessConnectionDetail), ref processConnectionDetail, value); }

        [Association("ElectricalDetail-Instruments"), DetailViewLayout("Installation Details"), VisibleInListView(false)]
        [DataSourceCriteria("Project.Oid = '@This.Project.Oid'")]
        public ElectricalDetail ElectricalDetail { get => electricalDetail; set => SetPropertyValue(nameof(ElectricalDetail), ref electricalDetail, value); }

        [Association("MountingDetail-Instruments"), DetailViewLayout("Installation Details"), VisibleInListView(false)]
        [DataSourceCriteria("Project.Oid = '@This.Project.Oid'")]
        public MountingDetail MountingDetail { get => mountingDetail; set => SetPropertyValue(nameof(MountingDetail), ref mountingDetail, value); }

        [Association("TubingDetail-Instruments"), DetailViewLayout("Installation Details"), VisibleInListView(false)]
        [DataSourceCriteria("Project.Oid = '@This.Project.Oid'")]
        public TubingDetail TubingDetail { get => tubingDetail; set => SetPropertyValue(nameof(TubingDetail), ref tubingDetail, value); }

        [Association("TracingDetail-Instruments"), DetailViewLayout("Installation Details"), VisibleInListView(false)]
        [DataSourceCriteria("Project.Oid = '@This.Project.Oid'")]
        public TracingDetail TracingDetail { get => tracingDetail; set => SetPropertyValue(nameof(TracingDetail), ref tracingDetail, value); }

        [Association("Instrument-VendorDocs"), DetailViewLayout("Drawings"), Aggregated]
        public XPCollection<VendorDocument> VendorDocs { get { return GetCollection<VendorDocument>(nameof(VendorDocs)); } }

        [VisibleInDetailView(false), VisibleInListView(false)]
        [Association("Instruments-InstrumentSpecCheckPackages")]
        public XPCollection<InstrumentSpecCheckPackage> InstrumentSpecCheckPackages { get { return GetCollection<InstrumentSpecCheckPackage>(nameof(InstrumentSpecCheckPackages)); } }

        [Association("Instruments-Quotes")]
        [DataSourceCriteria("Project.Oid = '@This.Project.Oid'")]
        public XPCollection<InstrumentQuote> Quotes { get { return GetCollection<InstrumentQuote>(nameof(Quotes)); } }

        //PermissionPolicyUser GetCurrentUser() => Session.GetObjectByKey<PermissionPolicyUser>(SecuritySystem.CurrentUserId);
    }

    public enum AreaClass_Class
    {
        ClassI = 1,
        ClassII = 2,
        ClassIII = 3,
        Unclassified = 4

    }

    public enum AreaClass_Division
    {
        Div1 = 1,
        Div2 = 2
    }

    public enum Physical_Instrument
    {
        PhysicalInstrument = 0,
        SoftTag = 1
    }

    [DefaultClassOptions, CreatableItem(false), NavigationItem("Instrumentation")]
    public class InstrumentType : BaseObject
    {
        public InstrumentType(Session session) : base(session) { }

        InstrumentDeviceCategory deviceCategory;
        InstrumentMeasurementCategory measurementCategory;
        byte[] notes;
        string name;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Name { get => name; set => SetPropertyValue(nameof(Name), ref name, value); }

        public InstrumentMeasurementCategory MeasurementCategory { get => measurementCategory; set => SetPropertyValue(nameof(MeasurementCategory), ref measurementCategory, value); }
        public InstrumentDeviceCategory DeviceCategory { get => deviceCategory; set => SetPropertyValue(nameof(DeviceCategory), ref deviceCategory, value); }

        [Size(SizeAttribute.Unlimited)]
        [EditorAlias(EditorAliases.RichTextPropertyEditor)]
        public byte[] Notes { get => notes; set => SetPropertyValue(nameof(Notes), ref notes, value); }
    }

    public class InstrumentStatus : BaseObject
    {
        public InstrumentStatus(Session session) : base(session) { }

        string name;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Name { get => name; set => SetPropertyValue(nameof(Name), ref name, value); }
    }

    public class InstrumentMeasurementCategory : BaseObject
    {

        public InstrumentMeasurementCategory(Session session) : base(session) { }

        string name;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        [RuleUniqueValue]
        public string Name { get => name; set => SetPropertyValue(nameof(Name), ref name, value); }

    }

    public class InstrumentDeviceCategory : BaseObject
    {

        public InstrumentDeviceCategory(Session session) : base(session) { }

        string name;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        [RuleUniqueValue]
        public string Name { get => name; set => SetPropertyValue(nameof(Name), ref name, value); }
    }

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

    //[DefaultClassOptions, CreatableItem(false), NavigationItem("Instrumentation")]
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

    public class ControlSystem : BaseObject
    {
        public ControlSystem(Session session) : base(session) { }


        Project.Project project;
        string description;
        string name;


        [Association("Project-ControlSystems")]
        public Project.Project Project { get => project; set => SetPropertyValue(nameof(Project), ref project, value); }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Name { get => name; set => SetPropertyValue(nameof(Name), ref name, value); }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Description { get => description; set => SetPropertyValue(nameof(Description), ref description, value); }

        [Association("ControlSystem-Instruments")]
        [DataSourceCriteria("Project.Oid = '@This.Project.Oid'")]
        public XPCollection<Instrument> Instruments { get { return GetCollection<Instrument>(nameof(Instruments)); } }
    }

    public class VendorDocument : BaseObject
    {
        public VendorDocument(Session session) : base(session) { }


        Instrument instrument;
        FileData file;
        string description;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Description { get => description; set => SetPropertyValue(nameof(Description), ref description, value); }


        public FileData File { get => file; set => SetPropertyValue(nameof(File), ref file, value); }

        [Association("Instrument-VendorDocs")]
        public Instrument Instrument { get => instrument; set => SetPropertyValue(nameof(Instrument), ref instrument, value); }

    }

    [DefaultClassOptions, NavigationItem("Instrumentation"), ImageName("InstrumentQuotes")]
    public class InstrumentQuote : Quote, IProjectQuote
    {

        public InstrumentQuote(Session session) : base(session) { }

        Project.Project project;

        [Association("Project-InstrumentQuotes")]
        public Project.Project Project { get => project; set => SetPropertyValue(nameof(Project), ref project, value); }
        [Association("Instruments-Quotes")]
        [DataSourceCriteria("Project.Oid = '@This.Project.Oid'")]
        public XPCollection<Instrument> Instruments { get { return GetCollection<Instrument>(nameof(Instruments)); } }
    }

    public class ResponsibleEngineeringCompany : BaseObject
    {

        public ResponsibleEngineeringCompany(Session session) : base(session) { }


        Project.Project project;
        string name;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Name { get => name; set => SetPropertyValue(nameof(Name), ref name, value); }

        public Project.Project Project { get => project; set => SetPropertyValue(nameof(Project), ref project, value); }

    }
}