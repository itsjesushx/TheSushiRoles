using UnityEngine;

namespace TheSushiRoles.Roles
{
    public static class Ninja 
    {
        public static PlayerControl Player;
        public static Color Color = Palette.ImpostorRed;

        public static PlayerControl ninjaMarked;
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
            markButtonSprite = Utils.LoadSpriteFromResources("TheSushiRoles.Resources.NinjaMarkButton.png", 115f);
            return markButtonSprite;
        }

        public static Sprite GetKillButtonSprite() 
        {
            if (killButtonSprite) return killButtonSprite;
            killButtonSprite = Utils.LoadSpriteFromResources("TheSushiRoles.Resources.NinjaAssassinateButton.png", 115f);
            return killButtonSprite;
        }

        public static void ClearAndReload() {
            Player = null;
            CurrentTarget = ninjaMarked = null;
            Cooldown = CustomOptionHolder.ninjaCooldown.GetFloat();
            knowsTargetLocation = CustomOptionHolder.ninjaKnowsTargetLocation.GetBool();
            traceTime = CustomOptionHolder.ninjaTraceTime.GetFloat();
            invisibleDuration = CustomOptionHolder.ninjaInvisibleDuration.GetFloat();
            invisibleTimer = 0f;
            isInvisble = false;
            if (arrow?.arrow != null) UnityEngine.Object.Destroy(arrow.arrow);
            arrow = new Arrow(Color.black);
            if (arrow.arrow != null) arrow.arrow.SetActive(false);
        }
    }
}