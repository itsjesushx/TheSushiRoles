using UnityEngine;

namespace TheSushiRoles.Roles
{
    public static class Trickster 
    {
        public static PlayerControl Player;
        public static Color Color = Palette.ImpostorRed;
        public static float placeBoxCooldown = 30f;
        public static float lightsOutCooldown = 30f;
        public static float lightsOutDuration = 10f;
        public static float lightsOutTimer = 0f;
        private static Sprite placeBoxButtonSprite;
        private static Sprite lightOutButtonSprite;
        private static Sprite tricksterVentButtonSprite;

        public static Sprite GetPlaceBoxButtonSprite() 
        {
            if (placeBoxButtonSprite) return placeBoxButtonSprite;
            placeBoxButtonSprite = Utils.LoadSprite("TheSushiRoles.Resources.PlaceJackInTheBoxButton.png", 115f);
            return placeBoxButtonSprite;
        }

        public static Sprite GetLightsOutButtonSprite() 
        {
            if (lightOutButtonSprite) return lightOutButtonSprite;
            lightOutButtonSprite = Utils.LoadSprite("TheSushiRoles.Resources.LightsOutButton.png", 115f);
            return lightOutButtonSprite;
        }

        public static Sprite GetTricksterVentButtonSprite() 
        {
            if (tricksterVentButtonSprite) return tricksterVentButtonSprite;
            tricksterVentButtonSprite = Utils.LoadSprite("TheSushiRoles.Resources.TricksterVentButton.png", 115f);
            return tricksterVentButtonSprite;
        }

        public static void ClearAndReload() 
        {
            Player = null;
            lightsOutTimer = 0f;
            placeBoxCooldown = CustomOptionHolder.tricksterPlaceBoxCooldown.GetFloat();
            lightsOutCooldown = CustomOptionHolder.tricksterLightsOutCooldown.GetFloat();
            lightsOutDuration = CustomOptionHolder.tricksterLightsOutDuration.GetFloat();
            JackInTheBox.UpdateStates(); // if the role is erased, we might have to update the state of the created objects
        }
    }
}