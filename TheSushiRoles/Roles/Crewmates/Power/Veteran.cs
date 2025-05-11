using UnityEngine;

namespace TheSushiRoles.Roles
{
    public static class Veteran
    {
        public static PlayerControl Player;
        public static Color Color = new Color32(153, 128, 64, byte.MaxValue);
        public static float Cooldown;
        public static float Duration;
        public static bool AlertActive;
        public static int Charges;
        public static int RechargeTasksNumber;
        public static int RechargedTasks;
        private static Sprite ButtonSprite;
        public static Sprite GetButtonSprite() 
        {
            if (ButtonSprite) return ButtonSprite;
            ButtonSprite = Utils.LoadSpriteFromResources("TheSushiRoles.Resources.VeteranButton.png", 115f);
            return ButtonSprite;
        }
        public static void ClearAndReload()
        {
            Player = null;
            Cooldown = CustomOptionHolder.VeteranCooldown.GetFloat();
            Duration = CustomOptionHolder.VeteranDuration.GetFloat();
            AlertActive = false;
            Charges = Mathf.RoundToInt(CustomOptionHolder.VeteranCharges.GetFloat());
            RechargeTasksNumber = Mathf.RoundToInt(CustomOptionHolder.VeteranRechargeTasksNumber.GetFloat());
            RechargedTasks = Mathf.RoundToInt(CustomOptionHolder.VeteranRechargeTasksNumber.GetFloat());
        }
    }
}