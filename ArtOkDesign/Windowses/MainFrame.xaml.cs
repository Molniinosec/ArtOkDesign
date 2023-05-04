using ArtOkDesign.Classes;
using ArtOkDesign.Pages;
using Microsoft.AspNetCore.Http;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace ArtOkDesign.Windowses
{
    /// <summary>
    /// Логика взаимодействия для MainFrame.xaml
    /// </summary>
    public partial class MainFrame : Window
    {
        public static System.Windows.Controls.Image global_sender;
        public MainFrame()
        {
            InitializeComponent();
            PostFrame.Navigate(new PostPage(1));
        }

        private void btnAllPosts_Click(object sender, RoutedEventArgs e)
        {
            PostFrame.Navigate(new PostPage(1));
        }

        private void btnFollowedPost_Click(object sender, RoutedEventArgs e)
        {
            PostFrame.Navigate(new PostPage(2));

        }

        private void imgPost_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(sender is System.Windows.Controls.Image)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Title = "Выдерите изображение";
                openFileDialog.Filter = "PNG|*.png|JPEG|*.jpeg| JPG|*.jpg";
                if (openFileDialog.ShowDialog() == true)
                {
                    imgPost.Source = new BitmapImage(new Uri(openFileDialog.FileName));
                    imgPost.Height = 250;
                    imgPost.Width = 250;
                    GlobalInformation.postImage= imgPost;
                    GlobalInformation.ImagePath = openFileDialog.FileName;
                    GlobalInformation.FileDialogFile = openFileDialog;
                }
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            BitmapImage bitmapImage = new BitmapImage(new Uri(GlobalInformation.ImagePath));
            MemoryStream memStream = new MemoryStream();
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
            encoder.Save(memStream);

            

            Post createPost = new Post();
            //createPost.IFile = bitmapImage;
            createPost.IDUser = GlobalInformation.currentUser.ID;
            createPost.Description = txtDescr.Text;
            createPost.TimeOfCreate = DateTime.Now;
            txtDescr.Text = "";
            imgPost.Width = 50;
            imgPost.Height = 50;
            imgPost.Source = new BitmapImage(new Uri(@"C:\\Users\\izran\\source\\repos\\ArtOkDesign\\ArtOkDesign\\Res\\addImage.png"));
            await ApiController.PushPost(createPost);
            await ApiController.PushImage(createPost, memStream.ToArray());
        }
    }
}
