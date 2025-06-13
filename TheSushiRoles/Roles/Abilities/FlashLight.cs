namespace TheSushiRoles.Roles.Abilities
{
    public static class FlashLight 
    {
        public static PlayerControl Player;
        public static Color Color = new Color32(200, 190, 150, byte.MaxValue);
        public static void ClearAndReload() 
        {
            Player = null;
        }
    }
}