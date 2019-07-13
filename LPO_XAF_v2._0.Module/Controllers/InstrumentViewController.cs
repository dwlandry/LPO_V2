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
using DevExpress.XtraEditors;
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
            if (lineNumber == null)
            {
                XtraMessageBox.Show($"No line number has been selected for instrument {((Instrument)e.CurrentObject).TagNumber}.");
                return;
            }
            ClientPipeSpec pipeSpec = lineNumber.PipeSpec;
            if (pipeSpec == null)
            {
                XtraMessageBox.Show($"There is no pipe spec selected for the line number {lineNumber.LineNumber}.");
                return;
            }
            FileData fd = pipeSpec.GetType().GetProperty("File").GetValue(pipeSpec, null) as FileData;
            if (fd == null)
            {
                XtraMessageBox.Show($"There is no file associated with the {pipeSpec.SpecNumber} pipe spec.");
                return;
            }
            
            var tempFolder = System.IO.Path.GetTempPath();
            var filename = System.IO.Path.Combine(tempFolder, fd.FileName);
            System.IO.File.WriteAllBytes(filename, fd.Content);
            System.Diagnostics.Process.Start(filename);
            
        }

        private void OpenPID_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            string dwgName = "P&ID";
            Instrument instrument = ((Instrument)e.CurrentObject);
            PID dwg = instrument.Pid;
            if (dwg == null)
            {
                XtraMessageBox.Show($"No {dwgName} has been selected for instrument {instrument.TagNumber}.");
                return;
            }
            FileData fd = dwg.GetType().GetProperty("File").GetValue(dwg, null) as FileData;
            if (fd == null)
            {
                XtraMessageBox.Show($"There is no file associated with {dwgName} {dwg.DrawingNumber}.");
                return;
            }

            var tempFolder = System.IO.Path.GetTempPath();
            var filename = System.IO.Path.Combine(tempFolder, fd.FileName);
            System.IO.File.WriteAllBytes(filename, fd.Content);
            System.Diagnostics.Process.Start(filename);
        }

        private void OpenMountingDetail_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            string dwgName = "Mounting Detail";
            Instrument instrument = ((Instrument)e.CurrentObject);
            MountingDetail dwg = instrument.MountingDetail;
            if (dwg == null)
            {
                XtraMessageBox.Show($"No {dwgName} has been selected for instrument {instrument.TagNumber}.");
                return;
            }
            FileData fd = dwg.GetType().GetProperty("File").GetValue(dwg, null) as FileData;
            if (fd == null)
            {
                XtraMessageBox.Show($"There is no file associated with {dwgName} {dwg.DrawingNumber}.");
                return;
            }
            var tempFolder = System.IO.Path.GetTempPath();
            var filename = System.IO.Path.Combine(tempFolder, fd.FileName);
            System.IO.File.WriteAllBytes(filename, fd.Content);
            System.Diagnostics.Process.Start(filename);
        }

        private void OpenElectricalDetail_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            string dwgName = "Electrical Detail";
            Instrument instrument = ((Instrument)e.CurrentObject);
            ElectricalDetail dwg = instrument.ElectricalDetail;
            if (dwg == null)
            {
                XtraMessageBox.Show($"No {dwgName} has been selected for instrument {instrument.TagNumber}.");
                return;
            }
            FileData fd = dwg.GetType().GetProperty("File").GetValue(dwg, null) as FileData;
            if (fd == null)
            {
                XtraMessageBox.Show($"There is no file associated with {dwgName} {dwg.DrawingNumber}.");
                return;
            }
            var tempFolder = System.IO.Path.GetTempPath();
            var filename = System.IO.Path.Combine(tempFolder, fd.FileName);
            System.IO.File.WriteAllBytes(filename, fd.Content);
            System.Diagnostics.Process.Start(filename);
        }

        private void OpenTracingDetail_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            string dwgName = "Tracing Detail";
            Instrument instrument = ((Instrument)e.CurrentObject);
            var dwg = instrument.TracingDetail;
            if (dwg == null)
            {
                XtraMessageBox.Show($"No {dwgName} has been selected for instrument {instrument.TagNumber}.");
                return;
            }
            FileData fd = dwg.GetType().GetProperty("File").GetValue(dwg, null) as FileData;
            if (fd == null)
            {
                XtraMessageBox.Show($"There is no file associated with {dwgName} {dwg.DrawingNumber}.");
                return;
            }
            var tempFolder = System.IO.Path.GetTempPath();
            var filename = System.IO.Path.Combine(tempFolder, fd.FileName);
            System.IO.File.WriteAllBytes(filename, fd.Content);
            System.Diagnostics.Process.Start(filename);
        }

        private void OpenAreaClassDrawing_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            string dwgName = "Area Classification Drawing";
            Instrument instrument = ((Instrument)e.CurrentObject);
            var dwg = instrument.AreaClassificationDrawing;
            if (dwg == null)
            {
                XtraMessageBox.Show($"No {dwgName} has been selected for instrument {instrument.TagNumber}.");
                return;
            }
            FileData fd = dwg.GetType().GetProperty("File").GetValue(dwg, null) as FileData;
            if (fd == null)
            {
                XtraMessageBox.Show($"There is no file associated with {dwgName} {dwg.DrawingNumber}.");
                return;
            }
            var tempFolder = System.IO.Path.GetTempPath();
            var filename = System.IO.Path.Combine(tempFolder, fd.FileName);
            System.IO.File.WriteAllBytes(filename, fd.Content);
            System.Diagnostics.Process.Start(filename);
        }

        private void OpenLoopDrawing_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            string dwgName = "Loop Drawing";
            Instrument instrument = ((Instrument)e.CurrentObject);
            var dwg = instrument.LoopDrawing;
            if (dwg == null)
            {
                XtraMessageBox.Show($"No {dwgName} has been selected for instrument {instrument.TagNumber}.");
                return;
            }
            FileData fd = dwg.GetType().GetProperty("File").GetValue(dwg, null) as FileData;
            if (fd == null)
            {
                XtraMessageBox.Show($"There is no file associated with {dwgName} {dwg.DrawingNumber}.");
                return;
            }
            var tempFolder = System.IO.Path.GetTempPath();
            var filename = System.IO.Path.Combine(tempFolder, fd.FileName);
            System.IO.File.WriteAllBytes(filename, fd.Content);
            System.Diagnostics.Process.Start(filename);
        }

        private void OpenPlanDrawing_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            string dwgName = "Plan Drawing";
            Instrument instrument = ((Instrument)e.CurrentObject);
            var dwg = instrument.PlanDrawing;
            if (dwg == null)
            {
                XtraMessageBox.Show($"No {dwgName} has been selected for instrument {instrument.TagNumber}.");
                return;
            }
            FileData fd = dwg.GetType().GetProperty("File").GetValue(dwg, null) as FileData;
            if (fd == null)
            {
                XtraMessageBox.Show($"There is no file associated with {dwgName} {dwg.DrawingNumber}.");
                return;
            }
            var tempFolder = System.IO.Path.GetTempPath();
            var filename = System.IO.Path.Combine(tempFolder, fd.FileName);
            System.IO.File.WriteAllBytes(filename, fd.Content);
            System.Diagnostics.Process.Start(filename);
        }

        private void OpenTubingDetail_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            string dwgName = "Tubing Detail";
            Instrument instrument = ((Instrument)e.CurrentObject);
            var dwg = instrument.TubingDetail;
            if (dwg == null)
            {
                XtraMessageBox.Show($"No {dwgName} has been selected for instrument {instrument.TagNumber}.");
                return;
            }
            FileData fd = dwg.GetType().GetProperty("File").GetValue(dwg, null) as FileData;
            if (fd == null)
            {
                XtraMessageBox.Show($"There is no file associated with {dwgName} {dwg.DrawingNumber}.");
                return;
            }
            var tempFolder = System.IO.Path.GetTempPath();
            var filename = System.IO.Path.Combine(tempFolder, fd.FileName);
            System.IO.File.WriteAllBytes(filename, fd.Content);
            System.Diagnostics.Process.Start(filename);
        }
    }
}
