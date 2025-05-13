
using System;
using Hazel;
using UnityEngine;
using System.Linq;
using static TheSushiRoles.GameHistory;
using System.Collections.Generic;
using Reactor.Utilities.Extensions;

namespace TheSushiRoles.Patches 
{

    [HarmonyPatch(typeof(Vent), nameof(Vent.CanUse))]
    public static class VentCanUsePatch
    {
        public static bool Prefix(Vent __instance, ref float __result, [HarmonyArgument(0)] NetworkedPlayerInfo pc, [HarmonyArgument(1)] ref bool canUse, [HarmonyArgument(2)] ref bool couldUse) 
        {
            if (GameOptionsManager.Instance.currentGameOptions.GameMode == GameModes.HideNSeek) return true;
            float num = float.MaxValue;
            PlayerControl @object = pc.Object;

            bool roleCouldUse = @object.IsVenter();

            if (__instance.name.StartsWith("SealedVent_")) 
            {
                canUse = couldUse = false;
                __result = num;
                return false;
            }

            // Submerged Compatability if needed:
            if (SubmergedCompatibility.IsSubmerged) 
            {
                // as submerged does, only change stuff for vents 9 and 14 of submerged. Code partially provided by AlexejheroYTB
                if (SubmergedCompatibility.GetInTransition()) 
                {
                    __result = float.MaxValue;
                    return canUse = couldUse = false;
                }                
                switch (__instance.Id) {
                    case 9:  // Cannot enter vent 9 (Engine Room Exit Only Vent)!
                        if (PlayerControl.LocalPlayer.inVent) break;
                        __result = float.MaxValue;
                        return canUse = couldUse = false;                    
                    case 14: // Lower Central
                        __result = float.MaxValue;
                        couldUse = roleCouldUse && !pc.IsDead && (@object.CanMove || @object.inVent);
                        canUse = couldUse;
                        if (canUse) {
                            Vector3 center = @object.Collider.bounds.center;
                            Vector3 position = __instance.transform.position;
                            __result = Vector2.Distance(center, position);
                            canUse &= __result <= __instance.UsableDistance;
                        }
                        return false;
                }
            }

            var usableDistance = __instance.UsableDistance;
            if (__instance.name.StartsWith("JackInTheBoxVent_")) {
                if(Trickster.Player != PlayerControl.LocalPlayer) {
                    // Only the Trickster can use the Jack-In-The-Boxes!
                    canUse = false;
                    couldUse = false;
                    __result = num;
                    return false; 
                } else {
                    // Reduce the usable distance to reduce the risk of gettings stuck while trying to jump into the box if it's placed near objects
                    usableDistance = 0.4f; 
                }
            }

            couldUse = (@object.inVent || roleCouldUse) && !pc.IsDead && (@object.CanMove || @object.inVent);
            canUse = couldUse;
            if (canUse)
            {
                Vector3 center = @object.Collider.bounds.center;
                Vector3 position = __instance.transform.position;
                num = Vector2.Distance(center, position);
                canUse &= (num <= usableDistance && (!PhysicsHelpers.AnythingBetween(@object.Collider, center, position, Constants.ShipOnlyMask, false) || __instance.name.StartsWith("JackInTheBoxVent_")));
            }
            __result = num;
            return false;
        }
    }

    [HarmonyPatch(typeof(VentButton), nameof(VentButton.DoClick))]
    class VentButtonDoClickPatch 
    {
        static  bool Prefix(VentButton __instance) 
        {
            // Manually modifying the VentButton to use Vent.Use again in order to trigger the Vent.Use prefix patch
		    if (__instance.currentTarget != null && !Glitch.HackedKnows.ContainsKey(PlayerControl.LocalPlayer.PlayerId)) __instance.currentTarget.Use();
            return false;
        }
    }

