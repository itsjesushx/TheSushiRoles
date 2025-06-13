using System.Collections.Generic;

namespace TheSushiRoles.Roles
{
    public static class Monarch
    {
        public static PlayerControl Player;
        public static PlayerControl CurrentTarget;
        public static List<PlayerControl> KnightedPlayers = new List<PlayerControl>();

        public static int Charges;
        public static int RechargeTasksNumber;
        public static int RechargedTasks;

        public static Color Color = new Color32(255, 132, 0, byte.MaxValue);
        public static void ClearAndReload()
        {
            Player = null;
            CurrentTarget = null;
            KnightedPlayers = new List<PlayerControl>();
            Charges = CustomGameOptions.MonarchCharges / 2;
            RechargeTasksNumber = CustomGameOptions.MonarchRechargeTasksNumber;
            RechargedTasks = CustomGameOptions.MonarchRechargeTasksNumber;
        }
    }
}