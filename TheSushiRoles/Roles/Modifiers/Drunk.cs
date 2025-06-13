using System.Collections.Generic;
namespace TheSushiRoles.Roles.Modifiers
{
    public static class Drunk 
    {
        public static List<PlayerControl> Players = new List<PlayerControl>();
        public static Color Color = new Color32(117, 128, 0, byte.MaxValue);
        public static int meetings;
        public static void ClearAndReload() 
        {
            Players = new List<PlayerControl>();
            meetings = (int)CustomGameOptions.ModifierDrunkDuration;
        }
    }
}