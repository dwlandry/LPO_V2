//-----------------------------------------------------------------------
// <copyright file="F:\Users\dlandry\source\repos\LPO_XAF_v2.0\LPO_XAF_v2._0.Module\BusinessObjects\Procurement\Procurement.cs" company="David W. Landry III">
//     Author: _**David Landry**_
//     *Copyright (c) David W. Landry III. All rights reserved.*
// </copyright>
//-----------------------------------------------------------------------
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.Linq;

namespace LPO_XAF_v2._0.Module.BusinessObjects.Procurement
{

    public class RFQ : BaseObject
    {
        public RFQ(Session session) : base(session) { }
        public override void AfterConstruction() => base.AfterConstruction();


        string engineer;
        string name;
        byte[] document;


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Name { get => name; set => SetPropertyValue(nameof(Name), ref name, value); }
        [Size(SizeAttribute.Unlimited)]
        [EditorAlias(EditorAliases.RichTextPropertyEditor)]
        public byte[] Document { get => document; set => SetPropertyValue(nameof(Document), ref document, value); }
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Engineer { get => engineer; set => SetPropertyValue(nameof(Engineer), ref engineer, value); }
    }
}