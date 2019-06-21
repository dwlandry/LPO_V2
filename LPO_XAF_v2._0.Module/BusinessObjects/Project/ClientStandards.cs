using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPO_XAF_v2._0.Module.BusinessObjects.Project
{

    public class ClientStandard : Document, IClientDocument
    {
        
        public ClientStandard(Session session) : base(session) { }


        Client client;

        [Association("Client-Standards")]
        public Client Client { get => client; set => SetPropertyValue(nameof(Client), ref client, value); }
    }

    public interface IClientDocument
    {
        Client Client { get; set; }
    }

    public class Document : BaseObject
    {
        
        public Document(Session session) : base(session) { }

        string description;
        string name;
        FileData file;

        public FileData File { get => file; set => SetPropertyValue(nameof(File), ref file, value); }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Name { get => name; set => SetPropertyValue(nameof(Name), ref name, value); }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Description { get => description; set => SetPropertyValue(nameof(Description), ref description, value); }
    }

}
