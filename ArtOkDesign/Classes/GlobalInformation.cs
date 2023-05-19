using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ArtOkDesign.Classes
{
    public class GlobalInformation
    {
        public static User currentUser { get; set; }
        public static Image postImage { get; set; }
        public static string ImagePath { get; set; }
        public static FileDialog FileDialogFile { get; set; }
        public static Post[] Posts { get; set; }
        public static Frame MainFrame { get; set; } = new Frame();
    }
}
