using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Media;
using System.Drawing;
using Microsoft.Win32;
using System.Windows.Media.Imaging;
using Microsoft.AspNetCore.Http;
using System.Security.Permissions;
using System.Windows.Shapes;

namespace ArtOkDesign.Classes
{
    internal class ApiController
    {
        public static HttpClient client = new HttpClient();


        public static async Task RunAsync()
        {
            // Update port # in the following line.
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            //ServicePointManager.Expect100Continue = false;
            client.BaseAddress = new Uri("http://localhost:2222/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.ConnectionClose = true;
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static async Task<User[]> GetUsersAsync()
        {
            User[] user = null;
            var response = await client.GetAsync($"https://localhost:2222/api/User");
            if (response.IsSuccessStatusCode)
            {
                user = await response.Content.ReadAsAsync<User[]>();
            }
            return user;
        }
        public static async Task<User> GetUserAsync(string path)
        {
            User user = null;
            var response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                user = await response.Content.ReadAsAsync<User>();             
            }
            return user;
        }
        public static async Task<Post[]> GetPostsAsync(string path)
        {
            Post[] posts = null;
            var response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                posts = await response.Content.ReadAsAsync<Post[]>();
            }   
            return posts;
        }
        public static async Task<Post[]> GetCurrentUserPosts(int IDUser)
        {
            Post[] posts = null;
            var response = await client.GetAsync($"https://localhost:2222/api/Post/{IDUser}");
            if (response.IsSuccessStatusCode)
            {
                posts = await response.Content.ReadAsAsync<Post[]>();
            }
            return posts;
        }
        public static async Task<User> CheckUser(User user)
        {
            HttpResponseMessage response = await client.GetAsync($"https://localhost:2222/api/User/UserExist?login={user.NickName}&password={user.Password}");
            if (response.IsSuccessStatusCode)
            {
                User message = await response.Content.ReadAsAsync<User>();
                return message;

            }
            return null;
        }
        public static async Task<Follower[]> GetFollowerAsync()
        {
            Follower[] follower = null;
            var response = await client.GetAsync($"https://localhost:2222/api/Follower");
            if (response.IsSuccessStatusCode)
            {
                follower = await response.Content.ReadAsAsync<Follower[]>();
            }
            return follower;
        }
        public static async Task<PostComment[]> GetPostComments(int IdPost)
        {
            PostComment[] comments = null;
            var responce = await client.GetAsync($"https://localhost:2222/api/PostComment/{IdPost}");
            if (responce.IsSuccessStatusCode)
            {
                comments=await responce.Content.ReadAsAsync<PostComment[]>();
            }
            return comments;
        }
        public static async Task<Dialog[]> GetUserDialogs(int IDUser)
        {
            Dialog[] dialogs = null;
            var responce = await client.GetAsync($"https://localhost:2222/api/DialogUser/{IDUser}");
            if (responce.IsSuccessStatusCode)
            {
                dialogs = await responce.Content.ReadAsAsync<Dialog[]>();
            }
            return dialogs;
        }
        public static async Task<DialogUser[]> GetAllUsersInDialog(int IDDialog)
        {
            DialogUser[] dialogs = null;
            var response = await client.GetAsync($"https://localhost:2222/api/DialogUser/AllDialogs-{IDDialog}");
            if (response.IsSuccessStatusCode)
            {
                dialogs = await response.Content.ReadAsAsync<DialogUser[]>();
            }
            return dialogs;
        }
        public static async Task<Like[]> GetUserLikes(int iDUser)
        {
            Like[] likes = null;
            var response = await client.GetAsync($"https://localhost:2222/api/Like/User-{iDUser}");
            if (response.IsSuccessStatusCode)
            {
                likes = await response.Content.ReadAsAsync<Like[]>();
            }
            return likes;
        }
        public static async Task<string> AdLike(Like like)
        {
            var res = await client.PostAsJsonAsync($"https://localhost:2222/api/Like/AddLike", like);
            return await res.Content.ReadAsStringAsync();
        }
        public static async Task<string> DeleteLike(int IDlike)
        {
            var res = await client.DeleteAsync($"https://localhost:2222/api/Like/DeleteLike-{IDlike}");
            return await res.Content.ReadAsStringAsync();
        }
        public static async Task<Messages[]> GetAllMessageInDialog(int IDUserDialog)
        {
            Messages[] messages = null;
            var response = await client.GetAsync($"https://localhost:2222/api/DialogUser/Messages-{IDUserDialog}");
            if (response.IsSuccessStatusCode)
            {
                messages = await response.Content.ReadAsAsync<Messages[]>();
            }
            return messages;
        }
        public static async Task<Follower[]> GetCurrentUserFollowers(int IDUser)
        {
            Follower[] follower = null;
            var response = await client.GetAsync($"https://localhost:2222/api/Follower/{IDUser}");
            if (response.IsSuccessStatusCode)
            {
                follower = await response.Content.ReadAsAsync<Follower[]>();
            }
            return follower;
            
        }
        public static async Task<int> GetPopAppCount(int IDPost)
        {
            int count = 0;
            var response = await client.GetAsync($"https://localhost:2222/api/PopApp/PopApps-{IDPost}");
            if (response.IsSuccessStatusCode)
            {
                count = await response.Content.ReadAsAsync<int>();
            }
            return count;
        }

