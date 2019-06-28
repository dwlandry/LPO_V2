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

namespace LPO_XAF_v2._0.Module.BusinessObjects.Vendor
{
    [DefaultClassOptions, CreatableItem(false), NavigationItem("Vendors")]
    [ImageName("Vendors_4")]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class Vendor : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Vendor(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        string name;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Name { get => name; set => SetPropertyValue(nameof(Name), ref name, value); }

        [Association("Vendor-Contacts")]
        public XPCollection<VendorContact> Contacts { get { return GetCollection<VendorContact>(nameof(Contacts)); } }

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
    }
}