using Hazel;
using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Reactor.Utilities.Extensions;
using Reactor.Utilities;

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
        private static CustomButton MonarchKnightButton;
        private static CustomButton ChronosRewindButton;
        private static CustomButton medicShieldButton;
        private static CustomButton OracleButton;
        private static CustomButton VeteranAlertButton;
        private static CustomButton morphlingButton;
        private static CustomButton PainterButton;
        private static CustomButton GatekeeperPlacePortalButton;
        private static CustomButton MinerMineButton;
        private static CustomButton usePortalButton;
        private static CustomButton GatekeeperMoveToPortalButton;
        private static CustomButton hackerButton;
        public static CustomButton hackerVitalsButton;
        public static CustomButton hackerAdminTableButton;
        private static CustomButton UndertakerButton;
        private static CustomButton HitmanDragButton;
        public static CustomButton LandlordButton;
        private static CustomButton trackerTrackPlayerButton;
        private static CustomButton trackerTrackCorpsesButton;
        private static CustomButton ScavengerScavengeButton;
        private static CustomButton RomanticSetTargetButton;
        public static CustomButton ViperKillButton;
        public static CustomButton HitmanKillButton;
        public static CustomButton JuggernautKillButton;
        public static CustomButton jackalKillButton;
        public static CustomButton PredatorTerminateButton;
        public static CustomButton PredatorKillButton;
        public static CustomButton GlitchKillButton;
        public static CustomButton RomanticKillButton;
        public static CustomButton WerewolfMaulButton;
        public static CustomButton RecruitKillButton;
        public static CustomButton PlaguebearerButton;
        public static CustomButton PestilenceButton;
        public static CustomButton jackalRecruitButton;
        public static CustomButton CultistButton;
        private static CustomButton eraserButton;
        private static CustomButton placeJackInTheBoxButton;
        private static CustomButton DisperserButton;
        private static CustomButton lightsOutButton;
        public static CustomButton WraithButton;
        public static CustomButton GrenadierButton;
        public static CustomButton JanitorCleanButton;
        public static CustomButton warlockCurseButton;
        public static CustomButton VigilanteButton;
        public static CustomButton VigilanteCamButton;
        public static CustomButton arsonistButton;
        public static CustomButton SnitchButton;
        public static CustomButton ScavengerEatButton;
        public static CustomButton PsychicButton;
        public static CustomButton SurvivorButton;
        public static CustomButton witchSpellButton;
        public static CustomButton AssassinButton;
        public static CustomButton mayorMeetingButton;
        public static CustomButton CowardButton;
        public static CustomButton trapperButton;
        public static CustomButton yoyoButton;
        public static CustomButton yoyoAdminTableButton;
        public static CustomButton zoomOutButton;
        public static CustomButton ViperBlindButton;

        public static Dictionary<byte, List<CustomButton>> PlayerHackedButtons = null;
        public static PoolablePlayer targetDisplay;

        public static TMPro.TMP_Text VigilanteButtonScrewsText;
        public static TMPro.TMP_Text VigilanteChargesText;
        public static TMPro.TMP_Text VeteranChargesText;
        public static TMPro.TMP_Text MysticChargesText;
        public static TMPro.TMP_Text OracleChargesText;
        public static TMPro.TMP_Text CrusaderChargesText;
        public static TMPro.TMP_Text MonarchChargesText;
        public static TMPro.TMP_Text GlitchButtonHacksText;
        public static TMPro.TMP_Text LandlordButtonChargesText;
        public static TMPro.TMP_Text ChronosChargesText;
        public static TMPro.TMP_Text SurvivorButtonBlanksText;
        public static TMPro.TMP_Text hackerAdminTableChargesText;
        public static TMPro.TMP_Text hackerVitalsChargesText;
        public static TMPro.TMP_Text DisperserChargesText;
        public static TMPro.TMP_Text trapperChargesText;
        public static TMPro.TMP_Text GatekeeperButtonText1;
        public static TMPro.TMP_Text GatekeeperButtonText2;

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
            ChronosRewindButton.MaxTimer = Chronos.Cooldown;
            WraithButton.MaxTimer = Wraith.Cooldown;
            SnitchButton.MaxTimer = Snitch.Cooldown;
            PlaguebearerButton.MaxTimer = Plaguebearer.Cooldown;
            medicShieldButton.MaxTimer = 0f;
            OracleButton.MaxTimer = Oracle.Cooldown;
            RomanticSetTargetButton.MaxTimer = 0f;
            ViperBlindButton.MaxTimer = Viper.BlindCooldown;
            VeteranAlertButton.MaxTimer = Veteran.Cooldown;
            BlackmailButton.MaxTimer = Blackmailer.Cooldown;
            LandlordButton.MaxTimer = Landlord.Cooldown;
            PredatorTerminateButton.MaxTimer = Predator.TerminateCooldown;
            PredatorKillButton.MaxTimer = Predator.TerminateKillCooldown;
            GrenadierButton.MaxTimer = Grenadier.Cooldown;
            UndertakerButton.MaxTimer = Undertaker.Cooldown;
            HitmanDragButton.MaxTimer = Hitman.DragCooldown;
            DisperserButton.MaxTimer = Disperser.Cooldown;
            morphlingButton.MaxTimer = Morphling.Cooldown;
            MimicButton.MaxTimer = Glitch.MimicCooldown;
            HitmanMorphButton.MaxTimer = Hitman.MorphCooldown;
            PainterButton.MaxTimer = Painter.Cooldown;
            GatekeeperPlacePortalButton.MaxTimer = Gatekeeper.Cooldown;
            MinerMineButton.MaxTimer = Miner.Cooldown;
            usePortalButton.MaxTimer = Gatekeeper.usePortalCooldown;
            CrusaderButton.MaxTimer = Crusader.Cooldown;
            MonarchKnightButton.MaxTimer = Monarch.Cooldown;
            MysticButton.MaxTimer = Mystic.Cooldown;
            GatekeeperMoveToPortalButton.MaxTimer = Gatekeeper.usePortalCooldown;
            hackerButton.MaxTimer = Hacker.Cooldown;
            hackerVitalsButton.MaxTimer = Hacker.Cooldown;
            hackerAdminTableButton.MaxTimer = Hacker.Cooldown;
            ViperKillButton.MaxTimer = Viper.Cooldown;
            trackerTrackPlayerButton.MaxTimer = 0f;
            PestilenceButton.MaxTimer = Pestilence.Cooldown;
            jackalKillButton.MaxTimer = Jackal.Cooldown;
            GlitchKillButton.MaxTimer = Glitch.KillCooldown;
            RomanticKillButton.MaxTimer = VengefulRomantic.Cooldown;
            WerewolfMaulButton.MaxTimer = Werewolf.Cooldown;
            JuggernautKillButton.MaxTimer = Juggernaut.Cooldown;
            RecruitKillButton.MaxTimer = Recruit.Cooldown;
            jackalRecruitButton.MaxTimer = Jackal.createRecruitCooldown;
            CultistButton.MaxTimer = Cultist.Cooldown;
            eraserButton.MaxTimer = Eraser.Cooldown;
            placeJackInTheBoxButton.MaxTimer = Trickster.placeBoxCooldown;
            lightsOutButton.MaxTimer = Trickster.lightsOutCooldown;
            JanitorCleanButton.MaxTimer = Janitor.Cooldown;
            warlockCurseButton.MaxTimer = Warlock.Cooldown;
            VigilanteButton.MaxTimer = Vigilante.Cooldown;
            VigilanteCamButton.MaxTimer = Vigilante.Cooldown;
            arsonistButton.MaxTimer = Arsonist.Cooldown;
            ScavengerEatButton.MaxTimer = Scavenger.Cooldown;
            PsychicButton.MaxTimer = Psychic.Cooldown;
            SurvivorButton.MaxTimer = Survivor.Cooldown;
            trackerTrackCorpsesButton.MaxTimer = Tracker.corpsesTrackingCooldown;
            ScavengerScavengeButton.MaxTimer = Scavenger.ScavengeCooldown;
            witchSpellButton.MaxTimer = Witch.Cooldown;
            AssassinButton.MaxTimer = Assassin.Cooldown;
            mayorMeetingButton.MaxTimer = GameManager.Instance.LogicOptions.GetEmergencyCooldown();
            CowardButton.MaxTimer = GameManager.Instance.LogicOptions.GetEmergencyCooldown();
            trapperButton.MaxTimer = Trapper.Cooldown;
            yoyoButton.MaxTimer = Yoyo.markCooldown;
            yoyoAdminTableButton.MaxTimer = Yoyo.adminCooldown;
            yoyoAdminTableButton.EffectDuration = 10f;

            ChronosRewindButton.EffectDuration = Chronos.RewindTimeDuration;
            hackerButton.EffectDuration = Hacker.Duration;
            hackerVitalsButton.EffectDuration = Hacker.Duration;
            hackerAdminTableButton.EffectDuration = Hacker.Duration;
            ViperKillButton.EffectDuration = Viper.delay;
            PainterButton.EffectDuration = Painter.Duration;
            GrenadierButton.EffectDuration = Grenadier.GrenadeDuration;
            morphlingButton.EffectDuration = Morphling.Duration;
            MimicButton.EffectDuration = Glitch.MimicDuration;
            HitmanMorphButton.EffectDuration = Hitman.MorphDuration;
            SnitchButton.EffectDuration = Snitch.Duration;
            lightsOutButton.EffectDuration = Trickster.lightsOutDuration;
            arsonistButton.EffectDuration = Arsonist.Duration;
            PsychicButton.EffectDuration = Psychic.Duration;
            VeteranAlertButton.EffectDuration = Veteran.Duration;
            WraithButton.EffectDuration = Wraith.Duration;
            PredatorTerminateButton.EffectDuration = Predator.TerminateDuration;
            trackerTrackCorpsesButton.EffectDuration = Tracker.corpsesTrackingDuration;
            ScavengerScavengeButton.EffectDuration = Scavenger.ScavengeDuration;
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
                if (targetDisplay != null)
                {  // Reset the poolable player
                    targetDisplay.gameObject.SetActive(false);
                    GameObject.Destroy(targetDisplay.gameObject);
                    targetDisplay = null;
                }
                return;
            }
            // Add poolable player to the button so that the target outfit is shown
            button.actionButton.cooldownTimerText.transform.localPosition = new Vector3(0, 0, -1f);  // Before the poolable player
            targetDisplay = UObject.Instantiate<PoolablePlayer>(Patches.IntroCutsceneOnDestroyPatch.playerPrefab, button.actionButton.transform);
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
                    Utils.SendRPC(CustomRPC.EngineerUsedRepair);
                    RPCProcedure.EngineerUsedRepair();
                    SoundEffectsManager.Play("engineerRepair");
                    foreach (PlayerTask task in PlayerControl.LocalPlayer.myTasks.GetFastEnumerator()) 
                    {
                        if (task.TaskType == TaskTypes.FixLights) 
                        {
                            Utils.SendRPC(CustomRPC.EngineerFixLights);
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
                            Utils.SendRPC(CustomRPC.EngineerFixSubmergedOxygen);
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
                CustomButton.ButtonPositions.lowerRowCenter,
                __instance,
                KeyCode.F,
                buttonText: "REPAIR"
            );

            // Sheriff Kill
            sheriffKillButton = new CustomButton(
                () => 
                {
                    if (Sheriff.CurrentTarget.CheckVeteranPestilenceKill()) return;

                    MurderAttemptResult murderAttemptResult = Utils.CheckMurderAttempt(Sheriff.Player, Sheriff.CurrentTarget);
                    if (murderAttemptResult == MurderAttemptResult.SuppressKill) return;

                    if (murderAttemptResult == MurderAttemptResult.PerformKill) 
                    {
                        byte targetId = 0;
                        if (Sheriff.Player == Recruit.Player)
                        {
                            // Recruit Sheriff can kill anyone
                            targetId = Sheriff.CurrentTarget.PlayerId;
                        }
                        // then the normal sheriff
                        else if (Sheriff.CurrentTarget.IsKiller()
                            || (Sheriff.spyCanDieToSheriff && Spy.Player == Sheriff.CurrentTarget)
                            || (Sheriff.canKillNeutrals && Sheriff.CurrentTarget.IsPassiveNeutral()))
                        {
                            targetId = Sheriff.CurrentTarget.PlayerId;
                        }
                        else
                        {
                            targetId = PlayerControl.LocalPlayer.PlayerId;
                        }

                        // Lucky sheriff shot doesnt kill if backfired
                        if (targetId == Sheriff.Player.PlayerId && Utils.CheckLucky(Sheriff.Player, true, true)) return;
                        
                        Utils.RpcMurderPlayer(Sheriff.Player, Utils.GetPlayerById(targetId), true);
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
                                    Utils.SendRPC(CustomRPC.DragBody, playerInfo.PlayerId);
                                    RPCProcedure.DragBody(playerInfo.PlayerId);
                                    Undertaker.CurrentTarget = deadBody;
                                    SoundEffectsManager.Play("JanitorClean");
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

                    Utils.SendRPC(CustomRPC.DropBody, playerId);
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
                if (Undertaker.CurrentTarget != null)
                {
                    UndertakerButton.actionButton.graphic.sprite = Undertaker.GetSecondButtonSprite();
                    UndertakerButton.buttonText = "DROP";
                }
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
            hotkey: KeyCode.F,
            buttonText: "DRAG"
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
                                    Utils.SendRPC(CustomRPC.HitmanDragBody, playerInfo.PlayerId);
                                    RPCProcedure.HitmanDragBody(playerInfo.PlayerId);
                                    Hitman.BodyTarget = deadBody;
                                    SoundEffectsManager.Play("JanitorClean");
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

                    Utils.SendRPC(CustomRPC.BlackmailerBlackmail, Blackmailer.CurrentTarget.PlayerId);
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

                    Utils.SendRPC(CustomRPC.GlitchUsedHacks, Glitch.CurrentTarget.PlayerId);
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

            // Landlord Teleport
            LandlordButton = new CustomButton(
                () => 
                {
                    try
                    {
                        ShapeShifterMenu.Singleton.Menu.ForceClose();
                    }
                    catch
                    {
                        Landlord.FirstTarget = null;
                        Landlord.SecondTarget = null;
                        List<byte> transportTargets = new List<byte>();
                        foreach (var player in PlayerControl.AllPlayerControls)
                        {
                            if (!player.Data.Disconnected)
                            {
                                if (!player.Data.IsDead) transportTargets.Add(player.PlayerId);
                                else
                                {
                                    foreach (var body in UObject.FindObjectsOfType<DeadBody>())
                                    {
                                        if (body.ParentId == player.PlayerId) transportTargets.Add(player.PlayerId);
                                    }
                                }
                            }
                        }
                        byte[] transporttargetIDs = transportTargets.ToArray();
                        var pk = new ShapeShifterMenu(LandlordButton, (x) =>
                        {
                            Landlord.FirstTarget = x;
                            Landlord.SwappingMenus = true;
                            Coroutines.Start(RPCProcedure.LandlordOpenSecondMenu());
                        }, (y) =>
                        {
                            return transporttargetIDs.Contains(y.PlayerId);
                        });
                            Coroutines.Start(pk.Open(0f, true));
                    }
                },
                () => 
                { return Landlord.Player != null && Landlord.Player == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => 
                {
                    if (LandlordButtonChargesText != null) LandlordButtonChargesText.text = $"{Landlord.Charges}";
                    return Landlord.Player != null && Landlord.Player == PlayerControl.LocalPlayer && Landlord.Charges > 0 && PlayerControl.LocalPlayer.CanMove;
                },
                () => { LandlordButton.Timer = LandlordButton.MaxTimer; },
                Landlord.GetButtonSprite(),
                CustomButton.ButtonPositions.lowerRowRight,
                __instance,
                KeyCode.F,
                buttonText: "TELEPORT"
            );
            // Landlord Charges counter
            LandlordButtonChargesText = GameObject.Instantiate(LandlordButton.actionButton.cooldownTimerText, LandlordButton.actionButton.cooldownTimerText.transform.parent);
            LandlordButtonChargesText.text = "";
            LandlordButtonChargesText.enableWordWrapping = false;
            LandlordButtonChargesText.transform.localScale = Vector3.one * 0.5f;
            LandlordButtonChargesText.transform.localPosition += new Vector3(-0.05f, 0.7f, 0);

            // Chronos Rewind Time
            ChronosRewindButton = new CustomButton(
                OnClick: () => 
                {
                    Utils.SendRPC(CustomRPC.ChronosRewindTime);
                    RPCProcedure.ChronosRewindTime();
                    SoundEffectsManager.Play("timemasterShield");
                },
                HasButton: () => { return Chronos.Player != null && Chronos.Player == PlayerControl.LocalPlayer && Chronos.Charges > 0 && !PlayerControl.LocalPlayer.Data.IsDead; },
                CouldUse: () => 
                { 
                    if (ChronosChargesText != null) ChronosChargesText.text = $"{Chronos.Charges}";
                    return PlayerControl.LocalPlayer.CanMove && Chronos.Charges > 0; 
                },
                OnMeetingEnds: () => 
                {
                    ChronosRewindButton.Timer = ChronosRewindButton.MaxTimer;
                    ChronosRewindButton.isEffectActive = false;
                    ChronosRewindButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
                },
                Sprite: Chronos.GetButtonSprite(),
                PositionOffset: CustomButton.ButtonPositions.lowerRowRight,
                hudManager: __instance,
                hotkey: KeyCode.F, 
                HasEffect: true,
                EffectDuration: Chronos.RewindTimeDuration,
                OnEffectEnds: () => 
                {
                    ChronosRewindButton.Timer = ChronosRewindButton.MaxTimer;
                    SoundEffectsManager.Stop("timemasterShield");
                },
                buttonText: "REWIND TIME"
            );
                // Chronos Charges counter
                ChronosChargesText = GameObject.Instantiate(ChronosRewindButton.actionButton.cooldownTimerText, ChronosRewindButton.actionButton.cooldownTimerText.transform.parent);
                ChronosChargesText.text = "";
                ChronosChargesText.enableWordWrapping = false;
                ChronosChargesText.transform.localScale = Vector3.one * 0.5f;
                ChronosChargesText.transform.localPosition += new Vector3(-0.05f, 0.7f, 0);

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
                    Utils.SendRPC(CustomRPC.SetVanish, Wraith.Player.PlayerId, byte.MinValue);
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
                    Utils.SendRPC(CustomRPC.VeteranAlert);
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
                    
                    Utils.SendRPC(CustomRPC.WerewolfMaul);
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
                        Utils.SendRPC(CustomRPC.HitmanMorph, Hitman.SampledTarget.PlayerId);
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
                        Utils.SendRPC(CustomRPC.GlitchMimic, Glitch.sampledTarget.PlayerId);
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
                    }
                },
                buttonText: "SAMPLE"
            );

            // Medic Shield
            medicShieldButton = new CustomButton(
                () => 
                {
                    medicShieldButton.Timer = 0f;
    
                    Utils.SendRPC(Medic.setShieldAfterMeeting ? CustomRPC.SetFutureShielded : CustomRPC.MedicSetShielded, Medic.CurrentTarget.PlayerId);
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

                    Utils.SendRPC(CustomRPC.RomanticSetBeloved, Romantic.CurrentTarget.PlayerId);
                    
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
                        Utils.SendRPC(CustomRPC.MorphlingMorph, Morphling.sampledTarget.PlayerId);
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
                    if (!Plaguebearer.InfectedPlayers.Contains(Plaguebearer.InfectTarget))
                    {
                        Plaguebearer.InfectedPlayers.Add(Plaguebearer.InfectTarget);
                    }
        
                    SoundEffectsManager.Play("knockKnock");

                    // Now check if everyone is infected
                    if (Plaguebearer.CanTransform()) 
                    {
                        Utils.SendRPC(CustomRPC.TurnPestilence);
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

                // Ghost Info
                Utils.SendRPC(CustomRPC.ShareGhostInfo, PlayerControl.LocalPlayer.PlayerId, (byte)GhostInfoTypes.PlaguebearerInfect, Plaguebearer.InfectTarget.PlayerId);

                Plaguebearer.InfectTarget = null;
            }
        );

            MonarchKnightButton = new CustomButton(
                OnClick: () =>
                {
                    if (Monarch.CurrentTarget.CheckVeteranPestilenceKill()) return;

                    Utils.SendRPC(CustomRPC.MonarchKnight, Monarch.CurrentTarget.PlayerId);
                    RPCProcedure.MonarchKnight(Monarch.CurrentTarget.PlayerId);
                    SoundEffectsManager.Play("Convert");

                    Monarch.CurrentTarget = null;

                    MonarchKnightButton.Timer = MonarchKnightButton.MaxTimer;
                },
                HasButton: () =>
                {
                    return Monarch.Player != null && Monarch.Player == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                CouldUse: () => 
                    {
                        if (MonarchChargesText != null) MonarchChargesText.text = $"{Monarch.Charges}";
                        return Monarch.Player != null && Monarch.Player == PlayerControl.LocalPlayer && Monarch.Charges > 0 && Monarch.CurrentTarget != null && PlayerControl.LocalPlayer.CanMove;
                    },
                OnMeetingEnds: () => { MonarchKnightButton.Timer = MonarchKnightButton.MaxTimer; },
                Sprite: Monarch.GetButtonSprite(),
                PositionOffset: CustomButton.ButtonPositions.lowerRowRight,
                hudManager: __instance,
                hotkey: KeyCode.F,
                HasEffect: true,
                EffectDuration: 0f,
                OnEffectEnds: () =>
                {
                    MonarchKnightButton.Timer = MonarchKnightButton.MaxTimer;
                },
                mirror: false,
                buttonText: "Knight"
            );
                MonarchChargesText = GameObject.Instantiate(MonarchKnightButton.actionButton.cooldownTimerText, MonarchKnightButton.actionButton.cooldownTimerText.transform.parent);
                MonarchChargesText.text = "";
                MonarchChargesText.enableWordWrapping = false;
                MonarchChargesText.transform.localScale = Vector3.one * 0.5f;
                MonarchChargesText.transform.localPosition += new Vector3(-0.05f, 0.7f, 0);

            CrusaderButton = new CustomButton(
            OnClick: () =>
            {
                if (Crusader.CurrentTarget.CheckVeteranPestilenceKill()) return;

                Utils.SendRPC(CustomRPC.Fortify, Crusader.CurrentTarget.PlayerId);
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
                
                Utils.SendRPC(CustomRPC.Confess, Oracle.CurrentTarget.PlayerId);
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

            // Painter paint
            PainterButton = new CustomButton(
                () => 
                {
                    Utils.SendRPC(CustomRPC.PainterPaint);
                    RPCProcedure.PainterPaint();
                    SoundEffectsManager.Play("morphlingMorph");
                },
                () => { return Painter.Player != null && Painter.Player == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => { return PlayerControl.LocalPlayer.CanMove; },
                () => {
                    PainterButton.Timer = PainterButton.MaxTimer;
                    PainterButton.isEffectActive = false;
                    PainterButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
                },
                Painter.GetButtonSprite(),
                CustomButton.ButtonPositions.upperRowLeft,
                __instance,
                KeyCode.F,
                true,
                Painter.Duration,
                () => {
                    PainterButton.Timer = PainterButton.MaxTimer;
                    SoundEffectsManager.Play("morphlingMorph");
                },
                buttonText: "PAINT"
            );

            // Grenadier Grenade
            GrenadierButton = new CustomButton(
                () => 
                {
                    Utils.SendRPC(CustomRPC.GrenadierFlash);
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

            // Snitch button
            SnitchButton = new CustomButton(
                () =>
                {
                    Snitch.ShouldSee = true;
                    Snitch.ShouldHaveButton = false;
                    SoundEffectsManager.Play("hackerHack");
                },
                () => { return Snitch.Player != null && Snitch.Player == PlayerControl.LocalPlayer && Snitch.ShouldHaveButton && Snitch.Active && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => { return Snitch.Active && Snitch.ShouldHaveButton; },
                () =>
                {
                    SnitchButton.Timer = SnitchButton.MaxTimer;
                    SnitchButton.isEffectActive = false;
                    SnitchButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
                },
                Snitch.GetButtonSprite(),
                CustomButton.ButtonPositions.lowerRowRight,
                __instance,
                KeyCode.F,
                true,
                0f,
                () =>
                {
                    SnitchButton.Timer = SnitchButton.MaxTimer;
                },
                buttonText: "FIND KILLER"
            );

            // Hacker button
            hackerButton = new CustomButton(
                () =>
                {
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
                CustomButton.ButtonPositions.lowerRowLeft,
                __instance,
                KeyCode.F,
                true,
                0f,
                () => { hackerButton.Timer = hackerButton.MaxTimer;}
            );

            hackerAdminTableButton = new CustomButton(
               () =>
               {
                   if (!MapBehaviour.Instance || !MapBehaviour.Instance.isActiveAndEnabled)
                   {
                       HudManager __instance = FastDestroyableSingleton<HudManager>.Instance;
                       __instance.InitMap();
                       MapBehaviour.Instance.ShowCountOverlay(allowedToMove: true, showLivePlayerPosition: true, includeDeadBodies: true);
                   }
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
                   if (GameOptionsManager.Instance.currentNormalGameOptions.MapId != 1)
                   {
                       if (Hacker.vitals == null)
                       {
                           var e = UObject.FindObjectsOfType<SystemConsole>().FirstOrDefault(x => x.gameObject.name.Contains("panel_vitals") || x.gameObject.name.Contains("Vitals"));
                           if (e == null || Camera.main == null) return;
                           Hacker.vitals = UObject.Instantiate(e.MinigamePrefab, Camera.main.transform, false);
                       }
                       Hacker.vitals.transform.SetParent(Camera.main.transform, false);
                       Hacker.vitals.transform.localPosition = new Vector3(0.0f, 0.0f, -50f);
                       Hacker.vitals.Begin(null);
                   } else {
                       if (Hacker.doorLog == null) {
                           var e = UObject.FindObjectsOfType<SystemConsole>().FirstOrDefault(x => x.gameObject.name.Contains("SurvLogConsole"));
                           if (e == null || Camera.main == null) return;
                           Hacker.doorLog = UObject.Instantiate(e.MinigamePrefab, Camera.main.transform, false);
                       }
                       Hacker.doorLog.transform.SetParent(Camera.main.transform, false);
                       Hacker.doorLog.transform.localPosition = new Vector3(0.0f, 0.0f, -50f);
                       Hacker.doorLog.Begin(null);
                   }
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
               () =>
               {
                   hackerVitalsButton.Timer = hackerVitalsButton.MaxTimer;
                   if (!hackerAdminTableButton.isEffectActive) PlayerControl.LocalPlayer.moveable = true;
                   if (Minigame.Instance)
                   {
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

                    Utils.SendRPC(CustomRPC.TrackerUsedTracker, Tracker.CurrentTarget.PlayerId);
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
                () =>
                {
                    Tracker.corpsesTrackingTimer = Tracker.corpsesTrackingDuration;
                    SoundEffectsManager.Play("trackerTrackCorpses");
                },
                () =>
                {
                    return Tracker.Player != null && Tracker.Player == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead && Tracker.canTrackCorpses;
                },
                () =>
                {
                    return PlayerControl.LocalPlayer.CanMove;
                },
                () =>
                {
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
                () =>
                {
                    trackerTrackCorpsesButton.Timer = trackerTrackCorpsesButton.MaxTimer;
                }
            );

            ScavengerScavengeButton = new CustomButton(
                () => 
                {
                    Scavenger.ScavengeTimer = Scavenger.ScavengeDuration;
                    SoundEffectsManager.Play("trackerTrackCorpses");
                },
                () => { return Scavenger.Player != null && Scavenger.Player == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => { return PlayerControl.LocalPlayer.CanMove; },
                () => 
                {
                    ScavengerScavengeButton.Timer = ScavengerScavengeButton.MaxTimer;
                    ScavengerScavengeButton.isEffectActive = false;
                    ScavengerScavengeButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
                },
                Scavenger.GetScavengeSprite(),
                CustomButton.ButtonPositions.lowerRowCenter,
                __instance,
                KeyCode.G,
                true,
                Scavenger.ScavengeDuration,
                () => 
                {
                    ScavengerScavengeButton.Timer = ScavengerScavengeButton.MaxTimer;
                    SoundEffectsManager.Play("scavengerScavenge");
                },
                buttonText: "SCAVENGE"
            );

            DisperserButton = new CustomButton(
               OnClick: () => 
                {
                    Utils.SendRPC(CustomRPC.Disperse);
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
    
            ViperKillButton = new CustomButton(
                () => 
                {
                    MurderAttemptResult murder = Utils.CheckMurderAttempt(Viper.Player, Viper.CurrentTarget);
                    if (murder == MurderAttemptResult.PerformKill) 
                    {
                        Viper.poisoned = Viper.CurrentTarget;
                         // Notify players about poisoned
                        Utils.SendRPC(CustomRPC.ViperSetPoisoned, Viper.poisoned.PlayerId, (byte)0);
                        RPCProcedure.ViperSetPoisoned(Viper.poisoned.PlayerId, 0);

                        byte lastTimer = (byte)Viper.delay;
                        FastDestroyableSingleton<HudManager>.Instance.StartCoroutine(Effects.Lerp(Viper.delay, new Action<float>((p) => { // Delayed action
                            if (p <= 1f) 
                            {
                                byte timer = (byte)ViperKillButton.Timer;
                                if (timer != lastTimer) 
                                {
                                    lastTimer = timer;
                                    Utils.SendRPC(CustomRPC.ShareGhostInfo, PlayerControl.LocalPlayer.PlayerId, (byte)GhostInfoTypes.ViperTimer, timer);
                                }
                            }
                            if (p == 1f) 
                            {
                                // Perform kill if possible and reset poisoned (regardless whether the kill was successful or not)
                                var res = Utils.CheckMurderAttemptAndKill(Viper.Player, Viper.poisoned, showAnimation: false);
                                if (res == MurderAttemptResult.PerformKill) 
                                {
                                    Utils.SendRPC(CustomRPC.ViperSetPoisoned, byte.MaxValue, byte.MaxValue);
                                    RPCProcedure.ViperSetPoisoned(byte.MaxValue, byte.MaxValue);
                                }
                            }
                        })));
                        SoundEffectsManager.Play("ViperPoison");
                        ViperKillButton.HasEffect = true; // Trigger effect on this click
                    }
                    else if (murder == MurderAttemptResult.BlankKill) 
                    {
                        ViperKillButton.Timer = ViperKillButton.MaxTimer;
                        ViperKillButton.HasEffect = false;
                    } 
                    else 
                    {
                        ViperKillButton.HasEffect = false;
                    }
                },
                () => { return Viper.Player != null && Viper.Player == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => 
                {
                    return Viper.CurrentTarget != null && PlayerControl.LocalPlayer.CanMove;
                },
                () =>
                {
                    ViperKillButton.Timer = ViperKillButton.MaxTimer;
                    ViperKillButton.isEffectActive = false;
                    ViperKillButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
                },
                Viper.GetButtonSprite(),
                CustomButton.ButtonPositions.upperRowRight,
                __instance,
                KeyCode.Q,
                false,
                0f,
                () => {
                    ViperKillButton.Timer = ViperKillButton.MaxTimer;
                },
                buttonText: "POISON"
            );

            // Viper blind button
            ViperBlindButton = new CustomButton(
                () => 
                {
                    var pos = PlayerControl.LocalPlayer.transform.position;
                    byte[] buff = new byte[sizeof(float) * 2];
                    Buffer.BlockCopy(BitConverter.GetBytes(pos.x), 0, buff, 0 * sizeof(float), sizeof(float));
                    Buffer.BlockCopy(BitConverter.GetBytes(pos.y), 0, buff, 1 * sizeof(float), sizeof(float));

                    Utils.SendRPC(CustomRPC.SetBlindTrap, buff);
                    RPCProcedure.SetBlindTrap(buff);

                    SoundEffectsManager.Play("trapperTrap");
                    ViperBlindButton.Timer = ViperBlindButton.MaxTimer;
                },
                () => 
                { 
                    return Viper.Player != null && Viper.Player == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; 
                },
                () => 
                {
                    return PlayerControl.LocalPlayer.CanMove;
                },
                () => { ViperBlindButton.Timer = ViperBlindButton.MaxTimer; },
                Viper.GetBlindSprite(),
                CustomButton.ButtonPositions.upperRowCenter,
                __instance,
                KeyCode.G,
                buttonText: "BLIND TRAP"
            );

            GatekeeperPlacePortalButton = new CustomButton(
                () =>
                {
                    GatekeeperPlacePortalButton.Timer = GatekeeperPlacePortalButton.MaxTimer;

                    var pos = PlayerControl.LocalPlayer.transform.position;
                    byte[] buff = new byte[sizeof(float) * 2];
                    Buffer.BlockCopy(BitConverter.GetBytes(pos.x), 0, buff, 0 * sizeof(float), sizeof(float));
                    Buffer.BlockCopy(BitConverter.GetBytes(pos.y), 0, buff, 1 * sizeof(float), sizeof(float));

                    Utils.SendRPC(CustomRPC.PlacePortal, buff);
                    RPCProcedure.PlacePortal(buff);
                    SoundEffectsManager.Play("tricksterPlaceBox");
                },
                () => { return Gatekeeper.Player != null && Gatekeeper.Player == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead && Portal.secondPortal == null; },
                () => { return PlayerControl.LocalPlayer.CanMove && Portal.secondPortal == null; },
                () => { GatekeeperPlacePortalButton.Timer = GatekeeperPlacePortalButton.MaxTimer; },
                Gatekeeper.GetPlacePortalButtonSprite(),
                CustomButton.ButtonPositions.lowerRowRight,
                __instance,
                KeyCode.F,
                buttonText: "CAST PORTAL"
            );

            MinerMineButton = new CustomButton(
                () => 
                {
                    MinerMineButton.Timer = MinerMineButton.MaxTimer;

                    var pos = PlayerControl.LocalPlayer.transform.position;
                    byte[] buff = new byte[sizeof(float) * 2];
                    Buffer.BlockCopy(BitConverter.GetBytes(pos.x), 0, buff, 0 * sizeof(float), sizeof(float));
                    Buffer.BlockCopy(BitConverter.GetBytes(pos.y), 0, buff, 1 * sizeof(float), sizeof(float));

                    Utils.SendRPC(CustomRPC.PlaceMine, buff);
                    RPCProcedure.PlaceMine(buff);
                    SoundEffectsManager.Play("tricksterPlaceBox");
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
                        Utils.SendRPC(CustomRPC.UsePortal, (byte)PlayerControl.LocalPlayer.PlayerId, portalMakerSoloTeleport ? (byte)1 : (byte)0);
                    }
                    RPCProcedure.UsePortal(PlayerControl.LocalPlayer.PlayerId, portalMakerSoloTeleport ? (byte)1 : (byte)0);
                    usePortalButton.Timer = usePortalButton.MaxTimer;
                    GatekeeperMoveToPortalButton.Timer = usePortalButton.MaxTimer;
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
                    if (PlayerControl.LocalPlayer == Gatekeeper.Player && Portal.bothPlacedAndEnabled)
                        GatekeeperButtonText1.text = Portal.LocationNearEntry(PlayerControl.LocalPlayer.transform.position) || !Gatekeeper.canPortalFromAnywhere ? "" : "1. " + Portal.firstPortal.room;
                    return Portal.bothPlacedAndEnabled; },
                () => { return PlayerControl.LocalPlayer.CanMove && (Portal.LocationNearEntry(PlayerControl.LocalPlayer.transform.position) || Gatekeeper.canPortalFromAnywhere && PlayerControl.LocalPlayer == Gatekeeper.Player) && !Portal.isTeleporting; },
                () => { usePortalButton.Timer = usePortalButton.MaxTimer; },
                Gatekeeper.getUsePortalButtonSprite(),
                new Vector3(0.9f, -0.06f, 0),
                __instance,
                KeyCode.J,
                mirror: true
            );

            GatekeeperMoveToPortalButton = new CustomButton(
                () => 
                {
                    bool didTeleport = false;
                    Vector3 exit = Portal.secondPortal.portalGameObject.transform.position;

                    if (!PlayerControl.LocalPlayer.Data.IsDead) 
                    {  
                        // Ghosts can portal too, but non-blocking and only with a local animation
                        Utils.SendRPC(CustomRPC.UsePortal, PlayerControl.LocalPlayer.PlayerId, (byte)2);
                    }
                    RPCProcedure.UsePortal(PlayerControl.LocalPlayer.PlayerId, 2);
                    usePortalButton.Timer = usePortalButton.MaxTimer;
                    GatekeeperMoveToPortalButton.Timer = usePortalButton.MaxTimer;
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
                () => { return Gatekeeper.canPortalFromAnywhere && Portal.bothPlacedAndEnabled && PlayerControl.LocalPlayer == Gatekeeper.Player; },
                () => { return PlayerControl.LocalPlayer.CanMove && !Portal.LocationNearEntry(PlayerControl.LocalPlayer.transform.position) && !Portal.isTeleporting; },
                () => { GatekeeperMoveToPortalButton.Timer = usePortalButton.MaxTimer; },
                Gatekeeper.getUsePortalButtonSprite(),
                new Vector3(0.9f, 1f, 0),
                __instance,
                KeyCode.G,
                mirror: true,
                buttonText: "USE"
            );


            GatekeeperButtonText1 = GameObject.Instantiate(usePortalButton.actionButton.cooldownTimerText, usePortalButton.actionButton.cooldownTimerText.transform.parent);
            GatekeeperButtonText1.text = "";
            GatekeeperButtonText1.enableWordWrapping = false;
            GatekeeperButtonText1.transform.localScale = Vector3.one * 0.5f;
            GatekeeperButtonText1.transform.localPosition += new Vector3(-0.05f, 0.55f, -1f);

            GatekeeperButtonText2 = GameObject.Instantiate(GatekeeperMoveToPortalButton.actionButton.cooldownTimerText, GatekeeperMoveToPortalButton.actionButton.cooldownTimerText.transform.parent);
            GatekeeperButtonText2.text = "";
            GatekeeperButtonText2.enableWordWrapping = false;
            GatekeeperButtonText2.transform.localScale = Vector3.one * 0.5f;
            GatekeeperButtonText2.transform.localPosition += new Vector3(-0.05f, 0.55f, -1f);


            // Jackal Recruit Button
            jackalRecruitButton = new CustomButton(
                () => 
                {
                    if (Jackal.CurrentTarget.CheckVeteranPestilenceKill() || Jackal.CurrentTarget.CheckFortifiedPlayer()) return;

                    Utils.SendRPC(CustomRPC.JackalCreatesRecruit, Jackal.CurrentTarget.PlayerId);
                    RPCProcedure.JackalCreatesRecruit(Jackal.CurrentTarget.PlayerId);
                    SoundEffectsManager.Play("Convert");
                },
                () => { return Jackal.canCreateRecruit && Jackal.Player != null && Jackal.Player == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => { return Jackal.canCreateRecruit && Jackal.CurrentTarget != null && PlayerControl.LocalPlayer.CanMove; },
                () => { jackalRecruitButton.Timer = jackalRecruitButton.MaxTimer;},
                Jackal.getRecruitButtonSprite(),
                CustomButton.ButtonPositions.lowerRowCenter,
                __instance,
                KeyCode.F,
                buttonText: "RECRUIT"
            );

            // Cultist Recruit Button
            CultistButton = new CustomButton(
                () => 
                {
                    if (Cultist.CurrentTarget.CheckVeteranPestilenceKill() || Cultist.CurrentTarget.CheckFortifiedPlayer()) return;

                    Utils.SendRPC(CustomRPC.CultistCreatesFollower, Cultist.CurrentTarget.PlayerId);
                    RPCProcedure.CultistCreatesFollower(Cultist.CurrentTarget.PlayerId);
                    SoundEffectsManager.Play("Convert");
                },
                () => { return !Cultist.HasFollower && Cultist.Player != null && Cultist.Player == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => { return !Cultist.HasFollower && Cultist.CurrentTarget != null && PlayerControl.LocalPlayer.CanMove; },
                () => { CultistButton.Timer = CultistButton.MaxTimer;},
                Cultist.GetButtonSprite(),
                CustomButton.ButtonPositions.lowerRowCenter,
                __instance,
                KeyCode.F,
                buttonText: "CREATE FOLLOWER"
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
                () =>
                {
                    bool canKill = !Jackal.canCreateRecruit || (Jackal.canCreateRecruit && Jackal.HasRecruit);
                    return Jackal.Player != null && Jackal.Player == PlayerControl.LocalPlayer && canKill && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => { return Jackal.CurrentTarget && PlayerControl.LocalPlayer.CanMove; },
                () => { jackalKillButton.Timer = jackalKillButton.MaxTimer;},
                __instance.KillButton.graphic.sprite,
                CustomButton.ButtonPositions.upperRowRight,
                __instance,
                KeyCode.Q
            );
            
            // Recruit Kill
            RecruitKillButton = new CustomButton(
                () => 
                {
                    if (Recruit.CurrentTarget.CheckVeteranPestilenceKill()  || Recruit.CurrentTarget.CheckFortifiedPlayer()) return;

                    if (Utils.CheckMurderAttemptAndKill(Recruit.Player, Recruit.CurrentTarget) == MurderAttemptResult.SuppressKill) return;
                    RecruitKillButton.Timer = RecruitKillButton.MaxTimer; 
                    Recruit.CurrentTarget = null;
                },
                () => { return Recruit.Player != null && Recruit.Player == PlayerControl.LocalPlayer
                && !PlayerControl.LocalPlayer.Data.IsDead && !PlayerControl.LocalPlayer.IsNeutralKiller()
                && !PlayerControl.LocalPlayer.IsImpostor() && PlayerControl.LocalPlayer != Sheriff.Player; },
                () => { return Recruit.CurrentTarget && PlayerControl.LocalPlayer.CanMove; },
                () => { RecruitKillButton.Timer = RecruitKillButton.MaxTimer;},
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

                    Utils.SendRPC(CustomRPC.SetFutureErased, Eraser.CurrentTarget.PlayerId);
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
                () =>
                {
                    placeJackInTheBoxButton.Timer = placeJackInTheBoxButton.MaxTimer;

                    var pos = PlayerControl.LocalPlayer.transform.position;
                    byte[] buff = new byte[sizeof(float) * 2];
                    Buffer.BlockCopy(BitConverter.GetBytes(pos.x), 0, buff, 0*sizeof(float), sizeof(float));
                    Buffer.BlockCopy(BitConverter.GetBytes(pos.y), 0, buff, 1*sizeof(float), sizeof(float));

                    Utils.SendRPC(CustomRPC.PlaceJackInTheBox, buff);
                    RPCProcedure.PlaceJackInTheBox(buff);
                    SoundEffectsManager.Play("tricksterPlaceBox");
                },
                () => { return Trickster.Player != null && Trickster.Player == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead && !JackInTheBox.HasJackInTheBoxLimitReached(); },
                () => { return PlayerControl.LocalPlayer.CanMove && !JackInTheBox.HasJackInTheBoxLimitReached(); },
                () => { placeJackInTheBoxButton.Timer = placeJackInTheBoxButton.MaxTimer;},
                Trickster.GetPlaceBoxButtonSprite(),
                CustomButton.ButtonPositions.upperRowLeft,
                __instance,
                KeyCode.F,
                buttonText: "PLACE BOX"
            );
            
            lightsOutButton = new CustomButton(
                () => 
                {
                    Utils.SendRPC(CustomRPC.LightsOut);
                    RPCProcedure.LightsOut();
                    SoundEffectsManager.Play("lighterLight");
                },
                () => { return Trickster.Player != null && Trickster.Player == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead
                                                           && JackInTheBox.HasJackInTheBoxLimitReached() && JackInTheBox.boxesConvertedToVents; },
                () => { return PlayerControl.LocalPlayer.CanMove && JackInTheBox.HasJackInTheBoxLimitReached() && JackInTheBox.boxesConvertedToVents; },
                () =>
                { 
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

            // Janitor Clean
            JanitorCleanButton = new CustomButton(
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
                                    
                                    Utils.SendRPC(CustomRPC.CleanBody, playerInfo.PlayerId, Janitor.Player.PlayerId);
                                    RPCProcedure.CleanBody(playerInfo.PlayerId, Janitor.Player.PlayerId);

                                    Janitor.Player.killTimer = GameOptionsManager.Instance.currentNormalGameOptions.KillCooldown;
                                    JanitorCleanButton.Timer = JanitorCleanButton.MaxTimer;
                                    SoundEffectsManager.Play("JanitorClean");
                                    break;
                                }
                            }
                        }
                    }
                },
                () => { return Janitor.Player != null && Janitor.Player == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => { return __instance.ReportButton.graphic.color == Palette.EnabledColor && PlayerControl.LocalPlayer.CanMove; },
                () => { JanitorCleanButton.Timer = JanitorCleanButton.MaxTimer; },
                Janitor.GetButtonSprite(),
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
                        Utils.SendRPC(CustomRPC.ShareGhostInfo, PlayerControl.LocalPlayer.PlayerId, (byte)GhostInfoTypes.WarlockTarget, Warlock.curseVictim.PlayerId);

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
                        Utils.SendRPC(CustomRPC.ShareGhostInfo, PlayerControl.LocalPlayer.PlayerId, (byte)GhostInfoTypes.WarlockTarget, Byte.MaxValue);

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
                        Utils.SendRPC(CustomRPC.SealVent, Vigilante.ventTarget.Id);
                        RPCProcedure.SealVent(Vigilante.ventTarget.Id);
                        Vigilante.ventTarget = null;
                        
                    } else if (!Utils.IsMira() && !Utils.IsFungle() && !SubmergedCompatibility.IsSubmerged) { // Place camera if there's no vent and it's not MiraHQ or Submerged
                        var pos = PlayerControl.LocalPlayer.transform.position;
                        byte[] buff = new byte[sizeof(float) * 2];
                        Buffer.BlockCopy(BitConverter.GetBytes(pos.x), 0, buff, 0*sizeof(float), sizeof(float));
                        Buffer.BlockCopy(BitConverter.GetBytes(pos.y), 0, buff, 1*sizeof(float), sizeof(float));

                        Utils.SendRPC(CustomRPC.PlaceCamera, buff);
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
                            var e = UObject.FindObjectsOfType<SystemConsole>().FirstOrDefault(x => x.gameObject.name.Contains("Surv_Panel") || x.name.Contains("Cam") || x.name.Contains("BinocularsSecurityConsole"));
                            if (Utils.IsSkeld() || mapId == 3) e = UObject.FindObjectsOfType<SystemConsole>().FirstOrDefault(x => x.gameObject.name.Contains("SurvConsole"));
                            else if (Utils.IsAirship()) e = UObject.FindObjectsOfType<SystemConsole>().FirstOrDefault(x => x.gameObject.name.Contains("task_cams"));
                            if (e == null || Camera.main == null) return;
                            Vigilante.minigame = UObject.Instantiate(e.MinigamePrefab, Camera.main.transform, false);
                        }
                        Vigilante.minigame.transform.SetParent(Camera.main.transform, false);
                        Vigilante.minigame.transform.localPosition = new Vector3(0.0f, 0.0f, -50f);
                        Vigilante.minigame.Begin(null);
                    } else {
                        if (Vigilante.minigame == null) {
                            var e = UObject.FindObjectsOfType<SystemConsole>().FirstOrDefault(x => x.gameObject.name.Contains("SurvLogConsole"));
                            if (e == null || Camera.main == null) return;
                            Vigilante.minigame = UObject.Instantiate(e.MinigamePrefab, Camera.main.transform, false);
                        }
                        Vigilante.minigame.transform.SetParent(Camera.main.transform, false);
                        Vigilante.minigame.transform.localPosition = new Vector3(0.0f, 0.0f, -50f);
                        Vigilante.minigame.Begin(null);
                    }
                    Vigilante.Charges--;
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
                        Utils.SendRPC(CustomRPC.ArsonistWin);
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
                    Utils.SendRPC(CustomRPC.ShareGhostInfo, PlayerControl.LocalPlayer.PlayerId, (byte)GhostInfoTypes.ArsonistDouse, Arsonist.douseTarget.PlayerId);
                    Arsonist.douseTarget = null;
                }
            );

            // Scavenger Eat
            ScavengerEatButton = new CustomButton(
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

                                    Utils.SendRPC(CustomRPC.CleanBody, playerInfo.PlayerId, Scavenger.Player.PlayerId);
                                    RPCProcedure.CleanBody(playerInfo.PlayerId, Scavenger.Player.PlayerId);

                                    Scavenger.Cooldown = ScavengerEatButton.Timer = ScavengerEatButton.MaxTimer;
                                    SoundEffectsManager.Play("ScavengerEat");
                                    break;
                                }
                            }
                        }
                    }
                },
                () => { return Scavenger.Player != null && Scavenger.Player == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => { return __instance.ReportButton.graphic.color == Palette.EnabledColor && PlayerControl.LocalPlayer.CanMove; },
                () => { ScavengerEatButton.Timer = ScavengerEatButton.MaxTimer; },
                Scavenger.GetButtonSprite(),
                CustomButton.ButtonPositions.upperRowRight,
                __instance,
                KeyCode.F,
                buttonText: "EAT"
            );

            // Psychic button
            PsychicButton = new CustomButton(
                () => 
                {
                    if (Psychic.target != null)
                    {
                        Psychic.soulTarget = Psychic.target;
                        PsychicButton.HasEffect = true;
                        SoundEffectsManager.Play("PsychicAsk");
                    }
                },
                () =>
                {
                    return Psychic.Player != null && Psychic.Player == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () =>
                {
                    if (PsychicButton.isEffectActive && Psychic.target != Psychic.soulTarget)
                    {
                        Psychic.soulTarget = null;
                        PsychicButton.Timer = 0f;
                        PsychicButton.isEffectActive = false;
                    }
                    return Psychic.target != null && PlayerControl.LocalPlayer.CanMove;
                },
                () =>
                {
                    PsychicButton.Timer = PsychicButton.MaxTimer;
                    PsychicButton.isEffectActive = false;
                    Psychic.soulTarget = null;
                },
                Psychic.GetQuestionSprite(),
                CustomButton.ButtonPositions.lowerRowRight,
                __instance,
                KeyCode.F,
                true,
                Psychic.Duration,
                () => 
                {
                    PsychicButton.Timer = PsychicButton.MaxTimer;
                    if (Psychic.target == null || Psychic.target.player == null) return;
                    string msg = Psychic.GetInfo(Psychic.target.player, Psychic.target.GetKiller, Psychic.target.DeathReason);
                    FastDestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, msg);
                    // Ghost Info
                    Utils.SendRPC(CustomRPC.ShareGhostInfo, Psychic.target.player.PlayerId, (byte)GhostInfoTypes.PsychicInfo, msg);

                    // Remove soul
                    if (Psychic.oneTimeUse)
                    {
                        float closestDistance = float.MaxValue;
                        SpriteRenderer target = null;

                        foreach ((DeadPlayer db, Vector3 ps) in Psychic.deadBodies)
                        {
                            if (db == Psychic.target)
                            {
                                Tuple<DeadPlayer, Vector3> deadBody = Tuple.Create(db, ps);
                                Psychic.deadBodies.Remove(deadBody);
                                break;
                            }

                        }
                        foreach (SpriteRenderer rend in Psychic.souls)
                        {
                            float distance = Vector2.Distance(rend.transform.position, PlayerControl.LocalPlayer.GetTruePosition());
                            if (distance < closestDistance)
                            {
                                closestDistance = distance;
                                target = rend;
                            }
                        }

                        FastDestroyableSingleton<HudManager>.Instance.StartCoroutine(Effects.Lerp(5f, new Action<float>((p) => {
                            if (target != null)
                            {
                                var tmp = target.color;
                                tmp.a = Mathf.Clamp01(1 - p);
                                target.color = tmp;
                            }
                            if (p == 1f && target != null && target.gameObject != null) UObject.Destroy(target.gameObject);
                        })));

                        Psychic.souls.Remove(target);
                    }
                    SoundEffectsManager.Stop("PsychicAsk");
                },
                buttonText: "ASK"
            );

            // Survivor button
            SurvivorButton = new CustomButton(
                () => {
                    if (Survivor.target != null) 
                    {
                        if (Survivor.target.CheckVeteranPestilenceKill() || Survivor.target.CheckFortifiedPlayer()) return;
                        
                        Utils.SendRPC(CustomRPC.SetBlanked, Survivor.target.PlayerId, Byte.MaxValue);
                        RPCProcedure.SetBlanked(Survivor.target.PlayerId, Byte.MaxValue);

                        Survivor.target = null;

                        Survivor.blanks++;
                        SurvivorButton.Timer = SurvivorButton.MaxTimer;
                        SoundEffectsManager.Play("SurvivorBlank");
                    }

                },
                () => { return Survivor.Player != null && Survivor.Player == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead && Survivor.blanks < Survivor.blanksNumber; },
                () => {
                    if (SurvivorButtonBlanksText != null) SurvivorButtonBlanksText.text = $"{Survivor.blanksNumber - Survivor.blanks}";

                    return Survivor.blanksNumber > Survivor.blanks && PlayerControl.LocalPlayer.CanMove && Survivor.target != null;
                },
                () => { SurvivorButton.Timer = SurvivorButton.MaxTimer; },
                Survivor.GetTargetSprite(),
                CustomButton.ButtonPositions.lowerRowRight,
                __instance,
                KeyCode.F,
                buttonText: "BLANK"
            );

            // Survivor button blanks left
            SurvivorButtonBlanksText = GameObject.Instantiate(SurvivorButton.actionButton.cooldownTimerText, SurvivorButton.actionButton.cooldownTimerText.transform.parent);
            SurvivorButtonBlanksText.text = "";
            SurvivorButtonBlanksText.enableWordWrapping = false;
            SurvivorButtonBlanksText.transform.localScale = Vector3.one * 0.5f;
            SurvivorButtonBlanksText.transform.localPosition += new Vector3(-0.05f, 0.7f, 0);


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
                    MurderAttemptResult attempt = Utils.CheckMurderAttempt(Witch.Player, Witch.spellCastingTarget);
                    if (attempt == MurderAttemptResult.PerformKill) 
                    {
                        Utils.SendRPC(CustomRPC.SetFutureSpelled, Witch.CurrentTarget.PlayerId);
                        RPCProcedure.SetFutureSpelled(Witch.CurrentTarget.PlayerId);
                    }
                    if (attempt == MurderAttemptResult.BlankKill || attempt == MurderAttemptResult.PerformKill) 
                    {
                        Witch.currentCooldownAddition += Witch.CooldownAddition;
                        witchSpellButton.MaxTimer = Witch.Cooldown + Witch.currentCooldownAddition;
                        witchSpellButton.Timer = witchSpellButton.MaxTimer;
                        if (Witch.triggerBothCooldowns)
                        {
                            float multiplier = 1f;
                            Witch.Player.killTimer = GameOptionsManager.Instance.currentNormalGameOptions.KillCooldown * multiplier;
                        }
                    }
                    else
                    {
                        witchSpellButton.Timer = 0f;
                    }
                    Witch.spellCastingTarget = null;
                },
                buttonText: "SPELL"
            );

            // Assassin mark and assassinate button 
            AssassinButton = new CustomButton(
                () => 
                {
                    if (Assassin.AssassinMarked != null) 
                    {
                        // Murder attempt with teleport
                        MurderAttemptResult attempt = Utils.CheckMurderAttempt(Assassin.Player, Assassin.AssassinMarked);
                        if (attempt == MurderAttemptResult.PerformKill) 
                        {
                            // Create first trace before killing
                            var pos = PlayerControl.LocalPlayer.transform.position;
                            byte[] buff = new byte[sizeof(float) * 2];
                            Buffer.BlockCopy(BitConverter.GetBytes(pos.x), 0, buff, 0 * sizeof(float), sizeof(float));
                            Buffer.BlockCopy(BitConverter.GetBytes(pos.y), 0, buff, 1 * sizeof(float), sizeof(float));

                            Utils.SendRPC(CustomRPC.PlaceAssassinTrace, buff);
                            RPCProcedure.PlaceAssassinTrace(buff);

                            Utils.SendRPC(CustomRPC.SetInvisible, Assassin.Player.PlayerId, byte.MinValue);
                            RPCProcedure.SetInvisible(Assassin.Player.PlayerId, byte.MinValue);

                            // Perform Kill
                            Utils.RpcMurderPlayer(PlayerControl.LocalPlayer, Assassin.AssassinMarked, true);
                            if (SubmergedCompatibility.IsSubmerged) 
                            {
                                SubmergedCompatibility.ChangeFloor(Assassin.AssassinMarked.transform.localPosition.y > -7);
                            }
                            // Create Second trace after killing
                            pos = Assassin.AssassinMarked.transform.position;
                            buff = new byte[sizeof(float) * 2];
                            Buffer.BlockCopy(BitConverter.GetBytes(pos.x), 0, buff, 0 * sizeof(float), sizeof(float));
                            Buffer.BlockCopy(BitConverter.GetBytes(pos.y), 0, buff, 1 * sizeof(float), sizeof(float));

                            Utils.SendRPC(CustomRPC.PlaceAssassinTrace, buff);
                            RPCProcedure.PlaceAssassinTrace(buff);
                        }

                        if (attempt == MurderAttemptResult.BlankKill || attempt == MurderAttemptResult.PerformKill) 
                        {
                            AssassinButton.Timer = AssassinButton.MaxTimer;
                            Assassin.Player.killTimer = GameOptionsManager.Instance.currentNormalGameOptions.KillCooldown;
                        } 
                        else if (attempt == MurderAttemptResult.SuppressKill) 
                        {
                            AssassinButton.Timer = 0f;
                        }
                        Assassin.AssassinMarked = null;
                        return;
                    } 
                    if (Assassin.CurrentTarget != null) 
                    {
                        Assassin.AssassinMarked = Assassin.CurrentTarget;
                        AssassinButton.Timer = 5f;
                        SoundEffectsManager.Play("warlockCurse");

                        // Ghost Info
                        Utils.SendRPC(CustomRPC.ShareGhostInfo, PlayerControl.LocalPlayer.PlayerId, (byte)GhostInfoTypes.AssassinMarked, Assassin.AssassinMarked.PlayerId);
                    }
                },
                () => { return Assassin.Player != null && Assassin.Player == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => {  // CouldUse
                    AssassinButton.Sprite = Assassin.AssassinMarked != null ? Assassin.GetKillButtonSprite() : Assassin.GetMarkButtonSprite(); 
                    return (Assassin.CurrentTarget != null || Assassin.AssassinMarked != null && !TransportationToolPatches.IsUsingTransportation(Assassin.AssassinMarked)) && PlayerControl.LocalPlayer.CanMove;
                },
                () => {  // on meeting ends
                    AssassinButton.Timer = AssassinButton.MaxTimer;
                    Assassin.AssassinMarked = null;
                },
                Assassin.GetMarkButtonSprite(),
                CustomButton.ButtonPositions.upperRowLeft,
                __instance,
                KeyCode.F                   
            );

            mayorMeetingButton = new CustomButton(
               () => 
               {
                   PlayerControl.LocalPlayer.NetTransform.Halt(); // Stop current movement 
                   Mayor.remoteMeetingsLeft--;
                   Utils.HandlePoisonedOnBodyReport(); // Manually call Viper handling, since the CmdReportDeadBody Prefix won't be called
                   RPCProcedure.UncheckedCmdReportDeadBody(PlayerControl.LocalPlayer.PlayerId, Byte.MaxValue);
                   Utils.SendRPC(CustomRPC.UncheckedCmdReportDeadBody, PlayerControl.LocalPlayer.PlayerId, Byte.MaxValue);
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
                   Utils.HandlePoisonedOnBodyReport(); // Manually call Viper handling, since the CmdReportDeadBody Prefix won't be called
                   RPCProcedure.UncheckedCmdReportDeadBody(PlayerControl.LocalPlayer.PlayerId, Byte.MaxValue);
                   Utils.SendRPC(CustomRPC.UncheckedCmdReportDeadBody, PlayerControl.LocalPlayer.PlayerId, Byte.MaxValue);
               },
               () => { return Coward.Player != null && Coward.CanUse && Coward.Player == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
               () => 
               {
                   return PlayerControl.LocalPlayer.CanMove && Coward.CanUse;
               },
               () => { CowardButton.Timer = CowardButton.MaxTimer; },
               Mayor.GetMeetingSprite(),
               new Vector3(0, -0.06f, 0),
               __instance,
               KeyCode.H,
               true,
               0f,
               () => {},
               mirror: true,
               buttonText: "Meeting"
           );

            // Trapper button
            trapperButton = new CustomButton(
                () => 
                {
                    var pos = PlayerControl.LocalPlayer.transform.position;
                    byte[] buff = new byte[sizeof(float) * 2];
                    Buffer.BlockCopy(BitConverter.GetBytes(pos.x), 0, buff, 0 * sizeof(float), sizeof(float));
                    Buffer.BlockCopy(BitConverter.GetBytes(pos.y), 0, buff, 1 * sizeof(float), sizeof(float));

                    Utils.SendRPC(CustomRPC.SetTrap, buff);
                    RPCProcedure.SetTrap(buff);

                    SoundEffectsManager.Play("trapperTrap");
                    trapperButton.Timer = trapperButton.MaxTimer;
                },
                () => 
                { 
                    return Trapper.Player != null && Trapper.Player == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; 
                },
                () => 
                {
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
                        Utils.SendRPC(CustomRPC.YoyoMarkLocation, buff);
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
                        Utils.SendRPC(CustomRPC.YoyoBlink, Byte.MaxValue, buff);
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
                    Utils.SendRPC(CustomRPC.YoyoBlink, (byte)0, buff);
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
               () =>
               {
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