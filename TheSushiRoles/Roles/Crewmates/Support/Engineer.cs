namespace TheSushiRoles.Roles
{
    public static class Engineer 
    {
        public static PlayerControl Player;
        
        public static Color Color = new Color32(0, 40, 245, byte.MaxValue);
    
        public static int remainingFixes = 1;
        public static void ClearAndReload() 
        {
            Player = null;
            remainingFixes = CustomGameOptions.EngineerNumberOfFixes;
        }
    }
}