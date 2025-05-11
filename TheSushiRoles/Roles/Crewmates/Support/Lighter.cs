using UnityEngine;

namespace TheSushiRoles.Roles
{
    public static class Lighter 
    {
        public static PlayerControl Player;
        public static Color Color = new Color32(200, 190, 150, byte.MaxValue);
        public static float lighterModeLightsOnVision = 2f;
        public static float lighterModeLightsOffVision = 0.75f;
        public static float flashlightWidth = 0.75f;
        public static void ClearAndReload() 
        {
            Player = null;
            flashlightWidth = CustomOptionHolder.lighterFlashlightWidth.GetFloat();
            lighterModeLightsOnVision = CustomOptionHolder.lighterModeLightsOnVision.GetFloat();
            lighterModeLightsOffVision = CustomOptionHolder.lighterModeLightsOffVision.GetFloat();
        }
    }
}