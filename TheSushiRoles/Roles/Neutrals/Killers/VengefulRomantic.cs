namespace TheSushiRoles.Roles
{
    public static class VengefulRomantic
    {
        public static PlayerControl Player;
        public static PlayerControl CurrentTarget;
        public static PlayerControl Lover;
        
        public static void ClearAndReload()
        {
            CurrentTarget = null;
            Player = null;
            Lover = null;
        }
    }
}