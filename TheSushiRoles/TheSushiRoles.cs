using BepInEx;
using BepInEx.Configuration;
using BepInEx.Unity.IL2CPP;
using TheSushiRoles.Modules.Debugger.Components;
using Reactor.Networking.Attributes;
using AmongUs.Data;

namespace TheSushiRoles
{
    [BepInPlugin(Id, "The Sushi Roles", VersionString)]
    [BepInDependency(SubmergedCompatibility.SUBMERGED_GUID, BepInDependency.DependencyFlags.SoftDependency)]
    [BepInProcess("Among Us.exe")]
    [ReactorModFlags(Reactor.Networking.ModFlags.RequireOnAllClients)]
    [BepInIncompatibility("MalumMenu")]
    public class TheSushiRoles : BasePlugin
    {
        public const string Id = "me.itsjesushx.thesushiroles";
        public const string VersionString = "1.0.0";
        public const string DevString = " - Dev 9";
        public static Version Version = Version.Parse(VersionString);

        internal static BepInEx.Logging.ManualLogSource Logger;

        public static TheSushiRoles Singleton { get; private set; } = null;         
        public Harmony Harmony { get; } = new Harmony(Id);
        public static TheSushiRoles Instance;
        public static SRandom rnd = new SRandom((int)DateTime.Now.Ticks);
        public static DateTime startTime = DateTime.UtcNow;
        
        public static int optionsPage = 2;

        public static ConfigEntry<bool> GhostsSeeInformation { get; set; }
        public static ConfigEntry<bool> GhostsSeeEverything { get; set; }
        public static ConfigEntry<bool> GhostsSeeVotes{ get; set; }
        public static ConfigEntry<bool> ShowLighterDarker { get; set; }
        public static ConfigEntry<bool> DisableLobbyMusic { get; set; }
        public static ConfigEntry<bool> EnableSoundEffects { get; set; }
        public static ConfigEntry<bool> ShowVentsOnMap { get; set; }
        public static ConfigEntry<bool> ShowChatNotifications { get; set; }

        public static IRegionInfo[] defaultRegions;

        public static bool DebuggerLoaded => AmongUsClient.Instance.NetworkMode == NetworkModes.LocalGame;
        public static string RobotName { get; set; } = "Bot";
        public static bool Persistence { get; set; } = true;


        // This is part of the Mini.RegionInstaller, Licensed under GPLv3
        // file="RegionInstallPlugin.cs" company="miniduikboot">
        public static void UpdateRegions()
        {
            ServerManager serverManager = ServerManager.Instance;
            IRegionInfo[] regions = new IRegionInfo[]
            {
                new StaticHttpRegionInfo("Modded NA (MNA)", StringNames.NoTranslation,"www.aumods.org", new Il2CppReferenceArray<ServerInfo>(new ServerInfo[1] { new ServerInfo("Http-1", "https://www.aumods.org",  443, false) })).CastFast<IRegionInfo>(),
                new StaticHttpRegionInfo("Modded EU (MEU)", StringNames.NoTranslation,"au-eu.duikbo.at", new Il2CppReferenceArray<ServerInfo>(new ServerInfo[1] { new ServerInfo("Http-1", "https://au-eu.duikbo.at",  443, false) })).CastFast<IRegionInfo>(),
                new StaticHttpRegionInfo("Modded Asia (MAS)", StringNames.NoTranslation,"au-as.duikbo.at", new Il2CppReferenceArray<ServerInfo>(new ServerInfo[1] { new ServerInfo("Http-1", "https://au-as.duikbo.at",  443, false) })).CastFast<IRegionInfo>(),
                //new StaticHttpRegionInfo("Custom", StringNames.NoTranslation, Ip.Value, new Il2CppReferenceArray<ServerInfo>(new ServerInfo[1] { new ServerInfo("Custom", Ip.Value, Port.Value, false) })).CastFast<IRegionInfo>()
            };

            IRegionInfo currentRegion = serverManager.CurrentRegion;

            Logger.LogDebug($"Adding {regions.Length} regions");
            foreach (IRegionInfo region in regions)
            {
                if (currentRegion != null && region.Name.Equals(currentRegion.Name, StringComparison.OrdinalIgnoreCase))
                    currentRegion = region;
                serverManager.AddOrUpdateRegion(region);
            }

            // AU remembers the previous region that was set, so we need to restore it
            if (currentRegion != null)
            {
                Logger.LogDebug("Resetting previous region");
                serverManager.SetRegion(currentRegion);
            }
        }
        public static bool NoEndGame { get; set; } = false;
        public static Debugger Debugger { get; set; } = null;
        public override void Load() 
        {
            if (Singleton != null) return;
            Logger = Log;
            Instance = this;
            CustomColors.Load();

            Singleton = this;

            ClassInjector.RegisterTypeInIl2Cpp<Debugger>();
            ClassInjector.RegisterTypeInIl2Cpp<Component>();
            AddComponent<Modules.Debugger.Embedded.ReactorCoroutines.Coroutines.Component>();
            Debugger = this.AddComponent<Debugger>();

            GhostsSeeInformation = Config.Bind("Custom", "Ghosts See Remaining Tasks", true);
            GhostsSeeEverything = Config.Bind("Custom", "Ghosts See Roles, Modifiers & Abilities", true);
            GhostsSeeVotes = Config.Bind("Custom", "Ghosts See Votes", true);
            DisableLobbyMusic = Config.Bind("Custom", "Disable Lobby Music", true);
            ShowLighterDarker = Config.Bind("Custom", "Show Lighter / Darker", true);
            EnableSoundEffects = Config.Bind("Custom", "Enable Sound Effects", true);
            ShowVentsOnMap = Config.Bind("Custom", "Show vent positions on minimap", false);
            ShowChatNotifications = Config.Bind("Custom", "Show Chat Notifications", true);

            defaultRegions = ServerManager.DefaultRegions;
            // Removes vanilla Servers
            ServerManager.DefaultRegions = new Il2CppReferenceArray<IRegionInfo>(new IRegionInfo[0]);
            UpdateRegions();

            ReactorCredits.Register($"TheSushiRoles v{VersionString}{DevString}", "", true, ReactorCredits.AlwaysShow);
            Harmony.PatchAll();
            
            CustomOptionHolder.Load();
            AddComponent<ModUpdater>();
            SubmergedCompatibility.Initialize();
            Logger.LogInfo("Successfully loaded The Sushi Roles!");
        }
    }

    [HarmonyPatch(typeof(ChatController), nameof(ChatController.Awake))]
    public static class ChatControllerAwakePatch 
    {
        private static void Prefix() 
        {
            if (!EOSManager.Instance.isKWSMinor) 
            {
                DataManager.Settings.Multiplayer.ChatMode = InnerNet.QuickChatModes.FreeChatOrQuickChat;
            }
        }
    }
}