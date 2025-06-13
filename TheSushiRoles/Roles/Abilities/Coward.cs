namespace TheSushiRoles.Roles.Abilities
{
    public static class Coward
    {
        public static PlayerControl Player;
        public static bool CanUse => Player.RemainingEmergencies > 0;
        public static Color Color = new Color32(118, 184, 194, byte.MaxValue);
        public static void ClearAndReload()
        {
            Player = null;
        }
    }
}