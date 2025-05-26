using UnityEngine;

namespace TheSushiRoles.Roles
{
    public static class Werewolf
    {
        public static Color Color = new Color32(168, 102, 41, byte.MaxValue);
        public static PlayerControl Player;
        public static PlayerControl CurrentTarget;
        public static float Cooldown;
        public static float Radius;
        public static bool CanUseVents;
        private static Sprite ButtonSprite;
        public static Sprite GetButtonSprite() 
        {
            if (ButtonSprite) return ButtonSprite;
            ButtonSprite = Utils.LoadSprite("TheSushiRoles.Resources.Maul.png", 115f);
            return ButtonSprite;
        }
        public static void ClearAndReload()
        {
            CurrentTarget = null;
            Player = null;
            Cooldown = CustomOptionHolder.WerewolfCooldown.GetFloat();
            CanUseVents = CustomOptionHolder.WerewolfCanUseVents.GetBool();
            Radius = CustomOptionHolder.WerewolfMaulRadius.GetFloat();
        }
    }
}