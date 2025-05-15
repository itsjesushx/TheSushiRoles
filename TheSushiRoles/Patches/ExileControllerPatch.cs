using Hazel;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using Reactor.Utilities;

namespace TheSushiRoles.Patches 
{
    [HarmonyPatch(typeof(ExileController), nameof(ExileController.BeginForGameplay))]
    [HarmonyPriority(Priority.First)]
    class ExileControllerBeginPatch 
    {
        public static void Prefix(ExileController __instance, [HarmonyArgument(0)]ref NetworkedPlayerInfo exiled) 
        {
            // Medic Shield
            if (Medic.Player != null && AmongUsClient.Instance.AmHost && Medic.futureShielded != null && !Medic.Player.Data.IsDead) 
            { 
                // Need to send the RPC from the host here, to make sure that the order of setting the Shield is correct(for that reason the futureShielded are being synced)
                Utils.StartRPC(CustomRPC.MedicSetShielded, Medic.futureShielded.PlayerId);
                RPCProcedure.MedicSetShielded(Medic.futureShielded.PlayerId);
            }
            if (Medic.usedShield) Medic.meetingAfterShielding = true;  // Has to be after the setting of the Shield

            // Eraser erase
            if (Eraser.Player != null && AmongUsClient.Instance.AmHost && Eraser.futureErased != null) 
            {  // Need to send the RPC from the host here, to make sure that the order of erasing is correct (for that reason the futureErased are being synced)
                foreach (PlayerControl target in Eraser.futureErased) 
                {
                    Utils.StartRPC(CustomRPC.ErasePlayerRoles, target.PlayerId);
                    RPCProcedure.ErasePlayerRoles(target.PlayerId);
                    if (!target.Data.Role.IsImpostor)
                    {
                        GameHistory.AddToRoleHistory(target.PlayerId, RoleInfo.crewmate);
                    }
                    else
                    {
                        GameHistory.AddToRoleHistory(target.PlayerId, RoleInfo.impostor);
                    }
                    Eraser.alreadyErased.Add(target.PlayerId);
                }
            }
            Eraser.futureErased = new List<PlayerControl>();

            // Trickster boxes
            if (Trickster.Player != null && JackInTheBox.HasJackInTheBoxLimitReached()) 
            {
                JackInTheBox.ConvertToVents();
            }

            if (PlayerControl.LocalPlayer != null && PlayerControl.LocalPlayer == Amnesiac.Player && !PlayerControl.LocalPlayer.Data.IsDead && !Amnesiac.Remembered)
            {
                foreach (var player in PlayerControl.AllPlayerControls)
                {
                    if (player != Amnesiac.Player && !player.Data.Disconnected)
                    {
                        if (player.Data.IsDead) Amnesiac.PlayersToRemember.Add(player.PlayerId);
                        //else return;
                    }
                }
                byte[] rememberTargets = Amnesiac.PlayersToRemember.ToArray();
                var pk = new ShapeShifterMenu((x) =>
                {
                    Utils.StartRPC(CustomRPC.AmnesiacRemember, x.PlayerId);
                    RPCProcedure.AmnesiacRemember(x.PlayerId);
                }, (y) =>
                {
                    return rememberTargets.Contains(y.PlayerId);
                });
                Coroutines.Start(pk.Open(3f, true));
            }

            // Activate portals.
            Portal.MeetingEndsUpdate();

            // Witch execute casted spells
            if (Witch.Player != null && Witch.futureSpelled != null && AmongUsClient.Instance.AmHost) 
            {
                bool exiledIsWitch = exiled != null && exiled.PlayerId == Witch.Player.PlayerId;
                bool witchDiesWithExiledLover = exiled != null && Lovers.Existing() && Lovers.bothDie && (Lovers.Lover1.PlayerId == Witch.Player.PlayerId || Lovers.Lover2.PlayerId == Witch.Player.PlayerId) && (exiled.PlayerId == Lovers.Lover1.PlayerId || exiled.PlayerId == Lovers.Lover2.PlayerId);

                if ((witchDiesWithExiledLover || exiledIsWitch) && Witch.witchVoteSavesTargets) Witch.futureSpelled = new List<PlayerControl>();
                foreach (PlayerControl target in Witch.futureSpelled) 
                {
                    if (target != null && !target.Data.IsDead && Utils.CheckMuderAttempt(Witch.Player, target, true) == MurderAttemptResult.PerformKill)
                    {
                        if (exiled != null && Prosecutor.Player != null && (target == Prosecutor.Player || target == Lovers.OtherLover(Prosecutor.Player)) 
                        && Prosecutor.target != null && Prosecutor.target.PlayerId == exiled.PlayerId) continue;

                        if (target == Prosecutor.target && Prosecutor.Player != null) 
                        {
                            Utils.StartRPC(CustomRPC.ProsecutorChangeRole);
                            RPCProcedure.ProsecutorChangeRole();
                        }

                        if (target == Lawyer.target && Lawyer.Player != null) 
                        {
                            Utils.StartRPC(CustomRPC.LawyerChangeRole);
                            RPCProcedure.LawyerChangeRole();
                        }

                        if (target == Romantic.beloved && Romantic.Player != null) 
                        {
                            Utils.StartRPC(CustomRPC.RomanticChangeRole);
                            RPCProcedure.RomanticChangeRole();
                        }

                        Utils.StartRPC(CustomRPC.UncheckedExilePlayer, target.PlayerId);
                        RPCProcedure.UncheckedExilePlayer(target.PlayerId);

                        Utils.StartRPC(CustomRPC.ShareGhostInfo, 
                        PlayerControl.LocalPlayer.PlayerId, 
                        (byte)GhostInfoTypes.DeathReasonAndKiller, 
                        target.PlayerId, 
                        (byte)DeadPlayer.CustomDeathReason.WitchExile,
                        Witch.Player.PlayerId);
                        GameHistory.CreateDeathReason(target, DeadPlayer.CustomDeathReason.WitchExile, killer: Witch.Player);
                    }
                }
            }
            Witch.futureSpelled = new List<PlayerControl>();

            // Vigilante vents and cameras
            var allCameras = MapUtilities.CachedShipStatus.AllCameras.ToList();
            MapOptions.CamsToAdd.ForEach(camera => 
            {
                camera.gameObject.SetActive(true);
                camera.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                allCameras.Add(camera);
            });
            MapUtilities.CachedShipStatus.AllCameras = allCameras.ToArray();
            MapOptions.CamsToAdd = new List<SurvCamera>();

            foreach (Vent vent in MapOptions.VentsToSeal) 
            {
                PowerTools.SpriteAnim animator = vent.GetComponent<PowerTools.SpriteAnim>();
                vent.EnterVentAnim = vent.ExitVentAnim = null;
                Sprite newSprite = animator == null ? Vigilante.GetStaticVentSealedSprite() : Vigilante.GetAnimatedVentSealedSprite();
                SpriteRenderer rend = vent.myRend;
                if (Utils.IsFungle()) 
                {
                    newSprite = Vigilante.GetFungleVentSealedSprite();
                    rend = vent.transform.GetChild(3).GetComponent<SpriteRenderer>();
                    animator = vent.transform.GetChild(3).GetComponent<PowerTools.SpriteAnim>();
                }
                animator?.Stop();
                rend.sprite = newSprite;
                if (SubmergedCompatibility.IsSubmerged && vent.Id == 0) vent.myRend.sprite = Vigilante.GetSubmergedCentralUpperSealedSprite();
                if (SubmergedCompatibility.IsSubmerged && vent.Id == 14) vent.myRend.sprite = Vigilante.GetSubmergedCentralLowerSealedSprite();
                rend.color = Color.white;
                vent.name = "SealedVent_" + vent.name;
            }
            MapOptions.VentsToSeal = new List<Vent>();
        }
    }

