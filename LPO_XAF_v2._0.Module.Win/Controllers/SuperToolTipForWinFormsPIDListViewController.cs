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
using LPO_XAF_v2._0.Module.BusinessObjects.Mechanical;
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
                OpenDetailView<PID>(item, Application.CreateObjectSpace(typeof(PID)));
            else if (category == HyperlinkCategory.Equipment.ToString())
                OpenDetailView<Equipment>(item, Application.CreateObjectSpace(typeof(Equipment)));
            else if (category == HyperlinkCategory.Instrument.ToString())
                OpenDetailView<Instrument>(item, Application.CreateObjectSpace(typeof(Instrument)));
            else if (category == HyperlinkCategory.Line.ToString())
                OpenDetailView<Line>(item, Application.CreateObjectSpace(typeof(Line)));
            else if (category == HyperlinkCategory.TiePoint.ToString())
                OpenDetailView<PipingTiePoint>(item, Application.CreateObjectSpace(typeof(PipingTiePoint)));
        }

        private void OpenDetailView<T>(string item, IObjectSpace space)
        {
            var businessObject = space.FindObject<T>(new BinaryOperator("Oid", item));
            ShowViewParameters svp = new ShowViewParameters();
            DetailView dv = Application.CreateDetailView(space, businessObject);
            svp.CreatedView = dv;
            Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(null, null));
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

                AddDataToSuperToolTip(superToolTip, "Instruments", HyperlinkCategory.Instrument, pid.Instruments.ToDictionary(x => x.Oid, x => x.TagNumber));
                AddDataToSuperToolTip(superToolTip, "Lines", HyperlinkCategory.Line, pid.Lines.ToDictionary(x => x.Oid, x => x.LineNumber));
                AddDataToSuperToolTip(superToolTip, "Tie Points", HyperlinkCategory.TiePoint, pid.TiePoints.ToDictionary(x => x.Oid, x => x.Number));
                AddDataToSuperToolTip(superToolTip, "Equipment", HyperlinkCategory.Equipment, pid.Equipment.ToDictionary(x => x.Oid, x => x.Name));

                info.SuperTip = superToolTip;
            }
            if (info != null)
                e.Info = info;
        }

        private static void AddDataToSuperToolTip(SuperToolTip superToolTip, string categoryName,
            HyperlinkCategory category, Dictionary<Guid, string> dictionary)
        {
            int itemCount = dictionary.Count();
            ToolTipTitleItem titleItemEquipment = new ToolTipTitleItem();
            titleItemEquipment.Text = $"{categoryName} ({itemCount})";
            superToolTip.Items.Add(titleItemEquipment);
            if (itemCount > 0)
            {
                ToolTipItem toolTipItem = new ToolTipItem();
                toolTipItem.Text = String.Join(", ", dictionary.Select(item => $"<href={category}:{item.Key}><color=145,197,242>{item.Value}</color></href>"));
                superToolTip.Items.Add(toolTipItem);
            }
            superToolTip.Items.Add(new ToolTipSeparatorItem());
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