        public static async Task<int> GetTagCount(int IDPost)
        {
            int count = 0;
            var response = await client.GetAsync($"https://localhost:2222/api/Tag/TagCount-{IDPost}");
            if (response.IsSuccessStatusCode)
            {
                count = await response.Content.ReadAsAsync<int>();
            }
            return count;
        }
        public static async Task<int> GetCommentCount(int IDPost)
        {
            int count = 0;
            var response = await client.GetAsync($"https://localhost:2222/api/PostComment/comment-{IDPost}");
            if (response.IsSuccessStatusCode)
            {
                count = await response.Content.ReadAsAsync<int>();
            }
            return count;
        }
        public static async Task<int> GetFollowersCount(int IDUser)
        {
            int count = 0;
            var response = await client.GetAsync($"https://localhost:2222/api/Follower/CountFollowers/{IDUser}");
            if (response.IsSuccessStatusCode)
            {
                count = await response.Content.ReadAsAsync<int>();
            }
            return count;
        }
        public static async Task<int> GetFollowedCount(int IDUser)
        {
            int count = 0;
            var response = await client.GetAsync($"https://localhost:2222/api/Follower/CountFollowed/{IDUser}");
            if (response.IsSuccessStatusCode)
            {
                count = await response.Content.ReadAsAsync<int>();
            }
            return count;
        }
        public static async Task<byte[]> GetProfileBackgroun(int UserID)
        {
            byte[] file = null;
            var responce = await client.GetAsync($"https://localhost:2222/api/User/{UserID}/ProfilePicture");
            if (responce.IsSuccessStatusCode)
            {
                file = await responce.Content.ReadAsByteArrayAsync();
            }
            return file;
        }
        public static async Task<Tag[]> GetAllTags()
        {
            Tag[] file = null;
            var responce = await client.GetAsync($"https://localhost:2222/api/Tag");
            if (responce.IsSuccessStatusCode)
            {
                file = await responce.Content.ReadAsAsync<Tag[]>();
            }
            return file;
        }
        public static async Task<string> PushPostTag(PostTag postTag)
        {
            var responce = await client.PostAsJsonAsync($"https://localhost:2222/api/Tag/PostTagSave", postTag);
            return await responce.Content.ReadAsStringAsync();

        }
        public static async Task<byte[]> GetProfilePicture(int UserID)
        {
            byte[] file = null;
            var responce = await client.GetAsync($"https://localhost:2222/api/User/{UserID}/ProfileBackGround");
            if (responce.IsSuccessStatusCode)
            {
                file = await responce.Content.ReadAsByteArrayAsync();
            }
            return file;
        }
        public static async Task<int> GetPostLikes(int IDPost)
        {
            int likes = 0;
            var responce = await client.GetAsync($"https://localhost:2222/api/Like/{IDPost}");
            if (responce.IsSuccessStatusCode)
            {
                likes = await responce.Content.ReadAsAsync<int>();
            }
            return likes;
        }
        public static async Task<Tag[]> GetPostTags(int IDPost)
        {
            Tag[] tags = null;
            var responce = await client.GetAsync($"https://localhost:2222/api/Tag/followers{IDPost}");
            if (responce.IsSuccessStatusCode)
            {
                tags = await responce.Content.ReadAsAsync<Tag[]>();
            }
            return tags;
        }
        public static async Task<byte[]> GetImage(int IDPost)
        {
            byte[] file = null;
            var responce = await client.GetAsync($"https://localhost:2222/api/Image/{IDPost}");
            if (responce.IsSuccessStatusCode)
            {
                file = await responce.Content.ReadAsByteArrayAsync();
            }
            return file;
        }
        public static async Task<string> PushPost(Post post)
        {
            var res = await client.PostAsJsonAsync($"https://localhost:2222/api/Post/CreatePost", post);

            return await res.Content.ReadAsStringAsync();
        }
        public static async Task<string> PushComment(PostComment postComment)
        {
            var res = await client.PostAsJsonAsync($"https://localhost:2222/api/PostComment/CreateComment", postComment);

            return await res.Content.ReadAsStringAsync();
        }
        public static async Task<string> PushImage(Post post, byte[] image)
        {
            var responce = await GetPostsAsync($"https://localhost:2222/api/Post/{post.IDUser}");
            //var formContent = new MultipartFormDataContent
            //{
            //    {new StringContent($"{responce[0].ID}"), "ID"},
            //    {new StreamContent(new MemoryStream(image)), "IFile"}
            //};
            //var fileStreamContent = new StreamContent(File.OpenRead(GlobalInformation.ImagePath));
            //formContent.Add(fileStreamContent, "Ifile");

            FilePic filePic = new FilePic();
            filePic.ID= responce[0].ID;
            filePic.byteFile = image;
            //filePic.Ifile =new BitmapImage(new Uri(GlobalInformation.FileDialogFile.FileName));


            var responcePic = await client.PostAsJsonAsync($"https://localhost:2222/api/Image/PicturePath", filePic);
            string res = await responcePic.Content.ReadAsStringAsync();
            return res; ;
        }

    }
}
