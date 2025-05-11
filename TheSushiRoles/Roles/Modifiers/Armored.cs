namespace TheSushiRoles.Roles.Modifiers
{
    public static class Armored 
    {
        public static PlayerControl Player;
        public static bool isBrokenArmor = false;
        public static void ClearAndReload() 
        {
            Player = null;
            isBrokenArmor = false;
        }
    }
}