using UnityEngine;

namespace TheSushiRoles.Roles
{
    public static class Wraith
    {
        public static PlayerControl Player;
        public static float Cooldown;
        public static float Duration;
        private static Sprite ButtonSprite;
        public static float VanishTimer;
        public static Color Color = Palette.ImpostorRed;
        public static bool IsVanished = false;
        public static Sprite GetButtonSprite()
        {
            if (ButtonSprite) return ButtonSprite;
            ButtonSprite = Utils.LoadSprite("TheSushiRoles.Resources.VanishButton.png", 115f);
            return ButtonSprite;
        }
        public static void ClearAndReload()
        {
            Player = null;
            Cooldown = CustomOptionHolder.WraithCooldown.GetFloat();
            Duration = CustomOptionHolder.WraithDuration.GetFloat();
            IsVanished = false;
            VanishTimer = 0f;
        }
    }
}