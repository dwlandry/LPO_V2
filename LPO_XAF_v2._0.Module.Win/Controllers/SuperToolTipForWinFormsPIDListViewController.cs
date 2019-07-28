using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Utils;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Base.ViewInfo;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using LPO_XAF_v2._0.Module.BusinessObjects.Instrument;
using LPO_XAF_v2._0.Module.BusinessObjects.Piping;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPO_XAF_v2._0.Module.Win.Controllers
{
    public class SuperToolTipForWinFormsPIDListViewController : ViewController<ListView>
    {
        ToolTipController toolTipController;
        GridControl gridControl;

        public SuperToolTipForWinFormsPIDListViewController()
        {
            TargetObjectType = typeof(PID);
            TargetViewNesting = Nesting.Any;

        }


        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Get the PID GridViewControl
            GridListEditor listEditor = ((ListView)View).Editor as GridListEditor;
            if (listEditor != null)
            {
                toolTipController = new ToolTipController();
                toolTipController.KeepWhileHovered = true;
                toolTipController.AllowHtmlText = true;
                
                GridView gridView = listEditor.GridView;
                gridControl = gridView.GridControl;
                gridControl.ToolTipController = toolTipController;
                
                toolTipController.GetActiveObjectInfo += ToolTipController_GetActiveObjectInfo;
                toolTipController.HyperlinkClick += ToolTipController_HyperlinkClick;
            }

        }

        private void ToolTipController_HyperlinkClick(object sender, HyperlinkClickEventArgs e)
        {
            string[] split = e.Link.Split(new char[] { ':' });
            var category = split[0];
            var item = split[1];

            if (category == HyperlinkCategory.OpenFile.ToString())
            {
                PID pid = ObjectSpace.FindObject<PID>(new BinaryOperator("Oid", item));
                FileData fd = pid.File;
                var tempFolder = System.IO.Path.GetTempPath();
                var filename = System.IO.Path.Combine(tempFolder, fd.FileName);
                System.IO.File.WriteAllBytes(filename, fd.Content);
                Process.Start(filename);
            }
            else if (category == HyperlinkCategory.OpenRecord.ToString())
            {
                IObjectSpace space = Application.CreateObjectSpace(typeof(PID));
                PID pid = space.FindObject<PID>(new BinaryOperator("Oid", item));

                ShowViewParameters svp = new ShowViewParameters();
                DetailView dv = Application.CreateDetailView(space, pid);
                svp.CreatedView = dv;
                Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(null, null));

            }
            else if (category == HyperlinkCategory.Equipment.ToString())
            {

            }
            else if (category == HyperlinkCategory.Instrument.ToString())
            {
                IObjectSpace space = Application.CreateObjectSpace(typeof(Instrument));
                Instrument instrument = space.FindObject<Instrument>(new BinaryOperator("Oid", item));

                ShowViewParameters svp = new ShowViewParameters();
                DetailView dv = Application.CreateDetailView(space, instrument);
                svp.CreatedView = dv;
                Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(null, null));
            }
            else if (category == HyperlinkCategory.Line.ToString())
            {
                IObjectSpace space = Application.CreateObjectSpace(typeof(Line));
                Line line = space.FindObject<Line>(new BinaryOperator("Oid", item));

                ShowViewParameters svp = new ShowViewParameters();
                DetailView dv = Application.CreateDetailView(space, line);
                svp.CreatedView = dv;
                Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(null, null));
            }
            else if (category == HyperlinkCategory.TiePoint.ToString())
            {
                IObjectSpace space = Application.CreateObjectSpace(typeof(PipingTiePoint));
                PipingTiePoint tiePoint = space.FindObject<PipingTiePoint>(new BinaryOperator("Oid", item));

                ShowViewParameters svp = new ShowViewParameters();
                DetailView dv = Application.CreateDetailView(space, tiePoint);
                svp.CreatedView = dv;
                Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(null, null));
            }
        }

        private void ToolTipController_GetActiveObjectInfo(object sender, ToolTipControllerGetActiveObjectInfoEventArgs e)
        {
            if (e.SelectedControl != gridControl) return;

            ToolTipControlInfo info = null;
            // Get the view at the current mouse position
            GridView view = gridControl.GetViewAt(e.ControlMousePosition) as GridView;

            if (view == null) return;
            // Get the view's element information that resides at the current position
            GridHitInfo hi = view.CalcHitInfo(e.ControlMousePosition);
            // Display a hint for row indicator cells
            if (hi.HitTest == GridHitTest.RowCell)
            {
                object o = hi.HitTest.ToString() + hi.RowHandle.ToString();

                PID pid = ObjectSpace.FindObject<PID>(new BinaryOperator("Oid", ((PID)hi.RowInfo.RowKey).Oid));
                info = new ToolTipControlInfo();
                info.Object = o;
                SuperToolTip superToolTip = new SuperToolTip();

                // PID Header
                ToolTipTitleItem pidNumber = new ToolTipTitleItem();
                pidNumber.Text = $"<size=+4>{pid.DrawingNumber}</size>";
                superToolTip.Items.Add(pidNumber);
                ToolTipItem pidLinks = new ToolTipItem();
                pidLinks.Text = $"<href={HyperlinkCategory.OpenFile}:{pid.Oid}><color=145,197,242>Open File</color></href> <href={HyperlinkCategory.OpenRecord}:{pid.Oid}><color=145,197,242>Open Record</color></href>";
                superToolTip.Items.Add(pidLinks);
                superToolTip.Items.Add(new ToolTipSeparatorItem());

                // Instruments
                var InstrumentCount = pid.Instruments.Count;
                ToolTipTitleItem titleItemInstruments = new ToolTipTitleItem();
                titleItemInstruments.Text = $"Instruments ({InstrumentCount})";
                superToolTip.Items.Add(titleItemInstruments);
                if (InstrumentCount > 0)
                {
                    ToolTipItem itemInstruments = new ToolTipItem();
                    itemInstruments.Text = String.Join(", ", pid.Instruments.Select(item => $"<href={HyperlinkCategory.Instrument}:{item.Oid}><color=145,197,242>{item.TagNumber}</color></href>"));
                    superToolTip.Items.Add(itemInstruments);
                }
                superToolTip.Items.Add(new ToolTipSeparatorItem());
                
                //Lines
                var LineCount = pid.Lines.Count;
                ToolTipTitleItem titleItemLines = new ToolTipTitleItem();
                titleItemLines.Text = $"Lines ({LineCount})";
                superToolTip.Items.Add(titleItemLines);
                if (LineCount>0)
                {
                    ToolTipItem itemLines = new ToolTipItem();
                    itemLines.Text = String.Join(", ", pid.Lines.Select(item => $"<href={HyperlinkCategory.Line}:{item.Oid}><color=145,197,242>{item.LineNumber}</color></href>"));
                    superToolTip.Items.Add(itemLines);
                }
                superToolTip.Items.Add(new ToolTipSeparatorItem());
                
                //Tie Points
                var TiePointCount = pid.TiePoints.Count;
                ToolTipTitleItem titleItemTiePoints = new ToolTipTitleItem();
                titleItemTiePoints.Text = $"Tie Points ({TiePointCount})";
                superToolTip.Items.Add(titleItemTiePoints);
                if (TiePointCount>0)
                {
                    ToolTipItem itemTiePoints = new ToolTipItem();
                    itemTiePoints.Text = String.Join(", ", pid.TiePoints.Select(item => $"<href={HyperlinkCategory.TiePoint}:{item.Oid}><color=145,197,242>{item.Number}</color></href>"));
                    superToolTip.Items.Add(itemTiePoints);
                }
                //superToolTip.Items.Add(new ToolTipSeparatorItem());
                
                // Equipment
                //ToolTipTitleItem titleItemEquipment = new ToolTipTitleItem();
                //titleItemEquipment.Text = "Equipment";
                //ToolTipItem itemEquipment = new ToolTipItem();
                //itemEquipment.Text = $"Count: "; //{pid.Equipment.Count}";
                //superToolTip.Items.Add(titleItemEquipment);
                //superToolTip.Items.Add(itemEquipment);
                //superToolTip.Items.Add(new ToolTipSeparatorItem());

                info.SuperTip = superToolTip;
            }
            if (info != null)
                e.Info = info;
        }

        private enum HyperlinkCategory
        {
            OpenFile,
            OpenRecord,
            Instrument,
            Line,
            TiePoint,
            Equipment
        }
    }
}
