using ArtOkDesign.Classes;
using Microsoft.Win32;
using System;
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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace ArtOkDesign.Pages
{
    /// <summary>
    /// Логика взаимодействия для EditProfilePage.xaml
    /// </summary>
    public partial class EditProfilePage : Page
    {
        User currentUser = GlobalInformation.currentUser;
        string ppPath= string.Empty;
        string backPath= string.Empty;
        public EditProfilePage()
        {
            InitializeComponent();
            GetProfile();
        }
        public async void GetProfile()
        {
            tbNick.Text = currentUser.NickName;
            byte[] profPicture = await ApiController.GetProfilePicture(currentUser.ID);
            byte[] profBackground = await ApiController.GetProfileBackgroun(currentUser.ID);

            if (profPicture != null)
            {
                MemoryStream byteStream = new MemoryStream(profPicture);
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = byteStream;
                image.EndInit();

                ImgPP.Source = image;
            }
            if (profBackground != null)
            {
                MemoryStream byteStream = new MemoryStream(profBackground);
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = byteStream;
                image.EndInit();

                ImgBackground.Source = image;
            }


        }

        private async void btnEdit_Click(object sender, RoutedEventArgs e)
        {

            User updateUser = currentUser;
            if (!String.IsNullOrWhiteSpace(ppPath))
            {
                BitmapImage bitmapImagePP = new BitmapImage(new Uri(ppPath));
                MemoryStream memStream = new MemoryStream();
                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapImagePP));
                encoder.Save(memStream);
                var res = await ApiController.SaveFileInFolder(currentUser, memStream.ToArray());
                res.Trim('"');
                updateUser.ProfilePicture = res;
            }
            if (!String.IsNullOrWhiteSpace(backPath))
            {
                BitmapImage bitmapImageBack = new BitmapImage(new Uri(backPath));
                MemoryStream memStreamB = new MemoryStream();
                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapImageBack));
                encoder.Save(memStreamB);
                var res = await ApiController.SaveFileInFolder(currentUser, memStreamB.ToArray());
                res= res.Trim('\"');
                updateUser.BackgroundPicture = res;

            }
            await ApiController.UpdateUser(updateUser);
            GlobalInformation.currentUser = updateUser;
            GlobalInformation.MainFrame.Navigate(new ProfilePage());
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            GlobalInformation.MainFrame.Navigate(new ProfilePage());
        }

        private void ImgBackground_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is System.Windows.Controls.Image)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Title = "Выдерите изображение";
                openFileDialog.Filter = "PNG|*.png|JPEG|*.jpeg| JPG|*.jpg";
                if (openFileDialog.ShowDialog() == true)
                {
                    ImgBackground.Source = new BitmapImage(new Uri(openFileDialog.FileName));
                    backPath = openFileDialog.FileName;                  
                }
            }
        }

        private void ImgPP_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is System.Windows.Controls.Image)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Title = "Выдерите изображение";
                openFileDialog.Filter = "PNG|*.png|JPEG|*.jpeg| JPG|*.jpg";
                if (openFileDialog.ShowDialog() == true)
                {
                    ImgPP.Source = new BitmapImage(new Uri(openFileDialog.FileName));
                    ppPath = openFileDialog.FileName;
                }
            }
        }
    }
}
