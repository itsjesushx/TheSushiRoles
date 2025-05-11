using System.Collections.Generic;
using UnityEngine;

namespace TheSushiRoles.Roles
{
    public static class Vulture 
    {
        public static PlayerControl Player;
        public static Color Color = new Color32(139, 69, 19, byte.MaxValue);
        public static List<Arrow> localArrows = new List<Arrow>();
        public static float Cooldown = 30f;
        public static int vultureNumberToWin = 4;
        public static int eatenBodies = 0;
        public static bool IsVultureWin = false;
        public static bool canUseVents = true;
        public static bool showArrows = true;
        private static Sprite ButtonSprite;
        public static Sprite GetButtonSprite() 
        {
            if (ButtonSprite) return ButtonSprite;
            ButtonSprite = Utils.LoadSpriteFromResources("TheSushiRoles.Resources.VultureButton.png", 115f);
            return ButtonSprite;
        }

        public static void ClearAndReload() 
        {
            Player = null;
            vultureNumberToWin = Mathf.RoundToInt(CustomOptionHolder.vultureNumberToWin.GetFloat());
            eatenBodies = 0;
            Cooldown = CustomOptionHolder.vultureCooldown.GetFloat();
            IsVultureWin = false;
            canUseVents = CustomOptionHolder.vultureCanUseVents.GetBool();
            showArrows = CustomOptionHolder.vultureShowArrows.GetBool();
            if (localArrows != null) 
            {
                foreach (Arrow arrow in localArrows)
                    if (arrow?.arrow != null)
                        UnityEngine.Object.Destroy(arrow.arrow);
            }
            localArrows = new List<Arrow>();
        }
    }
}