using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.Utils;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Base.ViewInfo;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using LPO_XAF_v2._0.Module.BusinessObjects.Instrument;
using System;
using System.Collections.Generic;
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
            throw new NotImplementedException();
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
                
                string text = $"Tie Points: {pid.TiePoints.Count}, Instrument: {pid.Instruments.Count}, Lines: {pid.Lines.Count}";
                info = new ToolTipControlInfo();
                info.Object = o;
                SuperToolTip superToolTip = new SuperToolTip();
                
                // Instruments
                var InstrumentCount = pid.Instruments.Count;
                ToolTipTitleItem titleItemInstruments = new ToolTipTitleItem();
                titleItemInstruments.Text = $"Instruments ({InstrumentCount})";
                superToolTip.Items.Add(titleItemInstruments);
                if (InstrumentCount > 0)
                {
                    ToolTipItem itemInstruments = new ToolTipItem();
                    itemInstruments.Text = String.Join(", ", pid.Instruments.Select(item => $"<href=Instrument:{item.Oid}><color=145,197,242>{item.TagNumber}</color></href>"));
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
                    itemLines.Text = String.Join(", ", pid.Lines.Select(item => $"<href=Line:{item.Oid}><color=145,197,242>{item.LineNumber}</color></href>"));
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
                    itemTiePoints.Text = String.Join(", ", pid.TiePoints.Select(item => $"<href=TiePoint:{item.Oid}><color=145,197,242>{item.Number}</color></href>"));
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
    }
}
