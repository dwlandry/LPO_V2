//-----------------------------------------------------------------------
// <copyright file="F:\Users\dlandry\Source\Repos\LPO_V2\LPO_XAF_v2._0.Module.Win\Editors\Manufacturer\ManufacturerCardListEditor.cs" company="David W. Landry III">
//     Author: _**David Landry**_
//     *Copyright (c) David W. Landry III. All rights reserved.*
// </copyright>
//-----------------------------------------------------------------------
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using System;
using System.Collections;
using System.Linq;
using System.Windows.Forms;

namespace LPO_XAF_v2._0.Module.Win.Editors.Manufacturer
{
    //[ListEditor(typeof(LPO_XAF_v2.))]
    // See http://www.eadslink.com/instrumentation/ for an example layout.
    // ![](A4D0FE23C51506C35D9C228669C4B2C4.png;;;0.02396,0.02396)


    public class ManufacturerCardListEditor : ListEditor
    {
        private System.Windows.Forms.ListView control;

        public override SelectionType SelectionType => throw new NotImplementedException();

        public override IList GetSelectedObjects()
        {
            throw new NotImplementedException();
        }

        public override void Refresh()
        {
            throw new NotImplementedException();
        }

        protected override void AssignDataSourceToControl(object dataSource)
        {
            throw new NotImplementedException();
        }

        protected override object CreateControlsCore()
        {
            control = new System.Windows.Forms.ListView();
            control.Sorting = SortOrder.Ascending;
            control.HideSelection = false;
            control.SelectedIndexChanged += new EventHandler(control_SelectedIndexChanged);
            control.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(control_ItemSelectionChanged);
            control.MouseDoubleClick += new MouseEventHandler(control_MouseDoubleClick);
            control.KeyDown += new System.Windows.Forms.KeyEventHandler(control_KeyDown);
            Refresh();
            return control;
        }
        void control_SelectedIndexChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        void control_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            throw new NotImplementedException();
        }
        void control_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            throw new NotImplementedException();
        }
        void control_KeyDown(object sender, KeyEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
