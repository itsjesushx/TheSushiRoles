using UnityEngine;

namespace TheSushiRoles.Roles.Abilities
{
    public static class FlashLight 
    {
        public static PlayerControl Player;
        public static Color Color = new Color32(200, 190, 150, byte.MaxValue);
        public static float AbilityFlashlightModeLightsOnVision = 2f;
        public static float AbilityFlashlightModeLightsOffVision = 0.75f;
        public static float flashlightWidth = 0.75f;
        public static void ClearAndReload() 
        {
            Player = null;
            flashlightWidth = CustomOptionHolder.AbilityFlashlightFlashlightWidth.GetFloat();
            AbilityFlashlightModeLightsOnVision = CustomOptionHolder.AbilityFlashlightModeLightsOnVision.GetFloat();
            AbilityFlashlightModeLightsOffVision = CustomOptionHolder.AbilityFlashlightModeLightsOffVision.GetFloat();
        }
    }
}