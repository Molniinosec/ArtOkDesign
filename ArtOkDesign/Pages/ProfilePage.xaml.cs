using ArtOkDesign.Classes;
using Newtonsoft.Json.Linq;
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
    /// Логика взаимодействия для ProfilePage.xaml
    /// </summary>
    public partial class ProfilePage : Page
    {
        public User currentUser;
        public ProfilePage()
        {
            InitializeComponent();
            currentUser = GlobalInformation.currentUser;
            GetProfile();
        }

        public async void GetProfile()
        {
            tbNick.Text = currentUser.NickName;
            byte[] profPicture = await ApiController.GetProfilePicture(currentUser.ID);
            byte[] profBackground = await ApiController.GetProfileBackgroun(currentUser.ID);
            tbFollowers.Text = Convert.ToString(await ApiController.GetFollowersCount(currentUser.ID));
            tbFollowed.Text = Convert.ToString(await ApiController.GetFollowedCount(currentUser.ID));
            MemoryStream byteStream = new MemoryStream(profPicture);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = byteStream;
            image.EndInit();

            ImgPP.Source = image;

            byteStream = new MemoryStream(profBackground);
            image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = byteStream;
            image.EndInit();

            ImgBackground.Source = image;

            Post[] posts = await ApiController.GetCurrentUserPosts(currentUser.ID);
            foreach (Post post in posts)
            {
                User user = await ApiController.GetUserAsync($"https://localhost:2222/api/User/{post.IDUser}");
                int likes = await ApiController.GetPostLikes(post.ID);
                byte[] images = await ApiController.GetImage(post.ID);
                byte[] profPictures = await ApiController.GetProfilePicture(user.ID);
                post.PostNickName = user.NickName;
                post.PostLikes = likes;
                post.UserProfPicture = profPictures;
                if (image != null)

                    post.PostImage = images;
            }
            LvPosts.ItemsSource = posts;


        } 
    }
}
