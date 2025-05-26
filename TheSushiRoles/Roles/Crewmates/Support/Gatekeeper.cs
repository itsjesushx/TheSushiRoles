using UnityEngine;

namespace TheSushiRoles.Roles
{
    public static class Gatekeeper 
    {
        public static PlayerControl Player;
        public static Color Color = new Color32(69, 69, 169, byte.MaxValue);

        public static float Cooldown;
        public static float usePortalCooldown;
        public static bool logOnlyHasColors;
        public static bool logShowsTime;
        public static bool canPortalFromAnywhere;

        private static Sprite placePortalButtonSprite;
        private static Sprite usePortalButtonSprite;
        private static Sprite usePortalSpecialButtonSprite1;
        private static Sprite usePortalSpecialButtonSprite2;
        private static Sprite logSprite;

        public static Sprite GetPlacePortalButtonSprite() 
        {
            if (placePortalButtonSprite) return placePortalButtonSprite;
            placePortalButtonSprite = Utils.LoadSprite("TheSushiRoles.Resources.PlacePortalButton.png", 115f);
            return placePortalButtonSprite;
        }

        public static Sprite getUsePortalButtonSprite() 
        {
            if (usePortalButtonSprite) return usePortalButtonSprite;
            usePortalButtonSprite = Utils.LoadSprite("TheSushiRoles.Resources.UsePortalButton.png", 115f);
            return usePortalButtonSprite;
        }

        public static Sprite GetUsePortalSpecialButtonSprite(bool first) 
        {
            if (first) 
            {
                if (usePortalSpecialButtonSprite1) return usePortalSpecialButtonSprite1;
                usePortalSpecialButtonSprite1 = Utils.LoadSprite("TheSushiRoles.Resources.UsePortalSpecialButton1.png", 115f);
                return usePortalSpecialButtonSprite1;
            } 
            else 
            {
                if (usePortalSpecialButtonSprite2) return usePortalSpecialButtonSprite2;
                usePortalSpecialButtonSprite2 = Utils.LoadSprite("TheSushiRoles.Resources.UsePortalSpecialButton2.png", 115f);
                return usePortalSpecialButtonSprite2;
            }
        }

        public static Sprite GetLogSprite() 
        {
            if (logSprite) return logSprite;
            logSprite = FastDestroyableSingleton<HudManager>.Instance.UseButton.fastUseSettings[ImageNames.DoorLogsButton].Image;
            return logSprite;
        }

        public static void ClearAndReload() 
        {
            Player = null;
            Cooldown = CustomOptionHolder.GatekeeperCooldown.GetFloat();
            usePortalCooldown = CustomOptionHolder.GatekeeperUsePortalCooldown.GetFloat();
            logOnlyHasColors = CustomOptionHolder.GatekeeperLogOnlyColorType.GetBool();
            logShowsTime = CustomOptionHolder.GatekeeperLogHasTime.GetBool();
            canPortalFromAnywhere = CustomOptionHolder.GatekeeperCanPortalFromAnywhere.GetBool();
        }
    }
}