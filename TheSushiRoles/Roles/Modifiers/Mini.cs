using System;
using UnityEngine;

namespace TheSushiRoles.Roles.Modifiers
{
    public static class Mini 
    {
        public static PlayerControl Player;
        public static Color Color = Color.yellow;
        public const float defaultColliderRadius = 0.2233912f;
        public const float defaultColliderOffset = 0.3636057f;

        public static float growingUpDuration = 400f;
        public static bool isGrowingUpInMeeting = true;
        public static DateTime timeOfGrowthStart = DateTime.UtcNow;
        public static DateTime timeOfMeetingStart = DateTime.UtcNow;
        public static float ageOnMeetingStart = 0f;
        public static bool IsMiniLose = false;

        public static void ClearAndReload() 
        {
            Player = null;
            IsMiniLose = false;
            growingUpDuration = CustomOptionHolder.modifierMiniGrowingUpDuration.GetFloat();
            isGrowingUpInMeeting = CustomOptionHolder.modifierMiniGrowingUpInMeeting.GetBool();
            timeOfGrowthStart = DateTime.UtcNow;
        }

        public static float GrowingProgress() 
        {
            float timeSinceStart = (float)(DateTime.UtcNow - timeOfGrowthStart).TotalMilliseconds;
            return Mathf.Clamp(timeSinceStart / (growingUpDuration * 1000), 0f, 1f);
        }

        public static bool IsGrownUp => GrowingProgress() == 1f;
    }
}