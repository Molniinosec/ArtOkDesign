using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtOkDesign.Classes
{
    public class UserAchievement
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int AchievementID { get; set; }
        public System.DateTime DateOfGeting { get; set; }

        public Achievement Achievement { get; set; }
        public User User { get; set; }
    }
}
