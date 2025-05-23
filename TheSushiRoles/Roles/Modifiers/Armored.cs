namespace TheSushiRoles.Roles.Modifiers
{
    public static class Lucky 
    {
        public static PlayerControl Player;
        public static bool ProtectionBroken = false;
        public static void ClearAndReload() 
        {
            Player = null;
            ProtectionBroken = false;
        }
    }
}