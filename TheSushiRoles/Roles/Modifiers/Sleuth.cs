using System.Collections.Generic;

namespace TheSushiRoles.Roles.Modifiers
{
    public static class Sleuth
    {
        public static List<byte> Reported = new List<byte>();
        public static List<PlayerControl> Players = new List<PlayerControl>();
        public static void ClearAndReload()
        {
            Reported = new List<byte>();
            Players = new List<PlayerControl>();
        }
    }
}