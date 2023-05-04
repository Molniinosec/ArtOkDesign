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
