﻿//-----------------------------------------------------------------------
// <copyright file="D:\Users\dlandry\Source\Repos\LPO_V2\LPO_XAF_v2._0.Module\BusinessObjects\Instrument\Documents.cs" company="David W. Landry III">
//     Author: _**David Landry**_
//     *Copyright (c) David W. Landry III. All rights reserved.*
// </copyright>
//-----------------------------------------------------------------------
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using LPO_XAF_v2._0.Module.BusinessObjects.Mechanical;
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
        [Association("PIDs-Lines")]
        [DataSourceCriteria("Project.Oid = '@This.Project.Oid'")]
        public XPCollection<Piping.Line> Lines { get { return GetCollection<Piping.Line>(nameof(Lines)); } }
        [Association("PID-TiePoints")]
        [DataSourceCriteria("Project.Oid = '@This.Project.Oid'")]
        public XPCollection<Piping.PipingTiePoint> TiePoints { get { return GetCollection<Piping.PipingTiePoint>(nameof(TiePoints)); } }
        [Association("PID-Equipment")]
        [DataSourceCriteria("Project.Oid = '@This.Oid'")]
        public XPCollection<Equipment> Equipment { get { return GetCollection<Equipment>(nameof(Equipment)); } }
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

    public interface IDrawing
    {
        string Description { get; set; }
        string DrawingNumber { get; set; }
        FileData File { get; set; }
    }

    public interface IDrawingVersion
    {
        Drawing Drawing { get; set; }
        string VersionDescription { get; set; }
        FileData File { get; set; }
        DateTime DateAdded { get; set; }
        string RevisionNumber { get; set; }
        string RevisionDescription { get; set; }
        bool IsMostCurrent { get; set; }

    }
}
