//-----------------------------------------------------------------------
// <copyright file="F:\Users\dlandry\Source\Repos\LPO_XAF_v2._0.Module\BusinessObjects\Mechanical\Equipment.cs" company="David W. Landry III">
//     Author: _**David Landry**_
//     *Copyright (c) David W. Landry III. All rights reserved.*
// </copyright>
//-----------------------------------------------------------------------
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using LPO_XAF_v2._0.Module.BusinessObjects.Instrument;
using System;
using System.Linq;

namespace LPO_XAF_v2._0.Module.BusinessObjects.Mechanical
{
    [DefaultClassOptions, CreatableItem(false), NavigationItem("Mechanical")]
    public class Equipment : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Equipment(Session session) : base(session) { }


        PID pID;
        Project.Project project;
        string name;
        string equipmentNumber;


        [Association("Project-Equipment")]
        public Project.Project Project { get => project; set => SetPropertyValue(nameof(Project), ref project, value); }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string EquipmentNumber { get => equipmentNumber; set => SetPropertyValue(nameof(EquipmentNumber), ref equipmentNumber, value); }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Name { get => name; set => SetPropertyValue(nameof(Name), ref name, value); }


        [Association("PID-Equipment")]
        [DataSourceCriteria("Project.Oid = '@This.Oid'")]
        public PID PID { get => pID; set => SetPropertyValue(nameof(PID), ref pID, value); }
    }
}