namespace TheSushiRoles.Roles
{
    public static class Crusader
    {
        public static PlayerControl Player;
        public static PlayerControl FortifiedPlayer;
        public static PlayerControl CurrentTarget;

        public static Color Color = new Color32(255, 134, 69, byte.MaxValue);

        public static bool Fortified;
        public static int Charges;
        public static int RechargeTasksNumber;
        public static int RechargedTasks;
        public static void ClearAndReload()
        {
            Player = null;
            CurrentTarget = null;
            Fortified = false;
            FortifiedPlayer = null;
            Charges = CustomGameOptions.CrusaderCharges;
            RechargeTasksNumber = CustomGameOptions.CrusaderRechargeTasksNumber;
            RechargedTasks = CustomGameOptions.CrusaderRechargeTasksNumber;
        }
    }
}