using UnityEngine;

namespace TheSushiRoles.Roles
{
    public static class Sidekick 
    {
        public static PlayerControl Player;
        public static Color Color = new Color32(0, 180, 235, byte.MaxValue);

        public static PlayerControl CurrentTarget;

        public static bool wasTeamRed;
        public static bool wasImpostor;
        public static bool wasSpy;

        public static float Cooldown = 30f;
        public static bool canUseVents = true;
        public static bool canKill = true;
        public static bool promotesToJackal = true;
        public static void ClearAndReload() 
        {
            Player = null;
            CurrentTarget = null;
            Cooldown = CustomOptionHolder.jackalKillCooldown.GetFloat();
            canUseVents = CustomOptionHolder.sidekickCanUseVents.GetBool();
            canKill = CustomOptionHolder.sidekickCanKill.GetBool();
            promotesToJackal = CustomOptionHolder.sidekickPromotesToJackal.GetBool();
            wasTeamRed = wasImpostor = wasSpy = false;
        }
    }
}