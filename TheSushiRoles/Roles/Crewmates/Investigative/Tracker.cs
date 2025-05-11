using System.Collections.Generic;
using Reactor.Utilities.Extensions;
using UnityEngine;

namespace TheSushiRoles.Roles
{
    public static class Tracker 
    {
        public static PlayerControl Player;
        public static Color Color = new Color32(100, 58, 220, byte.MaxValue);
        public static List<Arrow> localArrows = new();

        public static float updateIntervall = 5f;
        public static bool resetTargetAfterMeeting = false;
        public static bool canTrackCorpses = false;
        public static float corpsesTrackingCooldown = 30f;
        public static float corpsesTrackingDuration = 5f;
        public static float corpsesTrackingTimer = 0f;
        public static int trackingMode = 0;
        public static List<Vector3> deadBodyPositions = new();
        public static PlayerControl CurrentTarget;
        public static PlayerControl tracked;
        public static bool usedTracker = false;
        public static float timeUntilUpdate = 0f;
        public static Arrow arrow = new(Color.blue);

        public static GameObject DangerMeterParent;
        public static DangerMeter Meter;

        private static Sprite trackCorpsesButtonSprite;
        public static Sprite GetTrackCorpsesButtonSprite()
        {
            if (trackCorpsesButtonSprite) return trackCorpsesButtonSprite;
            trackCorpsesButtonSprite = Utils.LoadSpriteFromResources("TheSushiRoles.Resources.PathfindButton.png", 115f);
            return trackCorpsesButtonSprite;
        }

        private static Sprite ButtonSprite;
        public static Sprite GetButtonSprite() 
        {
            if (ButtonSprite) return ButtonSprite;
            ButtonSprite = Utils.LoadSpriteFromResources("TheSushiRoles.Resources.TrackerButton.png", 115f);
            return ButtonSprite;
        }

        public static void ResetTracked() 
        {
            CurrentTarget = tracked = null;
            usedTracker = false;
            if (arrow?.arrow != null) UnityEngine.Object.Destroy(arrow.arrow);
            arrow = new Arrow(Color.blue);
            if (arrow.arrow != null) arrow.arrow.SetActive(false);
        }

        public static void ClearAndReload() 
        {
            Player = null;
            ResetTracked();
            timeUntilUpdate = 0f;
            updateIntervall = CustomOptionHolder.trackerUpdateIntervall.GetFloat();
            resetTargetAfterMeeting = CustomOptionHolder.trackerResetTargetAfterMeeting.GetBool();
            if (localArrows != null) 
            {
                foreach (Arrow arrow in localArrows)
                    if (arrow?.arrow != null)
                        UnityEngine.Object.Destroy(arrow.arrow);
            }
            deadBodyPositions = new List<Vector3>();
            corpsesTrackingTimer = 0f;
            corpsesTrackingCooldown = CustomOptionHolder.trackerCorpsesTrackingCooldown.GetFloat();
            corpsesTrackingDuration = CustomOptionHolder.trackerCorpsesTrackingDuration.GetFloat();
            canTrackCorpses = CustomOptionHolder.trackerCanTrackCorpses.GetBool();
            trackingMode = CustomOptionHolder.trackerTrackingMethod.GetSelection();
            if (DangerMeterParent) 
            {
                Meter.gameObject.Destroy();
                DangerMeterParent.Destroy();
            }
        }
    }
}