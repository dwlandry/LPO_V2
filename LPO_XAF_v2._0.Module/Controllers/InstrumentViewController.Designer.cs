namespace LPO_XAF_v2._0.Module.Controllers
{
    partial class InstrumentViewController
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.OpenPID = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.OpenMountingDetail = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.OpenElectricalDetail = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.OpenTracingDetail = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.OpenTubingDetail = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.OpenAreaClassDrawing = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.OpenLoopDrawing = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.OpenPlanDrawing = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.OpenLinePipeSpec = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // OpenPID
            // 
            this.OpenPID.Caption = "P&&ID";
            this.OpenPID.Category = "OpenObject";
            this.OpenPID.ConfirmationMessage = null;
            this.OpenPID.Id = "c9fc22f9-d6f3-4d60-b496-27446764ba25";
            this.OpenPID.ToolTip = null;
            // 
            // OpenMountingDetail
            // 
            this.OpenMountingDetail.Caption = "Mounting Detail";
            this.OpenMountingDetail.Category = "OpenObject";
            this.OpenMountingDetail.ConfirmationMessage = null;
            this.OpenMountingDetail.Id = "74f1781e-dc6d-4a95-be4a-3c5bcde7645d";
            this.OpenMountingDetail.ToolTip = null;
            // 
            // OpenElectricalDetail
            // 
            this.OpenElectricalDetail.Caption = "Electrical Detail Drawing";
            this.OpenElectricalDetail.Category = "OpenObject";
            this.OpenElectricalDetail.ConfirmationMessage = null;
            this.OpenElectricalDetail.Id = "330eba5f-2249-479e-bc30-713abeb60a2c";
            this.OpenElectricalDetail.ToolTip = null;
            // 
            // OpenTracingDetail
            // 
            this.OpenTracingDetail.Caption = "Tracing Detail Drawing";
            this.OpenTracingDetail.Category = "OpenObject";
            this.OpenTracingDetail.ConfirmationMessage = null;
            this.OpenTracingDetail.Id = "0c52ca43-7133-4a22-be81-bbdd775c946f";
            this.OpenTracingDetail.ToolTip = null;
            // 
            // OpenTubingDetail
            // 
            this.OpenTubingDetail.Caption = "Tubing Detail Drawing";
            this.OpenTubingDetail.Category = "OpenObject";
            this.OpenTubingDetail.ConfirmationMessage = null;
            this.OpenTubingDetail.Id = "df862f06-420e-4560-95b7-754001ad9344";
            this.OpenTubingDetail.ToolTip = null;
            // 
            // OpenAreaClassDrawing
            // 
            this.OpenAreaClassDrawing.Caption = "Area Classification Drawing";
            this.OpenAreaClassDrawing.Category = "OpenObject";
            this.OpenAreaClassDrawing.ConfirmationMessage = null;
            this.OpenAreaClassDrawing.Id = "5e6b7be9-b38a-44b6-8b95-4d3a67b24465";
            this.OpenAreaClassDrawing.ToolTip = null;
            // 
            // OpenLoopDrawing
            // 
            this.OpenLoopDrawing.Caption = "Loop Drawing";
            this.OpenLoopDrawing.Category = "OpenObject";
            this.OpenLoopDrawing.ConfirmationMessage = null;
            this.OpenLoopDrawing.Id = "dbb925cb-d8c4-4661-834c-e67c3ea4e160";
            this.OpenLoopDrawing.ToolTip = null;
            // 
            // OpenPlanDrawing
            // 
            this.OpenPlanDrawing.Caption = "Plan Drawing";
            this.OpenPlanDrawing.Category = "OpenObject";
            this.OpenPlanDrawing.ConfirmationMessage = null;
            this.OpenPlanDrawing.Id = "54fca96a-4040-4f7d-bacc-ecc5531a59f0";
            this.OpenPlanDrawing.ToolTip = null;
            // 
            // OpenLinePipeSpec
            // 
            this.OpenLinePipeSpec.Caption = "Line Pipe Spec";
            this.OpenLinePipeSpec.Category = "OpenObject";
            this.OpenLinePipeSpec.ConfirmationMessage = null;
            this.OpenLinePipeSpec.Id = "b32aa212-9f26-4414-9cfd-deb91d0a7a77";
            this.OpenLinePipeSpec.ToolTip = null;
            this.OpenLinePipeSpec.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.OpenLinePipeSpec_Execute);
            // 
            // InstrumentViewController
            // 
            this.Actions.Add(this.OpenPID);
            this.Actions.Add(this.OpenMountingDetail);
            this.Actions.Add(this.OpenElectricalDetail);
            this.Actions.Add(this.OpenTracingDetail);
            this.Actions.Add(this.OpenTubingDetail);
            this.Actions.Add(this.OpenAreaClassDrawing);
            this.Actions.Add(this.OpenLoopDrawing);
            this.Actions.Add(this.OpenPlanDrawing);
            this.Actions.Add(this.OpenLinePipeSpec);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction OpenPID;
        private DevExpress.ExpressApp.Actions.SimpleAction OpenMountingDetail;
        private DevExpress.ExpressApp.Actions.SimpleAction OpenElectricalDetail;
        private DevExpress.ExpressApp.Actions.SimpleAction OpenTracingDetail;
        private DevExpress.ExpressApp.Actions.SimpleAction OpenTubingDetail;
        private DevExpress.ExpressApp.Actions.SimpleAction OpenAreaClassDrawing;
        private DevExpress.ExpressApp.Actions.SimpleAction OpenLoopDrawing;
        private DevExpress.ExpressApp.Actions.SimpleAction OpenPlanDrawing;
        private DevExpress.ExpressApp.Actions.SimpleAction OpenLinePipeSpec;
    }
}
