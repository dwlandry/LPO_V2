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
using LPO_XAF_v2._0.Module.BusinessObjects.Instrument;

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
        [DataSourceCriteria("Project.Oid = '@This.Oid'")]
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