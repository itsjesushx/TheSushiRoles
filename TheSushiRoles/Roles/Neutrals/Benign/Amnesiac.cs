using System.Collections.Generic;
using UnityEngine;

namespace TheSushiRoles.Roles
{
    public static class Amnesiac
    {
        public static PlayerControl Player;
        public static List<byte> PlayersToRemember = new List<byte>();
        public static Color Color = new Color32(34, 255, 255, byte.MaxValue);
        public static bool Remembered;
        public static void ClearAndReload()
        {
            Player = null;
            PlayersToRemember = new List<byte>();
            Remembered = false;
        }
    }
}