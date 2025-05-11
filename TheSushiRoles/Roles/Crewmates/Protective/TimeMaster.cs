using UnityEngine;

namespace TheSushiRoles.Roles
{
    public static class TimeMaster 
    {
        public static PlayerControl Player;
        public static Color Color = new Color32(112, 142, 239, byte.MaxValue);
        public static bool ReviveDuringRewind = false;
        public static float RewindTimeDuration = 3f;
        public static float Cooldown = 30f;
        public static bool isRewinding = false;
        private static Sprite ButtonSprite;
        public static Sprite GetButtonSprite() 
        {
            if (ButtonSprite) return ButtonSprite;
            ButtonSprite = Utils.LoadSpriteFromResources("TheSushiRoles.Resources.TimeMasterButton.png", 115f);
            return ButtonSprite;
        }

        public static void ClearAndReload() 
        {
            Player = null;
            isRewinding = false;
            RewindTimeDuration = CustomOptionHolder.timeMasterRewindTime.GetFloat();
            ReviveDuringRewind = CustomOptionHolder.TimeMasterReviveDuringRewind.GetBool();
            Cooldown = CustomOptionHolder.timeMasterCooldown.GetFloat();
        }
    }
}