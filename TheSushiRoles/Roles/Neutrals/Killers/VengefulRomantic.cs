namespace TheSushiRoles.Roles
{
    public static class VengefulRomantic
    {
        public static PlayerControl Player;
        public static PlayerControl CurrentTarget;
        public static bool notAckedExiled = false;
        public static float Cooldown;
        public static bool CanUseVents;
        public static PlayerControl Lover;
        public static void ClearAndReload()
        {
            CurrentTarget = null;
            Player = null;
            Lover = null;
            notAckedExiled = false;
            Cooldown = CustomOptionHolder.VengefulRomanticCooldown.GetFloat();
            CanUseVents = CustomOptionHolder.VengefulRomanticCanUseVents.GetBool();
        }
    }
}