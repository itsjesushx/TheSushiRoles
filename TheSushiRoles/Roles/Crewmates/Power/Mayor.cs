using UnityEngine;

namespace TheSushiRoles.Roles
{
    public static class Mayor 
    {
        public static PlayerControl Player;
        public static Color Color = new Color32(32, 77, 66, byte.MaxValue);
        public static Minigame emergency = null;
        public static Sprite emergencySprite = null;
        public static int remoteMeetingsLeft = 1;

        public static bool canSeeVoteColors = false;
        public static int tasksNeededToSeeVoteColors;
        public static bool meetingButton = true;
        public static int mayorChooseSingleVote;

        public static bool voteTwice = true;

        public static Sprite GetMeetingSprite()
        {
            if (emergencySprite) return emergencySprite;
            emergencySprite = Utils.LoadSpriteFromResources("TheSushiRoles.Resources.EmergencyButton.png", 550f);
            return emergencySprite;
        }

        public static void ClearAndReload() 
        {
            Player = null;
            emergency = null;
            emergencySprite = null;
		    remoteMeetingsLeft = Mathf.RoundToInt(CustomOptionHolder.mayorMaxRemoteMeetings.GetFloat()); 
            canSeeVoteColors = CustomOptionHolder.mayorCanSeeVoteColors.GetBool();
            tasksNeededToSeeVoteColors = (int)CustomOptionHolder.mayorTasksNeededToSeeVoteColors.GetFloat();
            meetingButton = CustomOptionHolder.mayorMeetingButton.GetBool();
            mayorChooseSingleVote = CustomOptionHolder.mayorChooseSingleVote.GetSelection();
            voteTwice = true;
        }
    }
}