using Microsoft.AspNetCore.Http;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ArtOkDesign.Classes
{
    internal class FilePic
    {
        public int ID { get; set; }
        public byte[] byteFile { get; set; }
    }
}
