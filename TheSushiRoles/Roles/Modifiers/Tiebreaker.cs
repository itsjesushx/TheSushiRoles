using UnityEngine;

namespace TheSushiRoles.Roles.Modifiers
{
    public static class Tiebreaker 
    {
        public static PlayerControl Player;
        public static bool isTiebreak = false;
        public static Color Color = new Color32(142, 237, 147, byte.MaxValue);
        public static void ClearAndReload()
        {
            Player = null;
            isTiebreak = false;
        }
    }
}