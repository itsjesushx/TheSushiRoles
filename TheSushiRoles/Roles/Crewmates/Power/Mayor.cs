namespace TheSushiRoles.Roles
{
    public static class Mayor 
    {
        public static PlayerControl Player;
        public static Color Color = new Color32(32, 77, 66, byte.MaxValue);
        public static Minigame emergency = null;
        public static Sprite emergencySprite = null;
        public static int remoteMeetingsLeft = 1;

        public static bool voteTwice = true;

        public static Sprite GetMeetingSprite()
        {
            if (emergencySprite) return emergencySprite;
            emergencySprite = Utils.LoadSprite("TheSushiRoles.Resources.EmergencyButton.png", 550f);
            return emergencySprite;
        }

        public static void ClearAndReload() 
        {
            Player = null;
            emergency = null;
            emergencySprite = null;
		    remoteMeetingsLeft = CustomGameOptions.MayorMaxRemoteMeetings; 
            voteTwice = true;
        }
    }
}