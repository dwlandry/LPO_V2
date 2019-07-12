//-----------------------------------------------------------------------
// <copyright file="F:\Users\dlandry\source\repos\LPO_XAF_v2.0\LPO_XAF_v2._0.Module\BusinessObjects\Instrument\InstrumentCalculator.cs" company="David W. Landry III">
//     Author: _**David Landry**_
//     *Copyright (c) David W. Landry III. All rights reserved.*
// </copyright>
//-----------------------------------------------------------------------
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.Linq;

namespace LPO_XAF_v2._0.Module.BusinessObjects.Instrument
{
    [DefaultClassOptions, CreatableItem(false), NavigationItem("Instrumentation"), ImageName("InstrumentCalculator")]
    public class InstrumentCalculator : _MyBaseObject.BaseObjectWithCreatedAndLastModified
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public InstrumentCalculator(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        Project.Client client;
        string notes;
        byte[] calculator;
        string name;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Name { get => name; set => SetPropertyValue(nameof(Name), ref name, value); }

        [EditorAlias(EditorAliases.SpreadsheetPropertyEditor)]
        public byte[] Calculator { get => calculator; set => SetPropertyValue(nameof(Calculator), ref calculator, value); }

        [Size(SizeAttribute.Unlimited)]
        public string Notes { get => notes; set => SetPropertyValue(nameof(Notes), ref notes, value); }

        public Project.Client Client { get => client; set => SetPropertyValue(nameof(Client), ref client, value); }
    }
}