    [HarmonyPatch]
    class ExileControllerWrapUpPatch 
    {

        [HarmonyPatch(typeof(ExileController), nameof(ExileController.WrapUp))]
        class BaseExileControllerPatch 
        {
            public static void Postfix(ExileController __instance) 
            {
                NetworkedPlayerInfo networkedPlayer = __instance.initData.networkedPlayer;
                WrapUpPostfix((networkedPlayer != null) ? networkedPlayer.Object : null);

                if (__instance == null) return;
            }
        }

        [HarmonyPatch(typeof(AirshipExileController), nameof(AirshipExileController.WrapUpAndSpawn))]
        class AirshipExileControllerPatch 
        {
            public static void Postfix(AirshipExileController __instance) 
            {
                NetworkedPlayerInfo networkedPlayer = __instance.initData.networkedPlayer;
                WrapUpPostfix((networkedPlayer != null) ? networkedPlayer.Object : null);
            }
        }

        // Workaround to add a "postfix" to the destroying of the exile controller (i.e. cutscene) and SpwanInMinigame of submerged
        [HarmonyPatch(typeof(UnityEngine.Object), nameof(UnityEngine.Object.Destroy), new Type[] { typeof(GameObject) })]
        public static void Prefix(GameObject obj) 
        {
            // Nightvision:
            if (obj != null && obj.name != null && obj.name.Contains("FungleSecurity")) 
            {
                SurveillanceMinigamePatch.ResetNightVision();
                return;
            }

            // submerged
            if (!SubmergedCompatibility.IsSubmerged) return;
            if (obj.name.Contains("ExileCutscene")) 
            {
                WrapUpPostfix(obj.GetComponent<ExileController>().initData.networkedPlayer?.Object);
            } else if (obj.name.Contains("SpawnInMinigame")) 
            {
                Lazy.SetPosition();
                Chameleon.lastMoved.Clear();
            }
        }

