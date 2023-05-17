using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtOkDesign.Classes
{
    public class Messages
    {
        public int ID { get; set; }
        public int IDUserDialog { get; set; }
        public string Message { get; set; }
        public Nullable<System.DateTime> TimeOfSend { get; set; }
        public string FilePath { get; set; }
        public Nullable<bool> IsEdit { get; set; }

        public DialogUser DialogUser { get; set; }
        
        public byte[] MessPP { get; set; } 
        public string MessNickName { get; set; }
        public string GridHorizont { get; set; }
        public string MessPPVisibility { get; set; }
        public string gridWidth { get; set; }
        public string GridBack { get; set; }
    }
}