    [HarmonyPatch(typeof(Vent), nameof(Vent.Use))]
    public static class VentUsePatch 
    {
        public static bool Prefix(Vent __instance)
        {
            if (GameOptionsManager.Instance.currentGameOptions.GameMode == GameModes.HideNSeek) return true;
            // Glitch Hack disables the vents
            if (Glitch.HackedPlayers.Contains(PlayerControl.LocalPlayer.PlayerId)) 
            {
                Glitch.SetHackedKnows();
                return false;
            }
            if (Trapper.playersOnMap.Contains(PlayerControl.LocalPlayer.PlayerId)) return false;

            bool canUse;
            bool couldUse;
            __instance.CanUse(PlayerControl.LocalPlayer.Data, out canUse, out couldUse);
            
            bool JesterCanMove = PlayerControl.LocalPlayer == Jester.Player && Jester.CanMoveInVents;
            bool canMoveInVents = PlayerControl.LocalPlayer != Spy.Player && !JesterCanMove && !Trapper.playersOnMap.Contains(PlayerControl.LocalPlayer.PlayerId);
            
            if (!canUse) return false; // No need to execute the native method as using is disallowed anyways

            bool isEnter = !PlayerControl.LocalPlayer.inVent;
            
            if (__instance.name.StartsWith("JackInTheBoxVent_")) 
            {
                __instance.SetButtons(isEnter && canMoveInVents);
                MessageWriter writer = AmongUsClient.Instance.StartRpc(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.UseUncheckedVent, Hazel.SendOption.Reliable);
                writer.WritePacked(__instance.Id);
                writer.Write(PlayerControl.LocalPlayer.PlayerId);
                writer.Write(isEnter ? byte.MaxValue : (byte)0);
                writer.EndMessage();
                RPCProcedure.UseUncheckedVent(__instance.Id, PlayerControl.LocalPlayer.PlayerId, isEnter ? byte.MaxValue : (byte)0);
                SoundEffectsManager.Play("tricksterUseBoxVent");
                return false;
            }

            if(isEnter) {
                PlayerControl.LocalPlayer.MyPhysics.RpcEnterVent(__instance.Id);
            } else {
                PlayerControl.LocalPlayer.MyPhysics.RpcExitVent(__instance.Id);
            }
            __instance.SetButtons(isEnter && canMoveInVents);
            return false;
        }
    }

