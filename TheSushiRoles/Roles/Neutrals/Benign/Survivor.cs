using System.Collections.Generic;

namespace TheSushiRoles.Roles
{
    public static class Survivor 
    {        
        public static PlayerControl Player;
        public static PlayerControl target;
        public static List<PlayerControl> blankedList = new List<PlayerControl>();

        public static Color Color = new Color32(255, 227, 105, byte.MaxValue);

        public static int blanks = 0;
        public static void ClearAndReload() 
        {
            Player = null;
            target = null;
            blankedList = new List<PlayerControl>();
            blanks = 0;
        }
    }
}