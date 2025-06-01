namespace TheSushiRoles.Roles
{
    public static class VengefulRomantic
    {
        public static PlayerControl Player;
        public static PlayerControl CurrentTarget;
        public static float Cooldown;
        public static bool CanUseVents;
        public static PlayerControl Lover;
        public static void ClearAndReload()
        {
            CurrentTarget = null;
            Player = null;
            Lover = null;
            Cooldown = CustomOptionHolder.VengefulRomanticCooldown.GetFloat();
            CanUseVents = CustomOptionHolder.VengefulRomanticCanUseVents.GetBool();
        }
    }
}