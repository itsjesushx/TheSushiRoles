using UnityEngine;

namespace TheSushiRoles.Roles
{
    public static class Romantic
    {
        public static PlayerControl Player;
        public static PlayerControl beloved;
        public static bool HasLover;
        public static PlayerControl CurrentTarget;
        public static Color Color = new Color32(255, 102, 204, byte.MaxValue);
        public static bool RomanticKnowsRole = false;
        private static Sprite ButtonSprite;
        public static bool IsAdditionalRomanticBonusWin = false;
        public static bool IsAdditionalBelovedBonusWin = false;
        public static Sprite GetButtonSprite()
        {
            if (ButtonSprite) return ButtonSprite;
            ButtonSprite = Utils.LoadSprite("TheSushiRoles.Resources.Romantic.png", 115f);
            return ButtonSprite;
        }
        public static void ClearAndReload(bool ClearBeloved = true) 
        {
            Player = null;
            HasLover = false;
            if (ClearBeloved) 
            {
                beloved = null;
            }
            IsAdditionalRomanticBonusWin = false;
            IsAdditionalBelovedBonusWin = false;
            RomanticKnowsRole = CustomOptionHolder.RomanticKnowsRole.GetBool();
        }
    }
}