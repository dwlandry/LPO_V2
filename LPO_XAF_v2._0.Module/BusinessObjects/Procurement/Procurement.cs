//-----------------------------------------------------------------------
// <copyright file="F:\Users\dlandry\source\repos\LPO_XAF_v2.0\LPO_XAF_v2._0.Module\BusinessObjects\Procurement\Procurement.cs" company="David W. Landry III">
//     Author: _**David Landry**_
//     *Copyright (c) David W. Landry III. All rights reserved.*
// </copyright>
//-----------------------------------------------------------------------
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using LPO_XAF_v2._0.Module.BusinessObjects.Products;
using System;
using System.ComponentModel;
using System.Drawing;
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

    public interface IProjectQuote
    {
        Project.Project Project { get; set; }
    }

    [DefaultProperty("DisplayText"), NavigationItem("Procurement")]
    [Appearance("Waiting For Quote", TargetItems = "*;Status;Comments",
        Criteria = "Status == 0 || Status == 2", FontColor = "DarkMagenta", BackColor = "LightGoldenrodYellow")]
    [Appearance("Rejected", TargetItems = "*;Status;Comments",
        Criteria = "Status == 4 || Status == 5", FontColor = "LightSlateGray", FontStyle = FontStyle.Strikeout)]
    [Appearance("Accepted", TargetItems = "*;Status;Comments",
        Criteria = "Status = 3", FontStyle = FontStyle.Bold)]
    public class Quote : BaseObject
    {

        public Quote(Session session) : base(session) { }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Status = QuoteStatus.Received;
        }

        Vendor vendor;
        QuoteStatus quoteStatus;
        string description;
        decimal taxTotal;
        string comments;
        FileData quoteFile;
        decimal quoteTotal;
        string vendorQuoteNumber;
        DateTime dateExpires;
        DateTime dateReceived;
        string contactPhoneNumber;
        string contactEmail;
        string contactName;
        //string vendor;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Description { get => description; set => SetPropertyValue(nameof(Description), ref description, value); }

        //[Size(SizeAttribute.DefaultStringMappingFieldSize)]
        //public string Vendor { get => vendor; set => SetPropertyValue(nameof(Vendor), ref vendor, value); }

        
        [Association("Vendor-Quotes")]
        public Vendor Vendor
        {
            get => vendor;
            set => SetPropertyValue(nameof(Vendor), ref vendor, value);
        }
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string ContactName { get => contactName; set => SetPropertyValue(nameof(ContactName), ref contactName, value); }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string ContactEmail { get => contactEmail; set => SetPropertyValue(nameof(ContactEmail), ref contactEmail, value); }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string ContactPhoneNumber { get => contactPhoneNumber; set => SetPropertyValue(nameof(ContactPhoneNumber), ref contactPhoneNumber, value); }

        public DateTime DateReceived { get => dateReceived; set => SetPropertyValue(nameof(DateReceived), ref dateReceived, value); }

        public DateTime DateExpires { get => dateExpires; set => SetPropertyValue(nameof(DateExpires), ref dateExpires, value); }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string VendorQuoteNumber { get => vendorQuoteNumber; set => SetPropertyValue(nameof(VendorQuoteNumber), ref vendorQuoteNumber, value); }

        public decimal QuoteTotal { get => quoteTotal; set => SetPropertyValue(nameof(QuoteTotal), ref quoteTotal, value); }

        public decimal TaxTotal { get => taxTotal; set => SetPropertyValue(nameof(TaxTotal), ref taxTotal, value); }

        public FileData QuoteFile { get => quoteFile; set => SetPropertyValue(nameof(QuoteFile), ref quoteFile, value); }

        [Size(SizeAttribute.Unlimited)]
        public string Comments { get => comments; set => SetPropertyValue(nameof(Comments), ref comments, value); }

        [Association("Quote-Documents"), Aggregated]
        public XPCollection<QuoteDocument> Documents { get { return GetCollection<QuoteDocument>(nameof(Documents)); } }

        [Association("Quote-LineItems"), Aggregated]
        public XPCollection<QuoteLineItem> LineItems { get { return GetCollection<QuoteLineItem>(nameof(LineItems)); } }

        [VisibleInListView(false), VisibleInDetailView(false)]
        public string DisplayText { get { return $"{Vendor} - Quote No: {VendorQuoteNumber}"; } }

        public QuoteStatus Status { get => quoteStatus; set => SetPropertyValue(nameof(QuoteStatus), ref quoteStatus, value); }
    }

    [DefaultProperty("DisplayText")]
    [DefaultListViewOptions(true, NewItemRowPosition.Top)]
    public class QuoteLineItem : BaseObject
    {

        public QuoteLineItem(Session session) : base(session) { }

        Quote quote;
        bool isTax;
        bool isOptional;
        int leadTimeInWeeks;
        decimal extendedCost;
        decimal unitCost;
        int quantity;
        string description;
        string lineNumber;


        [Association("Quote-LineItems")]
        public Quote Quote { get => quote; set => SetPropertyValue(nameof(Quote), ref quote, value); }

        [Size(20)]
        public string LineNumber { get => lineNumber; set => SetPropertyValue(nameof(LineNumber), ref lineNumber, value); }

        [Size(SizeAttribute.Unlimited)]
        public string Description { get => description; set => SetPropertyValue(nameof(Description), ref description, value); }

        public int Quantity { get => quantity; set => SetPropertyValue(nameof(Quantity), ref quantity, value); }

        public decimal UnitCost { get => unitCost; set => SetPropertyValue(nameof(UnitCost), ref unitCost, value); }

        public decimal ExtendedCost { get => extendedCost; set => SetPropertyValue(nameof(ExtendedCost), ref extendedCost, value); }

        public int LeadTimeInWeeks { get => leadTimeInWeeks; set => SetPropertyValue(nameof(LeadTimeInWeeks), ref leadTimeInWeeks, value); }

        public bool IsOptional { get => isOptional; set => SetPropertyValue(nameof(IsOptional), ref isOptional, value); }

        public bool IsTax { get => isTax; set => SetPropertyValue(nameof(IsTax), ref isTax, value); }

        [VisibleInDetailView(false), VisibleInListView(false)]
        public string DisplayText { get { return $"{LineNumber} - {(Description != null ? "Description.Substring(0, 15)" : string.Empty)}"; } }
    }

    [DefaultListViewOptions(allowEdit: true, NewItemRowPosition.Top)]
    public class QuoteDocument : BaseObject
    {

        public QuoteDocument(Session session) : base(session) { }

        string name;
        Quote quote;
        string description;
        FileData file;

        public FileData File { get => file; set => SetPropertyValue(nameof(File), ref file, value); }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Name { get => name; set => SetPropertyValue(nameof(Name), ref name, value); }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Description { get => description; set => SetPropertyValue(nameof(Description), ref description, value); }

        [Association("Quote-Documents")]
        public Quote Quote { get => quote; set => SetPropertyValue(nameof(Quote), ref quote, value); }
    }

    public enum QuoteStatus
    {
        WaitingForQuote = 0,
        Received = 1,
        ChangeRequested = 2,
        Accepted = 3,
        Rejected = 4,
        NotQuoted = 5

    }
}