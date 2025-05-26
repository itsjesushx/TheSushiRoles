using System.Collections.Generic;
using UnityEngine;

namespace TheSushiRoles.Roles
{
    public static class Deputy
    {
        public static PlayerControl Player;
        public static Color Color = new Color32(199, 163, 52, byte.MaxValue);
        public static List<GameObject> ExecuteButtons = new List<GameObject>();
        public static int Charges;
        public static int RechargeTasksNumber;
        public static int RechargedTasks;
        public static bool KillsThroughShield = false;
        public static bool CanExecute = true;
        public static bool CanKillNeutralBenign = false;
        public static bool CanKillNeutralEvil = false;
        public static void ClearAndReload()
        {
            Player = null;
            CanExecute = true;
            KillsThroughShield = CustomOptionHolder.DeputyKillsThroughShield.GetBool();
            Charges = Mathf.RoundToInt(CustomOptionHolder.DeputyCharges.GetFloat());
            RechargeTasksNumber = Mathf.RoundToInt(CustomOptionHolder.DeputyRechargeTasksNumber.GetFloat());
            RechargedTasks = Mathf.RoundToInt(CustomOptionHolder.DeputyRechargeTasksNumber.GetFloat());
            CanKillNeutralBenign = CustomOptionHolder.DeputyCanKillNeutralBenign.GetBool();
            CanKillNeutralEvil = CustomOptionHolder.DeputyCanKillNeutralEvil.GetBool();
        }
    }
}