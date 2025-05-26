using System;
using System.Collections.Generic;
using UnityEngine;

namespace TheSushiRoles.Roles
{
    public static class Trapper 
        {
        public static PlayerControl Player;
        public static Color Color = new Color32(110, 57, 105, byte.MaxValue);

        public static float Cooldown = 30f;
        public static int maxCharges = 5;
        public static int RechargeTasksNumber = 3;
        public static int RechargedTasks = 3;
        public static int Charges = 1;
        public static int trapCountToReveal = 2;
        public static List<byte> playersOnMap = new List<Byte>();
        public static bool anonymousMap = false;
        public static int infoType = 0; // 0 = Role, 1 = Good/Evil, 2 = Name
        public static float trapDuration = 5f; 

        private static Sprite trapButtonSprite;

        public static Sprite GetButtonSprite() 
        {
            if (trapButtonSprite) return trapButtonSprite;
            trapButtonSprite = Utils.LoadSprite("TheSushiRoles.Resources.Trapper_Place_Button.png", 115f);
            return trapButtonSprite;
        }

        public static void ClearAndReload() 
        {
            Player = null;
            Cooldown = CustomOptionHolder.trapperCooldown.GetFloat();
            maxCharges = Mathf.RoundToInt(CustomOptionHolder.trapperMaxCharges.GetFloat());
            RechargeTasksNumber = Mathf.RoundToInt(CustomOptionHolder.trapperRechargeTasksNumber.GetFloat());
            RechargedTasks = Mathf.RoundToInt(CustomOptionHolder.trapperRechargeTasksNumber.GetFloat());
            Charges = Mathf.RoundToInt(CustomOptionHolder.trapperMaxCharges.GetFloat()) / 2;
            trapCountToReveal = Mathf.RoundToInt(CustomOptionHolder.trapperTrapNeededTriggerToReveal.GetFloat());
            playersOnMap = new();
            anonymousMap = CustomOptionHolder.trapperAnonymousMap.GetBool();
            infoType = CustomOptionHolder.trapperInfoType.GetSelection();
            trapDuration = CustomOptionHolder.trapperTrapDuration.GetFloat();
        }
    }
}