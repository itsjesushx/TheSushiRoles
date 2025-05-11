using System.Collections.Generic;
using Sentry.Unity.NativeUtils;
using UnityEngine;
using Types = TheSushiRoles.CustomOptionType;

namespace TheSushiRoles 
{
    public class CustomOptionHolder 
    {
        public static string[] rates = new string[]{"1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15" };
        public static string[] presets = new string[]{"Preset 1", "Preset 2", "Random Preset Skeld", "Random Preset Mira HQ", "Random Preset Polus", "Random Preset Airship", "Random Preset Submerged" };

        public static CustomOption presetSelection;
        public static CustomOption activateRoles;
        public static CustomOption crewmateRolesCountMin;
        public static CustomOption crewmateRolesCountMax;
        public static CustomOption neutralRolesCountMin;
        public static CustomOption neutralRolesCountMax;
        public static CustomOption neutralKillingRolesCountMin;
        public static CustomOption neutralKillingRolesCountMax;
        public static CustomOption impostorRolesCountMin;
        public static CustomOption impostorRolesCountMax;
        public static CustomOption modifiersCountMin;
        public static CustomOption modifiersCountMax;
        public static CustomOption abilitiesCountMax;
        public static CustomOption abilitiesCountMin;

        /*public static CustomOption isDraftMode;
        public static CustomOption draftModeAmountOfChoices;
        public static CustomOption draftModeTimeToChoose;
        public static CustomOption draftModeShowRoles;
        public static CustomOption draftModeHideImpRoles;
        public static CustomOption draftModeHideNeutralKRoles;
        public static CustomOption draftModeHideNeutralRoles;*/

        public static CustomOption anyPlayerCanStopStart;
        public static CustomOption enableEventMode;
        public static CustomOption deadImpsBlockSabotage;

        public static CustomOption BlackmailerSpawnRate;
        public static CustomOption BlackmailCooldown;
        public static CustomOption BlackmailInvisible;

        public static CustomOption VeteranSpawnRate;
        public static CustomOption VeteranCooldown;
        public static CustomOption VeteranCharges;
        public static CustomOption VeteranRechargeTasksNumber;
        public static CustomOption VeteranDuration;

        public static CustomOption morphlingSpawnRate;
        public static CustomOption morphlingCooldown;
        public static CustomOption morphlingDuration;

        public static CustomOption camouflagerSpawnRate;
        public static CustomOption camouflagerCooldown;
        public static CustomOption camouflagerDuration;

        public static CustomOption poisonerSpawnRate;
        public static CustomOption poisonerKillDelay;
        public static CustomOption poisonerCooldown;

        public static CustomOption eraserSpawnRate;
        public static CustomOption eraserCooldown;
        public static CustomOption eraserCanEraseAnyone;

        public static CustomOption jesterSpawnRate;
        public static CustomOption jesterCanCallEmergency;
        public static CustomOption jesterHasImpostorVision;
        public static CustomOption jesterCanHideInVents;
        public static CustomOption jesterCanMoveInVents;

        public static CustomOption arsonistSpawnRate;
        public static CustomOption arsonistCooldown;
        public static CustomOption arsonistDuration;

        public static CustomOption WerewolfSpawnRate;
        public static CustomOption WerewolfCooldown;
        public static CustomOption WerewolfCanUseVents;
        public static CustomOption WerewolfMaulRadius;

        public static CustomOption PlaguebearerSpawnRate;
        public static CustomOption PlaguebearerCooldown;
        public static CustomOption PestilenceCooldown;
        public static CustomOption PestilenceCanUseVents;

        public static CustomOption jackalSpawnRate;
        public static CustomOption jackalKillCooldown;
        public static CustomOption jackalCreateSidekickCooldown;
        public static CustomOption jackalCanUseVents;
        public static CustomOption jackalCanCreateSidekick;
        public static CustomOption sidekickPromotesToJackal;
        public static CustomOption sidekickCanKill;
        public static CustomOption sidekickCanUseVents;
        public static CustomOption jackalPromotedFromSidekickCanCreateSidekick;
        public static CustomOption jackalCanCreateSidekickFromImpostor;

        public static CustomOption bountyHunterSpawnRate;
        public static CustomOption bountyHunterBountyDuration;
        public static CustomOption bountyHunterReducedCooldown;
        public static CustomOption bountyHunterPunishmentTime;
        public static CustomOption bountyHunterShowArrow;
        public static CustomOption bountyHunterArrowUpdateIntervall;

        public static CustomOption witchSpawnRate;
        public static CustomOption witchCooldown;
        public static CustomOption witchAdditionalCooldown;
        public static CustomOption witchCanSpellAnyone;
        public static CustomOption witchSpellCastingDuration;
        public static CustomOption witchTriggerBothCooldowns;
        public static CustomOption witchVoteSavesTargets;

        public static CustomOption WraithSpawnRate;
        public static CustomOption WraithCooldown;
        public static CustomOption WraithDuration;

        public static CustomOption ninjaSpawnRate;
        public static CustomOption ninjaCooldown;
        public static CustomOption ninjaKnowsTargetLocation;
        public static CustomOption ninjaTraceTime;
        public static CustomOption ninjaTraceColorTime;
        public static CustomOption ninjaInvisibleDuration;

        public static CustomOption mayorSpawnRate;
        public static CustomOption mayorCanSeeVoteColors;
        public static CustomOption mayorTasksNeededToSeeVoteColors;
        public static CustomOption mayorMeetingButton;
        public static CustomOption mayorMaxRemoteMeetings;
        public static CustomOption mayorChooseSingleVote;

        public static CustomOption portalmakerSpawnRate;
        public static CustomOption portalmakerCooldown;
        public static CustomOption portalmakerUsePortalCooldown;
        public static CustomOption portalmakerLogOnlyColorType;
        public static CustomOption portalmakerLogHasTime;
        public static CustomOption portalmakerCanPortalFromAnywhere;

        public static CustomOption engineerSpawnRate;
        public static CustomOption engineerNumberOfFixes;
        public static CustomOption engineerHighlightForImpostors;
        public static CustomOption engineerHighlightForTeamJackal;

        public static CustomOption sheriffSpawnRate;
        public static CustomOption sheriffCooldown;
        public static CustomOption sheriffCanKillNeutrals;

        public static CustomOption JuggernautSpawnRate;
        public static CustomOption JuggernautCooldown;
        public static CustomOption JuggernautReducedCooldown;
        public static CustomOption JuggernautCanUseVents;

        public static CustomOption PredatorSpawnRate;
        public static CustomOption PredatorTerminateCooldown;
        public static CustomOption PredatorTerminateDuration;
        public static CustomOption PredatorTerminateKillCooldown;
        public static CustomOption PredatorCanUseVents;

        public static CustomOption GlitchSpawnRate;
        public static CustomOption GlitchCanUseVents;
        public static CustomOption GlitchKillCooldowm;
        public static CustomOption GlitchNumberOfHacks;
        public static CustomOption GlitchHackCooldown;
        public static CustomOption GlitchMimicCooldown;
        public static CustomOption GlitchMimicDuration;
        public static CustomOption GlitchHackDuration;

        public static CustomOption lighterSpawnRate;
        public static CustomOption lighterModeLightsOnVision;
        public static CustomOption lighterModeLightsOffVision;
        public static CustomOption lighterFlashlightWidth;

        public static CustomOption detectiveSpawnRate;
        public static CustomOption detectiveAnonymousFootprints;
        public static CustomOption detectiveFootprintIntervall;
        public static CustomOption detectiveFootprintDuration;
        public static CustomOption detectiveReportNameDuration;
        public static CustomOption detectiveReportColorDuration;

        public static CustomOption timeMasterSpawnRate;
        public static CustomOption timeMasterCooldown;
        public static CustomOption timeMasterRewindTime;
        public static CustomOption TimeMasterReviveDuringRewind;

        public static CustomOption medicSpawnRate;
        public static CustomOption medicShowShielded;
        public static CustomOption medicShowAttemptToShielded;
        public static CustomOption medicSetOrShowShieldAfterMeeting;
        public static CustomOption medicShowAttemptToMedic;
        public static CustomOption medicSetShieldAfterMeeting;

        public static CustomOption swapperSpawnRate;
        public static CustomOption swapperCanCallEmergency;
        public static CustomOption swapperCanOnlySwapOthers;
        public static CustomOption swapperSwapsNumber;
        public static CustomOption swapperRechargeTasksNumber;

        public static CustomOption MysticSpawnRate;
        public static CustomOption MysticMode;
        public static CustomOption MysticSoulDuration;
        public static CustomOption MysticLimitSoulDuration;
        public static CustomOption MysticCooldown;
        public static CustomOption MysticCharges;
        public static CustomOption MysticRechargeTasksNumber;

        public static CustomOption hackerSpawnRate;
        public static CustomOption hackerCooldown;
        public static CustomOption hackerHackeringDuration;
        public static CustomOption hackerOnlyColorType;
        public static CustomOption hackerToolsNumber;
        public static CustomOption hackerRechargeTasksNumber;
        public static CustomOption hackerNoMove;

        public static CustomOption trackerSpawnRate;
        public static CustomOption trackerUpdateIntervall;
        public static CustomOption trackerResetTargetAfterMeeting;
        public static CustomOption trackerCanTrackCorpses;
        public static CustomOption trackerCorpsesTrackingCooldown;
        public static CustomOption trackerCorpsesTrackingDuration;
        public static CustomOption trackerTrackingMethod;

        public static CustomOption spySpawnRate;
        public static CustomOption spyCanDieToSheriff;
        public static CustomOption spyImpostorsCanKillAnyone;
        public static CustomOption spyCanEnterVents;
        public static CustomOption spyHasImpostorVision;

        public static CustomOption tricksterSpawnRate;
        public static CustomOption tricksterPlaceBoxCooldown;
        public static CustomOption tricksterLightsOutCooldown;
        public static CustomOption tricksterLightsOutDuration;

        public static CustomOption cleanerSpawnRate;
        public static CustomOption cleanerCooldown;
        
        public static CustomOption warlockSpawnRate;
        public static CustomOption warlockCooldown;
        public static CustomOption warlockRootTime;

        public static CustomOption VigilanteSpawnRate;
        public static CustomOption VigilanteCooldown;
        public static CustomOption VigilanteTotalScrews;
        public static CustomOption VigilanteCamPrice;
        public static CustomOption VigilanteVentPrice;
        public static CustomOption VigilanteCamDuration;
        public static CustomOption VigilanteCamMaxCharges;
        public static CustomOption VigilanteCamRechargeTasksNumber;
        public static CustomOption VigilanteNoMove;

