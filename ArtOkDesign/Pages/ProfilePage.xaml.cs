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
        public int currentUser;
        public bool openDialog = false;
        public Post[] GPost = null;
        public string _Comment;


        public ProfilePage()
        {
            InitializeComponent();
            currentUser = GlobalInformation.currentUser.ID;
            btnFollow.Visibility= Visibility.Collapsed;
            btnChange.Visibility = Visibility.Visible;
            GetProfile();
            GetPosts();
        }
        public ProfilePage(int id)
        {
            InitializeComponent();
            currentUser = id;
            btnFollow.Visibility= Visibility.Visible;
            btnChange.Visibility = Visibility.Collapsed;
            CheckFollows();
            GetProfile();
            GetPosts();
        }

        public async void GetProfile()
        {
            User temp = await ApiController.GetUserAsync($"https://localhost:2222/api/User/{currentUser}");

            tbNick.Text =temp.NickName;

            byte[] profPicture = await ApiController.GetProfilePicture(currentUser);
            byte[] profBackground = await ApiController.GetProfileBackgroun(currentUser);
            tbFollowers.Text = Convert.ToString(await ApiController.GetFollowersCount(currentUser));
            tbFollowed.Text = Convert.ToString(await ApiController.GetFollowedCount(currentUser));
            if(profPicture != null && profPicture.Length>1 ) 
            {
                MemoryStream byteStream = new MemoryStream(profPicture);
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = byteStream;
                image.EndInit();

                ImgPP.Source = image;
            }
            if (profBackground != null && profBackground.Length>1)
            {
                MemoryStream byteStream = new MemoryStream(profBackground);
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = byteStream;
                image.EndInit();

                ImgBackground.Source = image;
            }
           

        }
        public async void GetPosts()
        {
            Like[] Listlikes = await ApiController.GetUserLikes(GlobalInformation.currentUser.ID);

            Post[] posts = await ApiController.GetCurrentUserPosts(currentUser);
            foreach (Post post in posts)
            {
                PopApp[] popApps = await ApiController.GetPostPopApps(post.ID);

                User user = await ApiController.GetUserAsync($"https://localhost:2222/api/User/{post.IDUser}");
                Tag[] tags = await ApiController.GetPostTags(post.ID);
                foreach (Like like in Listlikes)
                {
                    if (like.IDPost != post.ID)
                    {
                        post.LikeIcon = "HeartOutline";
                    }
                    else
                    {
                        post.LikeIcon = "Heart";
                        post.LikeID = like.ID;
                        break;
                    }
                }
                foreach (Tag tag in tags)
                {
                    post.Taging += tag.NameTag + ", ";
                }
                foreach (PopApp pop in popApps)
                {
                    post.PopApping += pop.NamePopApp + ", ";
                }
                byte[] image = await ApiController.GetImage(post.ID);
                post.PostNickName = user.NickName;
                post.PostLikes = await ApiController.GetPostLikes(post.ID);
                post.UserProfPicture = await ApiController.GetProfilePicture(user.ID);
                post.CommentCount = await ApiController.GetCommentCount(post.ID);
                post.PopAppCount = await ApiController.GetPopAppCount(post.ID);
                post.TagCount = await ApiController.GetTagCount(post.ID);
                post.RepostCount = await ApiController.GetRepostCount(post.ID);

                if (image != null)
                {
                    post.PostImage = image;
                    post.ImageVisibility = "Visible";
                }
                else
                {
                    post.ImageVisibility = "Collapsed";
                }

                post.LVHeight = "0";
                post.ListVisability = "Collapsed";
                post.WPVisibility = "Collapsed";

            }
            GlobalInformation.Posts = posts;
            GPost = posts;
            LvPosts.ItemsSource = posts;
            GC.Collect();
        }
        public async void GetPosts(int IDPost)
        {
            Like[] Listlikes = await ApiController.GetUserLikes(GlobalInformation.currentUser.ID);
            //Post[] posts = await ApiController.GetPostsAsync($"https://localhost:2222/api/Post");
            foreach (Post post in GPost)
            {
                User user = await ApiController.GetUserAsync($"https://localhost:2222/api/User/{post.IDUser}");
                Tag[] tags = await ApiController.GetPostTags(post.ID);

                foreach (Like like in Listlikes)
                {
                    if (like.IDPost != post.ID)
                    {
                        post.LikeIcon = "HeartOutline";
                    }
                    else
                    {
                        post.LikeIcon = "Heart";
                        post.LikeID = like.ID;
                        break;
                    }
                }
                foreach (Tag tag in tags)
                {
                    post.Taging += tag.NameTag + ", ";
                }
                byte[] image = await ApiController.GetImage(post.ID);
                post.PostNickName = user.NickName;
                post.PostLikes = await ApiController.GetPostLikes(post.ID);
                post.UserProfPicture = await ApiController.GetProfilePicture(user.ID);
                post.CommentCount = await ApiController.GetCommentCount(post.ID);
                post.PopAppCount = await ApiController.GetPopAppCount(post.ID);
                post.TagCount = await ApiController.GetTagCount(post.ID);
                post.RepostCount= await ApiController.GetRepostCount(post.ID);

                if (image != null)
                {
                    post.PostImage = image;
                    post.ImageVisibility = "Visible";
                }
                else
                {
                    post.ImageVisibility = "Collapsed";
                }
                if (post.ID == IDPost)
                {
                    if (post.IsDialogsHidden == true)
                    {
                        post.LVHeight = "0";
                        post.ListVisability = "Collapsed";
                        post.WPVisibility = "Collapsed";
                        post.IsDialogsHidden = false;
                    }
                    else
                    {
                        post.LVHeight = "200";
                        post.ListVisability = "Visible";
                        post.WPVisibility = "Visible";
                        post.IsDialogsHidden = true;
                        PostComment[] comments = await ApiController.GetPostComments(post.ID);
                        foreach (PostComment comment in comments)
                        {
                            User userComm = await ApiController.GetUserAsync($"https://localhost:2222/api/User/{post.IDUser}");
                            comment.CommentNick = userComm.NickName;
                            byte[] profPictureComm = await ApiController.GetProfilePicture(user.ID);
                            comment.PostPP = profPictureComm;
                        }

                        post.CommentList = comments;
                    }
                }

            }

            LvPosts.ItemsSource = null;
            LvPosts.ItemsSource = GPost;
            GC.Collect();
        }
        private void btnComment_Click(object sender, RoutedEventArgs e)
        {
            GetPosts(((sender as Button).DataContext as Post).ID);
        }

        private async void btnAddComm_Click(object sender, RoutedEventArgs e)
        {
            PostComment newComm = new PostComment();
            newComm.IDUser = GlobalInformation.currentUser.ID;
            newComm.IDPost = ((sender as Button).DataContext as Post).ID;
            newComm.Comment = _Comment;
            newComm.TimeOfWrite = DateTime.Now;
            await ApiController.PushComment(newComm);
            GetPosts(((sender as Button).DataContext as Post).ID);
        }

        private void txtComm_TextChanged(object sender, TextChangedEventArgs e)
        {
            _Comment = (sender as TextBox).Text;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (((sender as Button).DataContext as Post).LikeIcon == "HeartOutline")
            {
                Like newlike = new Like();
                newlike.IDUser = GlobalInformation.currentUser.ID;
                newlike.IDPost = ((sender as Button).DataContext as Post).ID;
                newlike.DateOfLike = DateTime.Now;
                await ApiController.AdLike(newlike);
            }
            else
            {
                await ApiController.DeleteLike(((sender as Button).DataContext as Post).LikeID);
            }
            GetPosts(((sender as Button).DataContext as Post).ID);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            GlobalInformation.MainFrame.Navigate(new EditProfilePage());
        }

        private async void btnFollow_Click(object sender, RoutedEventArgs e)
        {
            if (btnFollow.Content.ToString()== "Подписаться")
            {
                Follower addfollower= new Follower();
                addfollower.IDCurrentUser= GlobalInformation.currentUser.ID;
                addfollower.IDFollowedUser = currentUser;
                addfollower.TimeOfFollowing= DateTime.Now;
                await ApiController.AddFollower(addfollower);
                GlobalInformation.MainFrame.Navigate(new ProfilePage(currentUser));
            }
            else
            {
                Follower[] temp = await ApiController.GetFollowerAsync();
                await ApiController.RemoveFollower(temp[0].ID);
                GlobalInformation.MainFrame.Navigate(new ProfilePage(currentUser));

            }
        }
        public async void CheckFollows()
        {
            Follower[] GUserFollows = await ApiController.GetCurrentUserFollowers(GlobalInformation.currentUser.ID);
            List<Follower> temp = GUserFollows.ToList();
            if (temp.Any(t => t.IDFollowedUser == currentUser))
            {
                btnFollow.Foreground = Brushes.Red;
                btnFollow.Content = "Отписаться";
            }
            else
            {
                btnFollow.Foreground = Brushes.LightGreen;
                btnFollow.Content = "Подписаться";
            }


        }
    }
}
