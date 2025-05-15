using System.Collections.Generic;
using UnityEngine;

namespace TheSushiRoles.Roles
{
    public static class TimeMaster 
    {
        public static PlayerControl Player;
        public static Color Color = new Color32(112, 142, 239, byte.MaxValue);
        public static bool ReviveDuringRewind = false;
        public static float RewindTimeDuration = 3f;
        public static float TimeRemaining = 0f;
        public static float Charges;
        public static float Cooldown = 30f;
        public static bool isRewinding = false;
        public static Dictionary<byte, float> RecentlyDied;
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
            Charges = Mathf.RoundToInt(CustomOptionHolder.TimeMasterCharges.GetFloat());
            TimeRemaining = 0f;
            RecentlyDied = new Dictionary<byte, float>();
        }
    }
}