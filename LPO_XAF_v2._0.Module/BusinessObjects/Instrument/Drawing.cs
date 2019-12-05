//-----------------------------------------------------------------------
// <copyright file="D:\Users\dlandry\Source\Repos\LPO_V2\LPO_XAF_v2._0.Module\BusinessObjects\Instrument\Drawing.cs" company="David W. Landry III">
//     Author: _**David Landry**_
//     *Copyright (c) David W. Landry III. All rights reserved.*
// </copyright>
//-----------------------------------------------------------------------
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace LPO_XAF_v2._0.Module.BusinessObjects.Instrument
{
    [DefaultProperty("DrawingNumber")]
    [DefaultClassOptions, CreatableItem(false)]
    //[DefaultListViewOptions(allowEdit: true, newItemRowPosition: NewItemRowPosition.Top)]
    public class Drawing : BaseObject, IDrawing
    {
        public Drawing(Session session) : base(session) { }

        string description;
        FileData file;
        string drawingNumber;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string DrawingNumber { get => drawingNumber; set => SetPropertyValue(nameof(DrawingNumber), ref drawingNumber, value); }
        [Size(200)]
        public string Description { get => description; set => SetPropertyValue(nameof(Description), ref description, value); }

        // TODO: Remove this FileData object and relocate to Drawing Version
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

        [Association("Drawing-Versions")]
        public XPCollection<DrawingVersion> Versions => GetCollection<DrawingVersion>(nameof(Versions));

    }

    [DefaultProperty("VersionDescription")]
    [DefaultClassOptions, CreatableItem(false)]
    [DefaultListViewOptions(true, NewItemRowPosition.Top)]
    public class DrawingVersion : _MyBaseObject.BaseObjectWithCreatedAndLastModified, IDrawingVersion
    {
        public DrawingVersion(Session session) : base(session) { }


        bool isMostCurrent;
        string revisionDescription;
        string revisionNumber;
        DateTime dateAdded;
        FileData file;
        string versionDescription;
        Drawing drawing;

        [Association("Drawing-Versions")]
        public Drawing Drawing { get => drawing; set => SetPropertyValue(nameof(Drawing), ref drawing, value); }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string VersionDescription { get => versionDescription; set => SetPropertyValue(nameof(VersionDescription), ref versionDescription, value); }

        [DevExpress.Xpo.Aggregated, ExpandObjectMembers(ExpandObjectMembers.Never)]
        public FileData File { get => file; set => SetPropertyValue(nameof(File), ref file, value); }

        [VisibleInListView(false), VisibleInDetailView(false), VisibleInReports(false)]
        [ModelDefault("PropertyEditor", "PdfViewerPropertyEditor")]
        public FileData FileView => File;

        public DateTime DateAdded { get => dateAdded; set => SetPropertyValue(nameof(DateAdded), ref dateAdded, value); }

        [Size(10)]
        public string RevisionNumber { get => revisionNumber; set => SetPropertyValue(nameof(RevisionNumber), ref revisionNumber, value); }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string RevisionDescription { get => revisionDescription; set => SetPropertyValue(nameof(RevisionDescription), ref revisionDescription, value); }

        public bool IsMostCurrent { get => isMostCurrent; set => SetPropertyValue(nameof(IsMostCurrent), ref isMostCurrent, value); }

    }
}
