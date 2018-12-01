using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using System.Diagnostics;

namespace checkAdd
{
    class PDF
    {
        public Document me;

        public PDF()
        {
            me = new Document();
        }

        public void writeAllText(string text)
        {
            Section sec = me.AddSection();
            Paragraph p = sec.AddParagraph(text);
            p.Format.Font.ApplyFont(new Font("Arial", 12));
        }

        public void save(string fileName)
        {
            PdfDocumentRenderer docRend = new PdfDocumentRenderer(false);
            docRend.Document = me;
            docRend.RenderDocument();
            docRend.PdfDocument.Save(fileName);
        }
    }
}
