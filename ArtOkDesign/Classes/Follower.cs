using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtOkDesign.Classes
{
    public class Follower
    {
        public int ID { get; set; }
        public int IDCurrentUser { get; set; }
        public int IDFollowedUser { get; set; }
        public Nullable<System.DateTime> TimeOfFollowing { get; set; }
        public bool IsBlockListed { get; set; }
        public User CUser { get; set; }
        public User FUser { get; set; }
    }
}
