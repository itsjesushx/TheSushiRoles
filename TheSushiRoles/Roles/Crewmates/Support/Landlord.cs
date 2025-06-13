using System;
using System.Collections.Generic;
namespace TheSushiRoles.Roles
{
    public static class Landlord
    {
        public static PlayerControl Player;
        public static PlayerControl FirstTarget;
        public static PlayerControl SecondTarget;

        public static bool SwappingMenus;

        public static int Charges;
        public static int RechargeTasksNumber;
        public static int RechargedTasks;

        public static Color Color = new Color32(141, 222, 133, byte.MaxValue);

        public static Dictionary<byte, DateTime> UnteleportablePlayers = new Dictionary<byte, DateTime>();
        public static void ClearAndReload()
        {
            Player = null;
            FirstTarget = null;
            SecondTarget = null;
            UnteleportablePlayers = new Dictionary<byte, DateTime>();
            SwappingMenus = false;
            Charges = CustomGameOptions.LandlordCharges / 2;
            RechargeTasksNumber = CustomGameOptions.LandlordRechargeTasksNumber;
            RechargedTasks = CustomGameOptions.LandlordRechargeTasksNumber;
        }
    }
}