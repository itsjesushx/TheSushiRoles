using UnityEngine;

namespace TheSushiRoles.Roles
{
    public static class Grenadier
    {
        public static PlayerControl Player;
        public static Color Color = Palette.ImpostorRed;
        public static float Cooldown = 30f;
        public static bool Active;
        public static float GrenadeDuration = 5f;
        public static float GrenadeRadius = 1f;
        public static Il2CppSystem.Collections.Generic.List<PlayerControl> ClosestPlayers = null;
        public static Il2CppSystem.Collections.Generic.List<PlayerControl> FlashedPlayers = new Il2CppSystem.Collections.Generic.List<PlayerControl>();
        private static Sprite ButtonSprite;
        public static Sprite GetButtonSprite() 
        {
            if (ButtonSprite) return ButtonSprite;
            ButtonSprite = Utils.LoadSpriteFromResources("TheSushiRoles.Resources.GrenadierButton.png", 115f);
            return ButtonSprite;
        }

        public static void ClearAndReload() 
        {
            Player = null;
            Active = false;
            Cooldown = CustomOptionHolder.GrenadierCooldown.GetFloat();
            GrenadeDuration = CustomOptionHolder.GrenadierGrenadeDuration.GetFloat();
            GrenadeRadius = CustomOptionHolder.GrenadierGrenadeRadius.GetFloat();
        }
    }
}