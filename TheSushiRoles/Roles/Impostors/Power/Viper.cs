using System.Collections.Generic;
namespace TheSushiRoles.Roles
{
    public static class Viper 
    {
        public static PlayerControl Player;

        public static Color Color = Palette.ImpostorRed;

        public static HashSet<byte> BlindedPlayers = new HashSet<byte>();

        public static PlayerControl CurrentTarget;
        public static PlayerControl poisoned; 

        public static void ClearAndReload() 
        {
            Player = null;
            poisoned = null;
            CurrentTarget = null;
            BlindedPlayers = new HashSet<byte>();
        }
    }
}