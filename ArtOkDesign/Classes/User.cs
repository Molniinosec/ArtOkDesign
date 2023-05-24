using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtOkDesign.Classes
{
    public class User
    {
        public int ID { get; set; }
        public string NickName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ProfilePicture { get; set; }
        public string BackgroundPicture { get; set; }
        public System.DateTime TimeOfRegistration { get; set; }
        public System.DateTime Birthday { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<bool> IsModerator { get; set; }

        public ICollection<DialogUser> DialogUser { get; set; }
        public ICollection<Follower> Follower { get; set; }
        public ICollection<Follower> Follower1 { get; set; }
        public ICollection<Like> Like { get; set; }
        public ICollection<Post> Post { get; set; }
        public ICollection<PostComment> PostComment { get; set; }
        public ICollection<Repost> Repost { get; set; }
        public ICollection<UserAchievement> UserAchievement { get; set; }

        public string IconCheck { get; set; }
    }
}
