using UnityEngine;
using System.Reflection;
using System.Collections.Generic;
using Hazel;
using System;
using System.Linq;
using Reactor.Utilities.Extensions;

namespace TheSushiRoles.Patches 
{
    public class GameStartManagerPatch  
    {
        public static Dictionary<int, PlayerVersion> playerVersions = new Dictionary<int, PlayerVersion>();
        public static float timer = 600f;
        private static float kickingTimer = 0f;
        private static bool versionSent = false;
        private static string lobbyCodeText = "";

        [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.OnPlayerJoined))]
        public class AmongUsClientOnPlayerJoinedPatch 
        {
            public static void Postfix(AmongUsClient __instance) 
            {
                if (PlayerControl.LocalPlayer != null) 
                {
                    Utils.ShareGameVersion();
                }
            }
        }

        [HarmonyPatch(typeof(GameStartManager), nameof(GameStartManager.Start))]
        public class GameStartManagerStartPatch {
            public static void Postfix(GameStartManager __instance) 
            {
                // Trigger version refresh
                versionSent = false;
                // Reset lobby countdown timer
                timer = 600f; 
                // Reset kicking timer
                kickingTimer = 0f;
                // Copy lobby code
                string code = InnerNet.GameCode.IntToGameName(AmongUsClient.Instance.GameId);
                GUIUtility.systemCopyBuffer = code;
                lobbyCodeText = FastDestroyableSingleton<TranslationController>.Instance.GetString(StringNames.RoomCode, new Il2CppReferenceArray<Il2CppSystem.Object>(0)) + "\r\n" + code;
            }
        }

        [HarmonyPatch(typeof(GameStartManager), nameof(GameStartManager.Update))]
        public class GameStartManagerUpdatePatch 
        {
            public static float startingTimer = 0;
            private static bool update = false;
            private static string currentText = "";
            private static GameObject copiedStartButton;

            public static void Prefix(GameStartManager __instance) 
            {
                if (!GameData.Instance ) return; // No instance
                __instance.MinPlayers = 1;
                update = GameData.Instance.PlayerCount != __instance.LastPlayerCount;
            }

            public static void Postfix(GameStartManager __instance) 
            {
                if (TheSushiRolesPlugin.DebuggerLoaded) return;

                // Send version as soon as PlayerControl.LocalPlayer exists
                if (PlayerControl.LocalPlayer != null && !versionSent) 
                {
                    versionSent = true;
                    Utils.ShareGameVersion();
                }


                // Check version handshake infos
                bool versionMismatch = false;
                string message = "";
                foreach (InnerNet.ClientData client in AmongUsClient.Instance.allClients.ToArray()) 
                {
                    if (client.Character == null) continue;
                    else if (!playerVersions.ContainsKey(client.Id))  
                    {
                        versionMismatch = true;
                        message += $"<color=#FF0000FF>{client.Character.Data.PlayerName} has a different version of The Sushi Roles or doesn't have The Sushi Roles installed\n</color>";
                    } 
                    else 
                    {
                        PlayerVersion PV = playerVersions[client.Id];
                        int diff = TheSushiRolesPlugin.Version.CompareTo(PV.version);
                        if (diff > 0) 
                        {
                            message += $"<color=#FF0000FF>{client.Character.Data.PlayerName} has an older version of The Sushi Roles (v{playerVersions[client.Id].version.ToString()})\n</color>";
                            versionMismatch = true;
                        } 
                        else if (diff < 0) 
                        {
                            message += $"<color=#FF0000FF>{client.Character.Data.PlayerName} has a newer version of The Sushi Roles (v{playerVersions[client.Id].version.ToString()})\n</color>";
                            versionMismatch = true;
                        } 
                        else if (!PV.GuidMatches()) 
                        { // version presumably matches, check if Guid matches
                            message += $"<color=#FF0000FF>{client.Character.Data.PlayerName} has a modified version of The Sushi Roles v{playerVersions[client.Id].version.ToString()} <size=30%>({PV.guid.ToString()})</size>\n</color>";
                            versionMismatch = true;
                        }
                    }
                }
                // Display message to the host
                if (AmongUsClient.Instance.AmHost) 
                {
                    if (versionMismatch) 
                    {
                        __instance.GameStartText.text = message;
                        __instance.GameStartText.transform.localPosition = __instance.StartButton.transform.localPosition + Vector3.up * 5;
                        __instance.GameStartText.transform.localScale = new Vector3(2f, 2f, 1f);
                        __instance.GameStartTextParent.SetActive(true);
                    } 
                    else 
                    {
                        __instance.GameStartText.transform.localPosition = Vector3.zero;
                        __instance.GameStartText.transform.localScale = new Vector3(1.2f, 1.2f, 1f);
                        if (!__instance.GameStartText.text.StartsWith("Starting")) 
                        {
                            __instance.GameStartText.text = String.Empty;
                            __instance.GameStartTextParent.SetActive(false);
                        }
                    }

                    if (__instance.startState != GameStartManager.StartingStates.Countdown)
                        copiedStartButton?.Destroy();
                    // Make starting info available to clients:
                    if (startingTimer <= 0 && __instance.startState == GameStartManager.StartingStates.Countdown) 
                    {
                        Utils.SendRPC(CustomRPC.SetGameStarting);
                        RPCProcedure.SetGameStarting();
                        // Activate Stop-Button
                        copiedStartButton = GameObject.Instantiate(__instance.StartButton.gameObject, __instance.StartButton.gameObject.transform.parent);
                        copiedStartButton.transform.localPosition = __instance.StartButton.transform.localPosition;
                        copiedStartButton.SetActive(true);
                        var startButtonText = copiedStartButton.GetComponentInChildren<TMPro.TextMeshPro>();
                        startButtonText.text = "";
                        startButtonText.fontSize *= 0.8f;
                        startButtonText.fontSizeMax = startButtonText.fontSize;
                        startButtonText.gameObject.transform.localPosition = Vector3.zero;
                        PassiveButton startButtonPassiveButton = copiedStartButton.GetComponent<PassiveButton>();
                        void StopStartFunc() 
                        {
                            __instance.ResetStartState();
                            Utils.SendRPC(CustomRPC.StopStart, PlayerControl.LocalPlayer.PlayerId);
                            copiedStartButton.Destroy();
                            startingTimer = 0;
                            SoundManager.Instance.StopSound(GameStartManager.Instance.gameStartSound);
                        }
                        startButtonPassiveButton.OnClick.AddListener((Action)(() => StopStartFunc()));
                        __instance.StartCoroutine(Effects.Lerp(.1f, new System.Action<float>((p) => 
                        {
                            startButtonText.text = "";
                        })));
                        

                    }
                }
                
                // Client update with handshake infos
                else {
                    if (!playerVersions.ContainsKey(AmongUsClient.Instance.HostId) || TheSushiRolesPlugin.Version.CompareTo(playerVersions[AmongUsClient.Instance.HostId].version) != 0) {
                        kickingTimer += Time.deltaTime;
                        if (kickingTimer > 10) {
                            kickingTimer = 0;
			                AmongUsClient.Instance.ExitGame(DisconnectReasons.ExitGame);
                            SceneChanger.ChangeScene("MainMenu");
                        }

                        __instance.GameStartText.text = $"<color=#FF0000FF>The host has no or a different version of The Sushi Roles\nYou will be kicked in {Math.Round(10 - kickingTimer)}s</color>";
                        __instance.GameStartText.transform.localPosition = __instance.StartButton.transform.localPosition + Vector3.up * 5;
                        __instance.GameStartText.transform.localScale = new Vector3(2f, 2f, 1f);
                        __instance.GameStartTextParent.SetActive(true);
                    } else if (versionMismatch) {
                        __instance.GameStartText.text = $"<color=#FF0000FF>Players With Different Versions:\n</color>" + message;
                        __instance.GameStartText.transform.localPosition = __instance.StartButton.transform.localPosition + Vector3.up * 5;
                        __instance.GameStartText.transform.localScale = new Vector3(2f, 2f, 1f);
                        __instance.GameStartTextParent.SetActive(true);
                    } else {
                        __instance.GameStartText.transform.localPosition = Vector3.zero;
                        __instance.GameStartText.transform.localScale = new Vector3(1.2f, 1.2f, 1f);
                        if (!__instance.GameStartText.text.StartsWith("Starting")) 
                        {
                            __instance.GameStartText.text = String.Empty;
                            __instance.GameStartTextParent.SetActive(false);
                        }
                    }

                    if (!__instance.GameStartText.text.StartsWith("Starting") || !CustomOptionHolder.EveryoneCanStopStart.GetBool())
                        copiedStartButton?.Destroy();
                    if (CustomOptionHolder.EveryoneCanStopStart.GetBool() && copiedStartButton == null && __instance.GameStartText.text.StartsWith("Starting") && !TheSushiRolesPlugin.DebuggerLoaded) 
                    {

                        // Activate Stop-Button
                        copiedStartButton = GameObject.Instantiate(__instance.StartButton.gameObject, __instance.StartButton.gameObject.transform.parent);
                        copiedStartButton.transform.localPosition = __instance.StartButton.transform.localPosition;
                        copiedStartButton.SetActive(true);
                        var startButtonText = copiedStartButton.GetComponentInChildren<TMPro.TextMeshPro>();
                        startButtonText.text = "";
                        startButtonText.fontSize *= 0.8f;
                        startButtonText.fontSizeMax = startButtonText.fontSize;
                        startButtonText.gameObject.transform.localPosition = Vector3.zero;
                        PassiveButton startButtonPassiveButton = copiedStartButton.GetComponent<PassiveButton>();

                        void StopStartFunc() 
                        {
                            Utils.SendRPC(CustomRPC.StopStart, PlayerControl.LocalPlayer.PlayerId);
                            copiedStartButton.Destroy();
                            __instance.GameStartText.text = String.Empty;
                            startingTimer = 0;
                            SoundManager.Instance.StopSound(GameStartManager.Instance.gameStartSound);
                        }
                        startButtonPassiveButton.OnClick.AddListener((Action)(() => StopStartFunc()));
                        __instance.StartCoroutine(Effects.Lerp(.1f, new System.Action<float>((p) => {
                            startButtonText.text = "";
                        })));
                    }
                }
                // Start Timer
                if (startingTimer > 0)
                {
                    startingTimer -= Time.deltaTime;
                }
                // Lobby timer
                if (!GameData.Instance || !__instance.PlayerCounter) return; // No instance

                if (update) currentText = __instance.PlayerCounter.text;

                timer = Mathf.Max(0f, timer -= Time.deltaTime);
                int minutes = (int)timer / 60;
                int seconds = (int)timer % 60;
                string suffix = $" ({minutes:00}:{seconds:00})";

                if (!AmongUsClient.Instance) return;
            }
        }

        [HarmonyPatch(typeof(GameStartManager), nameof(GameStartManager.BeginGame))]
        public class GameStartManagerBeginGame 
        {
            public static bool Prefix(GameStartManager __instance) 
            {
                // Block game start if not everyone has the same mod version
                bool continueStart = true;

                if (AmongUsClient.Instance.AmHost && !TheSushiRolesPlugin.DebuggerLoaded) 
                {
                    foreach (InnerNet.ClientData client in AmongUsClient.Instance.allClients.GetFastEnumerator()) {
                        if (client.Character == null) continue;
                        var dummyComponent = client.Character.GetComponent<DummyBehaviour>();
                        if (dummyComponent != null && dummyComponent.enabled)
                            continue;
                        
                        if (!playerVersions.ContainsKey(client.Id)) {
                            continueStart = false;
                            break;
                        }
                        
                        PlayerVersion PV = playerVersions[client.Id];
                        int diff = TheSushiRolesPlugin.Version.CompareTo(PV.version);
                        if (diff != 0 || !PV.GuidMatches()) {
                            continueStart = false;
                            break;
                        }
                    }
                    if (continueStart && GameOptionsManager.Instance.CurrentGameOptions.MapId != 6) 
                    {
                        byte mapId = 0;
                        if (mapId >= 3) mapId++;
                        Utils.SendRPC(CustomRPC.DynamicMapOption, mapId);
                        RPCProcedure.DynamicMapOption(mapId);
                    }            
                    else if (CustomOptionHolder.dynamicMap.GetBool() && continueStart) 
                    {
                        // 0 = Skeld
                        // 1 = Mira HQ
                        // 2 = Polus
                        // 3 = Dleks - deactivated
                        // 4 = Airship
                        // 5 = Submerged
                        byte chosenMapId = 0;
                        List<float> probabilities = new List<float>
                        {
                            CustomOptionHolder.dynamicMapEnableSkeld.GetSelection() / 10f,
                            CustomOptionHolder.dynamicMapEnableMira.GetSelection() / 10f,
                            CustomOptionHolder.dynamicMapEnablePolus.GetSelection() / 10f,
                            CustomOptionHolder.dynamicMapEnableAirShip.GetSelection() / 10f,
                            CustomOptionHolder.dynamicMapEnableFungle.GetSelection() / 10f,
                            CustomOptionHolder.dynamicMapEnableSubmerged.GetSelection() / 10f,
                        };

                        // if any map is at 100%, remove all maps that are not!
                        if (probabilities.Contains(1.0f)) 
                        {
                            for (int i=0; i < probabilities.Count; i++) 
                            {
                                if (probabilities[i] != 1.0) probabilities[i] = 0;
                            }
                        }

                        float sum = probabilities.Sum();
                        if (sum == 0) return continueStart;  // All maps set to 0, why are you doing this???
                        for (int i = 0; i < probabilities.Count; i++)
                        {  // Normalize to [0,1]
                            probabilities[i] /= sum;
                        }
                        float selection = (float)TheSushiRolesPlugin.rnd.NextDouble();
                        float cumsum = 0;
                        for (byte i = 0; i < probabilities.Count; i++)
                        {
                            cumsum += probabilities[i];
                            if (cumsum > selection)
                            {
                                chosenMapId = i;
                                break;
                            }
                        }

                        // Translate chosen map to presets page and use that maps random map preset page
                        if (CustomOptionHolder.dynamicMapSeparateSettings.GetBool())
                        {
                            CustomOptionHolder.presetSelection.UpdateSelection(chosenMapId + 2);
                        }
                        if (chosenMapId >= 3) chosenMapId++;  // Skip dlekS
                                                              
                        Utils.SendRPC(CustomRPC.DynamicMapOption, chosenMapId);
                        RPCProcedure.DynamicMapOption(chosenMapId);
                    }
                }
                return continueStart;
            }
        }

        public class PlayerVersion 
        {
            public readonly Version version;
            public readonly Guid guid;

            public PlayerVersion(Version version, Guid guid) 
            {
                this.version = version;
                this.guid = guid;
            }

            public bool GuidMatches() 
            {
                return Assembly.GetExecutingAssembly().ManifestModule.ModuleVersionId.Equals(this.guid);
            }
        }
    }
}
