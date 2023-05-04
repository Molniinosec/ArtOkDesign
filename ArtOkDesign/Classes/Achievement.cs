using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtOkDesign.Classes
{
    public class Achievement
    {
        public int ID { get; set; }
        public string AchievementName { get; set; }
        public string Description { get; set; }

        public ICollection<UserAchievement> UserAchievement { get; set; }
    }
}
