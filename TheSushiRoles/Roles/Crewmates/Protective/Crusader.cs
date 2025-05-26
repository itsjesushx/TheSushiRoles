using UnityEngine;

namespace TheSushiRoles.Roles
{
    public static class Crusader
    {
        public static PlayerControl Player;
        public static Color Color = new Color32(255, 134, 69, byte.MaxValue);
        public static PlayerControl CurrentTarget;
        public static bool Fortified;
        public static float Cooldown;
        public static PlayerControl FortifiedPlayer;
        private static Sprite ButtonSprite;
        public static Sprite GetButtonSprite() 
        {
            if (ButtonSprite) return ButtonSprite;
            ButtonSprite = Utils.LoadSprite("TheSushiRoles.Resources.Fortify.png", 115f);
            return ButtonSprite;
        }
        public static int Charges;
        public static int RechargeTasksNumber;
        public static int RechargedTasks;
        public static void ClearAndReload()
        {
            Player = null;
            CurrentTarget = null;
            Fortified = false;
            Cooldown = CustomOptionHolder.CrusaderCooldown.GetFloat();
            FortifiedPlayer = null;
            Charges = Mathf.RoundToInt(CustomOptionHolder.CrusaderCharges.GetFloat());
            RechargeTasksNumber = Mathf.RoundToInt(CustomOptionHolder.CrusaderRechargeTasksNumber.GetFloat());
            RechargedTasks = Mathf.RoundToInt(CustomOptionHolder.CrusaderRechargeTasksNumber.GetFloat());
        }
    }
}