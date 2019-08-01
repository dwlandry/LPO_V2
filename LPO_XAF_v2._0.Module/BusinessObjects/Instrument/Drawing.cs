//-----------------------------------------------------------------------
// <copyright file="F:\Users\dlandry\Source\Repos\LPO_XAF_v2._0.Module\BusinessObjects\Instrument\Documents.cs" company="David W. Landry III">
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
