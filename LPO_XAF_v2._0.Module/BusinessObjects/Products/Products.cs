﻿//-----------------------------------------------------------------------
// <copyright file="F:\Users\dlandry\source\repos\LPO_XAF_v2.0\LPO_XAF_v2._0.Module\BusinessObjects\Products\Products.cs" company="David W. Landry III">
//     Author: _**David Landry**_
//     *Copyright (c) David W. Landry III. All rights reserved.*
// </copyright>
//-----------------------------------------------------------------------
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.Linq;

namespace LPO_XAF_v2._0.Module.BusinessObjects.Products
{
    [DefaultClassOptions, CreatableItem(false), NavigationItem("Products")]
    [ImageName("Vendors")]
    public class Vendor : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Vendor(Session session) : base(session) { }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        Address address;
        string website;
        string name;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Name { get => name; set => SetPropertyValue(nameof(Name), ref name, value); }

        [Association("Vendor-Contacts")]
        public XPCollection<VendorContact> Contacts { get { return GetCollection<VendorContact>(nameof(Contacts)); } }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        [EditorAlias("HyperLinkStringPropertyEditor")]
        public string Website { get => website; set => SetPropertyValue(nameof(Website), ref website, value); }

        public Address Address { get => address; set => SetPropertyValue(nameof(Address), ref address, value); }

        [Association("Vendor-InteractionExperience")]
        public XPCollection<VendorJournalEntry> InteractionExperience { get { return GetCollection<VendorJournalEntry>(nameof(InteractionExperience)); } }

        [Association("Vendor-SupportedManufacturers")]
        public XPCollection<Manufacturer> SupportedManufacturers { get { return GetCollection<Manufacturer>(nameof(SupportedManufacturers)); } }

    }

    public class VendorContact : Person
    {

        public VendorContact(Session session) : base(session) { }

        string title;
        byte[] notes;
        Vendor vendor;

        [Association("Vendor-Contacts")]
        public Vendor Vendor { get => vendor; set => SetPropertyValue(nameof(Vendor), ref vendor, value); }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Title { get => title; set => SetPropertyValue(nameof(Title), ref title, value); }

        [EditorAlias(EditorAliases.RichTextPropertyEditor)]
        public byte[] Notes
        {
            get => notes;
            set => SetPropertyValue(nameof(Notes), ref notes, value);
        }
        [Association("VendorContact-Journal"), Aggregated]
        public XPCollection<VendorContactJournalEntry> Journal { get { return GetCollection<VendorContactJournalEntry>(nameof(Journal)); } }

    }

    public class VendorContactJournalEntry : JournalEntry
    {

        public VendorContactJournalEntry(Session session) : base(session) { }

        VendorContact vendor;

        [Association("VendorContact-Journal")]
        public VendorContact Vendor { get => vendor; set => SetPropertyValue(nameof(Vendor), ref vendor, value); }
    }

    public class VendorJournalEntry : JournalEntry
    {

        public VendorJournalEntry(Session session) : base(session) { }

        Vendor vendor;

        [Association("Vendor-InteractionExperience")]
        public Vendor Vendor { get => vendor; set => SetPropertyValue(nameof(Vendor), ref vendor, value); }

    }
    public class JournalEntry : BaseObject
    {

        public JournalEntry(Session session) : base(session) { }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            dateEntered = DateTime.Now;
            enteredBy = SecuritySystem.CurrentUserName;
        }


        string entry;
        [Persistent("EnteredBy")]
        string enteredBy;
        [Persistent("DateEntered")]
        DateTime dateEntered;

        [PersistentAlias("dateEntered")]
        public DateTime DateEntered { get => dateEntered; }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        [PersistentAlias("enteredBy")]
        public string EnteredBy { get => enteredBy; }


        [Size(SizeAttribute.Unlimited)]
        public string Entry { get => entry; set => SetPropertyValue(nameof(Entry), ref entry, value); }
    }

    [DefaultClassOptions, CreatableItem(false), NavigationItem("Products")]
    public class Manufacturer : BaseObject
    {

        public Manufacturer(Session session) : base(session) { }


        string website;
        string name;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Name { get => name; set => SetPropertyValue(nameof(Name), ref name, value); }

        [EditorAlias("HyperLinkStringPropertyEditor")]
        public string Website { get => website; set => SetPropertyValue(nameof(Website), ref website, value); }

        [Association("Vendor-SupportedManufacturers")]
        public XPCollection<Vendor> Vendors { get { return GetCollection<Vendor>(nameof(Vendors)); } }
    }
}