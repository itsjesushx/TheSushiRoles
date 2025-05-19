using System.Collections.Generic;
using UnityEngine;

namespace TheSushiRoles.Roles
{
    public static class Monarch
    {
        public static PlayerControl Player;
        public static float Cooldown = 30f;
        public static PlayerControl CurrentTarget;
        public static List<PlayerControl> KnightedPlayers = new List<PlayerControl>();
        public static bool KnightLoseOnDeath;
        public static int Charges;
        public static int RechargeTasksNumber;
        public static int RechargedTasks;
        public static Color Color = new Color32(255, 132, 0, byte.MaxValue);
        private static Sprite ButtonSprite;
        public static Sprite GetButtonSprite() 
        {
            if (ButtonSprite) return ButtonSprite;
            ButtonSprite = Utils.LoadSpriteFromResources("TheSushiRoles.Resources.KnightButton.png", 115f);
            return ButtonSprite;
        }
        public static void ClearAndReload()
        {
            Player = null;
            CurrentTarget = null;
            KnightedPlayers = new List<PlayerControl>();
            KnightLoseOnDeath = CustomOptionHolder.KnightLoseOnDeath.GetBool();
            Cooldown = CustomOptionHolder.MonarchKnightCooldown.GetFloat();
            Charges = Mathf.RoundToInt(CustomOptionHolder.MonarchCharges.GetFloat()) / 2;
            RechargeTasksNumber = Mathf.RoundToInt(CustomOptionHolder.MonarchRechargeTasksNumber.GetFloat());
            RechargedTasks = Mathf.RoundToInt(CustomOptionHolder.MonarchRechargeTasksNumber.GetFloat());
        }
    }
}