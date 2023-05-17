using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtOkDesign.Classes
{
    public class Like
    {
        [NotMapped]
        public int ID { get; set; }
        public int IDPost { get; set; }
        public int IDUser { get; set; }
        public Nullable<System.DateTime> DateOfLike { get; set; }
        public Post Post { get; set; }
        public User User { get; set; }
    }
}
