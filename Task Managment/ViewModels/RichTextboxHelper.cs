using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media.Imaging;

namespace Task_Managment.ViewModels
{
    public class RichTextboxHelper : DependencyObject
    {
        private static HashSet<Thread> _recursionProtection = new HashSet<Thread>();

        public static string GetDocumentXaml(DependencyObject obj)
        {
            return (string)obj.GetValue(DocumentXamlProperty);
        }

        public static void SetDocumentXaml(DependencyObject obj, string value)
        {
            _recursionProtection.Add(Thread.CurrentThread);
            obj.SetValue(DocumentXamlProperty, value);
            _recursionProtection.Remove(Thread.CurrentThread);
        }

        public static readonly DependencyProperty DocumentXamlProperty = DependencyProperty.RegisterAttached(
            "DocumentXaml",
            typeof(string),
            typeof(RichTextboxHelper),
            new FrameworkPropertyMetadata(
                "",
                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                (obj, e) => {
                    if (_recursionProtection.Contains(Thread.CurrentThread))
                        return;

                    var richTextBox = (RichTextBox)obj;

                    string test = (string)XamlWriter.Save(richTextBox.Document);

                    // Parse the XAML to a document (or use XamlReader.Parse())
                    var xaml = GetDocumentXaml(richTextBox);

                    string _byteOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());

                    if (xaml.StartsWith(_byteOrderMarkUtf8, StringComparison.Ordinal))
                        xaml = xaml.Remove(0, _byteOrderMarkUtf8.Length);

                    try
                    {
                        FlowDocument doc;

                        using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(xaml)))
                        {
                            doc = (FlowDocument)XamlReader.Load(stream);
                        }

                        foreach (Block b in doc.Blocks)
                            foreach (Inline i in ((Paragraph)b).Inlines)
                                if (i.GetType() == typeof(InlineUIContainer))
                                {
                                    InlineUIContainer inlineUIContainer = (InlineUIContainer)i;

                                    if (inlineUIContainer.Child.GetType() == typeof(Image))
                                    {
                                        Image image = (Image)inlineUIContainer.Child;

                                        BitmapImage bmImage = new BitmapImage();

                                        if (image.Tag != null)
                                        {
                                            string base64String = image.Tag.ToString();

                                            using (MemoryStream stream = new MemoryStream(Convert.FromBase64String(base64String)))
                                            {
                                                bmImage.BeginInit();
                                                bmImage.CacheOption = BitmapCacheOption.OnLoad;
                                                bmImage.StreamSource = stream;
                                                bmImage.EndInit();
                                            }
                                        }

                                        image.Source = bmImage;
                                    }
                                }

                        // Set the document
                        richTextBox.Document = doc;
                    }
                    catch (Exception)
                    {
                        richTextBox.Document = new FlowDocument();
                    }

                    // When the document changes update the source
                    richTextBox.TextChanged += (obj2, e2) =>
                    {
                        RichTextBox richTextBox2 = obj2 as RichTextBox;
                        if (richTextBox2 != null)
                        {
                            SetDocumentXaml(richTextBox, XamlWriter.Save(richTextBox2.Document));
                        }
                    };
                }
            )
        );
    }
}
