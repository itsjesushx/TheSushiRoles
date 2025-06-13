namespace TheSushiRoles.Roles
{
    public static class Wraith
    {
        public static PlayerControl Player;
        
        public static Color Color = Palette.ImpostorRed;

        public static float VanishTimer;
        public static bool IsVanished = false;
        public static void ClearAndReload()
        {
            Player = null;
            IsVanished = false;
            VanishTimer = 0f;
        }
    }
}