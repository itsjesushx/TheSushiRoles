using System.Collections.Generic;
using UnityEngine;

namespace TheSushiRoles.Roles.Modifiers
{
    public static class Blind 
    {
        public static List<PlayerControl> Players = new List<PlayerControl>();
        public static Color Color = Color.gray;
        public static int vision = 1;
        public static void ClearAndReload() 
        {
            Players = new List<PlayerControl>();
            vision = CustomOptionHolder.modifierBlindVision.GetSelection() + 1;
        }
    }
}