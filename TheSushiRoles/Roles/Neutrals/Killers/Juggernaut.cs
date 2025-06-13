namespace TheSushiRoles.Roles
{
    public static class Juggernaut
    {
        public static PlayerControl Player;
        public static PlayerControl CurrentTarget;
        public static float Cooldown = 25f;
        public static Color Color = new Color32(140, 0, 77, byte.MaxValue);
        public static void FixCooldown()
        {
            Cooldown -= CustomGameOptions.JuggernautReducedCooldown;
            if (Cooldown <= 0f) Cooldown = 0f;
        }
        public static void ClearAndReload()
        {
            Player = null;
            CurrentTarget = null;
            Cooldown = CustomGameOptions.JuggernautCooldown;
        }
    }
}