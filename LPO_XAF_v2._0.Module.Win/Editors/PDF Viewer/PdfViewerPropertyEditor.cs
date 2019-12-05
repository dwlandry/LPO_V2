//-----------------------------------------------------------------------
// <copyright file="D:\Users\dlandry\Source\Repos\LPO_V2\LPO_XAF_v2._0.Module.Win\Editors\PDF Viewer\PdfViewerPropertyEditor.cs" company="David W. Landry III">
//     Author: _**David Landry**_
//     *Copyright (c) David W. Landry III. All rights reserved.*
// </copyright>
//-----------------------------------------------------------------------
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.XtraPdfViewer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPO_XAF_v2._0.Module.Win.Editors.PDF_Viewer
{
    [PropertyEditor(typeof(FileData), false)]
    public class PdfViewerPropertyEditor : WinPropertyEditor
    {
        public PdfViewerPropertyEditor(Type objectType, IModelMemberViewItem model) : base(objectType, model) { }

        public new PdfViewer Control => ((PdfViewer)base.Control);

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && Control != null)
                {
                    Control.DocumentChanged -= PdfViewerOnDocumentChanged;
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }
        protected override void ReadValueCore()
        {
            var fileData = PropertyValue as IFileData;
            if (fileData != null && fileData.FileName.ToLower().Contains(".pdf"))
            {
                using (var stream = new MemoryStream())
                {
                    fileData.SaveToStream(stream);
                    Control.LoadDocument(stream);
                }
            }
            else
            {
                Control.CloseDocument();
            }
        }
        protected override object CreateControlCore()
        {
            var pdfViewer = new PdfViewer() { DetachStreamAfterLoadComplete = true };
            pdfViewer.DocumentChanged += PdfViewerOnDocumentChanged;
            return pdfViewer;
        }
        private void PdfViewerOnDocumentChanged(object sender, PdfDocumentChangedEventArgs e)
        {
            //OnControlValueChanged();
            //Control.Refresh();
        }
    }
}
