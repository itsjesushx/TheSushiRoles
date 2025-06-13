using System.Collections.Generic;
namespace TheSushiRoles.Roles.Modifiers
{
    public static class Bait 
    {
        public static List<PlayerControl> Players = new List<PlayerControl>();
        public static Dictionary<DeadPlayer, float> active = new Dictionary<DeadPlayer, float>();
        public static Color Color = new Color32(0, 247, 255, byte.MaxValue);
        public static void ClearAndReload() 
        {
            Players = new List<PlayerControl>();
            active = new Dictionary<DeadPlayer, float>();
        }
    }
}