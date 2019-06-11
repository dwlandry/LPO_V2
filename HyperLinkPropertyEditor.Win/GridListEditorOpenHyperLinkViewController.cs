using System;
using System.Drawing;
using DevExpress.ExpressApp;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors.Repository;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;

namespace HyperLinkPropertyEditor.Win {
	public class GridListEditorOpenHyperLinkViewController : ViewController<ListView> {
		private GridListEditor gridListEditor;
		protected override void OnViewControlsCreated() {
			base.OnViewControlsCreated();
			gridListEditor = ((ListView)View).Editor as GridListEditor;
			if(gridListEditor != null && gridListEditor.GridView != null) {
                gridListEditor.GridView.Click += GridView_Click;
                //gridListEditor.GridView.MouseDown += GridView_MouseDown;
			}
		}

        private void GridView_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MouseEventArgs me = (System.Windows.Forms.MouseEventArgs)e;
            GridView gv = (GridView)sender;
            GridHitInfo hi = gv.CalcHitInfo(new Point(me.X, me.Y));
            if (hi.InRowCell && me.Button == System.Windows.Forms.MouseButtons.Left)
            {
                RepositoryItemHyperLinkEdit repositoryItemHyperLinkEdit = hi.Column.ColumnEdit as RepositoryItemHyperLinkEdit;
                if (repositoryItemHyperLinkEdit != null)
                {
                    HyperLinkEdit editor = (HyperLinkEdit)repositoryItemHyperLinkEdit.CreateEditor();
                    editor.ShowBrowser(WinHyperLinkStringPropertyEditor.GetResolvedUrl(gv.GetRowCellValue(hi.RowHandle, hi.Column)));
                }
            }
        }

        protected override void OnDeactivated() {
			if(gridListEditor != null && gridListEditor.GridView != null) {
				//gridListEditor.GridView.MouseDown -= GridView_MouseDown;
                gridListEditor.GridView.Click -= GridView_Click;
			}
			base.OnDeactivated();
		}
		//private void GridView_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e) {
  //          GridView gv = (GridView)sender;
		//	GridHitInfo hi = gv.CalcHitInfo(new Point(e.X, e.Y));
		//	if(hi.InRowCell && e.Button == System.Windows.Forms.MouseButtons.Left) {
		//		RepositoryItemHyperLinkEdit repositoryItemHyperLinkEdit = hi.Column.ColumnEdit as RepositoryItemHyperLinkEdit;
		//		if(repositoryItemHyperLinkEdit != null) {
		//			HyperLinkEdit editor = (HyperLinkEdit)repositoryItemHyperLinkEdit.CreateEditor();
		//			editor.ShowBrowser(WinHyperLinkStringPropertyEditor.GetResolvedUrl(gv.GetRowCellValue(hi.RowHandle, hi.Column)));
		//		}
		//	}
		//}
	}
}