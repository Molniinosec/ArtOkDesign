using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtOkDesign.Classes
{
    public class DialogUser
    {
        public int ID { get; set; }
        public int IDUser { get; set; }
        public int IDDialog { get; set; }

        public Dialog Dialog { get; set; }
        public User User { get; set; }
        public ICollection<Messages> Message { get; set; }
    }
}
