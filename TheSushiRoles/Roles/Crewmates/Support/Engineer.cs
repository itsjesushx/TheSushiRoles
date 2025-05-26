using UnityEngine;

namespace TheSushiRoles.Roles
{
    public static class Engineer 
    {
        public static PlayerControl Player;
        public static Color Color = new Color32(0, 40, 245, byte.MaxValue);
        public static int remainingFixes = 1;
        public static bool highlightForImpostors = true;
        public static bool HighlightForNeutralKillers = true;
        private static Sprite ButtonSprite;
        public static Sprite GetButtonSprite() 
        {
            if (ButtonSprite) return ButtonSprite;
            ButtonSprite = Utils.LoadSprite("TheSushiRoles.Resources.RepairButton.png", 115f);
            return ButtonSprite;
        }

        public static void ClearAndReload() 
        {
            Player = null;
            remainingFixes = Mathf.RoundToInt(CustomOptionHolder.engineerNumberOfFixes.GetFloat());
            highlightForImpostors = CustomOptionHolder.engineerHighlightForImpostors.GetBool();
            HighlightForNeutralKillers = CustomOptionHolder.engineerHighlightForNeutralKillers.GetBool();
        }
    }
}