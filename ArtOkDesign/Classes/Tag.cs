using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtOkDesign.Classes
{
    public class Tag
    {
        public int ID { get; set; }
        public string NameTag { get; set; }

        public ICollection<PostTag> PostTag { get; set; }
    }
}
