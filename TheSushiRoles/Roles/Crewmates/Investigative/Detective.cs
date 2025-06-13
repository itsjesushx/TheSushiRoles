namespace TheSushiRoles.Roles
{
    public static class Detective 
    {
        public static PlayerControl Player;
        public static Color Color = new Color32(45, 106, 165, byte.MaxValue);
        public static float timer = 6.2f;
        public static void ClearAndReload() 
        {
            Player = null;
        }
    }   
}