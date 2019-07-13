using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using LPO_XAF_v2._0.Module.BusinessObjects.Instrument;
using LPO_XAF_v2._0.Module.BusinessObjects.Piping;

namespace LPO_XAF_v2._0.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class InstrumentViewController : ViewController
    {
        public InstrumentViewController()
        {
            InitializeComponent();
            TargetObjectType = typeof(Instrument);
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        private void OpenLinePipeSpec_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            Line lineNumber = ((Instrument)e.CurrentObject).LineNumber;
            if (lineNumber != null)
            {
                ClientPipeSpec pipeSpec = lineNumber.PipeSpec;
                if (pipeSpec != null)
                {
                    FileData fd = pipeSpec.GetType().GetProperty("File").GetValue(pipeSpec, null) as FileData;

                    if (fd != null)
                    {
                        var tempFolder = System.IO.Path.GetTempPath();
                        var filename = System.IO.Path.Combine(tempFolder, fd.FileName);
                        System.IO.File.WriteAllBytes(filename, fd.Content);
                        System.Diagnostics.Process.Start(filename);
                    }
                }
            }
        }
    }
}
