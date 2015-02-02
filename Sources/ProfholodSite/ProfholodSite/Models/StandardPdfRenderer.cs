// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardPdfRenderer.cs" company="SemanticArchitecture">
//   http://www.SemanticArchitecture.net
// </copyright>
// <summary>
//   This class is responsible for rendering a html text string to a PDF document
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ReportManagement
{
    using System.IO;

    using iTextSharp.text;
    using iTextSharp.text.html.simpleparser;
    using iTextSharp.text.pdf;
    using iTextSharp.text.html;
    using System;
 
    /// <summary>
    /// This class is responsible for rendering a html text string to a PDF document using the html renderer of iTextSharp.
    /// </summary>
    /// 
   

    public class StandardPdfRenderer
    {
        private const int HorizontalMargin = 40;
        private const int VerticalMargin = 40;

        public byte[] Render(string htmlText, string pageTitle)
        {
            byte[] renderedBuffer;

           // BaseFont baseFont = BaseFont.CreateFont(@"D:\ProfholodDevelop\ProfholodSite\ProfholodSite\fonts\arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            //iTextSharp.text.Font font = new iTextSharp.text.Font(baseFont, iTextSharp.text.Font.DEFAULTSIZE, iTextSharp.text.Font.NORMAL);
            string arialuniTff = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");
            //Register the font with iTextSharp
            iTextSharp.text.FontFactory.Register(arialuniTff);

            iTextSharp.text.html.simpleparser.StyleSheet ST = new iTextSharp.text.html.simpleparser.StyleSheet();
            //Set the default body font to our registered font's internal name
            ST.LoadTagStyle(HtmlTags.BODY, HtmlTags.FACE, "Arial");
            //Set the default encoding to support Unicode characters
            ST.LoadTagStyle(HtmlTags.BODY, HtmlTags.ENCODING, BaseFont.IDENTITY_H);

            using (var outputMemoryStream = new MemoryStream())
            {
                using (var pdfDocument = new Document(PageSize.A4, HorizontalMargin, HorizontalMargin, VerticalMargin, VerticalMargin))
                {
                    PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDocument, outputMemoryStream);
                    pdfWriter.CloseStream = false;
                    pdfWriter.PageEvent = new PrintHeaderFooter { Title = pageTitle };
                    pdfDocument.Open();
                  
                    using (var htmlViewReader = new StringReader(htmlText))
                    {
                        using (var htmlWorker = new HTMLWorker(pdfDocument))
                        {
                            htmlWorker.SetStyleSheet(ST);

                            //string HJ = "Привет !!!!!!!!!!!!!!! Привет";
                            //Chunk c1 = new Chunk(HJ,font);
                           // pdfDocument.Add(c1);

                            htmlWorker.Parse(htmlViewReader);
                        }
                    }
                }

                renderedBuffer = new byte[outputMemoryStream.Position];
                outputMemoryStream.Position = 0;
                outputMemoryStream.Read(renderedBuffer, 0, renderedBuffer.Length);
            }

            return renderedBuffer;
        }
    }
}