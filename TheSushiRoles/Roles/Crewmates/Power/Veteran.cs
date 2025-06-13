namespace TheSushiRoles.Roles
{
    public static class Veteran
    {
        public static PlayerControl Player;
        public static Color Color = new Color32(153, 128, 64, byte.MaxValue);
        public static bool AlertActive;
        public static int Charges;
        public static int RechargeTasksNumber;
        public static int RechargedTasks;
        public static void ClearAndReload()
        {
            Player = null;
            AlertActive = false;
            Charges = CustomGameOptions.VeteranCharges / 2;
            RechargeTasksNumber = CustomGameOptions.VeteranRechargeTasksNumber;
            RechargedTasks = CustomGameOptions.VeteranRechargeTasksNumber;
        }
    }
}