namespace TheSushiRoles.Roles
{
    public static class Janitor 
    {
        public static PlayerControl Player;
        public static Color Color = Palette.ImpostorRed;

        public static void ClearAndReload() 
        {
            Player = null;
        }
    }
}