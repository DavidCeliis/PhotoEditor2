using Microsoft.Win32;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;



namespace PhotoEditor2
{
    public partial class MainWindow : Window
    {
        private BitmapImage OriginalImage;
        private WriteableBitmap WriteableImage;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.jpg;*.png)|*.jpg;*.png|All Files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                OriginalImage = new BitmapImage(new Uri(openFileDialog.FileName));
                WriteableImage = new WriteableBitmap(OriginalImage);
                MainImage.Source = OriginalImage;
            }
        }
        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            MainImage.Source = OriginalImage;
            WriteableImage = new WriteableBitmap(OriginalImage);
        }
        private void RandomizePixelsButton_Click(object sender, RoutedEventArgs e)
        {
            if (WriteableImage == null)
            {
                MessageBox.Show("Please load an image first.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int height = WriteableImage.PixelHeight;
            int width = WriteableImage.PixelWidth;

            byte[] pixels = new byte[height * width * 4]; 
            WriteableImage.CopyPixels(pixels, width * 4, 0);

            Random rand = new Random();
            Parallel.For(0, height, y =>
            {
                for (int x = 0; x < width; x++)
                {
                    if (rand.NextDouble() <= 0.6)
                    {
                        int index = (y * width + x) * 4;
                        byte[] pixel = BitConverter.GetBytes(rand.Next());
                        Array.Copy(pixel, 0, pixels, index, 4);
                    }
                }
            });

            WriteableImage.WritePixels(new Int32Rect(0, 0, width, height), pixels, width * 4, 0);
            MainImage.Source = WriteableImage;
        }
        private void MonochromeButton_Click(object sender, RoutedEventArgs e)
        {
            if (WriteableImage == null)
            {
                MessageBox.Show("Please load an image first.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int height = WriteableImage.PixelHeight;
            int width = WriteableImage.PixelWidth;

            byte[] pixels = new byte[height * width * 4];
            WriteableImage.CopyPixels(pixels, width * 4, 0);

            Parallel.For(0, height, y =>
            {
                for (int x = 0; x < width; x++)
                {
                    int index = (y * width + x) * 4;
                    byte gray = (byte)((pixels[index] + pixels[index + 1] + pixels[index + 2]) / 3);
                    pixels[index] = gray;
                    pixels[index + 1] = gray;
                    pixels[index + 2] = gray;
                }
            });

            WriteableImage.WritePixels(new Int32Rect(0, 0, width, height), pixels, width * 4, 0);
            MainImage.Source = WriteableImage;
        }
       

    }
}