namespace TheSushiRoles.Roles
{
    public static class Pestilence
    {
        public static PlayerControl Player;
        
        public static PlayerControl CurrentTarget;

        public static Color Color = new Color32(66, 66, 66, byte.MaxValue);
        public static void ClearAndReload()
        {
            Player = null;
            CurrentTarget = null;
        }
    }
}