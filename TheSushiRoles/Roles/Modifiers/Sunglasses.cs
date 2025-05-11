using System.Collections.Generic;

namespace TheSushiRoles.Roles.Modifiers
{
    public static class Sunglasses 
    {
        public static List<PlayerControl> Players = new List<PlayerControl>();
        public static int vision = 1;
        public static void ClearAndReload() 
        {
            Players = new List<PlayerControl>();
            vision = CustomOptionHolder.modifierSunglassesVision.GetSelection() + 1;
        }
    }
}