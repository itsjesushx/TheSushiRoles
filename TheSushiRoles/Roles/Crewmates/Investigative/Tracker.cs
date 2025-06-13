using System.Collections.Generic;

namespace TheSushiRoles.Roles
{
    public static class Tracker 
    {
        public static PlayerControl Player;
        public static Color Color = new Color32(100, 58, 220, byte.MaxValue);
        public static List<Arrow> localArrows = new();
        public static float corpsesTrackingTimer = 0f;
        public static List<Vector3> deadBodyPositions = new();
        public static PlayerControl CurrentTarget;
        public static PlayerControl tracked;
        public static bool usedTracker = false;
        public static float timeUntilUpdate = 0f;
        public static Arrow arrow = new(Color.blue);

        public static GameObject DangerMeterParent;
        public static DangerMeter Meter;
        public static void ResetTracked() 
        {
            CurrentTarget = tracked = null;
            usedTracker = false;
            if (arrow?.arrow != null) UObject.Destroy(arrow.arrow);
            arrow = new Arrow(Color.blue);
            if (arrow.arrow != null) arrow.arrow.SetActive(false);
        }

        public static void ClearAndReload() 
        {
            Player = null;
            ResetTracked();
            timeUntilUpdate = 0f;
            if (localArrows != null) 
            {
                foreach (Arrow arrow in localArrows)
                    if (arrow?.arrow != null)
                        UObject.Destroy(arrow.arrow);
            }
            deadBodyPositions = new List<Vector3>();
            corpsesTrackingTimer = 0f;
            if (DangerMeterParent) 
            {
                Meter.gameObject.Destroy();
                DangerMeterParent.Destroy();
            }
        }
    }
}