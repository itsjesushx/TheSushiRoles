namespace TheSushiRoles.Roles
{
    public static class Sheriff 
    {
        public static PlayerControl Player;
        public static Color Color = new Color32(248, 205, 70, byte.MaxValue);
        public static PlayerControl CurrentTarget;
        public static void ClearAndReload() 
        {
            Player = null;
            CurrentTarget = null;
        }
    }
}