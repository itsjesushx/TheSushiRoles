using System.Collections.Generic;
namespace TheSushiRoles
{
    static class MapOptions 
    {
        // Plugin Options
        public static bool GhostsSeeEverything = true;
        public static bool DisableLobbyMusic = true;        
        public static bool ghostsSeeInformation = true;
        public static bool ghostsSeeVotes = true;
        public static bool showLighterDarker = true;
        public static bool enableSoundEffects = true;
        public static bool ShieldFirstKill = false;
        public static bool ShowVentsOnMap = true;
        public static bool ShowChatNotifications = true;

        // Updating values
        public static int meetingsCount = 0;
        public static List<SurvCamera> CamsToAdd = new List<SurvCamera>();
        public static List<Vent> VentsToSeal = new List<Vent>();
        public static List<byte> RevivedPlayers = new List<byte>();
        public static Dictionary<byte, PoolablePlayer> BeanIcons = new Dictionary<byte, PoolablePlayer>();
        public static string FirstKillName;
        public static PlayerControl FirstPlayerKilled;
        public static bool IsFirstRound { get; set; } = true;

        public static void ClearAndReloadMapOptions() 
        {
            meetingsCount = 0;
            CamsToAdd = new List<SurvCamera>();
            VentsToSeal = new List<Vent>();
            BeanIcons = new Dictionary<byte, PoolablePlayer>();
            RevivedPlayers = new List<byte>();
            IsFirstRound = true;
            FirstPlayerKilled = null;
        }

        public static void ReloadPluginOptions() 
        {
            GhostsSeeEverything = TheSushiRoles.GhostsSeeEverything.Value;
            ghostsSeeInformation = TheSushiRoles.GhostsSeeInformation.Value;
            ghostsSeeVotes = TheSushiRoles.GhostsSeeVotes.Value;
            DisableLobbyMusic = TheSushiRoles.DisableLobbyMusic.Value;
            showLighterDarker = TheSushiRoles.ShowLighterDarker.Value;
            enableSoundEffects = TheSushiRoles.EnableSoundEffects.Value;
            ShowVentsOnMap = TheSushiRoles.ShowVentsOnMap.Value;
            ShowChatNotifications = TheSushiRoles.ShowChatNotifications.Value;
        }
    }
}
