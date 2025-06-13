using System.Collections.Generic;

namespace TheSushiRoles.Roles
{
    public static class Trapper
    {
        public static PlayerControl Player;
        public static Color Color = new Color32(110, 57, 105, byte.MaxValue);
        public static int RechargeTasksNumber = 3;
        public static int RechargedTasks = 3;
        public static int Charges = 1;
        public static List<byte> playersOnMap = new List<Byte>();

        public static void ClearAndReload()
        {
            Player = null;
            RechargeTasksNumber = CustomGameOptions.TrapperRechargeTasksNumber;
            RechargedTasks = CustomGameOptions.TrapperRechargeTasksNumber;
            Charges = CustomGameOptions.TrapperMaxCharges / 2;
            playersOnMap = new();
        }
    }
}