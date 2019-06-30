using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.Editors;

namespace LPO_XAF_v2._0.Module.BusinessObjects.Instrument
{
    [DefaultClassOptions, CreatableItem(false), NavigationItem("Instrumentation"), DefaultListViewOptions(MasterDetailMode.ListViewAndDetailView)]
    [ImageName("InstrumentIndexTemplate")]
    
    public class InstrumentIndexTemplate : BaseObject
    {
        public InstrumentIndexTemplate(Session session) : base(session) { }
        public override void AfterConstruction() => base.AfterConstruction();

        string notes;
        byte[] instrumentIndex;
        string name;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Name { get => name; set => SetPropertyValue(nameof(Name), ref name, value); }

        [Size(200)]
        public string Notes { get => notes; set => SetPropertyValue(nameof(Notes), ref notes, value); }

        [EditorAlias(EditorAliases.SpreadsheetPropertyEditor)]
        public byte[] InstrumentIndex { get => instrumentIndex; set => SetPropertyValue(nameof(InstrumentIndex), ref instrumentIndex, value); }

    }
}