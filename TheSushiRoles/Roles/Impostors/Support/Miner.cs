namespace TheSushiRoles.Roles
{
    public static class Miner
    {
        public static PlayerControl Player;
        
        public static Color Color = Palette.ImpostorRed;
        public static void ClearAndReload() 
        {
            Player = null;
        }
    }
}