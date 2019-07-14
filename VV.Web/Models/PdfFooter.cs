using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VV.Web.Models
{
    public partial class PdfFooter : PdfPageEventHelper
    {
        public string InspectedBy { get; set; }

        public override void OnEndPage(PdfWriter writer, Document doc)
        {
            var inspectedBy = Convert.ToString(InspectedBy);

            Paragraph footer = new Paragraph("THANK YOU", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL));

            footer.Alignment = Element.ALIGN_RIGHT;

            PdfPTable footerTbl = new PdfPTable(1);

            footerTbl.TotalWidth = 300;

            footerTbl.HorizontalAlignment = Element.ALIGN_CENTER;

            PdfPCell cell = new PdfPCell(footer);

            //cell.Border = 0;

            //cell.PaddingLeft = 10;

            cell.BorderColor = BaseColor.BLACK;

            cell.Border = iTextSharp.text.Rectangle.BOX;

            footerTbl.AddCell(cell);

            footerTbl.WriteSelectedRows(0, -1, 415, 30, writer.DirectContent);

        }
    }
}