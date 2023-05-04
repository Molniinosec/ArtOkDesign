using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtOkDesign.Classes
{
    public class PopApp
    {
        public int ID { get; set; }
        public string NamePopApp { get; set; }
        public byte[] Logo { get; set; }

        public ICollection<PostPopApp> PostPopApp { get; set; }
    }
}
