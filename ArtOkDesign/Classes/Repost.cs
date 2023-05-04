using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtOkDesign.Classes
{
    public class Repost
    {
        public int ID { get; set; }
        public int IDUser { get; set; }
        public int IDRepostedPost { get; set; }
        public Nullable<System.DateTime> DateOfRepost { get; set; }

        public Post Post { get; set; }
        public User User { get; set; }
    }
}
