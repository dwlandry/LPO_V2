//-----------------------------------------------------------------------
// <copyright file="F:\Users\dlandry\Source\Repos\LPO_V2\LPO_XAF_v2._0.Module\BusinessObjects\_ViewVariantsManager\SettingsStore.cs" company="David W. Landry III">
//     Author: _**David Landry**_
//     *Copyright (c) David W. Landry III. All rights reserved.*
// </copyright>
//-----------------------------------------------------------------------
using DevExpress.ExpressApp;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using System.Linq;

namespace LPO_XAF_v2._0.Module.BusinessObjects._ViewVariantsManager
{
    // DLandry: Note that this example comes from https://www.devexpress.com/Support/Center/Example/Details/T537863/how-to-save-and-share-custom-view-settings

    public class SettingsStore : BaseObject, IObjectSpaceLink
    {
        private string xml;
        private string name;
        private string ownerId;
        private string viewId;
        private Boolean isShared;
        private IObjectSpace objectSpace;

        public SettingsStore(Session session) : base(session) { }

        [Browsable(false)]
        [Size(SizeAttribute.Unlimited)]
        public string Xml { get => xml; set => SetPropertyValue("XML", ref xml, value); }

        public string Name { get => name; set => SetPropertyValue("Name", ref name, value); }

        [Browsable(false)]
        public string OwnerId { get => ownerId; set => SetPropertyValue<string>("OwnerId", ref ownerId, value); }

        public Boolean IsShared { get => isShared; set => SetPropertyValue("IsShared", ref isShared, value); }

        [Browsable(false)]
        public string ViewId { get => viewId; set => SetPropertyValue<string>("ViewId", ref viewId, value); }

        IObjectSpace IObjectSpaceLink.ObjectSpace { get { return objectSpace; } set { objectSpace = value; } }

    }
}