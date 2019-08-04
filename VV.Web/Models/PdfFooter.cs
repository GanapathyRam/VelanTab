using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace VV.Web.Models
{
    public partial class PdfFooter : PdfPageEventHelper
    {
        public string InspectedBy { get; set; }

        //public override void OnEndPage(PdfWriter writer, Document doc)
        //{
        //    var inspectedBy = Convert.ToString(InspectedBy);

        //    Paragraph footer = new Paragraph("THANK YOU", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL));

        //    footer.Alignment = Element.ALIGN_RIGHT;

        //    PdfPTable footerTbl = new PdfPTable(1);

        //    footerTbl.TotalWidth = 300;

        //    footerTbl.HorizontalAlignment = Element.ALIGN_CENTER;

        //    PdfPCell cell = new PdfPCell(footer);

        //    //cell.Border = 0;

        //    //cell.PaddingLeft = 10;

        //    cell.BorderColor = BaseColor.BLACK;

        //    cell.Border = iTextSharp.text.Rectangle.BOX;

        //    footerTbl.AddCell(cell);

        //    footerTbl.WriteSelectedRows(0, -1, 415, 30, writer.DirectContent);

        //}

        //public override void OnOpenDocument(PdfWriter writer, Document document)
        //{
        //    base.OnOpenDocument(writer, document);
        //}

        //public override void OnStartPage(PdfWriter writer, Document document)
        //{
        //    string image2 = HttpContext.Current.Server.MapPath("../Image/logo_velan.png");
        //    float cellHeight = document.TopMargin;

        //    iTextSharp.text.Image jpg2 = iTextSharp.text.Image.GetInstance(image2);

        //    //jpg.ScaleToFit(280f, 400f);
        //    //jpg.Alignment = Element.ALIGN_TOP;
        //    PdfPTable Headertbl = new PdfPTable(3);

        //    Headertbl.TotalWidth = document.PageSize.Width;
        //    Headertbl.SpacingBefore = 2f;
        //    Headertbl.SpacingBefore = 2f;
        //    Headertbl.WidthPercentage = 90;

        //    PdfPCell cell11 = new PdfPCell();

        //    //cell11.AddElement(jpg);
        //    cell11.Border = 0;
        //    cell11.VerticalAlignment = Element.ALIGN_LEFT;

        //    PdfPCell cell12 = new PdfPCell();
        //    cell12.Border = 0;

        //    PdfPCell cell13 = new PdfPCell();
        //    //cell13.AddElement(new Paragraph(“”));
        //    //cell13.AddElement(new Paragraph(cellHeight + “Cell Height”));

        //    //cell13.AddElement(new Paragraph(” Last Updated”));

        //    //cell13.AddElement(new Paragraph(” ” +DateTime.Now.ToString()));
        //    cell13.Border = 0;

        //    Headertbl.AddCell(cell11);
        //    Headertbl.AddCell(cell12);
        //    Headertbl.AddCell(cell13);

        //    document.Add(Headertbl);

        //    base.OnStartPage(writer, document);
        //}

        public override void OnEndPage(PdfWriter writer, Document doc)
        {
            PdfPTable table = null;
            table = new PdfPTable(5);
            table.TotalWidth = 500f;
            table.LockedWidth = true;

            //table.HeaderRows = 1;

            //PdfPCell header = new PdfPCell(new Phrase("Inspector Sign   :  " + InspectedBy + "", FontFactory.GetFont("Arial", BaseFont.WINANSI, BaseFont.EMBEDDED, 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
            //header.Colspan = 5;
            ////header.PaddingTop = 30f;
            //header.FixedHeight = 45f;
            //header.VerticalAlignment = 1;
            //header.BorderWidth = 0.5f;
            //table.AddCell(header);
            //table.WriteSelectedRows(0, -1, doc.Left, doc.Top, writer.DirectContent);

            PdfPCell footer = new PdfPCell(new Phrase("Inspector Sign   :  "+ InspectedBy + "", FontFactory.GetFont("Arial", BaseFont.WINANSI, BaseFont.EMBEDDED, 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
            footer.Colspan = 5;
            footer.PaddingTop = 30f;
            footer.FixedHeight = 45f;
            footer.VerticalAlignment = 1;
            //footer.BorderWidth = 0.5f;
            table.AddCell(footer);

            table.WriteSelectedRows(0, -1, 47, 50, writer.DirectContent);
        }

        public override void OnStartPage(PdfWriter writer, Document document)
        {
            PdfPCell cell = null;
            PdfPTable table = null;

            Rectangle pageSize = document.PageSize;
            table = new PdfPTable(5);
            table.TotalWidth = 500f;
            table.LockedWidth = true;

            //table.DefaultCell.BorderWidth = 0.5f;
            cell = ImageCell("../Image/logo_velan.png", 4.5f, PdfPCell.ALIGN_LEFT);
            cell.FixedHeight = 45f;
            cell.VerticalAlignment = 0;
            cell.PaddingLeft = 2f;
            cell.PaddingTop = 10f;
            cell.BorderWidth = 0.5f;
            cell.BorderColor = BaseColor.BLACK;
            cell.Border = iTextSharp.text.Rectangle.BOX;
            cell.Colspan = 1;
            table.AddCell(cell);

            PdfPCell header = new PdfPCell(new Phrase("PATROL INSPECTION REPORT", FontFactory.GetFont("Arial", BaseFont.WINANSI, BaseFont.EMBEDDED, 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
            header.Colspan = 4;
            header.PaddingTop = 15f;
            header.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            table.AddCell(header);

            //string date = DateTime.Now.ToString("MMMMM dd, yyyy");
            //Paragraph header = new Paragraph(date, FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.BOLD, BaseColor.WHITE));
            //header.Alignment = Element.ALIGN_LEFT;
            //PdfPTable headerTbl = new PdfPTable(1);
            //headerTbl.TotalWidth = 600;
            //headerTbl.HorizontalAlignment = Element.ALIGN_LEFT;
            //PdfPCell cell = new PdfPCell(header);
            //cell.Border = 0;
            //cell.PaddingLeft = 10;
            //cell.BackgroundColor = BaseColor.GRAY;
            //headerTbl.AddCell(cell);
            table.WriteSelectedRows(0, -1, pageSize.GetTop(30f), 50, writer.DirectContent);
        }

        private static PdfPCell ImageCell(string path, float scale, int align)
        {
            iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath(path));
            image.ScalePercent(scale);
            PdfPCell cell = new PdfPCell(image);
            cell.BorderColor = BaseColor.WHITE;
            cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
            cell.HorizontalAlignment = align;
            cell.PaddingBottom = 0f;
            cell.PaddingTop = 0f;
            return cell;
        }
    }
}