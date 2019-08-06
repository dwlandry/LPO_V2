using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPO_XAF_v2._0.Module.BusinessObjects.Company
{
    [DefaultClassOptions, CreatableItem(false), NavigationItem("Company")]
    public class Employee : Person
    {
        public Employee(Session session) : base(session) { }


        EmploymentStatus employmentStatus;
        Department department;
        string title;
        Employee supervisor;

        //[DataSourceCriteria("Employee.Oid != '@This.Employee.Oid'")]
        public Employee Supervisor { get => supervisor; set => SetPropertyValue(nameof(Supervisor), ref supervisor, value); }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Title { get => title; set => SetPropertyValue(nameof(Title), ref title, value); }

        [Association("Department-Employees")]
        public Department Department { get => department; set => SetPropertyValue(nameof(Department), ref department, value); }

        public EmploymentStatus EmploymentStatus { get => employmentStatus; set => SetPropertyValue(nameof(EmploymentStatus), ref employmentStatus, value); }

        [DisplayName("Phone Numbers")]
        public string AllPhoneNumbers => String.Join(", ", base.PhoneNumbers.Select(x => $"({x?.PhoneType}) {x?.Number}").ToList());
    }

    [DefaultClassOptions, CreatableItem(false), NavigationItem("Company")]
    public class Department : BaseObject
    {
        public Department(Session session) : base(session) { }

        [Association("Department-Employees")]
        public XPCollection<Employee> Employees { get { return GetCollection<Employee>(nameof(Employees)); } }


        string name;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Name { get => name; set => SetPropertyValue(nameof(Name), ref name, value); }
    }

    public enum EmploymentStatus
    {
        Active,
        NotActive
    }
}
