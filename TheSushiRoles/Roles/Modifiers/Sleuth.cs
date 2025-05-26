using System.Collections.Generic;
using UnityEngine;

namespace TheSushiRoles.Roles.Modifiers
{
    public static class Sleuth
    {
        public static List<byte> Reported = new List<byte>();
        public static Color Color = new Color32(92, 50, 50, byte.MaxValue);
        public static List<PlayerControl> Players = new List<PlayerControl>();
        public static void ClearAndReload()
        {
            Reported = new List<byte>();
            Players = new List<PlayerControl>();
        }
    }
}