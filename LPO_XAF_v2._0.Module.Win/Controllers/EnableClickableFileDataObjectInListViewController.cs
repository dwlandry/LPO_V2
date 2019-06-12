//-----------------------------------------------------------------------
// <copyright file="F:\Users\dlandry\source\repos\LPO_XAF_v2.0\LPO_XAF_v2._0.Module.Win\Controllers\EnableClickableFileDataObjectInListViewController.cs" company="David W. Landry III">
//     Author: _**David Landry**_
//     *Copyright (c) David W. Landry III. All rights reserved.*
// </copyright>
//-----------------------------------------------------------------------
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.Persistent.BaseImpl;
using System;
using System.Drawing;
using System.Linq;

namespace LPO_XAF_v2._0.Module.Win.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class EnableClickableFileDataObjectInListViewController : ViewController
    {
        public EnableClickableFileDataObjectInListViewController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
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
            var controller = Frame.GetController<ListViewProcessCurrentObjectController>();
            if (controller != null)
                controller.CustomProcessSelectedItem += new EventHandler<CustomProcessListViewSelectedItemEventArgs>(controller_CustomProcessSelectedItem);
            if (View is ListView)
            {
                GridListEditor columnsListEditor = ((ListView)View).Editor as GridListEditor;
                if (columnsListEditor != null)
                {
                    foreach (XafGridColumnWrapper columnWrapper in columnsListEditor.Columns)
                    {
                        if (columnWrapper.GridColumnInfo.ModelMember.MemberInfo.MemberType == typeof(FileData))
                        {
                            columnWrapper.Column.AppearanceCell.ForeColor = Color.Blue;
                            columnWrapper.Column.AppearanceCell.Font = new Font(columnWrapper.Column.AppearanceCell.Font, FontStyle.Underline);
                        }
                    }
                }
            }

        }
        void controller_CustomProcessSelectedItem(object sender, CustomProcessListViewSelectedItemEventArgs e)
        {
            GridListEditor columnsListEditor = ((ListView)View).Editor as GridListEditor;
            var gv = columnsListEditor.GridView;
            if (gv != null)
            {
                var hi = gv.CalcHitInfo(gv.GridControl.PointToClient(System.Windows.Forms.Control.MousePosition));
                foreach (XafGridColumnWrapper columnWrapper in columnsListEditor.Columns)
                {
                    if (columnWrapper.GridColumnInfo.ModelMember.MemberInfo.MemberType == typeof(FileData) && columnWrapper.Column == hi.Column)
                    {
                        e.Handled = true;
                        if (hi != null && hi.HitTest == DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitTest.RowCell)
                        {
                            //MyFileObject obj = gv.GetRow(hi.RowHandle) as MyFileObject;
                            var obj = gv.GetRow(hi.RowHandle);
                            FileData fd = obj.GetType().GetProperty("File").GetValue(obj, null) as FileData;  //obj.File;
                            var tempFolder = System.IO.Path.GetTempPath();
                            var filename = System.IO.Path.Combine(tempFolder, fd.FileName);
                            System.IO.File.WriteAllBytes(filename, fd.Content);
                            System.Diagnostics.Process.Start(filename);
                        }
                    }
                }
            }
        }

        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            var controller = Frame.GetController<ListViewProcessCurrentObjectController>();
            if (controller != null)
                controller.CustomProcessSelectedItem -= new EventHandler<CustomProcessListViewSelectedItemEventArgs>(controller_CustomProcessSelectedItem);
            base.OnDeactivated();

        }
    }
}
