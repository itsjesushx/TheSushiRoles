using UnityEngine;

namespace TheSushiRoles.Roles
{
    public static class Warlock 
    {
        public static PlayerControl Player;
        public static Color Color = Palette.ImpostorRed;

        public static PlayerControl CurrentTarget;
        public static PlayerControl curseVictim;
        public static PlayerControl curseVictimTarget;

        public static float Cooldown = 25f;
        public static float rootTime = 5f;

        private static Sprite curseButtonSprite;
        private static Sprite curseKillButtonSprite;

        public static Sprite GetCurseButtonSprite() 
        {
            if (curseButtonSprite) return curseButtonSprite;
            curseButtonSprite = Utils.LoadSprite("TheSushiRoles.Resources.CurseButton.png", 115f);
            return curseButtonSprite;
        }

        public static Sprite GetCurseKillButtonSprite() 
        {
            if (curseKillButtonSprite) return curseKillButtonSprite;
            curseKillButtonSprite = Utils.LoadSprite("TheSushiRoles.Resources.CurseKillButton.png", 85f);
            return curseKillButtonSprite;
        }

        public static void ClearAndReload() 
        {
            Player = null;
            CurrentTarget = null;
            curseVictim = null;
            curseVictimTarget = null;
            Cooldown = CustomOptionHolder.warlockCooldown.GetFloat();
            rootTime = CustomOptionHolder.warlockRootTime.GetFloat();
        }

        public static void ResetCurse() 
        {
            HudManagerStartPatch.warlockCurseButton.Timer = HudManagerStartPatch.warlockCurseButton.MaxTimer;
            HudManagerStartPatch.warlockCurseButton.Sprite = GetCurseButtonSprite();
            HudManagerStartPatch.warlockCurseButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
            CurrentTarget = null;
            curseVictim = null;
            curseVictimTarget = null;
        }
    }
}