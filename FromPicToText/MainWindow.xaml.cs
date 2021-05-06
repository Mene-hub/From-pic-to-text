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
        #region Vars
        string fileDirecotry;
        string txtFile = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\image.txt";
        string brailleMap = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\brailleMapping.conf";
        string framesDirectory = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\frames.frs";
        string frames;

        string content;
        string[] args;
        Thread converter;
        #endregion

        #region char map
        string char1 = "██";
        string char2 = "▓▓";
        string char3 = "▒▒";
        string char4 = "░░";
        string char5 = "  ";
        string point1 = "##";
        string point2 = "==";
        string point3 = "--";
        string point4 = "..";
        string zero = "0";
        string uno = "1";
        string[] chars;
        string[] points;
        string[] boolean;
        void invert()
        {
            if(chars[0]!=char1)
                chars = new string[] { char1, char2, char3, char4, char5 };
            else
                chars = new string[] { char5, char4, char3, char2, char1 };

            if(points[0]!=point1)
                points = new string[] { point1, point2, point3, point4, char5 };
            else
                points = new string[] { char5, point4, point3, point2, point1 };

            if(boolean[0]==uno)
                boolean = new string[] { zero, uno };
            else
                boolean = new string[] { uno, zero };
        }
        #endregion

        #region Window region
        public MainWindow()
        {
            InitializeComponent();
            fileCheck();
            pixelPercent.Value = 0;
            perventValue.Text = "0%";
            chars = new string[] { char1, char2, char3, char4, char5 };
            points = new string[] { point1, point2, point3, point4, char5 };
            boolean = new string[] { zero, uno};
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
                case "Braille BETA":
                    converter = new Thread(new ThreadStart(brailleBrightness));

                    break;
                default:
                    MessageBox.Show("you have to select the render method!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
            }

            converter.Start();
            ((Button)sender).IsEnabled = false;
            invertBox.IsEnabled = false;
        }

        private void invertCheck(object sender, RoutedEventArgs e)
        {
            invert();
        }

        #endregion

        #region image readers
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
                    invertBox.IsEnabled = true;
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
                    invertBox.IsEnabled = true;
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
                    invertBox.IsEnabled = true;
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
                    invertBox.IsEnabled = true;
                }));
            }
            catch (ArgumentException e)
            {
                MessageBox.Show(e.Message);
            }
        }

        #endregion

        #region Render Methods
        void textBrightnessChar(System.Drawing.Color pixel)
        {
            if (pixel.GetBrightness() <= 0.20)
                content += chars[0];
            else
            if (pixel.GetBrightness() <= 0.40)
                content += chars[1];
            else
            if (pixel.GetBrightness() <= 0.60)
                content += chars[2];
            else
            if (pixel.GetBrightness() <= 0.80)
                content += chars[3];
            else
            if (pixel.GetBrightness() <= 1)
                content += chars[4];
        }

        void textBrightnessPoint(System.Drawing.Color pixel)
        {
            if (pixel.GetBrightness() <= 0.20)
                content += points[0];
            else
            if (pixel.GetBrightness() <= 0.40)
                content += points[1];
            else
            if (pixel.GetBrightness() <= 0.60)
                content += points[2];
            else
            if (pixel.GetBrightness() <= 0.80)
                content += points[3];
            else
            if (pixel.GetBrightness() <= 1)
                content += points[4];
        }

        void textRGBPoint(System.Drawing.Color pixel)
        {
            if (pixel.R > pixel.G && pixel.R > pixel.B)
                content += points[0];
            else
            if (pixel.G > pixel.R && pixel.G > pixel.B)
                content += points[1];
            else
            if (pixel.B > pixel.G && pixel.B > pixel.R)
                content += points[2];
            else
            if (pixel.R == pixel.G && pixel.R == pixel.B && pixel.R <= 127)
                content += points[3];
            else
            if (pixel.R == pixel.G && pixel.R == pixel.B && pixel.R > 127)
                content += points[4];
        }

        void textRGBChar(System.Drawing.Color pixel)
        {
            if (pixel.R > pixel.G && pixel.R > pixel.B)
                content += chars[0];
            else
            if (pixel.G > pixel.R && pixel.G > pixel.B)
                content += chars[1];
            else
            if (pixel.B > pixel.G && pixel.B > pixel.R)
                content += chars[2];
            else
            if (pixel.R == pixel.G && pixel.R == pixel.B && pixel.R <= 127)
                content += chars[3];
            else
            if (pixel.R == pixel.G && pixel.R == pixel.B && pixel.R > 127)
                content += chars[4];
        }
        #endregion

        #region braille
        //test with braille char
        // ⠁ ⠂ ⠃ ⠄ ⠅ ⠆ ⠇ ⠈ ⠉ ⠊ ⠋ ⠌ ⠍ ⠎ ⠏ ⠐ ⠑ ⠒ ⠓ ⠔ ⠕ ⠖ ⠗ ⠘ ⠙ ⠚ ⠛ ⠜ ⠝ ⠞ ⠟ ⠠ ⠡ ⠢ ⠣ ⠤ ⠥ ⠦ ⠧ ⠨ ⠩ ⠪ ⠫ ⠬ ⠭ ⠮ ⠯ ⠰ ⠱ ⠲ ⠳ ⠴ ⠵ ⠶ ⠷ ⠸ ⠹ ⠺ ⠻ ⠼ ⠽ ⠾ ⠿
        //check 6 pixel with 50% of brightness for each one and find the right char
        void brailleBrightness()
        {
            double pixelCont = 0;
            try
            {
                // Retrieve the image.
                var image1 = new Bitmap(@"" + fileDirecotry, true);
                double pixelImage = image1.Height * image1.Width;
                int x = 0, y = 0;
                string currentChar="";

                // Loop through the images pixels to reset color.
                for (y = 0; y < image1.Height; y += 3)
                {
                    for (x = 0; x < image1.Width; x += 2)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            for (int j = 0; j < 2; j++)
                            {
                                if (image1.GetPixel(x + j, y + i).GetBrightness() >= 0.5)
                                    currentChar += boolean[0];
                                else
                                    currentChar += boolean[1];

                                pixelCont++;
                                pixelPercent.Dispatcher.BeginInvoke((Action)(() =>
                                {
                                    pixelPercent.Value = (pixelCont / pixelImage) * 100;
                                    perventValue.Text = ((int)pixelPercent.Value).ToString() + "%";
                                }));
                            }
                            currentChar += i==2? "":" ";
                        }
                        checkbraillecode(currentChar);
                        currentChar = "";
                        if ((x + 2) >= image1.Width-1)
                            break;
                    }
                    if ((y + 3) >= image1.Height-1)
                        break;
                    content += "\n";
                }
                writer(content);
            }
            catch (ArgumentException e)
            {
                if (pixelCont > 0)
                {
                    MessageBox.Show("the resault won't be the best", "Result information", MessageBoxButton.OK, MessageBoxImage.Information);
                    writer(content);
                }else
                    MessageBox.Show("your image is not compatible", "Image Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            Process.Start(txtFile);
            LoadButton.Dispatcher.BeginInvoke((Action)(() =>
            {
                pixelPercent.Value = 100;
                perventValue.Text = "100%";
                LoadButton.IsEnabled = true;
                invertBox.IsEnabled = true;
            }));
        }

        void checkbraillecode(string code)
        {
            if (File.Exists(brailleMap))
            {
                string[] brailleAlph = File.ReadAllText(brailleMap).Replace("\r\n", "").Split(',');
                foreach (var item in brailleAlph)
                {
                    if (item.Split('=')[0] == code)
                    {
                        content += item.Split('=')[1];
                        return;
                    }
                }
            }
            else
                MessageBox.Show("There are some problem, brailleMapping.conf doesn't exists");
        }
        #endregion

        #region File managment
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
        #endregion

        #region Test console
        private void PLayerButton_Click(object sender, RoutedEventArgs e)
        {
            frames = "";
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = "";
            dialog.IsFolderPicker = true;
            string[] files;
            //string sequence_directory;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                fileDirecotry = dialog.FileName;
            }
            else
                return;

            //processing images, put in frames.frs splitted by ';'
            files = Directory.GetFiles(fileDirecotry);

            foreach (var item in files)
            {
                fileDirecotry = item;
                imageBrightnessCharControl();
                content = "";
                frames += File.ReadAllText(txtFile) + ";\n";
            }

            File.WriteAllText(framesDirectory, frames);
            ShowConsole();
        }
        public void ShowConsole()
        {
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\Player\CharactersVideoPlayer.exe";
            Process.Start(info);
        }
        #endregion

        #region Link management
        private void SupportButton_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.paypal.com/paypalme/MeneBot");
        }

        private void openGithub(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/Mene-hub");
        }
        #endregion
    }
}