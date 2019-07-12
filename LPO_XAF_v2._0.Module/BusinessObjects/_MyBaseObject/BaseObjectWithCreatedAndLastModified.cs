//-----------------------------------------------------------------------
// <copyright file="F:\Users\dlandry\source\repos\LPO_XAF_v2.0\LPO_XAF_v2._0.Module\BusinessObjects\_MyBaseObject\BaseObjectWithCreatedAndLastModified.cs" company="David W. Landry III">
//     Author: _**David Landry**_
//     *Copyright (c) David W. Landry III. All rights reserved.*
// </copyright>
//-----------------------------------------------------------------------
using DevExpress.ExpressApp;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.Xpo;
using System;
using System.Linq;

namespace LPO_XAF_v2._0.Module.BusinessObjects._MyBaseObject
{
    [NonPersistent]
    public abstract class BaseObjectWithCreatedAndLastModified : BaseObject
    {
        public BaseObjectWithCreatedAndLastModified(Session session) : base(session) { }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            createdBy = GetCurrentUser();
            createdOn = DateTime.Now;
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            lastModifiedBy = GetCurrentUser();
            lastModifiedOn = DateTime.Now;
        }

        [Persistent("CreatedBy")]
        PermissionPolicyUser createdBy;
        [Persistent("CreatedOn")]
        DateTime createdOn;
        [Persistent("LastModifiedBy")]
        PermissionPolicyUser lastModifiedBy;
        [Persistent("LastModifiedOn")]
        DateTime lastModifiedOn;

        [PersistentAlias("createdBy")]
        public PermissionPolicyUser CreatedBy { get => createdBy; protected set => SetPropertyValue(nameof(CreatedBy), ref createdBy, value); }
        [PersistentAlias("createdOn")]
        public DateTime CreatedOn { get => createdOn; protected set => SetPropertyValue(nameof(CreatedOn), ref createdOn, value); }
        [PersistentAlias("lastModifiedBy")]
        public PermissionPolicyUser LastModifiedBy { get => lastModifiedBy; protected set => SetPropertyValue(nameof(LastModifiedBy), ref lastModifiedBy, value); }
        [PersistentAlias("lastModifiedOn")]
        public DateTime LastModifiedOn { get => lastModifiedOn; protected set => SetPropertyValue(nameof(LastModifiedOn), ref lastModifiedOn, value); }


        PermissionPolicyUser GetCurrentUser() => Session.GetObjectByKey<PermissionPolicyUser>(SecuritySystem.CurrentUserId);

    }
}