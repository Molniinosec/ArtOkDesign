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
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace ArtOkDesign.Pages
{
    /// <summary>
    /// Логика взаимодействия для PostPage.xaml
    /// </summary>
    public partial class PostPage : Page
    {
        public bool openDialog = false;
        public Post[] GPost = null;
        public string _Comment;

        public PostPage(int num)
        {
            InitializeComponent();
            if (num == 1)
                GetPosts();
            else
                GetUserFollowersPosts();
        }
        public PostPage(string searchText)
        {
            InitializeComponent();
            GetSearchPost(searchText);
        }
        public void GetSearchPost(string Search)
        {
            List<Post> PostList= GlobalInformation.Posts.ToList();
            if (!String.IsNullOrWhiteSpace(Search))
            {
                PostList = PostList.Where(p => p.Description.ToLower().Contains(Search)).ToList();
            }
            LvPosts.ItemsSource= PostList;
        }
        public async void GetPosts()
        {
            Like[] Listlikes = await ApiController.GetUserLikes(GlobalInformation.currentUser.ID);

            Post[] posts = await ApiController.GetPostsAsync($"https://localhost:2222/api/Post");
            foreach (Post post in posts)
            {
                User user = await ApiController.GetUserAsync($"https://localhost:2222/api/User/{post.IDUser}");
                Tag[] tags = await ApiController.GetPostTags(post.ID);
                foreach(Like like in Listlikes)
                {
                    if (like.IDPost != post.ID)
                    {
                        post.LikeIcon = "HeartOutline";
                    }
                    else
                    {
                        post.LikeIcon = "Heart";
                        break;
                    }
                }
                foreach(Tag tag in tags)
                {
                    post.Taging += tag.NameTag + ", ";
                }
                byte[] image = await ApiController.GetImage(post.ID);
                post.PostNickName = user.NickName;
                post.PostLikes= await ApiController.GetPostLikes(post.ID);
                post.UserProfPicture = await ApiController.GetProfilePicture(user.ID);
                post.CommentCount = await ApiController.GetCommentCount(post.ID);
                post.PopAppCount = await ApiController.GetPopAppCount(post.ID);
                post.TagCount= await ApiController.GetTagCount(post.ID);
      
                if(image !=null)
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

            //Post[] posts = await ApiController.GetPostsAsync($"https://localhost:2222/api/Post");
            foreach (Post post in GPost)
            {
                User user = await ApiController.GetUserAsync($"https://localhost:2222/api/User/{post.IDUser}");
                Tag[] tags = await ApiController.GetPostTags(post.ID);

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
                    if (post.IsDialogsHidden==true)
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
                            //if (openDialog == false)
                            //{
                            //    comment.ListVisability = "Collapsed";

                            //}
                            //else
                            //{
                            //    comment.ListVisability = "Visible";

                            //}
                        }
                        
                        post.CommentList = comments;
                    }
                }

                //PostComment[] comments = await ApiController.GetPostComments(post.ID);
                //foreach (PostComment comment in comments)
                //{
                //    User userComm = await ApiController.GetUserAsync($"https://localhost:2222/api/User/{post.IDUser}");
                //    comment.CommentNick = userComm.NickName;
                //    byte[] profPictureComm = await ApiController.GetProfilePicture(user.ID);
                //    comment.PostPP = profPictureComm;
                //    //if (openDialog == false)
                //    //{
                //    //    comment.ListVisability = "Collapsed";

                //    //}
                //    //else
                //    //{
                //    //    comment.ListVisability = "Visible";

                //    //}
                //}
                //post.CommentList = comments;

            }
            //GPost = posts;
            //LvPosts.ItemsSource = posts;
            LvPosts.ItemsSource = null;
            LvPosts.ItemsSource = GPost;
            GC.Collect();
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
                    User user = await ApiController.GetUserAsync($"https://localhost:2222/api/User/{post.IDUser}");
                    Tag[] tags = await ApiController.GetPostTags(post.ID);

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
                    posts.Add(post);
                }
            }
            GPost=posts.ToArray();
            LvPosts.ItemsSource= posts;
            GC.Collect();
        }

        private void btnComment_Click(object sender, RoutedEventArgs e)
        {
            GetPosts(((sender as Button).DataContext as Post).ID);
        }

        private async void btnAddComm_Click(object sender, RoutedEventArgs e)
        {
            PostComment newComm= new PostComment();
            newComm.IDUser= GlobalInformation.currentUser.ID;
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
    }
}
