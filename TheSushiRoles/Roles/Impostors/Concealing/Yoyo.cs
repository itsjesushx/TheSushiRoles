using UnityEngine;

namespace TheSushiRoles.Roles
{
    public static class Yoyo 
    {
        public static PlayerControl Player = null;
        public static Color Color = Palette.ImpostorRed;

        public static float blinkDuration = 0;
        public static float markCooldown = 0;
        public static bool markStaysOverMeeting = false;
        public static bool hasAdminTable = false;
        public static float adminCooldown = 0;
        public static float SilhouetteVisibility => (silhouetteVisibility == 0 && (PlayerControl.LocalPlayer == Player || PlayerControl.LocalPlayer.Data.IsDead)) ? 0.1f : silhouetteVisibility;
        public static float silhouetteVisibility = 0;

        public static Vector3? markedLocation = null;

        private static Sprite markButtonSprite;

        public static Sprite GetMarkButtonSprite() 
        {
            if (markButtonSprite) return markButtonSprite;
            markButtonSprite = Utils.LoadSprite("TheSushiRoles.Resources.YoyoMarkButtonSprite.png", 115f);
            return markButtonSprite;
        }
        private static Sprite blinkButtonSprite;
        public static Sprite GetBlinkButtonSprite() 
        {
            if (blinkButtonSprite) return blinkButtonSprite;
            blinkButtonSprite = Utils.LoadSprite("TheSushiRoles.Resources.YoyoBlinkButtonSprite.png", 115f);
            return blinkButtonSprite;
        }

        public static void MarkLocation(UnityEngine.Vector3 position) 
        {
            markedLocation = position;
        }

        public static void ClearAndReload() 
        {
            blinkDuration = CustomOptionHolder.yoyoBlinkDuration.GetFloat();
            markCooldown = CustomOptionHolder.yoyoMarkCooldown.GetFloat();
            markStaysOverMeeting = CustomOptionHolder.yoyoMarkStaysOverMeeting.GetBool();
            hasAdminTable = CustomOptionHolder.yoyoHasAdminTable.GetBool();
            adminCooldown = CustomOptionHolder.yoyoAdminTableCooldown.GetFloat();
            silhouetteVisibility = CustomOptionHolder.yoyoSilhouetteVisibility.GetSelection() / 10f;
            markedLocation = null;
        }
    }
}