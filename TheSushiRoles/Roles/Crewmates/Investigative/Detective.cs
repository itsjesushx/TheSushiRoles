using UnityEngine;

namespace TheSushiRoles.Roles
{
    public static class Detective 
    {
        public static PlayerControl Player;
        public static Color Color = new Color32(45, 106, 165, byte.MaxValue);
        public static float footprintIntervall = 1f;
        public static float footprintDuration = 1f;
        public static bool anonymousFootprints = false;
        public static float reportNameDuration = 0f;
        public static float reportColorDuration = 20f;
        public static float timer = 6.2f;
        public static void ClearAndReload() 
        {
            Player = null;
            anonymousFootprints = CustomOptionHolder.detectiveAnonymousFootprints.GetBool();
            footprintIntervall = CustomOptionHolder.detectiveFootprintIntervall.GetFloat();
            footprintDuration = CustomOptionHolder.detectiveFootprintDuration.GetFloat();
            reportNameDuration = CustomOptionHolder.detectiveReportNameDuration.GetFloat();
            reportColorDuration = CustomOptionHolder.detectiveReportColorDuration.GetFloat();
            timer = 6.2f;
        }
    }   
}