    [HarmonyPatch(typeof(Vent), nameof(Vent.TryMoveToVent))]
    public static class MoveToVentPatch 
    {
        public static bool Prefix(Vent otherVent) 
        {
            return !Trapper.playersOnMap.Contains(PlayerControl.LocalPlayer.PlayerId);
        }
    }

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.FixedUpdate))]
    class VentButtonVisibilityPatch 
    {
        static void Postfix(PlayerControl __instance) 
        {
            if (__instance.AmOwner && __instance.IsVenter() && FastDestroyableSingleton<HudManager>.Instance.ReportButton.isActiveAndEnabled) 
            {
                FastDestroyableSingleton<HudManager>.Instance.ImpostorVentButton.Show();
            }
        }
    }

    [HarmonyPatch(typeof(VentButton), nameof(VentButton.SetTarget))]
    class VentButtonSetTargetPatch 
    {
        static Sprite defaultVentSprite = null;
        static void Postfix(VentButton __instance) 
        {
            // Trickster render special vent button
            if (Trickster.Player != null && Trickster.Player == PlayerControl.LocalPlayer) 
            {
                if (defaultVentSprite == null) defaultVentSprite = __instance.graphic.sprite;
                bool isSpecialVent = __instance.currentTarget != null && __instance.currentTarget.gameObject != null && __instance.currentTarget.gameObject.name.StartsWith("JackInTheBoxVent_");
                __instance.graphic.sprite = isSpecialVent ?  Trickster.GetTricksterVentButtonSprite() : defaultVentSprite;
                __instance.buttonLabelText.enabled = !isSpecialVent;
            }
        }
    }

    [HarmonyPatch(typeof(KillButton), nameof(KillButton.DoClick))]
    class KillButtonDoClickPatch {
        public static bool Prefix(KillButton __instance) 
        {
            if (__instance.isActiveAndEnabled && __instance.currentTarget && !__instance.isCoolingDown && !PlayerControl.LocalPlayer.Data.IsDead && PlayerControl.LocalPlayer.CanMove) 
            {
                // Glitch Hack update.
                if (Glitch.HackedPlayers.Contains(PlayerControl.LocalPlayer.PlayerId)) {
                    Glitch.SetHackedKnows();
                    return false;
                }
                
                // Use an unchecked kill command, to allow shorter kill Cooldowns etc. without getting kicked
                MurderAttemptResult res = Utils.CheckMurderAttemptAndKill(PlayerControl.LocalPlayer, __instance.currentTarget);
                // Handle blank kill
                if (res == MurderAttemptResult.BlankKill) 
                {
                    PlayerControl.LocalPlayer.killTimer = GameOptionsManager.Instance.currentNormalGameOptions.KillCooldown;
                    if (PlayerControl.LocalPlayer == Cleaner.Player)
                        Cleaner.Player.killTimer = HudManagerStartPatch.cleanerCleanButton.Timer = HudManagerStartPatch.cleanerCleanButton.MaxTimer;
                    else if (PlayerControl.LocalPlayer == Warlock.Player)
                        Warlock.Player.killTimer = HudManagerStartPatch.warlockCurseButton.Timer = HudManagerStartPatch.warlockCurseButton.MaxTimer;
                    else if (PlayerControl.LocalPlayer == Mini.Player && Mini.Player.Data.Role.IsImpostor)
                        Mini.Player.SetKillTimer(GameOptionsManager.Instance.currentNormalGameOptions.KillCooldown * (Mini.IsGrownUp ? 0.66f : 2f));
                    else if (PlayerControl.LocalPlayer == Witch.Player)
                        Witch.Player.killTimer = HudManagerStartPatch.witchSpellButton.Timer = HudManagerStartPatch.witchSpellButton.MaxTimer;
                    else if (PlayerControl.LocalPlayer == Ninja.Player)
                        Ninja.Player.killTimer = HudManagerStartPatch.ninjaButton.Timer = HudManagerStartPatch.ninjaButton.MaxTimer;
                }
                __instance.SetTarget(null);
            }
            return false;
        }
    }

    [HarmonyPatch(typeof(ReportButton), nameof(ReportButton.DoClick))]
    class ReportButtonDoClickPatch 
    {
        public static bool Prefix(ReportButton __instance) 
        {
            if (__instance.isActiveAndEnabled && Glitch.HackedPlayers.Contains(PlayerControl.LocalPlayer.PlayerId) && __instance.graphic.color == Palette.EnabledColor) Glitch.SetHackedKnows();
            return !Glitch.HackedKnows.ContainsKey(PlayerControl.LocalPlayer.PlayerId);
        }
    }

    [HarmonyPatch(typeof(EmergencyMinigame), nameof(EmergencyMinigame.Update))]
    class EmergencyMiniGameHackedPlayersUpdate
    {
        public static bool Prefix(EmergencyMinigame __instance)
        {
            if (Glitch.HackedPlayers.Contains(PlayerControl.LocalPlayer.PlayerId))
            {
                Glitch.SetHackedKnows();
                return false; // Prevent the emergency minigame from updating
            }
            return true; // Allow the emergency minigame to update
        }
    }

    [HarmonyPatch(typeof(EmergencyMinigame), nameof(EmergencyMinigame.Update))]
    class EmergencyMinigameUpdatePatch 
    {
        static void Postfix(EmergencyMinigame __instance) 
        {
            var CanCallEmergency = true;
            var statusText = "";

            if (Glitch.HackedKnows.ContainsKey(PlayerControl.LocalPlayer.PlayerId) && Glitch.HackedKnows[PlayerControl.LocalPlayer.PlayerId] > 0f)
            {
                CanCallEmergency = false;
                statusText = "You are hacked. You can't start a meeting.";
            }

            // Deactivate emergency button for Swapper
            if (Swapper.Player != null && Swapper.Player == PlayerControl.LocalPlayer && !Swapper.canCallEmergency) 
            {
                CanCallEmergency = false;
                statusText = "The Swapper can't start an emergency meeting";
            }
            // Potentially deactivate emergency button for Jester
            if (Jester.Player != null && Jester.Player == PlayerControl.LocalPlayer && !Jester.canCallEmergency) 
            {
                CanCallEmergency = false;
                statusText = "The Jester can't start an emergency meeting";
            }
            // Potentially deactivate emergency button for Lawyer
            if (Lawyer.Player != null && Lawyer.Player == PlayerControl.LocalPlayer && !Lawyer.canCallEmergency) 
            {
                CanCallEmergency = false;
                statusText = "The Lawyer can't start an emergency meeting";
            }

             // Potentially deactivate emergency button for Prosecutor
            if (Prosecutor.Player != null && Prosecutor.Player == PlayerControl.LocalPlayer && !Prosecutor.canCallEmergency) 
            {
                CanCallEmergency = false;
                statusText = "The Prosecutor can't start an emergency meeting";
            }

            // Potentially deactivate emergency button if 2 players are left alive
            if (MapOptions.LimitAbilities && Utils.TwoPlayersAlive() && Tiebreaker.Player != null && Tiebreaker.Player.IsCrew()) 
            {
                CanCallEmergency = false;
                statusText = "Two Players Alive Only. Impossible to start a meeting!";
            }
            else 
            {
                CanCallEmergency = true;
            }

            if (!CanCallEmergency) 
            {
                __instance.StatusText.text = statusText;
                __instance.NumberText.text = string.Empty;
                __instance.ClosedLid.gameObject.SetActive(true);
                __instance.OpenLid.gameObject.SetActive(false);
                __instance.ButtonActive = false;
                return;
            }

            // Handle max number of meetings
            if (__instance.state == 1) 
            {
                int localRemaining = PlayerControl.LocalPlayer.RemainingEmergencies;
                int teamRemaining = Mathf.Max(0, MapOptions.maxNumberOfMeetings - MapOptions.meetingsCount);
                int remaining = Mathf.Min(localRemaining, (Mayor.Player != null && Mayor.Player == PlayerControl.LocalPlayer) ? 1 : teamRemaining);
                __instance.NumberText.text = $"{localRemaining.ToString()} and the crew has {teamRemaining.ToString()}";
                __instance.ButtonActive = remaining > 0;
                __instance.ClosedLid.gameObject.SetActive(!__instance.ButtonActive);
                __instance.OpenLid.gameObject.SetActive(__instance.ButtonActive);
				return;
			}
        }
    }


    [HarmonyPatch(typeof(Console), nameof(Console.CanUse))]
    public static class ConsoleCanUsePatch 
    {
        public static bool Prefix(ref float __result, Console __instance, [HarmonyArgument(0)] NetworkedPlayerInfo pc, [HarmonyArgument(1)] out bool canUse, [HarmonyArgument(2)] out bool couldUse) 
        {
            canUse = couldUse = false;
            if (Swapper.Player != null && Swapper.Player == PlayerControl.LocalPlayer)
                return !__instance.TaskTypes.Any(x => x == TaskTypes.FixLights || x == TaskTypes.FixComms);
            if (__instance.AllowImpostor) return true;
            if (!Utils.HasFakeTasks(pc.Object)) return true;
            __result = float.MaxValue;
            return false;
        }
    }

    [HarmonyPatch(typeof(TuneRadioMinigame), nameof(TuneRadioMinigame.Begin))]
    class CommsMinigameBeginPatch 
    {
        static void Postfix(TuneRadioMinigame __instance) 
        {
            // Block Swapper from fixing comms. Still looking for a better way to do this, but deleting the task doesn't seem like a viable option since then the camera, admin table, ... work while comms are out
            if (Swapper.Player != null && Swapper.Player == PlayerControl.LocalPlayer) 
            {
                __instance.Close();
            }
        }
    }

    [HarmonyPatch(typeof(SwitchMinigame), nameof(SwitchMinigame.Begin))]
    class LightsMinigameBeginPatch 
    {
        static void Postfix(SwitchMinigame __instance) 
        {
            // Block Swapper from fixing lights. One could also just delete the PlayerTask, but I wanted to do it the same way as with coms for now.
            if (Swapper.Player != null && Swapper.Player == PlayerControl.LocalPlayer) 
            {
                __instance.Close();
            }
        }
    }

    [HarmonyPatch]
    class VitalsMinigamePatch 
    {
        private static List<TMPro.TextMeshPro> hackerTexts = new List<TMPro.TextMeshPro>();

        [HarmonyPatch(typeof(VitalsMinigame), nameof(VitalsMinigame.Begin))]
        class VitalsMinigameStartPatch 
        {
            static void Postfix(VitalsMinigame __instance) 
            {
                if (Hacker.Player != null && PlayerControl.LocalPlayer == Hacker.Player) 
                {
                    hackerTexts = new List<TMPro.TextMeshPro>();
                    foreach (VitalsPanel panel in __instance.vitals) {
                        TMPro.TextMeshPro text = UnityEngine.Object.Instantiate(__instance.SabText, panel.transform);
                        hackerTexts.Add(text);
                        UnityEngine.Object.DestroyImmediate(text.GetComponent<AlphaBlink>());
                        text.gameObject.SetActive(false);
                        text.transform.localScale = Vector3.one * 0.75f;
                        text.transform.localPosition = new Vector3(-0.75f, -0.23f, 0f);
                    
                    }
                }

                //Fix Visor in Vitals
                foreach (VitalsPanel panel in __instance.vitals) 
                {
                    if (panel.PlayerIcon != null && panel.PlayerIcon.cosmetics.skin != null) {
                         panel.PlayerIcon.cosmetics.skin.transform.position = new Vector3(0, 0, 0f);
                    }
                }
            }
        }

        [HarmonyPatch(typeof(VitalsMinigame), nameof(VitalsMinigame.Update))]
        class VitalsMinigameUpdatePatch
        {

            static void Postfix(VitalsMinigame __instance) 
            {
                // Hacker show time since death
                
                if (Hacker.Player != null && Hacker.Player == PlayerControl.LocalPlayer && Hacker.hackerTimer > 0) {
                    for (int k = 0; k < __instance.vitals.Length; k++) {
                        VitalsPanel vitalsPanel = __instance.vitals[k];
                        NetworkedPlayerInfo player = vitalsPanel.PlayerInfo;

                        // Hacker update
                        if (vitalsPanel.IsDead) 
                        {
                            DeadPlayer deadPlayer = deadPlayers?.Where(x => x.player?.PlayerId == player?.PlayerId)?.FirstOrDefault();
                            if (deadPlayer != null && k < hackerTexts.Count && hackerTexts[k] != null) {
                                float timeSinceDeath = ((float)(DateTime.UtcNow - deadPlayer.DeathTime).TotalMilliseconds);
                                hackerTexts[k].gameObject.SetActive(true);
                                hackerTexts[k].text = Math.Round(timeSinceDeath / 1000) + "s";
                            }
                        }
                    }
                } else {
                    foreach (TMPro.TextMeshPro text in hackerTexts)
                        if (text != null && text.gameObject != null)
                            text.gameObject.SetActive(false);
                }
            }
        }
    }

    [HarmonyPatch]
    class AdminPanelPatch 
    {
        static Dictionary<SystemTypes, List<Color>> players = new Dictionary<SystemTypes, System.Collections.Generic.List<Color>>();
        [HarmonyPatch(typeof(MapCountOverlay), nameof(MapCountOverlay.Update))]
        class MapCountOverlayUpdatePatch 
        {
            static bool Prefix(MapCountOverlay __instance) 
            {
                // Save colors for the Hacker
                __instance.timer += Time.deltaTime;
                if (__instance.timer < 0.1f)
                {
                    return false;
                }
                __instance.timer = 0f;
                players = new Dictionary<SystemTypes, List<Color>>();
                bool commsActive = false;
                    foreach (PlayerTask task in PlayerControl.LocalPlayer.myTasks.GetFastEnumerator())
                        if (task.TaskType == TaskTypes.FixComms) commsActive = true;       


                if (!__instance.isSab && commsActive)
                {
                    __instance.isSab = true;
                    __instance.BackgroundColor.SetColor(Palette.DisabledGrey);
                    __instance.SabotageText.gameObject.SetActive(true);
                    return false;
                }
                if (__instance.isSab && !commsActive)
                {
                    __instance.isSab = false;
                    __instance.BackgroundColor.SetColor(Color.green);
                    __instance.SabotageText.gameObject.SetActive(false);
                }

                for (int i = 0; i < __instance.CountAreas.Length; i++)
                {
                    CounterArea counterArea = __instance.CountAreas[i];
                    List<Color> roomColors = new List<Color>();
                    players.Add(counterArea.RoomType, roomColors);

                    if (!commsActive)
                    {
                        PlainShipRoom plainShipRoom = MapUtilities.CachedShipStatus.FastRooms[counterArea.RoomType];

                        if (plainShipRoom != null && plainShipRoom.roomArea) 
                        {
                            HashSet<int> hashSet = new HashSet<int>();
                            int num = plainShipRoom.roomArea.OverlapCollider(__instance.filter, __instance.buffer);
                            int num2 = 0;
                            for (int j = 0; j < num; j++) 
                            {
                                Collider2D collider2D = __instance.buffer[j];
                                if (collider2D.CompareTag("DeadBody") && __instance.includeDeadBodies) 
                                {
                                    num2++;
                                    DeadBody bodyComponent = collider2D.GetComponent<DeadBody>();
                                    if (bodyComponent) 
                                    {
                                        NetworkedPlayerInfo playerInfo = GameData.Instance.GetPlayerById(bodyComponent.ParentId);
                                        if (playerInfo != null) 
                                        {
                                            var color = Palette.PlayerColors[playerInfo.DefaultOutfit.ColorId];
                                            if (Hacker.onlyColorType)
                                                color = Utils.IsD(playerInfo.PlayerId) ? Palette.PlayerColors[7] : Palette.PlayerColors[6];
                                            roomColors.Add(color);
                                        }
                                    }
                                } else if (!collider2D.isTrigger) 
                                {
                                    PlayerControl component = collider2D.GetComponent<PlayerControl>();
                                    if (component && component.Data != null && !component.Data.Disconnected && !component.Data.IsDead && (__instance.showLivePlayerPosition || !component.AmOwner) && hashSet.Add((int)component.PlayerId)) {
                                        num2++;
                                        if (component?.cosmetics?.currentBodySprite?.BodySprite?.material != null) {
                                            Color color = component.cosmetics.currentBodySprite.BodySprite.material.GetColor("_BodyColor");
                                            if (Hacker.onlyColorType) 
                                            {
                                                color = Utils.IsLighterColor(component) ? Palette.PlayerColors[7] : Palette.PlayerColors[6];
                                            }
                                            roomColors.Add(color);
                                        }
                                    }
                                }
                            }

                            counterArea.UpdateCount(num2);
                        }
                        else
                        {
                            Debug.LogWarning("Couldn't find counter for:" + counterArea.RoomType);
                        }
                    }
                    else
                    {
                        counterArea.UpdateCount(0);
                    }
                }
                return false;
            }
        }

        [HarmonyPatch(typeof(CounterArea), nameof(CounterArea.UpdateCount))]
        class CounterAreaUpdateCountPatch 
        {
            private static Material defaultMat;
            private static Material newMat;
            static void Postfix(CounterArea __instance) 
            {
                // Hacker display saved colors on the admin panel
                bool showHackerInfo = Hacker.Player != null && Hacker.Player == PlayerControl.LocalPlayer && Hacker.hackerTimer > 0;
                if (players.ContainsKey(__instance.RoomType)) 
                {
                    List<Color> colors = players[__instance.RoomType];
                    int i = -1;
                    foreach (var icon in __instance.myIcons.GetFastEnumerator())
                    {
                        i += 1;
                        SpriteRenderer renderer = icon.GetComponent<SpriteRenderer>();

                        if (renderer != null) 
                        {
                            if (defaultMat == null) defaultMat = renderer.material;
                            if (newMat == null) newMat = UnityEngine.Object.Instantiate<Material>(defaultMat);
                            if (showHackerInfo && colors.Count > i) 
                            {
                                renderer.material = newMat;
                                var color = colors[i];
                                renderer.material.SetColor("_BodyColor", color);
                                var id = Palette.PlayerColors.IndexOf(color);
                                if (id < 0) 
                                {
                                    renderer.material.SetColor("_BackColor", color);
                                } 
                                else 
                                {
                                    renderer.material.SetColor("_BackColor", Palette.ShadowColors[id]);
                                }
                                renderer.material.SetColor("_VisorColor", Palette.VisorColor);
                            } else {
                                renderer.material = defaultMat;
                            }
                        }
                    }
                }
            }
        }
    }

    [HarmonyPatch]
    class SurveillanceMinigamePatch 
    {
        private static int page = 0;
        private static float timer = 0f;

        public static List<GameObject> nightVisionOverlays = null;
        private static Sprite overlaySprite = Utils.LoadSpriteFromResources("TheSushiRoles.Resources.NightVisionOverlay.png", 350f);
        public static bool nightVisionIsActive = false;
        private static bool isLightsOut;

        [HarmonyPatch(typeof(SurveillanceMinigame), nameof(SurveillanceMinigame.Begin))]
        class SurveillanceMinigameBeginPatch 
        {
            public static void Postfix(SurveillanceMinigame __instance) 
            {
                // Add Vigilante cameras
                page = 0;
                timer = 0;
                if (MapUtilities.CachedShipStatus.AllCameras.Length > 4 && __instance.FilteredRooms.Length > 0) 
                {
                    __instance.textures = __instance.textures.ToList().Concat(new RenderTexture[MapUtilities.CachedShipStatus.AllCameras.Length - 4]).ToArray();
                    for (int i = 4; i < MapUtilities.CachedShipStatus.AllCameras.Length; i++) {
                        SurvCamera surv = MapUtilities.CachedShipStatus.AllCameras[i];
                        Camera camera = UnityEngine.Object.Instantiate<Camera>(__instance.CameraPrefab);
                        camera.transform.SetParent(__instance.transform);
                        camera.transform.position = new Vector3(surv.transform.position.x, surv.transform.position.y, 8f);
                        camera.orthographicSize = 2.35f;
                        RenderTexture temporary = RenderTexture.GetTemporary(256, 256, 16, (RenderTextureFormat)0);
                        __instance.textures[i] = temporary;
                        camera.targetTexture = temporary;
                    }
                }
            }
        }

        [HarmonyPatch(typeof(SurveillanceMinigame), nameof(SurveillanceMinigame.Update))]
        class SurveillanceMinigameUpdatePatch 
        {
            public static bool Prefix(SurveillanceMinigame __instance) 
            {
                // Update normal and Vigilante cameras
                timer += Time.deltaTime;
                int numberOfPages = Mathf.CeilToInt(MapUtilities.CachedShipStatus.AllCameras.Length / 4f);

                bool update = false;

                if (timer > 3f || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) {
                    update = true;
                    timer = 0f;
                    page = (page + 1) % numberOfPages;
                } else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) {
                    page = (page + numberOfPages - 1) % numberOfPages;
                    update = true;
                    timer = 0f;
                }

                if ((__instance.isStatic || update) && !PlayerTask.PlayerHasTaskOfType<IHudOverrideTask>(PlayerControl.LocalPlayer)) {
                    __instance.isStatic = false;
                    for (int i = 0; i < __instance.ViewPorts.Length; i++) {
                        __instance.ViewPorts[i].sharedMaterial = __instance.DefaultMaterial;
                        __instance.SabText[i].gameObject.SetActive(false);
                        if (page * 4 + i < __instance.textures.Length)
                            __instance.ViewPorts[i].material.SetTexture("_MainTex", __instance.textures[page * 4 + i]);
                        else
                            __instance.ViewPorts[i].sharedMaterial = __instance.StaticMaterial;
                    }
                } else if (!__instance.isStatic && PlayerTask.PlayerHasTaskOfType<HudOverrideTask>(PlayerControl.LocalPlayer)) {
                    __instance.isStatic = true;
                    for (int j = 0; j < __instance.ViewPorts.Length; j++) {
                        __instance.ViewPorts[j].sharedMaterial = __instance.StaticMaterial;
                        __instance.SabText[j].gameObject.SetActive(true);
                    }
                }

                nightVisionUpdate(SkeldCamsMinigame: __instance);
                return false;
            }
        }

        [HarmonyPatch(typeof(PlanetSurveillanceMinigame), nameof(PlanetSurveillanceMinigame.Update))]
        class PlanetSurveillanceMinigameUpdatePatch {
            public static void Postfix(PlanetSurveillanceMinigame __instance) {
                if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
                    __instance.NextCamera(1);
                if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
                    __instance.NextCamera(-1);

                nightVisionUpdate(SwitchCamsMinigame: __instance);
            }
        }

        [HarmonyPatch(typeof(FungleSurveillanceMinigame), nameof(FungleSurveillanceMinigame.Update))]
        class FungleSurveillanceMinigameUpdatePatch {
            public static void Postfix(FungleSurveillanceMinigame __instance) {
                nightVisionUpdate(FungleCamMinigame: __instance);
            }
        }


        [HarmonyPatch(typeof(SurveillanceMinigame), nameof(SurveillanceMinigame.OnDestroy))]
        class SurveillanceMinigameDestroyPatch {
            public static void Prefix() {
                ResetNightVision();
            }
        }

        [HarmonyPatch(typeof(PlanetSurveillanceMinigame), nameof(PlanetSurveillanceMinigame.OnDestroy))]
        class PlanetSurveillanceMinigameDestroyPatch {
            public static void Prefix() {
                ResetNightVision();
            }
        }

        private static void nightVisionUpdate(SurveillanceMinigame SkeldCamsMinigame = null, PlanetSurveillanceMinigame SwitchCamsMinigame = null, FungleSurveillanceMinigame FungleCamMinigame = null) {
            GameObject closeButton = null;
            if (nightVisionOverlays == null) {
                List<MeshRenderer> viewPorts = new();
                Transform viewablesTransform = null;
                if (SkeldCamsMinigame != null) {
                    closeButton = SkeldCamsMinigame.Viewables.transform.Find("CloseButton").gameObject;
                    foreach (var rend in SkeldCamsMinigame.ViewPorts) viewPorts.Add(rend);
                    viewablesTransform = SkeldCamsMinigame.Viewables.transform;
                } else if (SwitchCamsMinigame != null) {
                    closeButton = SwitchCamsMinigame.Viewables.transform.Find("CloseButton").gameObject;
                    viewPorts.Add(SwitchCamsMinigame.ViewPort);
                    viewablesTransform = SwitchCamsMinigame.Viewables.transform;
                } else if (FungleCamMinigame != null) {
                    closeButton = FungleCamMinigame.transform.Find("CloseButton").gameObject;
                    viewPorts.Add(FungleCamMinigame.viewport);
                    viewablesTransform = FungleCamMinigame.viewport.transform;
                } else return;

                nightVisionOverlays = new List<GameObject>();

                foreach (var renderer in viewPorts) {
                    GameObject overlayObject;
                    float zPosition;
                    if (FungleCamMinigame != null) {
                        overlayObject = GameObject.Instantiate(closeButton, renderer.transform);
                        overlayObject.layer = renderer.gameObject.layer;
                        zPosition = - 0.5f;
                        overlayObject.transform.localPosition = new Vector3(0, 0, zPosition);
                    } else {
                        overlayObject = GameObject.Instantiate(closeButton, viewablesTransform);
                        zPosition = overlayObject.transform.position.z;
                        overlayObject.layer = closeButton.layer;
                        overlayObject.transform.position = new Vector3(renderer.transform.position.x, renderer.transform.position.y, zPosition);
                    }
                    Vector3 localScale = (SkeldCamsMinigame != null) ? new Vector3(0.91f, 0.612f, 1f) : new Vector3(2.124f, 1.356f, 1f);
                    localScale = (FungleCamMinigame != null) ? new Vector3(10f, 10f, 1f) : localScale;
                    overlayObject.transform.localScale = localScale;
                    var overlayRenderer = overlayObject.GetComponent<SpriteRenderer>();
                    overlayRenderer.sprite = overlaySprite;
                    overlayObject.SetActive(false);
                    GameObject.Destroy(overlayObject.GetComponent<CircleCollider2D>());
                    nightVisionOverlays.Add(overlayObject);
                }
            }

            isLightsOut = PlayerControl.LocalPlayer.myTasks.ToArray().Any(x => x.name.Contains("FixLightsTask")) || Trickster.lightsOutTimer > 0;
            bool ignoreNightVision = CustomOptionHolder.camsNoNightVisionIfImpVision.GetBool() && Utils.HasImpVision(GameData.Instance.GetPlayerById(PlayerControl.LocalPlayer.PlayerId)) || PlayerControl.LocalPlayer.Data.IsDead;
            bool nightVisionEnabled = CustomOptionHolder.camsNightVision.GetBool();

            if (isLightsOut && !nightVisionIsActive && nightVisionEnabled && !ignoreNightVision) {  // only update when something changed!
                foreach (PlayerControl pc in PlayerControl.AllPlayerControls) 
                {
                    if (pc == Ninja.Player && Ninja.invisibleTimer > 0f || pc == Wraith.Player && Wraith.VanishTimer > 0f) 
                    {
                        continue;
                    }
                    pc.SetLook("", 11, "", "", "", "", false);
                }
                foreach (var overlayObject in nightVisionOverlays) {
                    overlayObject.SetActive(true);
                }
                // Dead Bodies
                foreach (DeadBody deadBody in GameObject.FindObjectsOfType<DeadBody>()) {
                    SpriteRenderer component = deadBody.bodyRenderers.FirstOrDefault();
                    component.material.SetColor("_BackColor", Palette.ShadowColors[11]);
                    component.material.SetColor("_BodyColor", Palette.PlayerColors[11]);
                }
                nightVisionIsActive = true;
            } else if (!isLightsOut && nightVisionIsActive) {
                ResetNightVision();
            }
        }

        public static void ResetNightVision() 
        {
            foreach (var go in nightVisionOverlays) 
            {
                go.Destroy();
            }
            nightVisionOverlays = null;

            if (nightVisionIsActive) 
            {
                nightVisionIsActive = false;
                foreach (PlayerControl pc in PlayerControl.AllPlayerControls) 
                {
                    if (Camouflager.CamouflageTimer > 0) 
                    {
                        pc.SetLook("", 6, "", "", "", "", false);
                    }
                    else if (pc == Morphling.Player && Morphling.morphTimer > 0) 
                    {
                        PlayerControl target = Morphling.morphTarget;
                        Morphling.Player.SetLook(target.Data.PlayerName, target.Data.DefaultOutfit.ColorId, target.Data.DefaultOutfit.HatId, target.Data.DefaultOutfit.VisorId, target.Data.DefaultOutfit.SkinId, target.Data.DefaultOutfit.PetId, false);
                    }
                    else if (pc == Glitch.Player && Glitch.MimicTimer > 0) 
                    {
                        PlayerControl target = Glitch.MimicTarget;
                        Glitch.Player.SetLook(target.Data.PlayerName, target.Data.DefaultOutfit.ColorId, target.Data.DefaultOutfit.HatId, target.Data.DefaultOutfit.VisorId, target.Data.DefaultOutfit.SkinId, target.Data.DefaultOutfit.PetId, false);
                    }
                    else if (pc == Hitman.Player && Hitman.MorphTimer > 0) 
                    {
                        PlayerControl target = Hitman.MorphTarget;
                        Hitman.Player.SetLook(target.Data.PlayerName, target.Data.DefaultOutfit.ColorId, target.Data.DefaultOutfit.HatId, target.Data.DefaultOutfit.VisorId, target.Data.DefaultOutfit.SkinId, target.Data.DefaultOutfit.PetId, false);
                    }
                    else if (pc == Ninja.Player && Ninja.invisibleTimer > 0f || pc == Wraith.Player && Wraith.VanishTimer > 0f ) 
                    {
                        continue;
                    }
                    else 
                    {
                        Utils.SetDefaultLook(pc, false);
                    }
                    // Dead Bodies
                    foreach (DeadBody deadBody in GameObject.FindObjectsOfType<DeadBody>()) 
                    {
                        var colorId = GameData.Instance.GetPlayerById(deadBody.ParentId).Object.Data.DefaultOutfit.ColorId;
                        SpriteRenderer component = deadBody.bodyRenderers.FirstOrDefault();
                        component.material.SetColor("_BackColor", Palette.ShadowColors[colorId]);
                        component.material.SetColor("_BodyColor", Palette.PlayerColors[colorId]);
                    }
                }
            }

        }

        public static void EnforceNightVision(PlayerControl player) 
        {
            if (isLightsOut && nightVisionOverlays != null && nightVisionIsActive) 
            {
                player.SetLook("", 11, "", "", "", "", false);
            }
        }

        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.SetPlayerMaterialColors))]
        public static void Postfix(PlayerControl __instance, SpriteRenderer rend) 
        {
            if (!nightVisionIsActive) return;
            foreach (DeadBody deadBody in GameObject.FindObjectsOfType<DeadBody>()) 
            {
                foreach (SpriteRenderer component in new SpriteRenderer[2] { deadBody.bodyRenderers.FirstOrDefault(), deadBody.bloodSplatter }) { 
                    component.material.SetColor("_BackColor", Palette.ShadowColors[11]);
                    component.material.SetColor("_BodyColor", Palette.PlayerColors[11]);
                }
            }
        }
    }
    [HarmonyPatch(typeof(MapBehaviour), nameof(MapBehaviour.ShowSabotageMap))]
    class ShowSabotageMapPatch
    {
        static bool Prefix(MapBehaviour __instance) 
        {
            if (PlayerControl.LocalPlayer.Data.IsDead && CustomOptionHolder.deadImpsBlockSabotage.GetBool()  
            || Grenadier.Active || Utils.TwoPlayersAlive() && MapOptions.LimitAbilities) 
            {
                __instance.ShowNormalMap();
                return false;
            }
            return true;
        }
    }

}
