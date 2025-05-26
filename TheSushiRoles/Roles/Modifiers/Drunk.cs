using System.Collections.Generic;
using UnityEngine;

namespace TheSushiRoles.Roles.Modifiers
{
    public static class Drunk 
    {
        public static List<PlayerControl> Players = new List<PlayerControl>();
        public static Color Color = new Color32(117, 128, 0, byte.MaxValue);
        public static int meetings = 3;
        public static void ClearAndReload() 
        {
            Players = new List<PlayerControl>();
            meetings = (int)CustomOptionHolder.modifierDrunkDuration.GetFloat();
        }
    }
}