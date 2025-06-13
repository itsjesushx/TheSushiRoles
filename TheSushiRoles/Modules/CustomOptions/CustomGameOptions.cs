namespace TheSushiRoles.Modules.CustomOptions
{
    public static class CustomGameOptions
    {
        // Minimum/Maximum Count
        public static int CrewmateRolesCountMin => Mathf.RoundToInt(CustomOptionHolder.crewmateRolesCountMin.GetFloat());
        public static int CrewmateRolesCountMax => Mathf.RoundToInt(CustomOptionHolder.crewmateRolesCountMax.GetFloat());
        public static int MinNeutralEvilRoles => Mathf.RoundToInt(CustomOptionHolder.MinNeutralEvilRoles.GetFloat());
        public static int MaxNeutralEvilRoles => Mathf.RoundToInt(CustomOptionHolder.MaxNeutralEvilRoles.GetFloat());
        public static int MinNeutralBenignRoles => Mathf.RoundToInt(CustomOptionHolder.MinNeutralBenignRoles.GetFloat());
        public static int MaxNeutralBenignRoles => Mathf.RoundToInt(CustomOptionHolder.MaxNeutralBenignRoles.GetFloat());
        public static int NeutralKillingRolesCountMin => Mathf.RoundToInt(CustomOptionHolder.neutralKillingRolesCountMin.GetFloat());
        public static int NeutralKillingRolesCountMax => Mathf.RoundToInt(CustomOptionHolder.neutralKillingRolesCountMax.GetFloat());
        public static int ImpostorRolesCountMin => Mathf.RoundToInt(CustomOptionHolder.impostorRolesCountMin.GetFloat());
        public static int ImpostorRolesCountMax => Mathf.RoundToInt(CustomOptionHolder.impostorRolesCountMax.GetFloat());
        public static int ModifiersCountMin => Mathf.RoundToInt(CustomOptionHolder.modifiersCountMin.GetFloat());
        public static int ModifiersCountMax => Mathf.RoundToInt(CustomOptionHolder.modifiersCountMax.GetFloat());
        public static int AbilitiesCountMin => Mathf.RoundToInt(CustomOptionHolder.abilitiesCountMin.GetFloat());
        public static int AbilitiesCountMax => Mathf.RoundToInt(CustomOptionHolder.abilitiesCountMax.GetFloat());

        // Morphling
        public static float MorphlingCooldown => CustomOptionHolder.morphlingCooldown.GetFloat();
        public static float MorphlingDuration => CustomOptionHolder.morphlingDuration.GetFloat();

        // Wraith
        public static float WraithCooldown => CustomOptionHolder.WraithCooldown.GetFloat();
        public static float WraithDuration => CustomOptionHolder.WraithDuration.GetFloat();

        // Miner
        public static float MinerCooldown => CustomOptionHolder.MinerCooldown.GetFloat();
        public static int MineVisible => CustomOptionHolder.MineVisible.GetSelection();
        public static float MineDelay => CustomOptionHolder.MineDelay.GetFloat();

        // Painter
        public static float PainterCooldown => CustomOptionHolder.PainterCooldown.GetFloat();
        public static float PainterDuration => CustomOptionHolder.PainterDuration.GetFloat();

        // Viper
        public static float ViperKillDelay => CustomOptionHolder.ViperKillDelay.GetFloat();
        public static float ViperCooldown => CustomOptionHolder.ViperCooldown.GetFloat();
        public static float ViperBlindCooldown => CustomOptionHolder.BlindCooldown.GetFloat();
        public static float ViperBlindDuration => CustomOptionHolder.BlindDuration.GetFloat();

        // Blackmailer
        public static float BlackmailCooldown => CustomOptionHolder.BlackmailCooldown.GetFloat();
        public static bool BlackmailInvisible => CustomOptionHolder.BlackmailInvisible.GetBool();

        // Trickster
        public static float TricksterPlaceBoxCooldown => CustomOptionHolder.tricksterPlaceBoxCooldown.GetFloat();
        public static float TricksterLightsOutCooldown => CustomOptionHolder.tricksterLightsOutCooldown.GetFloat();
        public static float TricksterLightsOutDuration => CustomOptionHolder.tricksterLightsOutDuration.GetFloat();

        // Janitor
        public static float JanitorCooldown => CustomOptionHolder.JanitorCooldown.GetFloat();

        // Warlock
        public static float WarlockCooldown => CustomOptionHolder.warlockCooldown.GetFloat();
        public static float WarlockRootTime => CustomOptionHolder.warlockRootTime.GetFloat();

        // Bounty Hunter
        public static float BountyHunterBountyDuration => CustomOptionHolder.bountyHunterBountyDuration.GetFloat();
        public static float BountyHunterReducedCooldown => CustomOptionHolder.bountyHunterReducedCooldown.GetFloat();
        public static float BountyHunterPunishmentTime => CustomOptionHolder.bountyHunterPunishmentTime.GetFloat();
        public static bool BountyHunterShowArrow => CustomOptionHolder.bountyHunterShowArrow.GetBool();
        public static float BountyHunterArrowUpdateIntervall => CustomOptionHolder.bountyHunterArrowUpdateIntervall.GetFloat();

        // Witch
        public static float WitchCooldown => CustomOptionHolder.witchCooldown.GetFloat();
        public static float WitchAdditionalCooldown => CustomOptionHolder.witchAdditionalCooldown.GetFloat();
        public static bool WitchCanSpellAnyone => CustomOptionHolder.witchCanSpellAnyone.GetBool();
        public static float WitchSpellCastingDuration => CustomOptionHolder.witchSpellCastingDuration.GetFloat();
        public static bool WitchTriggerBothCooldowns => CustomOptionHolder.witchTriggerBothCooldowns.GetBool();
        public static bool WitchVoteSavesTargets => CustomOptionHolder.witchVoteSavesTargets.GetBool();

        // Undertaker
        public static float UndertakerCooldown => CustomOptionHolder.UndertakerCooldown.GetFloat();
        public static float UndertakerDragSpeed => CustomOptionHolder.UndertakerDragSpeed.GetFloat();

        // Assassin
        public static float AssassinCooldown => CustomOptionHolder.AssassinCooldown.GetFloat();
        public static bool AssassinKnowsTargetLocation => CustomOptionHolder.AssassinKnowsTargetLocation.GetBool();
        public static float AssassinTraceTime => CustomOptionHolder.AssassinTraceTime.GetFloat();
        public static float AssassinTraceColorTime => CustomOptionHolder.AssassinTraceColorTime.GetFloat();
        public static float AssassinInvisibleDuration => CustomOptionHolder.AssassinInvisibleDuration.GetFloat();

        // Grenadier
        public static float GrenadierCooldown => CustomOptionHolder.GrenadierCooldown.GetFloat();
        public static float GrenadeDuration => CustomOptionHolder.GrenadierGrenadeDuration.GetFloat();
        public static float GrenadeRadius => CustomOptionHolder.GrenadierGrenadeRadius.GetFloat();

        // Yo-Yo
        public static float YoyoBlinkDuration => CustomOptionHolder.yoyoBlinkDuration.GetFloat();
        public static float YoyoMarkCooldown => CustomOptionHolder.yoyoMarkCooldown.GetFloat();
        public static bool YoyoMarkStaysOverMeeting => CustomOptionHolder.yoyoMarkStaysOverMeeting.GetBool();
        public static bool YoyoHasAdminTable => CustomOptionHolder.yoyoHasAdminTable.GetBool();
        public static float YoyoAdminTableCooldown => CustomOptionHolder.yoyoAdminTableCooldown.GetFloat();
        public static int YoyoSilhouetteVisibility => CustomOptionHolder.yoyoSilhouetteVisibility.GetSelection();

        // Jester
        public static bool JesterCanCallEmergency => CustomOptionHolder.jesterCanCallEmergency.GetBool();
        public static bool JesterHasImpostorVision => CustomOptionHolder.jesterHasImpostorVision.GetBool();
        public static bool JesterCanHideInVents => CustomOptionHolder.jesterCanHideInVents.GetBool();
        public static bool JesterCanMoveInVents => CustomOptionHolder.jesterCanMoveInVents.GetBool();

        // Arsonist
        public static float ArsonistCooldown => CustomOptionHolder.arsonistCooldown.GetFloat();
        public static float ArsonistDuration => CustomOptionHolder.arsonistDuration.GetFloat();

        // Scavenger
        public static float ScavengerCooldown => CustomOptionHolder.ScavengerCooldown.GetFloat();
        public static float ScavengerNumberToWin => CustomOptionHolder.ScavengerNumberToWin.GetFloat();
        public static bool ScavengerCanUseVents => CustomOptionHolder.ScavengerCanUseVents.GetBool();
        public static float ScavengerScavengeCooldown => CustomOptionHolder.ScavengerScavengeCooldown.GetFloat();
        public static float ScavengerScavengeDuration => CustomOptionHolder.ScavengerScavengeDuration.GetFloat();

        // Romantic
        public static bool RomanticKnowsRole => CustomOptionHolder.RomanticKnowsRole.GetBool();
        public static bool VengefulRomanticCanUseVents => CustomOptionHolder.VengefulRomanticCanUseVents.GetBool();
        public static float VengefulRomanticCooldown => CustomOptionHolder.VengefulRomanticCooldown.GetFloat();

        // Lawyer
        public static float LawyerVision => CustomOptionHolder.lawyerVision.GetFloat();
        public static bool LawyerKnowsRole => CustomOptionHolder.lawyerKnowsRole.GetBool();
        public static bool LawyerCanCallEmergency => CustomOptionHolder.lawyerCanCallEmergency.GetBool();
        public static bool LawyerTargetCanBeJester => CustomOptionHolder.lawyerTargetCanBeJester.GetBool();

        // Prosecutor
        public static ProsecutorOnTargetDeath ProsecutorBecomeEnum => (ProsecutorOnTargetDeath)CustomOptionHolder.ProsecutorBecomeEnum.GetSelection();
        public static float ProsecutorVision => CustomOptionHolder.ProsecutorVision.GetFloat();
        public static bool ProsecutorKnowsRole => CustomOptionHolder.ProsecutorKnowsRole.GetBool();
        public static bool ProsecutorCanCallEmergency => CustomOptionHolder.ProsecutorCanCallEmergency.GetBool();

        // Survivor
        public static float SurvivorCooldown => CustomOptionHolder.SurvivorCooldown.GetFloat();
        public static float SurvivorBlanksNumber => CustomOptionHolder.SurvivorBlanksNumber.GetFloat();

        // Glitch
        public static bool GlitchCanUseVents => CustomOptionHolder.GlitchCanUseVents.GetBool();
        public static float GlitchKillCooldown => CustomOptionHolder.GlitchKillCooldown.GetFloat();
        public static int GlitchNumberOfHacks => Mathf.RoundToInt(CustomOptionHolder.GlitchNumberOfHacks.GetFloat());
        public static float GlitchHackCooldown => CustomOptionHolder.GlitchHackCooldown.GetFloat();
        public static float GlitchHackDuration => CustomOptionHolder.GlitchHackDuration.GetFloat();
        public static float GlitchMimicCooldown => CustomOptionHolder.GlitchMimicCooldown.GetFloat();
        public static float GlitchMimicDuration => CustomOptionHolder.GlitchMimicDuration.GetFloat();

        // Werewolf
        public static float WerewolfCooldown => CustomOptionHolder.WerewolfCooldown.GetFloat();
        public static bool WerewolfCanUseVents => CustomOptionHolder.WerewolfCanUseVents.GetBool();
        public static float WerewolfMaulRadius => CustomOptionHolder.WerewolfMaulRadius.GetFloat();

        // Plaguebearer/Pestilence
        public static float PlaguebearerCooldown => CustomOptionHolder.PlaguebearerCooldown.GetFloat();
        public static float PestilenceCooldown => CustomOptionHolder.PestilenceCooldown.GetFloat();
        public static bool PestilenceCanUseVents => CustomOptionHolder.PestilenceCanUseVents.GetBool();

        // Agent/Hitman
        public static bool AgentCanUseVents => CustomOptionHolder.AgentCanUseVents.GetBool();
        public static float HitmanCooldown => CustomOptionHolder.HitmanCooldown.GetFloat();
        public static bool HitmanCanUseVents => CustomOptionHolder.HitmanCanUseVents.GetBool();
        public static float HitmanDragCooldown => CustomOptionHolder.HitmanDragCooldown.GetFloat();
        public static float HitmanMorphDuration => CustomOptionHolder.HitmanMorphDuration.GetFloat();
        public static float HitmanMorphCooldown => CustomOptionHolder.HitmanMorphCooldown.GetFloat();
        public static float HitmanDragSpeed => CustomOptionHolder.HitmanDragSpeed.GetFloat();
        public static bool HitmanSpawnsWithNoAgent => CustomOptionHolder.HitmanSpawnsWithNoAgent.GetBool();

        // Juggernaut
        public static float JuggernautCooldown => CustomOptionHolder.JuggernautCooldown.GetFloat();
        public static float JuggernautReducedCooldown => CustomOptionHolder.JuggernautReducedCooldown.GetFloat();
        public static bool JuggernautCanUseVents => CustomOptionHolder.JuggernautCanUseVents.GetBool();

        // Predator
        public static float PredatorTerminateCooldown => CustomOptionHolder.PredatorTerminateCooldown.GetFloat();
        public static float PredatorTerminateDuration => CustomOptionHolder.PredatorTerminateDuration.GetFloat();
        public static float PredatorTerminateKillCooldown => CustomOptionHolder.PredatorTerminateKillCooldown.GetFloat();
        public static bool PredatorCanUseVents => CustomOptionHolder.PredatorCanUseVents.GetBool();

        // Mayor
        public static bool MayorCanSeeVoteColors => CustomOptionHolder.mayorCanSeeVoteColors.GetBool();
        public static float MayorTasksNeededToSeeVoteColors => CustomOptionHolder.mayorTasksNeededToSeeVoteColors.GetFloat();
        public static bool MayorMeetingButton => CustomOptionHolder.mayorMeetingButton.GetBool();
        public static int MayorMaxRemoteMeetings => Mathf.RoundToInt(CustomOptionHolder.mayorMaxRemoteMeetings.GetFloat());
        public static SingleVotesOptions MayorChooseSingleVote => (SingleVotesOptions)CustomOptionHolder.mayorChooseSingleVote.GetSelection();

        // Engineer
        public static int EngineerNumberOfFixes => Mathf.RoundToInt(CustomOptionHolder.engineerNumberOfFixes.GetFloat());
        public static bool EngineerHighlightForImpostors => CustomOptionHolder.engineerHighlightForImpostors.GetBool();
        public static bool EngineerHighlightForNeutralKillers => CustomOptionHolder.engineerHighlightForNeutralKillers.GetBool();

        // Monarch
        public static float MonarchKnightCooldown => CustomOptionHolder.MonarchKnightCooldown.GetFloat();
        public static bool KnightLoseOnDeath => CustomOptionHolder.KnightLoseOnDeath.GetBool();
        public static int MonarchCharges => Mathf.RoundToInt(CustomOptionHolder.MonarchCharges.GetFloat());
        public static int MonarchRechargeTasksNumber => Mathf.RoundToInt(CustomOptionHolder.MonarchRechargeTasksNumber.GetFloat());

        // Sheriff
        public static float SheriffCooldown => CustomOptionHolder.sheriffCooldown.GetFloat();
        public static bool SheriffCanKillNeutrals => CustomOptionHolder.sheriffCanKillNeutrals.GetBool();

        // Deputy
        public static int DeputyCharges => Mathf.RoundToInt(CustomOptionHolder.DeputyCharges.GetFloat());
        public static int DeputyRechargeTasksNumber => Mathf.RoundToInt(CustomOptionHolder.DeputyRechargeTasksNumber.GetFloat());
        public static bool DeputyKillsThroughShield => CustomOptionHolder.DeputyKillsThroughShield.GetBool();
        public static bool DeputyCanKillNeutralBenign => CustomOptionHolder.DeputyCanKillNeutralBenign.GetBool();
        public static bool DeputyCanKillNeutralEvil => CustomOptionHolder.DeputyCanKillNeutralEvil.GetBool();

        // Crusader
        public static float CrusaderCooldown => CustomOptionHolder.CrusaderCooldown.GetFloat();
        public static int CrusaderCharges => Mathf.RoundToInt(CustomOptionHolder.CrusaderCharges.GetFloat());
        public static int CrusaderRechargeTasksNumber => Mathf.RoundToInt(CustomOptionHolder.CrusaderRechargeTasksNumber.GetFloat());

        // Oracle
        public static float ConfessCooldown => CustomOptionHolder.ConfessCooldown.GetFloat();
        public static float OracleAccuracy => CustomOptionHolder.RevealAccuracy.GetFloat();
        public static bool NeutralBenignShowsEvil => CustomOptionHolder.NeutralBenignShowsEvil.GetBool();
        public static bool NeutralEvilShowsEvil => CustomOptionHolder.NeutralEvilShowsEvil.GetBool();
        public static int OracleCharges => Mathf.RoundToInt(CustomOptionHolder.OracleCharges.GetFloat());
        public static int OracleRechargeTasksNumber => Mathf.RoundToInt(CustomOptionHolder.OracleRechargeTasksNumber.GetFloat());

        // Snitch
        public static float SnitchCooldown => CustomOptionHolder.SnitchCooldown.GetFloat();
        public static float SnitchDuration => CustomOptionHolder.SnitchDuration.GetFloat();
        public static float SnitchAccuracy => CustomOptionHolder.SnitchAccuracy.GetFloat();
        public static bool SnitchSeesInMeetings => CustomOptionHolder.SnitchSeesInMeetings.GetBool();

        // Detective
        public static bool DetectiveAnonymousFootprints => CustomOptionHolder.detectiveAnonymousFootprints.GetBool();
        public static float DetectiveFootprintIntervall => CustomOptionHolder.detectiveFootprintIntervall.GetFloat();
        public static float DetectiveFootprintDuration => CustomOptionHolder.detectiveFootprintDuration.GetFloat();
        public static float DetectiveReportNameDuration => CustomOptionHolder.detectiveReportNameDuration.GetFloat();
        public static float DetectiveReportColorDuration => CustomOptionHolder.detectiveReportColorDuration.GetFloat();

        // Medic
        public static ShieldOptions MedicShowShielded => (ShieldOptions)CustomOptionHolder.medicShowShielded.GetSelection();
        public static bool MedicShowAttemptToShielded => CustomOptionHolder.medicShowAttemptToShielded.GetBool();
        public static ShieldTimingOptions MedicSetOrShowShieldAfterMeeting => (ShieldTimingOptions)CustomOptionHolder.medicSetOrShowShieldAfterMeeting.GetSelection();
        public static NotificationOptions MedicShowMurderAttempt => (NotificationOptions)CustomOptionHolder.MedicShowMurderAttempt.GetSelection();

        // Veteran
        public static float VeteranCooldown => CustomOptionHolder.VeteranCooldown.GetFloat();
        public static float VeteranDuration => CustomOptionHolder.VeteranDuration.GetFloat();
        public static int VeteranCharges => Mathf.RoundToInt(CustomOptionHolder.VeteranCharges.GetFloat());
        public static int VeteranRechargeTasksNumber => Mathf.RoundToInt(CustomOptionHolder.VeteranRechargeTasksNumber.GetFloat());

        // Landlord
        public static float LandlordCooldown => CustomOptionHolder.LandlordCooldown.GetFloat();
        public static int LandlordCharges => Mathf.RoundToInt(CustomOptionHolder.LandlordCharges.GetFloat());
        public static int LandlordRechargeTasksNumber => Mathf.RoundToInt(CustomOptionHolder.LandlordRechargeTasksNumber.GetFloat());

        // Mystic
        public static MysticModes MysticMode => (MysticModes)CustomOptionHolder.MysticMode.GetSelection();
        public static bool MysticLimitSoulDuration => CustomOptionHolder.MysticLimitSoulDuration.GetBool();
        public static float MysticSoulDuration => CustomOptionHolder.MysticSoulDuration.GetFloat();
        public static float MysticCooldown => CustomOptionHolder.MysticCooldown.GetFloat();
        public static int MysticCharges => Mathf.RoundToInt(CustomOptionHolder.MysticCharges.GetFloat());
        public static int MysticRechargeTasksNumber => Mathf.RoundToInt(CustomOptionHolder.MysticRechargeTasksNumber.GetFloat());

        // Hacker
        public static float HackerCooldown => CustomOptionHolder.hackerCooldown.GetFloat();
        public static float HackerDuration => CustomOptionHolder.hackerHackeringDuration.GetFloat();
        public static bool HackerOnlyColorType => CustomOptionHolder.hackerOnlyColorType.GetBool();
        public static int HackerToolsNumber => Mathf.RoundToInt(CustomOptionHolder.hackerToolsNumber.GetFloat());
        public static int HackerRechargeTasksNumber => Mathf.RoundToInt(CustomOptionHolder.hackerRechargeTasksNumber.GetFloat());

        // Tracker
        public static float TrackerUpdateIntervall => CustomOptionHolder.trackerUpdateIntervall.GetFloat();
        public static bool TrackerResetTargetAfterMeeting => CustomOptionHolder.trackerResetTargetAfterMeeting.GetBool();
        public static bool TrackerCanTrackCorpses => CustomOptionHolder.trackerCanTrackCorpses.GetBool();
        public static float TrackerCorpsesTrackingCooldown => CustomOptionHolder.trackerCorpsesTrackingCooldown.GetFloat();
        public static float TrackerCorpsesTrackingDuration => CustomOptionHolder.trackerCorpsesTrackingDuration.GetFloat();
        public static int TrackerTrackingMethod => CustomOptionHolder.trackerTrackingMethod.GetSelection();

        // Spy
        public static bool SpyCanDieToSheriff => CustomOptionHolder.spyCanDieToSheriff.GetBool();
        public static bool SpyImpostorsCanKillAnyone => CustomOptionHolder.spyImpostorsCanKillAnyone.GetBool();
        public static bool SpyCanEnterVents => CustomOptionHolder.spyCanEnterVents.GetBool();
        public static bool SpyHasImpostorVision => CustomOptionHolder.spyHasImpostorVision.GetBool();

        // Gatekeeper
        public static float GatekeeperCooldown => CustomOptionHolder.GatekeeperCooldown.GetFloat();
        public static float GatekeeperUsePortalCooldown => CustomOptionHolder.GatekeeperUsePortalCooldown.GetFloat();
        public static bool GatekeeperLogOnlyColorType => CustomOptionHolder.GatekeeperLogOnlyColorType.GetBool();
        public static bool GatekeeperLogHasTime => CustomOptionHolder.GatekeeperLogHasTime.GetBool();
        public static bool GatekeeperCanPortalFromAnywhere => CustomOptionHolder.GatekeeperCanPortalFromAnywhere.GetBool();

        // Vigilante
        public static float VigilanteCooldown => CustomOptionHolder.VigilanteCooldown.GetFloat();
        public static int VigilanteTotalScrews => Mathf.RoundToInt(CustomOptionHolder.VigilanteTotalScrews.GetFloat());
        public static int VigilanteCamPrice => Mathf.RoundToInt(CustomOptionHolder.VigilanteCamPrice.GetFloat());
        public static int VigilanteVentPrice => Mathf.RoundToInt(CustomOptionHolder.VigilanteVentPrice.GetFloat());
        public static float VigilanteCamDuration => CustomOptionHolder.VigilanteCamDuration.GetFloat();
        public static int VigilanteCamMaxCharges => Mathf.RoundToInt(CustomOptionHolder.VigilanteCamMaxCharges.GetFloat());
        public static int VigilanteCamRechargeTasksNumber => Mathf.RoundToInt(CustomOptionHolder.VigilanteCamRechargeTasksNumber.GetFloat());

        // Chronos
        public static float ChronosCooldown => CustomOptionHolder.ChronosCooldown.GetFloat();
        public static float ChronosRewindTime => CustomOptionHolder.ChronosRewindTime.GetFloat();
        public static int ChronosCharges => Mathf.RoundToInt(CustomOptionHolder.ChronosCharges.GetFloat());
        public static bool ChronosReviveDuringRewind => CustomOptionHolder.ChronosReviveDuringRewind.GetBool();

        // Psychic
        public static float PsychicCooldown => CustomOptionHolder.PsychicCooldown.GetFloat();
        public static float PsychicDuration => CustomOptionHolder.PsychicDuration.GetFloat();
        public static bool PsychicOneTimeUse => CustomOptionHolder.PsychicOneTimeUse.GetBool();
        public static float PsychicChanceAdditionalInfo => CustomOptionHolder.PsychicChanceAdditionalInfo.GetFloat();

        // Trapper
        public static float TrapperCooldown => CustomOptionHolder.trapperCooldown.GetFloat();
        public static int TrapperMaxCharges => Mathf.RoundToInt(CustomOptionHolder.trapperMaxCharges.GetFloat());
        public static int TrapperRechargeTasksNumber => Mathf.RoundToInt(CustomOptionHolder.trapperRechargeTasksNumber.GetFloat());
        public static int TrapperTrapNeededTriggerToReveal => Mathf.RoundToInt(CustomOptionHolder.trapperTrapNeededTriggerToReveal.GetFloat());
        public static bool TrapperAnonymousMap => CustomOptionHolder.trapperAnonymousMap.GetBool();
        public static int TrapperInfoType => CustomOptionHolder.trapperInfoType.GetSelection();
        public static float TrapperTrapDuration => CustomOptionHolder.trapperTrapDuration.GetFloat();

        // Modifier settings
        public static bool ModifiersAreHidden => CustomOptionHolder.modifiersAreHidden.GetBool();

        public static int ModifierLazyQuantity => CustomOptionHolder.modifierLazyQuantity.GetQuantity();

        public static int ModifierSleuthQuantity => CustomOptionHolder.ModifierSleuthQuantity.GetQuantity();

        public static int ModifierBaitQuantity => CustomOptionHolder.modifierBaitQuantity.GetQuantity();
        public static float ModifierBaitReportDelayMin => CustomOptionHolder.modifierBaitReportDelayMin.GetFloat();
        public static float ModifierBaitReportDelayMax => CustomOptionHolder.modifierBaitReportDelayMax.GetFloat();
        public static bool ModifierBaitShowKillFlash => CustomOptionHolder.modifierBaitShowKillFlash.GetBool();

        public static float ModifierLoverImpLoverRate => CustomOptionHolder.modifierLoverImpLoverRate.GetFloat();
        public static bool ModifierLoverBothDie => CustomOptionHolder.modifierLoverBothDie.GetBool();

        public static int ModifierBlindQuantity => CustomOptionHolder.modifierBlindQuantity.GetQuantity();
        public static int ModifierBlindVision => CustomOptionHolder.modifierBlindVision.GetSelection();

        public static float ModifierMiniSpeed => CustomOptionHolder.ModifierMiniSpeed.GetFloat();

        public static float ModifierGiantSpeed => CustomOptionHolder.ModifierGiantSpeed.GetFloat();

        public static int ModifierVipQuantity => CustomOptionHolder.modifierVipQuantity.GetQuantity();
        public static bool ModifierVipShowColor => CustomOptionHolder.modifierVipShowColor.GetBool();

        public static float ModifierDisperserCooldown => CustomOptionHolder.ModifierDisperserCooldown.GetFloat();
        public static int ModifierDisperserCharges => Mathf.RoundToInt(CustomOptionHolder.ModifierDisperserCharges.GetFloat());
        public static int ModifierDisperserKillCharges => Mathf.RoundToInt(CustomOptionHolder.ModifierDisperserKillCharges.GetFloat());

        public static int ModifierDrunkQuantity => CustomOptionHolder.modifierDrunkQuantity.GetQuantity();
        public static float ModifierDrunkDuration => CustomOptionHolder.modifierDrunkDuration.GetFloat();

        public static int ModifierChameleonQuantity => CustomOptionHolder.modifierChameleonQuantity.GetQuantity();
        public static float ModifierChameleonHoldDuration => CustomOptionHolder.modifierChameleonHoldDuration.GetFloat();
        public static float ModifierChameleonFadeDuration => CustomOptionHolder.modifierChameleonFadeDuration.GetFloat();
        public static int ModifierChameleonMinVisibility => CustomOptionHolder.modifierChameleonMinVisibility.GetSelection();

        // Guesser
        public static float GuesserCrewNumber => CustomOptionHolder.GuesserCrewNumber.GetFloat();
        public static float GuesserNeutralNumber => CustomOptionHolder.GuesserNeutralNumber.GetFloat();
        public static float GuesserImpNumber => CustomOptionHolder.GuesserImpNumber.GetFloat();
        public static bool RecruitIsAlwaysGuesser => CustomOptionHolder.RecruitIsAlwaysGuesser.GetBool();
        public static bool GuesserHaveModifier => CustomOptionHolder.GuesserHaveModifier.GetBool();
        public static int GuesserNumberOfShots => Mathf.RoundToInt(CustomOptionHolder.GuesserNumberOfShots.GetFloat());
        public static bool GuesserHasMultipleShotsPerMeeting => CustomOptionHolder.GuesserHasMultipleShotsPerMeeting.GetBool();
        public static int CrewGuesserNumberOfTasks => Mathf.RoundToInt(CustomOptionHolder.CrewGuesserNumberOfTasks.GetFloat());
        public static bool GuesserKillsThroughShield => CustomOptionHolder.GuesserKillsThroughShield.GetBool();
        public static bool GuesserEvilCanKillSpy => CustomOptionHolder.GuesserEvilCanKillSpy.GetBool();

        public static float AbilityFlashlightModeLightsOnVision => CustomOptionHolder.AbilityFlashlightModeLightsOnVision.GetFloat();
        public static float AbilityFlashlightModeLightsOffVision => CustomOptionHolder.AbilityFlashlightModeLightsOffVision.GetFloat();
        public static float AbilityFlashlightFlashlightWidth => CustomOptionHolder.AbilityFlashlightFlashlightWidth.GetFloat();

        // Other options
        public static int MaxNumberOfMeetings => Mathf.RoundToInt(CustomOptionHolder.maxNumberOfMeetings.GetFloat());
        public static bool DisableMedbayAnimation => CustomOptionHolder.DisableMedbayAnimation.GetBool();
        public static float GameStartCooldowns => CustomOptionHolder.GameStartCooldowns.GetFloat();
        public static bool LimitAbilities => CustomOptionHolder.LimitAbilities.GetBool();
        public static bool EveryoneCanStopStart => CustomOptionHolder.EveryoneCanStopStart.GetBool();
        public static SkipButtonOptions SkipButtonDisable => (SkipButtonOptions)CustomOptionHolder.SkipButtonDisable.GetSelection();
        public static bool NoVoteIsSelfVote => CustomOptionHolder.noVoteIsSelfVote.GetBool();
        public static bool HidePlayerNames => CustomOptionHolder.hidePlayerNames.GetBool();
        public static bool RandomSpawns => CustomOptionHolder.RandomSpawns.GetBool();
        public static bool AllowParallelMedBayScans => CustomOptionHolder.allowParallelMedBayScans.GetBool();
        public static bool ShieldFirstKill => CustomOptionHolder.ShieldFirstKill.GetBool();
        public static bool FinishTasksBeforeHauntingOrZoomingOut => CustomOptionHolder.finishTasksBeforeHauntingOrZoomingOut.GetBool();
        public static bool DeadImpsBlockSabotage => CustomOptionHolder.deadImpsBlockSabotage.GetBool();

        public static bool CamsNightVision => CustomOptionHolder.camsNightVision.GetBool();
        public static bool CamsNoNightVisionIfImpVision => CustomOptionHolder.camsNoNightVisionIfImpVision.GetBool();

        public static bool RoleBlockingSettings => CustomOptionHolder.RoleBlockingSettings.GetBool();
        public static bool BlockWarlockViper => CustomOptionHolder.BlockWarlockViper.GetBool();
        public static bool BlockScavengerJanitor => CustomOptionHolder.BlockScavengerJanitor.GetBool();
        public static bool BlockMorphlingPainter => CustomOptionHolder.BlockMorphlingPainter.GetBool();
        public static bool BlockMinerTrickster => CustomOptionHolder.BlockMinerTrickster.GetBool();

        public static bool EnableBetterPolus => CustomOptionHolder.EnableBetterPolus.GetBool();
        public static bool BPVentImprovements => CustomOptionHolder.BPVentImprovements.GetBool();
        public static bool BPVitalsLab => CustomOptionHolder.BPVitalsLab.GetBool();
        public static bool BPColdTempDeathValley => CustomOptionHolder.BPColdTempDeathValley.GetBool();
        public static bool BPWifiChartCourseSwap => CustomOptionHolder.BPWifiChartCourseSwap.GetBool();

        public static bool EnableRandomMaps => CustomOptionHolder.EnableRandomMaps.GetBool();
        public static float RandomMapEnableSkeld => CustomOptionHolder.RandomMapEnableSkeld.GetFloat();
        public static float RandomMapEnableMira => CustomOptionHolder.RandomMapEnableMira.GetFloat();
        public static float RandomMapEnablePolus => CustomOptionHolder.RandomMapEnablePolus.GetFloat();
        public static float RandomMapEnableAirShip => CustomOptionHolder.RandomMapEnableAirShip.GetFloat();
        public static float RandomMapEnableFungle => CustomOptionHolder.RandomMapEnableFungle.GetFloat();
        public static float RandomMapEnableSubmerged => CustomOptionHolder.RandomMapEnableSubmerged.GetFloat();
        public static bool RandomMapSeparateSettings => CustomOptionHolder.RandomMapSeparateSettings.GetBool();
    }
}
