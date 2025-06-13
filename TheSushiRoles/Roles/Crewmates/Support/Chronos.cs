using System.Collections.Generic;

namespace TheSushiRoles.Roles
{
    public static class Chronos 
    {
        public static PlayerControl Player;
        public static Color Color = new Color32(112, 142, 239, byte.MaxValue);
        public static float TimeRemaining = 0f;
        public static float Charges;
        public static bool isRewinding = false;
        public static Dictionary<byte, float> RecentlyDied;
        public static void ClearAndReload() 
        {
            Player = null;
            isRewinding = false;
            Charges = CustomGameOptions.ChronosCharges / 2;
            TimeRemaining = 0f;
            RecentlyDied = new Dictionary<byte, float>();
        }
    }
}