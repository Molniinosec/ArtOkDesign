using ArtOkDesign.Classes;
using MaterialDesignColors;
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

namespace ArtOkDesign.Pages
{
    /// <summary>
    /// Логика взаимодействия для CreateDialog.xaml
    /// </summary>
    public partial class CreateDialog : Page
    {
        List<User> GUser = new List<User>();
        List<User> AddUserList = new List<User>();
        public string DialogImagePath;
        public CreateDialog()
        {
            InitializeComponent();
            FillUserList();
            AddUserList.Add(GlobalInformation.currentUser);
        }

        public async void FillUserList()
        {
            User[] userList = await ApiController.GetUsersAsync();
            foreach (User user in userList)
            {
                user.IconCheck = "Collapsed";
            }
            GUser = userList.ToList();
            GUser.Remove(GUser.Where(u=>u.ID==GlobalInformation.currentUser.ID).FirstOrDefault());
            lvUsers.ItemsSource = GUser;
        }
        public void Filter()
        {
            List<User> PostList = GUser;
            if (!String.IsNullOrWhiteSpace(txtSerach.Text))
            {
                PostList = PostList.Where(p => p.NickName.ToLower().Contains(txtSerach.Text.ToLower())).ToList();
            }

            lvUsers.ItemsSource = PostList;
        }
        private void txtSerach_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filter();
        }
        private void lvUsers_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            
            if (lvUsers.SelectedItem is User)
            {
                var user = lvUsers.SelectedItem as User;
                if ((user.IconCheck == "Collapsed"))
                {
                    user.IconCheck = "Visible";
                    AddUserList.Add(user);
                }
                else
                {
                    user.IconCheck = "Collapsed";
                    AddUserList.Remove(user);
                }
            }
            lvUsers.ItemsSource = null;
            Filter();

        }

        private async void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            Dialog addDialog = new Dialog();
            addDialog.Name = txtDialogName.Text;
            addDialog.DateOfCreation = DateTime.Now;
            if (!String.IsNullOrWhiteSpace(DialogImagePath))
            {
                BitmapImage bitmapImagePP = new BitmapImage(new Uri(DialogImagePath));
                MemoryStream memStream = new MemoryStream();
                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapImagePP));
                encoder.Save(memStream);
                var res = await ApiController.SaveFileInFolder(GlobalInformation.currentUser, memStream.ToArray());
                res.Trim('"');
                addDialog.DialogPicture = res;
            }
          
            await ApiController.AddDialog(addDialog);


            Dialog[] CurrentDialog = await ApiController.GetDialogs();
            foreach(User user in AddUserList)
            {
                DialogUser dialogUser = new DialogUser();
                dialogUser.IDUser = user.ID;
                dialogUser.IDDialog = CurrentDialog[0].ID;
                await ApiController.AddUserInDialog(dialogUser);
            }


            GlobalInformation.MainFrame.Navigate(new SelectDialog());
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            GlobalInformation.MainFrame.Navigate(new SelectDialog());
        }

        private void ImgDialog_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is System.Windows.Controls.Image)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Title = "Выдерите изображение";
                openFileDialog.Filter = "PNG|*.png|JPEG|*.jpeg| JPG|*.jpg";
                if (openFileDialog.ShowDialog() == true)
                {
                    ImgDialog.Source = new BitmapImage(new Uri(openFileDialog.FileName));
                    DialogImagePath = openFileDialog.FileName;
                }
            }
        }
    }
}
