using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
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

namespace ImageClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static HttpClient httpClient = new HttpClient();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "Image files (*.BMP, *.JPG, *.GIF, *.TIF, *.PNG, *.ICO, *.EMF, *.WMF)|*.bmp;*.jpg;*.gif; *.tif; *.png; *.ico; *.emf; *.wmf";

            if (openDialog.ShowDialog() == true)
            {
                imagePath.Text = openDialog.FileName;
                img1.Source = new BitmapImage(new Uri(openDialog.FileName));
            }                
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //("file", System.IO.File.ReadAllBytes(@"F:\Vishal\tt12.txt"), "tt12.txt")
            var content = new ImageWithDescription { Name = imagePath.Text, CreatedAt = DateTime.Now, Image = imagePath.Text, Description = description.Text};
            await httpClient.PostAsync("https://localhost:44315/imagestorage/upload", (HttpContent)content);

        }
        static async Task GetAll()
        {
            var people = await httpClient.GetAsync("https://localhost:44315/imagestorage/get-all");
        }
    }
}
