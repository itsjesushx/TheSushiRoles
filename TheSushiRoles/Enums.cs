namespace TheSushiRoles
{
    public enum MurderAttemptResult 
    {
        PerformKill,
        SuppressKill,
        BlankKill,
        DelayViperKill,
        MirrorKill,
    }
    public enum CustomOptionType
    {
        General,
        Impostor,
        Neutral,
        Crewmate,
        Modifier,
        Ability,
        Map
    }
    public enum MysticModes
    {
        DeathAndSouls = 0,
        DeathFlash = 1,
        Souls = 2
    }
    public enum ShieldOptions
    {
        ShieldedAndMedic = 0,
        Medic = 1,
        Shielded = 2
    }
    public enum NotificationOptions
    {
        Medic = 0,
        Shielded = 1,
        Nobody = 2
    }
    public enum SpecialPsychicInfo
    {
        SheriffSuicide,
        ActiveLoverDies,
        PassiveLoverSuicide,
        LawyerKilledByClient,
        RomanticKilledByBeloved,
        ImpostorTeamkill,
        SubmergedO2,
        WarlockSuicide,
        BodyCleaned,
    }
    public enum SingleVotesOptions
    {
        Off = 1,
        BeforeVoting = 2,
        UntilMeetingEnds = 3,
    }
    public enum AbilityId
    {
        Coward,
        Paranoid,
        Lighter,
    }
    public enum ModifierId
    {
        Lover,
        Disperser,
        Bait,
        Lazy,
        Sleuth,
        Tiebreaker,
        Giant,
        Blind,
        Mini,
        Vip,
        Drunk,
        Chameleon,
        Lucky
    }
    public enum ShieldTimingOptions
    {
        Instantly = 0,
        InstantlyVisibleAfterMeeting = 1,
        AfterMeeting = 2
    }
    public enum RoleAlignment
    {
        CrewPower,
        CrewSupport,
        CrewSpecial,
        CrewProtect,
        CrewInvest,
        NeutralBenign,
        NeutralEvil,
        NeutralKilling,
        ImpPower,
        ImpSpecial,
        ImpConcealing,
        ImpSupport,
        None
    }
    public enum RoleId 
    {
        Jester,
        Mayor,
        Gatekeeper,
        Grenadier,
        Landlord,
        Oracle,
        Miner,
        Engineer,
        Sheriff,
        Deputy,
        Juggernaut,
        Glitch,
        Agent,
        Snitch,
        Hitman,
        Veteran,
        Detective,
        Predator,
        Chronos,
        Medic,
        Romantic,
        Mystic,
        Morphling,
        Painter,
        Hacker,
        Crusader,
        Tracker,
        Viper,
        Spy,
        VengefulRomantic,
        Trickster,
        Janitor,
        Monarch,
        Blackmailer,
        Warlock,
        Werewolf,
        Vigilante,
        Arsonist,
        BountyHunter,
        Wraith,
        Scavenger,
        Psychic,
        Pestilence,
        Plaguebearer,
        Trapper,
        Undertaker,
        Lawyer,
        Prosecutor,
        Survivor,
        Witch,
        Assassin,
        Yoyo,
        Amnesiac,
        Crewmate,
        Impostor,
    }
    public enum SkipButtonOptions
    {
        No,
        Emergency,
        Always
    }
    public enum CustomRPC
    {
        // Main Controls

        ResetVaribles = 100,
        ShareOptions,
        WorkaroundSetRoles,
        SetRole,
        SetModifier,
        SetAbility,
        VersionHandshake,
        UseUncheckedVent,
        SetPostRoles,
        UncheckedCmdReportDeadBody,
        SetUnteleportable,
        RemoveAllBodies,
        UncheckedExilePlayer,
        RandomMapOption,
        SetGameStarting,
        StopStart,

        // Role functionality

        EngineerFixLights = 120,
        EngineerFixSubmergedOxygen,
        DisableVanillaRoles,
        EngineerUsedRepair,
        CleanBody,
        HostSuicide,
        MedicSetShielded,
        WerewolfMaul,
        StopFortifiedInteraction,
        BecomeCrewmate,
        BecomeImpostor,
        TurnPestilence,
        ShieldedMurderAttempt,
        GlitchMimic,
        DeputyShoot,
        SetJesterWinner,
        HitmanMorph,
        HitmanDragBody,
        HitmanDropBody,
        PestilenceKill,
        BypassMultiKill,
        BypassKill,
        DropBody,
        PlaguebearerInfect,
        DragBody,
        Disperse,
        VeteranAlert,
        AmnesiacRemember,
        Fortify,
        Confess,
        VeteranAlertKill,
        ChronosRewindTime,
        FortifiedMurderAttempt,
        MorphlingMorph,
        PainterPaint,
        GrenadierFlash,
        TrackerUsedTracker,
        ViperSetPoisoned,
        GlitchUsedHacks,
        SetFutureShielded,
        SetFutureSpelled,
        PlaceAssassinTrace,
        PlacePortal,
        PlaceMine,
        UsePortal,
        PlaceJackInTheBox,
        LightsOut,
        PlaceCamera,
        SealVent,
        ArsonistWin,
        GuesserShoot,
        RomanticSetBeloved,
        AgentTurnIntoHitman,
        LawyerSetTarget,
        LawyerChangeRole,
        ProsecutorSetTarget,
        LandlordTeleport,
        MonarchKnight,
        RomanticChangeRole,
        ProsecutorChangeRole,
        SetBlanked,
        SetFirstKill,
        Drunk,
        SetTiebreak,
        SetInvisible,
        SetVanish,
        BlackmailerBlackmail,
        RemoveBlackmail,
        RemoveMedicShield,
        SetTrap,
        SetBlindTrap,
        TriggerTrap,
        TriggerBlindTrap,
        MayorSetVoteTwice,
        YoyoMarkLocation,
        YoyoBlink,
        LuckyBecomeUnlucky,
        SetGuesser,

        // Other functionality
        ShareTimer,
        ShareGhostInfo,
        CheckMurder,
    }
    public enum ProsecutorOnTargetDeath
    {
        Jester,
        Amnesiac,
        Survivor
    }
    public enum Faction
    {
        Crewmates,
        Neutrals,
        Impostors,
        Other
    }
    public enum GhostInfoTypes 
    {
        HackNoticed,
        HackOver,
        ArsonistDouse,
        BountyTarget,
        SnitchInfo,
        AssassinMarked,
        OracleInfo,
        PlaguebearerInfect,
        WarlockTarget,
        MysticInfo,
        PsychicInfo,
        BlankUsed,
        DetectiveOrMedicInfo,
        ViperTimer,
        DeathReasonAndKiller,
    }
    enum CustomGameOverReason
    {
        LoversWin = 10,
        MiniLose = 12,
        JesterWin = 13,
        ArsonistWin = 14,
        ScavengerWin = 15,
        ProsecutorWin = 16,
        GlitchWin = 17,
        WerewolfWin = 18,
        PredatorWin = 19,
        VRomanticWin = 20,
        JuggernautWin = 21,
        PestilenceWin = 22,
        PlaguebearerWin = 23,
        AgentWin = 24,
        HitmanWin = 25,
        HostEndedGame = 26,
    }

    enum WinCondition
    {
        Default,
        LoversTeamWin,
        LoversSoloWin,
        JesterWin,        MiniLose,
        ArsonistWin,
        ScavengerWin,
        AdditionalLawyerBonusWin,
        AdditionalRomanticBonusWin,
        AdditionalAliveSurvivorWin,
        AdditionalBelovedBonusWin,
        AdditionalLoversPartnerWin,
        ProsecutorWin,
        GlitchWin,
        WerewolfWin,
        PredatorWin,
        VRomanticWin,
        JuggernautWin,
        PestilenceWin,
        PlaguebearerWin,
        AgentWin,
        HitmanWin,
        HostEndedGame
    }
}