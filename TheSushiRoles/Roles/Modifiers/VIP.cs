using System.Collections.Generic;

namespace TheSushiRoles.Roles.Modifiers
{
    public static class Vip 
    {
        public static List<PlayerControl> Players = new List<PlayerControl>();
        public static bool showColor = true;

        public static void ClearAndReload() 
        {
            Players = new List<PlayerControl>();
            showColor = CustomOptionHolder.modifierVipShowColor.GetBool();
        }
    }
}