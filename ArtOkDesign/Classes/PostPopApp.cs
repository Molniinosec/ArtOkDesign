using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;

namespace ArtOkDesign.Classes
{
    public class PostPopApp
    {
        public int ID { get; set; }
        public int IDPost { get; set; }
        public int IDPopApp { get; set; }

        public PopApp PopApp { get; set; }
        public Post Post { get; set; }
    }
}
