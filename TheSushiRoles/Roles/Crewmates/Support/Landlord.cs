using System;
using System.Collections.Generic;
using UnityEngine;

namespace TheSushiRoles.Roles
{
    public static class Landlord
    {
        public static PlayerControl Player;
        public static PlayerControl FirstTarget;
        public static PlayerControl SecondTarget;
        public static float Cooldown = 30f;
        public static bool SwappingMenus;
        public static int Charges;
        public static int RechargeTasksNumber;
        public static int RechargedTasks;
        public static Color Color = new Color32(141, 222, 133, byte.MaxValue);
        public static Dictionary<byte, DateTime> UnteleportablePlayers = new Dictionary<byte, DateTime>();
        private static Sprite ButtonSprite;        
        public static Sprite GetButtonSprite()
        {
            if (ButtonSprite) return ButtonSprite;
            ButtonSprite = Utils.LoadSprite("TheSushiRoles.Resources.LandlordButton.png", 115f);
            return ButtonSprite;
        }
        public static void ClearAndReload()
        {
            Player = null;
            FirstTarget = null;
            SecondTarget = null;
            UnteleportablePlayers = new Dictionary<byte, DateTime>();
            SwappingMenus = false;
            Cooldown = CustomOptionHolder.LandlordCooldown.GetFloat();
            Charges = Mathf.RoundToInt(CustomOptionHolder.LandlordCharges.GetFloat()) / 2;
            RechargeTasksNumber = Mathf.RoundToInt(CustomOptionHolder.LandlordRechargeTasksNumber.GetFloat());
            RechargedTasks = Mathf.RoundToInt(CustomOptionHolder.LandlordRechargeTasksNumber.GetFloat());
        }
    }
}