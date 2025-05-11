using UnityEngine;

namespace TheSushiRoles.Roles
{
    public static class Poisoner 
    {
        public static PlayerControl Player;
        public static Color Color = Palette.ImpostorRed;

        public static float delay = 10f;
        public static float Cooldown = 30f;

        public static PlayerControl CurrentTarget;
        public static PlayerControl poisoned; 

        private static Sprite ButtonSprite;
        public static Sprite GetButtonSprite() 
        {
            if (ButtonSprite) return ButtonSprite;
            ButtonSprite = Utils.LoadSpriteFromResources("TheSushiRoles.Resources.PoisonButton.png", 115f);
            return ButtonSprite;
        }

        public static void ClearAndReload() 
        {
            Player = null;
            poisoned = null;
            CurrentTarget = null;
            delay = CustomOptionHolder.poisonerKillDelay.GetFloat();
            Cooldown = CustomOptionHolder.poisonerCooldown.GetFloat();
        }
    }
}