        public static CustomOption vultureSpawnRate;
        public static CustomOption vultureCooldown;
        public static CustomOption vultureNumberToWin;
        public static CustomOption vultureCanUseVents;
        public static CustomOption vultureShowArrows;

        public static CustomOption mediumSpawnRate;
        public static CustomOption mediumCooldown;
        public static CustomOption mediumDuration;
        public static CustomOption mediumOneTimeUse;
        public static CustomOption mediumChanceAdditionalInfo;

        public static CustomOption CrusaderSpawnRate;
        public static CustomOption CrusaderCooldown;
        public static CustomOption CrusaderCharges;
        public static CustomOption CrusaderRechargeTasksNumber;

        public static CustomOption RomanticSpawnChance;
        public static CustomOption RomanticKnowsRole;
        public static CustomOption VengefulRomanticCooldown;
        public static CustomOption VengefulRomanticCanUseVents;

        public static CustomOption OracleSpawnRate;
        public static CustomOption ConfessCooldown;
        public static CustomOption RevealAccuracy;
        public static CustomOption NeutralBenignShowsEvil;
        public static CustomOption NeutralEvilShowsEvil;
        public static CustomOption OracleCharges;
        public static CustomOption OracleRechargeTasksNumber;


        public static CustomOption lawyerSpawnRate;
        public static CustomOption lawyerTargetCanBeJester;
        public static CustomOption lawyerVision;
        public static CustomOption lawyerKnowsRole;
        public static CustomOption lawyerCanCallEmergency;

        public static CustomOption ProsecutorSpawnRate;
        public static CustomOption ProsecutorBecomeEnum;
        public static CustomOption ProsecutorVision;
        public static CustomOption ProsecutorKnowsRole;
        public static CustomOption ProsecutorCanCallEmergency;

        public static CustomOption pursuerCooldown;
        public static CustomOption pursuerBlanksNumber;

        public static CustomOption UndertakerSpawnRate;
        public static CustomOption UndertakerCooldown;
        public static CustomOption UndertakerDragSpeed;

        public static CustomOption AgentSpawnRate;
        public static CustomOption HitmanCooldown;
        public static CustomOption HitmanCanUseVents;
        public static CustomOption HitmanDragCooldown;
        public static CustomOption HitmanMorphDuration;
        public static CustomOption HitmanMorphCooldown;
        public static CustomOption HitmanDragSpeed;
        public static CustomOption HitmanSpawnsWithNoAgent;
        public static CustomOption AgentCanUseVents;

        public static CustomOption trapperSpawnRate;
        public static CustomOption trapperCooldown;
        public static CustomOption trapperMaxCharges;
        public static CustomOption trapperRechargeTasksNumber;
        public static CustomOption trapperTrapNeededTriggerToReveal;
        public static CustomOption trapperAnonymousMap;
        public static CustomOption trapperInfoType;
        public static CustomOption trapperTrapDuration;

        public static CustomOption yoyoSpawnRate;
        public static CustomOption yoyoBlinkDuration;
        public static CustomOption yoyoMarkCooldown;
        public static CustomOption yoyoMarkStaysOverMeeting;
        public static CustomOption yoyoHasAdminTable;
        public static CustomOption yoyoAdminTableCooldown;
        public static CustomOption yoyoSilhouetteVisibility;

        public static CustomOption GrenadierSpawnRate;
        public static CustomOption GrenadierCooldown;
        public static CustomOption GrenadierGrenadeDuration;
        public static CustomOption GrenadierGrenadeRadius;

        public static CustomOption modifiersAreHidden;

        public static CustomOption modifierBait;
        public static CustomOption modifierBaitQuantity;
        public static CustomOption modifierBaitReportDelayMin;
        public static CustomOption modifierBaitReportDelayMax;
        public static CustomOption modifierBaitShowKillFlash;

        public static CustomOption modifierLover;
        public static CustomOption modifierLoverImpLoverRate;
        public static CustomOption modifierLoverBothDie;
        public static CustomOption modifierLoverEnableChat;

        public static CustomOption modifierLazy;
        public static CustomOption modifierLazyQuantity;

        public static CustomOption modifierTieBreaker;

        public static CustomOption modifierSunglasses;
        public static CustomOption modifierSunglassesQuantity;
        public static CustomOption modifierSunglassesVision;

        public static CustomOption AmnesiacSpawnRate;
        public static CustomOption AmnesiacHasArrows;
        
        public static CustomOption modifierMini;
        public static CustomOption modifierMiniGrowingUpDuration;
        public static CustomOption modifierMiniGrowingUpInMeeting;

        public static CustomOption ModifierSleuth;
        public static CustomOption ModifierSleuthQuantity;

        public static CustomOption modifierVip;
        public static CustomOption modifierVipQuantity;
        public static CustomOption modifierVipShowColor;

        public static CustomOption modifierInvert;
        public static CustomOption modifierInvertQuantity;
        public static CustomOption modifierInvertDuration;

        public static CustomOption modifierChameleon;
        public static CustomOption modifierChameleonQuantity;
        public static CustomOption modifierChameleonHoldDuration;
        public static CustomOption modifierChameleonFadeDuration;
        public static CustomOption modifierChameleonMinVisibility;
        
        public static CustomOption modifierArmored;

        public static CustomOption maxNumberOfMeetings;
        public static CustomOption blockSkippingInEmergencyMeetings;
        public static CustomOption noVoteIsSelfVote;
        public static CustomOption hidePlayerNames;
        public static CustomOption allowParallelMedBayScans;
        public static CustomOption ShieldFirstKill;
        public static CustomOption finishTasksBeforeHauntingOrZoomingOut;
        public static CustomOption camsNightVision;
        public static CustomOption camsNoNightVisionIfImpVision;

        public static CustomOption MinerSpawnRate;
        public static CustomOption MinerCooldown;
        public static CustomOption MineVisible;
        public static CustomOption MineDelay;

        public static CustomOption RandomSpawns;
        public static CustomOption LimitAbilities;
        public static CustomOption SkeldVentImprovements;

        public static CustomOption BPVitalsLab;
        public static CustomOption EnableBetterPolus;
        public static CustomOption BPWifiChartCourseSwap;
        public static CustomOption BPVentImprovements;
        public static CustomOption BPColdTempDeathValley;

        public static CustomOption DisableMedbayAnimation;
        public static CustomOption GameStartCooldowns;

        public static CustomOption AbilityDisperser;
        public static CustomOption AbilityDisperserCooldown;
        public static CustomOption AbilityDisperserKillCharges;
        public static CustomOption AbilityDisperserCharges;

        public static CustomOption AbilityParanoid;
        public static CustomOption AbilityCoward;

        public static CustomOption dynamicMap;
        public static CustomOption dynamicMapEnableSkeld;
        public static CustomOption dynamicMapEnableMira;
        public static CustomOption dynamicMapEnablePolus;
        public static CustomOption dynamicMapEnableAirShip;
        public static CustomOption dynamicMapEnableFungle;
        public static CustomOption dynamicMapEnableSubmerged;
        public static CustomOption dynamicMapSeparateSettings;

        public static CustomOption ShowHidePursuerSettings;

        //Guesser Gamemode
        public static CustomOption GuesserCrewNumber;
        public static CustomOption GuesserNeutralNumber;
        public static CustomOption GuesserImpNumber;
        public static CustomOption GuesserHaveModifier;
        public static CustomOption GuesserNumberOfShots;
        public static CustomOption GuesserHasMultipleShotsPerMeeting;
        public static CustomOption GuesserKillsThroughShield;
        public static CustomOption GuesserEvilCanKillSpy;
        public static CustomOption CrewGuesserNumberOfTasks;
        public static CustomOption SidekickIsAlwaysGuesser;


        internal static Dictionary<byte, byte[]> blockedRolePairings = new Dictionary<byte, byte[]>();

        public static string ColorString(Color c, string s) 
        {
            return string.Format("<color=#{0:X2}{1:X2}{2:X2}{3:X2}>{4}</color>", ToByte(c.r), ToByte(c.g), ToByte(c.b), ToByte(c.a), s);
        }
 
