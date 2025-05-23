using System.Collections.Generic;

namespace TheSushiRoles.Roles.Modifiers
{
    public static class Drunk 
    {
        public static List<PlayerControl> Players = new List<PlayerControl>();
        public static int meetings = 3;
        public static void ClearAndReload() 
        {
            Players = new List<PlayerControl>();
            meetings = (int) CustomOptionHolder.modifierDrunkDuration.GetFloat();
        }
    }
}