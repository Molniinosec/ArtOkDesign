﻿using ArtOkDesign.Classes;
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
        public PostPage(string searchText,List<Tag> tags,List<PopApp> popApps)
        {
            InitializeComponent();
            GetSearchPost(searchText, tags,popApps);
        }
        public async void GetSearchPost(string Search,List<Tag> TagSearch,List<PopApp> popAppSearch)
        {
            List<Post> PostList= GlobalInformation.Posts.ToList();
            if (!String.IsNullOrWhiteSpace(Search))
            {
                PostList = PostList.Where(p => p.Description.ToLower().Contains(Search)).ToList();
            }
            List<Post> PostTag = new List<Post>();
            if(TagSearch.Count != 0 && popAppSearch.Count!=0)
            {
                foreach (Post post in PostList)
                {
                    Tag[] tags = await ApiController.GetPostTags(post.ID);
                    foreach (Tag tag in TagSearch)
                    {
                        Tag check =tags.Where(t => t.ID == tag.ID).FirstOrDefault();
                        if (check != null)
                        {
                            PopApp[] popApps = await ApiController.GetPostPopApps(post.ID);
                            foreach (PopApp popApp in popApps)
                            {
                                PopApp checkPopApp = popApps.Where(p => p.ID == popApp.ID).FirstOrDefault();
                                if (checkPopApp != null)
                                {
                                    PostTag.Add(post);
                                    break;
                                }
                            }

                        }
                    }
                }
                LvPosts.ItemsSource = PostTag;
                GPost = PostTag.ToArray();
            }
            else if (TagSearch.Count != 0)
            {
                foreach (Post post in PostList)
                {
                    Tag[] tags = await ApiController.GetPostTags(post.ID);
                    foreach (Tag tag in TagSearch)
                    {
                        Tag check = tags.Where(t => t.ID == tag.ID).FirstOrDefault();
                        if (check != null)
                        {                          
                            PostTag.Add(post);
                            break;                             
                        }
                    }
                }
                LvPosts.ItemsSource = PostTag;
                GPost = PostTag.ToArray();
            }
            else if (popAppSearch.Count != 0)
            {
                foreach (Post post in PostList)
                {
                    PopApp[] popApps = await ApiController.GetPostPopApps(post.ID);
                    foreach (PopApp popApp in popApps)
                    {
                        PopApp checkPopApp = popApps.Where(p => p.ID == popApp.ID).FirstOrDefault();
                        if (checkPopApp != null)
                        {
                            PostTag.Add(post);
                            break;
                        }
                    }
                }
                LvPosts.ItemsSource = PostTag;
                GPost = PostTag.ToArray();
            }
            else
            {
                LvPosts.ItemsSource = PostList;
                GPost = PostList.ToArray();
            }        
        }
        public async void GetPosts()
        {
            Like[] Listlikes = await ApiController.GetUserLikes(GlobalInformation.currentUser.ID);

            Post[] posts = await ApiController.GetPostsAsync($"https://localhost:2222/api/Post");
            foreach (Post post in posts)
            {
                User user = await ApiController.GetUserAsync($"https://localhost:2222/api/User/{post.IDUser}");
                Tag[] tags = await ApiController.GetPostTags(post.ID);
                PopApp[] popApps = await ApiController.GetPostPopApps(post.ID);
                foreach(Like like in Listlikes)
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
                foreach (PopApp pop in popApps)
                {
                    post.PopApping += pop.NamePopApp + ", ";
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
                post.RepostCount= await ApiController.GetRepostCount(post.ID);
      
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
            Like[] Listlikes = await ApiController.GetUserLikes(GlobalInformation.currentUser.ID);
            //Post[] posts = await ApiController.GetPostsAsync($"https://localhost:2222/api/Post");
            foreach (Post post in GPost)
            {
                User user = await ApiController.GetUserAsync($"https://localhost:2222/api/User/{post.IDUser}");
                Tag[] tags = await ApiController.GetPostTags(post.ID);
                PopApp[] popApps = await ApiController.GetPostPopApps(post.ID);
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
                foreach (PopApp pop in popApps)
                {
                    post.PopApping += pop.NamePopApp + ", ";
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
                            User userComm = await ApiController.GetUserAsync($"https://localhost:2222/api/User/{comment.IDUser}");
                            comment.CommentNick = userComm.NickName;
                            byte[] profPictureComm = await ApiController.GetProfilePicture(userComm.ID);
                            comment.PostPP = profPictureComm;
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
            Like[] Listlikes = await ApiController.GetUserLikes(GlobalInformation.currentUser.ID);
            List<Post> posts = new List<Post>();
            Follower[] followers = await ApiController.GetCurrentUserFollowers(GlobalInformation.currentUser.ID);
            foreach (Follower follower in followers)
            {
                Post[] postes = await ApiController.GetPostsAsync($"https://localhost:2222/api/Post/{follower.IDFollowedUser}");
                foreach (Post post in postes)
                {
                    User user = await ApiController.GetUserAsync($"https://localhost:2222/api/User/{post.IDUser}");
                    Tag[] tags = await ApiController.GetPostTags(post.ID);
                    PopApp[] popApp = await ApiController.GetPostPopApps(post.ID);
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
                    foreach( PopApp pApps in popApp)
                    {
                        post.PopApping += pApps.NamePopApp + ", ";
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

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if(((sender as Button).DataContext as Post).LikeIcon == "HeartOutline")
            {
                Like newlike = new Like();
                newlike.IDUser= GlobalInformation.currentUser.ID;
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

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBlock)
            {
                var res = (sender as TextBlock).DataContext as Post;
                GlobalInformation.MainFrame.Navigate(new ProfilePage(res.IDUser));
            }
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is System.Windows.Controls.Image)
            {
                var res = (sender as System.Windows.Controls.Image).DataContext as Post;
                GlobalInformation.MainFrame.Navigate(new ProfilePage(res.IDUser));
            }
        }
    }
}
