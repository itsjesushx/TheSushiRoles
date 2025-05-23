using UnityEngine;

namespace TheSushiRoles.Roles
{
    public static class Assassin 
    {
        public static PlayerControl Player;
        public static Color Color = Palette.ImpostorRed;
        public static PlayerControl AssassinMarked;
        public static PlayerControl CurrentTarget;
        public static float Cooldown = 30f;
        public static float traceTime = 1f;
        public static bool knowsTargetLocation = false;
        public static float invisibleDuration = 5f;
        public static float invisibleTimer = 0f;
        public static bool isInvisble = false;
        private static Sprite markButtonSprite;
        private static Sprite killButtonSprite;
        public static Arrow arrow = new Arrow(Color.black);
        public static Sprite GetMarkButtonSprite() 
        {
            if (markButtonSprite) return markButtonSprite;
            markButtonSprite = Utils.LoadSpriteFromResources("TheSushiRoles.Resources.AssassinMarkButton.png", 115f);
            return markButtonSprite;
        }

        public static Sprite GetKillButtonSprite() 
        {
            if (killButtonSprite) return killButtonSprite;
            killButtonSprite = Utils.LoadSpriteFromResources("TheSushiRoles.Resources.AssassinAssassinateButton.png", 115f);
            return killButtonSprite;
        }

        public static void ClearAndReload()
        {
            Player = null;
            CurrentTarget = AssassinMarked = null;
            Cooldown = CustomOptionHolder.AssassinCooldown.GetFloat();
            knowsTargetLocation = CustomOptionHolder.AssassinKnowsTargetLocation.GetBool();
            traceTime = CustomOptionHolder.AssassinTraceTime.GetFloat();
            invisibleDuration = CustomOptionHolder.AssassinInvisibleDuration.GetFloat();
            invisibleTimer = 0f;
            isInvisble = false;
            if (arrow?.arrow != null) UnityEngine.Object.Destroy(arrow.arrow);
            arrow = new Arrow(Color.black);
            if (arrow.arrow != null) arrow.arrow.SetActive(false);
        }
    }
}