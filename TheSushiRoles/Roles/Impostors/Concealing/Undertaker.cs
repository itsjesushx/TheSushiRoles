namespace TheSushiRoles.Roles
{
    public static class Undertaker
    {
        public static PlayerControl Player;
        public static DeadBody CurrentTarget;
        public static void ClearAndReload()
        {
            Player = null;
            CurrentTarget = null;
        }
    }
}