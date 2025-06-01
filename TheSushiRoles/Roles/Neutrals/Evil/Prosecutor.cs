using UnityEngine;

namespace TheSushiRoles.Roles
{
    public static class Prosecutor
    {
        public static PlayerControl Player;
        public static PlayerControl target;
        public static Color Color = new Color32(166, 131, 212, byte.MaxValue);
        public static bool canCallEmergency = true;
        public static float vision = 1f;
        public static bool KnowsRole = false;
        public static bool targetWasGuessed = false;
        public static bool IsProsecutorWin = false;
        public static ProsecutorOnTargetDeath BecomeEnum;
        public static void ClearAndReload(bool clearTarget = true) 
        {
            Player = null;
            if (clearTarget) 
            {
                target = null;
                targetWasGuessed = false;
            }
            IsProsecutorWin = false;
            BecomeEnum = (ProsecutorOnTargetDeath)CustomOptionHolder.ProsecutorBecomeEnum.GetSelection();
            vision = CustomOptionHolder.ProsecutorVision.GetFloat();
            KnowsRole = CustomOptionHolder.ProsecutorKnowsRole.GetBool();
            canCallEmergency = CustomOptionHolder.ProsecutorCanCallEmergency.GetBool();
        }
    }
}