//-----------------------------------------------------------------------
// <copyright file="F:\Users\dlandry\source\repos\LPO_XAF_v2.0\LPO_XAF_v2._0.Module\BusinessObjects\Instrument\InstrumentSpecCheckPackage.cs" company="David W. Landry III">
//     Author: _**David Landry**_
//     *Copyright (c) David W. Landry III. All rights reserved.*
// </copyright>
//-----------------------------------------------------------------------
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using System.Linq;

namespace LPO_XAF_v2._0.Module.BusinessObjects.Instrument
{
    [DefaultClassOptions, CreatableItem(false), DefaultProperty("CheckPackageName"), NavigationItem("Instrumentation")]
    //[ImageName("BO_Security_Permission_Type")]
    [ImageName("InstrumentCheckPackage")]
    public class InstrumentSpecCheckPackage : BaseObject
    {
        public InstrumentSpecCheckPackage(Session session) : base(session) { }
        public override void AfterConstruction() => base.AfterConstruction();

        bool approved;
        bool issuedForApproval;
        FileData ifpPackage;
        bool issuedForPurchase;
        FileData ifaPackage;
        FileData backCheckPackage;
        byte[] notes;
        bool checkFinalized;
        bool backCheckComplete;
        bool submittedForBackCheck;
        bool pickupsMade;
        bool @checked;
        bool submittedForCheck;
        string checker;
        DateTime dateReturnedFromChecker;
        DateTime dateSubmittedToCheck;
        FileData checkedPackage;
        FileData originalCheckPackage;
        string engineer;
        string clientProjectNumber;
        string projectManager;
        string projectDescription;
        string projectNumber;
        string clientName;
        Project.Project project;
        string description;
        string checkPackageName;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        [RuleRequiredField("RuleRequiredField for InstrumentSpecCheckPackage.Name", DefaultContexts.Save, "A Check Package Name must be specified.")]
        public string CheckPackageName { get => checkPackageName; set => SetPropertyValue(nameof(CheckPackageName), ref checkPackageName, value); }
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Description { get => description; set => SetPropertyValue(nameof(Description), ref description, value); }


        [Association("Project-InstrumentSpecCheckPackages")]
        public Project.Project Project
        {
            get => project;
            set
            {
                if (project == value) return;
                Project.Project prevProject = project;
                project = value;
                if (IsLoading) return;
                if (project != null)
                {
                    this.clientName = value.Client.Name;
                    this.projectNumber = value.ProjectNumber;
                    this.ProjectDescription = value.ProjectDescription;
                    this.clientProjectNumber = value.ClientProjectNumber;
                }
                SetPropertyValue(nameof(Project), ref project, value);
            }
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string ClientName { get => clientName; set => SetPropertyValue(nameof(ClientName), ref clientName, value); }
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string ProjectNumber { get => projectNumber; set => SetPropertyValue(nameof(ProjectNumber), ref projectNumber, value); }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string ProjectDescription { get => projectDescription; set => SetPropertyValue(nameof(ProjectDescription), ref projectDescription, value); }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string ProjectManager { get => projectManager; set => SetPropertyValue(nameof(ProjectManager), ref projectManager, value); }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string ClientProjectNumber { get => clientProjectNumber; set => SetPropertyValue(nameof(ClientProjectNumber), ref clientProjectNumber, value); }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Engineer { get => engineer; set => SetPropertyValue(nameof(Engineer), ref engineer, value); }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Checker { get => checker; set => SetPropertyValue(nameof(Checker), ref checker, value); }

        //[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
        [DataSourceCriteria("Project.Oid = '@This.Project.Oid'")]
        [Association("Instruments-InstrumentSpecCheckPackages")]
        public XPCollection<Instrument> Instruments => GetCollection<Instrument>(nameof(Instruments));


        public DateTime DateSubmittedToCheck { get => dateSubmittedToCheck; set => SetPropertyValue(nameof(DateSubmittedToCheck), ref dateSubmittedToCheck, value); }
        public DateTime DateReturnedFromChecker { get => dateReturnedFromChecker; set => SetPropertyValue(nameof(DateReturnedFromChecker), ref dateReturnedFromChecker, value); }

        public FileData OriginalCheckPackage { get => originalCheckPackage; set => SetPropertyValue(nameof(OriginalCheckPackage), ref originalCheckPackage, value); }
        public FileData CheckedPackage { get => checkedPackage; set => SetPropertyValue(nameof(CheckedPackage), ref checkedPackage, value); }
        public FileData BackCheckPackage { get => backCheckPackage; set => SetPropertyValue(nameof(BackCheckPackage), ref backCheckPackage, value); }
        [DevExpress.Xpo.DisplayName("IFA Package")]
        public FileData IfaPackage { get => ifaPackage; set => SetPropertyValue(nameof(IfaPackage), ref ifaPackage, value); }
        [DevExpress.Xpo.DisplayName("IFP Package")]
        public FileData IfpPackage { get => ifpPackage; set => SetPropertyValue(nameof(IfpPackage), ref ifpPackage, value); }

        public bool SubmittedForCheck { get => submittedForCheck; set => SetPropertyValue(nameof(SubmittedForCheck), ref submittedForCheck, value); }
        public bool Checked { get => @checked; set => SetPropertyValue(nameof(Checked), ref @checked, value); }
        public bool PickupsMade { get => pickupsMade; set => SetPropertyValue(nameof(PickupsMade), ref pickupsMade, value); }
        public bool SubmittedForBackCheck { get => submittedForBackCheck; set => SetPropertyValue(nameof(SubmittedForBackCheck), ref submittedForBackCheck, value); }
        public bool BackCheckComplete { get => backCheckComplete; set => SetPropertyValue(nameof(BackCheckComplete), ref backCheckComplete, value); }
        public bool CheckFinalized { get => checkFinalized; set => SetPropertyValue(nameof(CheckFinalized), ref checkFinalized, value); }
        public bool IssuedForApproval { get => issuedForApproval; set => SetPropertyValue(nameof(IssuedForApproval), ref issuedForApproval, value); }
        public bool Approved { get => approved; set => SetPropertyValue(nameof(Approved), ref approved, value); }
        public bool IssuedForPurchase { get => issuedForPurchase; set => SetPropertyValue(nameof(IssuedForPurchase), ref issuedForPurchase, value); }

        [Size(SizeAttribute.Unlimited)]
        [EditorAlias(EditorAliases.RichTextPropertyEditor)]
        public byte[] Notes
        {
            get => notes;
            set => SetPropertyValue(nameof(Notes), ref notes, value);
        }
    }
}