        static void WrapUpPostfix(PlayerControl exiled) 
        {
            // Prosecutor win condition
            if (exiled != null && Prosecutor.Player != null && Prosecutor.target != null && Prosecutor.target.PlayerId == exiled.PlayerId && !Prosecutor.Player.Data.IsDead)
            {
                Prosecutor.IsProsecutorWin = true;
            }

            // Mini exile lose condition
            else if (exiled != null && Mini.Player != null && Mini.Player.PlayerId == exiled.PlayerId && !Mini.IsGrownUp && !Mini.Player.Data.Role.IsImpostor && !RoleInfo.GetRoleInfoForPlayer(Mini.Player).Any(x => x.FactionId == Faction.Neutrals)) 
            {
                Mini.IsMiniLose = true;
            }
            // Jester win condition
            else if (exiled != null && Jester.Player != null && Jester.Player.PlayerId == exiled.PlayerId) 
            {
                Jester.IsJesterWin = true;
            }

            // Reset custom button timers where necessary
            CustomButton.MeetingEndedUpdate();

            // Blackmailer remove blackmailed
            if (Blackmailer.BlackmailedPlayer != null) 
            {
                Utils.StartRPC(CustomRPC.RemoveBlackmail);
                RPCProcedure.RemoveBlackmail();
            }


            Mystic.Investigated = false;

            Crusader.FortifiedPlayer = null;
            Crusader.Fortified = false;

            Oracle.Investigated = false;

            // Mini set adapted Cooldown
            if (Mini.Player != null && PlayerControl.LocalPlayer == Mini.Player && Mini.Player.Data.Role.IsImpostor) 
            {
                var multiplier = Mini.IsGrownUp ? 0.66f : 2f;
                Mini.Player.SetKillTimer(GameOptionsManager.Instance.currentNormalGameOptions.KillCooldown * multiplier);
            }

            // Mystic spawn souls
            if (Mystic.deadBodyPositions != null && Mystic.Player != null && PlayerControl.LocalPlayer == Mystic.Player && (Mystic.mode == 0 || Mystic.mode == 2)) 
            {
                foreach (Vector3 pos in Mystic.deadBodyPositions) 
                {
                    GameObject soul = new GameObject();
                    //soul.transform.position = pos;
                    soul.transform.position = new Vector3(pos.x, pos.y, pos.y / 1000 - 1f);
                    soul.layer = 5;
                    var rend = soul.AddComponent<SpriteRenderer>();
                    soul.AddSubmergedComponent(SubmergedCompatibility.Classes.ElevatorMover);
                    rend.sprite = Mystic.GetSoulSprite();
                    
                    if(Mystic.limitSoulDuration) 
                    {
                        FastDestroyableSingleton<HudManager>.Instance.StartCoroutine(Effects.Lerp(Mystic.soulDuration, new Action<float>((p) => {
                            if (rend != null) 
                            {
                                var tmp = rend.color;
                                tmp.a = Mathf.Clamp01(1 - p);
                                rend.color = tmp;
                            }    
                            if (p == 1f && rend != null && rend.gameObject != null) UnityEngine.Object.Destroy(rend.gameObject);
                        })));
                    }
                }
                Mystic.deadBodyPositions = new List<Vector3>();
            }

            // Tracker reset deadBodyPositions
            Tracker.deadBodyPositions = new List<Vector3>();

            // Arsonist deactivate dead poolable players
            if (Arsonist.Player != null && Arsonist.Player == PlayerControl.LocalPlayer) 
            {
                int visibleCounter = 0;
                Vector3 newBottomLeft = IntroCutsceneOnDestroyPatch.bottomLeft;
                var BottomLeft = newBottomLeft + new Vector3(-0.25f, -0.25f, 0);
                foreach (PlayerControl p in PlayerControl.AllPlayerControls) 
                {
                    if (!MapOptions.BeanIcons.ContainsKey(p.PlayerId)) continue;
                    if (p.Data.IsDead || p.Data.Disconnected) 
                    {
                        MapOptions.BeanIcons[p.PlayerId].gameObject.SetActive(false);
                    } 
                    else 
                    {
                        MapOptions.BeanIcons[p.PlayerId].transform.localPosition = newBottomLeft + Vector3.right * visibleCounter * 0.35f;
                        visibleCounter++;
                    }
                }
            }

            // Force Bounty Hunter Bounty Update
            if (BountyHunter.Player != null && BountyHunter.Player == PlayerControl.LocalPlayer)
                BountyHunter.bountyUpdateTimer = 0f;

            if (Miner.MineVisibility == 1) MinerVent.ConvertToVents();

            // Medium spawn souls
            if (Medium.medium != null && PlayerControl.LocalPlayer == Medium.medium) 
            {
                if (Medium.souls != null) 
                {
                    foreach (SpriteRenderer sr in Medium.souls) UnityEngine.Object.Destroy(sr.gameObject);
                    Medium.souls = new List<SpriteRenderer>();
                }

                if (Medium.futureDeadBodies != null) 
                {
                    foreach ((DeadPlayer db, Vector3 ps) in Medium.futureDeadBodies) 
                    {
                        GameObject s = new GameObject();
                        //s.transform.position = ps;
                        s.transform.position = new Vector3(ps.x, ps.y, ps.y / 1000 - 1f);
                        s.layer = 5;
                        var rend = s.AddComponent<SpriteRenderer>();
                        s.AddSubmergedComponent(SubmergedCompatibility.Classes.ElevatorMover);
                        rend.sprite = Medium.GetSoulSprite();
                        Medium.souls.Add(rend);
                    }
                    Medium.deadBodies = Medium.futureDeadBodies;
                    Medium.futureDeadBodies = new List<Tuple<DeadPlayer, Vector3>>();
                }
            }

            // Lazy set position
            Lazy.SetPosition();

            // Invert add meeting
            if (Invert.meetings > 0) Invert.meetings--;

            Chameleon.lastMoved.Clear();

            foreach (Trap trap in Trap.traps) trap.triggerable = false;
            FastDestroyableSingleton<HudManager>.Instance.StartCoroutine(Effects.Lerp(GameOptionsManager.Instance.currentNormalGameOptions.KillCooldown / 2 + 2, new Action<float>((p) => 
            {
            if (p == 1f) foreach (Trap trap in Trap.traps) trap.triggerable = true;
            })));

            if (!Yoyo.markStaysOverMeeting)
                Silhouette.ClearSilhouettes();
        }
    }

