using System.Collections.Generic;

namespace TheSushiRoles.Roles.Modifiers
{
    public static class Blind 
    {
        public static List<PlayerControl> Players = new List<PlayerControl>();
        public static Color Color = Color.gray;
        public static void ClearAndReload() 
        {
            Players = new List<PlayerControl>();
        }
    }
}