using UnityEngine;

namespace TheSushiRoles.Roles.Modifiers
{
    public static class Lucky 
    {
        public static PlayerControl Player;
        public static bool ProtectionBroken = false;
        public static Color Color = new Color32(255, 128, 128, byte.MaxValue);
        public static void ClearAndReload()
        {
            Player = null;
            ProtectionBroken = false;
        }
    }
}