    [HarmonyPatch(typeof(SpawnInMinigame), nameof(SpawnInMinigame.Close))]  // Set position of AntiTp players AFTER they have selected a spawn.
    class AirshipSpawnInPatch 
    {
        static void Postfix() 
        {
            Lazy.SetPosition();
            Chameleon.lastMoved.Clear();
        }
    }

    [HarmonyPatch(typeof(TranslationController), nameof(TranslationController.GetString), new Type[] { typeof(StringNames), typeof(Il2CppReferenceArray<Il2CppSystem.Object>) })]
    class ExileControllerMessagePatch 
    {
        static void Postfix(ref string __result, [HarmonyArgument(0)]StringNames id) 
        {
            try 
            {
                if (ExileController.Instance != null && ExileController.Instance.initData != null) 
                {
                    PlayerControl player = ExileController.Instance.initData.networkedPlayer.Object;
                    if (player == null) return;
                    // Exile role text
                    if (id == StringNames.ExileTextPN || id == StringNames.ExileTextSN || id == StringNames.ExileTextPP || id == StringNames.ExileTextSP) {
                        __result = player.Data.PlayerName + " was The " + String.Join(" ", RoleInfo.GetRoleInfoForPlayer(player).Select(x => x.Name).ToArray());
                    }
                    // Hide number of remaining impostors on Jester win
                    if (id == StringNames.ImpostorsRemainP || id == StringNames.ImpostorsRemainS) 
                    {
                        if (Jester.Player != null && player.PlayerId == Jester.Player.PlayerId) __result = "";
                    }
                    if (Tiebreaker.isTiebreak) __result += " (Tiebreaker Vote)";
                    Tiebreaker.isTiebreak = false;
                }
            } 
            catch 
            {
                // pass - Hopefully prevent leaving while exiling to softlock game
            }
        }
    }
}
