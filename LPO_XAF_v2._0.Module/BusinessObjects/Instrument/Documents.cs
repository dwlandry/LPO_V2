//-----------------------------------------------------------------------
// <copyright file="F:\Users\dlandry\source\repos\LPO_XAF_v2.0\LPO_XAF_v2._0.Module\BusinessObjects\Instrument\Documents.cs" company="David W. Landry III">
//     Author: _**David Landry**_
//     *Copyright (c) David W. Landry III. All rights reserved.*
// </copyright>
//-----------------------------------------------------------------------
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using System.Linq;

namespace LPO_XAF_v2._0.Module.BusinessObjects.Instrument
{

    public class PlanDrawing : Drawing
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

    public class PID : Drawing
    {
        public PID(Session session) : base(session) { }

        Project.Project project;

        [Association("Project-PIDs")]
        public Project.Project Project { get => project; set => SetPropertyValue(nameof(Project), ref project, value); }
        [Association("PID-Instruments")]
        [DataSourceCriteria("Project.Oid = '@This.Project.Oid'")]
        public XPCollection<Instrument> Instruments { get { return GetCollection<Instrument>(nameof(Instruments)); } }
    }

    public class LoopDrawing : Drawing
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

    public class WiringDrawing : Drawing
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

    public class LayoutDrawing : Drawing
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
    [DefaultProperty("DrawingNumber")]
    [DefaultClassOptions, CreatableItem(false)]
    [DefaultListViewOptions(allowEdit: true, newItemRowPosition: NewItemRowPosition.Top)]
    public class Drawing : BaseObject
    {
        public Drawing(Session session) : base(session) { }

        string description;
        FileData file;
        string drawingNumber;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string DrawingNumber { get => drawingNumber; set => SetPropertyValue(nameof(DrawingNumber), ref drawingNumber, value); }
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Description { get => description; set => SetPropertyValue(nameof(Description), ref description, value); }

        public FileData File { get => file; set => SetPropertyValue(nameof(File), ref file, value); }

    }
}
