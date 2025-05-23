using UnityEngine;

namespace TheSushiRoles.Roles
{
    public class Jester
    {
        public static PlayerControl Player;
        public static Color Color = new Color32(255, 191, 204, byte.MaxValue);
        public static bool IsJesterWin = false;
        public static bool canCallEmergency = true;
        public static bool hasImpostorVision = false;
        public static bool CanUseVents;
        public static bool CanMoveInVents;
        public static void ClearAndReload() 
        {
            Player = null;
            CanUseVents = CustomOptionHolder.jesterCanHideInVents.GetBool();
            CanMoveInVents = CustomOptionHolder.jesterCanMoveInVents.GetBool();
            IsJesterWin = false;
            canCallEmergency = CustomOptionHolder.jesterCanCallEmergency.GetBool();
            hasImpostorVision = CustomOptionHolder.jesterHasImpostorVision.GetBool();
        }
    }
}