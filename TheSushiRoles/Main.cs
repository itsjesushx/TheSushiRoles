global using Il2CppInterop.Runtime;
global using Il2CppInterop.Runtime.Attributes;
global using Il2CppInterop.Runtime.InteropTypes;
global using Il2CppInterop.Runtime.InteropTypes.Arrays;
global using Il2CppInterop.Runtime.Injection;

global using TheSushiRoles.Utilities;
global using TheSushiRoles.Patches;
global using TheSushiRoles.Objects;
global using static TheSushiRoles.TheSushiRoles;
global using TheSushiRoles.Roles;
global using TheSushiRoles.Roles.Modifiers;
global using TheSushiRoles.Roles.Abilities;
global using TheSushiRoles.Roles.AbilityInfo;
global using TheSushiRoles.Roles.ModifierInfo;
global using TheSushiRoles;
global using HarmonyLib;
global using AmongUs.GameOptions;

using BepInEx;
using BepInEx.Configuration;
using BepInEx.Unity.IL2CPP;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using TheSushiRoles.Modules;
using Il2CppSystem.Security.Cryptography;
using TheSushiRoles.Modules.Debugger.Components;
using Il2CppSystem.Text;
using Reactor.Networking.Attributes;
using AmongUs.Data;
using Reactor.Utilities;

namespace TheSushiRoles
{
    [BepInPlugin(Id, "The Sushi Roles", VersionString)]
    [BepInDependency(SubmergedCompatibility.SUBMERGED_GUID, BepInDependency.DependencyFlags.SoftDependency)]
    [BepInProcess("Among Us.exe")]
    [ReactorModFlags(Reactor.Networking.ModFlags.RequireOnAllClients)]
    
    public class TheSushiRolesPlugin : BasePlugin
    {
        public const string Id = "me.itsjesushx.thesushiroles";
        public const string VersionString = "1.0.0";
        public static Version Version = Version.Parse(VersionString);
        internal static BepInEx.Logging.ManualLogSource Logger;
        public static Color ModColor = new Color32(178, 254, 254, byte.MaxValue);
        public static TheSushiRolesPlugin Singleton { get; private set; } = null;         
        public Harmony Harmony { get; } = new Harmony(Id);
        public static string RobotName { get; set; } = "Bot";
        public static bool Persistence { get; set; } = true;
        public static TheSushiRolesPlugin Instance;

        public static int optionsPage = 2;

        public static ConfigEntry<string> DebugMode { get; private set; }
        public static ConfigEntry<bool> GhostsSeeInformation { get; set; }
        public static ConfigEntry<bool> GhostsSeeEverything { get; set; }
        public static ConfigEntry<bool> GhostsSeeVotes{ get; set; }
        public static ConfigEntry<bool> RoleSummaryVisible { get; set; }
        public static ConfigEntry<bool> ShowLighterDarker { get; set; }
        public static ConfigEntry<bool> DisableLobbyMusic { get; set; }
        public static ConfigEntry<bool> EnableSoundEffects { get; set; }
        public static ConfigEntry<bool> EnableHorseMode { get; set; }
        public static ConfigEntry<bool> ShowVentsOnMap { get; set; }
        public static ConfigEntry<bool> ShowChatNotifications { get; set; }
        public static ConfigEntry<string> Ip { get; set; }
        public static ConfigEntry<ushort> Port { get; set; }
        public static bool DebuggerLoaded => AmongUsClient.Instance.NetworkMode == NetworkModes.LocalGame;
        public static IRegionInfo[] defaultRegions;


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

            DebugMode = Config.Bind("Custom", "Enable Debug Mode", "false");
            GhostsSeeInformation = Config.Bind("Custom", "Ghosts See Remaining Tasks", true);
            GhostsSeeEverything = Config.Bind("Custom", "Ghosts See Roles & Modifiers", true);
            GhostsSeeVotes = Config.Bind("Custom", "Ghosts See Votes", true);
            DisableLobbyMusic = Config.Bind("Custom", "Disable Lobby Music", true);
            RoleSummaryVisible = Config.Bind("Custom", "Show Role Summary", true);
            ShowLighterDarker = Config.Bind("Custom", "Show Lighter / Darker", true);
            EnableSoundEffects = Config.Bind("Custom", "Enable Sound Effects", true);
            EnableHorseMode = Config.Bind("Custom", "Enable Horse Mode", false);
            ShowVentsOnMap = Config.Bind("Custom", "Show vent positions on minimap", false);
            ShowChatNotifications = Config.Bind("Custom", "Show Chat Notifications", true);

            Ip = Config.Bind("Custom", "Custom Server IP", "127.0.0.1");
            Port = Config.Bind("Custom", "Custom Server Port", (ushort)22023);
            defaultRegions = ServerManager.DefaultRegions;
            // Removes vanilla Servers
            ServerManager.DefaultRegions = new Il2CppReferenceArray<IRegionInfo>(new IRegionInfo[0]);
            UpdateRegions();

           //ReactorCredits.Register<TheSushiRolesPlugin>(ReactorCredits.AlwaysShow);

            DebugMode = Config.Bind("Custom", "Enable Debug Mode", "false");
            Harmony.PatchAll();
            
            CustomOptionHolder.Load();
            AddComponent<ModUpdater>();
            SubmergedCompatibility.Initialize();
            AddToKillDistanceSetting.AddKillDistance();
            Logger.LogInfo("Loading TSR completed!");
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
    
    // Debugging tools
    [HarmonyPatch(typeof(KeyboardJoystick), nameof(KeyboardJoystick.Update))]
    public static class DebugManager
    {
        private static readonly string passwordHash = "d1f51dfdfd8d38027fd2ca9dfeb299399b5bdee58e6c0b3b5e9a45cd4e502848";
        private static readonly System.Random random = new System.Random((int)DateTime.Now.Ticks);
        private static List<PlayerControl> bots = new List<PlayerControl>();

        public static void Postfix(KeyboardJoystick __instance)
        {
            // Check if debug mode is active.
            StringBuilder builder = new StringBuilder();
            SHA256 sha = SHA256Managed.Create();
            Byte[] hashed = sha.ComputeHash(Encoding.UTF8.GetBytes(TheSushiRolesPlugin.DebugMode.Value));
            foreach (var b in hashed) 
            {
                builder.Append(b.ToString("x2"));
            }
            string enteredHash = builder.ToString();
            if (enteredHash != passwordHash) return;

            // Terminate round
            if(Input.GetKeyDown(KeyCode.L)) 
            {
                Utils.SendRPC(CustomRPC.ForceEnd);
                RPCProcedure.ForceEnd();
            }
        }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
