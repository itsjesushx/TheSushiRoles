namespace TheSushiRoles.Roles.Abilities
{
    public static class Coward
    {
        public static PlayerControl Player;
        public static bool CanUse => Player.RemainingEmergencies > 0;
        public static void ClearAndReload()
        {
            Player = null;
        }
    }
}