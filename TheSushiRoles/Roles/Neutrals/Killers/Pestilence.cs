using UnityEngine;

namespace TheSushiRoles.Roles
{
    public static class Pestilence
    {
        public static PlayerControl Player;
        public static Color Color = new Color32(66, 66, 66, byte.MaxValue);
        public static PlayerControl CurrentTarget;
        public static float Cooldown = 30f;
        public static bool CanUseVents;
        public static void ClearAndReload()
        {
            Player = null;
            Cooldown = CustomOptionHolder.PestilenceCooldown.GetFloat();
            CurrentTarget = null;
            CanUseVents = CustomOptionHolder.PestilenceCanUseVents.GetBool();
        }
    }
}