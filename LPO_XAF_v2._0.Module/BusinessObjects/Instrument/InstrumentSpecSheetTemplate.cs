//-----------------------------------------------------------------------
// <copyright file="F:\Users\dlandry\Source\Repos\LPO_V2\LPO_XAF_v2._0.Module\BusinessObjects\Instrument\InstrumentSpecSheetTemplate.cs" company="David W. Landry III">
//     Author: _**David Landry**_
//     *Copyright (c) David W. Landry III. All rights reserved.*
// </copyright>
//-----------------------------------------------------------------------
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.Linq;

namespace LPO_XAF_v2._0.Module.BusinessObjects.Instrument
{
    [DefaultClassOptions, CreatableItem(false), NavigationItem("Instrumentation")]
    [ImageName("InstrumentSpecSheetTemplate")]
    [XafDefaultProperty("Title")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewAndDetailView)]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class InstrumentSpecSheetTemplate : BaseObject
    {
        public InstrumentSpecSheetTemplate(Session session) : base(session) { }

        public override void AfterConstruction() => base.AfterConstruction();


        InstrumentSpecSheetTemplateCategory category;
        string formNumber;
        string title;
        InstrumentSpecSheetTemplateSource source;
        byte[] specSheet;


        [Association("Category-SpecSheetTemplates")]
        public InstrumentSpecSheetTemplateCategory Category { get => category; set => SetPropertyValue(nameof(Category), ref category, value); }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string FormNumber { get => formNumber; set => SetPropertyValue(nameof(FormNumber), ref formNumber, value); }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Title { get => title; set => SetPropertyValue(nameof(Title), ref title, value); }

        [Association("InstrumentSpecSheetTemplateSource-InstrumentSpecSheetTemplates")]
        public InstrumentSpecSheetTemplateSource Source { get => source; set => SetPropertyValue(nameof(Source), ref source, value); }

        [EditorAlias(EditorAliases.SpreadsheetPropertyEditor)]
        public byte[] SpecSheet { get => specSheet; set => SetPropertyValue(nameof(SpecSheet), ref specSheet, value); }

        public string DisplayName
        {
            get
            {
                string category = Category != null ? Category.Name : string.Empty;
                string form_number = FormNumber;
                string title = Title;
                string source = Source != null ? Source.Name : string.Empty;

                return $"{category}: {title} - Form Number: {form_number}, Source: {source}";
            }
        }

    }

    public class InstrumentSpecSheetTemplateSource : BaseObject
    {

        public InstrumentSpecSheetTemplateSource(Session session) : base(session) { }

        string name;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Name { get => name; set => SetPropertyValue(nameof(Name), ref name, value); }

        [Association("InstrumentSpecSheetTemplateSource-InstrumentSpecSheetTemplates")]
        public XPCollection<InstrumentSpecSheetTemplate> InstrumentSpecSheetTemplates { get { return GetCollection<InstrumentSpecSheetTemplate>(nameof(InstrumentSpecSheetTemplates)); } }
    }

    public class InstrumentSpecSheetTemplateCategory : BaseObject
    {

        public InstrumentSpecSheetTemplateCategory(Session session) : base(session) { }

        string name;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Name { get => name; set => SetPropertyValue(nameof(Name), ref name, value); }

        [Association("Category-SpecSheetTemplates")]
        public XPCollection<InstrumentSpecSheetTemplate> SpecSheetTemplates { get { return GetCollection<InstrumentSpecSheetTemplate>(nameof(SpecSheetTemplates)); } }
    }
}