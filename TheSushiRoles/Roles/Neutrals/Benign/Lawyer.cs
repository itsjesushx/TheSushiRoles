using UnityEngine;

namespace TheSushiRoles.Roles
{
    public static class Lawyer 
    {
        public static PlayerControl Player;
        public static PlayerControl target;
        public static Color Color = new Color32(112, 185, 141, byte.MaxValue);
        public static bool canCallEmergency = true;
        public static float vision = 1f;
        public static bool lawyerKnowsRole = false;
        public static bool targetCanBeJester = false;
        public static bool targetWasGuessed = false;
        public static bool IsAdditionalLawyerBonusWin = false;
        public static void ClearAndReload(bool ClearTarget = true)
        {
            Player = null;
            if (ClearTarget)
            {
                target = null;
                targetWasGuessed = false;
            }
            vision = CustomOptionHolder.lawyerVision.GetFloat();
            lawyerKnowsRole = CustomOptionHolder.lawyerKnowsRole.GetBool();
            targetCanBeJester = CustomOptionHolder.lawyerTargetCanBeJester.GetBool();
            canCallEmergency = CustomOptionHolder.lawyerCanCallEmergency.GetBool();
            IsAdditionalLawyerBonusWin = false;
        }
    }
}