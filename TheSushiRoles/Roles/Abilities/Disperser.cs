using UnityEngine;

namespace TheSushiRoles.Roles.Abilities
{
    public static class Disperser
    {
        public static PlayerControl Player;
        public static float Cooldown = 30f;
        public static int Charges;
        public static int RechargeKillsCount;
        public static Sprite ButtonSprite;
        public static Sprite GetButtonSprite()
        {
            if (ButtonSprite) return ButtonSprite;
            ButtonSprite = Utils.LoadSpriteFromResources("TheSushiRoles.Resources.DisperseButton.png", 135f);
            return ButtonSprite;
        }
        public static void ClearAndReload()
        {
            Player = null;
            Charges = Mathf.RoundToInt(CustomOptionHolder.ModifierDisperserCharges.GetFloat());
            RechargeKillsCount = Mathf.RoundToInt(CustomOptionHolder.ModifierDisperserKillCharges.GetFloat());
            Cooldown = CustomOptionHolder.ModifierDisperserCooldown.GetFloat() / 2;
        }
    }
}