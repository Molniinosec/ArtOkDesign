using ArtOkDesign.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using ArtOkDesign.Classes;
using System.Windows.Controls;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using Microsoft.AspNetCore.Http;

namespace ArtOkDesign.Classes
{
    public class Post
    {
        public int ID { get; set; }
        public int IDUser { get; set; }
        public string Description { get; set; }
        public System.DateTime TimeOfCreate { get; set; }
        public Nullable<bool> IsDeleted { get; set; }

        public ICollection<Like> Like { get; set; }
        public ICollection<Picture> Picture { get; set; }
        public ICollection<PostTag> PostTag { get; set; }
        public ICollection<User> User { get; set; }
        public ICollection<PostComment> PostComment { get; set; }
        public ICollection<PostPopApp> PostPopApp { get; set; }
        public ICollection<Repost> Repost { get; set; }

        public string PostNickName { get; set; }

        public int PostLikes { get; set; }
        public byte[] PostImage { get; set; }
        public BitmapImage IFile { get; set; }
    }   
}
