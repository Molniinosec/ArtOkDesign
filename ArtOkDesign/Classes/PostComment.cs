using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtOkDesign.Classes
{
    public class PostComment
    {

        public int ID { get; set; }
        public int IDPost { get; set; }
        public int IDUser { get; set; }
        public string Comment { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<System.DateTime> TimeOfWrite { get; set; }

        public Post Post { get; set; }
        public User User { get; set; }
    }
}
