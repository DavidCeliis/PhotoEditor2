using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;



namespace PhotoEditor2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
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

            Random rand = new Random();
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (rand.NextDouble() <= 0.4)
                    {
                        byte[] pixel = BitConverter.GetBytes(rand.Next());
                        WriteableImage.WritePixels(new Int32Rect(x, y, 1, 1), pixel, 4, 0);
                    }
                }
            }

            MainImage.Source = WriteableImage;
        }
        public enum SelectedColor
        {
            Green,
            Red,
            Blue
        }

        //private void ApplyColorButton_Click(object sender, RoutedEventArgs e)
        //{
        //    if (WriteableImage == null)
        //    {
        //        MessageBox.Show("Please load an image first.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //        return;
        //    }
        //    int[,] pixels = Array2DBMIConverter.BitmapImageToArray2D(MainImage.Source as BitmapImage);

        //    // Get the selected color
        //    SelectedColor selectedColor = (SelectedColor)ColorComboBox.SelectedIndex;

        //    // Loop through the pixels and replace the color with the selected color
        //    for (int i = 0; i < pixels.GetLength(0); i++)
        //    {
        //        for (int j = 0; j < pixels.GetLength(1); j++)
        //        {
        //            int pixelColor = pixels[i, j];
        //            byte r = (byte)((pixelColor & 0x00FF0000) >> 16);
        //            byte g = (byte)((pixelColor & 0x0000FF00) >> 8);
        //            byte b = (byte)(pixelColor & 0x000000FF);

        //            switch (selectedColor)
        //            {
        //                case SelectedColor.Green:
        //                    r = 0;
        //                    b = 0;
        //                    break;
        //                case SelectedColor.Red:
        //                    g = 0;
        //                    b = 0;
        //                    break;
        //                case SelectedColor.Blue:
        //                    r = 0;
        //                    g = 0;
        //                    break;
        //            }

        //            int newPixelColor = (r << 16) + (g << 8) + b;
        //            pixels[i, j] = newPixelColor;
        //        }
        //    }

        //    WriteableImage = Array2DBMIConverter.BitmapImageToArray2D(pixels);
        //    MainImage.Source = WriteableImage;

        //}

    }
}