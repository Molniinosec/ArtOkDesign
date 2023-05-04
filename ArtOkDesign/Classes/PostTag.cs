using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtOkDesign.Classes
{
    public class PostTag
    {
        public int ID { get; set; }
        public int IDPost { get; set; }
        public int IDTag { get; set; }

        public Post Post { get; set; }
        public Tag Tag { get; set; }
    }
}
