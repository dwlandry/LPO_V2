using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
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


        Project project;

        [Association("Project-Team")]
        public Project Project { get => project; set => SetPropertyValue(nameof(Project), ref project, value); }


    }
}
