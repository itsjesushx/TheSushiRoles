using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Assets.CoreScripts;
using Sentry.Internal.Extensions;

namespace TheSushiRoles.Patches 
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.FixedUpdate))]
    public static class PlayerControlFixedUpdatePatch 
    {
        // Helpers
        static PlayerControl SetTarget(bool onlyCrewmates = false, bool targetPlayersInVents = false, List<PlayerControl> untargetablePlayers = null, PlayerControl targetingPlayer = null) 
        {
            PlayerControl result = null;
            float num = AmongUs.GameOptions.NormalGameOptionsV07.KillDistances[Mathf.Clamp(GameOptionsManager.Instance.currentNormalGameOptions.KillDistance, 0, 2)];
            if (!MapUtilities.CachedShipStatus) return result;
            if (targetingPlayer == null) targetingPlayer = PlayerControl.LocalPlayer;
            if (targetingPlayer.Data.IsDead) return result;

            Vector2 truePosition = targetingPlayer.GetTruePosition();
            foreach (var playerInfo in GameData.Instance.AllPlayers.GetFastEnumerator())
            {
                if (!playerInfo.Disconnected && playerInfo.PlayerId != targetingPlayer.PlayerId && !playerInfo.IsDead && (!onlyCrewmates || !playerInfo.Role.IsImpostor)) {
                    PlayerControl @object = playerInfo.Object;
                    if (untargetablePlayers != null && untargetablePlayers.Any(x => x == @object)) 
                    {
                        // if that player is not targetable: skip check
                        continue;
                    }

                    if (@object && (!@object.inVent || targetPlayersInVents)) 
                    {
                        Vector2 vector = @object.GetTruePosition() - truePosition;
                        float magnitude = vector.magnitude;
                        if (magnitude <= num && !PhysicsHelpers.AnyNonTriggersBetween(truePosition, vector.normalized, magnitude, Constants.ShipAndObjectsMask))
                        {
                            result = @object;
                            num = magnitude;
                        }
                    }
                }
            }
            return Utils.CheckForLandlordTargets(result);
        }

        static void SetPlayerOutline(PlayerControl target, Color Color) 
        {
            if (target == null || target.cosmetics?.currentBodySprite?.BodySprite == null) return;

            Color = Color.SetAlpha(Chameleon.Visibility(target.PlayerId));
            target.cosmetics.currentBodySprite.BodySprite.material.SetFloat("_Outline", 1f);
            target.cosmetics.currentBodySprite.BodySprite.material.SetColor("_OutlineColor", Color);
        }

        // Update functions

        static void SetBasePlayerOutlines() 
        {
            foreach (PlayerControl target in PlayerControl.AllPlayerControls) 
            {
                if (target == null || target.cosmetics?.currentBodySprite?.BodySprite == null) continue;

                bool isMorphedMorphling = target == Morphling.Player && Morphling.morphTarget != null && Morphling.morphTimer > 0f;
                bool isMimicGlitch = target == Glitch.Player && Glitch.MimicTarget != null && Glitch.MimicTimer > 0f;
                bool isMorphedHitman = target == Hitman.Player && Hitman.MorphTarget != null && Hitman.MorphTimer > 0f;
                bool hasVisibleShield = false;
                Color Color = Medic.ShieldedColor;
                if (Painter.PaintTimer <= 0f && !Utils.MushroomSabotageActive() && Medic.ShieldVisible(target))
                    hasVisibleShield = true;

                if (Painter.PaintTimer <= 0f && !Utils.MushroomSabotageActive() && MapOptions.FirstPlayerKilled != null && MapOptions.ShieldFirstKill && 
                    ((target == MapOptions.FirstPlayerKilled && !isMorphedMorphling && !isMimicGlitch && !isMorphedHitman) || 
                    (isMorphedMorphling && Morphling.morphTarget == MapOptions.FirstPlayerKilled) || 
                    (isMimicGlitch && Glitch.MimicTarget == MapOptions.FirstPlayerKilled) || 
                    (isMorphedHitman && Hitman.MorphTarget == MapOptions.FirstPlayerKilled))) 
                {
                    hasVisibleShield = true;
                    Color = Color.blue;
                }

                if (PlayerControl.LocalPlayer.Data.IsDead && Lucky.Player != null && target == Lucky.Player && !Lucky.ProtectionBroken && !hasVisibleShield) 
                {
                    hasVisibleShield = true;
                    Color = Color.yellow;
                }

                if (hasVisibleShield) 
                {
                    target.cosmetics.currentBodySprite.BodySprite.material.SetFloat("_Outline", 1f);
                    target.cosmetics.currentBodySprite.BodySprite.material.SetColor("_OutlineColor", Color);
                }
                else 
                {
                    target.cosmetics.currentBodySprite.BodySprite.material.SetFloat("_Outline", 0f);
                }                
            }
        }

        static void SetPetVisibility() 
        {
            bool localalive = PlayerControl.LocalPlayer.Data.IsDead;
            foreach (var player in PlayerControl.AllPlayerControls)
            {
                bool playeralive = !player.Data.IsDead;
                player.cosmetics.SetPetVisible((localalive && playeralive) || !localalive);
            }
        }

        public static void BendTimeUpdate() 
        {
            if (Chronos.isRewinding)
            {
                if (GameHistory.LocalPlayerPositions.Count > 0)
                {
                    // Set position
                    var next = GameHistory.LocalPlayerPositions[0];
                    if (next.Item2 == true) 
                    {
                        // Exit current vent if necessary
                        if (PlayerControl.LocalPlayer.inVent) 
                        {
                            foreach (Vent vent in MapUtilities.CachedShipStatus.AllVents) 
                            {
                                bool canUse;
                                bool couldUse;
                                vent.CanUse(PlayerControl.LocalPlayer.Data, out canUse, out couldUse);
                                if (canUse)
                                {
                                    PlayerControl.LocalPlayer.MyPhysics.RpcExitVent(vent.Id);
                                    vent.SetButtons(false);
                                }
                            }
                        }
                        // Set position
                        PlayerControl.LocalPlayer.transform.position = next.Item1;
                    }
                    else if (GameHistory.LocalPlayerPositions.Any(x => x.Item2 == true)) 
                    {
                        PlayerControl.LocalPlayer.transform.position = next.Item1;
                    }
                    if (SubmergedCompatibility.IsSubmerged)
                    {
                        SubmergedCompatibility.ChangeFloor(next.Item1.y > -7);
                    }
                    foreach (var deadPlayer in GameHistory.deadPlayers.ToList())
                    {
                        if (deadPlayer.player == null) continue;
                        if (Undertaker.CurrentTarget != null)
                        {
                            Utils.SendRPC(CustomRPC.DropBody, deadPlayer.player.Data.PlayerId);
                            RPCProcedure.DropBody(deadPlayer.player.Data.PlayerId);
                        }

                        if (Hitman.BodyTarget != null)
                        {
                            Utils.SendRPC(CustomRPC.HitmanDropBody, deadPlayer.player.Data.PlayerId);
                            RPCProcedure.HitmanDropBody(deadPlayer.player.Data.PlayerId);
                        }
                        
                        if ((DateTime.UtcNow - deadPlayer.DeathTime).TotalSeconds <
                            Chronos.RewindTimeDuration && Chronos.ReviveDuringRewind)
                        {
                            var player = deadPlayer.player;
                            if (player.Data.IsDead)
                            {
                                Utils.Revive(player);
                                GameHistory.deadPlayers.Remove(deadPlayer); // Clean up as they got revived.
                            }
                        }
                    }
                    GameHistory.LocalPlayerPositions.RemoveAt(0);
                }
                else
                {
                    Chronos.isRewinding = false;
                    PlayerControl.LocalPlayer.moveable = true;
                    Chronos.RecentlyDied.Remove(PlayerControl.LocalPlayer.PlayerId);
                }
            }
            else
            {
                while (GameHistory.LocalPlayerPositions.Count >= Mathf.Round(Chronos.RewindTimeDuration / Time.fixedDeltaTime)) GameHistory.LocalPlayerPositions.RemoveAt(GameHistory.LocalPlayerPositions.Count - 1);
                GameHistory.LocalPlayerPositions.Insert(0, new Tuple<Vector3, bool>(PlayerControl.LocalPlayer.transform.position, PlayerControl.LocalPlayer.CanMove)); // CanMove = CanMove
            }
        }

        static void MedicSetTarget() 
        {
            if (Medic.Player == null || Medic.Player != PlayerControl.LocalPlayer) return;
            Medic.CurrentTarget = SetTarget();
            if (!Medic.usedShield) SetPlayerOutline(Medic.CurrentTarget, Medic.ShieldedColor);
        }

        static void CrusaderSetTarget() 
        {
            if (Crusader.Player == null || Crusader.Player != PlayerControl.LocalPlayer) return;
            Crusader.CurrentTarget = SetTarget();
            if (!Crusader.Fortified) SetPlayerOutline(Crusader.CurrentTarget, Crusader.Color);
        }

        public static void CrusaderUpdate() 
        {
            if (Crusader.Player == null || PlayerControl.LocalPlayer != Crusader.Player || PlayerControl.LocalPlayer.Data.IsDead) return;
            var (playerCompleted, _) = TasksHandler.TaskInfo(PlayerControl.LocalPlayer.Data);
            if (playerCompleted == Crusader.RechargedTasks) 
            {
                Crusader.RechargedTasks += Crusader.RechargeTasksNumber;
                Crusader.Charges++;
            }
        }

        static void RomanticSetTarget()
        {
            if (Romantic.Player == null || Romantic.Player != PlayerControl.LocalPlayer) return;
            Romantic.CurrentTarget = SetTarget();
            if (!Romantic.HasLover) SetPlayerOutline(Romantic.CurrentTarget, Romantic.Color);
        }

        static void MysticSetTarget() 
        {
            if (Mystic.Player == null || Mystic.Player != PlayerControl.LocalPlayer) return;
            Mystic.CurrentTarget = SetTarget();
            SetPlayerOutline(Mystic.CurrentTarget, Mystic.Color);
        }

        static void MorphlingSetTarget() 
        {
            if (Morphling.Player == null || Morphling.Player != PlayerControl.LocalPlayer) return;
            Morphling.CurrentTarget = SetTarget();
            SetPlayerOutline(Morphling.CurrentTarget, Morphling.Color);
        }
        
        static void CultistSetTarget() 
        {
            if (Cultist.Player == null || Cultist.Player != PlayerControl.LocalPlayer) return;
            Cultist.CurrentTarget = SetTarget();
            SetPlayerOutline(Cultist.CurrentTarget, Palette.ImpostorRed);
        }

        static void BlackmailerSetTarget()
        {
            if (Blackmailer.Player == null || Blackmailer.Player != PlayerControl.LocalPlayer) return;
            Blackmailer.CurrentTarget = SetTarget();
            SetPlayerOutline(Blackmailer.CurrentTarget, Blackmailer.Color);
        }

        public static void PlaguebearerSetTarget()
        {
            if (Plaguebearer.Player == null || Plaguebearer.Player != PlayerControl.LocalPlayer) return;

            List<PlayerControl> untargetables = new List<PlayerControl>();

            foreach (var infected in Plaguebearer.InfectedPlayers)
            {
                untargetables.Add(infected);
            }

            Plaguebearer.CurrentTarget = SetTarget(targetPlayersInVents: true, untargetablePlayers: untargetables);
            if (Plaguebearer.CurrentTarget != null) 
                SetPlayerOutline(Plaguebearer.CurrentTarget, Plaguebearer.Color);
        }

        static void PestilenceSetTarget() 
        {
            if (Pestilence.Player == null || Pestilence.Player != PlayerControl.LocalPlayer) return;
            Pestilence.CurrentTarget = SetTarget(targetPlayersInVents: true);
            SetPlayerOutline(Pestilence.CurrentTarget, Pestilence.Color);
        }

        static void SheriffSetTarget() 
        {
            if (Sheriff.Player == null || Sheriff.Player != PlayerControl.LocalPlayer) return;
            Sheriff.CurrentTarget = SetTarget(targetPlayersInVents: true);
            SetPlayerOutline(Sheriff.CurrentTarget, Sheriff.Color);
        }

        static void GlitchSetTarget()
        {
            if (Glitch.Player == null || Glitch.Player != PlayerControl.LocalPlayer) return;
            Glitch.CurrentTarget = SetTarget(targetPlayersInVents: true);
            SetPlayerOutline(Glitch.CurrentTarget, Glitch.Color);
        }

        static void HitmanSetTarget()
        {
            if (Hitman.Player == null || Hitman.Player != PlayerControl.LocalPlayer) return;
            Hitman.CurrentTarget = SetTarget(targetPlayersInVents: true);
            SetPlayerOutline(Hitman.CurrentTarget, Hitman.Color);
        }

        static void MonarchSetTarget()
        {
            if (Monarch.Player == null || Monarch.Player != PlayerControl.LocalPlayer) return;
            List<PlayerControl> untargetables = new List<PlayerControl>();

            foreach (var knighted in Monarch.KnightedPlayers)
            {
                untargetables.Add(knighted);
            }

            Monarch.CurrentTarget = SetTarget(targetPlayersInVents: true, untargetablePlayers: untargetables);
            if (Monarch.CurrentTarget != null) 
                SetPlayerOutline(Monarch.CurrentTarget, Monarch.Color);
        }

        static void VengefulRomanticSetTarget()
        {
            if (VengefulRomantic.Player == null || VengefulRomantic.Player != PlayerControl.LocalPlayer) return;
            VengefulRomantic.CurrentTarget = SetTarget(targetPlayersInVents: true);
            SetPlayerOutline(VengefulRomantic.CurrentTarget, Romantic.Color);
        }

        static void TrackerSetTarget() 
        {
            if (Tracker.Player == null || Tracker.Player != PlayerControl.LocalPlayer) return;
            Tracker.CurrentTarget = SetTarget();
            if (!Tracker.usedTracker) SetPlayerOutline(Tracker.CurrentTarget, Tracker.Color);
        }

        static void DetectiveUpdateFootPrints() 
        {
            if (Detective.Player == null || Detective.Player != PlayerControl.LocalPlayer) return;

            Detective.timer -= Time.fixedDeltaTime;
            if (Detective.timer <= 0f) {
                Detective.timer = Detective.footprintIntervall;
                foreach (PlayerControl player in PlayerControl.AllPlayerControls) 
                {
                    if (player != null && player != PlayerControl.LocalPlayer && !player.Data.IsDead && !player.inVent) 
                    {
                        FootprintHolder.Instance.MakeFootprint(player);
                    }
                }
            }
        }

        static void ViperSetTarget() 
        {
            if (Viper.Player == null || Viper.Player != PlayerControl.LocalPlayer) return;

            PlayerControl target = null;
            if (Spy.Player != null) 
            {
                if (Spy.impostorsCanKillAnyone) 
                {
                    target = SetTarget(onlyCrewmates: false, targetPlayersInVents: true);
                }
                else
                {
                    target = SetTarget(onlyCrewmates: true, targetPlayersInVents: true);
                }
            }
            else 
            {
                target = SetTarget(onlyCrewmates: true, targetPlayersInVents : true);
            }

            Viper.CurrentTarget = target;
            SetPlayerOutline(Viper.CurrentTarget, Viper.Color);
        }

        static void JackalSetTarget() 
        {
            if (Jackal.Player == null || Jackal.Player != PlayerControl.LocalPlayer) return;
            var untargetablePlayers = new List<PlayerControl>();
            if (Jackal.canCreateRecruitFromImpostor)
            {
                // Only exclude Recruit from beeing targeted if the jackal can create Recruits from impostors
                if (Recruit.Player != null) untargetablePlayers.Add(Recruit.Player);
            }
            Jackal.CurrentTarget = SetTarget(targetPlayersInVents: true, untargetablePlayers: untargetablePlayers);
            SetPlayerOutline(Jackal.CurrentTarget, Jackal.Color);
        }

        static void JuggernautSetTarget()
        {
            if (Juggernaut.Player == null || Juggernaut.Player != PlayerControl.LocalPlayer) return;
            var untargetablePlayers = new List<PlayerControl>();
            Juggernaut.CurrentTarget = SetTarget(targetPlayersInVents: true, untargetablePlayers: untargetablePlayers);
            SetPlayerOutline(Juggernaut.CurrentTarget, Juggernaut.Color);
        }

        static void PredatorSetTarget() 
        {
            if (Predator.Player == null || Predator.Player != PlayerControl.LocalPlayer) return;
            var untargetablePlayers = new List<PlayerControl>();
            Predator.CurrentTarget = SetTarget(targetPlayersInVents: true, untargetablePlayers: untargetablePlayers);
            SetPlayerOutline(Predator.CurrentTarget, Predator.Color);
        }

        static void WerewolfSetTarget() 
        {
            if (Werewolf.Player == null || Werewolf.Player != PlayerControl.LocalPlayer) return;
            var untargetablePlayers = new List<PlayerControl>();
            Werewolf.CurrentTarget = SetTarget(targetPlayersInVents: true, untargetablePlayers: untargetablePlayers);
            SetPlayerOutline(Werewolf.CurrentTarget, Werewolf.Color);
        }

        static void RecruitSetTarget() 
        {
            if (Recruit.Player == null || Recruit.Player != PlayerControl.LocalPlayer) return;
            var untargetablePlayers = new List<PlayerControl>();
            if (Jackal.Player != null) untargetablePlayers.Add(Jackal.Player);
            Recruit.CurrentTarget = SetTarget(targetPlayersInVents: true, untargetablePlayers: untargetablePlayers);
            SetPlayerOutline(Recruit.CurrentTarget, Recruit.Color);
        }

        static void EraserSetTarget() 
        {
            if (Eraser.Player == null || Eraser.Player != PlayerControl.LocalPlayer) return;

            List<PlayerControl> untargetables = new List<PlayerControl>();
            if (Spy.Player != null) untargetables.Add(Spy.Player);
            Eraser.CurrentTarget = SetTarget(onlyCrewmates: !Eraser.canEraseAnyone, untargetablePlayers: Eraser.canEraseAnyone ? new List<PlayerControl>() : untargetables);
            SetPlayerOutline(Eraser.CurrentTarget, Eraser.Color);
        }

        static void GlitchUpdate()
        {
            if (PlayerControl.LocalPlayer == null || !Glitch.HackedKnows.ContainsKey(PlayerControl.LocalPlayer.PlayerId)) return;
            
            if (Glitch.HackedKnows[PlayerControl.LocalPlayer.PlayerId] <= 0)
            {
                Glitch.HackedKnows.Remove(PlayerControl.LocalPlayer.PlayerId);
                // Resets the buttons
                Glitch.SetHackedKnows(false);
                
                // Ghost info
                Utils.SendRPC(CustomRPC.ShareGhostInfo, PlayerControl.LocalPlayer.PlayerId, (byte)GhostInfoTypes.HackOver);
            }
            
        }

        static void EngineerUpdate() 
        {
            bool NKHighlight = Engineer.HighlightForNeutralKillers && PlayerControl.LocalPlayer.IsNeutralKiller();
            bool impostorHighlight = Engineer.highlightForImpostors && PlayerControl.LocalPlayer.IsImpostor();
            if ((NKHighlight || impostorHighlight) && MapUtilities.CachedShipStatus?.AllVents != null)
            {
                foreach (Vent vent in MapUtilities.CachedShipStatus.AllVents)
                {
                    try
                    {
                        if (vent?.myRend?.material != null)
                        {
                            if (Engineer.Player != null && Engineer.Player.inVent)
                            {
                                vent.myRend.material.SetFloat("_Outline", 1f);
                                vent.myRend.material.SetColor("_OutlineColor", Engineer.Color);
                            }
                            else if (vent.myRend.material.GetColor("_AddColor") != Color.red)
                            {
                                vent.myRend.material.SetFloat("_Outline", 0);
                            }
                        }
                    }
                    catch { }
                }
            }
        }

        static void ImpostorSetTarget() 
        {
            if (!PlayerControl.LocalPlayer.Data.Role.IsImpostor ||!PlayerControl.LocalPlayer.CanMove || PlayerControl.LocalPlayer.Data.IsDead)
            {
                // !isImpostor || !canMove || isDead
                FastDestroyableSingleton<HudManager>.Instance.KillButton.SetTarget(null);
                return;
            }

            PlayerControl target = null;
            if (Spy.Player != null)
            {
                if (Spy.impostorsCanKillAnyone)
                {
                    target = SetTarget(false, true);
                }
                else
                {
                    target = SetTarget(true, true);
                }
            }
            // Can kill partner if they are a Recruit
            else if (Recruit.Player.IsImpostor())
            {
                target = SetTarget(false, true);
            }
            else
            {
                target = SetTarget(true, true);
            }

            FastDestroyableSingleton<HudManager>.Instance.KillButton.SetTarget(target); // Includes setPlayerOutline(target, Palette.ImpstorRed);
        }

        static void WarlockSetTarget() 
        {
            if (Warlock.Player == null || Warlock.Player != PlayerControl.LocalPlayer) return;
            if (Warlock.curseVictim != null && (Warlock.curseVictim.Data.Disconnected || Warlock.curseVictim.Data.IsDead)) {
                // If the cursed victim is disconnected or dead reset the curse so a new curse can be applied
                Warlock.ResetCurse();
            }
            if (Warlock.curseVictim == null) {
                Warlock.CurrentTarget = SetTarget();
                SetPlayerOutline(Warlock.CurrentTarget, Warlock.Color);
            }
            else {
                Warlock.curseVictimTarget = SetTarget(targetingPlayer: Warlock.curseVictim);
                SetPlayerOutline(Warlock.curseVictimTarget, Warlock.Color);
            }
        }

        static void WraithUpdate()
        {
            if (Wraith.IsVanished && Wraith.VanishTimer <= 0 && Wraith.Player == PlayerControl.LocalPlayer)
            {
                Utils.SendRPC(CustomRPC.SetVanish, Wraith.Player.PlayerId, byte.MaxValue);
                RPCProcedure.SetVanish(Wraith.Player.PlayerId, byte.MaxValue);
            }
        }

        static void AssassinUpdate()
        {
            if (Assassin.isInvisble && Assassin.invisibleTimer <= 0 && Assassin.Player == PlayerControl.LocalPlayer)
            {
                Utils.SendRPC(CustomRPC.SetInvisible, Assassin.Player.PlayerId, byte.MaxValue);
                RPCProcedure.SetInvisible(Assassin.Player.PlayerId, byte.MaxValue);
            }
            if (Assassin.arrow?.arrow != null)
            {
                if (Assassin.Player == null || Assassin.Player != PlayerControl.LocalPlayer || !Assassin.knowsTargetLocation) 
                {
                    Assassin.arrow.arrow.SetActive(false);
                    return;
                }
                if (Assassin.AssassinMarked != null && !PlayerControl.LocalPlayer.Data.IsDead)
                {
                    bool trackedOnMap = !Assassin.AssassinMarked.Data.IsDead;
                    Vector3 position = Assassin.AssassinMarked.transform.position;
                    if (!trackedOnMap)
                    { // Check for dead body
                        DeadBody body = UnityEngine.Object.FindObjectsOfType<DeadBody>().FirstOrDefault(b => b.ParentId == Assassin.AssassinMarked.PlayerId);
                        if (body != null)
                        {
                            trackedOnMap = true;
                            position = body.transform.position;
                        }
                    }
                    Assassin.arrow.Update(position);
                    Assassin.arrow.arrow.SetActive(trackedOnMap);
                } 
                else
                {
                    Assassin.arrow.arrow.SetActive(false);
                }
            }
        }

        public static void CultistUpdate()
        {
            if (Cultist.Player == null || Cultist.Player != PlayerControl.LocalPlayer || Follower.Player == null) return;

            Utils.TrackPlayer(Follower.Player, Cultist.LocalArrows[0], Color.red);
        }
        public static void FollowerUpdate()
        {
            if (Follower.Player == null || Follower.Player != PlayerControl.LocalPlayer || Cultist.Player == null) return;

            Utils.TrackPlayer(Cultist.Player, Cultist.LocalArrows[1], Color.red);
        }
        static void TrackerUpdate()
        {
            // Handle player tracking
            if (Tracker.arrow?.arrow != null)
            {
                if (Tracker.Player == null || PlayerControl.LocalPlayer != Tracker.Player)
                {
                    Tracker.arrow.arrow.SetActive(false);
                    if (Tracker.DangerMeterParent) Tracker.DangerMeterParent.SetActive(false);
                    return;
                }

                if (Tracker.tracked != null && !Tracker.Player.Data.IsDead)
                {
                    Tracker.timeUntilUpdate -= Time.fixedDeltaTime;

                    if (Tracker.timeUntilUpdate <= 0f)
                    {
                        bool trackedOnMap = !Tracker.tracked.Data.IsDead;
                        Vector3 position = Tracker.tracked.transform.position;
                        if (!trackedOnMap)
                        { // Check for dead body
                            DeadBody body = UnityEngine.Object.FindObjectsOfType<DeadBody>().FirstOrDefault(b => b.ParentId == Tracker.tracked.PlayerId);
                            if (body != null)
                            {
                                trackedOnMap = true;
                                position = body.transform.position;
                            }
                        }

                        if (Tracker.trackingMode == 1 || Tracker.trackingMode == 2) Arrow.UpdateProximity(position);
                        if (Tracker.trackingMode == 0 || Tracker.trackingMode == 2)
                        {
                            Tracker.arrow.Update(position);
                            Tracker.arrow.arrow.SetActive(trackedOnMap);
                        }
                        Tracker.timeUntilUpdate = Tracker.updateIntervall;
                    }
                    else
                    {
                        if (Tracker.trackingMode == 0 || Tracker.trackingMode == 2) Tracker.arrow.Update();
                    }
                }
                else if (Tracker.Player.Data.IsDead)
                {
                    Tracker.DangerMeterParent?.SetActive(false);
                    Tracker.Meter?.gameObject.SetActive(false);
                }
            }

            // Handle corpses tracking
            if (Tracker.Player != null && Tracker.Player == PlayerControl.LocalPlayer && Tracker.corpsesTrackingTimer >= 0f && !Tracker.Player.Data.IsDead)
            {
                bool arrowsCountChanged = Tracker.localArrows.Count != Tracker.deadBodyPositions.Count();
                int index = 0;

                if (arrowsCountChanged)
                {
                    foreach (Arrow arrow in Tracker.localArrows) UnityEngine.Object.Destroy(arrow.arrow);
                    Tracker.localArrows = new List<Arrow>();
                }
                foreach (Vector3 position in Tracker.deadBodyPositions)
                {
                    if (arrowsCountChanged)
                    {
                        Tracker.localArrows.Add(new Arrow(Tracker.Color));
                        Tracker.localArrows[index].arrow.SetActive(true);
                    }
                    if (Tracker.localArrows[index] != null) Tracker.localArrows[index].Update(position);
                    index++;
                }
            }
            else if (Tracker.localArrows.Count > 0)
            {
                foreach (Arrow arrow in Tracker.localArrows) UnityEngine.Object.Destroy(arrow.arrow);
                Tracker.localArrows = new List<Arrow>();
            }
        }

        public static void PlayerSizeUpdate(PlayerControl p) 
        {
            // Set default player size
            CircleCollider2D collider = p.Collider.CastFast<CircleCollider2D>();
            p.transform.localScale = new Vector3(0.7f, 0.7f, 1f);
            collider.radius = Mini.defaultColliderRadius;
            collider.offset = Mini.defaultColliderOffset * Vector2.down;

            // Handle Mini (scaling down)
            if (Mini.Player != null && 
            !(Painter.PaintTimer > 0f || Utils.MushroomSabotageActive() || 
              (Mini.Player == Morphling.Player && Morphling.morphTimer > 0) ||
              (Mini.Player == Glitch.Player && Glitch.MimicTimer > 0) ||
              (Mini.Player == Hitman.Player && Hitman.MorphTimer > 0)))
            {
            Vector3 miniScale = Mini.SizeFactor;
            float baseRadius = Mini.defaultColliderRadius;
            float scaleFactor = miniScale.x / 0.7f;
            float correctedColliderRadius = baseRadius * 0.7f / scaleFactor;

            if (p == Mini.Player) 
            {
                p.transform.localScale = miniScale;
                collider.radius = correctedColliderRadius;
            }
            if (Morphling.Player != null && p == Morphling.Player && Morphling.morphTarget == Mini.Player && Morphling.morphTimer > 0f) 
            {
                p.transform.localScale = miniScale;
                collider.radius = correctedColliderRadius;
            }
            if (Glitch.Player != null && p == Glitch.Player && Glitch.MimicTarget == Mini.Player && Glitch.MimicTimer > 0f) 
            {
                p.transform.localScale = miniScale;
                collider.radius = correctedColliderRadius;
            }
            if (Hitman.Player != null && p == Hitman.Player && Hitman.MorphTarget == Mini.Player && Hitman.MorphTimer > 0f) 
            {
                p.transform.localScale = miniScale;
                collider.radius = correctedColliderRadius;
            }
            }
        
            // Handle Giant (scaling up)
            if (Giant.Player != null && 
            !(Painter.PaintTimer > 0f || Utils.MushroomSabotageActive() || 
              (Giant.Player == Morphling.Player && Morphling.morphTimer > 0) ||
              (Giant.Player == Glitch.Player && Glitch.MimicTimer > 0) ||
              (Giant.Player == Hitman.Player && Hitman.MorphTimer > 0)))
            {
                Vector3 giantScale = Giant.SizeFactor;
                float baseRadius = Mini.defaultColliderRadius; // same reference point as Mini
                float scaleFactor = giantScale.x / 0.7f;
                float correctedColliderRadius = baseRadius * 0.7f / scaleFactor;

                if (p == Giant.Player)
                {
                    p.transform.localScale = giantScale;
                    collider.radius = correctedColliderRadius;
                }
                if (Morphling.Player != null && p == Morphling.Player && Morphling.morphTarget == Giant.Player && Morphling.morphTimer > 0f) 
                    {
                    p.transform.localScale = giantScale;
                    collider.radius = correctedColliderRadius;
                }
                if (Glitch.Player != null && p == Glitch.Player && Glitch.MimicTarget == Giant.Player && Glitch.MimicTimer > 0f) 
                {
                    p.transform.localScale = giantScale;
                    collider.radius = correctedColliderRadius;
                }
                if (Hitman.Player != null && p == Hitman.Player && Hitman.MorphTarget == Giant.Player && Hitman.MorphTimer > 0f) 
                {
                    p.transform.localScale = giantScale;
                    collider.radius = correctedColliderRadius;
                }
            }
        }

        public static void UpdatePlayerInfo() 
        {
            Vector3 colorBlindTextMeetingInitialLocalPos = new Vector3(0.3384f, -0.16666f, -0.01f);
            Vector3 colorBlindTextMeetingInitialLocalScale = new Vector3(0.9f, 1f, 1f);

            foreach (PlayerControl player in PlayerControl.AllPlayerControls) 
            {
                UpdateMeetingColorBlindText(player, colorBlindTextMeetingInitialLocalPos, colorBlindTextMeetingInitialLocalScale);
                UpdateRoundColorBlindText(player);
                UpdatePlayerNameZIndex(player);
                if (ShouldSeePlayerInfo(player)) 
                {
                    UpdatePlayerInfoText(player);
                }
            }
        }

        private static void UpdateMeetingColorBlindText(PlayerControl player, Vector3 initialPos, Vector3 initialScale) 
        {
            PlayerVoteArea playerVoteArea = MeetingHud.Instance?.playerStates?.FirstOrDefault(x => x.TargetPlayerId == player.PlayerId);
            if (playerVoteArea != null && playerVoteArea.ColorBlindName.gameObject.active) 
            {
                playerVoteArea.ColorBlindName.transform.localPosition = initialPos + new Vector3(0f, 0.4f, 0f);
                playerVoteArea.ColorBlindName.transform.localScale = initialScale * 0.8f;
            }

            if (player == null || player.cosmetics.colorBlindText == null || playerVoteArea == null) return;
            var playerData = GameData.Instance.GetPlayerById(playerVoteArea.TargetPlayerId);
            Color playerColor = Palette.PlayerColors[playerData?.DefaultOutfit.ColorId ?? 0];
            playerVoteArea.ColorBlindName.color = playerColor;
        }

        private static void UpdateRoundColorBlindText(PlayerControl player) 
        {
            if (player.cosmetics.colorBlindText != null && player.cosmetics.showColorBlindText && player.cosmetics.colorBlindText.gameObject.active) 
            {
                player.cosmetics.colorBlindText.transform.localPosition = new Vector3(0, -1.4f, 0f);
            }
        }

        private static void UpdatePlayerNameZIndex(PlayerControl player) 
        {
            player.cosmetics.nameText.transform.parent.SetLocalZ(-0.0001f);
        }

        public static bool ShouldSeePlayerInfo(PlayerControl player)
        {
            if (player == null) return false;

            // Prevent showing if player just died and Chronos could use rewind
            if (Chronos.RecentlyDied.TryGetValue(player.PlayerId, out float deathTime) && Chronos.Player != null)
            {
                float rewindBuffer = Chronos.RewindTimeDuration + 2f;
                if (Time.time - deathTime <= rewindBuffer)
                    return false;
            }

            // impostors can see the roles of each other as long as there's no Spy, otherwise Spy is useless.
            // also: if one impostor is lover they cant see their partners role, but the partner can see their role.
            if (PlayerControl.LocalPlayer.Data.Role.IsImpostor && player.Data.Role.IsImpostor 
            && Spy.Player == null && !Lovers.IsLover(PlayerControl.LocalPlayer))
            {
                return true;
            }
            if (PlayerControl.LocalPlayer.Data.Role.IsImpostor && PlayerControl.LocalPlayer == Recruit.Player
            && player.Data.Role.IsImpostor)
            {
                return false;
            }

            return
                    (Lawyer.lawyerKnowsRole && PlayerControl.LocalPlayer == Lawyer.Player && player == Lawyer.target) ||
                    (Romantic.RomanticKnowsRole && PlayerControl.LocalPlayer == Romantic.Player && player == Romantic.beloved) ||
                    (Romantic.RomanticKnowsRole && PlayerControl.LocalPlayer == Romantic.beloved && player == Romantic.Player) ||
                    player == PlayerControl.LocalPlayer || Monarch.KnightedPlayers.Contains(PlayerControl.LocalPlayer) && player == Monarch.Player ||
                    (Sleuth.Players.Any(x => x.PlayerId == PlayerControl.LocalPlayer.PlayerId) && Sleuth.Reported.Contains(player.PlayerId)) ||
                    (PlayerControl.LocalPlayer.Data.IsDead && !MapOptions.RevivedPlayers.Contains(PlayerControl.LocalPlayer.PlayerId));
        }

        public static void UpdatePlayerInfoText(PlayerControl player)
        {
            Transform nameTransform = player.cosmetics.nameText.transform.parent;
            Transform infoTransform = nameTransform.Find("Info");

            // Clean up if not allowed to see roles
            if (!ShouldSeePlayerInfo(player))
            {
                if (infoTransform != null)
                    UnityEngine.Object.Destroy(infoTransform.gameObject);
        
                if (RoleInfo.RoleTexts.ContainsKey(player.PlayerId))
                    RoleInfo.RoleTexts.Remove(player.PlayerId);
        
                return;
            }
        
            // Create if missing
            TMPro.TextMeshPro infoText;
            if (infoTransform == null)
            {
                infoText = UnityEngine.Object.Instantiate(player.cosmetics.nameText, nameTransform);
                infoText.transform.localPosition += Vector3.up * 0.225f;
                infoText.fontSize *= 0.75f;
                infoText.name = "Info";
                infoText.color = infoText.color.SetAlpha(1f);
            }
            else
            {
                infoText = infoTransform.GetComponent<TMPro.TextMeshPro>();
            }
        
            RoleInfo.RoleTexts[player.PlayerId] = infoText;
            infoText.text = GetPlayerInfoText(player);
            infoText.gameObject.SetActive(player.Visible);

            PlayerVoteArea playerVoteArea = MeetingHud.Instance?.playerStates?.FirstOrDefault(x => x.TargetPlayerId == player.PlayerId);
            TMPro.TextMeshPro meetingInfo = GetOrCreateMeetingInfoText(playerVoteArea);
            UpdateMeetingPlayerNamePosition(playerVoteArea);
            string meetingInfoText = GetMeetingInfoText(player);

            if (meetingInfo != null)
            {
                meetingInfo.text = MeetingHud.Instance.state == MeetingHud.VoteStates.Results ? "" : meetingInfoText;
            }
        }
        private static TMPro.TextMeshPro GetOrCreateMeetingInfoText(PlayerVoteArea playerVoteArea) 
        {
            if (playerVoteArea == null) return null;

            Transform meetingInfoTransform = playerVoteArea.NameText.transform.parent.FindChild("Info");
            TMPro.TextMeshPro meetingInfo = meetingInfoTransform != null ? meetingInfoTransform.GetComponent<TMPro.TextMeshPro>() : null;

            if (meetingInfo == null) 
            {
                meetingInfo = UnityEngine.Object.Instantiate(playerVoteArea.NameText, playerVoteArea.NameText.transform.parent);
                meetingInfo.transform.localPosition += Vector3.down * 0.2f;
                meetingInfo.fontSize *= 0.60f;
                meetingInfo.gameObject.name = "Info";
            }

            return meetingInfo;
        }

        private static TMPro.TextMeshPro CreatePlayerInfoText(PlayerControl player) 
        {
            TMPro.TextMeshPro playerInfo = UnityEngine.Object.Instantiate(player.cosmetics.nameText, player.cosmetics.nameText.transform.parent);
            playerInfo.transform.localPosition += Vector3.up * 0.225f;
            playerInfo.fontSize *= 0.75f;
            playerInfo.gameObject.name = "Info";
            playerInfo.color = playerInfo.color.SetAlpha(1f);
            return playerInfo;
        }

        private static void UpdateMeetingPlayerNamePosition(PlayerVoteArea playerVoteArea) 
        {
            if (playerVoteArea != null) 
            {
                var playerName = playerVoteArea.NameText;
                playerName.transform.localPosition = new Vector3(0.3384f, 0.0311f, -0.1f);
            }
        }

        private static string GetPlayerInfoText(PlayerControl player) 
        {
            var (tasksCompleted, tasksTotal) = TasksHandler.TaskInfo(player.Data);
            string ghostInfo = RoleInfo.GetGhostInfoString(player);
            string dReason = RoleInfo.GetDeathReasonString(player);
            string taskInfo = tasksTotal > 0 ? $" <color=#FAD934FF>({tasksCompleted}/{tasksTotal})</color>" : "";

            string roleNames = "";
            string modifierNames = "";
            string abilityNames = "";

            bool shouldSee = ShouldSeePlayerInfo(player);
        
            if (shouldSee)
            {
                roleNames = RoleInfo.GetRolesString(player, true);
                abilityNames = AbilityInfo.GetAbilitiesString(player, true);
        
                if (player.Data.IsDead || PlayerControl.LocalPlayer.Data.IsDead)
                    modifierNames = ModifierInfo.GetModifiersString(player, true);
            }
        
            string mods = !string.IsNullOrEmpty(modifierNames) ? $"({modifierNames}) " : "";
            string abs = !string.IsNullOrEmpty(abilityNames) ? $"[{abilityNames}] " : "";
            string gInfo = !string.IsNullOrEmpty(ghostInfo) ? $"{ghostInfo}" : "";
            string dReasons = !string.IsNullOrEmpty(dReason) ? $" {dReason}" : "";
        
            if (player == PlayerControl.LocalPlayer) 
            {
                if (player.Data.IsDead) roleNames = $"{gInfo}{abs}{mods}{roleNames}{dReasons}";
                if (player == Swapper.Player) roleNames += Utils.ColorString(Swapper.Color, $" ({Swapper.Charges})");
                if (player == Deputy.Player) roleNames += Utils.ColorString(Deputy.Color, $" ({Deputy.Charges})");
        
                if (HudManager.Instance.TaskPanel != null) 
                {
                    TMPro.TextMeshPro tabText = HudManager.Instance.TaskPanel.tab.transform.FindChild("TabText_TMP").GetComponent<TMPro.TextMeshPro>();
                    tabText.SetText($"Tasks {taskInfo}");
                }
        
                return $"{roleNames}";
            }

            if (MapOptions.GhostsSeeEverything && MapOptions.ghostsSeeInformation)
            {
                return $"{gInfo}{abs}{mods}{roleNames}{taskInfo}{dReasons}".Trim();
            }

            if (MapOptions.ghostsSeeInformation) 
            {
                return $"{gInfo}{taskInfo}{dReasons}".Trim();
            }

            if (MapOptions.GhostsSeeEverything || shouldSee) 
            {
                return $"{abs}{mods}{roleNames}";
            }

            return "";
        }


        private static string GetMeetingInfoText(PlayerControl player) 
        {
            return GetPlayerInfoText(player);
        }

        public static void VigilanteSetTarget() 
        {
            if (Vigilante.Player == null || Vigilante.Player != PlayerControl.LocalPlayer || MapUtilities.CachedShipStatus == null || MapUtilities.CachedShipStatus.AllVents == null) return;

            Vent target = null;
            Vector2 truePosition = PlayerControl.LocalPlayer.GetTruePosition();
            float closestDistance = float.MaxValue;
            for (int i = 0; i < MapUtilities.CachedShipStatus.AllVents.Length; i++)
            {
                Vent vent = MapUtilities.CachedShipStatus.AllVents[i];
                if (vent.gameObject.name.StartsWith("JackInTheBoxVent_") || vent.gameObject.name.StartsWith("SealedVent_") || vent.gameObject.name.StartsWith("FutureSealedVent_")) continue;
                if (SubmergedCompatibility.IsSubmerged && vent.Id == 9) continue; // cannot seal submergeds exit only vent!
                float distance = Vector2.Distance(vent.transform.position, truePosition);
                if (distance <= vent.UsableDistance && distance < closestDistance)
                {
                    closestDistance = distance;
                    target = vent;
                }
            }
            Vigilante.ventTarget = target;
        }

        public static void VigilanteUpdate() 
        {
            if (Vigilante.Player == null || PlayerControl.LocalPlayer != Vigilante.Player || Vigilante.Player.Data.IsDead) return;
            var (playerCompleted, _) = TasksHandler.TaskInfo(Vigilante.Player.Data);
            if (playerCompleted == Vigilante.RechargedTasks)
            {
                Vigilante.RechargedTasks += Vigilante.RechargeTasksNumber;
                if (Vigilante.maxCharges > Vigilante.Charges) Vigilante.Charges++;
            }
        }

        public static void ArsonistSetTarget() 
        {
            if (Arsonist.Player == null || Arsonist.Player != PlayerControl.LocalPlayer) return;
            List<PlayerControl> untargetables;
            if (Arsonist.douseTarget != null)
            {
                untargetables = new();
                foreach (PlayerControl player in PlayerControl.AllPlayerControls)
                {
                    if (player.PlayerId != Arsonist.douseTarget.PlayerId)
                    {
                        untargetables.Add(player);
                    }
                }
            }
            else untargetables = Arsonist.dousedPlayers;
            Arsonist.CurrentTarget = SetTarget(untargetablePlayers: untargetables);
            if (Arsonist.CurrentTarget != null) SetPlayerOutline(Arsonist.CurrentTarget, Arsonist.Color);
        }

        static void BountyHunterUpdate() 
        {
            if (BountyHunter.Player == null || PlayerControl.LocalPlayer != BountyHunter.Player) return;

            if (BountyHunter.Player.Data.IsDead) 
            {
                if (BountyHunter.arrow != null || BountyHunter.arrow.arrow != null) UnityEngine.Object.Destroy(BountyHunter.arrow.arrow);
                BountyHunter.arrow = null;
                if (BountyHunter.CooldownText != null && BountyHunter.CooldownText.gameObject != null) UnityEngine.Object.Destroy(BountyHunter.CooldownText.gameObject);
                BountyHunter.CooldownText = null;
                BountyHunter.bounty = null;
                foreach (PoolablePlayer p in MapOptions.BeanIcons.Values) 
                {
                    if (p != null && p.gameObject != null) p.gameObject.SetActive(false);
                }
                return;
            }

            BountyHunter.arrowUpdateTimer -= Time.fixedDeltaTime;
            BountyHunter.bountyUpdateTimer -= Time.fixedDeltaTime;

            if (BountyHunter.bounty == null || BountyHunter.bountyUpdateTimer <= 0f) 
            {
                // Set new bounty
                BountyHunter.bounty = null;
                BountyHunter.arrowUpdateTimer = 0f; // Force arrow to update
                BountyHunter.bountyUpdateTimer = BountyHunter.bountyDuration;
                var possibleTargets = new List<PlayerControl>();
                foreach (PlayerControl p in PlayerControl.AllPlayerControls) 
                {
                    if (!p.Data.IsDead && !p.Data.Disconnected && p != p.Data.Role.IsImpostor && p != Spy.Player && (Romantic.beloved == BountyHunter.Player && p != Romantic.Player) && (p != Recruit.Player) && (p != Jackal.Player) && (Lovers.GetPartner(BountyHunter.Player) == null || p != Lovers.GetPartner(BountyHunter.Player))) possibleTargets.Add(p);
                }
                BountyHunter.bounty = possibleTargets[TheSushiRoles.rnd.Next(0, possibleTargets.Count)];
                if (BountyHunter.bounty == null) return;

                // Ghost Info
                Utils.SendRPC(CustomRPC.ShareGhostInfo, PlayerControl.LocalPlayer.PlayerId, (byte)GhostInfoTypes.BountyTarget, BountyHunter.bounty.PlayerId);

                // Show poolable player
                if (FastDestroyableSingleton<HudManager>.Instance != null && FastDestroyableSingleton<HudManager>.Instance.UseButton != null) 
                {
                    foreach (PoolablePlayer pp in MapOptions.BeanIcons.Values) pp.gameObject.SetActive(false);
                    if (MapOptions.BeanIcons.ContainsKey(BountyHunter.bounty.PlayerId) && MapOptions.BeanIcons[BountyHunter.bounty.PlayerId].gameObject != null)
                        MapOptions.BeanIcons[BountyHunter.bounty.PlayerId].gameObject.SetActive(true);
                }
            }

            // Hide in meeting
            if (MeetingHud.Instance && MapOptions.BeanIcons.ContainsKey(BountyHunter.bounty.PlayerId) && MapOptions.BeanIcons[BountyHunter.bounty.PlayerId].gameObject != null)
                MapOptions.BeanIcons[BountyHunter.bounty.PlayerId].gameObject.SetActive(false);

            // Update Cooldown Text
            if (BountyHunter.CooldownText != null)
            {
                BountyHunter.CooldownText.text = Mathf.CeilToInt(Mathf.Clamp(BountyHunter.bountyUpdateTimer, 0, BountyHunter.bountyDuration)).ToString();
                BountyHunter.CooldownText.gameObject.SetActive(!MeetingHud.Instance);  // Show if not in meeting
            }

            // Update Arrow
            if (BountyHunter.showArrow && BountyHunter.bounty != null) 
            {
                if (BountyHunter.arrow == null) BountyHunter.arrow = new Arrow(Color.red);
                if (BountyHunter.arrowUpdateTimer <= 0f) {
                    BountyHunter.arrow.Update(BountyHunter.bounty.transform.position);
                    BountyHunter.arrowUpdateTimer = BountyHunter.arrowUpdateIntervall;
                }
                BountyHunter.arrow.Update();
            }
        }

        static void UndertakerUpdate()
        {
            if (Undertaker.Player == null || Undertaker.Player.Data.IsDead) return;
            if (Undertaker.CurrentTarget != null)
            {
                Vector3 currentPosition = Undertaker.Player.transform.position;
                Undertaker.CurrentTarget.transform.position = currentPosition;
            }
        }

        static void HitmantUpdate()
        {
            if (Hitman.Player == null || Hitman.Player.Data.IsDead) return;
            if (Hitman.BodyTarget != null)
            {
                Vector3 currentPosition = Hitman.Player.transform.position;
                Hitman.BodyTarget.transform.position = currentPosition;
            }
        }
        public static void AgentUpdate() 
        {
            if (Agent.Player == null || PlayerControl.LocalPlayer != Agent.Player || PlayerControl.LocalPlayer.Data.IsDead) return;
            var (tasksCompleted, tasksTotal) = TasksHandler.TaskInfo(PlayerControl.LocalPlayer.Data);
            bool CompletedAllTasks = tasksCompleted == tasksTotal;
            if (CompletedAllTasks) 
            {
                Utils.SendRPC(CustomRPC.AgentTurnIntoHitman);
                RPCProcedure.AgentTurnIntoHitman(); 
            }
        }

        static void ScavengerUpdate() 
        {
            if (Scavenger.Player == null || PlayerControl.LocalPlayer != Scavenger.Player || Scavenger.localArrows == null) return;
            int index = 0;
            // Handle corpses tracking
            if (Scavenger.Player != null && Scavenger.Player == PlayerControl.LocalPlayer && Scavenger.ScavengeTimer >= 0f && !Scavenger.Player.Data.IsDead)
            {
                bool arrowsCountChanged = Scavenger.localArrows.Count != Scavenger.DeadBodyPositions.Count();

                if (arrowsCountChanged)
                {
                    foreach (Arrow arrow in Scavenger.localArrows) UnityEngine.Object.Destroy(arrow.arrow);
                    Scavenger.localArrows = new List<Arrow>();
                }
                foreach (Vector3 position in Scavenger.DeadBodyPositions)
                {
                    if (arrowsCountChanged)
                    {
                        Scavenger.localArrows.Add(new Arrow(Scavenger.Color));
                        Scavenger.localArrows[index].arrow.SetActive(true);
                    }
                    if (Scavenger.localArrows[index] != null) Scavenger.localArrows[index].Update(position);
                    index++;
                }
            }
            else if (Scavenger.localArrows.Count > 0)
            {
                foreach (Arrow arrow in Scavenger.localArrows) UnityEngine.Object.Destroy(arrow.arrow);
                Scavenger.localArrows = new List<Arrow>();
            }
        }

        public static void PsychicSetTarget() 
        {
            if (Psychic.Player == null || Psychic.Player != PlayerControl.LocalPlayer || Psychic.Player.Data.IsDead || Psychic.deadBodies == null || MapUtilities.CachedShipStatus?.AllVents == null) return;

            DeadPlayer target = null;
            Vector2 truePosition = PlayerControl.LocalPlayer.GetTruePosition();
            float closestDistance = float.MaxValue;
            float usableDistance = MapUtilities.CachedShipStatus.AllVents.FirstOrDefault().UsableDistance;
            foreach ((DeadPlayer dp, Vector3 ps) in Psychic.deadBodies) {
                float distance = Vector2.Distance(ps, truePosition);
                if (distance <= usableDistance && distance < closestDistance) {
                    closestDistance = distance;
                    target = dp;
                }
            }
            Psychic.target = target;
        }

        static bool mushroomSaboWasActive = false;
        static void MorphlingAndPainterUpdate() 
        {
            bool mushRoomSaboIsActive = Utils.MushroomSabotageActive();
            if (!mushroomSaboWasActive) mushroomSaboWasActive = mushRoomSaboIsActive;
            
            float oldPaintTimer = Painter.PaintTimer;
            float oldMorphTimer = Morphling.morphTimer;
            float oldGlitchTimer = Glitch.MimicTimer;
            float oldHitmanTimer = Hitman.MorphTimer;

            Painter.PaintTimer = Mathf.Max(0f, Painter.PaintTimer - Time.fixedDeltaTime);
            Morphling.morphTimer = Mathf.Max(0f, Morphling.morphTimer - Time.fixedDeltaTime);
            Glitch.MimicTimer = Mathf.Max(0f, Glitch.MimicTimer - Time.fixedDeltaTime);
            Hitman.MorphTimer = Mathf.Max(0f, Hitman.MorphTimer - Time.fixedDeltaTime);

            if (mushRoomSaboIsActive) return;

            // Paint reset and set Morphling look if necessary
            if (oldPaintTimer > 0f && Painter.PaintTimer <= 0f)
            {
                Painter.ResetPaint();
                if (Morphling.morphTimer > 0f && Morphling.Player != null && Morphling.morphTarget != null) 
                {
                    PlayerControl target = Morphling.morphTarget;
                    Morphling.Player.SetLook(target.Data.PlayerName, target.Data.DefaultOutfit.ColorId, target.Data.DefaultOutfit.HatId, target.Data.DefaultOutfit.VisorId, target.Data.DefaultOutfit.SkinId, target.Data.DefaultOutfit.PetId);
                }
                if (Glitch.MimicTimer > 0f && Glitch.Player != null && Glitch.MimicTarget != null) 
                {
                    PlayerControl target = Glitch.MimicTarget;
                    Glitch.Player.SetLook(target.Data.PlayerName, target.Data.DefaultOutfit.ColorId, target.Data.DefaultOutfit.HatId, target.Data.DefaultOutfit.VisorId, target.Data.DefaultOutfit.SkinId, target.Data.DefaultOutfit.PetId);
                }
                if (Hitman.MorphTimer > 0f && Hitman.Player != null && Hitman.MorphTarget != null)
                {
                    PlayerControl target = Hitman.MorphTarget;
                    Hitman.Player.SetLook(target.Data.PlayerName, target.Data.DefaultOutfit.ColorId, target.Data.DefaultOutfit.HatId, target.Data.DefaultOutfit.VisorId, target.Data.DefaultOutfit.SkinId, target.Data.DefaultOutfit.PetId);
                }
            }

            // If the MushRoomSabotage ends while Morph is still active set the Morphlings look to the target's look
            if (mushroomSaboWasActive) 
            {
                if (Morphling.morphTimer > 0f && Morphling.Player != null && Morphling.morphTarget != null) 
                {
                    PlayerControl target = Morphling.morphTarget;
                    Morphling.Player.SetLook(target.Data.PlayerName, target.Data.DefaultOutfit.ColorId, target.Data.DefaultOutfit.HatId, target.Data.DefaultOutfit.VisorId, target.Data.DefaultOutfit.SkinId, target.Data.DefaultOutfit.PetId);
                }
                if (Glitch.MimicTimer > 0f && Glitch.Player != null && Glitch.MimicTarget != null) 
                {
                    PlayerControl target = Glitch.MimicTarget;
                    Glitch.Player.SetLook(target.Data.PlayerName, target.Data.DefaultOutfit.ColorId, target.Data.DefaultOutfit.HatId, target.Data.DefaultOutfit.VisorId, target.Data.DefaultOutfit.SkinId, target.Data.DefaultOutfit.PetId);
                }
                if (Hitman.MorphTimer > 0f && Hitman.Player != null && Hitman.MorphTarget != null)
                {
                    PlayerControl target = Hitman.MorphTarget;
                    Hitman.Player.SetLook(target.Data.PlayerName, target.Data.DefaultOutfit.ColorId, target.Data.DefaultOutfit.HatId, target.Data.DefaultOutfit.VisorId, target.Data.DefaultOutfit.SkinId, target.Data.DefaultOutfit.PetId);
                }
                if (Painter.PaintTimer > 0) 
                {
                    List<int> availableColors = Enumerable.Range(0, Palette.PlayerColors.Count).ToList();
                    System.Random rng = new System.Random();
                    availableColors = availableColors.OrderBy(x => rng.Next()).ToList();
                    int index = 0;

                    int randomColorId = rng.Next(Palette.PlayerColors.Count); // full color range
                    int randomColor = availableColors[index % availableColors.Count];
                    foreach (PlayerControl player in PlayerControl.AllPlayerControls)
                    {
                        player.SetLook("", randomColor, "", "", "", "");
                        index++;
                    }
                }
            }

            // Morphling reset (only if paint is inactive)
            if (Painter.PaintTimer <= 0f && oldMorphTimer > 0f && Morphling.morphTimer <= 0f && Morphling.Player != null)
                Morphling.ResetMorph();
            if (Painter.PaintTimer <= 0f && oldGlitchTimer > 0f && Glitch.MimicTimer <= 0f && Glitch.Player != null)
                Glitch.ResetMimic();
            if (Painter.PaintTimer <= 0f && oldHitmanTimer > 0f && Hitman.MorphTimer <= 0f && Hitman.Player != null)
                Hitman.ResetMorph();
            mushroomSaboWasActive = false;
        }

        public static void LawyerUpdate() 
        {
            if (Lawyer.Player == null || Lawyer.Player != PlayerControl.LocalPlayer) return;

            // Promote to Survivor
            if (Lawyer.target != null && Lawyer.target.Data.Disconnected && !Lawyer.Player.Data.IsDead) 
            {
                Utils.SendRPC(CustomRPC.LawyerChangeRole);
                RPCProcedure.LawyerChangeRole();
                return;
            }
        }

        public static void RomanticUpdate() 
        {
            if (Romantic.Player == null || Romantic.Player != PlayerControl.LocalPlayer) return;

            if (Romantic.beloved != null && Romantic.beloved.Data.Disconnected && !Romantic.Player.Data.IsDead) 
            {
                Utils.SendRPC(CustomRPC.RomanticChangeRole);
                RPCProcedure.RomanticChangeRole();
                return;
            }
        }

        public static void HackerUpdate()
        {
            if (Hacker.Player == null || PlayerControl.LocalPlayer != Hacker.Player || Hacker.Player.Data.IsDead) return;
            var (playerCompleted, _) = TasksHandler.TaskInfo(Hacker.Player.Data);
            if (playerCompleted == Hacker.RechargedTasks) {
                Hacker.RechargedTasks += Hacker.RechargeTasksNumber;
                if (Hacker.toolsNumber > Hacker.chargesVitals) Hacker.chargesVitals++;
                if (Hacker.toolsNumber > Hacker.chargesAdminTable) Hacker.chargesAdminTable++;
            }
        }

        // For Charges
        public static void SwapperUpdate() 
        {
            if (Swapper.Player == null || PlayerControl.LocalPlayer != Swapper.Player || PlayerControl.LocalPlayer.Data.IsDead) return;
            var (playerCompleted, _) = TasksHandler.TaskInfo(PlayerControl.LocalPlayer.Data);
            if (playerCompleted == Swapper.RechargedTasks) 
            {
                Swapper.RechargedTasks += Swapper.RechargeTasksNumber;
                Swapper.Charges++;
            }
        }

        public static void VeteranUpdate()
        {
            if (Veteran.Player == null || PlayerControl.LocalPlayer != Veteran.Player || PlayerControl.LocalPlayer.Data.IsDead) return;
            var (playerCompleted, _) = TasksHandler.TaskInfo(PlayerControl.LocalPlayer.Data);
            if (playerCompleted == Veteran.RechargedTasks) 
            {
                Veteran.RechargedTasks += Veteran.RechargeTasksNumber;
                Veteran.Charges++;
            }
        }
        
        public static void DeputyUpdate()
        {
            if (Deputy.Player == null || PlayerControl.LocalPlayer != Deputy.Player || PlayerControl.LocalPlayer.Data.IsDead) return;
            var (playerCompleted, _) = TasksHandler.TaskInfo(PlayerControl.LocalPlayer.Data);
            if (playerCompleted == Deputy.RechargedTasks) 
            {
                Deputy.RechargedTasks += Deputy.RechargeTasksNumber;
                Deputy.Charges++;
            }
        }
        
        public static void LandlordUpdate()
        {
            if (Landlord.Player == null || PlayerControl.LocalPlayer != Landlord.Player || PlayerControl.LocalPlayer.Data.IsDead) return;
            var (playerCompleted, _) = TasksHandler.TaskInfo(PlayerControl.LocalPlayer.Data);
            if (playerCompleted == Landlord.RechargedTasks)
            {
                Landlord.RechargedTasks += Landlord.RechargeTasksNumber;
                Landlord.Charges++;
            }
        }
        public static void LandlordUnteleportableUpdate()
        {
            if (Landlord.Player == null || !GameData.Instance || PlayerControl.LocalPlayer != Landlord.Player || PlayerControl.LocalPlayer.Data.IsDead) return;

            foreach (var entry in Landlord.UnteleportablePlayers)
            {
                var player = Utils.PlayerById(entry.Key);
                if (player == null || player.Data == null || player.Data.IsDead || player.Data.Disconnected) continue;

                if (Landlord.UnteleportablePlayers.ContainsKey(player.PlayerId) && player.moveable == true &&
                Landlord.UnteleportablePlayers.GetValueSafe(player.PlayerId).AddSeconds(0.5) < DateTime.UtcNow)
                {
                    Landlord.UnteleportablePlayers.Remove(player.PlayerId);
                }
            }
        }
        
        public static void MonarchUpdate()
        {
            if (Monarch.Player == null || PlayerControl.LocalPlayer != Monarch.Player || PlayerControl.LocalPlayer.Data.IsDead) return;
            var (playerCompleted, _) = TasksHandler.TaskInfo(PlayerControl.LocalPlayer.Data);
            if (playerCompleted == Monarch.RechargedTasks)
            {
                Monarch.RechargedTasks += Monarch.RechargeTasksNumber;
                Monarch.Charges++;
            }
        }

        public static void MysticUpdate()
        {
            if (Mystic.Player == null || PlayerControl.LocalPlayer != Mystic.Player || PlayerControl.LocalPlayer.Data.IsDead) return;
            var (playerCompleted, _) = TasksHandler.TaskInfo(PlayerControl.LocalPlayer.Data);
            if (playerCompleted == Mystic.RechargedTasks)
            {
                Mystic.RechargedTasks += Mystic.RechargeTasksNumber;
                Mystic.Charges++;
            }
        }

        public static void OracleUpdate() 
        {
            if (Oracle.Player == null || PlayerControl.LocalPlayer != Oracle.Player || PlayerControl.LocalPlayer.Data.IsDead) return;
            var (playerCompleted, _) = TasksHandler.TaskInfo(PlayerControl.LocalPlayer.Data);
            if (playerCompleted == Oracle.RechargedTasks) 
            {
                Oracle.RechargedTasks += Oracle.RechargeTasksNumber;
                Oracle.Charges++;
            }
        }

        static void OracleSetTarget()
        {
            if (Oracle.Player == null || Oracle.Player != PlayerControl.LocalPlayer) return;
            Oracle.CurrentTarget = SetTarget();
            SetPlayerOutline(Oracle.CurrentTarget, Oracle.Color);
        }

        static void SurvivorSetTarget() 
        {
            if (Survivor.Player == null || Survivor.Player != PlayerControl.LocalPlayer) return;
            Survivor.target = SetTarget();
            SetPlayerOutline(Survivor.target, Survivor.Color);
        }

        static void WitchSetTarget() 
        {
            if (Witch.Player == null || Witch.Player != PlayerControl.LocalPlayer) return;
            List<PlayerControl> untargetables;
            if (Witch.spellCastingTarget != null)
                untargetables = PlayerControl.AllPlayerControls.ToArray().Where(x => x.PlayerId != Witch.spellCastingTarget.PlayerId).ToList(); // Don't switch the target from the the one you're currently casting a spell on
            else 
            {
                untargetables = new List<PlayerControl>(); // Also target players that have already been spelled, to hide spells that were blanks/blocked by Shields
                if (Spy.Player != null && !Witch.canSpellAnyone) untargetables.Add(Spy.Player);
            }
            Witch.CurrentTarget = SetTarget(onlyCrewmates: !Witch.canSpellAnyone, untargetablePlayers: untargetables);
            SetPlayerOutline(Witch.CurrentTarget, Witch.Color);
        }

        static void AssassinSetTarget()
        {
            if (Assassin.Player == null || Assassin.Player != PlayerControl.LocalPlayer) return;
            List<PlayerControl> untargetables = new List<PlayerControl>();
            if (Spy.Player != null && !Spy.impostorsCanKillAnyone) untargetables.Add(Spy.Player);
            Assassin.CurrentTarget = SetTarget(onlyCrewmates: Spy.Player == null || !Spy.impostorsCanKillAnyone, untargetablePlayers: untargetables);
            SetPlayerOutline(Assassin.CurrentTarget, Assassin.Color);
        }

        static void BaitUpdate() 
        {
            if (!Bait.active.Any()) return;

            // Bait report
            foreach (KeyValuePair<DeadPlayer, float> entry in new Dictionary<DeadPlayer, float>(Bait.active)) 
            {
                Bait.active[entry.Key] = entry.Value - Time.fixedDeltaTime;
                if (entry.Value <= 0) 
                {
                    Bait.active.Remove(entry.Key);
                    if (entry.Key.GetKiller != null && entry.Key.GetKiller.PlayerId == PlayerControl.LocalPlayer.PlayerId) {
                        Utils.HandlePoisonedOnBodyReport(); // Manually call Viper handling, since the CmdReportDeadBody Prefix won't be called
                        RPCProcedure.UncheckedCmdReportDeadBody(entry.Key.GetKiller.PlayerId, entry.Key.player.PlayerId);
                        Utils.SendRPC(CustomRPC.UncheckedCmdReportDeadBody, entry.Key.GetKiller.PlayerId,
                        entry.Key.player.PlayerId);
                    }
                }
            }
        }

        public static void TrapperUpdate() 
        {
            if (Trapper.Player == null || PlayerControl.LocalPlayer != Trapper.Player || Trapper.Player.Data.IsDead) return;
            var (playerCompleted, _) = TasksHandler.TaskInfo(Trapper.Player.Data);
            if (playerCompleted == Trapper.RechargedTasks) {
                Trapper.RechargedTasks += Trapper.RechargeTasksNumber;
                if (Trapper.maxCharges > Trapper.Charges) Trapper.Charges++;
            }
        }

        public static void Postfix(PlayerControl __instance) 
        {
            if (AmongUsClient.Instance.GameState != InnerNet.InnerNetClient.GameStates.Started || GameOptionsManager.Instance.currentGameOptions.GameMode == GameModes.HideNSeek) return;

            // Mini and Morphling shrink
            PlayerSizeUpdate(__instance);
            
            // set position of colorblind text
            foreach (var pc in PlayerControl.AllPlayerControls) 
            {
                //pc.cosmetics.colorBlindText.gameObject.transform.localPosition = new Vector3(0, 0, -0.0001f);
            }
            
            if (PlayerControl.LocalPlayer == __instance) 
            {
                // Update player outlines
                SetBasePlayerOutlines();

                // Update Role & Modifier Description
                Utils.RefreshRoleDescription(__instance);

                // Update Player Info
                UpdatePlayerInfo();

                //Update pet visibility
                SetPetVisibility();
                
                // Chronos
                BendTimeUpdate();
                // Morphling
                MorphlingSetTarget();
                //Cultist
                CultistSetTarget();
                // Blackmailer
                BlackmailerSetTarget();
                // Medic
                MedicSetTarget();
                // Crusader
                CrusaderSetTarget();
                CrusaderUpdate();
                //Juggernaut
                JuggernautSetTarget();
                //Romantic
                RomanticSetTarget();
                VengefulRomanticSetTarget();
                //Pesti/plague
                PestilenceSetTarget();
                PlaguebearerSetTarget();
                // Sheriff
                SheriffSetTarget();
                // Glitch
                GlitchSetTarget();
                GlitchUpdate();
                // Detective
                DetectiveUpdateFootPrints();
                // Tracker
                TrackerSetTarget();
                // Viper
                ViperSetTarget();
                BlindTrap.Update();
                Trap.Update();
                // Eraser
                EraserSetTarget();
                // Engineer
                EngineerUpdate();
                // Tracker
                TrackerUpdate();
                // Cultist
                FollowerUpdate();
                CultistUpdate();
                // Jackal
                JackalSetTarget();
                //Predator
                PredatorSetTarget();
                //Werewolf
                WerewolfSetTarget();
                // Recruit
                RecruitSetTarget();
                // Impostor
                ImpostorSetTarget();
                // Warlock
                WarlockSetTarget();
                // Vigilante
                VigilanteSetTarget();
                VigilanteUpdate();
                // Arsonist
                ArsonistSetTarget();
                //Mystic
                MysticSetTarget();
                MysticUpdate();
                //Oracle
                OracleUpdate();
                OracleSetTarget();
                // BountyHunter
                BountyHunterUpdate();
                // Scavenger
                ScavengerUpdate();
                // Psychic
                PsychicSetTarget();
                // Morphling and Painter
                MorphlingAndPainterUpdate();
                // Undertaker
                UndertakerUpdate();
                // Hitman
                HitmantUpdate();
                HitmanSetTarget();
                // Agent
                AgentUpdate();
                // Lawyer
                LawyerUpdate();
                //Romantic
                RomanticUpdate();
                // Survivor
                SurvivorSetTarget();
                // Witch
                WitchSetTarget();
                // Assassin
                AssassinSetTarget();
                AssassinTrace.UpdateAll();
                AssassinUpdate();
                // Wraith
                WraithUpdate();
                // yoyo
                Silhouette.UpdateAll();

                HackerUpdate();
                SwapperUpdate();
                // Veteran
                VeteranUpdate();
                // Deputy
                DeputyUpdate();
                // Landlord
                LandlordUpdate();
                LandlordUnteleportableUpdate();
                // Monarch
                MonarchUpdate();
                MonarchSetTarget();
                // Hacker
                HackerUpdate();
                // Trapper
                TrapperUpdate();

                // Bait
                BaitUpdate();
                // Chameleon
                Chameleon.Update();
            } 
        }
    }

    [HarmonyPatch(typeof(PlayerPhysics), nameof(PlayerPhysics.WalkPlayerTo))]
    class PlayerPhysicsWalkPlayerToPatch 
    {
        private static Vector2 offset = Vector2.zero;
        public static void Prefix(PlayerPhysics __instance) 
        {
            bool correctOffset = Painter.PaintTimer <= 0f && !Utils.MushroomSabotageActive() && (__instance.myPlayer == Mini.Player ||  (Morphling.Player != null && __instance.myPlayer == Morphling.Player && Morphling.morphTarget == Mini.Player && Morphling.morphTimer > 0f) 
            || (Glitch.Player != null && __instance.myPlayer == Glitch.Player && Glitch.MimicTarget == Mini.Player && Glitch.MimicTimer > 0f) 
            || (Hitman.Player != null && __instance.myPlayer == Hitman.Player && Hitman.MorphTarget == Mini.Player && Hitman.MorphTimer > 0f));
            
            correctOffset = correctOffset && !(Mini.Player == Morphling.Player && Morphling.morphTimer > 0f)
            || correctOffset && !(Mini.Player == Glitch.Player && Glitch.MimicTimer > 0f) || correctOffset && !(Mini.Player == Hitman.Player && Hitman.MorphTimer > 0f);
            if (correctOffset) 
            {
                float scaleFactor = Mini.SizeFactor.x / 0.7f;
                __instance.myPlayer.Collider.offset = Mini.defaultColliderOffset * Vector2.down / scaleFactor;
            }

            bool correctGiantOffset = Painter.PaintTimer <= 0f && !Utils.MushroomSabotageActive() && (__instance.myPlayer == Giant.Player || 
                (Morphling.Player != null && __instance.myPlayer == Morphling.Player && Morphling.morphTarget == Giant.Player && Morphling.morphTimer > 0f) ||
                (Glitch.Player != null && __instance.myPlayer == Glitch.Player && Glitch.MimicTarget == Giant.Player && Glitch.MimicTimer > 0f) ||
                (Hitman.Player != null && __instance.myPlayer == Hitman.Player && Hitman.MorphTarget == Giant.Player && Hitman.MorphTimer > 0f));
            correctGiantOffset = correctGiantOffset && !(Giant.Player == Morphling.Player && Morphling.morphTimer > 0f)
                || correctGiantOffset && !(Giant.Player == Glitch.Player && Glitch.MimicTimer > 0f) || correctGiantOffset && !(Giant.Player == Hitman.Player && Hitman.MorphTimer > 0f);
            if (correctGiantOffset)
            {
                float scaleFactor = Giant.SizeFactor.x / 0.7f;
                __instance.myPlayer.Collider.offset = Mini.defaultColliderOffset * Vector2.down / scaleFactor;
            }
        }
    }

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.CmdReportDeadBody))]
    class PlayerControlCmdReportDeadBodyPatch 
    {
        public static bool Prefix(PlayerControl __instance) 
        {
            Utils.HandlePoisonedOnBodyReport();
            return true;
        }
    }

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.LocalPlayer.CmdReportDeadBody))]
    class BodyReportPatch
    {
        static void Postfix(PlayerControl __instance, [HarmonyArgument(0)]NetworkedPlayerInfo target)
        {
            // Medic or Detective report
            bool isMedicReport = Medic.Player != null && Medic.Player == PlayerControl.LocalPlayer && __instance.PlayerId == Medic.Player.PlayerId;
            bool isDetectiveReport = Detective.Player != null && Detective.Player == PlayerControl.LocalPlayer && __instance.PlayerId == Detective.Player.PlayerId;
            bool IsSleuthReport = Sleuth.Players.FindAll(x => x.PlayerId == PlayerControl.LocalPlayer.PlayerId && __instance.PlayerId == PlayerControl.LocalPlayer.PlayerId).Count > 0;

            DeadPlayer deadPlayer = GameHistory.deadPlayers?.Where(x => x.player?.PlayerId == target?.PlayerId)?.FirstOrDefault();
            if (isMedicReport || isDetectiveReport)
            {
                if (deadPlayer != null && deadPlayer.GetKiller != null) {
                    float timeSinceDeath = ((float)(DateTime.UtcNow - deadPlayer.DeathTime).TotalMilliseconds);
                    string msg = "";

                    if (isMedicReport) 
                    {
                        msg = $"Body Report: Killed {Math.Round(timeSinceDeath / 1000)}s ago!";
                    } else if (isDetectiveReport) 
                    {
                        if (timeSinceDeath < Detective.reportNameDuration * 1000) 
                        {
                            msg =  $"Body Report: The killer appears to be {deadPlayer.GetKiller.Data.PlayerName}!";
                        } 
                        else if (timeSinceDeath < Detective.reportColorDuration * 1000) 
                        {
                            var typeOfColor = Utils.IsLighterColor(deadPlayer.GetKiller) ? "lighter" : "darker";
                            msg =  $"Body Report: The killer appears to be a {typeOfColor} Color!";
                        } else {
                            msg = $"Body Report: The corpse is too old to gain information from!";
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(msg))
                    {   
                        if (AmongUsClient.Instance.AmClient && FastDestroyableSingleton<HudManager>.Instance)
                        {
                            FastDestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, msg);

                            // Ghost Info
                            Utils.SendRPC(CustomRPC.ShareGhostInfo, PlayerControl.LocalPlayer.PlayerId, (byte)GhostInfoTypes.DetectiveOrMedicInfo, msg);
                        }
                        if (msg.IndexOf("who", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            FastDestroyableSingleton<UnityTelemetry>.Instance.SendWho();
                        }
                    }
                }
            }
            if (IsSleuthReport)
            {
                Sleuth.Reported.Add(deadPlayer.player.PlayerId);
            }
        }
    }

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.MurderPlayer))]
    public static class MurderPlayerPatch
    {
        public static bool resetToCrewmate = false;
        public static bool resetToDead = false;

        public static void Prefix(PlayerControl __instance, [HarmonyArgument(0)]PlayerControl target)
        {
            // Allow everyone to murder players
            resetToCrewmate = !__instance.Data.Role.IsImpostor;
            resetToDead = __instance.Data.IsDead;
            __instance.Data.Role.TeamType = RoleTeamTypes.Impostor;
            __instance.Data.IsDead = false;
        }

        public static void Postfix(PlayerControl __instance, [HarmonyArgument(0)]PlayerControl target)
        {
            // Collect dead player info
            DeadPlayer deadPlayer = new DeadPlayer(target, DateTime.UtcNow, DeadPlayer.CustomDeathReason.Kill, __instance);
            GameHistory.deadPlayers.Add(deadPlayer);

            // Reset killer to crewmate if resetToCrewmate
            if (resetToCrewmate) __instance.Data.Role.TeamType = RoleTeamTypes.Crewmate;
            if (resetToDead) __instance.Data.IsDead = true;

            // Remove fake tasks when player dies
            if (target.HasFakeTasks() || target == Lawyer.Player || target == Survivor.Player)
                target.ClearAllTasks();

            if (Monarch.Player != null && Monarch.Player == target && Monarch.KnightLoseOnDeath) Monarch.KnightedPlayers.Clear();

            // First kill (set before lover suicide)
            if (MapOptions.FirstKillName == "") MapOptions.FirstKillName = target.Data.PlayerName;

            // Lover suicide trigger on murder
            if ((Lovers.Lover1 != null && target == Lovers.Lover1) || (Lovers.Lover2 != null && target == Lovers.Lover2)) {
                PlayerControl otherLover = target == Lovers.Lover1 ? Lovers.Lover2 : Lovers.Lover1;
                if (otherLover != null && !otherLover.Data.IsDead && Lovers.bothDie) {
                    otherLover.MurderPlayer(otherLover);
                    GameHistory.CreateDeathReason(otherLover, DeadPlayer.CustomDeathReason.LoverSuicide);
                }
            }

            if (__instance.AmOwner)
            {
                if (PlayerControl.LocalPlayer == Disperser.Player)
                {
                    for (int i = 0; i < Disperser.RechargeKillsCount; i++)
                    {
                        Disperser.Charges++;
                    }
                }
            }

            if (Deputy.Player == target) Deputy.CanExecute = false;

            // Cultist show kill flash
            if (Cultist.Player != null && Follower.Player != null)
            {
                PlayerControl player = null;
                if (__instance == Cultist.Player) player = Follower.Player;
                else if (__instance == Follower.Player) player = Cultist.Player;

                if (player != null && !player.Data.IsDead && player == PlayerControl.LocalPlayer)
                    Utils.ShowFlash(Palette.ImpostorRed, 1.5f);
            }

            if (PlayerControl.LocalPlayer == target)
            {
                try
                {
                    ShapeShifterMenu.Singleton.Menu.Close();
                }
                catch { }
            }

            // Survivor promotion trigger on murder (the host sends the call such that everyone recieves the update before a possible game End)
            if (target == Lawyer.target && AmongUsClient.Instance.AmHost && Lawyer.Player != null)
            {
                Utils.SendRPC(CustomRPC.LawyerChangeRole);
                RPCProcedure.LawyerChangeRole();
            }

            // Survivor promotion trigger on murder (the host sends the call such that everyone recieves the update before a possible game End)
            if (target == Romantic.beloved && AmongUsClient.Instance.AmHost && Romantic.Player != null) 
            {
                Utils.SendRPC(CustomRPC.RomanticChangeRole);
                RPCProcedure.RomanticChangeRole();
            }

            // Mystic show flash and add dead player position
            if (Mystic.Player != null && (PlayerControl.LocalPlayer == Mystic.Player || Utils.ShouldShowGhostInfo()) && !Mystic.Player.Data.IsDead && Mystic.Player != target && Mystic.mode <= 1) 
            {
                Utils.ShowFlash(new Color(42f / 255f, 187f / 255f, 245f / 255f), message : "Mystic Info: Someone Died");
                SoundEffectsManager.Play("DeadSound");
            }

            if (Mystic.deadBodyPositions != null) Mystic.deadBodyPositions.Add(target.transform.position);

            // Tracker store body positions
            if (Tracker.deadBodyPositions != null) Tracker.deadBodyPositions.Add(target.transform.position);

            // Psychic add body
            if (Psychic.deadBodies != null)
            {
                Psychic.futureDeadBodies.Add(new Tuple<DeadPlayer, Vector3>(deadPlayer, target.transform.position));
            }

            // Set bountyHunter Cooldown
            if (BountyHunter.Player != null && PlayerControl.LocalPlayer == BountyHunter.Player && __instance == BountyHunter.Player) {
                if (target == BountyHunter.bounty) 
                {
                    BountyHunter.Player.SetKillTimer(BountyHunter.bountyKillCooldown);
                    BountyHunter.bountyUpdateTimer = 0f; // Force bounty update
                }
                else
                    BountyHunter.Player.SetKillTimer(GameOptionsManager.Instance.currentNormalGameOptions.KillCooldown + BountyHunter.punishmentTime); 
            }

            // Janitor Button Sync
            if (Janitor.Player != null && PlayerControl.LocalPlayer == Janitor.Player && __instance == Janitor.Player && HudManagerStartPatch.JanitorCleanButton != null)
                HudManagerStartPatch.JanitorCleanButton.Timer = Janitor.Player.killTimer;

            // Witch Button Sync
            if (Witch.triggerBothCooldowns && Witch.Player != null && PlayerControl.LocalPlayer == Witch.Player && __instance == Witch.Player && HudManagerStartPatch.witchSpellButton != null)
                HudManagerStartPatch.witchSpellButton.Timer = HudManagerStartPatch.witchSpellButton.MaxTimer;

            // Warlock Button Sync
            if (Warlock.Player != null && PlayerControl.LocalPlayer == Warlock.Player && __instance == Warlock.Player && HudManagerStartPatch.warlockCurseButton != null) {
                if (Warlock.Player.killTimer > HudManagerStartPatch.warlockCurseButton.Timer) {
                    HudManagerStartPatch.warlockCurseButton.Timer = Warlock.Player.killTimer;
                }
            }
            // Assassin Button Sync
            if (Assassin.Player != null && PlayerControl.LocalPlayer == Assassin.Player && __instance == Assassin.Player && HudManagerStartPatch.AssassinButton != null)
                HudManagerStartPatch.AssassinButton.Timer = HudManagerStartPatch.AssassinButton.MaxTimer;

            // Bait
            if (Bait.Players.FindAll(x => x.PlayerId == target.PlayerId).Count > 0) 
            {
                float reportDelay = (float) rnd.Next((int)Bait.reportDelayMin, (int)Bait.reportDelayMax + 1);
                Bait.active.Add(deadPlayer, reportDelay);

                if (Bait.showKillFlash && __instance == PlayerControl.LocalPlayer) Utils.ShowFlash(new Color(204f / 255f, 102f / 255f, 0f / 255f));
            }

            // VIP Modifier
            if (Vip.Players.FindAll(x => x.PlayerId == target.PlayerId).Count > 0) 
            {
                Color Color = Color.yellow;
                if (Vip.showColor) 
                {
                    Color = Color.white;
                    if (target.Data.Role.IsImpostor) Color = Color.red;
                    else if (target.IsNeutral()) Color = Color.blue;
                }
                Utils.ShowFlash(Color, 1.5f);
            }
        }
    }

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.SetKillTimer))]
    class PlayerControlSetCoolDownPatch 
    {
        public static bool Prefix(PlayerControl __instance, [HarmonyArgument(0)]float time) 
        {
            if (GameOptionsManager.Instance.currentGameOptions.GameMode == GameModes.HideNSeek) return true;
            if (GameOptionsManager.Instance.currentNormalGameOptions.KillCooldown <= 0f) return false;
            float multiplier = 1f;
            float addition = 0f;
            if (BountyHunter.Player != null && PlayerControl.LocalPlayer == BountyHunter.Player) addition = BountyHunter.punishmentTime;

            __instance.killTimer = Mathf.Clamp(time, 0f, GameOptionsManager.Instance.currentNormalGameOptions.KillCooldown * multiplier + addition);
            FastDestroyableSingleton<HudManager>.Instance.KillButton.SetCoolDown(__instance.killTimer, GameOptionsManager.Instance.currentNormalGameOptions.KillCooldown * multiplier + addition);
            return false;
        }
    }

    [HarmonyPatch(typeof(KillAnimation), nameof(KillAnimation.CoPerformKill))]
    class KillAnimationCoPerformKillPatch 
    {
        public static bool hideNextAnimation = false;
        public static void Prefix(KillAnimation __instance, [HarmonyArgument(0)]ref PlayerControl source, [HarmonyArgument(1)]ref PlayerControl target) 
        {
            if (hideNextAnimation)
                source = target;
            hideNextAnimation = false;
        }
    }

    [HarmonyPatch(typeof(KillAnimation), nameof(KillAnimation.SetMovement))]
    class KillAnimationSetMovementPatch {
        private static int? colorId = null;
        public static void Prefix(PlayerControl source, bool canMove) 
        {
            Color Color = source.cosmetics.currentBodySprite.BodySprite.material.GetColor("_BodyColor");
            if ((Morphling.Player != null && source.Data.PlayerId == Morphling.Player.PlayerId) || 
            (Glitch.Player != null && source.Data.PlayerId == Glitch.Player.PlayerId)) 
            {
            var index = Palette.PlayerColors.IndexOf(Color);
            if (index != -1) colorId = index;
            }
        }

        public static void Postfix(PlayerControl source, bool canMove) {
            if (colorId.HasValue) source.RawSetColor(colorId.Value);
            colorId = null;
        }
    }

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.Exiled))]
    public static class ExilePlayerPatch
    {
        public static void Postfix(PlayerControl __instance)
        {
            // Collect dead player info
            DeadPlayer deadPlayer = new DeadPlayer(__instance, DateTime.UtcNow, DeadPlayer.CustomDeathReason.Exile, null);
            GameHistory.deadPlayers.Add(deadPlayer);


            // Remove fake tasks when player dies
            if (__instance.HasFakeTasks() || __instance == Lawyer.Player || __instance == Survivor.Player)
                __instance.ClearAllTasks();

            // Lover suicide trigger on exile
            if ((Lovers.Lover1 != null && __instance == Lovers.Lover1) || (Lovers.Lover2 != null && __instance == Lovers.Lover2)) {
                PlayerControl otherLover = __instance == Lovers.Lover1 ? Lovers.Lover2 : Lovers.Lover1;
                if (otherLover != null && !otherLover.Data.IsDead && Lovers.bothDie) {
                    otherLover.Exiled();
                    GameHistory.CreateDeathReason(otherLover, DeadPlayer.CustomDeathReason.LoverSuicide);
                }
            }

            if (Romantic.Player != null && !Romantic.Player.Data.IsDead && __instance == Romantic.beloved)
            {
                if (AmongUsClient.Instance.AmHost && (Romantic.beloved != Jester.Player))
                {
                    Utils.SendRPC(CustomRPC.RomanticChangeRole);
                    RPCProcedure.RomanticChangeRole();
                }
            }

            // Survivor promotion trigger on exile & suicide (the host sends the call such that everyone recieves the update before a possible game End)
            if (Lawyer.Player != null && __instance == Lawyer.target) 
            {
                PlayerControl lawyer = Lawyer.Player;
                if (AmongUsClient.Instance.AmHost && ((Lawyer.target != Jester.Player) || Lawyer.targetWasGuessed)) 
                {
                    Utils.SendRPC(CustomRPC.LawyerChangeRole);
                    RPCProcedure.LawyerChangeRole();
                }

                if (!Lawyer.targetWasGuessed) 
                {
                    if (Lawyer.Player != null) Lawyer.Player.Exiled();
                    if (Survivor.Player != null) Survivor.Player.Exiled();

                    Utils.SendRPC(CustomRPC.ShareGhostInfo, PlayerControl.LocalPlayer.PlayerId,
                    (byte)GhostInfoTypes.DeathReasonAndKiller, lawyer.PlayerId, 
                    (byte)DeadPlayer.CustomDeathReason.LawyerSuicide, lawyer.PlayerId);
                    GameHistory.CreateDeathReason(lawyer, DeadPlayer.CustomDeathReason.LawyerSuicide, lawyer);  // TODO: only executed on host?!
                }
            }
        }
    }

    [HarmonyPatch(typeof(PlayerPhysics), nameof(PlayerPhysics.FixedUpdate))]
    public static class PlayerPhysicsFixedUpdate 
    {
        public static void Postfix(PlayerPhysics __instance)
        {
            bool shouldInvert = Drunk.Players.FindAll(x => x.PlayerId == PlayerControl.LocalPlayer.PlayerId).Count > 0 && Drunk.meetings > 0;
            if (__instance.AmOwner &&
                AmongUsClient.Instance &&
                AmongUsClient.Instance.GameState == InnerNet.InnerNetClient.GameStates.Started &&
                !PlayerControl.LocalPlayer.Data.IsDead && 
                shouldInvert && 
                GameData.Instance && 
                __instance.myPlayer.CanMove)  
                __instance.body.velocity *= -1;
        }
    }

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.IsFlashlightEnabled))]
    public static class IsFlashlightEnabledPatch {
        public static bool Prefix(ref bool __result) {
            if (GameOptionsManager.Instance.currentGameOptions.GameMode == GameModes.HideNSeek)
                return true;
            __result = false;
            if (!PlayerControl.LocalPlayer.Data.IsDead && Lighter.Player != null && Lighter.Player.PlayerId == PlayerControl.LocalPlayer.PlayerId) {
                __result = true;
            }

            return false;
        }
    }

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.AdjustLighting))]
    public static class AdjustLight
    {
        public static bool Prefix(PlayerControl __instance)
        {
            if (__instance == null || PlayerControl.LocalPlayer == null || Lighter.Player == null) return true;

            bool hasFlashlight = !PlayerControl.LocalPlayer.Data.IsDead && Lighter.Player.PlayerId == PlayerControl.LocalPlayer.PlayerId;
            __instance.SetFlashlightInputMethod();
            __instance.lightSource.SetupLightingForGameplay(hasFlashlight, Lighter.flashlightWidth, __instance.TargetFlashlight.transform);

            return false;
        }
    }
    
    [HarmonyPatch(typeof(GameData), nameof(GameData.HandleDisconnect), new[] {typeof(PlayerControl), typeof(DisconnectReasons) })]
    public static class GameDataHandleDisconnectPatch 
    {
        public static void Prefix(GameData __instance, PlayerControl player, DisconnectReasons reason) 
        {
            if (MeetingHud.Instance) 
            {
                MeetingHudPatch.SwapperCheckAndReturnSwap(MeetingHud.Instance, player.PlayerId); 
                PlayerVoteArea voteArea = MeetingHud.Instance.playerStates.First(x => x.TargetPlayerId == player.PlayerId);
                if (Blackmailer.BlackmailedPlayer != null && voteArea.TargetPlayerId == Blackmailer.BlackmailedPlayer.PlayerId)
                {
                    if (BlackmailMeetingUpdate.prevXMark != null && BlackmailMeetingUpdate.prevOverlay != null)
                    {
                        voteArea.XMark.sprite = BlackmailMeetingUpdate.prevXMark;
                        voteArea.Overlay.sprite = BlackmailMeetingUpdate.prevOverlay;
                        voteArea.XMark.transform.localPosition = new Vector3(
                        voteArea.XMark.transform.localPosition.x - BlackmailMeetingUpdate.LetterXOffset,
                        voteArea.XMark.transform.localPosition.y - BlackmailMeetingUpdate.LetterYOffset,
                        voteArea.XMark.transform.localPosition.z);
                    }
                }
            }
        }
    }
}
