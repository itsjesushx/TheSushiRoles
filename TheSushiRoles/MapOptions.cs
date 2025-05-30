using System.Collections.Generic;
using UnityEngine;

namespace TheSushiRoles
{
    static class MapOptions 
    {
        // Set values
        public static int maxNumberOfMeetings = 10;
        public static bool blockSkippingInEmergencyMeetings = false;
        public static float Meetingtime { get; set; } = 20f;
        public static float GameStartCooldowns = 15f;
        public static bool noVoteIsSelfVote = false;
        public static bool hidePlayerNames = false;
        public static bool GhostsSeeEverything = true;
        public static bool DisableLobbyMusic = true;        
        public static bool ghostsSeeInformation = true;
        public static bool ghostsSeeVotes = true;
        public static bool RoleSummaryVisible = true;
        public static bool allowParallelMedBayScans = false;
        public static bool showLighterDarker = true;
        public static bool enableSoundEffects = true;
        public static bool enableHorseMode = false;
        public static bool ShieldFirstKill = false;
        public static bool ShowVentsOnMap = true;
        public static bool ShowChatNotifications = true;
        public static bool SkeldVentImprovements = false;
        public static bool LimitAbilities = true;
        public static bool DisableMedbayAnimation = true;

        public static bool BPVitalsLab = false;
        public static bool BPVentImprovements = false;
        public static bool BPColdTempDeathValley = false;
        public static bool BPWifiChartCourseSwap = false;
        public static bool EnableBetterPolus = false;

        // Updating values
        public static int meetingsCount = 0;
        public static List<SurvCamera> CamsToAdd = new List<SurvCamera>();
        public static List<Vent> VentsToSeal = new List<Vent>();
        public static List<byte> RevivedPlayers = new List<byte>();
        public static Dictionary<byte, PoolablePlayer> BeanIcons = new Dictionary<byte, PoolablePlayer>();
        public static string FirstKillName;
        public static PlayerControl FirstPlayerKilled;

        public static void ClearAndReloadMapOptions() 
        {
            meetingsCount = 0;
            CamsToAdd = new List<SurvCamera>();
            VentsToSeal = new List<Vent>();
            BeanIcons = new Dictionary<byte, PoolablePlayer>();
            RevivedPlayers = new List<byte>();

            maxNumberOfMeetings = Mathf.RoundToInt(CustomOptionHolder.maxNumberOfMeetings.GetSelection());
            blockSkippingInEmergencyMeetings = CustomOptionHolder.blockSkippingInEmergencyMeetings.GetBool();
            noVoteIsSelfVote = CustomOptionHolder.noVoteIsSelfVote.GetBool();
            hidePlayerNames = CustomOptionHolder.hidePlayerNames.GetBool();
            allowParallelMedBayScans = CustomOptionHolder.allowParallelMedBayScans.GetBool();
            ShieldFirstKill = CustomOptionHolder.ShieldFirstKill.GetBool();
            SkeldVentImprovements = CustomOptionHolder.SkeldVentImprovements.GetBool();
            LimitAbilities = CustomOptionHolder.LimitAbilities.GetBool();
            DisableMedbayAnimation = CustomOptionHolder.DisableMedbayAnimation.GetBool();
            GameStartCooldowns = CustomOptionHolder.GameStartCooldowns.GetFloat();

            BPVitalsLab = CustomOptionHolder.BPVitalsLab.GetBool();
            BPWifiChartCourseSwap = CustomOptionHolder.BPWifiChartCourseSwap.GetBool();
            BPColdTempDeathValley = CustomOptionHolder.BPColdTempDeathValley.GetBool();
            BPVentImprovements = CustomOptionHolder.BPVentImprovements.GetBool();
            EnableBetterPolus = CustomOptionHolder.EnableBetterPolus.GetBool();

            FirstPlayerKilled = null;
            Meetingtime = GameOptionsManager.Instance.currentNormalGameOptions.VotingTime + GameOptionsManager.Instance.currentNormalGameOptions.DiscussionTime;
        }

        public static void ReloadPluginOptions() 
        {
            GhostsSeeEverything = TheSushiRolesPlugin.GhostsSeeEverything.Value;
            ghostsSeeInformation = TheSushiRolesPlugin.GhostsSeeInformation.Value;
            ghostsSeeVotes = TheSushiRolesPlugin.GhostsSeeVotes.Value;
            RoleSummaryVisible = TheSushiRolesPlugin.RoleSummaryVisible.Value;
            DisableLobbyMusic = TheSushiRolesPlugin.DisableLobbyMusic.Value;
            showLighterDarker = TheSushiRolesPlugin.ShowLighterDarker.Value;
            enableSoundEffects = TheSushiRolesPlugin.EnableSoundEffects.Value;
            enableHorseMode = TheSushiRolesPlugin.EnableHorseMode.Value;
            ShowVentsOnMap = TheSushiRolesPlugin.ShowVentsOnMap.Value;
            ShowChatNotifications = TheSushiRolesPlugin.ShowChatNotifications.Value;
        }
    }
}
