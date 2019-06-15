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
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.XtraEditors;
using LPO_XAF_v2._0.Module.BusinessObjects.Instrument;

namespace LPO_XAF_v2._0.Module.Win.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class ContextMenusForInstrumentListViewController : ViewController
    {
        public ContextMenusForInstrumentListViewController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            TargetObjectType = typeof(Instrument);
            TargetViewType = ViewType.ListView;

            SimpleAction openDrawingAction = new SimpleAction(this, "Open Drawing", PredefinedCategory.Menu);
            openDrawingAction.ToolTip = "Open a copy of the drawing file.";
            openDrawingAction.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
            openDrawingAction.Execute += OpenDrawingAction_Execute;
            openDrawingAction.ImageName = "Open";
        }

        private void OpenDrawingAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            GridListEditor columnsListEditor = ((ListView)View).Editor as GridListEditor;
            var gv = columnsListEditor.GridView;
            if (gv != null)
            {
                var focusedValue = gv.FocusedValue;
                if (focusedValue != null)
                {
                    if (focusedValue.GetType().IsSubclassOf(typeof(Drawing)))
                    {
                        FileData fd = focusedValue.GetType().GetProperty("File").GetValue(focusedValue, null) as FileData;  //obj.File;
                        if (fd != null)
                        {
                            var tempFolder = System.IO.Path.GetTempPath();
                            var filename = System.IO.Path.Combine(tempFolder, fd.FileName);
                            if (!System.IO.File.Exists(filename))
                                System.IO.File.WriteAllBytes(filename, fd.Content);
                            System.Diagnostics.Process.Start(filename);
                        }
                        else
                        {
                            XtraMessageBox.Show($"There is no file associated with {focusedValue.ToString()}");
                        }
                    }
                }
            }
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
    }
}