        private static byte ToByte(float f) 
        {
            f = Mathf.Clamp01(f);
            return (byte)(f * 255);
        }
        public static void Load() 
        {

            CustomOption.vanillaSettings = TheSushiRolesPlugin.Instance.Config.Bind("Preset0", "VanillaOptions", "");

            // Role Options
            presetSelection = CustomOption.Create(0, Types.General, ColorString(new Color(204f / 255f, 204f / 255f, 0, 1f), "Preset"), presets, null, true);

            // Using new id's for the options to not break compatibilty with older versions
            crewmateRolesCountMin = CustomOption.Create(300, Types.General, ColorString(new Color(204f / 255f, 204f / 255f, 0, 1f), "Minimum Crewmate Roles"), 15f, 0f, 15f, 1f, null, true, Heading: "Min/Max Roles");
            crewmateRolesCountMax = CustomOption.Create(301, Types.General, ColorString(new Color(204f / 255f, 204f / 255f, 0, 1f), "Maximum Crewmate Roles"), 15f, 0f, 15f, 1f);            
            neutralRolesCountMin = CustomOption.Create(302, Types.General, ColorString(new Color(204f / 255f, 204f / 255f, 0, 1f), "Minimum Neutral Roles"), 15f, 0f, 15f, 1f);
            neutralRolesCountMax = CustomOption.Create(303, Types.General, ColorString(new Color(204f / 255f, 204f / 255f, 0, 1f), "Maximum Neutral Roles"), 15f, 0f, 15f, 1f);
            neutralKillingRolesCountMin = CustomOption.Create(30211, Types.General, ColorString(new Color(204f / 255f, 204f / 255f, 0, 1f), "Minimum Neutral Killing Roles"), 15f, 0f, 15f, 1f);
            neutralKillingRolesCountMax = CustomOption.Create(30311, Types.General, ColorString(new Color(204f / 255f, 204f / 255f, 0, 1f), "Maximum Neutral Killing Roles"), 15f, 0f, 15f, 1f);
            impostorRolesCountMin = CustomOption.Create(304, Types.General, ColorString(new Color(204f / 255f, 204f / 255f, 0, 1f), "Minimum Impostor Roles"), 15f, 0f, 15f, 1f);
            impostorRolesCountMax = CustomOption.Create(305, Types.General, ColorString(new Color(204f / 255f, 204f / 255f, 0, 1f), "Maximum Impostor Roles"), 15f, 0f, 15f, 1f);
            modifiersCountMin = CustomOption.Create(306, Types.General, ColorString(new Color(204f / 255f, 204f / 255f, 0, 1f), "Minimum Modifiers"), 15f, 0f, 15f, 1f);
            modifiersCountMax = CustomOption.Create(307, Types.General, ColorString(new Color(204f / 255f, 204f / 255f, 0, 1f), "Maximum Modifiers"), 15f, 0f, 15f, 1f);
            abilitiesCountMin = CustomOption.Create(520, Types.General, ColorString(new Color(204f / 255f, 204f / 255f, 0, 1f), "Minimum Abilities"), 15f, 0f, 15f, 1f);
            abilitiesCountMax = CustomOption.Create(521, Types.General, ColorString(new Color(204f / 255f, 204f / 255f, 0, 1f), "Maximum Abilities"), 15f, 0f, 15f, 1f);

            /*isDraftMode = CustomOption.Create(600, Types.General, ColorString(Color.yellow, "Enable Role Draft"), false, null, true, null, "Role Draft");
            draftModeAmountOfChoices = CustomOption.Create(601, Types.General, ColorString(Color.yellow, "Max Amount Of Roles\nTo Choose From"), 5f, 2f, 15f, 1f, isDraftMode, false);
            draftModeTimeToChoose = CustomOption.Create(602, Types.General, ColorString(Color.yellow, "Time For Selection"), 5f, 3f, 20f, 1f, isDraftMode, false, Format: "s");
            draftModeShowRoles = CustomOption.Create(603, Types.General, ColorString(Color.yellow, "Show Picked Roles"), false, isDraftMode, false);
            draftModeHideImpRoles = CustomOption.Create(604, Types.General, ColorString(Color.yellow, "Hide Impostor Roles"), false, draftModeShowRoles, false);
            draftModeHideNeutralRoles = CustomOption.Create(605, Types.General, ColorString(Color.yellow, "Hide Neutral Roles"), false, draftModeShowRoles, false);*/

            morphlingSpawnRate = CustomOption.Create(20, Types.Impostor, ColorString(Morphling.Color, "Morphling"), 0f, 0f, 100f, 10f, null, true, Format: "%");
            morphlingCooldown = CustomOption.Create(21, Types.Impostor, "Morphling Cooldown", 30f, 10f, 60f, 2.5f, morphlingSpawnRate, Format: "s");
            morphlingDuration = CustomOption.Create(22, Types.Impostor, "Morph Duration", 10f, 1f, 20f, 0.5f, morphlingSpawnRate, Format: "s");

            WraithSpawnRate = CustomOption.Create(510, Types.Impostor, ColorString(Wraith.Color, "Wraith"), 0f, 0f, 100f, 10f, null, true, Format: "%");
            WraithCooldown = CustomOption.Create(511, Types.Impostor, "Wraith Cooldown", 30f, 10f, 60f, 2.5f, WraithSpawnRate, Format: "s");
            WraithDuration = CustomOption.Create(512, Types.Impostor, "Vanish Duration", 10f, 1f, 20f, 0.5f, WraithSpawnRate, Format: "s");

            MinerSpawnRate = CustomOption.Create(23, Types.Impostor, ColorString(Miner.Color, "Miner"), 0f, 0f, 100f, 10f, null, true, Format: "%");
            MinerCooldown = CustomOption.Create(24, Types.Impostor, "Miner Cooldown", 30f, 10f, 60f, 2.5f, MinerSpawnRate, Format: "s");
            MineVisible = CustomOption.Create(25, Types.Impostor, "When Are The Vents Visible", new string [] {"Instantly", "After Next Meeting", "Delayed"}, MinerSpawnRate);
            MineDelay = CustomOption.Create(26, Types.Impostor, "Time Until Vents Are Visible (If Set To Delayed)", 5f, 1f, 20f, 1f, MinerSpawnRate, Format: "s");

            camouflagerSpawnRate = CustomOption.Create(30, Types.Impostor, ColorString(Camouflager.Color, "Camouflager"), 0f, 0f, 100f, 10f, null, true, Format: "%");
            camouflagerCooldown = CustomOption.Create(31, Types.Impostor, "Camouflager Cooldown", 30f, 10f, 60f, 2.5f, camouflagerSpawnRate, Format: "s");
            camouflagerDuration = CustomOption.Create(32, Types.Impostor, "Camo Duration", 10f, 1f, 20f, 0.5f, camouflagerSpawnRate, Format: "s");

            poisonerSpawnRate = CustomOption.Create(40, Types.Impostor, ColorString(Poisoner.Color, "Poisoner"), 0f, 0f, 100f, 10f, null, true, Format: "%");
            poisonerKillDelay = CustomOption.Create(41, Types.Impostor, "Poisoner Kill Delay", 10f, 1f, 20f, 1f, poisonerSpawnRate, Format: "s");
            poisonerCooldown = CustomOption.Create(42, Types.Impostor, "Poisoner Cooldown", 30f, 10f, 60f, 2.5f, poisonerSpawnRate, Format: "s");

            eraserSpawnRate = CustomOption.Create(230, Types.Impostor, ColorString(Eraser.Color, "Eraser"), 0f, 0f, 100f, 10f, null, true, Format: "%");
            eraserCooldown = CustomOption.Create(231, Types.Impostor, "Eraser Cooldown", 30f, 10f, 120f, 5f, eraserSpawnRate, Format: "s");
            eraserCanEraseAnyone = CustomOption.Create(232, Types.Impostor, "Eraser Can Erase Anyone", false, eraserSpawnRate);

            BlackmailerSpawnRate = CustomOption.Create(50, Types.Impostor, ColorString(Blackmailer.Color, "Blackmailer"), 0f, 0f, 100f, 10f, null, true, Format: "%");
            BlackmailCooldown = CustomOption.Create(51, Types.Impostor, "Blackmail Cooldown", 30f, 10f, 60f, 2.5f, BlackmailerSpawnRate, Format: "s");
            BlackmailInvisible = CustomOption.Create(52, Types.Impostor, "Only Target Sees Blackmail", false, BlackmailerSpawnRate);

            tricksterSpawnRate = CustomOption.Create(250, Types.Impostor, ColorString(Trickster.Color, "Trickster"), 0f, 0f, 100f, 10f, null, true, Format: "%");
            tricksterPlaceBoxCooldown = CustomOption.Create(251, Types.Impostor, "Trickster Box Cooldown", 10f, 2.5f, 30f, 2.5f, tricksterSpawnRate, Format: "s");
            tricksterLightsOutCooldown = CustomOption.Create(252, Types.Impostor, "Trickster Lights Out Cooldown", 30f, 10f, 60f, 5f, tricksterSpawnRate, Format: "s");
            tricksterLightsOutDuration = CustomOption.Create(253, Types.Impostor, "Trickster Lights Out Duration", 15f, 5f, 60f, 2.5f, tricksterSpawnRate, Format: "s");

            cleanerSpawnRate = CustomOption.Create(260, Types.Impostor, ColorString(Cleaner.Color, "Cleaner"), 0f, 0f, 100f, 10f, null, true, Format: "%");
            cleanerCooldown = CustomOption.Create(261, Types.Impostor, "Cleaner Cooldown", 30f, 10f, 60f, 2.5f, cleanerSpawnRate, Format: "s");

            warlockSpawnRate = CustomOption.Create(270, Types.Impostor, ColorString(Cleaner.Color, "Warlock"), 0f, 0f, 100f, 10f, null, true, Format: "%");
            warlockCooldown = CustomOption.Create(271, Types.Impostor, "Warlock Cooldown", 30f, 10f, 60f, 2.5f, warlockSpawnRate, Format: "s");
            warlockRootTime = CustomOption.Create(272, Types.Impostor, "Warlock Root Time", 5f, 0f, 15f, 1f, warlockSpawnRate, Format: "s");

            bountyHunterSpawnRate = CustomOption.Create(320, Types.Impostor, ColorString(BountyHunter.Color, "Bounty Hunter"), 0f, 0f, 100f, 10f, null, true, Format: "%");
            bountyHunterBountyDuration = CustomOption.Create(321, Types.Impostor, "Duration After Which Bounty Changes",  60f, 10f, 180f, 10f, bountyHunterSpawnRate, Format: "s");
            bountyHunterReducedCooldown = CustomOption.Create(322, Types.Impostor, "Cooldown After Killing Bounty", 2.5f, 0f, 30f, 2.5f, bountyHunterSpawnRate, Format: "s");
            bountyHunterPunishmentTime = CustomOption.Create(323, Types.Impostor, "Additional Cooldown After Killing Others", 20f, 0f, 60f, 2.5f, bountyHunterSpawnRate, Format: "s");
            bountyHunterShowArrow = CustomOption.Create(324, Types.Impostor, "Show Arrow Pointing Towards The Bounty", true, bountyHunterSpawnRate);
            bountyHunterArrowUpdateIntervall = CustomOption.Create(325, Types.Impostor, "Arrow Update Intervall", 15f, 2.5f, 60f, 2.5f, bountyHunterShowArrow, Format: "s");

            witchSpawnRate = CustomOption.Create(370, Types.Impostor, ColorString(Witch.Color, "Witch"), 0f, 0f, 100f, 10f, null, true, Format: "%");
            witchCooldown = CustomOption.Create(371, Types.Impostor, "Witch Spell Casting Cooldown", 30f, 10f, 120f, 5f, witchSpawnRate, Format: "s");
            witchAdditionalCooldown = CustomOption.Create(372, Types.Impostor, "Witch Additional Cooldown", 10f, 0f, 60f, 5f, witchSpawnRate, Format: "s");
            witchCanSpellAnyone = CustomOption.Create(373, Types.Impostor, "Witch Can Spell Anyone", false, witchSpawnRate);
            witchSpellCastingDuration = CustomOption.Create(374, Types.Impostor, "Spell Casting Duration", 1f, 0f, 10f, 1f, witchSpawnRate, Format: "s");
            witchTriggerBothCooldowns = CustomOption.Create(375, Types.Impostor, "Trigger Both Cooldowns", true, witchSpawnRate);
            witchVoteSavesTargets = CustomOption.Create(376, Types.Impostor, "Voting The Witch Saves All The Targets", true, witchSpawnRate);

            UndertakerSpawnRate = CustomOption.Create(377, Types.Impostor, ColorString(Ninja.Color, "Undertaker"), 0f, 0f, 100f, 10f, null, true, Format: "%");
            UndertakerCooldown = CustomOption.Create(378, Types.Impostor, "Undertaker Cooldown", 30f, 10f, 120f, 5f, UndertakerSpawnRate, Format: "s");
            UndertakerDragSpeed = CustomOption.Create(379, Types.Impostor, "Undertaker Drag Speed", 1f, 0.5f, 3f, 0.1f, UndertakerSpawnRate, Format: "x");

            ninjaSpawnRate = CustomOption.Create(380, Types.Impostor, ColorString(Ninja.Color, "Ninja"), 0f, 0f, 100f, 10f, null, true, Format: "%");
            ninjaCooldown = CustomOption.Create(381, Types.Impostor, "Ninja Mark Cooldown", 30f, 10f, 120f, 5f, ninjaSpawnRate, Format: "s");
            ninjaKnowsTargetLocation = CustomOption.Create(382, Types.Impostor, "Ninja Knows Location Of Target", true, ninjaSpawnRate);
            ninjaTraceTime = CustomOption.Create(383, Types.Impostor, "Trace Duration", 5f, 1f, 20f, 0.5f, ninjaSpawnRate, Format: "s");
            ninjaTraceColorTime = CustomOption.Create(384, Types.Impostor, "Time Till Trace Color Has Faded", 2f, 0f, 20f, 0.5f, ninjaSpawnRate, Format: "s");
            ninjaInvisibleDuration = CustomOption.Create(385, Types.Impostor, "Time The Ninja Is Invisible", 3f, 0f, 20f, 1f, ninjaSpawnRate, Format: "s");

            GrenadierSpawnRate = CustomOption.Create(386, Types.Impostor, ColorString(Grenadier.Color, "Grenadier"), 0f, 0f, 100f, 10f, null, true, Format: "%");
            GrenadierCooldown = CustomOption.Create(387, Types.Impostor, "Grenadier Cooldown", 30f, 10f, 120f, 5f, GrenadierSpawnRate, Format: "s");
            GrenadierGrenadeDuration = CustomOption.Create(388, Types.Impostor, "Grenade Duration", 5f, 1f, 20f, 0.5f, GrenadierSpawnRate, Format: "s");
            GrenadierGrenadeRadius = CustomOption.Create(389, Types.Impostor, "Grenade Radius", 1f, 0.25f, 5f, 0.25f, GrenadierSpawnRate, Format: "x");

            yoyoSpawnRate = CustomOption.Create(470, Types.Impostor, ColorString(Yoyo.Color, "Yo-Yo"), 0f, 0f, 100f, 10f, null, true, Format: "%");
            yoyoBlinkDuration = CustomOption.Create(471, Types.Impostor, "Blink Duration", 20f, 2.5f, 120f, 2.5f, yoyoSpawnRate, Format: "s");
            yoyoMarkCooldown = CustomOption.Create(472, Types.Impostor, "Mark Location Cooldown", 20f, 2.5f, 120f, 2.5f, yoyoSpawnRate, Format: "s");
            yoyoMarkStaysOverMeeting = CustomOption.Create(473, Types.Impostor, "Marked Location Stays After Meeting", true, yoyoSpawnRate);
            yoyoHasAdminTable = CustomOption.Create(474, Types.Impostor, "Has Admin Table", true, yoyoSpawnRate);
            yoyoAdminTableCooldown = CustomOption.Create(475, Types.Impostor, "Admin Table Cooldown", 20f, 2.5f, 120f, 2.5f, yoyoHasAdminTable, Format: "s");
            yoyoSilhouetteVisibility = CustomOption.Create(476, Types.Impostor, "Silhouette Visibility", new string[] { "0%", "10%", "20%", "30%", "40%", "50%" }, yoyoSpawnRate);

            GlitchSpawnRate = CustomOption.Create(103, Types.NeutralKiller, ColorString(Glitch.Color, "Glitch"), 0f, 0f, 100f, 10f, null, true, Format: "%");
            GlitchCanUseVents = CustomOption.Create(107, Types.NeutralKiller, "Glitch Can Use Vents", false, GlitchSpawnRate);
            GlitchKillCooldowm = CustomOption.Create(108, Types.NeutralKiller, "Kill Cooldown", 30f, 10f, 60f, 2.5f, GlitchSpawnRate, Format: "s");
            GlitchNumberOfHacks = CustomOption.Create(104, Types.NeutralKiller, "Number Of Hacks", 3f, 1f, 10f, 1f, GlitchSpawnRate);
            GlitchHackCooldown = CustomOption.Create(105, Types.NeutralKiller, "Hack Cooldown", 30f, 10f, 60f, 2.5f, GlitchSpawnRate, Format: "s");
            GlitchHackDuration = CustomOption.Create(106, Types.NeutralKiller, "Hack Duration", 15f, 5f, 60f, 2.5f, GlitchSpawnRate, Format: "s");
            GlitchMimicCooldown = CustomOption.Create(109, Types.NeutralKiller, "Mimic Cooldown", 30f, 10f, 60f, 2.5f, GlitchSpawnRate, Format: "s");
            GlitchMimicDuration = CustomOption.Create(117, Types.NeutralKiller, "Mimic Duration", 10f, 1f, 20f, 0.5f, GlitchSpawnRate, Format: "s");
            
            WerewolfSpawnRate = CustomOption.Create(2923, Types.NeutralKiller, ColorString(Werewolf.Color, "Werewolf"), 0f, 0f, 100f, 10f, null, true, Format: "%");
            WerewolfCooldown = CustomOption.Create(2924, Types.NeutralKiller, "Werewolf Maul Cooldown", 30f, 10f, 60f, 2.5f, WerewolfSpawnRate, Format: "s");
            WerewolfCanUseVents = CustomOption.Create(2926, Types.NeutralKiller, "Werewolf Can Use Vents", true, WerewolfSpawnRate);
            WerewolfMaulRadius = CustomOption.Create(2925, Types.NeutralKiller, "Maul Radius", 0.25f, 0.05f, 1f, 0.05f, WerewolfSpawnRate, Format: "x");

            PlaguebearerSpawnRate = CustomOption.Create(2927, Types.NeutralKiller, ColorString(Plaguebearer.Color, "Plaguebearer"), 0f, 0f, 100f, 10f, null, true, Format: "%");
            PlaguebearerCooldown = CustomOption.Create(2929, Types.NeutralKiller, "Infect Cooldown", 30f, 10f, 60f, 2.5f, PlaguebearerSpawnRate, Format: "s");
            PestilenceCooldown = CustomOption.Create(2928, Types.NeutralKiller, "Pestilence Kill Cooldown", 30f, 10f, 60f, 2.5f, PlaguebearerSpawnRate, Format: "s");
            PestilenceCanUseVents = CustomOption.Create(2930, Types.NeutralKiller, "Pestilence Can Use Vents", true, PlaguebearerSpawnRate);

            AgentSpawnRate = CustomOption.Create(2931, Types.NeutralKiller, ColorString(Agent.Color, "Agent"), 0f, 0f, 100f, 10f, null, true, Format: "%");
            AgentCanUseVents = CustomOption.Create(2939, Types.NeutralKiller, "Agent Can Use Vents", true, AgentSpawnRate);
            HitmanCooldown = CustomOption.Create(2932, Types.NeutralKiller, "Hitman Kill Cooldown", 30f, 10f, 60f, 2.5f, AgentSpawnRate, Format: "s");
            HitmanCanUseVents = CustomOption.Create(2933, Types.NeutralKiller, "Hitman Can Use Vents", true, AgentSpawnRate);
            HitmanDragCooldown = CustomOption.Create(2934, Types.NeutralKiller, "Hitman Drag Cooldown", 30f, 10f, 60f, 2.5f, AgentSpawnRate, Format: "s");
            HitmanMorphDuration = CustomOption.Create(2935, Types.NeutralKiller, "Hitman Morph Duration", 10f, 1f, 20f, 0.5f, AgentSpawnRate, Format: "s");
            HitmanMorphCooldown = CustomOption.Create(2936, Types.NeutralKiller, "Hitman Morph Cooldown", 30f, 10f, 60f, 2.5f, AgentSpawnRate, Format: "s");
            HitmanDragSpeed = CustomOption.Create(2937, Types.NeutralKiller, "Hitman Drag Speed", 1f, 0.5f, 3f, 0.1f, AgentSpawnRate, Format: "x");
            HitmanSpawnsWithNoAgent = CustomOption.Create(2938, Types.NeutralKiller, "Hitman Can Spawn With No Agent In Game", false, AgentSpawnRate);

            jackalSpawnRate = CustomOption.Create(220, Types.NeutralKiller, ColorString(Jackal.Color, "Jackal"), 0f, 0f, 100f, 10f, null, true, Format: "%");
            jackalKillCooldown = CustomOption.Create(221, Types.NeutralKiller, "Team Jackal Kill Cooldown", 30f, 10f, 60f, 2.5f, jackalSpawnRate, Format: "s");
            jackalCreateSidekickCooldown = CustomOption.Create(222, Types.NeutralKiller, "Jackal Create Sidekick Cooldown", 30f, 10f, 60f, 2.5f, jackalSpawnRate, Format: "s");
            jackalCanUseVents = CustomOption.Create(223, Types.NeutralKiller, "Jackal Can Use Vents", true, jackalSpawnRate);
            jackalCanCreateSidekick = CustomOption.Create(224, Types.NeutralKiller, "Jackal Can Create A Sidekick", false, jackalSpawnRate);
            sidekickPromotesToJackal = CustomOption.Create(225, Types.NeutralKiller, "Sidekick Gets Promoted To Jackal On Jackal Death", false, jackalCanCreateSidekick);
            sidekickCanKill = CustomOption.Create(226, Types.NeutralKiller, "Sidekick Can Kill", false, jackalCanCreateSidekick);
            sidekickCanUseVents = CustomOption.Create(227, Types.NeutralKiller, "Sidekick Can Use Vents", true, jackalCanCreateSidekick);
            jackalPromotedFromSidekickCanCreateSidekick = CustomOption.Create(228, Types.NeutralKiller, "Promoted Sidekicks Can Create A New Sidekick", false, sidekickPromotesToJackal);
            jackalCanCreateSidekickFromImpostor = CustomOption.Create(229, Types.NeutralKiller, "Jackals Can Sidekick Killers", true, jackalCanCreateSidekick);

            JuggernautSpawnRate = CustomOption.Create(872, Types.NeutralKiller, ColorString(Juggernaut.Color, "Juggernaut"), 0f, 0f, 100f, 10f, null, true, Format: "%");
            JuggernautCooldown = CustomOption.Create(882, Types.NeutralKiller, "Initial Juggernaut Cooldown", 25f, 10f, 60f, 2.5f, JuggernautSpawnRate, Format: "s");
            JuggernautReducedCooldown = CustomOption.Create(892, Types.NeutralKiller, "Juggernaut Reduced Cooldown Per Kill", 5f, 2.5f, 10f, 2.5f, JuggernautSpawnRate, Format: "s");
            JuggernautCanUseVents = CustomOption.Create(902, Types.NeutralKiller, "Juggernaut Can Use Vents", false, JuggernautSpawnRate);

            PredatorSpawnRate = CustomOption.Create(2291, Types.NeutralKiller, ColorString(Predator.Color, "Predator"), 0f, 0f, 100f, 10f, null, true, Format: "%");
            PredatorTerminateCooldown = CustomOption.Create(2211, Types.NeutralKiller, "Predator Terminate Cooldown", 25f, 10f, 60f, 2.5f, PredatorSpawnRate, Format: "s");
            PredatorTerminateDuration = CustomOption.Create(2221, Types.NeutralKiller, "Predator Terminate Duration", 25f, 10f, 60f, 2.5f, PredatorSpawnRate, Format: "s");
            PredatorTerminateKillCooldown = CustomOption.Create(2231, Types.NeutralKiller, "Predator Terminating Kill Cooldown", 10f, 0.5f, 15f, 0.5f, PredatorSpawnRate, Format: "s");
            PredatorCanUseVents = CustomOption.Create(2241, Types.NeutralKiller, "Predator Can Use Vents", false, PredatorSpawnRate);

            jesterSpawnRate = CustomOption.Create(60, Types.Neutral, ColorString(Jester.Color, "Jester"), 0f, 0f, 100f, 10f, null, true, Format: "%");
            jesterCanCallEmergency = CustomOption.Create(61, Types.Neutral, "Jester Can Call Emergency Meeting", true, jesterSpawnRate);
            jesterHasImpostorVision = CustomOption.Create(62, Types.Neutral, "Jester Has Impostor Vision", false, jesterSpawnRate);
            jesterCanHideInVents = CustomOption.Create(6211, Types.Neutral, "Jester Can Hide In Vents", false, jesterSpawnRate);
            jesterCanMoveInVents = CustomOption.Create(6222, Types.Neutral, "Jester Can Move In Vents", false, jesterCanHideInVents);

            arsonistSpawnRate = CustomOption.Create(290, Types.Neutral, ColorString(Arsonist.Color, "Arsonist"), 0f, 0f, 100f, 10f, null, true, Format: "%");
            arsonistCooldown = CustomOption.Create(291, Types.Neutral, "Arsonist Cooldown", 12.5f, 2.5f, 60f, 2.5f, arsonistSpawnRate, Format: "s");
            arsonistDuration = CustomOption.Create(292, Types.Neutral, "Arsonist Douse Duration", 3f, 1f, 10f, 1f, arsonistSpawnRate, Format: "s");

            vultureSpawnRate = CustomOption.Create(340, Types.Neutral, ColorString(Vulture.Color, "Vulture"), 0f, 0f, 100f, 10f, null, true, Format: "%");
            vultureCooldown = CustomOption.Create(341, Types.Neutral, "Vulture Cooldown", 15f, 10f, 60f, 2.5f, vultureSpawnRate, Format: "s");
            vultureNumberToWin = CustomOption.Create(342, Types.Neutral, "Number Of Corpses Needed To Be Eaten", 4f, 1f, 10f, 1f, vultureSpawnRate);
            vultureCanUseVents = CustomOption.Create(343, Types.Neutral, "Vulture Can Use Vents", true, vultureSpawnRate);
            vultureShowArrows = CustomOption.Create(344, Types.Neutral, "Show Arrows Pointing Towards The Corpses", true, vultureSpawnRate);

            AmnesiacSpawnRate = CustomOption.Create(3521, Types.Neutral, ColorString(Amnesiac.Color, "Amnesiac"), 0f, 0f, 100f, 10f, null, true, Format: "%");
            AmnesiacHasArrows = CustomOption.Create(3522, Types.Neutral, "Amnesiac Has Arrows Pointing To Corpses", false, AmnesiacSpawnRate);

            RomanticSpawnChance = CustomOption.Create(3501, Types.Neutral, ColorString(Romantic.Color, "Romantic"), 0f, 0f, 100f, 10f, null, true, Format: "%");
            RomanticKnowsRole = CustomOption.Create(3551, Types.Neutral, "Romantic And Beloved Know Each Other's Role", false, RomanticSpawnChance);
            VengefulRomanticCanUseVents = CustomOption.Create(3552, Types.Neutral, "Vengeful Romantic Can Use Vents", false, RomanticSpawnChance);
            VengefulRomanticCooldown = CustomOption.Create(3553, Types.Neutral, "Vengeful Romantic Kill Cooldown", 30f, 10f, 60f, 2.5f, RomanticSpawnChance, Format: "s");

            lawyerSpawnRate = CustomOption.Create(350, Types.Neutral, ColorString(Lawyer.Color, "Lawyer"), 0f, 0f, 100f, 10f, null, true, Format: "%");
            lawyerVision = CustomOption.Create(354, Types.Neutral, "Vision", 1f, 0.25f, 3f, 0.25f, lawyerSpawnRate);
            lawyerKnowsRole = CustomOption.Create(355, Types.Neutral, "Lawyer Knows Target Role", false, lawyerSpawnRate);
            lawyerCanCallEmergency = CustomOption.Create(352, Types.Neutral, "Lawyer Can Call Emergency Meeting", true, lawyerSpawnRate);
            lawyerTargetCanBeJester = CustomOption.Create(351, Types.Neutral, "Lawyer Target Can Be The Jester", false, lawyerSpawnRate);

            ProsecutorSpawnRate = CustomOption.Create(3501, Types.Neutral, ColorString(Prosecutor.Color, "Prosecutor"), 0f, 0f, 100f, 10f, null, true, Format: "%");
            ProsecutorBecomeEnum = CustomOption.Create(3545, Types.Neutral, "Prosecutor Role On Target Death", new string [] {"Jester", "Amnesiac", "Pursuer" }, ProsecutorSpawnRate);
            ProsecutorVision = CustomOption.Create(3541, Types.Neutral, "Vision", 1f, 0.25f, 3f, 0.25f, ProsecutorSpawnRate);
            ProsecutorKnowsRole = CustomOption.Create(3551, Types.Neutral, "Prosecutor Knows Target Role", true, ProsecutorSpawnRate);
            ProsecutorCanCallEmergency = CustomOption.Create(3521, Types.Neutral, "Prosecutor Can Call Emergency Meeting", true, ProsecutorSpawnRate);

            ShowHidePursuerSettings = CustomOption.Create(3561, Types.Neutral, "Show/Hide Pursuer Options", true, null, true);
            pursuerCooldown = CustomOption.Create(356, Types.Neutral, "Pursuer Blank Cooldown", 30f, 5f, 60f, 2.5f, ShowHidePursuerSettings, Format: "s");
            pursuerBlanksNumber = CustomOption.Create(357, Types.Neutral, "Pursuer Number Of Blanks", 5f, 1f, 20f, 1f, ShowHidePursuerSettings);

            mayorSpawnRate = CustomOption.Create(80, Types.Crewmate, ColorString(Mayor.Color, "Mayor"), 0f, 0f, 100f, 10f, null, true, Format: "%");
            mayorCanSeeVoteColors = CustomOption.Create(81, Types.Crewmate, "Mayor Can See Vote Colors", false, mayorSpawnRate);
            mayorTasksNeededToSeeVoteColors = CustomOption.Create(82, Types.Crewmate, "Completed Tasks Needed To See Vote Colors", 5f, 0f, 20f, 1f, mayorCanSeeVoteColors);
            mayorMeetingButton = CustomOption.Create(83, Types.Crewmate, "Mobile Emergency Button", true, mayorSpawnRate);
            mayorMaxRemoteMeetings = CustomOption.Create(84, Types.Crewmate, "Number Of Remote Meetings", 1f, 1f, 5f, 1f, mayorMeetingButton);
            mayorChooseSingleVote = CustomOption.Create(85, Types.Crewmate, "Mayor Can Choose Single Vote", new string[] { "Off", "On (Before Voting)", "On (Until Meeting Ends)" }, mayorSpawnRate);

            engineerSpawnRate = CustomOption.Create(90, Types.Crewmate, ColorString(Engineer.Color, "Engineer"), 0f, 0f, 100f, 10f, null, true, Format: "%");
            engineerNumberOfFixes = CustomOption.Create(91, Types.Crewmate, "Number Of Sabotage Fixes", 1f, 1f, 3f, 1f, engineerSpawnRate);
            engineerHighlightForImpostors = CustomOption.Create(92, Types.Crewmate, "Impostors See Vents Highlighted", true, engineerSpawnRate);
            engineerHighlightForTeamJackal = CustomOption.Create(93, Types.Crewmate, "Jackal and Sidekick See Vents Highlighted ", true, engineerSpawnRate);

            sheriffSpawnRate = CustomOption.Create(100, Types.Crewmate, ColorString(Sheriff.Color, "Sheriff"), 0f, 0f, 100f, 10f, null, true, Format: "%");
            sheriffCooldown = CustomOption.Create(101, Types.Crewmate, "Sheriff Cooldown", 30f, 10f, 60f, 2.5f, sheriffSpawnRate, Format: "s");
            sheriffCanKillNeutrals = CustomOption.Create(102, Types.Crewmate, "Sheriff Can Kill Passive Neutrals", false, sheriffSpawnRate);

            lighterSpawnRate = CustomOption.Create(110, Types.Crewmate, ColorString(Lighter.Color, "Lighter"), 0f, 0f, 100f, 10f, null, true, Format: "%");
            lighterModeLightsOnVision = CustomOption.Create(111, Types.Crewmate, "Vision On Lights On", 1.5f, 0.25f, 5f, 0.25f, lighterSpawnRate);
            lighterModeLightsOffVision = CustomOption.Create(112, Types.Crewmate, "Vision On Lights Off", 0.5f, 0.25f, 5f, 0.25f, lighterSpawnRate);
            lighterFlashlightWidth = CustomOption.Create(113, Types.Crewmate, "Flashlight Width", 0.3f, 0.1f, 1f, 0.1f, lighterSpawnRate);

            CrusaderSpawnRate = CustomOption.Create(1201, Types.Crewmate, ColorString(Crusader.Color, "Crusader"), 0f, 0f, 100f, 10f, null, true, Format: "%");
            CrusaderCooldown = CustomOption.Create(1211, Types.Crewmate, "Crusader Cooldown", 30f, 10f, 60f, 2.5f, CrusaderSpawnRate, Format: "s");
            CrusaderCharges = CustomOption.Create(1212, Types.Crewmate, "Initial Fortify Charges", 1f, 0f, 5f, 1f, CrusaderSpawnRate);
            CrusaderRechargeTasksNumber = CustomOption.Create(1213, Types.Crewmate, "Number Of Tasks The Crusader Needs For Recharging", 2f, 1f, 10f, 1f, CrusaderSpawnRate);

            OracleSpawnRate = CustomOption.Create(5512, Types.Crewmate, ColorString(Oracle.Color, "Oracle"), 0f, 0f, 100f, 10f, null, true, Format: "%");
            ConfessCooldown = CustomOption.Create(5612, Types.Crewmate, "Oracle Cooldown", 25f, 10f, 60f, 2.5f, OracleSpawnRate, Format: "s");
            RevealAccuracy = CustomOption.Create(5712, Types.Crewmate, "Oracle Reveal Accuracy", 0f, 0f, 100f, 10f, OracleSpawnRate, Format: "%");
            NeutralBenignShowsEvil = CustomOption.Create(5812, Types.Crewmate, "Neutral Benign Roles Show Evil", false, OracleSpawnRate);
            NeutralEvilShowsEvil = CustomOption.Create(5912, Types.Crewmate, "Neutral Evil Roles Show Evil", false, OracleSpawnRate);
            OracleCharges = CustomOption.Create(1631, Types.Crewmate, "Initial Oracle Charges", 1f, 0f, 5f, 1f, OracleSpawnRate);
            OracleRechargeTasksNumber = CustomOption.Create(1632, Types.Crewmate, "Number Of Oracle Tasks Needed For Recharging", 2f, 1f, 10f, 1f, OracleSpawnRate);

            detectiveSpawnRate = CustomOption.Create(120, Types.Crewmate, ColorString(Detective.Color, "Detective"), 0f, 0f, 100f, 10f, null, true, Format: "%");
            detectiveAnonymousFootprints = CustomOption.Create(121, Types.Crewmate, "Anonymous Footprints", false, detectiveSpawnRate); 
            detectiveFootprintIntervall = CustomOption.Create(122, Types.Crewmate, "Footprint Intervall", 0.5f, 0.25f, 10f, 0.25f, detectiveSpawnRate, Format: "x");
            detectiveFootprintDuration = CustomOption.Create(123, Types.Crewmate, "Footprint Duration", 5f, 0.25f, 10f, 0.25f, detectiveSpawnRate, Format: "s");
            detectiveReportNameDuration = CustomOption.Create(124, Types.Crewmate, "Time Where Detective Reports Will Have Name", 0, 0, 60, 2.5f, detectiveSpawnRate, Format: "s");
            detectiveReportColorDuration = CustomOption.Create(125, Types.Crewmate, "Time Where Detective Reports Will Have Color Type", 20, 0, 120, 2.5f, detectiveSpawnRate, Format: "s");

            medicSpawnRate = CustomOption.Create(140, Types.Crewmate, ColorString(Medic.Color, "Medic"), 0f, 0f, 100f, 10f, null, true, Format: "%");
            medicShowShielded = CustomOption.Create(143, Types.Crewmate, "Show Shielded Player", new string[] {"Shielded + Medic", "Medic Only", "Shielded Only"}, medicSpawnRate);
            medicShowAttemptToShielded = CustomOption.Create(144, Types.Crewmate, "Shielded Player Sees Murder Attempt", false, medicSpawnRate);
            medicSetOrShowShieldAfterMeeting = CustomOption.Create(145, Types.Crewmate, "Shield Will Be Activated", new string[] { "Instantly", "Instantly, Visible\nAfter Meeting", "After Meeting" }, medicSpawnRate);
            medicShowAttemptToMedic = CustomOption.Create(146, Types.Crewmate, "Medic Sees Murder Attempt On Shielded Player", false, medicSpawnRate);

            VeteranSpawnRate = CustomOption.Create(15011, Types.Crewmate, ColorString(Veteran.Color, "Veteran"), 0f, 0f, 100f, 10f, null, true, Format: "%");
            VeteranCooldown = CustomOption.Create(151, Types.Crewmate, "Alert Cooldown", 30f, 10f, 120f, 2.5f, VeteranSpawnRate, Format: "s");
            VeteranDuration = CustomOption.Create(152, Types.Crewmate, "Alert Duration", 10f, 5f, 15f, 1f, VeteranSpawnRate, Format: "s");
            VeteranCharges = CustomOption.Create(153, Types.Crewmate, "Initial Alert Charges", 1f, 0f, 5f, 1f, VeteranSpawnRate);
            VeteranRechargeTasksNumber = CustomOption.Create(154, Types.Crewmate, "Number Of Tasks The Veteran Needs For Recharging", 2f, 1f, 10f, 1f, VeteranSpawnRate);

            swapperSpawnRate = CustomOption.Create(150, Types.Crewmate, ColorString(Swapper.Color, "Swapper"), 0f, 0f, 100f, 10f, null, true, Format: "%");
            swapperCanCallEmergency = CustomOption.Create(151, Types.Crewmate, "Swapper Can Call Emergency Meeting", false, swapperSpawnRate);
            swapperCanOnlySwapOthers = CustomOption.Create(152, Types.Crewmate, "Swapper Can Only Swap Others", false, swapperSpawnRate);
            swapperSwapsNumber = CustomOption.Create(153, Types.Crewmate, "Initial Swap Charges", 1f, 0f, 5f, 1f, swapperSpawnRate);
            swapperRechargeTasksNumber = CustomOption.Create(154, Types.Crewmate, "Number Of Tasks The Swapper Needs For Recharging", 2f, 1f, 10f, 1f, swapperSpawnRate);


            MysticSpawnRate = CustomOption.Create(160, Types.Crewmate, ColorString(Mystic.Color, "Mystic"), 0f, 0f, 100f, 10f, null, true, Format: "%");
            MysticMode = CustomOption.Create(161, Types.Crewmate, "Mystic Mode", new string[]{ "Show Death Flash + Souls", "Show Death Flash", "Show Souls"}, MysticSpawnRate);
            MysticLimitSoulDuration = CustomOption.Create(163, Types.Crewmate, "Mystic Limit Soul Duration", false, MysticSpawnRate);
            MysticSoulDuration = CustomOption.Create(162, Types.Crewmate, "Mystic Soul Duration", 15f, 0f, 120f, 5f, MysticLimitSoulDuration, Format: "s");
            MysticCooldown = CustomOption.Create(163, Types.Crewmate, "Mystic Reveal Cooldown", 30f, 10f, 120f, 2.5f, MysticSpawnRate, Format: "s");
            MysticCharges = CustomOption.Create(16311, Types.Crewmate, "Initial Mystic Charges", 1f, 0f, 5f, 1f, MysticSpawnRate);
            MysticRechargeTasksNumber = CustomOption.Create(16312, Types.Crewmate, "Number Of Tasks The Mystic Needs For Recharging", 2f, 1f, 10f, 1f, MysticSpawnRate);
        
            hackerSpawnRate = CustomOption.Create(170, Types.Crewmate, ColorString(Hacker.Color, "Hacker"), 0f, 0f, 100f, 10f, null, true, Format: "%");
            hackerCooldown = CustomOption.Create(171, Types.Crewmate, "Hacker Cooldown", 30f, 5f, 60f, 5f, hackerSpawnRate, Format: "s");
            hackerHackeringDuration = CustomOption.Create(172, Types.Crewmate, "Hacker Duration", 10f, 2.5f, 60f, 2.5f, hackerSpawnRate, Format: "s");
            hackerOnlyColorType = CustomOption.Create(173, Types.Crewmate, "Hacker Only Sees Color Type", false, hackerSpawnRate);
            hackerToolsNumber = CustomOption.Create(174, Types.Crewmate, "Max Mobile Gadget Charges", 5f, 1f, 30f, 1f, hackerSpawnRate);
            hackerRechargeTasksNumber = CustomOption.Create(175, Types.Crewmate, "Number Of Tasks The Hacker Needs For Recharging", 2f, 1f, 5f, 1f, hackerSpawnRate);
            hackerNoMove = CustomOption.Create(176, Types.Crewmate, "Cant Move During Mobile Gadget Duration", true, hackerSpawnRate);

            trackerSpawnRate = CustomOption.Create(200, Types.Crewmate, ColorString(Tracker.Color, "Tracker"), 0f, 0f, 100f, 10f, null, true, Format: "%");
            trackerUpdateIntervall = CustomOption.Create(201, Types.Crewmate, "Tracker Update Intervall", 5f, 1f, 30f, 1f, trackerSpawnRate, Format: "s");
            trackerResetTargetAfterMeeting = CustomOption.Create(202, Types.Crewmate, "Tracker Reset Target After Meeting", false, trackerSpawnRate);
            trackerCanTrackCorpses = CustomOption.Create(203, Types.Crewmate, "Tracker Can Track Corpses", true, trackerSpawnRate);
            trackerCorpsesTrackingCooldown = CustomOption.Create(204, Types.Crewmate, "Corpses Tracking Cooldown", 30f, 5f, 120f, 5f, trackerCanTrackCorpses, Format: "s");
            trackerCorpsesTrackingDuration = CustomOption.Create(205, Types.Crewmate, "Corpses Tracking Duration", 5f, 2.5f, 30f, 2.5f, trackerCanTrackCorpses, Format: "s");
            trackerTrackingMethod = CustomOption.Create(206, Types.Crewmate, "How Tracker Gets Target Location", new string[] { "Arrow Only", "Proximity Dectector Only", "Arrow + Proximity" }, trackerSpawnRate);
                           
            spySpawnRate = CustomOption.Create(240, Types.Crewmate, ColorString(Spy.Color, "Spy"), 0f, 0f, 100f, 10f, null, true, Format: "%");
            spyCanDieToSheriff = CustomOption.Create(241, Types.Crewmate, "Spy Can Die To Sheriff", false, spySpawnRate);
            spyImpostorsCanKillAnyone = CustomOption.Create(242, Types.Crewmate, "Impostors Can Kill Anyone If There Is A Spy", true, spySpawnRate);
            spyCanEnterVents = CustomOption.Create(243, Types.Crewmate, "Spy Can Enter Vents", false, spySpawnRate);
            spyHasImpostorVision = CustomOption.Create(244, Types.Crewmate, "Spy Has Impostor Vision", false, spySpawnRate);

            portalmakerSpawnRate = CustomOption.Create(390, Types.Crewmate, ColorString(Portalmaker.Color, "Portalmaker"), 0f, 0f, 100f, 10f, null, true, Format: "%");
            portalmakerCooldown = CustomOption.Create(391, Types.Crewmate, "Portalmaker Cooldown", 30f, 10f, 60f, 2.5f, portalmakerSpawnRate, Format: "s");
            portalmakerUsePortalCooldown = CustomOption.Create(392, Types.Crewmate, "Use Portal Cooldown", 30f, 10f, 60f, 2.5f, portalmakerSpawnRate, Format: "s");
            portalmakerLogOnlyColorType = CustomOption.Create(393, Types.Crewmate, "Portalmaker Log Only Shows Color Type", true, portalmakerSpawnRate);
            portalmakerLogHasTime = CustomOption.Create(394, Types.Crewmate, "Log Shows Time", true, portalmakerSpawnRate);
            portalmakerCanPortalFromAnywhere = CustomOption.Create(395, Types.Crewmate, "Can Port To Portal From Everywhere", true, portalmakerSpawnRate);

            VigilanteSpawnRate = CustomOption.Create(280, Types.Crewmate, ColorString(Vigilante.Color, "Vigilante"), 0f, 0f, 100f, 10f, null, true, Format: "%");
            VigilanteCooldown = CustomOption.Create(281, Types.Crewmate, "Vigilante Cooldown", 30f, 10f, 60f, 2.5f, VigilanteSpawnRate, Format: "s");
            VigilanteTotalScrews = CustomOption.Create(282, Types.Crewmate, "Vigilante Number Of Screws", 7f, 1f, 15f, 1f, VigilanteSpawnRate);
            VigilanteCamPrice = CustomOption.Create(283, Types.Crewmate, "Number Of Screws Per Cam", 2f, 1f, 15f, 1f, VigilanteSpawnRate);
            VigilanteVentPrice = CustomOption.Create(284, Types.Crewmate, "Number Of Screws Per Vent", 1f, 1f, 15f, 1f, VigilanteSpawnRate);
            VigilanteCamDuration = CustomOption.Create(285, Types.Crewmate, "Vigilante Duration", 10f, 2.5f, 60f, 2.5f, VigilanteSpawnRate, Format: "s");
            VigilanteCamMaxCharges = CustomOption.Create(286, Types.Crewmate, "Gadget Max Charges", 5f, 1f, 30f, 1f, VigilanteSpawnRate);
            VigilanteCamRechargeTasksNumber = CustomOption.Create(287, Types.Crewmate, "Number Of Tasks The Vigilante Needs For Recharging", 3f, 1f, 10f, 1f, VigilanteSpawnRate);
            VigilanteNoMove = CustomOption.Create(288, Types.Crewmate, "Cant Move During Cam Duration", true, VigilanteSpawnRate);

            timeMasterSpawnRate = CustomOption.Create(130, Types.Crewmate, ColorString(TimeMaster.Color, "Time Master"), 0f, 0f, 100f, 10f, null, true, Format: "%");
            timeMasterCooldown = CustomOption.Create(131, Types.Crewmate, "Time Master Cooldown", 30f, 20f, 120f, 5f, timeMasterSpawnRate, Format: "s");
            timeMasterRewindTime = CustomOption.Create(132, Types.Crewmate, "Rewind Time Duration", 3f, 1f, 5f, 1f, timeMasterSpawnRate, Format: "s");
            TimeMasterReviveDuringRewind = CustomOption.Create(133, Types.Crewmate, "Time Master Revives During Rewind", false, timeMasterSpawnRate);

            mediumSpawnRate = CustomOption.Create(360, Types.Crewmate, ColorString(Medium.Color, "Medium"), 0f, 0f, 100f, 10f, null, true, Format: "%");
            mediumCooldown = CustomOption.Create(361, Types.Crewmate, "Medium Questioning Cooldown", 30f, 5f, 120f, 5f, mediumSpawnRate, Format: "s");
            mediumDuration = CustomOption.Create(362, Types.Crewmate, "Medium Questioning Duration", 3f, 0f, 15f, 1f, mediumSpawnRate, Format: "s");
            mediumOneTimeUse = CustomOption.Create(363, Types.Crewmate, "Each Soul Can Only Be Questioned Once", false, mediumSpawnRate);
            mediumChanceAdditionalInfo = CustomOption.Create(364, Types.Crewmate, "Chance That The Answer Contains \n    Additional Information", 0f, 0f, 100f, 10f, mediumSpawnRate, Format: "%");

            trapperSpawnRate = CustomOption.Create(410, Types.Crewmate, ColorString(Trapper.Color, "Trapper"), 0f, 0f, 100f, 10f, null, true, Format: "%");
            trapperCooldown = CustomOption.Create(420, Types.Crewmate, "Trapper Cooldown", 30f, 5f, 120f, 5f, trapperSpawnRate, Format: "s");
            trapperMaxCharges = CustomOption.Create(440, Types.Crewmate, "Max Traps Charges", 5f, 1f, 15f, 1f, trapperSpawnRate);
            trapperRechargeTasksNumber = CustomOption.Create(450, Types.Crewmate, "Number Of Tasks The Trapper Needs For Recharging", 2f, 1f, 15f, 1f, trapperSpawnRate);
            trapperTrapNeededTriggerToReveal = CustomOption.Create(451, Types.Crewmate, "Trap Needed Trigger To Reveal", 3f, 2f, 10f, 1f, trapperSpawnRate);
            trapperAnonymousMap = CustomOption.Create(452, Types.Crewmate, "Show Anonymous Map", false, trapperSpawnRate);
            trapperInfoType = CustomOption.Create(453, Types.Crewmate, "Trap Information Type", new string[] { "Role", "Good/Evil Role", "Name" }, trapperSpawnRate);
            trapperTrapDuration = CustomOption.Create(454, Types.Crewmate, "Trap Duration", 5f, 1f, 15f, 1f, trapperSpawnRate, Format: "s");

            // Modifier (1000 - 1999)
            modifiersAreHidden = CustomOption.Create(1009, Types.Modifier, ColorString(Color.yellow, "VIP & Bait Are Hidden"), true, null, true, Heading: ColorString(Color.yellow, "Hide After Death Modifiers"));

            modifierLazy = CustomOption.Create(1010, Types.Modifier, ColorString(Color.yellow, "Lazy"), 0f, 0f, 100f, 10f, null, true, Format: "%");
            modifierLazyQuantity = CustomOption.Create(1011, Types.Modifier, ColorString(Color.yellow, "Lazy Quantity"), rates, modifierLazy);

            ModifierSleuth = CustomOption.Create(1005, Types.Modifier, ColorString(Color.yellow, "Sleuth"), 0f, 0f, 100f, 10f, null, true, Format: "%");
            ModifierSleuthQuantity = CustomOption.Create(1006, Types.Modifier, ColorString(Color.yellow, "Sleuth Quantity"), rates, ModifierSleuth);

            modifierTieBreaker = CustomOption.Create(1020, Types.Modifier, ColorString(Color.yellow, "Tie Breaker"), 0f, 0f, 100f, 10f, null, true, Format: "%");

            modifierBait = CustomOption.Create(1030, Types.Modifier, ColorString(Color.yellow, "Bait"), 0f, 0f, 100f, 10f, null, true, Format: "%");
            modifierBaitQuantity = CustomOption.Create(1031, Types.Modifier, ColorString(Color.yellow, "Bait Quantity"), rates, modifierBait);
            modifierBaitReportDelayMin = CustomOption.Create(1032, Types.Modifier, "Bait Report Delay Min", 0f, 0f, 10f, 1f, modifierBait, Format: "s");
            modifierBaitReportDelayMax = CustomOption.Create(1033, Types.Modifier, "Bait Report Delay Max", 0f, 0f, 10f, 1f, modifierBait, Format: "s");
            modifierBaitShowKillFlash = CustomOption.Create(1034, Types.Modifier, "Warn The Killer With A Flash", true, modifierBait);

            modifierLover = CustomOption.Create(1040, Types.Modifier, ColorString(Color.yellow, "Lovers"), 0f, 0f, 100f, 10f, null, true, Format: "%");
            modifierLoverImpLoverRate = CustomOption.Create(1041, Types.Modifier, "Chance That One Lover Is A Killer", 0f, 0f, 100f, 10f, modifierLover, Format: "%");
            modifierLoverBothDie = CustomOption.Create(1042, Types.Modifier, "Lovers Die Together", true, modifierLover);
            modifierLoverEnableChat = CustomOption.Create(1043, Types.Modifier, "Enable Lover Chat", true, modifierLover);

            modifierSunglasses = CustomOption.Create(1050, Types.Modifier, ColorString(Color.yellow, "Sunglasses"), 0f, 0f, 100f, 10f, null, true, Format: "%");
            modifierSunglassesQuantity = CustomOption.Create(1051, Types.Modifier, ColorString(Color.yellow, "Sunglasses Quantity"), rates, modifierSunglasses);
            modifierSunglassesVision = CustomOption.Create(1052, Types.Modifier, "Vision With Sunglasses", new string[] { "-10%", "-20%", "-30%", "-40%", "-50%" }, modifierSunglasses);

            modifierMini = CustomOption.Create(1061, Types.Modifier, ColorString(Color.yellow, "Mini"), 0f, 0f, 100f, 10f, null, true, Format: "%");
            modifierMiniGrowingUpDuration = CustomOption.Create(1062, Types.Modifier, "Mini Growing Up Duration", 400f, 100f, 1500f, 100f, modifierMini);
            modifierMiniGrowingUpInMeeting = CustomOption.Create(1063, Types.Modifier, "Mini Grows Up In Meeting", true, modifierMini);

            modifierVip = CustomOption.Create(1070, Types.Modifier, ColorString(Color.yellow, "VIP"), 0f, 0f, 100f, 10f, null, true, Format: "%");
            modifierVipQuantity = CustomOption.Create(1071, Types.Modifier, ColorString(Color.yellow, "VIP Quantity"), rates, modifierVip);
            modifierVipShowColor = CustomOption.Create(1072, Types.Modifier, "Show Team Color", true, modifierVip);

            modifierInvert = CustomOption.Create(1080, Types.Modifier, ColorString(Color.yellow, "Invert"), 0f, 0f, 100f, 10f, null, true, Format: "%");
            modifierInvertQuantity = CustomOption.Create(1081, Types.Modifier, ColorString(Color.yellow, "Modifier Quantity"), rates, modifierInvert);
            modifierInvertDuration = CustomOption.Create(1082, Types.Modifier, "Number Of Meetings Inverted", 3f, 1f, 15f, 1f, modifierInvert);

            modifierChameleon = CustomOption.Create(1090, Types.Modifier, ColorString(Color.yellow, "Chameleon"), 0f, 0f, 100f, 10f, null, true, Format: "%");
            modifierChameleonQuantity = CustomOption.Create(1091, Types.Modifier, ColorString(Color.yellow, "Chameleon Quantity"), rates, modifierChameleon);
            modifierChameleonHoldDuration = CustomOption.Create(1092, Types.Modifier, "Time Until Fading Starts", 3f, 1f, 10f, 0.5f, modifierChameleon, Format: "s");
            modifierChameleonFadeDuration = CustomOption.Create(1093, Types.Modifier, "Fade Duration", 1f, 0.25f, 10f, 0.25f, modifierChameleon, Format: "s");
            modifierChameleonMinVisibility = CustomOption.Create(1094, Types.Modifier, "Minimum Visibility", new string[] { "0%", "10%", "20%", "30%", "40%", "50%" }, modifierChameleon);

            modifierArmored = CustomOption.Create(1101, Types.Modifier, ColorString(Color.yellow, "Armored"), 0f, 0f, 100f, 10f, null, true, Format: "%");

            // Guesser Gamemode (2000 - 2999)
            GuesserCrewNumber = CustomOption.Create(2001, Types.Modifier, ColorString(Palette.CrewmateBlue, "Number of Crew Guesser"), 15f, 0f, 15f, 1f, null, true, Heading: "Ability Settings");
            GuesserNeutralNumber = CustomOption.Create(2002, Types.Modifier, ColorString(Color.gray, "Number of Neutral Guesser"), 15f, 0f, 15f, 1f);
            GuesserImpNumber = CustomOption.Create(2003, Types.Modifier, ColorString(Palette.ImpostorRed, "Number of Impostor Guesser"), 15f, 0f, 15f, 1f);
            SidekickIsAlwaysGuesser = CustomOption.Create(2012, Types.Modifier, "Sidekick Is Always Guesser", false);
            GuesserHaveModifier = CustomOption.Create(2004, Types.Modifier, "Guesser Can Have A Modifier", true);
            GuesserNumberOfShots = CustomOption.Create(2005, Types.Modifier, "Guesser Number Of Shots", 3f, 1f, 15f, 1f);
            GuesserHasMultipleShotsPerMeeting = CustomOption.Create(2006, Types.Modifier, "Guesser Can Shoot Multiple Times Per Meeting", false);
            CrewGuesserNumberOfTasks = CustomOption.Create(2013, Types.Modifier, "Number Of Tasks A Crew Guesser Needs To Unlock Shooting", 0f, 0f, 15f, 1f);
            GuesserKillsThroughShield = CustomOption.Create(2008, Types.Modifier, "Guesses Ignore The Medic Shield", true);
            GuesserEvilCanKillSpy = CustomOption.Create(2009, Types.Modifier, "Evil Guesser Can Guess The Spy", true);

            AbilityCoward = CustomOption.Create(1029, Types.Modifier, ColorString(Guesser.AbilityColor, "Coward"), 0f, 0f, 100f, 10f, null, true, Format: "%");
            
            AbilityParanoid = CustomOption.Create(522, Types.Modifier, ColorString(Guesser.AbilityColor, "Paranoid"), 0f, 0f, 100f, 10f, null, true, Format: "%");

            AbilityDisperser = CustomOption.Create(1021, Types.Modifier, ColorString(Color.red, "Disperser"), 0f, 0f, 100f, 10f, null, true, Format: "%");
            AbilityDisperserCooldown = CustomOption.Create(1022, Types.Modifier, "Disperser Cooldown", 30f, 10f, 120f, 5f, AbilityDisperser, Format: "s");
            AbilityDisperserCharges = CustomOption.Create(1023, Types.Modifier, "Initial Disperser Charges", 1f, 0f, 5f, 1f, AbilityDisperser);
            AbilityDisperserKillCharges = CustomOption.Create(1024, Types.Modifier, "Disperse Charges per Kill", 1f, 0f, 5f, 1f, AbilityDisperser);

            // Other options
            maxNumberOfMeetings = CustomOption.Create(3, Types.General, "Number Of Meetings (excluding Mayor meeting)", 10, 0, 15, 1, null, true, Heading: "Gameplay Settings");
            DisableMedbayAnimation = CustomOption.Create(3131, Types.General, "Disable Medbay Walk Animation", true);
            GameStartCooldowns = CustomOption.Create(518, Types.General, "Game Start Cooldowns", 10f, 10f, 30f, 2.5f, Format: "s");
            LimitAbilities = CustomOption.Create(1321, Types.General, "Limit Player Abilities When 2 Players Are Left Alive", true);
            anyPlayerCanStopStart = CustomOption.Create(2, Types.General, ColorString(new Color(204f / 255f, 204f / 255f, 0, 1f), "Any Player Can Stop The Start"), false, null, false);
            blockSkippingInEmergencyMeetings = CustomOption.Create(4, Types.General, "Block Skipping In Emergency Meetings", false);
            noVoteIsSelfVote = CustomOption.Create(5, Types.General, "No Vote Is Self Vote", false, blockSkippingInEmergencyMeetings);
            hidePlayerNames = CustomOption.Create(6, Types.General, "Hide Player Names", false);
            RandomSpawns = CustomOption.Create(13213, Types.General, "Enable Random Player Spawns", false);
            allowParallelMedBayScans = CustomOption.Create(7, Types.General, "Allow Parallel MedBay Scans", false);
            ShieldFirstKill = CustomOption.Create(8, Types.General, "Shield Last Game First Kill", false);
            finishTasksBeforeHauntingOrZoomingOut = CustomOption.Create(9, Types.General, "Finish Tasks Before Haunting Or Zooming Out", true);
            deadImpsBlockSabotage = CustomOption.Create(13, Types.General, "Block Dead Impostor From Sabotaging", false, null, false);

            camsNightVision = CustomOption.Create(11, Types.General, "Cams Switch To Night Vision If Lights Are Off", false, null, true, Heading: "Night Vision Cams");
            camsNoNightVisionIfImpVision = CustomOption.Create(12, Types.General, "Impostor Vision Ignores Night Vision Cams", false, camsNightVision, false);

            SkeldVentImprovements = CustomOption.Create(3313, Types.General, "Enable Vent Improvements", false, null, true, Heading: "Better Skeld Settings");

            EnableBetterPolus = CustomOption.Create(3314, Types.General, "Enable Better Polus", false, null, true, Heading: "Better Polus Settings");
            BPVentImprovements = CustomOption.Create(3315, Types.General, "Enable Vent Layout", false, EnableBetterPolus);
            BPVitalsLab = CustomOption.Create(3315, Types.General, "Vitals Moved To Lab", false, EnableBetterPolus);
            BPColdTempDeathValley = CustomOption.Create(3316, Types.General, "Cold Temp Moved To Death Valley", false, EnableBetterPolus);
            BPWifiChartCourseSwap = CustomOption.Create(3317, Types.General, "Reboot Wifi And Chart Course Swapped", false, EnableBetterPolus);

            dynamicMap = CustomOption.Create(500, Types.General, "Play On A Random Map", false, null, true, Heading: "Random Maps");
            dynamicMapEnableSkeld = CustomOption.Create(501, Types.General, "Skeld", 0f, 0f, 100f, 10f, dynamicMap, false, Format: "%");
            dynamicMapEnableMira = CustomOption.Create(502, Types.General, "Mira", 0f, 0f, 100f, 10f, dynamicMap, false, Format: "%");
            dynamicMapEnablePolus = CustomOption.Create(503, Types.General, "Polus", 0f, 0f, 100f, 10f, dynamicMap, false, Format: "%");
            dynamicMapEnableAirShip = CustomOption.Create(504, Types.General, "Airship", 0f, 0f, 100f, 10f, dynamicMap, false, Format: "%");
            dynamicMapEnableFungle = CustomOption.Create(506, Types.General, "Fungle", 0f, 0f, 100f, 10f, dynamicMap, false, Format: "%");
            dynamicMapEnableSubmerged = CustomOption.Create(505, Types.General, "Submerged", 0f, 0f, 100f, 10f, dynamicMap, false, Format: "%");
            dynamicMapSeparateSettings = CustomOption.Create(509, Types.General, "Use Random Map Setting Presets", false, dynamicMap, false);

            blockedRolePairings.Add((byte)RoleId.Poisoner, new [] { (byte)RoleId.Warlock});
            blockedRolePairings.Add((byte)RoleId.Warlock, new [] { (byte)RoleId.Poisoner});
            blockedRolePairings.Add((byte)RoleId.Vulture, new [] { (byte)RoleId.Cleaner});
            blockedRolePairings.Add((byte)RoleId.Cleaner, new [] { (byte)RoleId.Vulture});
            blockedRolePairings.Add((byte)RoleId.Amnesiac, new [] { (byte)RoleId.Prosecutor});
            blockedRolePairings.Add((byte)RoleId.Prosecutor, new [] { (byte)RoleId.Amnesiac});
            blockedRolePairings.Add((byte)RoleId.Jester, new [] { (byte)RoleId.Prosecutor});
            blockedRolePairings.Add((byte)RoleId.Prosecutor, new [] { (byte)RoleId.Jester});
            blockedRolePairings.Add((byte)RoleId.Lawyer, new [] { (byte)RoleId.Prosecutor});
            blockedRolePairings.Add((byte)RoleId.Prosecutor, new [] { (byte)RoleId.Lawyer});
            blockedRolePairings.Add((byte)RoleId.Camouflager, new [] { (byte)RoleId.Morphling});
            blockedRolePairings.Add((byte)RoleId.Morphling, new [] { (byte)RoleId.Camouflager});
            
        }
    }
}
