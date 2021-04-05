using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FromPicToText
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string fileDirecotry;
        string txtFile = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\image.txt";
        string content;
        Thread converter;
        public MainWindow()
        {
            InitializeComponent();
            fileCheck();
            pixelPercent.Value = 0;
            perventValue.Text = "0%";
            Window.GetWindow(this).Title = "FPTT " + Assembly.GetExecutingAssembly().GetName().ToString().Split('=')[1].Split(',')[0];
            if (File.Exists(txtFile))
                if (File.ReadAllText(txtFile).ToString() != "" && File.ReadLines(txtFile).First() == "DECREASE THE CHARACTER SIZE ;)")
                    Process.Start(txtFile);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            content = "";
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = "";
            dialog.IsFolderPicker = false;
            perventValue.Text = "0%";
            pixelPercent.Value = 0;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                fileDirecotry = dialog.FileName;
            }
            else
                return;

            switch (CB_Method.Text)
            {
                case "Char with Brightness":
                    converter = new Thread(new ThreadStart(imageBrightnessCharControl));
                    break;
                case "Points with Brightness":
                    converter = new Thread(new ThreadStart(imageBrightnessPointControl));
                    break;
                case "Char with RGB":
                    converter = new Thread(new ThreadStart(imageRGBCharControl));
                    break;
                case "Points with RGB":
                    converter = new Thread(new ThreadStart(imageRGBPointControl));
                    break;
                default:
                    MessageBox.Show("you have to select the render method!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
            }

            converter.Start();
            ((Button)sender).IsEnabled = false;
        }

        private void imageBrightnessCharControl()
        {

            try
            {
                // Retrieve the image.
                var image1 = new Bitmap(@"" + fileDirecotry, true);
                double pixelCont = 0;
                double pixelImage = image1.Height * image1.Width;
                int x = 0, y = 0;

                // Loop through the images pixels to reset color.
                for (x = 0; x < image1.Height; x++)
                {
                    for (y = 0; y < image1.Width; y++)
                    {
                        System.Drawing.Color pixelColor = image1.GetPixel(y, x);
                        pixelCont++;
                        textBrightnessChar(pixelColor);
                        pixelPercent.Dispatcher.BeginInvoke((Action)(() =>
                        {
                            pixelPercent.Value = (pixelCont / pixelImage) * 100;
                            perventValue.Text = ((int)pixelPercent.Value).ToString() + "%";
                        }));
                    }
                    content += "\n";
                }
                writer(content);
                Process.Start(txtFile);
                LoadButton.Dispatcher.BeginInvoke((Action)(() =>
                {
                    LoadButton.IsEnabled = true;
                }));
            }
            catch (ArgumentException e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void imageRGBCharControl()
        {

            try
            {
                // Retrieve the image.
                var image1 = new Bitmap(@"" + fileDirecotry, true);
                double pixelCont = 0;
                double pixelImage = image1.Height * image1.Width;
                int x = 0, y = 0;

                // Loop through the images pixels to reset color.
                for (x = 0; x < image1.Height; x++)
                {
                    for (y = 0; y < image1.Width; y++)
                    {
                        System.Drawing.Color pixelColor = image1.GetPixel(y, x);
                        pixelCont++;
                        textRGBChar(pixelColor);
                        pixelPercent.Dispatcher.BeginInvoke((Action)(() =>
                        {
                            pixelPercent.Value = (pixelCont / pixelImage) * 100;
                            perventValue.Text = ((int)pixelPercent.Value).ToString() + "%";
                        }));
                    }
                    content += "\n";
                }
                writer(content);
                Process.Start(txtFile);
                LoadButton.Dispatcher.BeginInvoke((Action)(() =>
                {
                    LoadButton.IsEnabled = true;
                }));
            }
            catch (ArgumentException e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void imageBrightnessPointControl()
        {

            try
            {
                // Retrieve the image.
                var image1 = new Bitmap(@"" + fileDirecotry, true);
                double pixelCont = 0;
                double pixelImage = image1.Height * image1.Width;
                int x = 0, y = 0;

                // Loop through the images pixels to reset color.
                for (x = 0; x < image1.Height; x++)
                {
                    for (y = 0; y < image1.Width; y++)
                    {
                        System.Drawing.Color pixelColor = image1.GetPixel(y, x);
                        pixelCont++;
                        textBrightnessPoint(pixelColor);
                        pixelPercent.Dispatcher.BeginInvoke((Action)(() =>
                        {
                            pixelPercent.Value = (pixelCont / pixelImage) * 100;
                            perventValue.Text = ((int)pixelPercent.Value).ToString() + "%";
                        }));
                    }
                    content += "\n";
                }
                writer(content);
                Process.Start(txtFile);
                LoadButton.Dispatcher.BeginInvoke((Action)(() =>
                {
                    LoadButton.IsEnabled = true;
                }));
            }
            catch (ArgumentException e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void imageRGBPointControl()
        {

            try
            {
                // Retrieve the image.
                var image1 = new Bitmap(@"" + fileDirecotry, true);
                double pixelCont = 0;
                double pixelImage = image1.Height * image1.Width;
                int x = 0, y = 0;

                // Loop through the images pixels to reset color.
                for (x = 0; x < image1.Height; x++)
                {
                    for (y = 0; y < image1.Width; y++)
                    {
                        System.Drawing.Color pixelColor = image1.GetPixel(y, x);
                        pixelCont++;
                        textRGBPoint(pixelColor);
                        pixelPercent.Dispatcher.BeginInvoke((Action)(() =>
                        {
                            pixelPercent.Value = (pixelCont / pixelImage) * 100;
                            perventValue.Text = ((int)pixelPercent.Value).ToString() + "%";
                        }));
                    }
                    content += "\n";
                }
                writer(content);
                Process.Start(txtFile);
                LoadButton.Dispatcher.BeginInvoke((Action)(() =>
                {
                    LoadButton.IsEnabled = true;
                }));
            }
            catch (ArgumentException e)
            {
                MessageBox.Show(e.Message);
            }
        }

        void textBrightnessChar(System.Drawing.Color pixel)
        {
            if (pixel.GetBrightness() <= 0.20)
                content += "██";
            else
            if (pixel.GetBrightness() <= 0.40)
                content += "▓▓";
            else
            if (pixel.GetBrightness() <= 0.60)
                content += "▒▒";
            else
            if (pixel.GetBrightness() <= 0.80)
                content += "░░";
            else
            if (pixel.GetBrightness() <= 1)
                content += "  ";
        }

        void textBrightnessPoint(System.Drawing.Color pixel)
        {
            if (pixel.GetBrightness() <= 0.20)
                content += "##";
            else
            if (pixel.GetBrightness() <= 0.40)
                content += "==";
            else
            if (pixel.GetBrightness() <= 0.60)
                content += "--";
            else
            if (pixel.GetBrightness() <= 0.80)
                content += "..";
            else
            if (pixel.GetBrightness() <= 1)
                content += "  ";
        }

        void textRGBPoint(System.Drawing.Color pixel)
        {
            if (pixel.R > pixel.G && pixel.R > pixel.B)
                content += "..";
            else
            if (pixel.G > pixel.R && pixel.G > pixel.B)
                content += "==";
            else
            if (pixel.B > pixel.G && pixel.B > pixel.R)
                content += "--";
            else
            if (pixel.R == pixel.G && pixel.R == pixel.B && pixel.R <= 127)
                content += "##";
            else
            if (pixel.R == pixel.G && pixel.R == pixel.B && pixel.R > 127)
                content += "  ";
        }

        void textRGBChar(System.Drawing.Color pixel)
        {
            if (pixel.R > pixel.G && pixel.R > pixel.B)
                content += "▓▓";
            else
            if (pixel.G > pixel.R && pixel.G > pixel.B)
                content += "▒▒";
            else
            if (pixel.B > pixel.G && pixel.B > pixel.R)
                content += "░░";
            else
            if (pixel.R == pixel.G && pixel.R == pixel.B && pixel.R <= 127)
                content += "██";
            else
            if (pixel.R == pixel.G && pixel.R == pixel.B && pixel.R > 127)
                content += "  ";
        }

        void writer(string textLines)
        {
            using (StreamWriter writer = new StreamWriter(txtFile))
            {
                foreach (char ln in textLines.ToCharArray())
                {
                    writer.Write(ln);
                }
            }
        }

        void fileCheck()
        {
            if (!File.Exists(txtFile))
            {
                File.Create(txtFile);
            }
        }

        private void LastButton_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(txtFile))
                Process.Start(txtFile);
            else
                MessageBox.Show("there is not a recent file", "Last file not found", MessageBoxButton.OK, MessageBoxImage.Exclamation);

        }

        private void SupportButton_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.paypal.com/paypalme/MeneBot");
        }

        private void openGithub(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/Mene-hub");
        }
    }
}
