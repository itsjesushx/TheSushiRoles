using UnityEngine;

namespace TheSushiRoles.Roles
{
    public static class Janitor 
    {
        public static PlayerControl Player;
        public static Color Color = Palette.ImpostorRed;

        public static float Cooldown = 30f;

        private static Sprite ButtonSprite;
        public static Sprite GetButtonSprite() 
        {
            if (ButtonSprite) return ButtonSprite;
            ButtonSprite = Utils.LoadSpriteFromResources("TheSushiRoles.Resources.CleanButton.png", 115f);
            return ButtonSprite;
        }

        public static void ClearAndReload() 
        {
            Player = null;
            Cooldown = CustomOptionHolder.JanitorCooldown.GetFloat();
        }
    }
}