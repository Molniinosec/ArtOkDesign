using ArtOkDesign.Classes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
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
    /// Логика взаимодействия для PostPage.xaml
    /// </summary>
    public partial class PostPage : Page
    {

        public PostPage(int num)
        {
            InitializeComponent();
            if (num == 1)
                GetPosts();
            else
                GetUserFollowersPosts();
            
        }

        public async void GetPosts()
        {

            Post[] posts = await ApiController.GetPostsAsync($"https://localhost:2222/api/Post");
            foreach (Post post in posts)
            {
                User user = await ApiController.GetUserAsync($"https://localhost:2222/api/User/{post.IDUser}");
                int likes = await ApiController.GetPostLikes(post.ID);
                byte[] image = await ApiController.GetImage(post.ID);
                post.PostNickName = user.NickName;
                post.PostLikes= likes;
                if(image !=null)
                    
                    post.PostImage = image;
            }
            LvPosts.ItemsSource = posts;
            
        }
        public async void GetUserFollowersPosts()
        {
            List<Post> posts = new List<Post>();
            Follower[] followers = await ApiController.GetCurrentUserFollowers(GlobalInformation.currentUser.ID);
            foreach (Follower follower in followers)
            {
                Post[] postes = await ApiController.GetPostsAsync($"https://localhost:2222/api/Post/{follower.IDFollowedUser}");
                foreach (Post post in postes)
                {
                    posts.Add(post);
                    User user = await ApiController.GetUserAsync($"https://localhost:2222/api/User/{post.IDUser}");
                    post.PostNickName = user.NickName;
                }
            }
            
            LvPosts.ItemsSource= posts;
        }
    }
}
