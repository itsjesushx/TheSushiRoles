using System.Collections.Generic;
using UnityEngine;

namespace TheSushiRoles.Roles.Modifiers
{
    public static class Vip 
    {
        public static List<PlayerControl> Players = new List<PlayerControl>();
        public static Color Color = new Color32(222, 194, 122, byte.MaxValue);
        public static bool showColor = true;

        public static void ClearAndReload() 
        {
            Players = new List<PlayerControl>();
            showColor = CustomOptionHolder.modifierVipShowColor.GetBool();
        }
    }
}