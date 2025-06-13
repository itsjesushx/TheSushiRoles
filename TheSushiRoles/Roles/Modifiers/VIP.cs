using System.Collections.Generic;
namespace TheSushiRoles.Roles.Modifiers
{
    public static class Vip 
    {
        public static List<PlayerControl> Players = new List<PlayerControl>();
        public static Color Color = new Color32(222, 194, 122, byte.MaxValue);
        public static void ClearAndReload() 
        {
            Players = new List<PlayerControl>();
        }
    }
}