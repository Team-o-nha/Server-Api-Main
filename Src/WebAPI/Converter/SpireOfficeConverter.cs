using Spire.Doc;
using Spire.Pdf;
using Spire.Pdf.Graphics;
using Spire.Presentation;
using Spire.Xls;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace ColabSpace.WebAPI.Converter
{
    public class SpireOfficeConverter
    {
        public static byte[] Word2Images(Stream inputFile)
        {
            //Load Document
            Document document = new Document();
            document.LoadFromStream(inputFile, Spire.Doc.FileFormat.Auto);
            //Convert Word to Pdf
            MemoryStream stream = new MemoryStream();
            document.SaveToStream(stream, Spire.Doc.FileFormat.PDF);

            return Pdf2Images(stream);
        }

        public static byte[] Excel2Images(Stream inputFile)
        {
            //Load Workbook
            Workbook workbook = new Workbook();
            workbook.LoadFromStream(inputFile);
            Worksheet sheet = workbook.Worksheets[0];

            //Convert Excel to Pdf
            MemoryStream stream = new MemoryStream();
            sheet.SaveToPdfStream(stream);

            return Pdf2Images(stream);
        }

        public static byte[] PowerPoint2Images(Stream inputFile, string filename)
        {
            // TODO Don't write file next time
            using (var fileStream = File.Create(filename))
            {
                inputFile.Seek(0, SeekOrigin.Begin);
                inputFile.CopyTo(fileStream);
            }

            Presentation presentation = new Presentation();
            presentation.LoadFromFile(filename);

            if (presentation.Slides.Count > 0)
            {
                //save the slide to Image
                Image image = presentation.Slides[0].SaveAsImage();

                using var memoryStream = new MemoryStream();
                image.Save(memoryStream, ImageFormat.Png);
                
                return memoryStream.ToArray();
            }

            return null;
        }

        public static byte[] Text2Images(Stream inputFile, string filename)
        {
            // TODO Don't write file next time
            using (var fileStream = File.Create(filename))
            {
                inputFile.Seek(0, SeekOrigin.Begin);
                inputFile.CopyTo(fileStream);
            }

            string text = File.ReadAllText(filename);
            PdfDocument doc = new PdfDocument();
            PdfSection section = doc.Sections.Add();
            PdfPageBase page = section.Pages.Add();
            PdfFont font = new PdfFont(PdfFontFamily.Helvetica, 11);
            PdfStringFormat format = new PdfStringFormat();
            format.LineSpacing = 20f;
            PdfBrush brush = PdfBrushes.Black;
            PdfTextWidget textWidget = new PdfTextWidget(text, font, brush);
            float y = 0;
            PdfTextLayout textLayout = new PdfTextLayout();
            textLayout.Break = PdfLayoutBreakType.FitPage;
            textLayout.Layout = PdfLayoutType.Paginate;
            RectangleF bounds = new RectangleF(new PointF(0, y), page.Canvas.ClientSize);
            textWidget.StringFormat = format;
            textWidget.Draw(page, bounds, textLayout);
            
            MemoryStream stream = new MemoryStream();
            doc.SaveToStream(stream, Spire.Pdf.FileFormat.PDF);

            return Pdf2Images(stream);
        }

        public static byte[] Image2Images(Stream inputFile)
        {
            // Create a pdf document with a section and page added.
            PdfDocument doc = new PdfDocument();
            PdfSection section = doc.Sections.Add();
            PdfPageBase page = doc.Pages.Add();
            //Load a tiff image from system
            PdfImage image = PdfImage.FromStream(inputFile);
            //Set image display location and size in PDF
            float widthFitRate = image.PhysicalDimension.Width / page.Canvas.ClientSize.Width;
            float heightFitRate = image.PhysicalDimension.Height / page.Canvas.ClientSize.Height;
            float fitRate = Math.Max(widthFitRate, heightFitRate);
            float fitWidth = image.PhysicalDimension.Width / fitRate;
            float fitHeight = image.PhysicalDimension.Height / fitRate;
            page.Canvas.DrawImage(image, 30, 30, fitWidth, fitHeight);
            //save and launch the file
            MemoryStream stream = new MemoryStream();
            doc.SaveToStream(stream, Spire.Pdf.FileFormat.PDF);
            doc.Close();
            
            return Pdf2Images(stream);
        }

        public static byte[] Pdf2Images(Stream stream) {
            // Convert Pdf to images
            PdfDocument doc = new PdfDocument();
            doc.LoadFromStream(stream);
            Image emf = doc.SaveAsImage(0, PdfImageType.Bitmap);

            using var memoryStream = new MemoryStream();
            emf.Save(memoryStream, ImageFormat.Png);

            return memoryStream.ToArray();
        }
    }
}
