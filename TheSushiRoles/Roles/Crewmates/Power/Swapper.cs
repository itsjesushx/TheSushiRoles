using System;
using UnityEngine;

namespace TheSushiRoles.Roles
{
    public static class Swapper 
    {
        public static PlayerControl Player;
        public static Color Color = new Color32(134, 55, 86, byte.MaxValue);
        private static Sprite spriteCheck;
        public static bool canCallEmergency = false;
        public static bool canOnlySwapOthers = false;
        public static int Charges;
        public static float RechargeTasksNumber;
        public static float RechargedTasks;
 
        public static byte playerId1 = Byte.MaxValue;
        public static byte playerId2 = Byte.MaxValue;

        public static Sprite GetCheckSprite() 
        {
            if (spriteCheck) return spriteCheck;
            spriteCheck = Utils.LoadSprite("TheSushiRoles.Resources.SwapperCheck.png", 150f);
            return spriteCheck;
        }

        public static void ClearAndReload() 
        {
            Player = null;
            playerId1 = Byte.MaxValue;
            playerId2 = Byte.MaxValue;
            canCallEmergency = CustomOptionHolder.swapperCanCallEmergency.GetBool();
            canOnlySwapOthers = CustomOptionHolder.swapperCanOnlySwapOthers.GetBool();
            Charges = Mathf.RoundToInt(CustomOptionHolder.swapperSwapsNumber.GetFloat());
            RechargeTasksNumber = Mathf.RoundToInt(CustomOptionHolder.swapperRechargeTasksNumber.GetFloat());
            RechargedTasks = Mathf.RoundToInt(CustomOptionHolder.swapperRechargeTasksNumber.GetFloat());
        }
    }
}