using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtOkDesign.Classes
{
    public class Dialog
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public System.DateTime DateOfCreation { get; set; }
        public string DialogPicture { get; set; }

        public ICollection<DialogUser> DialogUser { get; set; }
        
        public List<Messages> DialogMessages { get; set; }
    }
}
