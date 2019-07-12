//-----------------------------------------------------------------------
// <copyright file="F:\Users\dlandry\source\repos\LPO_XAF_v2.0\LPO_XAF_v2._0.Module\BusinessObjects\Education\Tutorial.cs" company="David W. Landry III">
//     Author: _**David Landry**_
//     *Copyright (c) David W. Landry III. All rights reserved.*
// </copyright>
//-----------------------------------------------------------------------
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using System.Linq;

namespace LPO_XAF_v2._0.Module.BusinessObjects.Education
{
    [DefaultClassOptions, ImageName("Book"), CreatableItem(false), NavigationItem("Education")]
    [DefaultProperty("Topic")]
    public class Tutorial : _MyBaseObject.BaseObjectWithCreatedAndLastModified
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Tutorial(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }


        byte[] content;
        string searchableTerms;
        string topic;
        Category category;

        [Association("Category-Tutorials")]
        [RuleRequiredField]
        public Category Category { get => category; set => SetPropertyValue(nameof(Category), ref category, value); }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        [RuleRequiredField]
        public string Topic { get => topic; set => SetPropertyValue(nameof(Topic), ref topic, value); }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string SearchableTerms { get => searchableTerms; set => SetPropertyValue(nameof(SearchableTerms), ref searchableTerms, value); }

        [EditorAlias(EditorAliases.RichTextPropertyEditor)]
        public byte[] Content { get => content; set => SetPropertyValue(nameof(Content), ref content, value); }
    }
    public class Category : _MyBaseObject.BaseObjectWithCreatedAndLastModified
    {

        public Category(Session session) : base(session) { }


        string name;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Name { get => name; set => SetPropertyValue(nameof(Name), ref name, value); }
        [Association("Category-Tutorials")]
        public XPCollection<Tutorial> Tutorials { get { return GetCollection<Tutorial>(nameof(Tutorials)); } }
    }
}