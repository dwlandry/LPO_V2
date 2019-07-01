//-----------------------------------------------------------------------
// <copyright file="F:\Users\dlandry\source\repos\LPO_XAF_v2.0\LPO_XAF_v2._0.Module\BusinessObjects\Instrument\Documents.cs" company="David W. Landry III">
//     Author: _**David Landry**_
//     *Copyright (c) David W. Landry III. All rights reserved.*
// </copyright>
//-----------------------------------------------------------------------
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace LPO_XAF_v2._0.Module.BusinessObjects.Instrument
{

    public class PlanDrawing : Drawing, IProjectDrawing
    {
        public PlanDrawing(Session session) : base(session) { }

        Project.Project project;

        [Association("Project-PlanDrawings")]
        public Project.Project Project { get => project; set => SetPropertyValue(nameof(Project), ref project, value); }
        [Association("PlanDrawing-Instruments")]
        [DataSourceCriteria("Project.Oid = '@This.Project.Oid'")]
        public XPCollection<Instrument> Instruments { get { return GetCollection<Instrument>(nameof(Instruments)); } }

        [Association("PlanDrawing-JunctionBoxes")]
        [DataSourceCriteria("Project.Oid = '@This.Project.Oid'")]
        public XPCollection<JunctionBox> JunctionBoxes { get { return GetCollection<JunctionBox>(nameof(JunctionBoxes)); } }
    }

    public class PID : Drawing, IProjectDrawing
    {
        public PID(Session session) : base(session) { }

        Project.Project project;

        [Association("Project-PIDs")]
        public Project.Project Project { get { return project; } set { SetPropertyValue(nameof(Project), ref project, value); } }
        [Association("PID-Instruments")]
        [DataSourceCriteria("Project.Oid = '@This.Project.Oid'")]
        public XPCollection<Instrument> Instruments { get { return GetCollection<Instrument>(nameof(Instruments)); } }
    }

    public class LoopDrawing : Drawing, IProjectDrawing
    {
        public LoopDrawing(Session session) : base(session) { }

        string loopNumber;
        Project.Project project;

        [Association("Project-LoopDrawings")]
        public Project.Project Project { get => project; set => SetPropertyValue(nameof(Project), ref project, value); }
        [Association("LoopDrawing-Instruments")]
        [DataSourceCriteria("Project.Oid = '@This.Project.Oid'")]
        public XPCollection<Instrument> Instruments => GetCollection<Instrument>(nameof(Instruments));

        [Size(50)]
        public string LoopNumber { get => loopNumber; set => SetPropertyValue(nameof(LoopNumber), ref loopNumber, value); }
    }

    public class WiringDrawing : Drawing, IProjectDrawing
    {
        public WiringDrawing(Session session) : base(session) { }


        JunctionBox junctionBox;
        Project.Project project;

        [Association("Project-WiringDrawings")]
        public Project.Project Project { get => project; set => SetPropertyValue(nameof(Project), ref project, value); }

        [Association("JunctionBox-WiringDiagrams")]
        [DataSourceCriteria("Project.Oid = '@This.Project.Oid'")]
        public JunctionBox JunctionBox { get => junctionBox; set => SetPropertyValue(nameof(JunctionBox), ref junctionBox, value); }
    }

    public class LayoutDrawing : Drawing, IProjectDrawing
    {
        public LayoutDrawing(Session session) : base(session) { }


        JunctionBox junctionBox;
        Project.Project project;

        [Association("Project-LayoutDrawings")]
        public Project.Project Project { get => project; set => SetPropertyValue(nameof(Project), ref project, value); }


        // one-to-one relationship
        [DataSourceCriteria("Project.Oid = '@This.Project.Oid'")]
        public JunctionBox JunctionBox
        {
            get => junctionBox;
            set
            {
                if (junctionBox == value)
                    return;
                JunctionBox prevJunctionBox = junctionBox;
                junctionBox = value;
                if (IsLoading) return;
                if (prevJunctionBox != null && prevJunctionBox.LayoutDrawing == this)
                    prevJunctionBox.LayoutDrawing = null;
                if (junctionBox != null)
                    junctionBox.LayoutDrawing = this;
                OnChanged("Contact");

                SetPropertyValue(nameof(JunctionBox), ref junctionBox, value);
            }
        }

    }

    public class AreaClassificationDrawing : Drawing, IProjectDrawing
    {

        public AreaClassificationDrawing(Session session) : base(session) { }

        Project.Project project;

        [Association("Project-AreaClassificationDrawings")]
        public Project.Project Project { get => project; set => SetPropertyValue(nameof(Project), ref project, value); }

        [DataSourceCriteria("Project.Oid = '@This.Project.Oid'")]
        [Association("AreaClassificationDrawing-Instruments")]
        public XPCollection<Instrument> Instruments { get { return GetCollection<Instrument>(nameof(Instruments)); } }
    }

    public class ProjectInstallationDetail : Drawing, IProjectDrawing
    {

        public ProjectInstallationDetail(Session session) : base(session) { }

        Project.Project project;

        [Association("Project-InstallationDetails")]
        public Project.Project Project { get => project; set => SetPropertyValue(nameof(Project), ref project, value); }

    }
    public class ProcessConnectionDetail : ProjectInstallationDetail
    {
        public ProcessConnectionDetail(Session session) : base(session) { }

        [Association("ProcessConnectionDetail-Instruments")]
        [DataSourceCriteria("Project.Oid = '@This.Project.Oid'")]
        public XPCollection<Instrument> Instruments => GetCollection<Instrument>(nameof(Instruments));
    }
    public class ElectricalDetail : ProjectInstallationDetail
    {
        public ElectricalDetail(Session session) : base(session) { }

        [Association("ElectricalDetail-Instruments")]
        [DataSourceCriteria("Project.Oid = '@This.Project.Oid'")]
        public XPCollection<Instrument> Instruments => GetCollection<Instrument>(nameof(Instruments));
    }
    public class MountingDetail : ProjectInstallationDetail
    {
        public MountingDetail(Session session) : base(session) { }

        [Association("MountingDetail-Instruments")]
        [DataSourceCriteria("Project.Oid = '@This.Project.Oid'")]
        public XPCollection<Instrument> Instruments => GetCollection<Instrument>(nameof(Instruments));
    }
    public class TubingDetail : ProjectInstallationDetail
    {
        public TubingDetail(Session session) : base(session) { }

        [Association("TubingDetail-Instruments")]
        [DataSourceCriteria("Project.Oid = '@This.Project.Oid'")]
        public XPCollection<Instrument> Instruments => GetCollection<Instrument>(nameof(Instruments));
    }
    public class TracingDetail : ProjectInstallationDetail
    {
        public TracingDetail(Session session) : base(session) { }

        [Association("TracingDetail-Instruments")]
        [DataSourceCriteria("Project.Oid = '@This.Project.Oid'")]
        public XPCollection<Instrument> Instruments => GetCollection<Instrument>(nameof(Instruments));
    }
    public interface IProjectDrawing
    {
        Project.Project Project { get; set; }
    }

    [DefaultProperty("DrawingNumber")]
    [DefaultClassOptions, CreatableItem(false)]
    //[DefaultListViewOptions(allowEdit: true, newItemRowPosition: NewItemRowPosition.Top)]
    public class Drawing : BaseObject
    {
        public Drawing(Session session) : base(session) { }

        string description;
        FileData file;
        string drawingNumber;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string DrawingNumber { get => drawingNumber; set => SetPropertyValue(nameof(DrawingNumber), ref drawingNumber, value); }
        [Size(200)]
        public string Description { get => description; set => SetPropertyValue(nameof(Description), ref description, value); }

        public FileData File
        {
            get => file;
            set
            {
                if (DrawingNumber == null || DrawingNumber.Length == 0)
                    DrawingNumber = Path.GetFileNameWithoutExtension(value.FileName);
                SetPropertyValue(nameof(File), ref file, value);
            }
        }

    }
}
