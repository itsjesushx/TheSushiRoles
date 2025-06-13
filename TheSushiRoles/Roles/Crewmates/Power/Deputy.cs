using System.Collections.Generic;
namespace TheSushiRoles.Roles
{
    public static class Deputy
    {
        public static PlayerControl Player;
        public static Color Color = Color.yellow;
        public static List<GameObject> ExecuteButtons = new List<GameObject>();
        public static int Charges;
        public static int RechargeTasksNumber;
        public static int RechargedTasks;
        public static bool CanExecute = true;
        public static void ClearAndReload()
        {
            Player = null;
            CanExecute = true;
            Charges = CustomGameOptions.DeputyCharges;
            RechargeTasksNumber = CustomGameOptions.DeputyRechargeTasksNumber;
            RechargedTasks = CustomGameOptions.DeputyRechargeTasksNumber;
        }
    }
}