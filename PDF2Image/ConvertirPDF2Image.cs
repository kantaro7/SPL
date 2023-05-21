using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Telerik.Windows.Documents.Fixed.FormatProviders;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.Model;
using Telerik.Windows.Documents.Fixed.Model.Collections;
using Telerik.Windows.Documents.Fixed.Utilities.Rendering;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Docx;
using Telerik.Windows.Documents.Lists;
using Telerik.Windows.Documents.Model;
using Telerik.Windows.Documents.UI;

namespace PDF2Image
{
    public static class ConvertirPDF2Image
    {
        public static List<byte[]> PDF2ImageConverter(string base64, double filas, string nombre)
        {
            List<byte[]> iamges = new List<byte[]>();
            double x = 0;
            double w = 700;
            double h = 800;
            double y = 0;
            bool continuar = true;
            try
            {
                /*RadDocument document = new RadDocument();
                document.LayoutMode = DocumentLayoutMode.Paged;

                Section cover = new Section();
                Paragraph title = new Paragraph();
                title.Inlines.Add(new Span("Sample Word Document Test") { FontSize = 30 });
                cover.Blocks.Add(title);
                document.Sections.Add(cover);

                Section tocSection = new Section();
                document.Sections.Add(tocSection);

                ListStyle newListStyle = DefaultListStyles.Numbered;
                DocumentList documentList = new DocumentList(newListStyle, document);

                Section content = new Section();
                Paragraph heading = new Paragraph() { StyleName = RadDocumentDefaultStyles.GetHeadingStyleNameByIndex(1) };
                heading.ListId = documentList.ID;

                heading.Inlines.Add(new Span("Section A Heading"));
                content.Blocks.Add(heading);

                Paragraph body = new Paragraph();
                body.Inlines.Add(new Span("Lorem ipsum dolor sit amet, consectetur adipiscing elit. In in elementum ipsum. Duis vel vulputate massa, eget iaculis urna. Morbi feugiat, magna eget accumsan mollis, leo lectus porta diam, id sollicitudin mi tellus nec tortor. Nullam lacinia consequat blandit. Sed tincidunt pulvinar ultricies. Interdum et malesuada fames ac ante ipsum primis in faucibus. Praesent nec convallis nunc. Maecenas fermentum, dolor sed egestas aliquet, diam sem tempus nulla, sed vehicula ipsum metus ut odio. Proin commodo malesuada justo in mollis. Nullam et blandit est, ac dapibus tortor. Aliquam ligula mauris, sodales vitae gravida a, bibendum eget arcu."));
                content.Blocks.Add(body);

                Paragraph heading2 = new Paragraph() { StyleName = RadDocumentDefaultStyles.GetHeadingStyleNameByIndex(2) };
                heading2.Inlines.Add(new Span("Subsection A1"));
                heading2.ListId = documentList.ID;
                heading2.ListLevel = 1;

                content.Blocks.Add(heading2);

                Paragraph body2 = new Paragraph();
                body2.Inlines.Add(new Span("Proin sodales aliquam lorem ac laoreet. Integer diam lorem, cursus at arcu sed, ornare luctus diam. Maecenas a blandit sem. Donec quam nunc, euismod quis quam vel, pulvinar rhoncus urna. Duis ornare magna mi, id commodo sem pulvinar et. Quisque adipiscing diam purus, nec posuere eros fringilla non. Nam a dictum lacus. In sit amet dignissim est. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Suspendisse potenti"));
                content.Blocks.Add(body2);

                document.Sections.Add(content);

                TableOfContentsField field = new TableOfContentsField();

                RadDocumentEditor documentEditor = new RadDocumentEditor(document);
                documentEditor.Document.CaretPosition.MoveToStartOfDocumentElement(tocSection);
                documentEditor.InsertField(field, FieldDisplayMode.Result);

                documentEditor.UpdateAllFields(FieldDisplayMode.Result);

                DocxFormatProvider provider = new DocxFormatProvider();
                using (MemoryStream output = new MemoryStream())
                {
                    provider.Export(document, output);
                    byte[] a = output.ToArray();
                    System.IO.File.WriteAllBytes(@"C:\Users\Barboza\Downloads\test.docx", a);
                }*/
                
                Thread t = new Thread(() =>
                {
                    Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.PdfFormatProvider docProvider2 = new Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.PdfFormatProvider();
                    Telerik.Windows.Documents.Fixed.Model.RadFixedDocument documen2 = new Telerik.Windows.Documents.Fixed.Model.RadFixedDocument();

                    var file = Convert.FromBase64String(base64);
                    byte[] data = null;

                    try
                    {
                        documen2 = docProvider2.Import(file);
                        data = docProvider2.Export(documen2);

                    }
                    catch (Exception e)
                    {
                        continuar = false;
                    }

                    PdfImportSettings settings = new
                    PdfImportSettings();
                    settings.ReadingMode = ReadingMode.OnDemand;
                    PdfFormatProvider provider = new PdfFormatProvider();
                    provider.ImportSettings = settings;
                    RadFixedDocument doc = provider.Import(data);
                    ThumbnailFactory factory = new ThumbnailFactory();
                    PageCollection pages = doc.Pages;
                    var lineas = System.IO.File.ReadAllLines(@"C:\Users\Barboza\Downloads\doc.txt");
                    int i = 0;
                    if (continuar)
                    {
                        foreach (var page in pages)
                        {
                            BitmapEncoder encoder = new JpegBitmapEncoder();
                            System.Windows.Controls.Image image = new System.Windows.Controls.Image();
                            System.Windows.Media.ImageSource imageSource = factory.CreateThumbnail(page, page.Size);
                            image.Source = imageSource;
                            Grid container = new Grid();
                            container.Background = System.Windows.Media.Brushes.White;
                            container.Children.Add(image);
                            container.Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
                            container.Arrange(new Rect(new System.Windows.Point(0, 0), container.DesiredSize));
                            var u = new Rect(new System.Windows.Point(0, 0), container.DesiredSize);
                            w = (int)PageLayoutHelper.GetActualWidth(pages[0]);
                            h = (int)PageLayoutHelper.GetActualHeight(pages[0]);
                            x = u.X;
                            y = u.Y;
                            RenderTargetBitmap bitmap = new RenderTargetBitmap((int)w, (int)h, 0, 0, PixelFormats.Pbgra32);
                            bitmap.Render(container);
                            bitmap.Render(container);
                            encoder.Frames.Add(BitmapFrame.Create(bitmap));
                            Stream guardar = new MemoryStream();
                            encoder.Save(guardar);



                            try
                            {
                                using (MemoryStream memory = new MemoryStream())
                                {
                                     Bitmap croppedBitmap = new Bitmap(guardar);
                                     croppedBitmap = croppedBitmap.Clone(
                                                     new Rectangle(0,120, (int)w, (int)h-350),
                                                     System.Drawing.Imaging.PixelFormat.DontCare);

                                     croppedBitmap.Save(memory, ImageFormat.Jpeg);
                                     byte[] bytes = memory.ToArray();

                                     iamges.Add(bytes);
                                }

                            }
                            catch (Exception e2)
                            {

                            }


                            int porcentaje = 0;
                            if (filas == 59)
                            {
                                porcentaje = 220;
                            }
                            else if (filas == 69)
                            {
                                porcentaje = 250;
                            }
                            else if (filas >= 69)
                            {
                                var div = (filas / (filas / 69));
                                if (div >= 65)
                                {
                                    porcentaje = 120;
                                }
                            }

                            System.Drawing.Rectangle ab = new System.Drawing.Rectangle(0, 120, (int)w, (int)h - porcentaje);
                            Bitmap bmpImage = new Bitmap(guardar);

                            var p = bmpImage.Clone(ab, bmpImage.PixelFormat);

                            using (MemoryStream memory = new MemoryStream())
                            {
                                p.Save(memory, ImageFormat.Jpeg);
                                byte[] bytes = memory.ToArray();
                                // System.IO.File.WriteAllBytes(@"C:\Users\Barboza\Downloads\img" + i + ".jpg", bytes);
                                iamges.Add(bytes);
                            }
                            i++;

                        }
                    }
                  


                });

                t.SetApartmentState(ApartmentState.STA);
                t.Start();
                t.Join();



                return iamges;
            }
            catch (Exception e)
            {
                return new List<byte[]>();
            }
            

        }


       
    }
}
