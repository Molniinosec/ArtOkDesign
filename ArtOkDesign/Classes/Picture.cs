using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtOkDesign.Classes
{
    public class Picture
    {
        public int ID { get; set; }
        public string PicturePath { get; set; }
        public int PostID { get; set; }
        public Post Post { get; set; }
    }
}
