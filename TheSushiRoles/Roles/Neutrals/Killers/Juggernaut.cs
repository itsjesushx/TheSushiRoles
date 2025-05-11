using UnityEngine;

namespace TheSushiRoles.Roles
{
    public static class Juggernaut
    {
        public static PlayerControl Player;
        public static PlayerControl CurrentTarget;
        public static float Cooldown = 30f;
        public static bool CanUseVents;
        public static Color Color = new Color32(140, 0, 77, byte.MaxValue);
        public static float ReducedCooldown = 5f;
        public static void FixCooldown()
        {
            Cooldown -= ReducedCooldown;
            if (Cooldown <= 0f) Cooldown = 0f;
        }
        public static void ClearAndReload()
        {
            Player = null;
            CurrentTarget = null;
            Cooldown = CustomOptionHolder.JuggernautCooldown.GetFloat();
            CanUseVents = CustomOptionHolder.JuggernautCanUseVents.GetBool();
            ReducedCooldown = CustomOptionHolder.JuggernautReducedCooldown.GetFloat();
        }
    }
}