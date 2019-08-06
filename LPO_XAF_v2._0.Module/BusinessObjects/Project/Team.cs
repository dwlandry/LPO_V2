using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using LPO_XAF_v2._0.Module.BusinessObjects.Company;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPO_XAF_v2._0.Module.BusinessObjects.Project
{
    public class ProjectTeamMember : BaseObject
    {
        public ProjectTeamMember(Session session) : base(session) { }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            this.StartDate = DateTime.Now;
        }

        DateTime endDate;
        DateTime startDate;
        bool isActive;
        string projectRole;
        Employee employee;
        Project project;

        [Association("Project-Team")]
        public Project Project { get => project; set => SetPropertyValue(nameof(Project), ref project, value); }

        [Association("Employee-Projects")]
        public Employee Employee { get => employee; set => SetPropertyValue(nameof(Employee), ref employee, value); }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string ProjectRole { get => projectRole; set => SetPropertyValue(nameof(ProjectRole), ref projectRole, value); }

        [CaptionsForBoolValues("YES", "NO")]
        public bool IsActive { get => isActive; set => SetPropertyValue(nameof(IsActive), ref isActive, value); }

        public DateTime StartDate { get => startDate; set => SetPropertyValue(nameof(StartDate), ref startDate, value); }

        public DateTime EndDate { get => endDate; set => SetPropertyValue(nameof(EndDate), ref endDate, value); }

    }
}
