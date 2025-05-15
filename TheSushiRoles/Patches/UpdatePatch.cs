using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace TheSushiRoles.Patches 
{
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    class HudManagerUpdatePatch
    {
        private static Dictionary<byte, (string name, Color color)> TagColorDict = new();
        static void ResetNameTagsAndColors() 
        {
            var localPlayer = PlayerControl.LocalPlayer;
            var myData = PlayerControl.LocalPlayer.Data;
            var amImpostor = myData.Role.IsImpostor;
            var morphTimerNotUp = Morphling.morphTimer > 0f;
            var morphTargetNotNull = Morphling.morphTarget != null;

            var glitchTimerNotUp = Glitch.MimicTimer > 0f;
            var glitchTargetNotNull = Glitch.MimicTarget != null;

            var hitmanTimerNotUp = Hitman.MorphTimer > 0f;
            var hitmanTargetNotNull = Hitman.MorphTarget != null;

            var dict = TagColorDict;
            dict.Clear();
            
            foreach (var data in GameData.Instance.AllPlayers.GetFastEnumerator())
            {
                var player = data.Object;
                string text = data.PlayerName;
                Color color;
                if (player)
                {
                    var playerName = text;
                    if (morphTimerNotUp && morphTargetNotNull && Morphling.Player == player) playerName = Morphling.morphTarget.Data.PlayerName;
                    if (glitchTimerNotUp && glitchTargetNotNull && Glitch.Player == player) playerName = Glitch.MimicTarget.Data.PlayerName;
                    if (hitmanTimerNotUp && hitmanTargetNotNull && Hitman.Player == player) playerName = Hitman.MorphTarget.Data.PlayerName;
                    var nameText = player.cosmetics.nameText;
                
                    nameText.text = Utils.HidePlayerName(localPlayer, player) ? "" : playerName;
                    nameText.color = color = amImpostor && data.Role.IsImpostor ? Palette.ImpostorRed : Color.white;
                    nameText.color = nameText.color.SetAlpha(Chameleon.Visibility(player.PlayerId));
                }
                else
                {
                    color = Color.white;
                }
                
                
                dict.Add(data.PlayerId, (text, color));
            }
            
            if (MeetingHud.Instance != null) 
            {
                foreach (PlayerVoteArea playerVoteArea in MeetingHud.Instance.playerStates)
                {
                    var data = dict[playerVoteArea.TargetPlayerId];
                    var text = playerVoteArea.NameText;
                    text.text = data.name;
                    text.color = data.color;
                }
            }
        }
        static void SetPlayerNameColor(PlayerControl p, Color color) 
        {
            p.cosmetics.nameText.color = color.SetAlpha(Chameleon.Visibility(p.PlayerId));
            if (MeetingHud.Instance != null)
                foreach (PlayerVoteArea player in MeetingHud.Instance.playerStates)
                    if (player.NameText != null && p.PlayerId == player.TargetPlayerId)
                        player.NameText.color = color;
        }

        static void ParanoidUpdate()
        {
            if (Paranoid.Arrow?.arrow != null &&
                Paranoid.Player != null &&
                PlayerControl.LocalPlayer == Paranoid.Player &&
                !Paranoid.Player.Data.IsDead)
            {
                var players = PlayerControl.AllPlayerControls;
                PlayerControl closest = null;
                float minDist = float.MaxValue;

                foreach (var pc in players)
                {
                    if (pc == null || pc == Paranoid.Player || pc.Data == null || pc.Data.IsDead)
                        continue;

                    float dist = Vector2.Distance(pc.transform.position, Paranoid.Player.transform.position);
                    if (dist < minDist)
                    {
                        minDist = dist;
                        closest = pc;
                    }
                }
        
                Paranoid.ClosestPlayer = closest;
        
                if (closest != null && closest.transform != null)
                {
                    Paranoid.Arrow.Update(closest.transform.position);
                    if (!Paranoid.Arrow.arrow.activeSelf)
                        Paranoid.Arrow.arrow.SetActive(true);
                }
                else
                {
                    Paranoid.Arrow.arrow.SetActive(false);
                }
            }
            else if (Paranoid.Arrow?.arrow != null)
            {
                Paranoid.Arrow.arrow.SetActive(false);
            }
        }


        static void UpdateOracle(MeetingHud __instance)
        {
            foreach (var player in PlayerControl.AllPlayerControls)
            {
                foreach (var state in __instance.playerStates)
                {
                    if (player.PlayerId != state.TargetPlayerId) continue;
                    if (player == Oracle.Confessor)
                    {
                        if (Oracle.RevealedFaction == Faction.Crewmates) state.NameText.text = state.NameText.text + $" <size=60%>(<color=#00FFFFFF>{Oracle.Accuracy}% Crew</color>) </size>";
                        else if (Oracle.RevealedFaction == Faction.Impostors) state.NameText.text = state.NameText.text + $" <size=60%>(<color=#FF0000FF>{Oracle.Accuracy}% Imp</color>) </size>";
                        else state.NameText.text = state.NameText.text + $" <size=60%>(<color=#808080FF>{Oracle.Accuracy}% Neut</color>) </size>";
                    }
                }
            }
        }

        static void SetNameColors() 
        {
            var localPlayer = PlayerControl.LocalPlayer;
            var localRole = RoleInfo.GetRoleInfoForPlayer(localPlayer).FirstOrDefault();
            SetPlayerNameColor(localPlayer, localRole.Color);
            if (Jackal.Player != null && Jackal.Player == localPlayer) 
            {
                // Jackal can see his sidekick
                SetPlayerNameColor(Jackal.Player, Jackal.Color);
                if (Sidekick.Player != null) 
                {
                    SetPlayerNameColor(Sidekick.Player, Jackal.Color);
                }
            }

            // Show flashed players
            if (Grenadier.Player != null && (Grenadier.Player == PlayerControl.LocalPlayer || Utils.ShouldShowGhostInfo())) 
            {
                foreach (PlayerControl player in Grenadier.FlashedPlayers)
                    if (!player.Data.Role.IsImpostor && !player.Data.IsDead)
                        SetPlayerNameColor(player, Color.black);
            }

            // a Lover of team Jackal needs the colors
            if (Sidekick.Player != null && Sidekick.Player == localPlayer) 
            {
                // Sidekick can see the jackal
                SetPlayerNameColor(Sidekick.Player, Sidekick.Color);
                if (Jackal.Player != null) 
                {
                    SetPlayerNameColor(Jackal.Player, Jackal.Color);
                }
            }

            // No else if here, as the Impostors need the Spy name to be colored
            if (Spy.Player != null && localPlayer.Data.Role.IsImpostor) 
            {
                SetPlayerNameColor(Spy.Player, Spy.Color);
            }
            if (Sidekick.Player != null && Sidekick.wasTeamRed && localPlayer.Data.Role.IsImpostor) 
            {
                SetPlayerNameColor(Sidekick.Player, Spy.Color);
            }
            if (Jackal.Player != null && Jackal.wasTeamRed && localPlayer.Data.Role.IsImpostor) 
            {
                SetPlayerNameColor(Jackal.Player, Spy.Color);
            }

            // Crewmate roles with no changes: Mini
            // Impostor roles with no changes: Morphling, Camouflager, Poisoner, Eraser, Cleaner, Warlock, BountyHunter,  Witch
        }

        static void SetNameTags() 
        {
            // Lovers
            if (Lovers.Lover1 != null && Lovers.Lover2 != null && (Lovers.Lover1 == PlayerControl.LocalPlayer || Lovers.Lover2 == PlayerControl.LocalPlayer)) 
            {
                string suffix = Utils.ColorString(Lovers.Color, " ♥");
                Lovers.Lover1.cosmetics.nameText.text += suffix;
                Lovers.Lover2.cosmetics.nameText.text += suffix;

                if (MeetingHud.Instance != null)
                    foreach (PlayerVoteArea player in MeetingHud.Instance.playerStates)
                        if (Lovers.Lover1.PlayerId == player.TargetPlayerId || Lovers.Lover2.PlayerId == player.TargetPlayerId)
                            player.NameText.text += suffix;
            }

            // Plaguebearer infected players
            if (Plaguebearer.Player != null && Plaguebearer.Player == PlayerControl.LocalPlayer) 
            {
                foreach (PlayerControl player in PlayerControl.AllPlayerControls)
                {
                    if (Plaguebearer.InfectedPlayers.Contains(player.PlayerId))
                    {
                        string suffix = Utils.ColorString(Plaguebearer.Color, " ⦿");
                        player.cosmetics.nameText.text += suffix;

                        if (MeetingHud.Instance != null)
                        {
                            foreach (PlayerVoteArea voteArea in MeetingHud.Instance.playerStates)
                            {
                                if (voteArea.TargetPlayerId == player.PlayerId)
                                {
                                    voteArea.NameText.text += suffix;
                                }
                            }
                        }
                    }
                }
            }

            // Lawyer
            if (Lawyer.Player != null && Lawyer.target != null && Lawyer.Player == PlayerControl.LocalPlayer) 
            {
                Color color = Lawyer.Color;
                PlayerControl target = Lawyer.target;
                string suffix = Utils.ColorString(color, " §");
                target.cosmetics.nameText.text += suffix;

                if (MeetingHud.Instance != null)
                    foreach (PlayerVoteArea player in MeetingHud.Instance.playerStates)
                        if (player.TargetPlayerId == target.PlayerId)
                            player.NameText.text += suffix;
            }

            // Prosecutor
            if (Prosecutor.Player != null && Prosecutor.target != null && Prosecutor.Player == PlayerControl.LocalPlayer) 
            {
                PlayerControl target = Prosecutor.target;
                string suffix = Utils.ColorString(Prosecutor.Color, " ⦿");
                target.cosmetics.nameText.text += suffix;

                if (MeetingHud.Instance != null)
                    foreach (PlayerVoteArea player in MeetingHud.Instance.playerStates)
                        if (player.TargetPlayerId == target.PlayerId)
                            player.NameText.text += suffix;
            }

            // Display lighter / darker color for all alive players
            if (PlayerControl.LocalPlayer != null && MeetingHud.Instance != null && MapOptions.showLighterDarker) 
            {
                foreach (PlayerVoteArea player in MeetingHud.Instance.playerStates) {
                    var target = Utils.PlayerById(player.TargetPlayerId);
                    if (target != null)  player.NameText.text += $" ({(Utils.IsLighterColor(target) ? "L" : "D")})";
                }
            }

            // Add medic Shield info:
            if (MeetingHud.Instance != null && Medic.Player != null && Medic.Shielded != null && Medic.ShieldVisible(Medic.Shielded)) 
            {
                foreach (PlayerVoteArea player in MeetingHud.Instance.playerStates)
                    if (player.TargetPlayerId == Medic.Shielded.PlayerId) 
                    {
                        player.NameText.text = Utils.ColorString(Medic.Color, "[") + player.NameText.text + Utils.ColorString(Medic.Color, "]");
                        // player.HighlightedFX.color = Medic.Color;
                        // player.HighlightedFX.enabled = true;
                    }
            }
        }

        static void UpdateShielded() 
        {
            if (Medic.Shielded == null) return;

            if (Medic.Shielded.Data.IsDead || Medic.Player == null || Medic.Player.Data.IsDead) 
            {
                Medic.Shielded = null;
            }
            else if (Medic.Player.Data.IsDead) 
            {
                Medic.Shielded.ShowFailedMurder();
            }
        }

        static void TimerUpdate() 
        {
            var dt = Time.deltaTime;
            Hacker.hackerTimer -= dt;
            Trickster.lightsOutTimer -= dt;
            Tracker.corpsesTrackingTimer -= dt;
            Ninja.invisibleTimer -= dt;
            Wraith.VanishTimer -= dt;
            foreach (byte key in Glitch.HackedKnows.Keys)
                Glitch.HackedKnows[key] -= dt;
        }

        public static void MiniUpdate() 
        {
            if (Mini.Player == null || Camouflager.CamouflageTimer > 0f || Utils.MushroomSabotageActive() || Mini.Player == Morphling.Player && Morphling.morphTimer > 0f || Mini.Player == Glitch.Player && Glitch.MimicTimer > 0f 
            || Mini.Player == Hitman.Player && Hitman.MorphTimer > 0f || Mini.Player == Ninja.Player && Ninja.isInvisble || Mini.Player == Wraith.Player && Wraith.IsVanished || SurveillanceMinigamePatch.nightVisionIsActive) return;
                
            float growingProgress = Mini.GrowingProgress();
            float scale = growingProgress * 0.35f + 0.35f;
            string suffix = "";
            if (growingProgress != 1f)
                suffix = " <color=#FAD934FF>(" + Mathf.FloorToInt(growingProgress * 18) + ")</color>"; 
            if (!Mini.isGrowingUpInMeeting && MeetingHud.Instance != null && Mini.ageOnMeetingStart != 0 && !(Mini.ageOnMeetingStart >= 18))
                suffix = " <color=#FAD934FF>(" + Mini.ageOnMeetingStart + ")</color>";

            Mini.Player.cosmetics.nameText.text += suffix;
            if (MeetingHud.Instance != null) 
            {
                foreach (PlayerVoteArea player in MeetingHud.Instance.playerStates)
                    if (player.NameText != null && Mini.Player.PlayerId == player.TargetPlayerId)
                        player.NameText.text += suffix;
            }

            if (Morphling.Player != null && Morphling.morphTarget == Mini.Player && Morphling.morphTimer > 0f)
                Morphling.Player.cosmetics.nameText.text += suffix;
            if (Glitch.Player != null && Glitch.MimicTarget == Mini.Player && Glitch.MimicTimer > 0f)
                Glitch.Player.cosmetics.nameText.text += suffix;
            if (Hitman.Player != null && Hitman.MorphTarget == Mini.Player && Hitman.MorphTimer > 0f)
                Hitman.Player.cosmetics.nameText.text += suffix;
        }

        static void UpdateImpostorKillButton(HudManager __instance) 
        {
            if (!PlayerControl.LocalPlayer.Data.Role.IsImpostor) return;
            if (MeetingHud.Instance) {
                __instance.KillButton.Hide();
                return;
            }
            bool enabled = true;
            if (Poisoner.Player != null && Poisoner.Player == PlayerControl.LocalPlayer) enabled = false;
            
            if (enabled) __instance.KillButton.Show();
            else __instance.KillButton.Hide();

            if (Glitch.HackedKnows.ContainsKey(PlayerControl.LocalPlayer.PlayerId) && Glitch.HackedKnows[PlayerControl.LocalPlayer.PlayerId] > 0) __instance.KillButton.Hide();
        }

        static void UpdateReportButton(HudManager __instance) 
        {
            if (GameOptionsManager.Instance.currentGameOptions.GameMode == GameModes.HideNSeek) return;
            if (Glitch.HackedKnows.ContainsKey(PlayerControl.LocalPlayer.PlayerId) && Glitch.HackedKnows[PlayerControl.LocalPlayer.PlayerId] > 0 || MeetingHud.Instance || Utils.TwoPlayersAlive() && MapOptions.LimitAbilities) __instance.ReportButton.Hide();
            else if (!__instance.ReportButton.isActiveAndEnabled) __instance.ReportButton.Show();
        }
         
        static void UpdateVentButton(HudManager __instance)
        {
            if (GameOptionsManager.Instance.currentGameOptions.GameMode == GameModes.HideNSeek) return;
            if (Utils.TwoPlayersAlive() && MapOptions.LimitAbilities) return;
            if (PlayerControl.LocalPlayer == Poisoner.Player) 
            {
                __instance.ImpostorVentButton.Show();
                __instance.ImpostorVentButton.transform.localPosition = new Vector3(-1f, 1f, 0f);
            }
            if (PlayerControl.LocalPlayer == Wraith.Player) __instance.ImpostorVentButton.Hide();
            if (Glitch.HackedKnows.ContainsKey(PlayerControl.LocalPlayer.PlayerId) && Glitch.HackedKnows[PlayerControl.LocalPlayer.PlayerId] > 0 || MeetingHud.Instance) __instance.ImpostorVentButton.Hide();
            else if (PlayerControl.LocalPlayer.IsVenter() && !__instance.ImpostorVentButton.isActiveAndEnabled) 
            {
                __instance.ImpostorVentButton.Show();
            }
            if (Rewired.ReInput.players.GetPlayer(0).GetButtonDown(RewiredConsts.Action.UseVent) && !PlayerControl.LocalPlayer.Data.Role.IsImpostor && PlayerControl.LocalPlayer.IsVenter()) 
            {
                __instance.ImpostorVentButton.DoClick();
            }
        }

        static void UpdateUseButton(HudManager __instance) 
        {
            if (MeetingHud.Instance) __instance.UseButton.Hide();
        }

        static void UpdateSabotageButton(HudManager __instance) 
        {
            if (MeetingHud.Instance || Utils.TwoPlayersAlive() && MapOptions.LimitAbilities) __instance.SabotageButton.Hide();
            if (PlayerControl.LocalPlayer.Data.IsDead && CustomOptionHolder.deadImpsBlockSabotage.GetBool()) __instance.SabotageButton.Hide();
        }

        static void UpdateMapButton(HudManager __instance) 
        {
            if (Trapper.Player == null || !(PlayerControl.LocalPlayer.PlayerId == Trapper.Player.PlayerId) || __instance == null || __instance.MapButton.HeldButtonSprite == null) return;
            __instance.MapButton.HeldButtonSprite.color = Trapper.playersOnMap.Any() ? Trapper.Color : Color.white;
        }

        static void Postfix(HudManager __instance)
        {
            if (AmongUsClient.Instance.GameState != InnerNet.InnerNetClient.GameStates.Started || GameOptionsManager.Instance.currentGameOptions.GameMode == GameModes.HideNSeek) return;

            CustomButton.HudUpdate();
            ResetNameTagsAndColors();
            SetNameColors();
            UpdateShielded();
            SetNameTags();

            // Impostors
            UpdateImpostorKillButton(__instance);

            if (Oracle.Player != null && Oracle.Player.Data.IsDead && Oracle.Confessor != null) UpdateOracle(MeetingHud.Instance);

            // Timer updates
            TimerUpdate();
            // Mini
            MiniUpdate();

            // Glitch Sabotage, Use and Vent Button Disabling
            UpdateReportButton(__instance);
            UpdateVentButton(__instance);
            // Meeting hide buttons if needed (used for the map usage, because closing the map would show buttons)
            UpdateSabotageButton(__instance);
            UpdateUseButton(__instance);
            ParanoidUpdate();
            UpdateMapButton(__instance);
            if (!MeetingHud.Instance) __instance.AbilityButton?.Update();

            // Fix dead player's pets being visible by just always updating whether the pet should be visible at all.
            foreach (PlayerControl target in PlayerControl.AllPlayerControls) 
            {
                var pet = target.GetPet();
                if (pet != null) 
                {
                    pet.Visible = (PlayerControl.LocalPlayer.Data.IsDead && target.Data.IsDead || !target.Data.IsDead) && !target.inVent;
                }
            }
        }
    }
    //Reports can't happen by clicking the corpse
    [HarmonyPatch(typeof(DeadBody), nameof(DeadBody.OnClick))]
    public static class DeadBodyOnClickUpdate
    {
        public static bool Prefix(DeadBody __instance) 
        {
            if (GameOptionsManager.Instance.CurrentGameOptions.GameMode == GameModes.HideNSeek) return false;
            if (Utils.TwoPlayersAlive() && MapOptions.LimitAbilities  || Glitch.HackedKnows.ContainsKey(PlayerControl.LocalPlayer.PlayerId) && Glitch.HackedKnows[PlayerControl.LocalPlayer.PlayerId] > 0f)  return false;
            return true;
        }
    }
    [HarmonyPatch(typeof(Vent), nameof(Vent.SetOutline))]
    class SetVentOutlinePatch
    {
        public static void Postfix(Vent __instance, [HarmonyArgument(1)] ref bool mainTarget)
        {
            var roleInfo = RoleInfo.GetRoleInfoForPlayer(PlayerControl.LocalPlayer);
            foreach (RoleInfo role in roleInfo)
            {
                Color color = role.Color;
                __instance.myRend.material.SetColor("_OutlineColor", color);
                __instance.myRend.material.SetColor("_AddColor", mainTarget ? color : Color.clear);
            }
        }
    }
}
