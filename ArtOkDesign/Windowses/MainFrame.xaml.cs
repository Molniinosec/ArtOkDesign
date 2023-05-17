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
        public List<Tag> GTags=new List<Tag>();
        public List<PopApp> GpopApps=new List<PopApp>();
        public List<Tag> SearchTag=new List<Tag>();
        public List<PopApp> SearchPopApp =new List<PopApp>();

        public static System.Windows.Controls.Image global_sender;
        public MainFrame()
        {
            InitializeComponent();
            PostFrame.Navigate(new PostPage(1));
            FillTagList();
        }

        public async void FillTagList()
        {
            lvTagsInAddPost.ItemsSource = await ApiController.GetAllTags();
            LVPopAppInPost.ItemsSource= await ApiController.GetAllPopApps();
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
            var currentPost = await ApiController.GetPostsAsync($"https://localhost:2222/api/Post/{GlobalInformation.currentUser.ID}");

            foreach (Tag tag in GTags)
            {
                PostTag postTag = new PostTag();
                postTag.IDPost = currentPost[0].ID;
                postTag.IDTag = tag.ID;
                await ApiController.PushPostTag(postTag);
            }
            foreach(PopApp popApp in GpopApps)
            {
                PostPopApp postPopApp = new PostPopApp();
                postPopApp.IDPost = currentPost[0].ID;
                postPopApp.IDPopApp = popApp.ID;
                await ApiController.PushPostPopApp(postPopApp);
            }
            GC.Collect();
        }

        private void btnProfile_Click(object sender, RoutedEventArgs e)
        {
            PostFrame.Navigate(new ProfilePage());
        }

        private void btnSelectDialog_Click(object sender, RoutedEventArgs e)
        {
            PostFrame.Navigate(new SelectDialog());
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if ((sender as CheckBox).IsChecked == true)
            {
                GTags.Add(((sender as CheckBox).DataContext as Tag));
            }
            else
            {
                GTags.Remove(((sender as CheckBox).DataContext as Tag));
            }
        }
        private void CheckBoxPopApp_Checked(object sender, RoutedEventArgs e)
        {
            if ((sender as CheckBox).IsChecked == true)
            {
                GpopApps.Add(((sender as CheckBox).DataContext as PopApp));
            }
            else
            {
                GpopApps.Remove(((sender as CheckBox).DataContext as PopApp));
            }
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && sender is TextBox)
            {
                PostFrame.Navigate(new PostPage(txtSearch.Text));
            }
        }
        
        private void SearchTag_Unchecked(object sender, RoutedEventArgs e)
        {
            if ((sender as CheckBox).IsChecked == true)
            {
                SearchTag.Add(((sender as CheckBox).DataContext as Tag));
            }
            else
            {
                SearchTag.Remove(((sender as CheckBox).DataContext as Tag));
            }
        }
        private void SearchPopApp_Unchecked(object sender, RoutedEventArgs e)
        {
            if ((sender as CheckBox).IsChecked == true)
            {
                SearchPopApp.Add(((sender as CheckBox).DataContext as PopApp));
            }
            else
            {
                SearchPopApp.Remove(((sender as CheckBox).DataContext as PopApp));
            }
        }
    }
}
