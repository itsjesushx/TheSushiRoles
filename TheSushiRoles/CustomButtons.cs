using Hazel;
using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Reactor.Utilities.Extensions;

namespace TheSushiRoles
{
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Start))]
    static class HudManagerStartPatch
    {
        private static bool initialized = false;
        private static CustomButton engineerRepairButton;
        private static CustomButton BlackmailButton;
        public static CustomButton sheriffKillButton;
        private static CustomButton GlitchHackButton;
        private static CustomButton MysticButton;
        private static CustomButton MimicButton;
        private static CustomButton HitmanMorphButton;
        private static CustomButton CrusaderButton;
        private static CustomButton TimeMasterRewindButton;
        private static CustomButton medicShieldButton;
        private static CustomButton OracleButton;
        private static CustomButton VeteranAlertButton;
        private static CustomButton morphlingButton;
        private static CustomButton camouflagerButton;
        private static CustomButton portalmakerPlacePortalButton;
        private static CustomButton MinerMineButton;
        private static CustomButton usePortalButton;
        private static CustomButton portalmakerMoveToPortalButton;
        private static CustomButton hackerButton;
        public static CustomButton hackerVitalsButton;
        public static CustomButton hackerAdminTableButton;
        private static CustomButton UndertakerButton;
        private static CustomButton HitmanDragButton;
        private static CustomButton trackerTrackPlayerButton;
        private static CustomButton trackerTrackCorpsesButton;
        private static CustomButton RomanticSetTargetButton;
        public static CustomButton poisonerKillButton;
        public static CustomButton HitmanKillButton;
        public static CustomButton JuggernautKillButton;
        public static CustomButton jackalKillButton;
        public static CustomButton PredatorTerminateButton;
        public static CustomButton PredatorKillButton;
        public static CustomButton GlitchKillButton;
        public static CustomButton RomanticKillButton;
        public static CustomButton WerewolfMaulButton;
        public static CustomButton sidekickKillButton;
        public static CustomButton PlaguebearerButton;
        public static CustomButton PestilenceButton;
        public static CustomButton jackalSidekickButton;
        private static CustomButton eraserButton;
        private static CustomButton placeJackInTheBoxButton;
        private static CustomButton DisperserButton;
        private static CustomButton lightsOutButton;
        public static CustomButton WraithButton;
        public static CustomButton GrenadierButton;
        public static CustomButton cleanerCleanButton;
        public static CustomButton warlockCurseButton;
        public static CustomButton VigilanteButton;
        public static CustomButton VigilanteCamButton;
        public static CustomButton arsonistButton;
        public static CustomButton vultureEatButton;
        public static CustomButton mediumButton;
        public static CustomButton pursuerButton;
        public static CustomButton witchSpellButton;
        public static CustomButton ninjaButton;
        public static CustomButton mayorMeetingButton;
        public static CustomButton CowardButton;
        public static CustomButton trapperButton;
        public static CustomButton yoyoButton;
        public static CustomButton yoyoAdminTableButton;
        public static CustomButton zoomOutButton;

        public static Dictionary<byte, List<CustomButton>> PlayerHackedButtons = null;
        public static PoolablePlayer targetDisplay;

        public static TMPro.TMP_Text VigilanteButtonScrewsText;
        public static TMPro.TMP_Text VigilanteChargesText;
        public static TMPro.TMP_Text VeteranChargesText;
        public static TMPro.TMP_Text MysticChargesText;
        public static TMPro.TMP_Text OracleChargesText;
        public static TMPro.TMP_Text CrusaderChargesText;
        public static TMPro.TMP_Text GlitchButtonHacksText;
        public static TMPro.TMP_Text TimeMasterChargesText;
        public static TMPro.TMP_Text pursuerButtonBlanksText;
        public static TMPro.TMP_Text hackerAdminTableChargesText;
        public static TMPro.TMP_Text hackerVitalsChargesText;
        public static TMPro.TMP_Text DisperserChargesText;
        public static TMPro.TMP_Text trapperChargesText;
        public static TMPro.TMP_Text portalmakerButtonText1;
        public static TMPro.TMP_Text portalmakerButtonText2;

        public static void SetCustomButtonCooldowns() 
        {
            if (!initialized) 
            {
                try 
                {
                    CreateButtonsPostfix(HudManager.Instance);
                } 
                catch 
                {
                    TheSushiRolesPlugin.Logger.LogWarning("Button Cooldowns not set, there's something wrong.");
                    return;
                }
            }
            engineerRepairButton.MaxTimer = 0f;
            sheriffKillButton.MaxTimer = Sheriff.Cooldown;
            HitmanKillButton.MaxTimer = Hitman.Cooldown;
            GlitchHackButton.MaxTimer = Glitch.HackCooldown;
            TimeMasterRewindButton.MaxTimer = TimeMaster.Cooldown;
            WraithButton.MaxTimer = Wraith.Cooldown;
            PlaguebearerButton.MaxTimer = Plaguebearer.Cooldown;
            medicShieldButton.MaxTimer = 0f;
            OracleButton.MaxTimer = Oracle.Cooldown;
            RomanticSetTargetButton.MaxTimer = 0f;
            VeteranAlertButton.MaxTimer = Veteran.Cooldown;
            BlackmailButton.MaxTimer = Blackmailer.Cooldown;
            PredatorTerminateButton.MaxTimer = Predator.TerminateCooldown;
            PredatorKillButton.MaxTimer = Predator.TerminateKillCooldown;
            GrenadierButton.MaxTimer = Grenadier.Cooldown;
            UndertakerButton.MaxTimer = Undertaker.Cooldown;
            HitmanDragButton.MaxTimer = Hitman.DragCooldown;
            DisperserButton.MaxTimer = Disperser.Cooldown;
            morphlingButton.MaxTimer = Morphling.Cooldown;
            MimicButton.MaxTimer = Glitch.MimicCooldown;
            HitmanMorphButton.MaxTimer = Hitman.MorphCooldown;
            camouflagerButton.MaxTimer = Camouflager.Cooldown;
            portalmakerPlacePortalButton.MaxTimer = Portalmaker.Cooldown;
            MinerMineButton.MaxTimer = Miner.Cooldown;
            usePortalButton.MaxTimer = Portalmaker.usePortalCooldown;
            CrusaderButton.MaxTimer = Crusader.Cooldown;
            MysticButton.MaxTimer = Mystic.Cooldown;
            portalmakerMoveToPortalButton.MaxTimer = Portalmaker.usePortalCooldown;
            hackerButton.MaxTimer = Hacker.Cooldown;
            hackerVitalsButton.MaxTimer = Hacker.Cooldown;
            hackerAdminTableButton.MaxTimer = Hacker.Cooldown;
            poisonerKillButton.MaxTimer = Poisoner.Cooldown;
            trackerTrackPlayerButton.MaxTimer = 0f;
            PestilenceButton.MaxTimer = Pestilence.Cooldown;
            jackalKillButton.MaxTimer = Jackal.Cooldown;
            GlitchKillButton.MaxTimer = Glitch.KillCooldown;
            RomanticKillButton.MaxTimer = VengefulRomantic.Cooldown;
            WerewolfMaulButton.MaxTimer = Werewolf.Cooldown;
            JuggernautKillButton.MaxTimer = Juggernaut.Cooldown;
            sidekickKillButton.MaxTimer = Sidekick.Cooldown;
            jackalSidekickButton.MaxTimer = Jackal.createSidekickCooldown;
            eraserButton.MaxTimer = Eraser.Cooldown;
            placeJackInTheBoxButton.MaxTimer = Trickster.placeBoxCooldown;
            lightsOutButton.MaxTimer = Trickster.lightsOutCooldown;
            cleanerCleanButton.MaxTimer = Cleaner.Cooldown;
            warlockCurseButton.MaxTimer = Warlock.Cooldown;
            VigilanteButton.MaxTimer = Vigilante.Cooldown;
            VigilanteCamButton.MaxTimer = Vigilante.Cooldown;
            arsonistButton.MaxTimer = Arsonist.Cooldown;
            vultureEatButton.MaxTimer = Vulture.Cooldown;
            mediumButton.MaxTimer = Medium.Cooldown;
            pursuerButton.MaxTimer = Pursuer.Cooldown;
            trackerTrackCorpsesButton.MaxTimer = Tracker.corpsesTrackingCooldown;
            witchSpellButton.MaxTimer = Witch.Cooldown;
            ninjaButton.MaxTimer = Ninja.Cooldown;
            mayorMeetingButton.MaxTimer = GameManager.Instance.LogicOptions.GetEmergencyCooldown();
            CowardButton.MaxTimer = GameManager.Instance.LogicOptions.GetEmergencyCooldown();
            trapperButton.MaxTimer = Trapper.Cooldown;
            yoyoButton.MaxTimer = Yoyo.markCooldown;
            yoyoAdminTableButton.MaxTimer = Yoyo.adminCooldown;
            yoyoAdminTableButton.EffectDuration = 10f;

            TimeMasterRewindButton.EffectDuration = TimeMaster.RewindTimeDuration;
            hackerButton.EffectDuration = Hacker.Duration;
            hackerVitalsButton.EffectDuration = Hacker.Duration;
            hackerAdminTableButton.EffectDuration = Hacker.Duration;
            poisonerKillButton.EffectDuration = Poisoner.delay;
            camouflagerButton.EffectDuration = Camouflager.Duration;
            GrenadierButton.EffectDuration = Grenadier.GrenadeDuration;
            morphlingButton.EffectDuration = Morphling.Duration;
            MimicButton.EffectDuration = Glitch.MimicDuration;
            HitmanMorphButton.EffectDuration = Hitman.MorphDuration;
            lightsOutButton.EffectDuration = Trickster.lightsOutDuration;
            arsonistButton.EffectDuration = Arsonist.Duration;
            mediumButton.EffectDuration = Medium.Duration;
            VeteranAlertButton.EffectDuration = Veteran.Duration;
            WraithButton.EffectDuration = Wraith.Duration;
            PredatorTerminateButton.EffectDuration = Predator.TerminateDuration;
            trackerTrackCorpsesButton.EffectDuration = Tracker.corpsesTrackingDuration;
            witchSpellButton.EffectDuration = Witch.spellCastingDuration;
            VigilanteCamButton.EffectDuration = Vigilante.Duration;
            // Already set the timer to the max, as the button is enabled during the game and not available at the start
            lightsOutButton.Timer = lightsOutButton.MaxTimer;
            zoomOutButton.MaxTimer = 0f;
        }

        private static void AddReplacementHackedButton(CustomButton button, Vector3? positionOffset = null, Func<bool> couldUse = null)
        {
            Vector3 positionOffsetValue = positionOffset ?? button.PositionOffset;  // For non custom buttons, we can set these manually.
            positionOffsetValue.z = -0.1f;
            couldUse = couldUse ?? button.CouldUse;
            CustomButton replacementHackedButton = new CustomButton(() => { }, () => { return true; }, couldUse, () => { }, Glitch.GetHackedButtonSprite(), positionOffsetValue, button.hudManager, button.hotkey,
                true, Glitch.HackDuration, () => { }, button.mirror);
            replacementHackedButton.Timer = replacementHackedButton.EffectDuration;
            replacementHackedButton.actionButton.cooldownTimerText.color = new Color(0F, 0.8F, 0F);
            replacementHackedButton.isEffectActive = true;
            if (PlayerHackedButtons.ContainsKey(PlayerControl.LocalPlayer.PlayerId))
                PlayerHackedButtons[PlayerControl.LocalPlayer.PlayerId].Add(replacementHackedButton);
            else
                PlayerHackedButtons.Add(PlayerControl.LocalPlayer.PlayerId, new List<CustomButton> { replacementHackedButton });
        }
        
        // Disables / Enables all Buttons (except the ones disabled in the Glitch class), and replaces them with new buttons.
        public static void SetAllButtonsHackedStatus(bool Hacked, bool reset = false)
        {
            if (reset) 
            {
                PlayerHackedButtons = new Dictionary<byte, List<CustomButton>>();
                return;
            }
            if (Hacked && !PlayerHackedButtons.ContainsKey(PlayerControl.LocalPlayer.PlayerId))
            {
                int maxI = CustomButton.buttons.Count;
                for (int i = 0; i < maxI; i++)
                {
                    try
                    {
                        if (CustomButton.buttons[i].HasButton())  // For each custombutton the player has
                        {
                            AddReplacementHackedButton(CustomButton.buttons[i]);  // The new buttons are the only non-Hacked buttons now!
                        }
                        CustomButton.buttons[i].isHacked = true;
                    }
                    catch (NullReferenceException)
                    {
                        System.Console.WriteLine("[WARNING] NullReferenceException from MeetingEndedUpdate().HasButton(), if theres only one warning its fine");  // Note: idk what this is good for, but i copied it from above /gendelo
                    }
                }
                // Non Custom (Vanilla) Buttons. The Originals are disabled / hidden in UpdatePatch.cs already, just need to replace them. Can use any button, as we replace onclick etc anyways.
                // Kill Button if enabled for the Role
                if (FastDestroyableSingleton<HudManager>.Instance.KillButton.isActiveAndEnabled) AddReplacementHackedButton(arsonistButton, CustomButton.ButtonPositions.upperRowRight, couldUse: () => { return FastDestroyableSingleton<HudManager>.Instance.KillButton.currentTarget != null; });
                // Vent Button if enabled
                if (PlayerControl.LocalPlayer.IsVenter()) AddReplacementHackedButton(arsonistButton, CustomButton.ButtonPositions.upperRowCenter, couldUse: () => { return FastDestroyableSingleton<HudManager>.Instance.ImpostorVentButton.currentTarget != null; });
                // Report Button
                AddReplacementHackedButton(arsonistButton, (!PlayerControl.LocalPlayer.Data.Role.IsImpostor) ? new Vector3(-1f, -0.06f, 0): CustomButton.ButtonPositions.lowerRowRight, () => { return FastDestroyableSingleton<HudManager>.Instance.ReportButton.graphic.color == Palette.EnabledColor; });
            }
            else if (!Hacked && PlayerHackedButtons.ContainsKey(PlayerControl.LocalPlayer.PlayerId))  // Reset to original. Disables the replacements, enables the original buttons.
            {
                foreach (CustomButton replacementButton in PlayerHackedButtons[PlayerControl.LocalPlayer.PlayerId])
                {
                    replacementButton.HasButton = () => { return false; };
                    replacementButton.Update(); // To make it disappear properly.
                    CustomButton.buttons.Remove(replacementButton);
                }
                PlayerHackedButtons.Remove(PlayerControl.LocalPlayer.PlayerId);

                foreach (CustomButton button in CustomButton.buttons)
                {
                    button.isHacked = false;
                }
            }
        }

        private static void SetButtonTargetDisplay(PlayerControl target, CustomButton button = null, Vector3? offset=null) {
            if (target == null || button == null) 
            {
                if (targetDisplay != null) {  // Reset the poolable player
                    targetDisplay.gameObject.SetActive(false);
                    GameObject.Destroy(targetDisplay.gameObject);
                    targetDisplay = null;
                }
                return;
            }
            // Add poolable player to the button so that the target outfit is shown
            button.actionButton.cooldownTimerText.transform.localPosition = new Vector3(0, 0, -1f);  // Before the poolable player
            targetDisplay = UnityEngine.Object.Instantiate<PoolablePlayer>(Patches.IntroCutsceneOnDestroyPatch.playerPrefab, button.actionButton.transform);
            NetworkedPlayerInfo data = target.Data;
            target.SetPlayerMaterialColors(targetDisplay.cosmetics.currentBodySprite.BodySprite);
            targetDisplay.SetSkin(data.DefaultOutfit.SkinId, data.DefaultOutfit.ColorId);
            targetDisplay.SetHat(data.DefaultOutfit.HatId, data.DefaultOutfit.ColorId);
            targetDisplay.cosmetics.nameText.text = "";  // Hide the name!
            targetDisplay.transform.localPosition = new Vector3(0f, 0.22f, -0.01f);
            if (offset != null) targetDisplay.transform.localPosition += (Vector3)offset;
            targetDisplay.transform.localScale = Vector3.one * 0.33f;
            targetDisplay.SetSemiTransparent(false);
            targetDisplay.gameObject.SetActive(true);
        }

        public static void Postfix(HudManager __instance) 
        {
            initialized = false;

            try 
            {
                CreateButtonsPostfix(__instance);
            } 
            catch { }
        }
         
        public static void CreateButtonsPostfix(HudManager __instance) 
        {
            // get map id, or raise error to wait...
            var mapId = GameOptionsManager.Instance.currentNormalGameOptions.MapId;

            // Engineer Repair
            engineerRepairButton = new CustomButton(
                () => 
                {
                    engineerRepairButton.Timer = 0f;
                    Utils.StartRPC(CustomRPC.EngineerUsedRepair);
                    RPCProcedure.EngineerUsedRepair();
                    SoundEffectsManager.Play("engineerRepair");
                    foreach (PlayerTask task in PlayerControl.LocalPlayer.myTasks.GetFastEnumerator()) 
                    {
                        if (task.TaskType == TaskTypes.FixLights) 
                        {
                            Utils.StartRPC(CustomRPC.EngineerFixLights);
                            RPCProcedure.EngineerFixLights();
                        }
                        else if (task.TaskType == TaskTypes.RestoreOxy) 
                        {
                            MapUtilities.CachedShipStatus.RpcRepairSystem(SystemTypes.LifeSupp, 0 | 64);
                            MapUtilities.CachedShipStatus.RpcRepairSystem(SystemTypes.LifeSupp, 1 | 64);
                        } 
                        else if (task.TaskType == TaskTypes.ResetReactor) 
                        {
                            MapUtilities.CachedShipStatus.RpcRepairSystem(SystemTypes.Reactor, 16);
                        } 
                        else if (task.TaskType == TaskTypes.ResetSeismic) 
                        {
                            MapUtilities.CachedShipStatus.RpcRepairSystem(SystemTypes.Laboratory, 16);
                        } 
                        else if (task.TaskType == TaskTypes.FixComms) 
                        {
                            MapUtilities.CachedShipStatus.RpcRepairSystem(SystemTypes.Comms, 16 | 0);
                            MapUtilities.CachedShipStatus.RpcRepairSystem(SystemTypes.Comms, 16 | 1);
                        } 
                        else if (task.TaskType == TaskTypes.StopCharles) 
                        {
                            MapUtilities.CachedShipStatus.RpcRepairSystem(SystemTypes.Reactor, 0 | 16);
                            MapUtilities.CachedShipStatus.RpcRepairSystem(SystemTypes.Reactor, 1 | 16);
                        } 
                        else if (SubmergedCompatibility.IsSubmerged && task.TaskType == SubmergedCompatibility.RetrieveOxygenMask) 
                        {
                            Utils.StartRPC(CustomRPC.EngineerFixSubmergedOxygen);
                            RPCProcedure.EngineerFixSubmergedOxygen();
                        }

                    }
                },
                () => { return Engineer.Player != null && Engineer.Player == PlayerControl.LocalPlayer && Engineer.remainingFixes > 0 && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => {
                    bool sabotageActive = false;
                    foreach (PlayerTask task in PlayerControl.LocalPlayer.myTasks.GetFastEnumerator())
                        if (task.TaskType == TaskTypes.FixLights || task.TaskType == TaskTypes.RestoreOxy || task.TaskType == TaskTypes.ResetReactor || task.TaskType == TaskTypes.ResetSeismic || task.TaskType == TaskTypes.FixComms || task.TaskType == TaskTypes.StopCharles
                            || SubmergedCompatibility.IsSubmerged && task.TaskType == SubmergedCompatibility.RetrieveOxygenMask)
                            sabotageActive = true;
                    return sabotageActive && Engineer.remainingFixes > 0 && PlayerControl.LocalPlayer.CanMove;
                },
                () => {},
                Engineer.GetButtonSprite(),
                CustomButton.ButtonPositions.upperRowRight,
                __instance,
                KeyCode.F,
                buttonText: "REPAIR"
            );

            // Sheriff Kill
            sheriffKillButton = new CustomButton(
                () => 
                {
                    if (Sheriff.CurrentTarget.CheckVeteranPestilenceKill()) return;

                    MurderAttemptResult murderAttemptResult = Utils.CheckMuderAttempt(Sheriff.Player, Sheriff.CurrentTarget);
                    if (murderAttemptResult == MurderAttemptResult.SuppressKill) return;

                    if (murderAttemptResult == MurderAttemptResult.PerformKill) 
                    {
                        byte targetId = 0;
                        if ((Sheriff.CurrentTarget.Data.Role.IsImpostor && (Sheriff.CurrentTarget != Mini.Player || Mini.IsGrownUp)) 
                        || Sheriff.CurrentTarget.IsNeutralKiller() ||
                            (Sheriff.spyCanDieToSheriff && Spy.Player == Sheriff.CurrentTarget) ||
                            (Sheriff.canKillNeutrals && Sheriff.CurrentTarget.IsNeutral())) 
                        {
                            targetId = Sheriff.CurrentTarget.PlayerId;
                        }
                        else 
                        {
                            targetId = PlayerControl.LocalPlayer.PlayerId;
                        }

                        // Armored sheriff shot doesnt kill if backfired
                        if (targetId == Sheriff.Player.PlayerId && Utils.CheckArmored(Sheriff.Player, true, true))
                            return;
                        Utils.StartRPC(CustomRPC.UncheckedMurderPlayer, Sheriff.Player.Data.PlayerId, targetId, byte.MaxValue);
                        RPCProcedure.UncheckedMurderPlayer(Sheriff.Player.Data.PlayerId, targetId, Byte.MaxValue);
                    }

                    sheriffKillButton.Timer = sheriffKillButton.MaxTimer;
                    Sheriff.CurrentTarget = null;
                },
                () => { return Sheriff.Player != null && Sheriff.Player == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => { return Sheriff.CurrentTarget && PlayerControl.LocalPlayer.CanMove; },
                () => { sheriffKillButton.Timer = sheriffKillButton.MaxTimer;},
                __instance.KillButton.graphic.sprite,
                CustomButton.ButtonPositions.lowerRowRight,
                __instance,
                KeyCode.Q
            );

            UndertakerButton = new CustomButton(
            OnClick: () =>
            {
                if (Undertaker.CurrentTarget == null)
                {
                    foreach (Collider2D collider2D in Physics2D.OverlapCircleAll(PlayerControl.LocalPlayer.GetTruePosition(), PlayerControl.LocalPlayer.MaxReportDistance, Constants.PlayersOnlyMask))
                    {
                        if (collider2D.tag == "DeadBody")
                        {
                            DeadBody deadBody = collider2D.GetComponent<DeadBody>();
                            if (deadBody && !deadBody.Reported)
                            {
                                Vector2 playerPosition = PlayerControl.LocalPlayer.GetTruePosition();
                                Vector2 deadBodyPosition = deadBody.TruePosition;
                                if (Vector2.Distance(deadBodyPosition, playerPosition) <= PlayerControl.LocalPlayer.MaxReportDistance && PlayerControl.LocalPlayer.CanMove && !PhysicsHelpers.AnythingBetween(playerPosition, deadBodyPosition, Constants.ShipAndObjectsMask, false) && Undertaker.CurrentTarget == null)
                                {
                                    NetworkedPlayerInfo playerInfo = GameData.Instance.GetPlayerById(deadBody.ParentId);
                                    if (playerInfo == null) continue;

                                    // Drag the body
                                    Utils.StartRPC(CustomRPC.DragBody, playerInfo.PlayerId);
                                    RPCProcedure.DragBody(playerInfo.PlayerId);
                                    Undertaker.CurrentTarget = deadBody;
                                    SoundEffectsManager.Play("cleanerClean");
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                   UndertakerButton.Timer = UndertakerButton.MaxTimer;

                   byte playerId = PlayerControl.LocalPlayer.PlayerId;

                    var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.DropBody, SendOption.Reliable, -1);
                    writer.Write(playerId);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    RPCProcedure.DropBody(playerId);
                }
            },
            HasButton: () => 
                {
                    if (Undertaker.CurrentTarget != null)
                    {
                        return FastDestroyableSingleton<HudManager>.Instance.ReportButton.graphic.color == Palette.EnabledColor && Undertaker.Player != null  && Undertaker.Player == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead;
                    }
                    return Undertaker.Player != null  && Undertaker.Player == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead;
                },
            CouldUse: () =>
            {
                if (Undertaker.CurrentTarget != null) UndertakerButton.actionButton.graphic.sprite = Undertaker.GetSecondButtonSprite();
                if (Undertaker.CurrentTarget != null) return true;
                else
                {
                    foreach (Collider2D collider2D in Physics2D.OverlapCircleAll(PlayerControl.LocalPlayer.GetTruePosition(), PlayerControl.LocalPlayer.MaxReportDistance, Constants.PlayersOnlyMask))
                    {
                        if (collider2D.tag == "DeadBody")
                        {
                            DeadBody deadBody = collider2D.GetComponent<DeadBody>();
                            Vector2 deadBodyPosition = deadBody.TruePosition;
                            deadBodyPosition.x -= 0.2f;
                            deadBodyPosition.y -= 0.2f;
                            return PlayerControl.LocalPlayer.CanMove && Vector2.Distance(PlayerControl.LocalPlayer.GetTruePosition(), deadBodyPosition) < 0.80f;
                        }
                    }
                    return false;
                }

            },
            OnMeetingEnds: () => { UndertakerButton.Timer = UndertakerButton.MaxTimer;  },
            Sprite: Undertaker.GetFirstButtonSprite(),
            PositionOffset: CustomButton.ButtonPositions.upperRowLeft,
            hudManager: __instance,
            hotkey: KeyCode.F
        );

            HitmanDragButton = new CustomButton(
            OnClick: () =>
            {
                if (Hitman.BodyTarget == null)
                {
                    foreach (Collider2D collider2D in Physics2D.OverlapCircleAll(PlayerControl.LocalPlayer.GetTruePosition(), PlayerControl.LocalPlayer.MaxReportDistance, Constants.PlayersOnlyMask))
                    {
                        if (collider2D.tag == "DeadBody")
                        {
                            DeadBody deadBody = collider2D.GetComponent<DeadBody>();
                            if (deadBody && !deadBody.Reported)
                            {
                                Vector2 playerPosition = PlayerControl.LocalPlayer.GetTruePosition();
                                Vector2 deadBodyPosition = deadBody.TruePosition;
                                if (Vector2.Distance(deadBodyPosition, playerPosition) <= PlayerControl.LocalPlayer.MaxReportDistance && PlayerControl.LocalPlayer.CanMove && !PhysicsHelpers.AnythingBetween(playerPosition, deadBodyPosition, Constants.ShipAndObjectsMask, false) && Hitman.BodyTarget == null)
                                {
                                    NetworkedPlayerInfo playerInfo = GameData.Instance.GetPlayerById(deadBody.ParentId);
                                    if (playerInfo == null) continue;

                                    // Drag the body
                                    Utils.StartRPC(CustomRPC.HitmanDragBody, playerInfo.PlayerId);
                                    RPCProcedure.HitmanDragBody(playerInfo.PlayerId);
                                    Hitman.BodyTarget = deadBody;
                                    SoundEffectsManager.Play("cleanerClean");
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                   HitmanDragButton.Timer = HitmanDragButton.MaxTimer;

                   byte playerId = PlayerControl.LocalPlayer.PlayerId;

                    var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.HitmanDropBody, SendOption.Reliable, -1);
                    writer.Write(playerId);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    RPCProcedure.HitmanDropBody(playerId);
                }
            },
            HasButton: () => 
                {
                    if (Hitman.BodyTarget != null)
                    {
                        return FastDestroyableSingleton<HudManager>.Instance.ReportButton.graphic.color == Palette.EnabledColor && Hitman.Player != null  && Hitman.Player == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead;
                    }
                    
                    return Hitman.Player != null  && Hitman.Player == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead;
                },
            CouldUse: () =>
            {
                if (Hitman.BodyTarget != null) HitmanDragButton.actionButton.graphic.sprite = Hitman.GetDropButtonSprite();
                if (Hitman.BodyTarget != null) return true;
                else
                {
                    foreach (Collider2D collider2D in Physics2D.OverlapCircleAll(PlayerControl.LocalPlayer.GetTruePosition(), PlayerControl.LocalPlayer.MaxReportDistance, Constants.PlayersOnlyMask))
                    {
                        if (collider2D.tag == "DeadBody")
                        {
                            DeadBody deadBody = collider2D.GetComponent<DeadBody>();
                            Vector2 deadBodyPosition = deadBody.TruePosition;
                            deadBodyPosition.x -= 0.2f;
                            deadBodyPosition.y -= 0.2f;
                            return PlayerControl.LocalPlayer.CanMove && Vector2.Distance(PlayerControl.LocalPlayer.GetTruePosition(), deadBodyPosition) < 0.80f;
                        }
                    }
                    return false;
                }

            },
            OnMeetingEnds: () => { HitmanDragButton.Timer = HitmanDragButton.MaxTimer;  },
            Sprite: Hitman.GetDragButtonSprite(),
            PositionOffset: CustomButton.ButtonPositions.upperRowCenter,
            hudManager: __instance,
            hotkey: KeyCode.G
        );
        
            // Blackmailer blackmail
            BlackmailButton = new CustomButton(
                () => 
                {
                    if (Blackmailer.CurrentTarget.CheckVeteranPestilenceKill() || Blackmailer.CurrentTarget.CheckFortifiedPlayer()) return;

                    Utils.StartRPC(CustomRPC.BlackmailerBlackmail, Blackmailer.CurrentTarget.PlayerId);
                    RPCProcedure.BlackmailerBlackmail(Blackmailer.CurrentTarget.PlayerId);
                    Blackmailer.CurrentTarget = null;
                    BlackmailButton.Timer = BlackmailButton.MaxTimer;

                    SoundEffectsManager.Play("Blackmail");
                },
                () => 
                { return Blackmailer.Player != null && Blackmailer.Player == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => 
                {
                    return Blackmailer.Player != null && Blackmailer.Player == PlayerControl.LocalPlayer && Blackmailer.CurrentTarget && PlayerControl.LocalPlayer.CanMove;
                },
                () => { BlackmailButton.Timer = BlackmailButton.MaxTimer; },
                Blackmailer.GetButtonSprite(),
                CustomButton.ButtonPositions.upperRowLeft,
                __instance,
                KeyCode.F,
                buttonText: "BLACKMAIL"
            );

            // Glitch Hack
            GlitchHackButton = new CustomButton(
                () => 
                {
                    if (Glitch.CurrentTarget.CheckVeteranPestilenceKill() || Glitch.CurrentTarget.CheckFortifiedPlayer()) return;

                    Utils.StartRPC(CustomRPC.GlitchUsedHacks, Glitch.CurrentTarget.PlayerId);
                    RPCProcedure.GlitchUsedHacks(Glitch.CurrentTarget.PlayerId);
                    Glitch.CurrentTarget = null;
                    GlitchHackButton.Timer = GlitchHackButton.MaxTimer;

                    SoundEffectsManager.Play("glitchHack");
                },
                () => 
                { return Glitch.Player != null && Glitch.Player == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => 
                {
                    if (GlitchButtonHacksText != null) GlitchButtonHacksText.text = $"{Glitch.remainingHacks}";
                    return Glitch.Player != null && Glitch.Player == PlayerControl.LocalPlayer && Glitch.CurrentTarget && Glitch.remainingHacks > 0 && PlayerControl.LocalPlayer.CanMove;
                },
                () => { GlitchHackButton.Timer = GlitchHackButton.MaxTimer; },
                Glitch.GetButtonSprite(),
                CustomButton.ButtonPositions.upperRowCenter,
                __instance,
                KeyCode.F
            );
            // Glitch Hack button Hack counter
            GlitchButtonHacksText = GameObject.Instantiate(GlitchHackButton.actionButton.cooldownTimerText, GlitchHackButton.actionButton.cooldownTimerText.transform.parent);
            GlitchButtonHacksText.text = "";
            GlitchButtonHacksText.enableWordWrapping = false;
            GlitchButtonHacksText.transform.localScale = Vector3.one * 0.5f;
            GlitchButtonHacksText.transform.localPosition += new Vector3(-0.05f, 0.7f, 0);

            // Time Master Rewind Time
            TimeMasterRewindButton = new CustomButton(
                OnClick: () => 
                {
                    Utils.StartRPC(CustomRPC.TimeMasterRewindTime);
                    RPCProcedure.TimeMasterRewindTime();
                    SoundEffectsManager.Play("timemasterShield");
                },
                HasButton: () => { return TimeMaster.Player != null && TimeMaster.Player == PlayerControl.LocalPlayer && TimeMaster.Charges > 0 && !PlayerControl.LocalPlayer.Data.IsDead; },
                CouldUse: () => 
                { 
                    if (TimeMasterChargesText != null) TimeMasterChargesText.text = $"{TimeMaster.Charges}";
                    return PlayerControl.LocalPlayer.CanMove && TimeMaster.Charges > 0; 
                },
                OnMeetingEnds: () => 
                {
                    TimeMasterRewindButton.Timer = TimeMasterRewindButton.MaxTimer;
                    TimeMasterRewindButton.isEffectActive = false;
                    TimeMasterRewindButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
                },
                Sprite: TimeMaster.GetButtonSprite(),
                PositionOffset: CustomButton.ButtonPositions.lowerRowRight,
                hudManager: __instance,
                hotkey: KeyCode.F, 
                HasEffect: true,
                EffectDuration: TimeMaster.RewindTimeDuration,
                OnEffectEnds: () => 
                {
                    TimeMasterRewindButton.Timer = TimeMasterRewindButton.MaxTimer;
                    SoundEffectsManager.Stop("timemasterShield");
                },
                buttonText: "REWIND TIME"
            );
                // Time Master Charges counter
                TimeMasterChargesText = GameObject.Instantiate(TimeMasterRewindButton.actionButton.cooldownTimerText, TimeMasterRewindButton.actionButton.cooldownTimerText.transform.parent);
                TimeMasterChargesText.text = "";
                TimeMasterChargesText.enableWordWrapping = false;
                TimeMasterChargesText.transform.localScale = Vector3.one * 0.5f;
                TimeMasterChargesText.transform.localPosition += new Vector3(-0.05f, 0.7f, 0);

                // Predator Terminate
                PredatorTerminateButton = new CustomButton(
                () => 
                { 
                    Predator.Terminating = true; 
                    Predator.HasImpostorVision = true; 
                    PredatorKillButton.Timer = 0f; 
                },
                () => 
                { 
                    return Predator.Player != null && Predator.Player == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => 
                {
                    return PlayerControl.LocalPlayer.CanMove; },
                () =>
                {
                    PredatorTerminateButton.Timer = PredatorTerminateButton.MaxTimer;
                    PredatorTerminateButton.isEffectActive = false;
                    PredatorTerminateButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
                    Predator.Terminating = false;
                    Predator.HasImpostorVision = false;

                },
                Predator.GetButtonSprite(),
                CustomButton.ButtonPositions.upperRowCenter,
                __instance,
                KeyCode.F,
               true,
                Predator.TerminateDuration,
                () => 
                { 
                    PredatorTerminateButton.Timer = PredatorTerminateButton.MaxTimer; 
                    Predator.Terminating = false; 
                    Predator.HasImpostorVision = false; 
                },
                buttonText: "TERMINATE"
            );

             // Predator Kill
            PredatorKillButton = new CustomButton(
                () => 
                {
                    if (Predator.CurrentTarget.CheckVeteranPestilenceKill()) return;
                    if (Utils.CheckMurderAttemptAndKill(Predator.Player, Predator.CurrentTarget) == MurderAttemptResult.SuppressKill) return;

                    PredatorKillButton.Timer = PredatorKillButton.MaxTimer; 
                    Predator.CurrentTarget = null;
                },
                () => { return Predator.Player != null && Predator.Player == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead && Predator.Terminating; },
                () => { return Predator.CurrentTarget && PlayerControl.LocalPlayer.CanMove; },
                () => { PredatorKillButton.Timer = PredatorKillButton.MaxTimer;},
                __instance.KillButton.graphic.sprite,
                CustomButton.ButtonPositions.upperRowRight,
                __instance,
                KeyCode.Q
            );
            
            // Wraith vanish
            WraithButton = new CustomButton(
                () => 
                {
                    Utils.StartRPC(CustomRPC.SetVanish, Wraith.Player.PlayerId, byte.MinValue);
                    RPCProcedure.SetVanish(Wraith.Player.PlayerId, byte.MinValue);
                    
                    SoundEffectsManager.Play("VanillaPhantomSoundFirst");
                },
                () => { return Wraith.Player != null && Wraith.Player == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () =>
                {
                    return Wraith.Player != null && Wraith.Player == PlayerControl.LocalPlayer && PlayerControl.LocalPlayer.CanMove;
                },
                () => 
                {
                    WraithButton.Timer = WraithButton.MaxTimer;
                    WraithButton.isEffectActive = false;
                    WraithButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
                },
                Wraith.GetButtonSprite(),
                CustomButton.ButtonPositions.upperRowCenter,
                __instance,
                KeyCode.F, 
                true,
                Wraith.Duration,
                () => 
                {
                    WraithButton.Timer = WraithButton.MaxTimer;
                    SoundEffectsManager.Play("VanillaPhantomSoundSecond");
                },
                buttonText: "Vanish"
            );

            // Veteran Alert
            VeteranAlertButton = new CustomButton(
                () => 
                {
                    Utils.StartRPC(CustomRPC.VeteranAlert);
                    RPCProcedure.VeteranAlert();
                    SoundEffectsManager.Play("warlockCurse");
                },
                () => { return Veteran.Player != null && Veteran.Player == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => 
                {
                    if (VeteranChargesText != null) VeteranChargesText.text = $"{Veteran.Charges}";
                    return Veteran.Player != null && Veteran.Player == PlayerControl.LocalPlayer && Veteran.Charges > 0 && PlayerControl.LocalPlayer.CanMove;
                },
                () => {
                    VeteranAlertButton.Timer = VeteranAlertButton.MaxTimer;
                    VeteranAlertButton.isEffectActive = false;
                    VeteranAlertButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
                },
                Veteran.GetButtonSprite(),
                CustomButton.ButtonPositions.lowerRowRight,
                __instance,
                KeyCode.F, 
                true,
                Veteran.Duration,
                () => 
                {
                    VeteranAlertButton.Timer = VeteranAlertButton.MaxTimer;
                    SoundEffectsManager.Stop("warlockCurse");
                },
                buttonText: "ALERT"
            );
            // Veteran Alert button charge counter
            VeteranChargesText = GameObject.Instantiate(VeteranAlertButton.actionButton.cooldownTimerText, VeteranAlertButton.actionButton.cooldownTimerText.transform.parent);
            VeteranChargesText.text = "";
            VeteranChargesText.enableWordWrapping = false;
            VeteranChargesText.transform.localScale = Vector3.one * 0.5f;
            VeteranChargesText.transform.localPosition += new Vector3(-0.05f, 0.7f, 0);

            // Glitch Kill
            GlitchKillButton = new CustomButton(
                () => 
                {
                    if (Glitch.CurrentTarget.CheckVeteranPestilenceKill()) return;

                    if (Utils.CheckMurderAttemptAndKill(Glitch.Player, Glitch.CurrentTarget) == MurderAttemptResult.SuppressKill) return;

                    GlitchKillButton.Timer = GlitchKillButton.MaxTimer; 
                    Glitch.CurrentTarget = null;
                },
                () => { return Glitch.Player != null && Glitch.Player == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => { return Glitch.CurrentTarget && PlayerControl.LocalPlayer.CanMove; },
                () => { GlitchKillButton.Timer = GlitchKillButton.MaxTimer;},
                __instance.KillButton.graphic.sprite,
                CustomButton.ButtonPositions.upperRowRight,
                __instance,
                KeyCode.Q
            );

            // Juggernaut Kill
            JuggernautKillButton = new CustomButton(
                () =>
                {
                    if (Juggernaut.CurrentTarget.CheckVeteranPestilenceKill()) return;

                    if (Utils.CheckMurderAttemptAndKill(Juggernaut.Player, Juggernaut.CurrentTarget) == MurderAttemptResult.SuppressKill) return;
                    if (JuggernautKillButton.MaxTimer >= 0f)
                    {
                        Juggernaut.FixCooldown();
                        JuggernautKillButton.MaxTimer = Juggernaut.Cooldown;
                    }

                    JuggernautKillButton.Timer = JuggernautKillButton.MaxTimer;
                    Juggernaut.CurrentTarget = null;
                },
                () =>
                {
                    return Juggernaut.Player != null && Juggernaut.Player == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () =>
                {
                    return Juggernaut.CurrentTarget && PlayerControl.LocalPlayer.CanMove;
                },
                () => { JuggernautKillButton.Timer = JuggernautKillButton.MaxTimer; },
                __instance.KillButton.graphic.sprite,
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );

            //Romantic
            RomanticKillButton = new CustomButton(
                () => 
                {
                    if (VengefulRomantic.CurrentTarget.CheckVeteranPestilenceKill()) return;

                    if (Utils.CheckMurderAttemptAndKill(VengefulRomantic.Player, VengefulRomantic.CurrentTarget) == MurderAttemptResult.SuppressKill) return;

                    RomanticKillButton.Timer = RomanticKillButton.MaxTimer; 
                    VengefulRomantic.CurrentTarget = null;
                },
                () => { return VengefulRomantic.Player != null && VengefulRomantic.Player == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => { return VengefulRomantic.CurrentTarget && PlayerControl.LocalPlayer.CanMove; },
                () => { RomanticKillButton.Timer = RomanticKillButton.MaxTimer;},
                __instance.KillButton.graphic.sprite,
                CustomButton.ButtonPositions.upperRowRight,
                __instance,
                KeyCode.Q
            );

            // Werewolf Maul Kill
            WerewolfMaulButton = new CustomButton(
                () => 
                {
                    if (Werewolf.CurrentTarget.CheckVeteranPestilenceKill()) return;

                    if (Utils.CheckMurderAttemptAndKill(Werewolf.Player, Werewolf.CurrentTarget) == MurderAttemptResult.SuppressKill) return;
                    
                    Utils.StartRPC(CustomRPC.WerewolfMaul);
                    RPCProcedure.WerewolfMaul();
                    WerewolfMaulButton.Timer = WerewolfMaulButton.MaxTimer;
                    Werewolf.CurrentTarget = null;
                },
                () => { return Werewolf.Player != null && Werewolf.Player == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => { return Werewolf.CurrentTarget && PlayerControl.LocalPlayer.CanMove; },
                () => { WerewolfMaulButton.Timer = WerewolfMaulButton.MaxTimer;},
                Werewolf.GetButtonSprite(),
                CustomButton.ButtonPositions.upperRowRight,
                __instance,
                KeyCode.Q,
                buttonText: "MAUL"
            );

            // Hitman morph
            HitmanMorphButton = new CustomButton(
                () => 
                {
                    if (Hitman.SampledTarget != null) 
                    {
                        Utils.StartRPC(CustomRPC.HitmanMorph, Hitman.SampledTarget.PlayerId);
                        RPCProcedure.HitmanMorph(Hitman.SampledTarget.PlayerId);
                        Hitman.SampledTarget = null;
                        HitmanMorphButton.EffectDuration = Hitman.MorphDuration;
                        HitmanMorphButton.buttonText = "SAMPLE";
                        SoundEffectsManager.Play("morphlingMorph");
                    }
                    else if (Hitman.CurrentTarget != null) 
                    {
                        Hitman.SampledTarget = Hitman.CurrentTarget;
                        HitmanMorphButton.Sprite = Hitman.GetMorphSprite();
                        HitmanMorphButton.EffectDuration = 1f;
                        HitmanMorphButton.buttonText = "MORPH";
                        SoundEffectsManager.Play("morphlingSample");

                        // Add poolable player to the button so that the target outfit is shown
                        SetButtonTargetDisplay(Hitman.SampledTarget, HitmanMorphButton);
                    }
                },
                () => { return Hitman.Player != null && Hitman.Player == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => { return (Hitman.CurrentTarget || Hitman.SampledTarget) && PlayerControl.LocalPlayer.CanMove && !Utils.MushroomSabotageActive(); },
                () => { 
                    HitmanMorphButton.Timer = MimicButton.MaxTimer;
                    HitmanMorphButton.Sprite = Hitman.GetSampleSprite();
                    HitmanMorphButton.isEffectActive = false;
                    HitmanMorphButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
                    Hitman.SampledTarget = null;
                    HitmanMorphButton.buttonText = "SAMPLE";
                    SetButtonTargetDisplay(null);
                },
                Hitman.GetSampleSprite(),
                CustomButton.ButtonPositions.upperRowLeft,
                __instance,
                KeyCode.F,
                true,
                Hitman.MorphDuration,
                () => 
                {
                    if (Hitman.SampledTarget == null) 
                    {
                        HitmanMorphButton.Timer = HitmanMorphButton.MaxTimer;
                        HitmanMorphButton.Sprite = Hitman.GetSampleSprite();
                        SoundEffectsManager.Play("morphlingMorph");

                        // Reset the poolable player
                        SetButtonTargetDisplay(null);
                    }
                },
                buttonText: "SAMPLE"
            );

            // Glitch mimic
            MimicButton = new CustomButton(
                () => 
                {
                    if (Glitch.sampledTarget != null) 
                    {
                        Utils.StartRPC(CustomRPC.GlitchMimic, Glitch.sampledTarget.PlayerId);
                        RPCProcedure.GlitchMimic(Glitch.sampledTarget.PlayerId);
                        Glitch.sampledTarget = null;
                        MimicButton.EffectDuration = Glitch.MimicDuration;
                        MimicButton.buttonText = "SAMPLE";
                        SoundEffectsManager.Play("morphlingMorph");
                    }
                    else if (Glitch.CurrentTarget != null) 
                    {
                        Glitch.sampledTarget = Glitch.CurrentTarget;
                        MimicButton.Sprite = Glitch.GetMimicSprite();
                        MimicButton.EffectDuration = 1f;
                        SoundEffectsManager.Play("morphlingSample");
                        MimicButton.buttonText = "MORPH";
                        // Add poolable player to the button so that the target outfit is shown
                        SetButtonTargetDisplay(Glitch.sampledTarget, MimicButton);
                    }
                },
                () => { return Glitch.Player != null && Glitch.Player == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => { return (Glitch.CurrentTarget || Glitch.sampledTarget) && PlayerControl.LocalPlayer.CanMove && !Utils.MushroomSabotageActive(); },
                () => 
                { 
                    MimicButton.Timer = MimicButton.MaxTimer;
                    MimicButton.Sprite = Glitch.GetSampleSprite();
                    MimicButton.isEffectActive = false;
                    MimicButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
                    Glitch.sampledTarget = null;
                    MimicButton.buttonText = "SAMPLE";
                    SetButtonTargetDisplay(null);
                },
                Glitch.GetSampleSprite(),
                CustomButton.ButtonPositions.upperRowLeft,
                __instance,
                KeyCode.G,
                true,
                Glitch.MimicDuration,
                () => 
                {
                    if (Glitch.sampledTarget == null) 
                    {
                        MimicButton.Timer = MimicButton.MaxTimer;
                        MimicButton.Sprite = Glitch.GetSampleSprite();
                        SoundEffectsManager.Play("morphlingMorph");

                        // Reset the poolable player
                        SetButtonTargetDisplay(null);
                    }
                },
                buttonText: "SAMPLE"
            );

            // Medic Shield
            medicShieldButton = new CustomButton(
                () => 
                {
                    medicShieldButton.Timer = 0f;
    
                    Utils.StartRPC(Medic.setShieldAfterMeeting ? CustomRPC.SetFutureShielded : CustomRPC.MedicSetShielded, Medic.CurrentTarget.PlayerId);
                    if (Medic.setShieldAfterMeeting)
                        RPCProcedure.SetFutureShielded(Medic.CurrentTarget.PlayerId);
                    else
                        RPCProcedure.MedicSetShielded(Medic.CurrentTarget.PlayerId);
                    Medic.meetingAfterShielding = false;

                    SoundEffectsManager.Play("medicShield");
                    },
                () => { return Medic.Player != null && Medic.Player == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => { return !Medic.usedShield && Medic.CurrentTarget && PlayerControl.LocalPlayer.CanMove; },
                () => {},
                Medic.GetButtonSprite(),
                CustomButton.ButtonPositions.lowerRowRight,
                __instance,
                KeyCode.F,
                buttonText: "SHIELD"
            );

            // Romantic Romance
            RomanticSetTargetButton = new CustomButton
            (
                () => 
                {
                    RomanticSetTargetButton.Timer = 0f;

                    Utils.StartRPC(CustomRPC.RomanticSetBeloved, Romantic.CurrentTarget.PlayerId);
                    
                    RPCProcedure.RomanticSetBeloved(Romantic.CurrentTarget.PlayerId);

                    SoundEffectsManager.Play("medicShield");
                    },
                () => { return Romantic.Player != null && !Romantic.HasLover && Romantic.Player == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => { return !Romantic.HasLover && Romantic.CurrentTarget && PlayerControl.LocalPlayer.CanMove; },
                () => {},
                Romantic.GetButtonSprite(),
                CustomButton.ButtonPositions.lowerRowRight,
                __instance,
                KeyCode.F,
                buttonText: "PICK LOVER"
            );

            // Morphling morph
            
            morphlingButton = new CustomButton(
                () => 
                {
                    if (Morphling.sampledTarget != null) 
                    {
                        Utils.StartRPC(CustomRPC.MorphlingMorph, Morphling.sampledTarget.PlayerId);
                        RPCProcedure.MorphlingMorph(Morphling.sampledTarget.PlayerId);
                        Morphling.sampledTarget = null;
                        morphlingButton.EffectDuration = Morphling.Duration;
                        morphlingButton.buttonText = "SAMPLE";
                        SoundEffectsManager.Play("morphlingMorph");
                    } 
                    else if (Morphling.CurrentTarget != null) 
                    {
                        Morphling.sampledTarget = Morphling.CurrentTarget;
                        morphlingButton.Sprite = Morphling.GetMorphSprite();
                        morphlingButton.EffectDuration = 1f;
                        morphlingButton.buttonText = "MORPH";
                        SoundEffectsManager.Play("morphlingSample");

                        // Add poolable player to the button so that the target outfit is shown
                        SetButtonTargetDisplay(Morphling.sampledTarget, morphlingButton);
                    }
                },
                () => { return Morphling.Player != null && Morphling.Player == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => { return (Morphling.CurrentTarget || Morphling.sampledTarget) && PlayerControl.LocalPlayer.CanMove && !Utils.MushroomSabotageActive(); 
                    },
                () => 
                {
                    morphlingButton.Timer = morphlingButton.MaxTimer;
                    morphlingButton.Sprite = Morphling.GetSampleSprite();
                    morphlingButton.isEffectActive = false;
                    morphlingButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
                    Morphling.sampledTarget = null;
                    morphlingButton.buttonText = "SAMPLE";
                    SetButtonTargetDisplay(null);
                },
                Morphling.GetSampleSprite(),
                CustomButton.ButtonPositions.upperRowLeft,
                __instance,
                KeyCode.F,
                true,
                Morphling.Duration,
                () => 
                {
                    if (Morphling.sampledTarget == null) 
                    {
                        morphlingButton.Timer = morphlingButton.MaxTimer;
                        morphlingButton.Sprite = Morphling.GetSampleSprite();
                        morphlingButton.buttonText = "SAMPLE";
                        SoundEffectsManager.Play("morphlingMorph");
                        // Reset the poolable player
                        SetButtonTargetDisplay(null);
                    }
                },
                buttonText: "SAMPLE"
            );

            // Pestilence Kill
            PestilenceButton = new CustomButton(
                OnClick: () => 
                {
                    if (Utils.CheckMurderAttemptAndKill(Pestilence.Player, Pestilence.CurrentTarget) == MurderAttemptResult.SuppressKill) return;

                    PestilenceButton.Timer = PestilenceButton.MaxTimer; 
                    Pestilence.CurrentTarget = null;
                },
                HasButton: () => { return Pestilence.Player != null && Pestilence.Player == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                CouldUse: () => { return Pestilence.CurrentTarget && PlayerControl.LocalPlayer.CanMove; },
                OnMeetingEnds: () => { PestilenceButton.Timer = PestilenceButton.MaxTimer;},
                Sprite: __instance.KillButton.graphic.sprite,
                PositionOffset: CustomButton.ButtonPositions.upperRowRight,
                hudManager: __instance,
                hotkey: KeyCode.Q
            );

            PlaguebearerButton = new CustomButton(
            OnClick: () => 
            {
                if (Plaguebearer.CurrentTarget != null) 
                {
                    if (Plaguebearer.CurrentTarget.CheckVeteranPestilenceKill() || Plaguebearer.CurrentTarget.CheckFortifiedPlayer()) return;

                    Plaguebearer.InfectTarget = Plaguebearer.CurrentTarget;

                    // Move this before checking CanTransform because otherwise the Plaguebearer has to infect twice the last player
                    if (!Plaguebearer.InfectedPlayers.Contains(Plaguebearer.InfectTarget.PlayerId))
                    {
                        Plaguebearer.InfectedPlayers.Add(Plaguebearer.InfectTarget.PlayerId);
                    }
        
                    SoundEffectsManager.Play("knockKnock");

                    // Now check if everyone is infected
                    if (Plaguebearer.CanTransform()) 
                    {
                        Utils.StartRPC(CustomRPC.TurnPestilence);
                        RPCProcedure.PlaguebearerTurnPestilence();
                    }
                }
            },
            HasButton: () => { return Plaguebearer.Player != null && Plaguebearer.Player == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
            CouldUse: () => 
            {
                return PlayerControl.LocalPlayer.CanMove && Plaguebearer.CurrentTarget != null;
            },
            OnMeetingEnds: () => 
            {
                PlaguebearerButton.Timer = PlaguebearerButton.MaxTimer;
                PlaguebearerButton.isEffectActive = false;
                Plaguebearer.InfectTarget = null;
            },
            Sprite: Plaguebearer.GetButtonSprite(),
            PositionOffset: CustomButton.ButtonPositions.lowerRowRight,
            hudManager:  __instance,
            hotkey: KeyCode.F,
            HasEffect: true,
            EffectDuration: 0f,
            OnEffectEnds: () => 
            {
                PlaguebearerButton.Timer = PlaguebearerButton.MaxTimer;

                foreach (PlayerControl p in Plaguebearer.InfectedPlayers) 
                {
                    if (MapOptions.BeanIcons.ContainsKey(p.PlayerId)) 
                    {
                        MapOptions.BeanIcons[p.PlayerId].SetSemiTransparent(false);
                    }
                }

                // Ghost Info
                Utils.StartRPC(CustomRPC.ShareGhostInfo, PlayerControl.LocalPlayer.PlayerId, (byte)GhostInfoTypes.PlaguebearerInfect, Plaguebearer.InfectTarget.PlayerId);

                Plaguebearer.InfectTarget = null;
            }
        );

            CrusaderButton = new CustomButton(
            OnClick: () =>
            {
                if (Crusader.CurrentTarget.CheckVeteranPestilenceKill()) return;

                Utils.StartRPC(CustomRPC.Fortify, Crusader.CurrentTarget.PlayerId);
                RPCProcedure.Fortify(Crusader.CurrentTarget.PlayerId);
                SoundEffectsManager.Play("medicShield");

                Crusader.CurrentTarget = null;
                Crusader.Fortified = true;

                CrusaderButton.Timer = CrusaderButton.MaxTimer;
            },
            HasButton: () =>
            {
                return Crusader.Player != null && !Crusader.Fortified && Crusader.Player == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead;
            },
            CouldUse: () => 
                {
                    if (CrusaderChargesText != null) CrusaderChargesText.text = $"{Crusader.Charges}";
                    return Crusader.Player != null && Crusader.Player == PlayerControl.LocalPlayer && Crusader.Charges > 0 && !Crusader.Fortified && Crusader.CurrentTarget != null && PlayerControl.LocalPlayer.CanMove;
                },
            OnMeetingEnds: () => { CrusaderButton.Timer = CrusaderButton.MaxTimer; },
            Sprite: Crusader.GetButtonSprite(),
            PositionOffset: CustomButton.ButtonPositions.lowerRowRight,
            hudManager: __instance,
            hotkey: KeyCode.F,
            HasEffect: true,
            EffectDuration: 0f,
            OnEffectEnds: () =>
            {
                CrusaderButton.Timer = CrusaderButton.MaxTimer;
            },
            mirror: false,
            buttonText: "Fortify"
        );
            CrusaderChargesText = GameObject.Instantiate(CrusaderButton.actionButton.cooldownTimerText, CrusaderButton.actionButton.cooldownTimerText.transform.parent);
            CrusaderChargesText.text = "";
            CrusaderChargesText.enableWordWrapping = false;
            CrusaderChargesText.transform.localScale = Vector3.one * 0.5f;
            CrusaderChargesText.transform.localPosition += new Vector3(-0.05f, 0.7f, 0);

            OracleButton = new CustomButton(
            () =>
            {
                if (Oracle.CurrentTarget.CheckVeteranPestilenceKill() || Oracle.CurrentTarget.CheckFortifiedPlayer()) return;
                
                Utils.StartRPC(CustomRPC.Confess, Oracle.CurrentTarget.PlayerId);
                RPCProcedure.Confess(Oracle.CurrentTarget.PlayerId);

                Oracle.Charges--;
                Oracle.Investigated = true;
                SoundEffectsManager.Play("knockKnock");

                Oracle.CurrentTarget = null;

                OracleButton.Timer = OracleButton.MaxTimer;
            },
            () =>
            {
                return Oracle.Player != null && !Oracle.Investigated && Oracle.Player == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead;
            },
            () => 
                {
                    if (OracleChargesText != null) OracleChargesText.text = $"{Oracle.Charges}";
                    return Oracle.Player != null && Oracle.Player == PlayerControl.LocalPlayer && Oracle.Charges > 0 && !Oracle.Investigated && Oracle.CurrentTarget != null && PlayerControl.LocalPlayer.CanMove;
                },
            () => { OracleButton.Timer = OracleButton.MaxTimer; },
            Oracle.GetButtonSprite(),
            CustomButton.ButtonPositions.lowerRowRight,
            __instance,
            KeyCode.F,
            true,
            0f,
            () =>
            {
                OracleButton.Timer = OracleButton.MaxTimer;
            },
            buttonText: "CONFESS"
        );
            // Oracle button charge counter
            OracleChargesText = GameObject.Instantiate(OracleButton.actionButton.cooldownTimerText, OracleButton.actionButton.cooldownTimerText.transform.parent);
            OracleChargesText.text = "";
            OracleChargesText.enableWordWrapping = false;
            OracleChargesText.transform.localScale = Vector3.one * 0.5f;
            OracleChargesText.transform.localPosition += new Vector3(-0.05f, 0.7f, 0);

            MysticButton = new CustomButton(
            () =>
            {
                if (Mystic.CurrentTarget.CheckVeteranPestilenceKill() || Mystic.CurrentTarget.CheckFortifiedPlayer()) return;

                MysticButton.Timer = MysticButton.MaxTimer;
                SoundEffectsManager.Play("knockKnock");
            },
            () =>
            {
                return Mystic.Player != null && !Mystic.Investigated && Mystic.Player == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead;
            },
           () => 
                {
                    if (MysticChargesText != null) MysticChargesText.text = $"{Mystic.Charges}";
                    return Mystic.Player != null && Mystic.Player == PlayerControl.LocalPlayer && Mystic.Charges > 0 && !Mystic.Investigated && Mystic.CurrentTarget != null && PlayerControl.LocalPlayer.CanMove;
                },
            () => { MysticButton.Timer = MysticButton.MaxTimer; },
            Mystic.GetButtonSprite(),
            CustomButton.ButtonPositions.lowerRowRight,
            __instance,
            KeyCode.F,
            true,
            0f,
            () =>
            {
                MysticButton.Timer = MysticButton.MaxTimer;
                var msg = Mystic.GetInfo(Mystic.CurrentTarget);
                FastDestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, $"{msg}");

                // Ghost Info
                var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                    (byte)CustomRPC.ShareGhostInfo, SendOption.Reliable);
                writer.Write(Mystic.CurrentTarget.PlayerId);
                writer.Write((byte)GhostInfoTypes.MysticInfo);
                writer.Write(msg);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                Mystic.Investigated = true;
            },
            buttonText: "READ MIND"
        );
            // Mystic button charge counter
            MysticChargesText = GameObject.Instantiate(MysticButton.actionButton.cooldownTimerText, MysticButton.actionButton.cooldownTimerText.transform.parent);
            MysticChargesText.text = "";
            MysticChargesText.enableWordWrapping = false;
            MysticChargesText.transform.localScale = Vector3.one * 0.5f;
            MysticChargesText.transform.localPosition += new Vector3(-0.05f, 0.7f, 0);

            // Camouflager camouflage
            camouflagerButton = new CustomButton(
                () => 
                {
                    Utils.StartRPC(CustomRPC.CamouflagerCamouflage);
                    RPCProcedure.CamouflagerCamouflage();
                    SoundEffectsManager.Play("morphlingMorph");
                },
                () => { return Camouflager.Player != null && Camouflager.Player == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => { return PlayerControl.LocalPlayer.CanMove; },
                () => {
                    camouflagerButton.Timer = camouflagerButton.MaxTimer;
                    camouflagerButton.isEffectActive = false;
                    camouflagerButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
                },
                Camouflager.GetButtonSprite(),
                CustomButton.ButtonPositions.upperRowLeft,
                __instance,
                KeyCode.F,
                true,
                Camouflager.Duration,
                () => {
                    camouflagerButton.Timer = camouflagerButton.MaxTimer;
                    SoundEffectsManager.Play("morphlingMorph");
                },
                buttonText: "CAMOUFLAGE"
            );

            // Grenadier Grenade
            GrenadierButton = new CustomButton(
                () => 
                {
                    Utils.StartRPC(CustomRPC.GrenadierFlash);
                    RPCProcedure.GrenadierFlash();
                    Grenadier.Active = true;
                    SoundEffectsManager.Play("grenadierGrenade");
                },
                () => { return Grenadier.Player != null && Grenadier.Player == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => { return PlayerControl.LocalPlayer.CanMove && !ShipStatus.Instance.Systems[SystemTypes.Sabotage].Cast<SabotageSystemType>().AnyActive; },
                () => {
                    GrenadierButton.Timer = GrenadierButton.MaxTimer;
                    GrenadierButton.isEffectActive = false;
                    GrenadierButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
                },
                Grenadier.GetButtonSprite(),
                CustomButton.ButtonPositions.upperRowLeft,
                __instance,
                KeyCode.F,
                true,
                Grenadier.GrenadeDuration,
                () => 
                {
                    GrenadierButton.Timer = GrenadierButton.MaxTimer;
                    Grenadier.Active = false;
                },
                buttonText: "GRENADE"
            );

            // Hacker button
            hackerButton = new CustomButton(
                () => {
                    Hacker.hackerTimer = Hacker.Duration;
                    SoundEffectsManager.Play("hackerHack");
                },
                () => { return Hacker.Player != null && Hacker.Player == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => { return true; },
                () => {
                    hackerButton.Timer = hackerButton.MaxTimer;
                    hackerButton.isEffectActive = false;
                    hackerButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
                },
                Hacker.GetButtonSprite(),
                CustomButton.ButtonPositions.upperRowRight,
                __instance,
                KeyCode.F,
                true,
                0f,
                () => { hackerButton.Timer = hackerButton.MaxTimer;}
            );

            hackerAdminTableButton = new CustomButton(
               () => {
                   if (!MapBehaviour.Instance || !MapBehaviour.Instance.isActiveAndEnabled) {
                       HudManager __instance = FastDestroyableSingleton<HudManager>.Instance;
                       __instance.InitMap();
                       MapBehaviour.Instance.ShowCountOverlay(allowedToMove: true, showLivePlayerPosition: true, includeDeadBodies: true);
                   }
                   if (Hacker.cantMove) PlayerControl.LocalPlayer.moveable = false;
                   PlayerControl.LocalPlayer.NetTransform.Halt(); // Stop current movement 
                   Hacker.chargesAdminTable--;
               },
               () => { return Hacker.Player != null && Hacker.Player == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead;},
               () => {
                   if (hackerAdminTableChargesText != null) hackerAdminTableChargesText.text = $"{Hacker.chargesAdminTable} / {Hacker.toolsNumber}";
                   return Hacker.chargesAdminTable > 0; 
               },
               () => {
                   hackerAdminTableButton.Timer = hackerAdminTableButton.MaxTimer;
                   hackerAdminTableButton.isEffectActive = false;
                   hackerAdminTableButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
               },
               Hacker.GetAdminSprite(),
               CustomButton.ButtonPositions.lowerRowRight,
               __instance,
               KeyCode.G,
               true,
               0f,
               () => { 
                   hackerAdminTableButton.Timer = hackerAdminTableButton.MaxTimer;
                   if (!hackerVitalsButton.isEffectActive) PlayerControl.LocalPlayer.moveable = true;
                   if (MapBehaviour.Instance && MapBehaviour.Instance.isActiveAndEnabled) MapBehaviour.Instance.Close();
               },
               GameOptionsManager.Instance.currentNormalGameOptions.MapId == 3,
               "ADMIN"
           );

            // Hacker Admin Table Charges
            hackerAdminTableChargesText = GameObject.Instantiate(hackerAdminTableButton.actionButton.cooldownTimerText, hackerAdminTableButton.actionButton.cooldownTimerText.transform.parent);
            hackerAdminTableChargesText.text = "";
            hackerAdminTableChargesText.enableWordWrapping = false;
            hackerAdminTableChargesText.transform.localScale = Vector3.one * 0.5f;
            hackerAdminTableChargesText.transform.localPosition += new Vector3(-0.05f, 0.7f, 0);

            hackerVitalsButton = new CustomButton(
               () => {
                   if (GameOptionsManager.Instance.currentNormalGameOptions.MapId != 1) {
                       if (Hacker.vitals == null) {
                           var e = UnityEngine.Object.FindObjectsOfType<SystemConsole>().FirstOrDefault(x => x.gameObject.name.Contains("panel_vitals") || x.gameObject.name.Contains("Vitals"));
                           if (e == null || Camera.main == null) return;
                           Hacker.vitals = UnityEngine.Object.Instantiate(e.MinigamePrefab, Camera.main.transform, false);
                       }
                       Hacker.vitals.transform.SetParent(Camera.main.transform, false);
                       Hacker.vitals.transform.localPosition = new Vector3(0.0f, 0.0f, -50f);
                       Hacker.vitals.Begin(null);
                   } else {
                       if (Hacker.doorLog == null) {
                           var e = UnityEngine.Object.FindObjectsOfType<SystemConsole>().FirstOrDefault(x => x.gameObject.name.Contains("SurvLogConsole"));
                           if (e == null || Camera.main == null) return;
                           Hacker.doorLog = UnityEngine.Object.Instantiate(e.MinigamePrefab, Camera.main.transform, false);
                       }
                       Hacker.doorLog.transform.SetParent(Camera.main.transform, false);
                       Hacker.doorLog.transform.localPosition = new Vector3(0.0f, 0.0f, -50f);
                       Hacker.doorLog.Begin(null);
                   }

                   if (Hacker.cantMove) PlayerControl.LocalPlayer.moveable = false;
                   PlayerControl.LocalPlayer.NetTransform.Halt(); // Stop current movement 

                   Hacker.chargesVitals--;
               },
               () => { return Hacker.Player != null && Hacker.Player == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead && GameOptionsManager.Instance.currentGameOptions.MapId != 0 && GameOptionsManager.Instance.currentNormalGameOptions.MapId != 3; },
               () => {
                   if (hackerVitalsChargesText != null) hackerVitalsChargesText.text = $"{Hacker.chargesVitals} / {Hacker.toolsNumber}";
                   hackerVitalsButton.actionButton.graphic.sprite = Utils.IsMira() ? Hacker.GetLogSprite() : Hacker.GetVitalsSprite();
                   hackerVitalsButton.actionButton.OverrideText(Utils.IsMira() ? "DOORLOG" : "VITALS");
                   return Hacker.chargesVitals > 0;
               },
               () => {
                   hackerVitalsButton.Timer = hackerVitalsButton.MaxTimer;
                   hackerVitalsButton.isEffectActive = false;
                   hackerVitalsButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
               },
               Hacker.GetVitalsSprite(),
               CustomButton.ButtonPositions.lowerRowCenter,
               __instance,
               KeyCode.H,
               true,
               0f,
               () => {
                   hackerVitalsButton.Timer = hackerVitalsButton.MaxTimer;
                   if (!hackerAdminTableButton.isEffectActive) PlayerControl.LocalPlayer.moveable = true;
                   if (Minigame.Instance) {
                       if (Utils.IsMira()) Hacker.doorLog.ForceClose();
                       else Hacker.vitals.ForceClose();
                   }
               },
               false,
              Utils.IsMira() ? "DOORLOG" : "VITALS"
           );

            // Hacker Vitals Charges
            hackerVitalsChargesText = GameObject.Instantiate(hackerVitalsButton.actionButton.cooldownTimerText, hackerVitalsButton.actionButton.cooldownTimerText.transform.parent);
            hackerVitalsChargesText.text = "";
            hackerVitalsChargesText.enableWordWrapping = false;
            hackerVitalsChargesText.transform.localScale = Vector3.one * 0.5f;
            hackerVitalsChargesText.transform.localPosition += new Vector3(-0.05f, 0.7f, 0);

            // Tracker button
            trackerTrackPlayerButton = new CustomButton(
                () => 
                {
                    if (Tracker.CurrentTarget.CheckFortifiedPlayer()) return;

                    Utils.StartRPC(CustomRPC.TrackerUsedTracker, Tracker.CurrentTarget.PlayerId);
                    RPCProcedure.TrackerUsedTracker(Tracker.CurrentTarget.PlayerId);
                    SoundEffectsManager.Play("trackerTrackPlayer");
                },
                () => { return Tracker.Player != null && Tracker.Player == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => { return PlayerControl.LocalPlayer.CanMove && Tracker.CurrentTarget != null && !Tracker.usedTracker; },
                () => 
                { 
                    if (Tracker.resetTargetAfterMeeting) Tracker.ResetTracked();
                    else if (Tracker.CurrentTarget != null && Tracker.CurrentTarget.Data.IsDead) Tracker.CurrentTarget = null; 
                },
                Tracker.GetButtonSprite(),
                CustomButton.ButtonPositions.lowerRowRight,
                __instance,
                KeyCode.F
            );

            trackerTrackCorpsesButton = new CustomButton(
                () => { Tracker.corpsesTrackingTimer = Tracker.corpsesTrackingDuration;
                            SoundEffectsManager.Play("trackerTrackCorpses"); },
                () => { return Tracker.Player != null && Tracker.Player == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead && Tracker.canTrackCorpses; },
                () => { return PlayerControl.LocalPlayer.CanMove; },
                () => {
                    trackerTrackCorpsesButton.Timer = trackerTrackCorpsesButton.MaxTimer;
                    trackerTrackCorpsesButton.isEffectActive = false;
                    trackerTrackCorpsesButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
                },
                Tracker.GetTrackCorpsesButtonSprite(),
                CustomButton.ButtonPositions.lowerRowCenter,
                __instance,
                KeyCode.G,
                true,
                Tracker.corpsesTrackingDuration,
                () => {
                    trackerTrackCorpsesButton.Timer = trackerTrackCorpsesButton.MaxTimer;
                }
            );

            DisperserButton = new CustomButton(
               OnClick: () => 
                {
                    Utils.StartRPC(CustomRPC.Disperse);
                    RPCProcedure.Disperse();

                    DisperserButton.Timer = DisperserButton.MaxTimer;
                    SoundEffectsManager.Play("morphlingMorph");
                },
               HasButton: () => { return Disperser.Player != null && Disperser.Charges > 0 && Disperser.Player == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
               CouldUse: () => 
                {     
                    if (DisperserChargesText != null) DisperserChargesText.text = $"{Disperser.Charges}";  
                    return Disperser.Player != null && Disperser.Player == PlayerControl.LocalPlayer && Disperser.Charges > 0 && PlayerControl.LocalPlayer.CanMove;
                },
               OnMeetingEnds: () => { DisperserButton.Timer = DisperserButton.MaxTimer; },
               Sprite: Disperser.GetButtonSprite(),
               PositionOffset: new Vector3(0, -0.06f, 0),
               hudManager: __instance,
               hotkey: KeyCode.G,
               HasEffect: true,
               mirror: true,
               EffectDuration: 0f,
               OnEffectEnds: () => 
                {
                    DisperserButton.Timer = DisperserButton.MaxTimer;
                },
                buttonText: "DISPERSE"
            );
            DisperserChargesText = GameObject.Instantiate(DisperserButton.actionButton.cooldownTimerText, DisperserButton.actionButton.cooldownTimerText.transform.parent);
            DisperserChargesText.text = "";
            DisperserChargesText.enableWordWrapping = false;
            DisperserChargesText.transform.localScale = Vector3.one * 0.5f;
            DisperserChargesText.transform.localPosition += new Vector3(-0.05f, 0.7f, 0);
    
            poisonerKillButton = new CustomButton(
                () => 
                {
                    MurderAttemptResult murder = Utils.CheckMuderAttempt(Poisoner.Player, Poisoner.CurrentTarget);
                    if (murder == MurderAttemptResult.PerformKill) 
                    {
                        Poisoner.poisoned = Poisoner.CurrentTarget;
                         // Notify players about poisoned
                        Utils.StartRPC(CustomRPC.PoisonerSetPoisoned, Poisoner.poisoned.PlayerId, (byte)0);
                        RPCProcedure.PoisonerSetPoisoned(Poisoner.poisoned.PlayerId, 0);

                        byte lastTimer = (byte)Poisoner.delay;
                        FastDestroyableSingleton<HudManager>.Instance.StartCoroutine(Effects.Lerp(Poisoner.delay, new Action<float>((p) => { // Delayed action
                            if (p <= 1f) 
                            {
                                byte timer = (byte)poisonerKillButton.Timer;
                                if (timer != lastTimer) 
                                {
                                    lastTimer = timer;
                                    Utils.StartRPC(CustomRPC.ShareGhostInfo, PlayerControl.LocalPlayer.PlayerId, (byte)GhostInfoTypes.PoisonerTimer, timer);
                                }
                            }
                            if (p == 1f) 
                            {
                                // Perform kill if possible and reset poisoned (regardless whether the kill was successful or not)
                                var res = Utils.CheckMurderAttemptAndKill(Poisoner.Player, Poisoner.poisoned, showAnimation: false);
                                if (res == MurderAttemptResult.PerformKill) 
                                {
                                    Utils.StartRPC(CustomRPC.PoisonerSetPoisoned, byte.MaxValue, byte.MaxValue);
                                    RPCProcedure.PoisonerSetPoisoned(byte.MaxValue, byte.MaxValue);
                                }
                            }
                        })));
                        SoundEffectsManager.Play("poisonerPoison");
                        poisonerKillButton.HasEffect = true; // Trigger effect on this click
                    }
                    else if (murder == MurderAttemptResult.BlankKill) 
                    {
                        poisonerKillButton.Timer = poisonerKillButton.MaxTimer;
                        poisonerKillButton.HasEffect = false;
                    } 
                    else 
                    {
                        poisonerKillButton.HasEffect = false;
                    }
                },
                () => { return Poisoner.Player != null && Poisoner.Player == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => 
                {
                    return Poisoner.CurrentTarget != null && PlayerControl.LocalPlayer.CanMove;
                },
                () => {
                    poisonerKillButton.Timer = poisonerKillButton.MaxTimer;
                    poisonerKillButton.isEffectActive = false;
                    poisonerKillButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
                },
                Poisoner.GetButtonSprite(),
                CustomButton.ButtonPositions.upperRowLeft,
                __instance,
                KeyCode.Q,
                false,
                0f,
                () => {
                    poisonerKillButton.Timer = poisonerKillButton.MaxTimer;
                },
                buttonText: "POISON"
            );

            portalmakerPlacePortalButton = new CustomButton(
                () => 
                {
                    portalmakerPlacePortalButton.Timer = portalmakerPlacePortalButton.MaxTimer;

                    var pos = PlayerControl.LocalPlayer.transform.position;
                    byte[] buff = new byte[sizeof(float) * 2];
                    Buffer.BlockCopy(BitConverter.GetBytes(pos.x), 0, buff, 0 * sizeof(float), sizeof(float));
                    Buffer.BlockCopy(BitConverter.GetBytes(pos.y), 0, buff, 1 * sizeof(float), sizeof(float));

                    Utils.StartRPC(CustomRPC.PlacePortal, buff);
                    RPCProcedure.PlacePortal(buff);
                    SoundEffectsManager.Play("tricksterPlaceBox");
                },
                () => { return Portalmaker.Player != null && Portalmaker.Player == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead && Portal.secondPortal == null; },
                () => { return PlayerControl.LocalPlayer.CanMove && Portal.secondPortal == null; },
                () => { portalmakerPlacePortalButton.Timer = portalmakerPlacePortalButton.MaxTimer; },
                Portalmaker.GetPlacePortalButtonSprite(),
                CustomButton.ButtonPositions.lowerRowRight,
                __instance,
                KeyCode.F
            );

            MinerMineButton = new CustomButton(
                () => 
                {
                    MinerMineButton.Timer = MinerMineButton.MaxTimer;

                    var pos = PlayerControl.LocalPlayer.transform.position;
                    byte[] buff = new byte[sizeof(float) * 2];
                    Buffer.BlockCopy(BitConverter.GetBytes(pos.x), 0, buff, 0 * sizeof(float), sizeof(float));
                    Buffer.BlockCopy(BitConverter.GetBytes(pos.y), 0, buff, 1 * sizeof(float), sizeof(float));

                    Utils.StartRPC(CustomRPC.PlaceMine, buff);
                    RPCProcedure.PlaceMine(buff);
                    var meetingIntroAnimation = UnityEngine.Object.FindObjectOfType<MeetingIntroAnimation>();
                    if (meetingIntroAnimation != null)
                    {
                        SoundManager.Instance.PlaySound(meetingIntroAnimation.PlayerDeadSound, false, 1f, null);
                    }
                    //SoundEffectsManager.Play("tricksterPlaceBox");
                },
                () => { return Miner.Player != null && Miner.Player == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead && Portal.secondPortal == null; },
                () => { return PlayerControl.LocalPlayer.CanMove && Portal.secondPortal == null; },
                () => { MinerMineButton.Timer = MinerMineButton.MaxTimer; },
                Miner.GetMineSprite(),
                CustomButton.ButtonPositions.upperRowLeft,
                __instance,
                KeyCode.F,
                buttonText: "Mine"
            );

            usePortalButton = new CustomButton(
                () => {
                    bool didTeleport = false;
                    Vector3 exit = Portal.FindExit(PlayerControl.LocalPlayer.transform.position);
                    Vector3 entry = Portal.FindEntry(PlayerControl.LocalPlayer.transform.position);

                    bool portalMakerSoloTeleport = !Portal.LocationNearEntry(PlayerControl.LocalPlayer.transform.position);
                    if (portalMakerSoloTeleport) {
                        exit = Portal.firstPortal.portalGameObject.transform.position;
                        entry = PlayerControl.LocalPlayer.transform.position;
                    }

                    PlayerControl.LocalPlayer.NetTransform.RpcSnapTo(entry);

                    if (!PlayerControl.LocalPlayer.Data.IsDead) 
                    {  
                        // Ghosts can portal too, but non-blocking and only with a local animation
                        Utils.StartRPC(CustomRPC.UsePortal, (byte)PlayerControl.LocalPlayer.PlayerId, portalMakerSoloTeleport ? (byte)1 : (byte)0);
                    }
                    RPCProcedure.UsePortal(PlayerControl.LocalPlayer.PlayerId, portalMakerSoloTeleport ? (byte)1 : (byte)0);
                    usePortalButton.Timer = usePortalButton.MaxTimer;
                    portalmakerMoveToPortalButton.Timer = usePortalButton.MaxTimer;
                    SoundEffectsManager.Play("portalUse");
                    FastDestroyableSingleton<HudManager>.Instance.StartCoroutine(Effects.Lerp(Portal.teleportDuration, new Action<float>((p) => { // Delayed action
                        PlayerControl.LocalPlayer.moveable = false;
                        PlayerControl.LocalPlayer.NetTransform.Halt();
                        if (p >= 0.5f && p <= 0.53f && !didTeleport && !MeetingHud.Instance) {
                            if (SubmergedCompatibility.IsSubmerged) {
                                SubmergedCompatibility.ChangeFloor(exit.y > -7);
                            }
                            PlayerControl.LocalPlayer.NetTransform.RpcSnapTo(exit);
                            didTeleport = true;
                        }
                        if (p == 1f) {
                            PlayerControl.LocalPlayer.moveable = true;
                        }
                    })));
                    },
                () => {
                    if (PlayerControl.LocalPlayer == Portalmaker.Player && Portal.bothPlacedAndEnabled)
                        portalmakerButtonText1.text = Portal.LocationNearEntry(PlayerControl.LocalPlayer.transform.position) || !Portalmaker.canPortalFromAnywhere ? "" : "1. " + Portal.firstPortal.room;
                    return Portal.bothPlacedAndEnabled; },
                () => { return PlayerControl.LocalPlayer.CanMove && (Portal.LocationNearEntry(PlayerControl.LocalPlayer.transform.position) || Portalmaker.canPortalFromAnywhere && PlayerControl.LocalPlayer == Portalmaker.Player) && !Portal.isTeleporting; },
                () => { usePortalButton.Timer = usePortalButton.MaxTimer; },
                Portalmaker.getUsePortalButtonSprite(),
                new Vector3(0.9f, -0.06f, 0),
                __instance,
                KeyCode.J,
                mirror: true
            );

            portalmakerMoveToPortalButton = new CustomButton(
                () => 
                {
                    bool didTeleport = false;
                    Vector3 exit = Portal.secondPortal.portalGameObject.transform.position;

                    if (!PlayerControl.LocalPlayer.Data.IsDead) 
                    {  
                        // Ghosts can portal too, but non-blocking and only with a local animation
                        Utils.StartRPC(CustomRPC.UsePortal, PlayerControl.LocalPlayer.PlayerId, (byte)2);
                    }
                    RPCProcedure.UsePortal(PlayerControl.LocalPlayer.PlayerId, 2);
                    usePortalButton.Timer = usePortalButton.MaxTimer;
                    portalmakerMoveToPortalButton.Timer = usePortalButton.MaxTimer;
                    SoundEffectsManager.Play("portalUse");
                    FastDestroyableSingleton<HudManager>.Instance.StartCoroutine(Effects.Lerp(Portal.teleportDuration, new Action<float>((p) => { // Delayed action
                        PlayerControl.LocalPlayer.moveable = false;
                        PlayerControl.LocalPlayer.NetTransform.Halt();
                        if (p >= 0.5f && p <= 0.53f && !didTeleport && !MeetingHud.Instance) 
                        {
                            if (SubmergedCompatibility.IsSubmerged) 
                            {
                                SubmergedCompatibility.ChangeFloor(exit.y > -7);
                            }
                            PlayerControl.LocalPlayer.NetTransform.RpcSnapTo(exit);
                            didTeleport = true;
                        }
                        if (p == 1f) 
                        {
                            PlayerControl.LocalPlayer.moveable = true;
                        }
                    })));
                },
                () => { return Portalmaker.canPortalFromAnywhere && Portal.bothPlacedAndEnabled && PlayerControl.LocalPlayer == Portalmaker.Player; },
                () => { return PlayerControl.LocalPlayer.CanMove && !Portal.LocationNearEntry(PlayerControl.LocalPlayer.transform.position) && !Portal.isTeleporting; },
                () => { portalmakerMoveToPortalButton.Timer = usePortalButton.MaxTimer; },
                Portalmaker.getUsePortalButtonSprite(),
                new Vector3(0.9f, 1f, 0),
                __instance,
                KeyCode.G,
                mirror: true
            );


            portalmakerButtonText1 = GameObject.Instantiate(usePortalButton.actionButton.cooldownTimerText, usePortalButton.actionButton.cooldownTimerText.transform.parent);
            portalmakerButtonText1.text = "";
            portalmakerButtonText1.enableWordWrapping = false;
            portalmakerButtonText1.transform.localScale = Vector3.one * 0.5f;
            portalmakerButtonText1.transform.localPosition += new Vector3(-0.05f, 0.55f, -1f);

            portalmakerButtonText2 = GameObject.Instantiate(portalmakerMoveToPortalButton.actionButton.cooldownTimerText, portalmakerMoveToPortalButton.actionButton.cooldownTimerText.transform.parent);
            portalmakerButtonText2.text = "";
            portalmakerButtonText2.enableWordWrapping = false;
            portalmakerButtonText2.transform.localScale = Vector3.one * 0.5f;
            portalmakerButtonText2.transform.localPosition += new Vector3(-0.05f, 0.55f, -1f);


            // Jackal Sidekick Button
            jackalSidekickButton = new CustomButton(
                () => 
                {
                    if (Jackal.CurrentTarget.CheckFortifiedPlayer()) return;
                    if (!Jackal.canCreateSidekickFromImpostor && Jackal.CurrentTarget.Data.Role.IsImpostor || !Jackal.canCreateSidekickFromImpostor && Jackal.CurrentTarget.IsNeutralKiller()) 
                    {
                        // Murder the current target of they can't be sidekicked. this gets rid of fake SKs (yw M)
                        Utils.CheckMurderAttemptAndKill(Jackal.Player, Jackal.CurrentTarget);
                        jackalKillButton.Timer = Jackal.Cooldown;
                        jackalSidekickButton.Timer = Jackal.createSidekickCooldown;
                        Utils.ShowFlash(Color.red, 0.5f);
                        
                        // death reason
                        Utils.StartRPC(CustomRPC.ShareGhostInfo, 
                        PlayerControl.LocalPlayer.PlayerId, 
                        (byte)GhostInfoTypes.DeathReasonAndKiller, 
                        Jackal.CurrentTarget.PlayerId, 
                        (byte)DeadPlayer.CustomDeathReason.WrongSidekick,
                        Jackal.Player.PlayerId);
                        GameHistory.CreateDeathReason(Jackal.CurrentTarget, DeadPlayer.CustomDeathReason.WrongSidekick, killer: Jackal.Player);

                        SoundEffectsManager.Play("DeadSound");
                    }
                    else
                    {
                        Utils.StartRPC(CustomRPC.JackalCreatesSidekick, Jackal.CurrentTarget.PlayerId);
                        RPCProcedure.JackalCreatesSidekick(Jackal.CurrentTarget.PlayerId);
                        SoundEffectsManager.Play("jackalSidekick");
                    }
                },
                () => { return Jackal.canCreateSidekick && Jackal.Player != null && Jackal.Player == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => { return Jackal.canCreateSidekick && Jackal.CurrentTarget != null && PlayerControl.LocalPlayer.CanMove; },
                () => { jackalSidekickButton.Timer = jackalSidekickButton.MaxTimer;},
                Jackal.getSidekickButtonSprite(),
                CustomButton.ButtonPositions.lowerRowCenter,
                __instance,
                KeyCode.F,
                buttonText: "SIDEKICK"
            );

            // Hitman Kill
            HitmanKillButton = new CustomButton(
                () => 
                {
                    if (Utils.CheckMurderAttemptAndKill(Hitman.Player, Hitman.CurrentTarget) == MurderAttemptResult.SuppressKill) return;
                    if (Hitman.CurrentTarget.CheckVeteranPestilenceKill() || Hitman.CurrentTarget.CheckFortifiedPlayer()) return;

                    HitmanKillButton.Timer = HitmanKillButton.MaxTimer; 
                    Hitman.CurrentTarget = null;
                },
                () => { return Hitman.Player != null && Hitman.Player == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => { return Hitman.CurrentTarget && PlayerControl.LocalPlayer.CanMove; },
                () => { HitmanKillButton.Timer = HitmanKillButton.MaxTimer;},
                __instance.KillButton.graphic.sprite,
                CustomButton.ButtonPositions.upperRowRight,
                __instance,
                KeyCode.Q
            );

            // Jackal Kill
            jackalKillButton = new CustomButton(
                () => 
                {
                    if (Utils.CheckMurderAttemptAndKill(Jackal.Player, Jackal.CurrentTarget) == MurderAttemptResult.SuppressKill) return;
                    if (Jackal.CurrentTarget.CheckVeteranPestilenceKill() || Jackal.CurrentTarget.CheckFortifiedPlayer()) return;

                    jackalKillButton.Timer = jackalKillButton.MaxTimer; 
                    Jackal.CurrentTarget = null;
                },
                () => { return Jackal.Player != null && Jackal.Player == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => { return Jackal.CurrentTarget && PlayerControl.LocalPlayer.CanMove; },
                () => { jackalKillButton.Timer = jackalKillButton.MaxTimer;},
                __instance.KillButton.graphic.sprite,
                CustomButton.ButtonPositions.upperRowRight,
                __instance,
                KeyCode.Q
            );
            
            // Sidekick Kill
            sidekickKillButton = new CustomButton(
                () => 
                {
                    if (Sidekick.CurrentTarget.CheckVeteranPestilenceKill()  || Sidekick.CurrentTarget.CheckFortifiedPlayer()) return;

                    if (Utils.CheckMurderAttemptAndKill(Sidekick.Player, Sidekick.CurrentTarget) == MurderAttemptResult.SuppressKill) return;
                    sidekickKillButton.Timer = sidekickKillButton.MaxTimer; 
                    Sidekick.CurrentTarget = null;
                },
                () => { return Sidekick.canKill && Sidekick.Player != null && Sidekick.Player == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => { return Sidekick.CurrentTarget && PlayerControl.LocalPlayer.CanMove; },
                () => { sidekickKillButton.Timer = sidekickKillButton.MaxTimer;},
                __instance.KillButton.graphic.sprite,
                CustomButton.ButtonPositions.upperRowRight,
                __instance,
                KeyCode.Q
            );

            // Eraser erase button
            eraserButton = new CustomButton(
                () => 
                {
                    if (Eraser.CurrentTarget.CheckVeteranPestilenceKill() || Eraser.CurrentTarget.CheckFortifiedPlayer()) return;

                    eraserButton.MaxTimer += 10;
                    eraserButton.Timer = eraserButton.MaxTimer;

                    Utils.StartRPC(CustomRPC.SetFutureErased, Eraser.CurrentTarget.PlayerId);
                    RPCProcedure.SetFutureErased(Eraser.CurrentTarget.PlayerId);
                    SoundEffectsManager.Play("eraserErase");
                },
                () => { return Eraser.Player != null && Eraser.Player == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => { return PlayerControl.LocalPlayer.CanMove && Eraser.CurrentTarget != null; },
                () => { eraserButton.Timer = eraserButton.MaxTimer;},
                Eraser.GetButtonSprite(),
                CustomButton.ButtonPositions.upperRowLeft,
                __instance,
                KeyCode.F,
                buttonText: "ERASE"
            );

            placeJackInTheBoxButton = new CustomButton(
                () => {
                    placeJackInTheBoxButton.Timer = placeJackInTheBoxButton.MaxTimer;

                    var pos = PlayerControl.LocalPlayer.transform.position;
                    byte[] buff = new byte[sizeof(float) * 2];
                    Buffer.BlockCopy(BitConverter.GetBytes(pos.x), 0, buff, 0*sizeof(float), sizeof(float));
                    Buffer.BlockCopy(BitConverter.GetBytes(pos.y), 0, buff, 1*sizeof(float), sizeof(float));

                    Utils.StartRPC(CustomRPC.PlaceJackInTheBox, buff);
                    RPCProcedure.PlaceJackInTheBox(buff);
                    SoundEffectsManager.Play("tricksterPlaceBox");
                },
                () => { return Trickster.Player != null && Trickster.Player == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead && !JackInTheBox.HasJackInTheBoxLimitReached(); },
                () => { return PlayerControl.LocalPlayer.CanMove && !JackInTheBox.HasJackInTheBoxLimitReached(); },
                () => { placeJackInTheBoxButton.Timer = placeJackInTheBoxButton.MaxTimer;},
                Trickster.GetPlaceBoxButtonSprite(),
                CustomButton.ButtonPositions.upperRowLeft,
                __instance,
                KeyCode.F
            );
            
            lightsOutButton = new CustomButton(
                () => 
                {
                    Utils.StartRPC(CustomRPC.LightsOut);
                    RPCProcedure.LightsOut();
                    SoundEffectsManager.Play("lighterLight");
                },
                () => { return Trickster.Player != null && Trickster.Player == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead
                                                           && JackInTheBox.HasJackInTheBoxLimitReached() && JackInTheBox.boxesConvertedToVents; },
                () => { return PlayerControl.LocalPlayer.CanMove && JackInTheBox.HasJackInTheBoxLimitReached() && JackInTheBox.boxesConvertedToVents; },
                () => { 
                    lightsOutButton.Timer = lightsOutButton.MaxTimer;
                    lightsOutButton.isEffectActive = false;
                    lightsOutButton.actionButton.graphic.color = Palette.EnabledColor;
                },
                Trickster.GetLightsOutButtonSprite(),
                CustomButton.ButtonPositions.upperRowLeft,
                __instance,
                KeyCode.F,
                true,
                Trickster.lightsOutDuration,
                () => {
                    lightsOutButton.Timer = lightsOutButton.MaxTimer;
                    SoundEffectsManager.Play("lighterLight");
                }
            );

            // Cleaner Clean
            cleanerCleanButton = new CustomButton(
                () => 
                {
                    foreach (Collider2D collider2D in Physics2D.OverlapCircleAll(PlayerControl.LocalPlayer.GetTruePosition(), PlayerControl.LocalPlayer.MaxReportDistance, Constants.PlayersOnlyMask)) {
                        if (collider2D.tag == "DeadBody")
                        {
                            DeadBody component = collider2D.GetComponent<DeadBody>();
                            if (component && !component.Reported)
                            {
                                Vector2 truePosition = PlayerControl.LocalPlayer.GetTruePosition();
                                Vector2 truePosition2 = component.TruePosition;
                                if (Vector2.Distance(truePosition2, truePosition) <= PlayerControl.LocalPlayer.MaxReportDistance && PlayerControl.LocalPlayer.CanMove && !PhysicsHelpers.AnythingBetween(truePosition, truePosition2, Constants.ShipAndObjectsMask, false))
                                {
                                    NetworkedPlayerInfo playerInfo = GameData.Instance.GetPlayerById(component.ParentId);
                                    
                                    Utils.StartRPC(CustomRPC.CleanBody, playerInfo.PlayerId, Cleaner.Player.PlayerId);
                                    RPCProcedure.CleanBody(playerInfo.PlayerId, Cleaner.Player.PlayerId);

                                    Cleaner.Player.killTimer = cleanerCleanButton.Timer = cleanerCleanButton.MaxTimer;
                                    SoundEffectsManager.Play("cleanerClean");
                                    break;
                                }
                            }
                        }
                    }
                },
                () => { return Cleaner.Player != null && Cleaner.Player == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => { return __instance.ReportButton.graphic.color == Palette.EnabledColor && PlayerControl.LocalPlayer.CanMove; },
                () => { cleanerCleanButton.Timer = cleanerCleanButton.MaxTimer; },
                Cleaner.GetButtonSprite(),
                CustomButton.ButtonPositions.upperRowLeft,
                __instance,
                KeyCode.F,
                buttonText: "CLEAN"
            );

            // Warlock curse
            warlockCurseButton = new CustomButton(
                () => 
                {
                    if (Warlock.curseVictim == null) 
                    {
                        if (Warlock.CurrentTarget.CheckVeteranPestilenceKill() || Warlock.CurrentTarget.CheckFortifiedPlayer()) return;

                        // Apply Curse
                        Warlock.curseVictim = Warlock.CurrentTarget;
                        warlockCurseButton.Sprite = Warlock.GetCurseKillButtonSprite();
                        warlockCurseButton.buttonText = "KILL";
                        warlockCurseButton.Timer = 1f;
                        SoundEffectsManager.Play("warlockCurse");

                        // Ghost Info
                        Utils.StartRPC(CustomRPC.ShareGhostInfo, PlayerControl.LocalPlayer.PlayerId, (byte)GhostInfoTypes.WarlockTarget, Warlock.curseVictim.PlayerId);

                    } 
                    else if (Warlock.curseVictim != null && Warlock.curseVictimTarget != null) 
                    {
                        MurderAttemptResult murder = Utils.CheckMurderAttemptAndKill(Warlock.Player, Warlock.curseVictimTarget, showAnimation: false);
                        if (murder == MurderAttemptResult.SuppressKill) return; 

                        // If blanked or killed
                        if(Warlock.rootTime > 0) 
                        {
                            Lazy.position = PlayerControl.LocalPlayer.transform.position;
                            PlayerControl.LocalPlayer.moveable = false;
                            PlayerControl.LocalPlayer.NetTransform.Halt(); // Stop current movement so the warlock is not just running straight into the next object
                            FastDestroyableSingleton<HudManager>.Instance.StartCoroutine(Effects.Lerp(Warlock.rootTime, new Action<float>((p) => { // Delayed action
                                if (p == 1f) {
                                    PlayerControl.LocalPlayer.moveable = true;
                                }
                            })));
                        }
                        
                        Warlock.curseVictim = null;
                        Warlock.curseVictimTarget = null;
                        warlockCurseButton.buttonText = "CURSE";
                        warlockCurseButton.Sprite = Warlock.GetCurseButtonSprite();
                        Warlock.Player.killTimer = warlockCurseButton.Timer = warlockCurseButton.MaxTimer;
                        Utils.StartRPC(CustomRPC.ShareGhostInfo, PlayerControl.LocalPlayer.PlayerId, (byte)GhostInfoTypes.WarlockTarget, Byte.MaxValue);

                    }
                },
                () => { return Warlock.Player != null && Warlock.Player == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => { return ((Warlock.curseVictim == null && Warlock.CurrentTarget != null) || (Warlock.curseVictim != null && Warlock.curseVictimTarget != null)) && PlayerControl.LocalPlayer.CanMove; },
                () => { 
                    warlockCurseButton.Timer = warlockCurseButton.MaxTimer;
                    warlockCurseButton.Sprite = Warlock.GetCurseButtonSprite();
                    warlockCurseButton.buttonText = "CURSE";
                    Warlock.curseVictim = null;
                    Warlock.curseVictimTarget = null;
                },
                Warlock.GetCurseButtonSprite(),
                CustomButton.ButtonPositions.upperRowLeft,
                __instance,
                KeyCode.F,
                buttonText: "CURSE"
            );

            // Vigilante button
            VigilanteButton = new CustomButton(
                () => {
                    if (Vigilante.ventTarget != null) 
                    { // Seal vent
                        Utils.StartRPC(CustomRPC.SealVent, Vigilante.ventTarget.Id);
                        RPCProcedure.SealVent(Vigilante.ventTarget.Id);
                        Vigilante.ventTarget = null;
                        
                    } else if (!Utils.IsMira() && !Utils.IsFungle() && !SubmergedCompatibility.IsSubmerged) { // Place camera if there's no vent and it's not MiraHQ or Submerged
                        var pos = PlayerControl.LocalPlayer.transform.position;
                        byte[] buff = new byte[sizeof(float) * 2];
                        Buffer.BlockCopy(BitConverter.GetBytes(pos.x), 0, buff, 0*sizeof(float), sizeof(float));
                        Buffer.BlockCopy(BitConverter.GetBytes(pos.y), 0, buff, 1*sizeof(float), sizeof(float));

                        Utils.StartRPC(CustomRPC.PlaceCamera, buff);
                        RPCProcedure.PlaceCamera(buff); 
                    }
                    SoundEffectsManager.Play("VigilantePlaceCam");  // Same sound used for both types (cam or vent)!
                    VigilanteButton.Timer = VigilanteButton.MaxTimer;
                },
                () => { return Vigilante.Player != null && Vigilante.Player == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead && Vigilante.remainingScrews >= Mathf.Min(Vigilante.ventPrice, Vigilante.camPrice); },
                () => {
                    VigilanteButton.actionButton.graphic.sprite = (Vigilante.ventTarget == null && !Utils.IsMira() && !Utils.IsFungle() && !SubmergedCompatibility.IsSubmerged) ? Vigilante.GetPlaceCameraButtonSprite() : Vigilante.GetCloseVentButtonSprite(); 
                    if (VigilanteButtonScrewsText != null) VigilanteButtonScrewsText.text = $"{Vigilante.remainingScrews}/{Vigilante.totalScrews}";

                    if (Vigilante.ventTarget != null)
                        return Vigilante.remainingScrews >= Vigilante.ventPrice && PlayerControl.LocalPlayer.CanMove;
                    return !Utils.IsMira() && !Utils.IsFungle() && !SubmergedCompatibility.IsSubmerged && Vigilante.remainingScrews >= Vigilante.camPrice && PlayerControl.LocalPlayer.CanMove;
                },
                () => { VigilanteButton.Timer = VigilanteButton.MaxTimer; },
                Vigilante.GetPlaceCameraButtonSprite(),
                CustomButton.ButtonPositions.lowerRowRight,
                __instance,
                KeyCode.F
            );
            
            // Vigilante button screws counter
            VigilanteButtonScrewsText = GameObject.Instantiate(VigilanteButton.actionButton.cooldownTimerText, VigilanteButton.actionButton.cooldownTimerText.transform.parent);
            VigilanteButtonScrewsText.text = "";
            VigilanteButtonScrewsText.enableWordWrapping = false;
            VigilanteButtonScrewsText.transform.localScale = Vector3.one * 0.5f;
            VigilanteButtonScrewsText.transform.localPosition += new Vector3(-0.05f, 0.7f, 0);

            VigilanteCamButton = new CustomButton(
                () => {
                    if (!Utils.IsMira()) {
                        if (Vigilante.minigame == null) {
                            byte mapId = GameOptionsManager.Instance.currentNormalGameOptions.MapId;
                            var e = UnityEngine.Object.FindObjectsOfType<SystemConsole>().FirstOrDefault(x => x.gameObject.name.Contains("Surv_Panel") || x.name.Contains("Cam") || x.name.Contains("BinocularsSecurityConsole"));
                            if (Utils.IsSkeld() || mapId == 3) e = UnityEngine.Object.FindObjectsOfType<SystemConsole>().FirstOrDefault(x => x.gameObject.name.Contains("SurvConsole"));
                            else if (Utils.IsAirship()) e = UnityEngine.Object.FindObjectsOfType<SystemConsole>().FirstOrDefault(x => x.gameObject.name.Contains("task_cams"));
                            if (e == null || Camera.main == null) return;
                            Vigilante.minigame = UnityEngine.Object.Instantiate(e.MinigamePrefab, Camera.main.transform, false);
                        }
                        Vigilante.minigame.transform.SetParent(Camera.main.transform, false);
                        Vigilante.minigame.transform.localPosition = new Vector3(0.0f, 0.0f, -50f);
                        Vigilante.minigame.Begin(null);
                    } else {
                        if (Vigilante.minigame == null) {
                            var e = UnityEngine.Object.FindObjectsOfType<SystemConsole>().FirstOrDefault(x => x.gameObject.name.Contains("SurvLogConsole"));
                            if (e == null || Camera.main == null) return;
                            Vigilante.minigame = UnityEngine.Object.Instantiate(e.MinigamePrefab, Camera.main.transform, false);
                        }
                        Vigilante.minigame.transform.SetParent(Camera.main.transform, false);
                        Vigilante.minigame.transform.localPosition = new Vector3(0.0f, 0.0f, -50f);
                        Vigilante.minigame.Begin(null);
                    }
                    Vigilante.Charges--;

                    if (Vigilante.cantMove) PlayerControl.LocalPlayer.moveable = false;
                    PlayerControl.LocalPlayer.NetTransform.Halt(); // Stop current movement 
                },
                () => {
                    return Vigilante.Player != null && Vigilante.Player == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead && Vigilante.remainingScrews < Mathf.Min(Vigilante.ventPrice, Vigilante.camPrice)
                               && !SubmergedCompatibility.IsSubmerged;
                },
                () => {
                    if (VigilanteChargesText != null) VigilanteChargesText.text = $"{Vigilante.Charges} / {Vigilante.maxCharges}";
                    VigilanteCamButton.actionButton.graphic.sprite = Utils.IsMira() ? Vigilante.GetLogSprite() : Vigilante.GetCamSprite();
                    VigilanteCamButton.actionButton.OverrideText(Utils.IsMira() ? "DOORLOG" : "SECURITY");
                    return PlayerControl.LocalPlayer.CanMove && Vigilante.Charges > 0;
                },
                () => {
                    VigilanteCamButton.Timer = VigilanteCamButton.MaxTimer;
                    VigilanteCamButton.isEffectActive = false;
                    VigilanteCamButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
                },
                Vigilante.GetCamSprite(),
                CustomButton.ButtonPositions.lowerRowRight,
                __instance,
                KeyCode.G,
                true,
                0f,
                () => {
                    VigilanteCamButton.Timer = VigilanteCamButton.MaxTimer;
                    if (Minigame.Instance) {
                        Vigilante.minigame.ForceClose();
                    }
                    PlayerControl.LocalPlayer.moveable = true;
                },
                false,
                Utils.IsMira() ? "DOORLOG" : "SECURITY"
            );

            // Vigilante cam button Charges
            VigilanteChargesText = GameObject.Instantiate(VigilanteCamButton.actionButton.cooldownTimerText, VigilanteCamButton.actionButton.cooldownTimerText.transform.parent);
            VigilanteChargesText.text = "";
            VigilanteChargesText.enableWordWrapping = false;
            VigilanteChargesText.transform.localScale = Vector3.one * 0.5f;
            VigilanteChargesText.transform.localPosition += new Vector3(-0.05f, 0.7f, 0);

            // Arsonist button
            arsonistButton = new CustomButton(
                () => 
                {
                    bool dousedEveryoneAlive = Arsonist.DousedEveryoneAlive();
                    if (dousedEveryoneAlive) 
                    {
                        Utils.StartRPC(CustomRPC.ArsonistWin);
                        RPCProcedure.ArsonistWin();
                        arsonistButton.HasEffect = false;
                    } 
                    else if (Arsonist.CurrentTarget != null) 
                    {
                        if (Arsonist.CurrentTarget.CheckVeteranPestilenceKill() || Arsonist.CurrentTarget.CheckFortifiedPlayer()) return;

                        Arsonist.douseTarget = Arsonist.CurrentTarget;
                        arsonistButton.HasEffect = true;
                        SoundEffectsManager.Play("arsonistDouse");
                    }
                },
                () => { return Arsonist.Player != null && Arsonist.Player == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => 
                {
                    bool dousedEveryoneAlive = Arsonist.DousedEveryoneAlive();
                    if (dousedEveryoneAlive) arsonistButton.actionButton.graphic.sprite = Arsonist.GetIgniteSprite();
                    
                    if (arsonistButton.isEffectActive && Arsonist.douseTarget != Arsonist.CurrentTarget) {
                        Arsonist.douseTarget = null;
                        arsonistButton.Timer = 0f;
                        arsonistButton.isEffectActive = false;
                    }

                    return PlayerControl.LocalPlayer.CanMove && (dousedEveryoneAlive || Arsonist.CurrentTarget != null);
                },
                () => {
                    arsonistButton.Timer = arsonistButton.MaxTimer;
                    arsonistButton.isEffectActive = false;
                    Arsonist.douseTarget = null;
                },
                Arsonist.GetDouseSprite(),
                CustomButton.ButtonPositions.lowerRowRight,
                __instance,
                KeyCode.F,
                true,
                Arsonist.Duration,
                () => {
                    if (Arsonist.douseTarget != null) Arsonist.dousedPlayers.Add(Arsonist.douseTarget);
                    
                    arsonistButton.Timer = Arsonist.DousedEveryoneAlive() ? 0 : arsonistButton.MaxTimer;

                    foreach (PlayerControl p in Arsonist.dousedPlayers) 
                    {
                        if (MapOptions.BeanIcons.ContainsKey(p.PlayerId)) 
                        {
                            MapOptions.BeanIcons[p.PlayerId].SetSemiTransparent(false);
                        }
                    }

                    // Ghost Info
                    Utils.StartRPC(CustomRPC.ShareGhostInfo, PlayerControl.LocalPlayer.PlayerId, (byte)GhostInfoTypes.ArsonistDouse, Arsonist.douseTarget.PlayerId);
                    Arsonist.douseTarget = null;
                }
            );

            // Vulture Eat
            vultureEatButton = new CustomButton(
                () => {
                    foreach (Collider2D collider2D in Physics2D.OverlapCircleAll(PlayerControl.LocalPlayer.GetTruePosition(), PlayerControl.LocalPlayer.MaxReportDistance, Constants.PlayersOnlyMask)) {
                        if (collider2D.tag == "DeadBody") 
                        {
                            DeadBody component = collider2D.GetComponent<DeadBody>();
                            if (component && !component.Reported) 
                            {
                                Vector2 truePosition = PlayerControl.LocalPlayer.GetTruePosition();
                                Vector2 truePosition2 = component.TruePosition;
                                if (Vector2.Distance(truePosition2, truePosition) <= PlayerControl.LocalPlayer.MaxReportDistance && PlayerControl.LocalPlayer.CanMove && !PhysicsHelpers.AnythingBetween(truePosition, truePosition2, Constants.ShipAndObjectsMask, false)) {
                                    NetworkedPlayerInfo playerInfo = GameData.Instance.GetPlayerById(component.ParentId);

                                    Utils.StartRPC(CustomRPC.CleanBody, playerInfo.PlayerId, Vulture.Player.PlayerId);
                                    RPCProcedure.CleanBody(playerInfo.PlayerId, Vulture.Player.PlayerId);

                                    Vulture.Cooldown = vultureEatButton.Timer = vultureEatButton.MaxTimer;
                                    SoundEffectsManager.Play("vultureEat");
                                    break;
                                }
                            }
                        }
                    }
                },
                () => { return Vulture.Player != null && Vulture.Player == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => { return __instance.ReportButton.graphic.color == Palette.EnabledColor && PlayerControl.LocalPlayer.CanMove; },
                () => { vultureEatButton.Timer = vultureEatButton.MaxTimer; },
                Vulture.GetButtonSprite(),
                CustomButton.ButtonPositions.lowerRowCenter,
                __instance,
                KeyCode.F,
                buttonText: "EAT"
            );

            // Medium button
            mediumButton = new CustomButton(
                () => 
                {
                    if (Medium.target != null) {
                        Medium.soulTarget = Medium.target;
                        mediumButton.HasEffect = true;
                        SoundEffectsManager.Play("mediumAsk");
                    }
                },
                () => { return Medium.medium != null && Medium.medium == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => {
                    if (mediumButton.isEffectActive && Medium.target != Medium.soulTarget) {
                        Medium.soulTarget = null;
                        mediumButton.Timer = 0f;
                        mediumButton.isEffectActive = false;
                    }
                    return Medium.target != null && PlayerControl.LocalPlayer.CanMove;
                },
                () => {
                    mediumButton.Timer = mediumButton.MaxTimer;
                    mediumButton.isEffectActive = false;
                    Medium.soulTarget = null;
                },
                Medium.GetQuestionSprite(),
                CustomButton.ButtonPositions.lowerRowRight,
                __instance,
                KeyCode.F,
                true,
                Medium.Duration,
                () => 
                {
                    mediumButton.Timer = mediumButton.MaxTimer;
                    if (Medium.target == null || Medium.target.player == null) return;
                    string msg = Medium.GetInfo(Medium.target.player, Medium.target.GetKiller, Medium.target.DeathReason);
                    FastDestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, msg);
                    // Ghost Info
                    Utils.StartRPC(CustomRPC.ShareGhostInfo, Medium.target.player.PlayerId, (byte)GhostInfoTypes.MediumInfo, msg);

                    // Remove soul
                    if (Medium.oneTimeUse) {
                        float closestDistance = float.MaxValue;
                        SpriteRenderer target = null;

                        foreach ((DeadPlayer db, Vector3 ps) in Medium.deadBodies) {
                            if (db == Medium.target) {
                                Tuple<DeadPlayer, Vector3> deadBody = Tuple.Create(db, ps);
                                Medium.deadBodies.Remove(deadBody);
                                break;
                            }

                        }
                        foreach (SpriteRenderer rend in Medium.souls) {
                            float distance = Vector2.Distance(rend.transform.position, PlayerControl.LocalPlayer.GetTruePosition());
                            if (distance < closestDistance) {
                                closestDistance = distance;
                                target = rend;
                            }
                        }

                        FastDestroyableSingleton<HudManager>.Instance.StartCoroutine(Effects.Lerp(5f, new Action<float>((p) => {
                            if (target != null) {
                                var tmp = target.color;
                                tmp.a = Mathf.Clamp01(1 - p);
                                target.color = tmp;
                            }
                            if (p == 1f && target != null && target.gameObject != null) UnityEngine.Object.Destroy(target.gameObject);
                        })));

                        Medium.souls.Remove(target);
                    }
                    SoundEffectsManager.Stop("mediumAsk");
                }
            );

            // Pursuer button
            pursuerButton = new CustomButton(
                () => {
                    if (Pursuer.target != null) 
                    {
                        if (Pursuer.target.CheckVeteranPestilenceKill() || Pursuer.target.CheckFortifiedPlayer()) return;
                        
                        Utils.StartRPC(CustomRPC.SetBlanked, Pursuer.target.PlayerId, Byte.MaxValue);
                        RPCProcedure.SetBlanked(Pursuer.target.PlayerId, Byte.MaxValue);

                        Pursuer.target = null;

                        Pursuer.blanks++;
                        pursuerButton.Timer = pursuerButton.MaxTimer;
                        SoundEffectsManager.Play("pursuerBlank");
                    }

                },
                () => { return Pursuer.Player != null && Pursuer.Player == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead && Pursuer.blanks < Pursuer.blanksNumber; },
                () => {
                    if (pursuerButtonBlanksText != null) pursuerButtonBlanksText.text = $"{Pursuer.blanksNumber - Pursuer.blanks}";

                    return Pursuer.blanksNumber > Pursuer.blanks && PlayerControl.LocalPlayer.CanMove && Pursuer.target != null;
                },
                () => { pursuerButton.Timer = pursuerButton.MaxTimer; },
                Pursuer.GetTargetSprite(),
                CustomButton.ButtonPositions.lowerRowRight,
                __instance,
                KeyCode.F
            );

            // Pursuer button blanks left
            pursuerButtonBlanksText = GameObject.Instantiate(pursuerButton.actionButton.cooldownTimerText, pursuerButton.actionButton.cooldownTimerText.transform.parent);
            pursuerButtonBlanksText.text = "";
            pursuerButtonBlanksText.enableWordWrapping = false;
            pursuerButtonBlanksText.transform.localScale = Vector3.one * 0.5f;
            pursuerButtonBlanksText.transform.localPosition += new Vector3(-0.05f, 0.7f, 0);


            // Witch Spell button
            witchSpellButton = new CustomButton(
                () => {
                    if (Witch.CurrentTarget != null) 
                    {
                        if (Witch.CurrentTarget.CheckVeteranPestilenceKill() || Witch.CurrentTarget.CheckFortifiedPlayer()) return;

                        Witch.spellCastingTarget = Witch.CurrentTarget;
                        SoundEffectsManager.Play("witchSpell");
                    }
                },
                () => { return Witch.Player != null && Witch.Player == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => {
                    if (witchSpellButton.isEffectActive && Witch.spellCastingTarget != Witch.CurrentTarget) {
                        Witch.spellCastingTarget = null;
                        witchSpellButton.Timer = 0f;
                        witchSpellButton.isEffectActive = false;
                    }
                    return PlayerControl.LocalPlayer.CanMove && Witch.CurrentTarget != null;
                },
                () => {
                    witchSpellButton.Timer = witchSpellButton.MaxTimer;
                    witchSpellButton.isEffectActive = false;
                    Witch.spellCastingTarget = null;
                },
                Witch.GetButtonSprite(),
                CustomButton.ButtonPositions.upperRowLeft,
                __instance,
                KeyCode.F,
                true,
                Witch.spellCastingDuration,
                () => {
                    if (Witch.spellCastingTarget == null) return;
                    MurderAttemptResult attempt = Utils.CheckMuderAttempt(Witch.Player, Witch.spellCastingTarget);
                    if (attempt == MurderAttemptResult.PerformKill) 
                    {
                        Utils.StartRPC(CustomRPC.SetFutureSpelled, Witch.CurrentTarget.PlayerId);
                        RPCProcedure.SetFutureSpelled(Witch.CurrentTarget.PlayerId);
                    }
                    if (attempt == MurderAttemptResult.BlankKill || attempt == MurderAttemptResult.PerformKill) 
                    {
                        Witch.currentCooldownAddition += Witch.CooldownAddition;
                        witchSpellButton.MaxTimer = Witch.Cooldown + Witch.currentCooldownAddition;
                        Patches.PlayerControlFixedUpdatePatch.MiniCooldownUpdate();  // Modifies the MaxTimer if the witch is the mini
                        witchSpellButton.Timer = witchSpellButton.MaxTimer;
                        if (Witch.triggerBothCooldowns) {
                            float multiplier = (Mini.Player != null && PlayerControl.LocalPlayer == Mini.Player) ? (Mini.IsGrownUp ? 0.66f : 2f) : 1f;
                            Witch.Player.killTimer = GameOptionsManager.Instance.currentNormalGameOptions.KillCooldown * multiplier;
                        }
                    } else {
                        witchSpellButton.Timer = 0f;
                    }
                    Witch.spellCastingTarget = null;
                }
            );

            // Ninja mark and assassinate button 
            ninjaButton = new CustomButton(
                () => 
                {
                    if (Ninja.ninjaMarked != null) 
                    {
                        // Murder attempt with teleport
                        MurderAttemptResult attempt = Utils.CheckMuderAttempt(Ninja.Player, Ninja.ninjaMarked);
                        if (attempt == MurderAttemptResult.PerformKill) 
                        {
                            // Create first trace before killing
                            var pos = PlayerControl.LocalPlayer.transform.position;
                            byte[] buff = new byte[sizeof(float) * 2];
                            Buffer.BlockCopy(BitConverter.GetBytes(pos.x), 0, buff, 0 * sizeof(float), sizeof(float));
                            Buffer.BlockCopy(BitConverter.GetBytes(pos.y), 0, buff, 1 * sizeof(float), sizeof(float));

                            Utils.StartRPC(CustomRPC.PlaceNinjaTrace, buff);
                            RPCProcedure.PlaceNinjaTrace(buff);

                            Utils.StartRPC(CustomRPC.SetInvisible, Ninja.Player.PlayerId, byte.MinValue);
                            RPCProcedure.SetInvisible(Ninja.Player.PlayerId, byte.MinValue);

                            // Perform Kill

                            Utils.StartRPC(CustomRPC.UncheckedMurderPlayer, PlayerControl.LocalPlayer.PlayerId, Ninja.ninjaMarked.PlayerId, byte.MaxValue);
                            if (SubmergedCompatibility.IsSubmerged) 
                            {
                                SubmergedCompatibility.ChangeFloor(Ninja.ninjaMarked.transform.localPosition.y > -7);
                            }
                                RPCProcedure.UncheckedMurderPlayer(PlayerControl.LocalPlayer.PlayerId, Ninja.ninjaMarked.PlayerId, byte.MaxValue);
                            // Create Second trace after killing
                            pos = Ninja.ninjaMarked.transform.position;
                            buff = new byte[sizeof(float) * 2];
                            Buffer.BlockCopy(BitConverter.GetBytes(pos.x), 0, buff, 0 * sizeof(float), sizeof(float));
                            Buffer.BlockCopy(BitConverter.GetBytes(pos.y), 0, buff, 1 * sizeof(float), sizeof(float));

                            Utils.StartRPC(CustomRPC.PlaceNinjaTrace, buff);
                            RPCProcedure.PlaceNinjaTrace(buff);
                        }

                        if (attempt == MurderAttemptResult.BlankKill || attempt == MurderAttemptResult.PerformKill) 
                        {
                            ninjaButton.Timer = ninjaButton.MaxTimer;
                            Ninja.Player.killTimer = GameOptionsManager.Instance.currentNormalGameOptions.KillCooldown;
                        } 
                        else if (attempt == MurderAttemptResult.SuppressKill) 
                        {
                            ninjaButton.Timer = 0f;
                        }
                        Ninja.ninjaMarked = null;
                        return;
                    } 
                    if (Ninja.CurrentTarget != null) 
                    {
                        Ninja.ninjaMarked = Ninja.CurrentTarget;
                        ninjaButton.Timer = 5f;
                        SoundEffectsManager.Play("warlockCurse");

                        // Ghost Info
                        Utils.StartRPC(CustomRPC.ShareGhostInfo, PlayerControl.LocalPlayer.PlayerId, (byte)GhostInfoTypes.NinjaMarked, Ninja.ninjaMarked.PlayerId);
                    }
                },
                () => { return Ninja.Player != null && Ninja.Player == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => {  // CouldUse
                    ninjaButton.Sprite = Ninja.ninjaMarked != null ? Ninja.GetKillButtonSprite() : Ninja.GetMarkButtonSprite(); 
                    return (Ninja.CurrentTarget != null || Ninja.ninjaMarked != null && !TransportationToolPatches.IsUsingTransportation(Ninja.ninjaMarked)) && PlayerControl.LocalPlayer.CanMove;
                },
                () => {  // on meeting ends
                    ninjaButton.Timer = ninjaButton.MaxTimer;
                    Ninja.ninjaMarked = null;
                },
                Ninja.GetMarkButtonSprite(),
                CustomButton.ButtonPositions.upperRowLeft,
                __instance,
                KeyCode.F                   
            );

            mayorMeetingButton = new CustomButton(
               () => 
               {
                   PlayerControl.LocalPlayer.NetTransform.Halt(); // Stop current movement 
                   Mayor.remoteMeetingsLeft--;
	               Utils.HandlePoisonerBiteOnBodyReport(); // Manually call Poisoner handling, since the CmdReportDeadBody Prefix won't be called
                   RPCProcedure.UncheckedCmdReportDeadBody(PlayerControl.LocalPlayer.PlayerId, Byte.MaxValue);

                   Utils.StartRPC(CustomRPC.UncheckedCmdReportDeadBody, PlayerControl.LocalPlayer.PlayerId, Byte.MaxValue);
                   mayorMeetingButton.Timer = 1f;
               },
               () => { return Mayor.Player != null && Mayor.Player == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead && Mayor.meetingButton; },
               () => {
                   mayorMeetingButton.actionButton.OverrideText("Emergency ("+ Mayor.remoteMeetingsLeft + ")");
                   bool sabotageActive = false;
                   foreach (PlayerTask task in PlayerControl.LocalPlayer.myTasks.GetFastEnumerator())
                       if (task.TaskType == TaskTypes.FixLights || task.TaskType == TaskTypes.RestoreOxy || task.TaskType == TaskTypes.ResetReactor || task.TaskType == TaskTypes.ResetSeismic || task.TaskType == TaskTypes.FixComms || task.TaskType == TaskTypes.StopCharles
                           || SubmergedCompatibility.IsSubmerged && task.TaskType == SubmergedCompatibility.RetrieveOxygenMask)
                           sabotageActive = true;
                   return !sabotageActive && PlayerControl.LocalPlayer.CanMove && (Mayor.remoteMeetingsLeft > 0);
               },
               () => { mayorMeetingButton.Timer = mayorMeetingButton.MaxTimer; },
               Mayor.GetMeetingSprite(),
               CustomButton.ButtonPositions.lowerRowRight,
               __instance,
               KeyCode.F,
               true,
               0f,
               () => {},
               false,
               "Meeting"
           );

           CowardButton = new CustomButton(
               () => 
               {
                   PlayerControl.LocalPlayer.NetTransform.Halt(); // Stop current movement 
	               Utils.HandlePoisonerBiteOnBodyReport(); // Manually call Poisoner handling, since the CmdReportDeadBody Prefix won't be called
                   RPCProcedure.UncheckedCmdReportDeadBody(PlayerControl.LocalPlayer.PlayerId, Byte.MaxValue);

                   Utils.StartRPC(CustomRPC.UncheckedCmdReportDeadBody, PlayerControl.LocalPlayer.PlayerId, Byte.MaxValue);
               },
               () => { return Coward.Player != null && Coward.CanUse && Coward.Player == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead && Mayor.meetingButton; },
               () => 
               {
                   return PlayerControl.LocalPlayer.CanMove && Coward.CanUse;
               },
               () => { CowardButton.Timer = CowardButton.MaxTimer; },
               Mayor.GetMeetingSprite(),
               new Vector3(0, -0.06f, 0),
               __instance,
               KeyCode.F,
               true,
               0f,
               () => {},
               mirror: true,
               buttonText: "Meeting"
           );

            // Trapper button
            trapperButton = new CustomButton(
                () => {


                    var pos = PlayerControl.LocalPlayer.transform.position;
                    byte[] buff = new byte[sizeof(float) * 2];
                    Buffer.BlockCopy(BitConverter.GetBytes(pos.x), 0, buff, 0 * sizeof(float), sizeof(float));
                    Buffer.BlockCopy(BitConverter.GetBytes(pos.y), 0, buff, 1 * sizeof(float), sizeof(float));

                    Utils.StartRPC(CustomRPC.SetTrap, buff);
                    RPCProcedure.SetTrap(buff);

                    SoundEffectsManager.Play("trapperTrap");
                    trapperButton.Timer = trapperButton.MaxTimer;
                },
                () => { return Trapper.Player != null && Trapper.Player == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => {
                    if (trapperChargesText != null) trapperChargesText.text = $"{Trapper.Charges} / {Trapper.maxCharges}";
                    return PlayerControl.LocalPlayer.CanMove && Trapper.Charges > 0;
                },
                () => { trapperButton.Timer = trapperButton.MaxTimer; },
                Trapper.GetButtonSprite(),
                CustomButton.ButtonPositions.lowerRowRight,
                __instance,
                KeyCode.F,
                buttonText: "TRAP"
            );
            // Trapper Charges
            trapperChargesText = GameObject.Instantiate(trapperButton.actionButton.cooldownTimerText, trapperButton.actionButton.cooldownTimerText.transform.parent);
            trapperChargesText.text = "";
            trapperChargesText.enableWordWrapping = false;
            trapperChargesText.transform.localScale = Vector3.one * 0.5f;
            trapperChargesText.transform.localPosition += new Vector3(-0.05f, 0.7f, 0);


           // Yoyo button
            yoyoButton = new CustomButton(
                () => 
                {
                    var pos = PlayerControl.LocalPlayer.transform.position;
                    byte[] buff = new byte[sizeof(float) * 2];
                    Buffer.BlockCopy(BitConverter.GetBytes(pos.x), 0, buff, 0 * sizeof(float), sizeof(float));
                    Buffer.BlockCopy(BitConverter.GetBytes(pos.y), 0, buff, 1 * sizeof(float), sizeof(float));
                    
                    if (Yoyo.markedLocation == null) 
                    {
                        Utils.StartRPC(CustomRPC.YoyoMarkLocation, buff);
                        RPCProcedure.YoyoMarkLocation(buff);
                        SoundEffectsManager.Play("tricksterPlaceBox");
                        yoyoButton.Sprite = Yoyo.GetBlinkButtonSprite();
                        yoyoButton.Timer = 10f;
                        yoyoButton.HasEffect = false;
                        yoyoButton.buttonText = "Blink";
                    } else {
                        // Jump to location
                        var exit = (Vector3)Yoyo.markedLocation;
                        if (SubmergedCompatibility.IsSubmerged) {
                            SubmergedCompatibility.ChangeFloor(exit.y > -7);
                        }
                        Utils.StartRPC(CustomRPC.YoyoBlink, Byte.MaxValue, buff);
                        RPCProcedure.YoyoBlink(true, buff);
                        yoyoButton.EffectDuration = Yoyo.blinkDuration;
                        yoyoButton.Timer = 10f;
                        yoyoButton.HasEffect = true;
                        yoyoButton.buttonText = "Returning...";
                        SoundEffectsManager.Play("morphlingMorph");
                    }
                },
                () => { return Yoyo.Player != null && Yoyo.Player == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => { return PlayerControl.LocalPlayer.CanMove; },
                () => {
                    if (Yoyo.markStaysOverMeeting) {
                        yoyoButton.Timer = 10f;
                    } else {
                        Yoyo.markedLocation = null;
                        yoyoButton.Timer = yoyoButton.MaxTimer;
                        yoyoButton.Sprite = Yoyo.GetMarkButtonSprite();
                        yoyoButton.buttonText = "Mark Location";
                    }
                },
                Yoyo.GetMarkButtonSprite(),
                CustomButton.ButtonPositions.upperRowLeft,
                __instance,
                KeyCode.F,
                false,
                Yoyo.blinkDuration,
                () => {
                    if (TransportationToolPatches.IsUsingTransportation(Yoyo.Player)) 
                    {
                        yoyoButton.Timer = 0.5f;
                        yoyoButton.GlitchTimer = 0.5f;
                        yoyoButton.isEffectActive = true;
                        yoyoButton.actionButton.cooldownTimerText.color = new Color(0F, 0.8F, 0F);
                        return;
                    } else if (Yoyo.Player.inVent) {
                        __instance.ImpostorVentButton.DoClick();
                    }

                    // jump back!
                    var pos = PlayerControl.LocalPlayer.transform.position;
                    byte[] buff = new byte[sizeof(float) * 2];
                    Buffer.BlockCopy(BitConverter.GetBytes(pos.x), 0, buff, 0 * sizeof(float), sizeof(float));
                    Buffer.BlockCopy(BitConverter.GetBytes(pos.y), 0, buff, 1 * sizeof(float), sizeof(float));
                    var exit = (Vector3)Yoyo.markedLocation;
                    if (SubmergedCompatibility.IsSubmerged) {
                        SubmergedCompatibility.ChangeFloor(exit.y > -7);
                    }
                    Utils.StartRPC(CustomRPC.YoyoBlink, (byte)0, buff);
                    RPCProcedure.YoyoBlink(false, buff);

                    yoyoButton.Timer = yoyoButton.MaxTimer;
                    yoyoButton.isEffectActive = false;
                    yoyoButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
                    yoyoButton.HasEffect = false;
                    yoyoButton.Sprite = Yoyo.GetMarkButtonSprite();
                    yoyoButton.buttonText = "Mark Location";
                    SoundEffectsManager.Play("morphlingMorph");
                    if (Minigame.Instance) {
                        Minigame.Instance.Close();
                    }
                },
                buttonText: "Mark Location"
            );

            yoyoAdminTableButton = new CustomButton(
               () => 
               {
                   if (!MapBehaviour.Instance || !MapBehaviour.Instance.isActiveAndEnabled) 
                   {
                       HudManager __instance = FastDestroyableSingleton<HudManager>.Instance;
                       __instance.InitMap();
                       MapBehaviour.Instance.ShowCountOverlay(allowedToMove: true, showLivePlayerPosition: true, includeDeadBodies: true);
                   }
               },
               () => 
               {
                 return Yoyo.Player != null && Yoyo.Player == PlayerControl.LocalPlayer && Yoyo.hasAdminTable && !PlayerControl.LocalPlayer.Data.IsDead; },
               () => 
               {
                   return true;
               },
               () => {
                   yoyoAdminTableButton.Timer = yoyoAdminTableButton.MaxTimer;
                   yoyoAdminTableButton.isEffectActive = false;
                   yoyoAdminTableButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
               },
               Hacker.GetAdminSprite(),
               CustomButton.ButtonPositions.lowerRowCenter,
               __instance,
               KeyCode.G,
               true,
               0f,
               () => {
                   yoyoAdminTableButton.Timer = yoyoAdminTableButton.MaxTimer;
                   if (MapBehaviour.Instance && MapBehaviour.Instance.isActiveAndEnabled) MapBehaviour.Instance.Close();
               },
               GameOptionsManager.Instance.currentNormalGameOptions.MapId == 3,
               "ADMIN"
           );


            zoomOutButton = new CustomButton(
                () => { Utils.ToggleZoom();
                },
                () => 
                { 
                    if (PlayerControl.LocalPlayer == null || !PlayerControl.LocalPlayer.Data.IsDead) return false;
                    var (playerCompleted, playerTotal) = TasksHandler.TaskInfo(PlayerControl.LocalPlayer.Data);
                    int numberOfLeftTasks = playerTotal - playerCompleted;
                    return numberOfLeftTasks <= 0 || !CustomOptionHolder.finishTasksBeforeHauntingOrZoomingOut.GetBool();
                },
                () => { return true; },
                () => { return; },
                null,  // Invisible button!
                new Vector3(0.4f, 2.8f, 0),
                __instance,
                KeyCode.KeypadPlus
                );
            zoomOutButton.Timer = 0f;

            // Set the default (or settings from the previous game) timers / durations when spawning the buttons
            initialized = true;
            SetCustomButtonCooldowns();
            PlayerHackedButtons = new Dictionary<byte, List<CustomButton>>();
            
        }
    }
}
