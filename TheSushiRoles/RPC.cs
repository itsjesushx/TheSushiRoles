using Hazel;
using static TheSushiRoles.HudManagerStartPatch;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using AmongUs.Data;
using Assets.CoreScripts;
using Reactor.Networking.Extensions;
using Reactor.Utilities.Extensions;

namespace TheSushiRoles
{
    public static class RPCProcedure 
    {
        // Main Controls
        public static void ResetVariables() 
        {
            ClearAndReloadMapOptions();
            GlobalClearAndReload();
            GameHistory.ClearGameHistory();
            SetCustomButtonCooldowns();
            ReloadPluginOptions();
        }

        public static void HandleShareOptions(byte numberOfOptions, MessageReader reader) 
        {
            try 
            {
                for (int i = 0; i < numberOfOptions; i++) 
                {
                    uint optionId = reader.ReadPackedUInt32();
                    uint selection = reader.ReadPackedUInt32();
                    CustomOption option = CustomOption.options.First(option => option.id == (int)optionId);
                    option.UpdateSelection((int)selection, i == numberOfOptions - 1);
                }
            } 
            catch (Exception e) 
            {
                TheSushiRolesPlugin.Logger.LogError("Error while deserializing options: " + e.Message);
            }
        }

        public static void ForceEnd() 
        {
            if (AmongUsClient.Instance.GameState != InnerNet.InnerNetClient.GameStates.Started) return;
            foreach (PlayerControl player in PlayerControl.AllPlayerControls)
            {
                if (!player.Data.Role.IsImpostor)
                {
                    
                    GameData.Instance.GetPlayerById(player.PlayerId); // player.RemoveInfected(); (was removed in 2022.12.08, no idea if we ever need that part again, replaced by these 2 lines.) 
                    player.CoSetRole(RoleTypes.Crewmate, true);

                    player.MurderPlayer(player);
                    player.Data.IsDead = true;
                }
            }
        }

        public static void StopStart(byte playerId) 
        {
            if (!CustomOptionHolder.anyPlayerCanStopStart.GetBool()) return;

            SoundManager.Instance.StopSound(GameStartManager.Instance.gameStartSound);
            if (AmongUsClient.Instance.AmHost) 
            {
                GameStartManager.Instance.ResetStartState();
                PlayerControl.LocalPlayer.RpcSendChat($"{Utils.PlayerById(playerId).Data.PlayerName} stopped the game start!");
            }
        }

        public static void WorkaroundSetRoles(byte numberOfRoles, MessageReader reader)
        {
                for (int i = 0; i < numberOfRoles; i++)
                {                   
                    byte playerId = (byte) reader.ReadPackedUInt32();
                    byte roleId = (byte) reader.ReadPackedUInt32();
                    try {
                        SetRole(roleId, playerId);
                    } catch (Exception e) {
                        TheSushiRolesPlugin.Logger.LogError("Error while deserializing roles: " + e.Message);
                    }
            }
            
        }

        public static void SetRole(byte roleId, byte playerId) 
        {
            foreach (PlayerControl player in PlayerControl.AllPlayerControls) 
            {
                if (player.PlayerId == playerId) 
                {
                    switch ((RoleId)roleId) 
                    {
                    case RoleId.Jester:
                        Jester.Player = player;
                        RoleInfo.AddToRoleHistory(player.PlayerId, RoleInfo.jester);
                        break;
                    case RoleId.Mayor:
                        Mayor.Player = player;
                        RoleInfo.AddToRoleHistory(player.PlayerId, RoleInfo.mayor);
                        break;
                    case RoleId.Portalmaker:
                        Portalmaker.Player = player;
                        RoleInfo.AddToRoleHistory(player.PlayerId, RoleInfo.portalmaker);
                        break;
                    case RoleId.Engineer:
                        Engineer.Player = player;
                        RoleInfo.AddToRoleHistory(player.PlayerId, RoleInfo.engineer);
                        break;
                    case RoleId.Sheriff:
                        Sheriff.Player = player;
                        RoleInfo.AddToRoleHistory(player.PlayerId, RoleInfo.sheriff);
                        break;
                    case RoleId.VengefulRomantic:
                        VengefulRomantic.Player = player;
                        RoleInfo.AddToRoleHistory(player.PlayerId, RoleInfo.vromantic);
                        break;
                    case RoleId.Blackmailer:
                        Blackmailer.Player = player;
                        RoleInfo.AddToRoleHistory(player.PlayerId, RoleInfo.blackmailer);
                        break;
                    case RoleId.Glitch:
                        Glitch.Player = player;
                        RoleInfo.AddToRoleHistory(player.PlayerId, RoleInfo.glitch);
                        break;
                    case RoleId.Werewolf:
                        Werewolf.Player = player;
                        RoleInfo.AddToRoleHistory(player.PlayerId, RoleInfo.werewolf);
                        break;
                    case RoleId.Lighter:
                        Lighter.Player = player;
                        RoleInfo.AddToRoleHistory(player.PlayerId, RoleInfo.lighter);
                        break;
                    case RoleId.Agent:
                        Agent.Player = player;
                        RoleInfo.AddToRoleHistory(player.PlayerId, RoleInfo.agent);
                        break;
                    case RoleId.Hitman:
                        Hitman.Player = player;
                        RoleInfo.AddToRoleHistory(player.PlayerId, RoleInfo.hitman);
                        break;
                    case RoleId.Undertaker:
                        Undertaker.Player = player;
                        RoleInfo.AddToRoleHistory(player.PlayerId, RoleInfo.undertaker);
                        break;
                    case RoleId.Oracle:
                        Oracle.Player = player;
                        RoleInfo.AddToRoleHistory(player.PlayerId, RoleInfo.oracle);
                        break;
                    case RoleId.Amnesiac:
                        Amnesiac.Player = player;
                        RoleInfo.AddToRoleHistory(player.PlayerId, RoleInfo.amnesiac);
                        break;
                    case RoleId.Plaguebearer:
                        Plaguebearer.Player = player;
                        RoleInfo.AddToRoleHistory(player.PlayerId, RoleInfo.plaguebearer);
                        break;
                    case RoleId.Pestilence:
                        Pestilence.Player = player;
                        RoleInfo.AddToRoleHistory(player.PlayerId, RoleInfo.pestilence);
                        break;
                    case RoleId.Detective:
                        Detective.Player = player;
                        RoleInfo.AddToRoleHistory(player.PlayerId, RoleInfo.detective);
                        break;
                    case RoleId.TimeMaster:
                        TimeMaster.Player = player;
                        RoleInfo.AddToRoleHistory(player.PlayerId, RoleInfo.timeMaster);
                        break;
                    case RoleId.Veteran:
                        Veteran.Player = player;
                        RoleInfo.AddToRoleHistory(player.PlayerId, RoleInfo.veteran);
                        break;
                    case RoleId.Medic:
                        Medic.Player = player;
                        RoleInfo.AddToRoleHistory(player.PlayerId, RoleInfo.medic);
                        break;
                    case RoleId.Crusader:
                        Crusader.Player = player;
                        RoleInfo.AddToRoleHistory(player.PlayerId, RoleInfo.crusader);
                        break;
                    case RoleId.Miner:
                        Miner.Player = player;
                        RoleInfo.AddToRoleHistory(player.PlayerId, RoleInfo.miner);
                        break;
                    case RoleId.Swapper:
                        Swapper.Player = player;
                        RoleInfo.AddToRoleHistory(player.PlayerId, RoleInfo.swapper);
                        break;
                    case RoleId.Mystic:
                        Mystic.Player = player;
                        RoleInfo.AddToRoleHistory(player.PlayerId, RoleInfo.mystic);
                        break;
                    case RoleId.Juggernaut:
                        Juggernaut.Player = player;
                        RoleInfo.AddToRoleHistory(player.PlayerId, RoleInfo.juggernaut);
                        break;
                    case RoleId.Morphling:
                        Morphling.Player = player;
                        RoleInfo.AddToRoleHistory(player.PlayerId, RoleInfo.morphling);
                        break;
                    case RoleId.Camouflager:
                        Camouflager.Player = player;
                        RoleInfo.AddToRoleHistory(player.PlayerId, RoleInfo.camouflager);
                        break;
                    case RoleId.Predator:
                        Predator.Player = player;
                        RoleInfo.AddToRoleHistory(player.PlayerId, RoleInfo.predator);
                        break;
                    case RoleId.Hacker:
                        Hacker.Player = player;
                        RoleInfo.AddToRoleHistory(player.PlayerId, RoleInfo.hacker);
                        break;
                    case RoleId.Tracker:
                        Tracker.Player = player;
                        RoleInfo.AddToRoleHistory(player.PlayerId, RoleInfo.tracker);
                        break;
                    case RoleId.Poisoner:
                        Poisoner.Player = player;
                        RoleInfo.AddToRoleHistory(player.PlayerId, RoleInfo.poisoner);
                        break;
                    case RoleId.Jackal:
                        Jackal.Player = player;
                        RoleInfo.AddToRoleHistory(player.PlayerId, RoleInfo.jackal);
                        break;
                    case RoleId.Romantic:
                        Romantic.Player = player;
                        RoleInfo.AddToRoleHistory(player.PlayerId, RoleInfo.romantic);
                        break;
                    case RoleId.Sidekick:
                        Sidekick.Player = player;
                        RoleInfo.AddToRoleHistory(player.PlayerId, RoleInfo.sidekick);
                        break;
                    case RoleId.Eraser:
                        Eraser.Player = player;
                        RoleInfo.AddToRoleHistory(player.PlayerId, RoleInfo.eraser);
                        break;
                    case RoleId.Spy:
                        Spy.Player = player;
                        RoleInfo.AddToRoleHistory(player.PlayerId, RoleInfo.spy);
                        break;
                    case RoleId.Trickster:
                        Trickster.Player = player;
                        RoleInfo.AddToRoleHistory(player.PlayerId, RoleInfo.trickster);
                        break;
                    case RoleId.Cleaner:
                        Cleaner.Player = player;
                        RoleInfo.AddToRoleHistory(player.PlayerId, RoleInfo.cleaner);
                        break;
                    case RoleId.Warlock:
                        Warlock.Player = player;
                        RoleInfo.AddToRoleHistory(player.PlayerId, RoleInfo.warlock);
                        break;
                    case RoleId.Grenadier:
                        Grenadier.Player = player;
                        RoleInfo.AddToRoleHistory(player.PlayerId, RoleInfo.grenadier);
                        break;
                    case RoleId.Vigilante:
                        Vigilante.Player = player;
                        RoleInfo.AddToRoleHistory(player.PlayerId, RoleInfo.vigilante);
                        break;
                    case RoleId.Arsonist:
                        Arsonist.Player = player;
                        RoleInfo.AddToRoleHistory(player.PlayerId, RoleInfo.arsonist);
                        break;
                    case RoleId.BountyHunter:
                        BountyHunter.Player = player;
                        RoleInfo.AddToRoleHistory(player.PlayerId, RoleInfo.bountyHunter);
                        break;
                    case RoleId.Vulture:
                        Vulture.Player = player;
                        RoleInfo.AddToRoleHistory(player.PlayerId, RoleInfo.vulture);
                        break;
                    case RoleId.Medium:
                        Medium.medium = player;
                        RoleInfo.AddToRoleHistory(player.PlayerId, RoleInfo.medium);
                        break;
                    case RoleId.Trapper:
                        Trapper.Player = player;
                        RoleInfo.AddToRoleHistory(player.PlayerId, RoleInfo.trapper);
                        break;
                    case RoleId.Lawyer:
                        Lawyer.Player = player;
                        RoleInfo.AddToRoleHistory(player.PlayerId, RoleInfo.lawyer);
                        break;
                    case RoleId.Prosecutor:
                        Prosecutor.Player = player;
                        RoleInfo.AddToRoleHistory(player.PlayerId, RoleInfo.prosecutor);
                        break;
                    case RoleId.Pursuer:
                        Pursuer.Player = player;
                        RoleInfo.AddToRoleHistory(player.PlayerId, RoleInfo.pursuer);
                        break;
                    case RoleId.Witch:
                        Witch.Player = player;
                        RoleInfo.AddToRoleHistory(player.PlayerId, RoleInfo.witch);
                        break;
                    case RoleId.Ninja:
                        Ninja.Player = player;
                        RoleInfo.AddToRoleHistory(player.PlayerId, RoleInfo.ninja);
                        break;
                    case RoleId.Wraith:
                        Wraith.Player = player;
                        RoleInfo.AddToRoleHistory(player.PlayerId, RoleInfo.wraith);
                        break;
                    case RoleId.Yoyo:
                        Yoyo.Player = player;
                        RoleInfo.AddToRoleHistory(player.PlayerId, RoleInfo.yoyo);
                        break;
                    }
                    if (AmongUsClient.Instance.AmHost && player.IsVenter() && !player.Data.Role.IsImpostor) 
                    {
                        player.RpcSetRole(RoleTypes.Engineer);
                        player.CoSetRole(RoleTypes.Engineer, true);
                    }
                }
            }
        }
        public static void SetAbility(byte abilityId, byte playerId, byte flag) 
        {
            PlayerControl player = Utils.PlayerById(playerId); 
            switch ((AbilityId)abilityId) 
            {
                case AbilityId.Coward:
                    Coward.Player = player;
                    break;
                case AbilityId.Disperser:
                    Disperser.Player = player;
                    break;
                case AbilityId.Paranoid:
                    Paranoid.Player = player;
                    break;
            }
        }

        public static void SetModifier(byte modifierId, byte playerId, byte flag) 
        {
            PlayerControl player = Utils.PlayerById(playerId); 
            switch ((ModifierId)modifierId) 
            {
                case ModifierId.Bait:
                    Bait.Players.Add(player);
                    break;
                case ModifierId.Lover:
                    if (flag == 0) Lovers.Lover1 = player;
                    else Lovers.Lover2 = player;
                    break;
                case ModifierId.Lazy:
                    Lazy.Players.Add(player);
                    break;
                case ModifierId.Sleuth:
                    Sleuth.Players.Add(player);
                    break;
                case ModifierId.Tiebreaker:
                    Tiebreaker.Player = player;
                    break;
                case ModifierId.Sunglasses:
                    Sunglasses.Players.Add(player);
                    break;
                case ModifierId.Mini:
                    Mini.Player = player;
                    break;
                case ModifierId.Vip:
                    Vip.Players.Add(player);
                    break;
                case ModifierId.Invert:
                    Invert.Players.Add(player);
                    break;
                case ModifierId.Chameleon:
                    Chameleon.Players.Add(player);
                    break;
                case ModifierId.Armored:
                    Armored.Player = player;
                    break;
            }
        }

        public static void VersionHandshake(int major, int minor, int build, int revision, Guid guid, int clientId) 
        {
            System.Version ver;
            if (revision < 0) 
                ver = new System.Version(major, minor, build);
            else 
                ver = new System.Version(major, minor, build, revision);
            GameStartManagerPatch.playerVersions[clientId] = new GameStartManagerPatch.PlayerVersion(ver, guid);
        }

        public static void UseUncheckedVent(int ventId, byte playerId, byte isEnter) 
        {
            PlayerControl player = Utils.PlayerById(playerId);
            if (player == null) return;
            // Fill dummy MessageReader and call MyPhysics.HandleRpc as the corountines cannot be accessed
            MessageReader reader = new MessageReader();
            byte[] bytes = BitConverter.GetBytes(ventId);
            if (!BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            reader.Buffer = bytes;
            reader.Length = bytes.Length;

            JackInTheBox.StartAnimation(ventId);
            player.MyPhysics.HandleRpc(isEnter != 0 ? (byte)19 : (byte)20, reader);
        }

        public static void UncheckedMurderPlayer(byte sourceId, byte targetId, byte showAnimation) 
        {
            if (AmongUsClient.Instance.GameState != InnerNet.InnerNetClient.GameStates.Started) return;
            PlayerControl source = Utils.PlayerById(sourceId);
            PlayerControl target = Utils.PlayerById(targetId);
            if (source != null && target != null) 
            {
                if (showAnimation == 0) KillAnimationCoPerformKillPatch.hideNextAnimation = true;
                source.MurderPlayer(target);
            }
        }

        public static void UncheckedCmdReportDeadBody(byte sourceId, byte targetId) 
        {
            PlayerControl source = Utils.PlayerById(sourceId);
            var t = targetId == Byte.MaxValue ? null : Utils.PlayerById(targetId).Data;
            if (source != null) source.ReportDeadBody(t);
        }

        public static void UncheckedExilePlayer(byte targetId) 
        {
            PlayerControl target = Utils.PlayerById(targetId);
            if (target != null) target.Exiled();
        }

        public static void DynamicMapOption(byte mapId) 
        {
           GameOptionsManager.Instance.currentNormalGameOptions.MapId = mapId;
        }

        public static void SetGameStarting() 
        {
            GameStartManagerPatch.GameStartManagerUpdatePatch.startingTimer = 5f;
        }

        // Role functionality

        public static void EngineerFixLights() 
        {
            SwitchSystem switchSystem = MapUtilities.Systems[SystemTypes.Electrical].CastFast<SwitchSystem>();
            switchSystem.ActualSwitches = switchSystem.ExpectedSwitches;
        }

        public static void EngineerFixSubmergedOxygen() 
        {
            SubmergedCompatibility.RepairOxygen();
        }

        public static void EngineerUsedRepair() 
        {
            Engineer.remainingFixes--;
            if (Utils.ShouldShowGhostInfo()) 
            {
                Utils.ShowFlash(Engineer.Color, 0.5f, "Engineer Fix");
            }
        }

        public static void CleanBody(byte playerId, byte cleaningPlayerId) 
        {
            if (Medium.futureDeadBodies != null) 
            {
                var deadBody = Medium.futureDeadBodies.Find(x => x.Item1.player.PlayerId == playerId).Item1;
                if (deadBody != null) deadBody.WasCleanedOrEaten = true;
            }

            DeadBody[] array = UnityEngine.Object.FindObjectsOfType<DeadBody>();
            for (int i = 0; i < array.Length; i++) 
            {
                if (GameData.Instance.GetPlayerById(array[i].ParentId).PlayerId == playerId) {
                    UnityEngine.Object.Destroy(array[i].gameObject);
                }     
            }
            if (Vulture.Player != null && cleaningPlayerId == Vulture.Player.PlayerId) 
            {
                Vulture.eatenBodies++;
                if (Vulture.eatenBodies == Vulture.vultureNumberToWin) 
                {
                    Vulture.IsVultureWin = true;
                }
            }
        }

        public static void TimeMasterRewindTime()
        {
            TimeMaster.isRewinding = true;
            TimeMaster.Charges--;
            PlayerControl.LocalPlayer.moveable = false;
            Utils.ShowFlash(TimeMaster.Color, TimeMaster.RewindTimeDuration);
            foreach (var deadPlayer in GameHistory.deadPlayers)
            {
                if (deadPlayer.player == null) continue;
                if ((DateTime.UtcNow - deadPlayer.DeathTime).TotalSeconds < TimeMaster.RewindTimeDuration && TimeMaster.ReviveDuringRewind)
                {
                    var player = deadPlayer.player;
                    if (player.Data.IsDead)
                    {
                        Utils.Revive(player);
                        GameHistory.deadPlayers.Remove(deadPlayer); // Clean up as they got revived.
                        MapOptions.RevivedPlayers.Add(player.PlayerId);
                    }
                }
            }
            if (MapBehaviour.Instance)
                MapBehaviour.Instance.Close();
            if (Minigame.Instance)
                Minigame.Instance.ForceClose();
        }
        public static void TimeMasterStopRewindTime()
        {
            TimeMaster.isRewinding = false;
            PlayerControl.LocalPlayer.moveable = true;
            if (DestroyableSingleton<HudManager>.InstanceExists && DestroyableSingleton<HudManager>.Instance.FullScreen)
            {
                var fullscreen = DestroyableSingleton<HudManager>.Instance.FullScreen;
                if (fullscreen.color.Equals(TimeMaster.Color))
                {
                    fullscreen.color = new Color(1f, 0f, 0f, 0.37254903f);
                    fullscreen.enabled = false;
                }
            }
        }

        public static void VeteranAlert() 
        {
            Veteran.AlertActive = true;
            FastDestroyableSingleton<HudManager>.Instance.StartCoroutine(Effects.Lerp(Veteran.Duration, new Action<float>((p) => 
            {
                if (p == 1f) Veteran.AlertActive = false;
            })));
        }

        public static void VeteranAlertKill(byte targetId)
        {
            if (PlayerControl.LocalPlayer == Veteran.Player)
            {
                PlayerControl player = Utils.PlayerById(targetId);
                Utils.CheckMurderAttemptAndKill(Veteran.Player, player);
            }
        }
        public static void PestilenceKill(byte targetId)
        {
            if (PlayerControl.LocalPlayer == Pestilence.Player)
            {
                PlayerControl player = Utils.PlayerById(targetId);
                Utils.CheckMurderAttemptAndKill(Pestilence.Player, player);
            }
        }

        public static void MedicSetShielded(byte ShieldedId) 
        {
            Medic.usedShield = true;
            Medic.Shielded = Utils.PlayerById(ShieldedId);
            Medic.futureShielded = null;
        }
        public static void RomanticSetBeloved(byte belovedId) 
        {
            Romantic.HasLover = true;
            Romantic.beloved = Utils.PlayerById(belovedId);
        }
        public static void WerewolfMaul() 
        {
           var nearbyPlayers = Utils.GetClosestPlayers(Werewolf.Player.GetTruePosition(), Werewolf.Radius);

            foreach (var player in nearbyPlayers)
            {
                if (Werewolf.Player == player || player.Data.IsDead || player == Armored.Player && !Armored.isBrokenArmor || player == Medic.Shielded || player == FirstPlayerKilled)
                    continue;
                    
                Utils.CheckMurderAttemptAndKill(Werewolf.Player, player, showAnimation: false);

                Utils.StartRPC(CustomRPC.ShareGhostInfo, 
                PlayerControl.LocalPlayer.PlayerId, 
                (byte)GhostInfoTypes.DeathReasonAndKiller, 
                player.PlayerId, 
                (byte)DeadPlayer.CustomDeathReason.Maul,
                Werewolf.Player.PlayerId);
                GameHistory.CreateDeathReason(player, DeadPlayer.CustomDeathReason.Maul, killer: Werewolf.Player);
            }
        }

        public static void Disperse()
        {
            Dictionary<byte, Vector2> coordinates = GenerateDisperseCoordinates();

            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.Disperse, SendOption.Reliable, -1);
            writer.Write((byte)coordinates.Count);
            foreach ((byte key, Vector2 value) in coordinates)
            {
                writer.Write(key);
                writer.Write(value);
            }
            AmongUsClient.Instance.FinishRpcImmediately(writer);

            StartTransportation(coordinates);
            Disperser.Charges--;
        }
        public static void StartTransportation(Dictionary<byte, Vector2> coordinates)
        {
            if (coordinates.ContainsKey(PlayerControl.LocalPlayer.PlayerId))
            {
                Utils.ShowFlash(Palette.ImpostorRed, 2.5f);
                if (Minigame.Instance)
                {
                    try
                    {
                        Minigame.Instance.Close();
                    }
                    catch
                    {

                    }
                }

                if (PlayerControl.LocalPlayer.inVent)
                {
                    PlayerControl.LocalPlayer.MyPhysics.RpcExitVent(Vent.currentVent.Id);
                    PlayerControl.LocalPlayer.MyPhysics.ExitAllVents();
                }
            }


            foreach ((byte key, Vector2 value) in coordinates)
            {
                PlayerControl player = Utils.PlayerById(key);
                player.transform.position = value;
                if (PlayerControl.LocalPlayer == player) PlayerControl.LocalPlayer.NetTransform.RpcSnapTo(value);
            }

            if (PlayerControl.LocalPlayer.walkingToVent)
            {
                PlayerControl.LocalPlayer.inVent = false;
                Vent.currentVent = null;
                PlayerControl.LocalPlayer.moveable = true;
                PlayerControl.LocalPlayer.MyPhysics.StopAllCoroutines();
            }

            if (SubmergedCompatibility.IsSubmerged) SubmergedCompatibility.ChangeFloor(PlayerControl.LocalPlayer.transform.position.y > -7f);
        }

        private static Dictionary<byte, Vector2> GenerateDisperseCoordinates()
        {
            List<PlayerControl> targets = PlayerControl.AllPlayerControls.ToArray().Where(player => !player.Data.IsDead && !player.Data.Disconnected).ToList();

            HashSet<Vent> vents = UnityEngine.Object.FindObjectsOfType<Vent>().ToHashSet();

            Dictionary<byte, Vector2> coordinates = new Dictionary<byte, Vector2>(targets.Count);
            foreach (PlayerControl target in targets)
            {
                Vent vent = vents.Random();

                Vector3 destination = SendPlayerToVent(vent);
                coordinates.Add(target.PlayerId, destination);
            }
            return coordinates;
        }

        public static Vector3 SendPlayerToVent(Vent vent)
        {
            Vector2 size = vent.GetComponent<BoxCollider2D>().size;
            Vector3 destination = vent.transform.position;
            destination.y += 0.3636f;
            return destination;
        }

        public static void Fortify(byte fortifiedId)
        {
            Crusader.Charges--;
            Crusader.FortifiedPlayer = Utils.PlayerById(fortifiedId);
        }

        public static void Confess(byte confessorId)
        {
            if (Oracle.Player == null || Oracle.Player.Data.IsDead) return;

            Oracle.Confessor = Utils.PlayerById(confessorId);
            if (Oracle.Confessor == null) return;

            RoleInfo roleInfo = RoleInfo.GetRoleInfoForPlayer(Oracle.Confessor).FirstOrDefault();
            if (roleInfo == null) return;

            bool showsCorrectFaction = UnityEngine.Random.RandomRangeInt(1, 101) <= Oracle.Accuracy;
            Faction revealedFaction;

            if (showsCorrectFaction)
            {
                // Reveal the actual faction
                revealedFaction = roleInfo.FactionId;
            }
            else
            {
                // Get all possible factions
                List<Faction> possibleFaction = new List<Faction> { Faction.Crewmates, Faction.Impostors, Faction.Neutrals };

                // Remove the actual faction from the list so we never guess correctly
                possibleFaction.Remove(roleInfo.FactionId);

                // Choose a random incorrect faction
                revealedFaction = possibleFaction[UnityEngine.Random.RandomRangeInt(0, possibleFaction.Count)];
            }

            // Save the revealed faction
            Oracle.RevealedFaction = revealedFaction;

            var results = Oracle.GetInfo(Oracle.Confessor);
            FastDestroyableSingleton<HudManager>.Instance.Chat.AddChat(Oracle.Player, $"{results}");

            // Send RPC to notify clients
            Utils.StartRPC(CustomRPC.Confess, Oracle.Confessor.PlayerId, (int)revealedFaction);

            // Ghost Info
            Utils.StartRPC(CustomRPC.ShareGhostInfo, Oracle.Confessor.PlayerId, (byte)GhostInfoTypes.OracleInfo, results);
        }

        public static void GrenadierFlash() 
        {
            if (Grenadier.Player == null) return;

            foreach (PlayerControl player in PlayerControl.AllPlayerControls)
            {
                if (player == null || player.Data == null) continue;
                Grenadier.ClosestPlayers = Utils.GetClosestPlayers(Grenadier.Player.GetTruePosition(), Grenadier.GrenadeRadius);
                Grenadier.FlashedPlayers = Grenadier.ClosestPlayers;

                bool isExempt = player.Data.Role.IsImpostor || player == Spy.Player || player.Data.IsDead;

                if (player == PlayerControl.LocalPlayer)
                {
                    SoundEffectsManager.Play("grenadierGrenade");
                    var hud = HudManager.Instance;
                    if (hud?.FullScreen != null && hud != null)
                    {
                        Color targetColor = Grenadier.FlashedPlayers.Contains(player) && !isExempt
                            ? new Color(0.6f, 0.6f, 0.6f, 1f)
                            : new Color(0.6f, 0.6f, 0.6f, 0.2f);

                        hud.StartCoroutine(Effects.Lerp(Grenadier.GrenadeDuration, (Action<float>)(p =>
                        {
                            hud.FullScreen.color = Color.Lerp(hud.FullScreen.color, targetColor, p);
                        })));
                    }

                        hud.FullScreen.enabled = true;
                        hud.FullScreen.gameObject.SetActive(true);
                        hud.StartCoroutine(Effects.Lerp(Grenadier.GrenadeDuration, (Action<float>)(p =>
                        {
                            if (p == 1f && hud.FullScreen != null)
                            {
                                hud.FullScreen.enabled = false;
                                hud.FullScreen.gameObject.SetActive(false);
                                Grenadier.FlashedPlayers.Clear();
                            }
                        })));
                }
            }
            if (Grenadier.GrenadeDuration > 0.5f)
            {
                try
                {
                    if (PlayerControl.LocalPlayer.Data.Role.IsImpostor && MapBehaviour.Instance.infectedOverlay.sabSystem.Timer < 0.5f)
                    {
                        MapBehaviour.Instance.infectedOverlay.sabSystem.Timer = 0.5f;
                    }
                }
                catch { }
            }
        }
        public static void ShieldedMurderAttempt() 
        {
            if (Medic.Shielded == null || Medic.Player == null) return;
            
            bool isShieldedAndShow = Medic.Shielded == PlayerControl.LocalPlayer && Medic.showAttemptToShielded;
            isShieldedAndShow = isShieldedAndShow && (Medic.meetingAfterShielding || !Medic.showShieldAfterMeeting);  // Dont show attempt, if Shield is not shown yet
            bool isMedicAndShow = Medic.Player == PlayerControl.LocalPlayer && Medic.showAttemptToMedic;

            if (isShieldedAndShow || isMedicAndShow || Utils.ShouldShowGhostInfo()) Utils.ShowFlash(Palette.ImpostorRed, Duration: 0.5f, "Failed Murder Attempt on Shielded Player");
        }

        public static void FortifiedMurderAttempt() 
        {
            if (Crusader.FortifiedPlayer == null || Crusader.Player == null) return;

            if (Crusader.FortifiedPlayer == PlayerControl.LocalPlayer || Crusader.Player == PlayerControl.LocalPlayer || Utils.ShouldShowGhostInfo()) Utils.ShowFlash(Palette.ImpostorRed, Duration: 0.5f, "Murder Attempt on Fortified Player");
        }

        public static void SwapperSwap(byte playerId1, byte playerId2) 
        {
            if (MeetingHud.Instance) 
            {
                Swapper.playerId1 = playerId1;
                Swapper.playerId2 = playerId2;
            }
        }

        public static void MorphlingMorph(byte playerId) 
        {
            PlayerControl target = Utils.PlayerById(playerId);
            if (Morphling.Player == null || target == null) return;

            Morphling.morphTimer = Morphling.Duration;
            Morphling.morphTarget = target;
            if (Camouflager.CamouflageTimer <= 0f)
                Morphling.Player.SetLook(target.Data.PlayerName, target.Data.DefaultOutfit.ColorId, target.Data.DefaultOutfit.HatId, target.Data.DefaultOutfit.VisorId, target.Data.DefaultOutfit.SkinId, target.Data.DefaultOutfit.PetId);
        }

        public static void GlitchMimic(byte playerId) 
        {
            PlayerControl target = Utils.PlayerById(playerId);
            if (Glitch.Player == null || target == null) return;

            Glitch.MimicTimer = Glitch.MimicDuration;
            Glitch.MimicTarget = target;
            if (Camouflager.CamouflageTimer <= 0f)
                Glitch.Player.SetLook(target.Data.PlayerName, target.Data.DefaultOutfit.ColorId, target.Data.DefaultOutfit.HatId, target.Data.DefaultOutfit.VisorId, target.Data.DefaultOutfit.SkinId, target.Data.DefaultOutfit.PetId);
        }

        public static void HitmanMorph(byte playerId) 
        {
            PlayerControl target = Utils.PlayerById(playerId);
            if (Hitman.Player == null || target == null) return;

            Hitman.MorphTimer = Hitman.MorphDuration;
            Hitman.MorphTarget = target;
            if (Camouflager.CamouflageTimer <= 0f)
                Hitman.Player.SetLook(target.Data.PlayerName, target.Data.DefaultOutfit.ColorId, target.Data.DefaultOutfit.HatId, target.Data.DefaultOutfit.VisorId, target.Data.DefaultOutfit.SkinId, target.Data.DefaultOutfit.PetId);
        }

        public static void CamouflagerCamouflage() 
        {
            if (Camouflager.Player == null) return;

            Camouflager.CamouflageTimer = Camouflager.Duration;
            if (Utils.MushroomSabotageActive()) return; // Dont overwrite the fungle "camo"
            foreach (PlayerControl player in PlayerControl.AllPlayerControls)
                player.SetLook("", 6, "", "", "", "");
        }

        public static void PoisonerSetPoisoned(byte targetId, byte performReset) 
        {
            if (performReset != 0) 
            {
                Poisoner.poisoned = null;
                return;
            }

            if (Poisoner.Player == null) return;
            foreach (PlayerControl player in PlayerControl.AllPlayerControls) 
            {
                if (player.PlayerId == targetId && !player.Data.IsDead) 
                {
                        Poisoner.poisoned = player;
                }
            }
        }

        public static void TrackerUsedTracker(byte targetId) 
        {
            Tracker.usedTracker = true;
            foreach (PlayerControl player in PlayerControl.AllPlayerControls)
                if (player.PlayerId == targetId)
                    Tracker.tracked = player;
        }

        public static void BlackmailerBlackmail(byte targetId)
        {
            PlayerControl player = Utils.PlayerById(targetId);
            Blackmailer.BlackmailedPlayer = player;
        }

        public static void GlitchUsedHacks(byte targetId)
        {
            Glitch.remainingHacks--;
            Glitch.HackedPlayers.Add(targetId);
        }

        public static void JackalCreatesSidekick(byte targetId) 
        {
            PlayerControl player = Utils.PlayerById(targetId);
            if (player == null) return;
            
            bool wasSpy = Spy.Player != null && player == Spy.Player;
            bool wasImpostor = player.Data.Role.IsImpostor;  // This can only be reached if impostors can be sidekicked, else they would die anyways.
            FastDestroyableSingleton<RoleManager>.Instance.SetRole(player, RoleTypes.Crewmate);
            if (player == Lawyer.Player && Lawyer.target != null)
            {
                Transform playerInfoTransform = Lawyer.target.cosmetics.nameText.transform.parent.FindChild("Info");
                TMPro.TextMeshPro playerInfo = playerInfoTransform != null ? playerInfoTransform.GetComponent<TMPro.TextMeshPro>() : null;
                if (playerInfo != null) playerInfo.text = "";
            }
            if (player == Prosecutor.Player && Prosecutor.target != null)
            {
                Transform playerInfoTransform = Prosecutor.target.cosmetics.nameText.transform.parent.FindChild("Info");
                TMPro.TextMeshPro playerInfo = playerInfoTransform != null ? playerInfoTransform.GetComponent<TMPro.TextMeshPro>() : null;
                if (playerInfo != null) playerInfo.text = "";
            }

            ErasePlayerRoles(player.PlayerId);

            Sidekick.Player = player;

            // Save Sidekick after it's created
            RoleInfo.AddToRoleHistory(player.PlayerId, RoleInfo.sidekick);

            if (player.PlayerId == PlayerControl.LocalPlayer.PlayerId) PlayerControl.LocalPlayer.moveable = true;
            if (wasSpy || wasImpostor) Sidekick.wasTeamRed = true;
            Sidekick.wasSpy = wasSpy;
            Sidekick.wasImpostor = wasImpostor;
            if (player == PlayerControl.LocalPlayer) SoundEffectsManager.Play("jackalSidekick");
            if (CustomOptionHolder.SidekickIsAlwaysGuesser.GetBool() && !Guesser.IsGuesser(targetId)) SetGuesser(targetId);
            Jackal.canCreateSidekick = false;
        }

        public static void SidekickPromotes() 
        {
            Jackal.RemoveCurrentJackal();
            Jackal.Player = Sidekick.Player;
            Jackal.canCreateSidekick = Jackal.jackalPromotedFromSidekickCanCreateSidekick;
            Jackal.wasTeamRed = Sidekick.wasTeamRed;
            Jackal.wasSpy = Sidekick.wasSpy;
            Jackal.wasImpostor = Sidekick.wasImpostor;
            Sidekick.ClearAndReload();
            return;
        }
        
        public static void ErasePlayerRoles(byte playerId) 
        {
            PlayerControl player = Utils.PlayerById(playerId);
            if (player == null || !player.CanBeErased()) return;

            // Crewmate roles
            if (player == Mayor.Player) Mayor.ClearAndReload();
            if (player == Portalmaker.Player) Portalmaker.ClearAndReload();
            if (player == Engineer.Player) Engineer.ClearAndReload();
            if (player == Sheriff.Player) Sheriff.ClearAndReload();
            if (player == Oracle.Player) Oracle.ClearAndReload();
            if (player == Lighter.Player) Lighter.ClearAndReload();
            if (player == Detective.Player) Detective.ClearAndReload();
            if (player == TimeMaster.Player) TimeMaster.ClearAndReload();
            if (player == Veteran.Player) Veteran.ClearAndReload();
            if (player == Medic.Player) Medic.ClearAndReload();
            if (player == Mystic.Player) Mystic.ClearAndReload();
            if (player == Hacker.Player) Hacker.ClearAndReload();
            if (player == Tracker.Player) Tracker.ClearAndReload();
            if (player == Swapper.Player) Swapper.ClearAndReload();
            if (player == Spy.Player) Spy.ClearAndReload();
            if (player == Crusader.Player) Crusader.ClearAndReload();
            if (player == Vigilante.Player) Vigilante.ClearAndReload();
            if (player == Medium.medium) Medium.ClearAndReload();
            if (player == Trapper.Player) Trapper.ClearAndReload();

            // Impostor roles
            if (player == Morphling.Player) Morphling.ClearAndReload();
            if (player == Camouflager.Player) Camouflager.ClearAndReload();
            if (player == Poisoner.Player) Poisoner.ClearAndReload();
            if (player == Eraser.Player) Eraser.ClearAndReload();
            if (player == Trickster.Player) Trickster.ClearAndReload();
            if (player == Undertaker.Player) Undertaker.ClearAndReload();
            if (player == Cleaner.Player) Cleaner.ClearAndReload();
            if (player == Blackmailer.Player) Blackmailer.ClearAndReload();
            if (player == Warlock.Player) Warlock.ClearAndReload();
            if (player == Witch.Player) Witch.ClearAndReload();
            if (player == Miner.Player) Miner.ClearAndReload();
            if (player == Ninja.Player) Ninja.ClearAndReload();
            if (player == BountyHunter.Player) BountyHunter.ClearAndReload();
            if (player == Wraith.Player) Wraith.ClearAndReload();
            if (player == Grenadier.Player) Grenadier.ClearAndReload();
            if (player == Yoyo.Player) Yoyo.ClearAndReload();

            // Guessers
            if (Guesser.IsGuesser(player.PlayerId)) Guesser.Clear(player.PlayerId);

            // Neutral Killing roles
            if (player == Glitch.Player) Glitch.ClearAndReload();
            if (player == Werewolf.Player) Werewolf.ClearAndReload();
            if (player == Hitman.Player) Hitman.ClearAndReload();
            if (player == Agent.Player) Agent.ClearAndReload();
            if (player == Plaguebearer.Player) Plaguebearer.ClearAndReload();
            if (player == Pestilence.Player) Pestilence.ClearAndReload();
            if (player == VengefulRomantic.Player) VengefulRomantic.ClearAndReload();
            if (player == Juggernaut.Player) Juggernaut.ClearAndReload();
            if (player == Predator.Player) Predator.ClearAndReload();
            if (player == Sidekick.Player) Sidekick.ClearAndReload();

            if (player == Jackal.Player) 
            { // Promote Sidekick and hence override the the Jackal or erase Jackal
                if (Sidekick.promotesToJackal && Sidekick.Player != null && !Sidekick.Player.Data.IsDead) 
                {
                    SidekickPromotes();
                }
                else 
                {
                    Jackal.ClearAndReload();
                }
            }
            
            // Passive Neutral Roles
            if (player == Jester.Player) Jester.ClearAndReload();
            if (player == Vulture.Player) Vulture.ClearAndReload();
            if (player == Lawyer.Player) Lawyer.ClearAndReload();
            if (player == Arsonist.Player) Arsonist.ClearAndReload();
            if (player == Amnesiac.Player) Amnesiac.ClearAndReload();
            if (player == Romantic.Player) Romantic.ClearAndReload();
            if (player == Pursuer.Player) Pursuer.ClearAndReload();
        }

        public static void SetFutureErased(byte playerId) 
        {
            PlayerControl player = Utils.PlayerById(playerId);
            if (Eraser.futureErased == null) 
                Eraser.futureErased = new List<PlayerControl>();
            if (player != null) 
            {
                Eraser.futureErased.Add(player);
            }
        }

        public static void SetFutureShielded(byte playerId) 
        {
            Medic.futureShielded = Utils.PlayerById(playerId);
            Medic.usedShield = true;
        }

        public static void SetFutureSpelled(byte playerId) 
        {
            PlayerControl player = Utils.PlayerById(playerId);
            if (Witch.futureSpelled == null)
                Witch.futureSpelled = new List<PlayerControl>();
            if (player != null) 
            {
                Witch.futureSpelled.Add(player);
            }
        }

        public static void RemoveBlackmail() 
        {
            Blackmailer.BlackmailedPlayer = null;
            BlackmailMeetingUpdate.shookAlready = false;
        }

        public static void PlaceNinjaTrace(byte[] buff) 
        {
            Vector3 position = Vector3.zero;
            position.x = BitConverter.ToSingle(buff, 0 * sizeof(float));
            position.y = BitConverter.ToSingle(buff, 1 * sizeof(float));
            new NinjaTrace(position, Ninja.traceTime);
            if (PlayerControl.LocalPlayer != Ninja.Player)
                Ninja.ninjaMarked = null;
        }

        public static void SetVanish(byte playerId, byte flag)
        {
            PlayerControl target = Utils.PlayerById(playerId);
            if (target == null) return;
            if (flag == byte.MaxValue)
            {
                target.cosmetics.currentBodySprite.BodySprite.color = Color.white;
                target.cosmetics.colorBlindText.gameObject.SetActive(DataManager.Settings.Accessibility.ColorBlindMode);
                target.cosmetics.colorBlindText.color = target.cosmetics.colorBlindText.color.SetAlpha(1f);

                if (Camouflager.CamouflageTimer <= 0 && !Utils.MushroomSabotageActive()) target.SetDefaultLook();
                Wraith.IsVanished = false;
                return;
            }

            target.SetLook("", 6, "", "", "", "");
            Color color = Color.clear;
            bool canSee = PlayerControl.LocalPlayer.Data.Role.IsImpostor || PlayerControl.LocalPlayer.Data.IsDead;
            if (canSee) color.a = 0.1f;
            target.cosmetics.currentBodySprite.BodySprite.color = color;
            target.cosmetics.colorBlindText.gameObject.SetActive(false);
            target.cosmetics.colorBlindText.color = target.cosmetics.colorBlindText.color.SetAlpha(canSee ? 0.1f : 0f);
            Wraith.VanishTimer = Wraith.Duration;
            Wraith.IsVanished = true;
        }

        public static void SetInvisible(byte playerId, byte flag)
        {
            PlayerControl target = Utils.PlayerById(playerId);
            if (target == null) return;
            if (flag == byte.MaxValue)
            {
                target.cosmetics.currentBodySprite.BodySprite.color = Color.white;
                target.cosmetics.colorBlindText.gameObject.SetActive(DataManager.Settings.Accessibility.ColorBlindMode);
                target.cosmetics.colorBlindText.color = target.cosmetics.colorBlindText.color.SetAlpha(1f);

                if (Camouflager.CamouflageTimer <= 0 && !Utils.MushroomSabotageActive()) target.SetDefaultLook();
                Ninja.isInvisble = false;
                return;
            }

            target.SetLook("", 6, "", "", "", "");
            Color color = Color.clear;
            bool canSee = PlayerControl.LocalPlayer.Data.Role.IsImpostor || PlayerControl.LocalPlayer.Data.IsDead;
            if (canSee) color.a = 0.1f;
            target.cosmetics.currentBodySprite.BodySprite.color = color;
            target.cosmetics.colorBlindText.gameObject.SetActive(false);
            target.cosmetics.colorBlindText.color = target.cosmetics.colorBlindText.color.SetAlpha(canSee ? 0.1f : 0f);
            Ninja.invisibleTimer = Ninja.invisibleDuration;
            Ninja.isInvisble = true;
        }

        public static void PlacePortal(byte[] buff) 
        {
            Vector3 position = Vector2.zero;
            position.x = BitConverter.ToSingle(buff, 0 * sizeof(float));
            position.y = BitConverter.ToSingle(buff, 1 * sizeof(float));
            new Portal(position);
        }

        public static void PlaceMine(byte[] buff) 
        {
            Vector3 position = Vector2.zero;
            position.x = BitConverter.ToSingle(buff, 0 * sizeof(float));
            position.y = BitConverter.ToSingle(buff, 1 * sizeof(float));
            new MinerVent(position);
        }

        public static void UsePortal(byte playerId, byte exit) 
        {
            Portal.StartTeleport(playerId, exit);
        }

        public static void PlaceJackInTheBox(byte[] buff) 
        {
            Vector3 position = Vector3.zero;
            position.x = BitConverter.ToSingle(buff, 0*sizeof(float));
            position.y = BitConverter.ToSingle(buff, 1*sizeof(float));
            new JackInTheBox(position);
        }

        public static void LightsOut() 
        {
            Trickster.lightsOutTimer = Trickster.lightsOutDuration;
            // If the local player is impostor indicate lights out
            if(Utils.HasImpVision(GameData.Instance.GetPlayerById(PlayerControl.LocalPlayer.PlayerId))) {
                _ = new CustomMessage("Lights are out", Trickster.lightsOutDuration);
            }
        }

        public static void PlaceCamera(byte[] buff) 
        {
            var referenceCamera = UnityEngine.Object.FindObjectOfType<SurvCamera>(); 
            if (referenceCamera == null) return; // Mira HQ

            Vigilante.remainingScrews -= Vigilante.camPrice;
            Vigilante.placedCameras++;

            Vector3 position = Vector3.zero;
            position.x = BitConverter.ToSingle(buff, 0*sizeof(float));
            position.y = BitConverter.ToSingle(buff, 1*sizeof(float));

            var camera = UnityEngine.Object.Instantiate<SurvCamera>(referenceCamera);
            camera.transform.position = new Vector3(position.x, position.y, referenceCamera.transform.position.z - 1f);
            camera.CamName = $"Security Camera {Vigilante.placedCameras}";
            camera.Offset = new Vector3(0f, 0f, camera.Offset.z);
            if (GameOptionsManager.Instance.currentNormalGameOptions.MapId == 2 || GameOptionsManager.Instance.currentNormalGameOptions.MapId == 4) camera.transform.localRotation = new Quaternion(0, 0, 1, 1); // Polus and Airship 

            if (SubmergedCompatibility.IsSubmerged) 
            {
                // remove 2d box collider of console, so that no barrier can be created. (irrelevant for now, but who knows... maybe we need it later)
                var fixConsole = camera.transform.FindChild("FixConsole");
                if (fixConsole != null) {
                    var boxCollider = fixConsole.GetComponent<BoxCollider2D>();
                    if (boxCollider != null) UnityEngine.Object.Destroy(boxCollider);
                }
            }


            if (PlayerControl.LocalPlayer == Vigilante.Player) 
            {
                camera.gameObject.SetActive(true);
                camera.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
            } else {
                camera.gameObject.SetActive(false);
            }
            MapOptions.CamsToAdd.Add(camera);
        }

        public static void SealVent(int ventId) 
        {
            Vent vent = MapUtilities.CachedShipStatus.AllVents.FirstOrDefault((x) => x != null && x.Id == ventId);
            if (vent == null) return;

            Vigilante.remainingScrews -= Vigilante.ventPrice;
            if (PlayerControl.LocalPlayer == Vigilante.Player) 
            {
                PowerTools.SpriteAnim animator = vent.GetComponent<PowerTools.SpriteAnim>(); 
                
                vent.EnterVentAnim = vent.ExitVentAnim = null;
                Sprite newSprite = animator == null ? Vigilante.GetStaticVentSealedSprite() : Vigilante.GetAnimatedVentSealedSprite();
                SpriteRenderer rend = vent.myRend;
                if (Utils.IsFungle()) {
                    newSprite = Vigilante.GetFungleVentSealedSprite();
                    rend = vent.transform.GetChild(3).GetComponent<SpriteRenderer>();
                    animator = vent.transform.GetChild(3).GetComponent<PowerTools.SpriteAnim>();
                }
                animator?.Stop();
                rend.sprite = newSprite;
                if (SubmergedCompatibility.IsSubmerged && vent.Id == 0) vent.myRend.sprite = Vigilante.GetSubmergedCentralUpperSealedSprite();
                if (SubmergedCompatibility.IsSubmerged && vent.Id == 14) vent.myRend.sprite = Vigilante.GetSubmergedCentralLowerSealedSprite();
                rend.color = new Color(1f, 1f, 1f, 0.5f);
                vent.name = "FutureSealedVent_" + vent.name;
            }

            MapOptions.VentsToSeal.Add(vent);
        }

        public static void ArsonistWin() 
        {
            Arsonist.IsArsonistWin = true;
            foreach (PlayerControl p in PlayerControl.AllPlayerControls) 
            {
                if (p != Arsonist.Player && !p.Data.IsDead) 
                {
                    p.Exiled();
                    GameHistory.CreateDeathReason(p, DeadPlayer.CustomDeathReason.Arson, Arsonist.Player);
                }
            }
        }

        public static void LawyerSetTarget(byte playerId) 
        {
            Lawyer.target = Utils.PlayerById(playerId);
        }

        public static void LawyerChangeRole() 
        {
            PlayerControl player = Lawyer.Player;
            PlayerControl client = Lawyer.target;
            Lawyer.ClearAndReload(false);

            Pursuer.Player = player;
            var newRole = RoleInfo.GetRoleInfoForPlayer(player).FirstOrDefault();
            if (newRole != null && newRole != RoleInfo.pursuer) RoleInfo.AddToRoleHistory(player.PlayerId, newRole);

            if (player.PlayerId == PlayerControl.LocalPlayer.PlayerId && client != null) 
            {
                    Transform playerInfoTransform = client.cosmetics.nameText.transform.parent.FindChild("Info");
                    TMPro.TextMeshPro playerInfo = playerInfoTransform != null ? playerInfoTransform.GetComponent<TMPro.TextMeshPro>() : null;
                    if (playerInfo != null) playerInfo.text = "";
            }
        }
        public static void ProsecutorSetTarget(byte playerId) 
        {
            Prosecutor.target = Utils.PlayerById(playerId);
        }

        public static void ProsecutorChangeRole() 
        {
            PlayerControl player = Prosecutor.Player;
            PlayerControl client = Prosecutor.target;
            Prosecutor.ClearAndReload(false);
            
            if (Prosecutor.BecomeEnum == 0)
            {
                Jester.Player = player;
                var newRole = RoleInfo.GetRoleInfoForPlayer(player).FirstOrDefault();
                if (newRole != null && newRole != RoleInfo.jester) RoleInfo.AddToRoleHistory(player.PlayerId, newRole);
            }
            else if (Prosecutor.BecomeEnum == 1)
            {
                Amnesiac.Player = player;
                var newRole = RoleInfo.GetRoleInfoForPlayer(player).FirstOrDefault();
                if (newRole != null && newRole != RoleInfo.amnesiac) RoleInfo.AddToRoleHistory(player.PlayerId, newRole);
            }
            else
            {
                Pursuer.Player = player;
                var newRole = RoleInfo.GetRoleInfoForPlayer(player).FirstOrDefault();
                if (newRole != null && newRole != RoleInfo.pursuer) RoleInfo.AddToRoleHistory(player.PlayerId, newRole);
            }

            if (player.PlayerId == PlayerControl.LocalPlayer.PlayerId && client != null) 
            {
                    Transform playerInfoTransform = client.cosmetics.nameText.transform.parent.FindChild("Info");
                    TMPro.TextMeshPro playerInfo = playerInfoTransform != null ? playerInfoTransform.GetComponent<TMPro.TextMeshPro>() : null;
                    if (playerInfo != null) playerInfo.text = "";
            }
        }

        public static void PlaguebearerTurnPestilence() 
        {
            PlayerControl player = Plaguebearer.Player;
            Plaguebearer.ClearAndReload();

            Pestilence.Player = player;
            
            var newRole = RoleInfo.GetRoleInfoForPlayer(player).FirstOrDefault();
            if (newRole != null && newRole != RoleInfo.plaguebearer) RoleInfo.AddToRoleHistory(player.PlayerId, newRole);
            
            if (player == PlayerControl.LocalPlayer)
            {
                Utils.ShowFlash(Pestilence.Color, 2.5f);
                SoundManager.Instance.PlaySound(ShipStatus.Instance.SabotageSound, false, 1f, null);
                Utils.ShowTextToast("You just transformed into the Pestilence!", 2.5f);
            }
        }

        public static void AgentTurnIntoHitman() 
        {
            PlayerControl player = Agent.Player;
            Agent.ClearAndReload();

            Hitman.Player = player;
            
            var newRole = RoleInfo.GetRoleInfoForPlayer(player).FirstOrDefault();
            if (newRole != null && newRole != RoleInfo.agent) RoleInfo.AddToRoleHistory(player.PlayerId, newRole);

            if (player == PlayerControl.LocalPlayer)
            {
                Utils.ShowFlash(Hitman.Color, 2.5f);
                SoundManager.Instance.PlaySound(ShipStatus.Instance.SabotageSound, false, 1f, null);
                Utils.ShowTextToast("You just became the Hitman!", 2.5f);
            }
        }

        public static void DragBody(byte BodyId)
        {
            DeadBody[] array = UnityEngine.Object.FindObjectsOfType<DeadBody>();
            for (int i = 0; i < array.Length; i++)
            {
                if (GameData.Instance.GetPlayerById(array[i].ParentId).PlayerId == BodyId)
                {
                    Undertaker.CurrentTarget = array[i];
                }
            }
        }

        public static void DropBody(byte bodyId)
        {
            if (Undertaker.Player == null || Undertaker.CurrentTarget == null) return;
            Undertaker.CurrentTarget = null;
            Undertaker.CurrentTarget.transform.position = new Vector3(Undertaker.Player.GetTruePosition().x, Undertaker.Player.GetTruePosition().y, Undertaker.Player.transform.position.z);
        }

        public static void HitmanDragBody(byte BodyId)
        {
            DeadBody[] array = UnityEngine.Object.FindObjectsOfType<DeadBody>();
            for (int i = 0; i < array.Length; i++)
            {
                if (GameData.Instance.GetPlayerById(array[i].ParentId).PlayerId == BodyId)
                {
                    Hitman.BodyTarget = array[i];
                }
            }
        }

        public static void HitmanDropBody(byte bodyId)
        {
            if (Hitman.Player == null || Hitman.BodyTarget == null) return;
            Hitman.BodyTarget = null;
            Hitman.BodyTarget.transform.position = new Vector3(Hitman.Player.GetTruePosition().x, Hitman.Player.GetTruePosition().y, Hitman.Player.transform.position.z);
        }


        public static void RomanticChangeRole() 
        {
            PlayerControl player = Romantic.Player;
            PlayerControl target = Romantic.beloved;
            Romantic.ClearAndReload(false);

            VengefulRomantic.Player = player;
            var newRole = RoleInfo.GetRoleInfoForPlayer(player).FirstOrDefault();
            if (newRole != null && newRole != RoleInfo.romantic) RoleInfo.AddToRoleHistory(player.PlayerId, newRole);
            VengefulRomantic.Lover = target;

            if (player.PlayerId == PlayerControl.LocalPlayer.PlayerId && target != null) 
            {
                    Transform playerInfoTransform = target.cosmetics.nameText.transform.parent.FindChild("Info");
                    TMPro.TextMeshPro playerInfo = playerInfoTransform != null ? playerInfoTransform.GetComponent<TMPro.TextMeshPro>() : null;
                    if (playerInfo != null) playerInfo.text = "";
            }
        }
        public static void Guesserhoot(byte killerId, byte dyingTargetId, byte guessedTargetId, byte guessedRoleId) 
        {
            PlayerControl dyingTarget = Utils.PlayerById(dyingTargetId);
            if (dyingTarget == null ) return;
            if (Lawyer.target != null && dyingTarget == Lawyer.target) Lawyer.targetWasGuessed = true;  // Lawyer shouldn't be exiled with the client for guesses
            PlayerControl dyingLoverPartner = Lovers.bothDie ? dyingTarget.GetPartner() : null; // Lover check
            if (Lawyer.target != null && dyingLoverPartner == Lawyer.target) Lawyer.targetWasGuessed = true;  // Lawyer shouldn't be exiled with the client for guesses

            PlayerControl guesser = Utils.PlayerById(killerId);
            dyingTarget.Exiled();
            GameHistory.CreateDeathReason(dyingTarget, DeadPlayer.CustomDeathReason.Guess, guesser);
            byte partnerId = dyingLoverPartner != null ? dyingLoverPartner.PlayerId : dyingTargetId;

            Guesser.RemainingShots(killerId, true);
            if (Constants.ShouldPlaySfx()) SoundManager.Instance.PlaySound(dyingTarget.KillSfx, false, 0.8f);
            if (MeetingHud.Instance) 
            {
                PlayerVoteArea voteArea = MeetingHud.Instance.playerStates.First(x => x.TargetPlayerId == dyingTarget.PlayerId);
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
                foreach (PlayerVoteArea pva in MeetingHud.Instance.playerStates) 
                {
                    if (pva.TargetPlayerId == dyingTargetId || pva.TargetPlayerId == partnerId) 
                    {
                        pva.SetDead(pva.DidReport, true);
                        pva.Overlay.gameObject.SetActive(true);
                        MeetingHudPatch.SwapperCheckAndReturnSwap(MeetingHud.Instance, pva.TargetPlayerId);
                    }

                    //Give players back their vote if target is shot dead
                    if (pva.VotedFor != dyingTargetId && pva.VotedFor != partnerId) continue;
                    pva.UnsetVote();
                    var voteAreaPlayer = Utils.PlayerById(pva.TargetPlayerId);
                    if (!voteAreaPlayer.AmOwner) continue;
                    MeetingHud.Instance.ClearVote();

                }

                if (AmongUsClient.Instance.AmHost) 
                    MeetingHud.Instance.CheckForEndVoting();
            }
            if (FastDestroyableSingleton<HudManager>.Instance != null && guesser != null)
                if (PlayerControl.LocalPlayer == dyingTarget) 
                {
                    FastDestroyableSingleton<HudManager>.Instance.KillOverlay.ShowKillAnimation(guesser.Data, dyingTarget.Data);
                    if (MeetingHudPatch.GuesserUI != null) MeetingHudPatch.GuesserUIExitButton.OnClick.Invoke();
                } 
                else if (dyingLoverPartner != null && PlayerControl.LocalPlayer == dyingLoverPartner) 
                {
                    FastDestroyableSingleton<HudManager>.Instance.KillOverlay.ShowKillAnimation(dyingLoverPartner.Data, dyingLoverPartner.Data);
                    if (MeetingHudPatch.GuesserUI != null) MeetingHudPatch.GuesserUIExitButton.OnClick.Invoke();
                }

            // remove shoot button from targets for all Guesser and close their GuesserUI
            if (Guesser.IsGuesser(PlayerControl.LocalPlayer.PlayerId) && PlayerControl.LocalPlayer != guesser && !PlayerControl.LocalPlayer.Data.IsDead && Guesser.RemainingShots(PlayerControl.LocalPlayer.PlayerId) > 0 && MeetingHud.Instance)
            {
                MeetingHud.Instance.playerStates.ToList().ForEach(x => { if (x.TargetPlayerId == dyingTarget.PlayerId && x.transform.FindChild("ShootButton") != null) UnityEngine.Object.Destroy(x.transform.FindChild("ShootButton").gameObject); });
                if (dyingLoverPartner != null)
                    MeetingHud.Instance.playerStates.ToList().ForEach(x => { if (x.TargetPlayerId == dyingLoverPartner.PlayerId && x.transform.FindChild("ShootButton") != null) UnityEngine.Object.Destroy(x.transform.FindChild("ShootButton").gameObject); });

                if (MeetingHudPatch.GuesserUI != null && MeetingHudPatch.GuesserUIExitButton != null) 
                {
                    if (MeetingHudPatch.guesserCurrentTarget == dyingTarget.PlayerId)
                        MeetingHudPatch.GuesserUIExitButton.OnClick.Invoke();
                    else if (dyingLoverPartner != null && MeetingHudPatch.guesserCurrentTarget == dyingLoverPartner.PlayerId)
                        MeetingHudPatch.GuesserUIExitButton.OnClick.Invoke();
                }
            }

            PlayerControl guessedTarget = Utils.PlayerById(guessedTargetId);
            if (PlayerControl.LocalPlayer.Data.IsDead && guessedTarget != null && guesser != null) 
            {
                RoleInfo roleInfo = RoleInfo.allRoleInfos.FirstOrDefault(x => (byte)x.RoleId == guessedRoleId);
                string msg = $"{guesser.Data.PlayerName} guessed the role {roleInfo?.Name ?? ""} for {guessedTarget.Data.PlayerName}!";
                if (AmongUsClient.Instance.AmClient && FastDestroyableSingleton<HudManager>.Instance)
                    FastDestroyableSingleton<HudManager>.Instance.Chat.AddChat(guesser, msg);
                if (msg.IndexOf("who", StringComparison.OrdinalIgnoreCase) >= 0)
                    FastDestroyableSingleton<UnityTelemetry>.Instance.SendWho();
            }
        }

        public static void SetBlanked(byte playerId, byte value) 
        {
            PlayerControl target = Utils.PlayerById(playerId);
            if (target == null) return;
            Pursuer.blankedList.RemoveAll(x => x.PlayerId == playerId);
            if (value > 0) Pursuer.blankedList.Add(target);            
        }
        public static void SetFirstKill(byte playerId) 
        {
            PlayerControl target = Utils.PlayerById(playerId);
            if (target == null) return;
            MapOptions.FirstPlayerKilled = target;
        }

        public static void SetTiebreak() 
        {
            Tiebreaker.isTiebreak = true;
        }
        public static void AmnesiacRemember(byte targetId)
        {
            Amnesiac.Remembered = true;

            PlayerControl target = Utils.PlayerById(targetId);
            PlayerControl AmnesiacPlayer = Amnesiac.Player;
            if (target == null || AmnesiacPlayer == null) return;
            List<RoleInfo> targetInfo = RoleInfo.GetRoleInfoForPlayer(target);
            RoleInfo roleInfo = targetInfo.Where(info => info.FactionId != Faction.Other).FirstOrDefault();

            switch (roleInfo.RoleId)
            {
                case RoleId.Crewmate:
                    Amnesiac.ClearAndReload();
                    Amnesiac.Player = target;
                    break;
                case RoleId.Impostor:
                    Utils.BecomeImpostor(Amnesiac.Player);
                    Amnesiac.ClearAndReload();
                    break;
                case RoleId.Jester:
                    Jester.ClearAndReload();
                    Jester.Player = AmnesiacPlayer;
                    Amnesiac.ClearAndReload();
                    Amnesiac.Player = target;
                    break;

                case RoleId.Werewolf:
                    Werewolf.ClearAndReload();
                    Werewolf.Player = AmnesiacPlayer;
                    Amnesiac.ClearAndReload();
                    Amnesiac.Player = target;
                    break;
                
                case RoleId.Miner:
                    Utils.BecomeImpostor(Amnesiac.Player);
                    Miner.ClearAndReload();
                    Miner.Player = AmnesiacPlayer;
                    Amnesiac.ClearAndReload();
                    Amnesiac.Player = target;
                    break;

                case RoleId.Prosecutor:
                    Prosecutor.Player = AmnesiacPlayer;
                    Amnesiac.ClearAndReload();
                    Amnesiac.Player = target;
                    break;

                case RoleId.Mayor:
                    Mayor.ClearAndReload();
                    Mayor.Player = AmnesiacPlayer;
                    Amnesiac.ClearAndReload();
                    Amnesiac.Player = target;
                    break;

                case RoleId.Portalmaker:
                    Portalmaker.ClearAndReload();
                    Portalmaker.Player = AmnesiacPlayer;
                    Amnesiac.ClearAndReload();
                    Amnesiac.Player = target;
                    break;
                
                case RoleId.Crusader:
                    Crusader.ClearAndReload();
                    Crusader.Player = AmnesiacPlayer;
                    Amnesiac.ClearAndReload();
                    Amnesiac.Player = target;
                    break;

                case RoleId.Engineer:
                    Engineer.ClearAndReload();
                    Engineer.Player = AmnesiacPlayer;
                    Amnesiac.ClearAndReload();
                    Amnesiac.Player = target;
                    break;

                case RoleId.Sheriff:
                    Sheriff.ClearAndReload();
                    Sheriff.Player = AmnesiacPlayer;
                    Amnesiac.ClearAndReload();
                    Amnesiac.Player = target;
                    break;

                case RoleId.Lighter:
                    Lighter.ClearAndReload();
                    Lighter.Player = AmnesiacPlayer;
                    Amnesiac.ClearAndReload();
                    Amnesiac.Player = target;
                    break;

                case RoleId.Detective:
                    Detective.ClearAndReload();
                    Detective.Player = AmnesiacPlayer;
                    Amnesiac.ClearAndReload();
                    Amnesiac.Player = target;
                    break;

                case RoleId.TimeMaster:
                    TimeMaster.ClearAndReload();
                    TimeMaster.Player = AmnesiacPlayer;
                    Amnesiac.ClearAndReload();
                    Amnesiac.Player = target;
                    break;

                case RoleId.Veteran:
                    Veteran.ClearAndReload();
                    Veteran.Player = AmnesiacPlayer;
                    Amnesiac.ClearAndReload();
                    Amnesiac.Player = target;
                    break;

                case RoleId.Medic:
                    Medic.ClearAndReload();
                    Medic.Player = AmnesiacPlayer;
                    Amnesiac.ClearAndReload();
                    Amnesiac.Player = target;
                    break;

                case RoleId.Swapper:
                    Swapper.ClearAndReload();
                    Swapper.Player = AmnesiacPlayer;
                    Amnesiac.ClearAndReload();
                    Amnesiac.Player = target;
                    break;
                
                case RoleId.Grenadier:
                    Utils.BecomeImpostor(Amnesiac.Player);
                    Grenadier.ClearAndReload();
                    Grenadier.Player = AmnesiacPlayer;
                    Amnesiac.ClearAndReload();
                    Amnesiac.Player = target;
                    break;
                
                case RoleId.Blackmailer:
                    Utils.BecomeImpostor(Amnesiac.Player);
                    Blackmailer.ClearAndReload();
                    Blackmailer.Player = AmnesiacPlayer;
                    Amnesiac.ClearAndReload();
                    Amnesiac.Player = target;
                    break;

                case RoleId.Mystic:
                    Mystic.ClearAndReload();
                    Mystic.Player = AmnesiacPlayer;
                    Amnesiac.ClearAndReload();
                    Amnesiac.Player = target;
                    break;

                case RoleId.Morphling:
                    Utils.BecomeImpostor(Amnesiac.Player);
                    Morphling.ClearAndReload();
                    Morphling.Player = AmnesiacPlayer;
                    Amnesiac.ClearAndReload();
                    Amnesiac.Player = target;
                    break;

                case RoleId.Yoyo:
                    Utils.BecomeImpostor(Amnesiac.Player);
                    Yoyo.ClearAndReload();
                    Yoyo.Player = AmnesiacPlayer;
                    Amnesiac.ClearAndReload();
                    Amnesiac.Player = target;
                    break;

                case RoleId.Camouflager:
                    Utils.BecomeImpostor(Amnesiac.Player);
                    Camouflager.ClearAndReload();
                    Camouflager.Player = AmnesiacPlayer;
                   Amnesiac.ClearAndReload();
                    Amnesiac.Player = target;
                    break;

                case RoleId.Hacker:
                    Hacker.ClearAndReload();
                    Hacker.Player = AmnesiacPlayer;
                    Amnesiac.ClearAndReload();
                    Amnesiac.Player = target;
                    break;

                case RoleId.Tracker:
                    Tracker.ClearAndReload();
                    Tracker.Player = AmnesiacPlayer;
                    Amnesiac.ClearAndReload();
                    Amnesiac.Player = target;
                    break;

                case RoleId.Poisoner:
                    Utils.BecomeImpostor(Amnesiac.Player);
                    Poisoner.ClearAndReload();
                    Poisoner.Player = AmnesiacPlayer;
                    Amnesiac.ClearAndReload();
                    Amnesiac.Player = target;
                    break;

                case RoleId.Glitch:
                    Glitch.ClearAndReload();
                    Glitch.Player = AmnesiacPlayer;
                    Amnesiac.ClearAndReload();
                    Amnesiac.Player = target;
                    break;
                
                case RoleId.Predator:
                    Predator.ClearAndReload();
                    Predator.Player = AmnesiacPlayer;
                    Amnesiac.ClearAndReload();
                    Amnesiac.Player = target;
                    break;
                
                case RoleId.Juggernaut:
                    Juggernaut.ClearAndReload();
                    Juggernaut.Player = AmnesiacPlayer;
                    Amnesiac.ClearAndReload();
                    Amnesiac.Player = target;
                    break;
                
                case RoleId.Oracle:
                    Oracle.ClearAndReload();
                    Oracle.Player = AmnesiacPlayer;
                    Amnesiac.ClearAndReload();
                    Amnesiac.Player = target;
                    break;
                
                case RoleId.Agent:
                    Agent.ClearAndReload();
                    Agent.Player = AmnesiacPlayer;
                    Amnesiac.ClearAndReload();
                    Amnesiac.Player = target;
                    break;
                
                 case RoleId.Hitman:
                    Hitman.ClearAndReload();
                    Hitman.Player = AmnesiacPlayer;
                    Amnesiac.ClearAndReload();
                    Amnesiac.Player = target;
                    break;
                
                case RoleId.Undertaker:
                    Utils.BecomeImpostor(Amnesiac.Player);
                    Undertaker.ClearAndReload();
                    Undertaker.Player = AmnesiacPlayer;
                    Amnesiac.ClearAndReload();
                    Amnesiac.Player = target;
                    break;
                
                case RoleId.Romantic:
                    Romantic.ClearAndReload();
                    Romantic.Player = AmnesiacPlayer;
                    Amnesiac.ClearAndReload();
                    Amnesiac.Player = target;
                    break;
                case RoleId.Jackal:
                    // Only demote current jackal to sidekick if its not the amnesiac and theyre alive
                    if (Jackal.Player != null && Jackal.Player != AmnesiacPlayer && !Jackal.Player.Data.IsDead)
                    {
                        RoleInfo.AddToRoleHistory(Jackal.Player.PlayerId, RoleInfo.jackal);
                        Sidekick.Player = Jackal.Player;
                        Sidekick.wasTeamRed = Jackal.wasTeamRed;
                        Sidekick.wasSpy = Jackal.wasSpy;
                        Sidekick.wasImpostor = Jackal.wasImpostor;
                    }
                    // now make amnesiac the new Jackal.
                    Jackal.ClearAndReload();
                    Jackal.Player = AmnesiacPlayer;
                    Jackal.canCreateSidekick = false;
                    Amnesiac.ClearAndReload();
                    Amnesiac.Player = target;
                    break;
                case RoleId.Sidekick:
                    Sidekick.ClearAndReload();
                    Sidekick.Player = AmnesiacPlayer;
                    Amnesiac.ClearAndReload();
                    Amnesiac.Player = target;
                    break;

                case RoleId.Eraser:
                    Utils.BecomeImpostor(Amnesiac.Player);
                    Eraser.ClearAndReload();
                    Eraser.Player = AmnesiacPlayer;
                    Amnesiac.ClearAndReload();
                    Amnesiac.Player = target;
                    break;

                case RoleId.Spy:
                    Spy.ClearAndReload();
                    Spy.Player = AmnesiacPlayer;
                    Amnesiac.ClearAndReload();
                    Amnesiac.Player = target;
                    break;

                case RoleId.Trickster:
                    Utils.BecomeImpostor(Amnesiac.Player);
                    Trickster.ClearAndReload();
                    Trickster.Player = AmnesiacPlayer;
                    Amnesiac.ClearAndReload();
                    Amnesiac.Player = target;
                    break;

                case RoleId.Cleaner:
                    Utils.BecomeImpostor(Amnesiac.Player);
                    Cleaner.ClearAndReload();
                    Cleaner.Player = AmnesiacPlayer;
                    Amnesiac.ClearAndReload();
                    Amnesiac.Player = target;
                    break;

                case RoleId.Warlock:
                    Utils.BecomeImpostor(Amnesiac.Player);
                    Warlock.ClearAndReload();
                    Warlock.Player = AmnesiacPlayer;
                    Amnesiac.ClearAndReload();
                    Amnesiac.Player = target;
                    break;

                case RoleId.Vigilante:
                    Vigilante.ClearAndReload();
                    Vigilante.Player = AmnesiacPlayer;
                    Amnesiac.ClearAndReload();
                    Amnesiac.Player = target;
                    break;
                
                case RoleId.Plaguebearer:
                    Plaguebearer.ClearAndReload();
                    Plaguebearer.Player = AmnesiacPlayer;
                    Amnesiac.ClearAndReload();
                    Amnesiac.Player = target;
                    break;
                
                case RoleId.Pestilence:
                    Pestilence.ClearAndReload();
                    Pestilence.Player = AmnesiacPlayer;
                    Amnesiac.ClearAndReload();
                    Amnesiac.Player = target;
                    break;

                case RoleId.Arsonist:
                    Arsonist.ClearAndReload();
                    Arsonist.Player = AmnesiacPlayer;
                    Amnesiac.ClearAndReload();
                    Amnesiac.Player = target;

                    if (PlayerControl.LocalPlayer == Arsonist.Player)
                    {
                        int playerCounter = 0;
                        Vector3 bottomLeft = new(-FastDestroyableSingleton<HudManager>.Instance.UseButton.transform.localPosition.x, FastDestroyableSingleton<HudManager>.Instance.UseButton.transform.localPosition.y, FastDestroyableSingleton<HudManager>.Instance.UseButton.transform.localPosition.z);
                        foreach (PlayerControl p in PlayerControl.AllPlayerControls)
                        {
                            if (BeanIcons.ContainsKey(p.PlayerId) && p != Arsonist.Player)
                            {
                                //Arsonist.poolIcons.Add(p);
                                if (Arsonist.dousedPlayers.Contains(p))
                                {
                                    BeanIcons[p.PlayerId].SetSemiTransparent(false);
                                }
                                else
                                {
                                    BeanIcons[p.PlayerId].SetSemiTransparent(true);
                                }

                                BeanIcons[p.PlayerId].transform.localPosition = bottomLeft + new Vector3(-0.25f, -0.25f, 0) + (Vector3.right * playerCounter++ * 0.35f);
                                BeanIcons[p.PlayerId].transform.localScale = Vector3.one * 0.2f;
                                BeanIcons[p.PlayerId].gameObject.SetActive(true);
                            }
                        }
                    }
                    break;

                case RoleId.BountyHunter:
                    Utils.BecomeImpostor(Amnesiac.Player);
                    BountyHunter.ClearAndReload();
                    BountyHunter.Player = AmnesiacPlayer;
                    Amnesiac.ClearAndReload();
                    Amnesiac.Player = target;

                    BountyHunter.bountyUpdateTimer = 0f;
                    if (PlayerControl.LocalPlayer == BountyHunter.Player)
                    {
                        Vector3 bottomLeft = new Vector3(-FastDestroyableSingleton<HudManager>.Instance.UseButton.transform.localPosition.x, FastDestroyableSingleton<HudManager>.Instance.UseButton.transform.localPosition.y, FastDestroyableSingleton<HudManager>.Instance.UseButton.transform.localPosition.z) + new Vector3(-0.25f, 1f, 0);
                        BountyHunter.CooldownText = UnityEngine.Object.Instantiate(FastDestroyableSingleton<HudManager>.Instance.KillButton.cooldownTimerText, FastDestroyableSingleton<HudManager>.Instance.transform);
                        BountyHunter.CooldownText.alignment = TMPro.TextAlignmentOptions.Center;
                        BountyHunter.CooldownText.transform.localPosition = bottomLeft + new Vector3(0f, -1f, -1f);
                        BountyHunter.CooldownText.gameObject.SetActive(true);

                        foreach (PlayerControl p in PlayerControl.AllPlayerControls)
                        {
                            if (BeanIcons.ContainsKey(p.PlayerId))
                            {
                                BeanIcons[p.PlayerId].SetSemiTransparent(false);
                                BeanIcons[p.PlayerId].transform.localPosition = bottomLeft + new Vector3(0f, -1f, 0);
                                BeanIcons[p.PlayerId].transform.localScale = Vector3.one * 0.4f;
                                BeanIcons[p.PlayerId].gameObject.SetActive(false);
                            }
                        }
                    }
                    break;

                case RoleId.Vulture:
                    Vulture.ClearAndReload();
                    Vulture.Player = AmnesiacPlayer;
                    Amnesiac.ClearAndReload();
                    Amnesiac.Player = target;
                    break;

                case RoleId.Medium:
                    Medium.ClearAndReload();
                    Medium.medium = AmnesiacPlayer;
                    Amnesiac.ClearAndReload();
                    Amnesiac.Player = target;
                    break;

                case RoleId.Lawyer:
                    Lawyer.Player = AmnesiacPlayer;
                    Amnesiac.ClearAndReload();
                    Amnesiac.Player = target;
                    break;

                case RoleId.Pursuer:
                    Pursuer.ClearAndReload();
                    Pursuer.Player = AmnesiacPlayer;
                    Amnesiac.ClearAndReload();
                    Amnesiac.Player = target;
                    break;

                case RoleId.Witch:
                    Utils.BecomeImpostor(Amnesiac.Player);
                    Witch.ClearAndReload();
                    Witch.Player = AmnesiacPlayer;
                    Amnesiac.ClearAndReload();
                    Amnesiac.Player = target;
                    break;

                case RoleId.Trapper:
                    Trapper.ClearAndReload();
                    Trapper.Player = AmnesiacPlayer;
                    Amnesiac.ClearAndReload();
                    Amnesiac.Player = target;
                    break;

                case RoleId.Ninja:
                    Utils.BecomeImpostor(Amnesiac.Player);
                    Ninja.ClearAndReload();
                    Ninja.Player = AmnesiacPlayer;
                    Amnesiac.ClearAndReload();
                    Amnesiac.Player = target;
                    break;
                
                case RoleId.Wraith:
                    Utils.BecomeImpostor(Amnesiac.Player);
                    Wraith.ClearAndReload();
                    Wraith.Player = AmnesiacPlayer;
                    Amnesiac.ClearAndReload();
                    Amnesiac.Player = target;
                    break;
            }
            var newRole = RoleInfo.GetRoleInfoForPlayer(AmnesiacPlayer).FirstOrDefault();
            var newRole3 = RoleInfo.GetRoleInfoForPlayer(target).FirstOrDefault();
            var vowel = "aeiou".Contains(newRole.Name.ToLower()[0]);
            var article = vowel ? "an" : "a";
            if (PlayerControl.LocalPlayer == AmnesiacPlayer)
            {
                Utils.ShowTextToast($"You remembered you were {article} {newRole.Name}!", 3.5f);
                SoundManager.Instance.PlaySound(ShipStatus.Instance.SabotageSound, false, 1f, null);
                Utils.ShowFlash(newRole.Color);
            }
            if (newRole != null && newRole != RoleInfo.amnesiac)
            {
                RoleInfo.AddToRoleHistory(AmnesiacPlayer.PlayerId, newRole);
            }
            if (newRole3 != null)
            {
                RoleInfo.AddToRoleHistory(target.PlayerId, newRole3);
            }
        }
        public static void SetTrap(byte[] buff) 
        {
            if (Trapper.Player == null) return;
            Trapper.Charges -= 1;
            Vector3 position = Vector3.zero;
            position.x = BitConverter.ToSingle(buff, 0 * sizeof(float));
            position.y = BitConverter.ToSingle(buff, 1 * sizeof(float));
            new Trap(position);
        }

        public static void TriggerTrap(byte playerId, byte trapId) 
        {
            Trap.TriggerTrap(playerId, trapId);
        }

        public static void SetGuesser(byte playerId)
        {
            PlayerControl target = Utils.PlayerById(playerId);
            if (target == null) return;
            new Guessers(target);
        }

        public static void ReceiveGhostInfo (byte senderId, MessageReader reader)
        {
            PlayerControl sender = Utils.PlayerById(senderId);

            GhostInfoTypes infoType = (GhostInfoTypes)reader.ReadByte();
            switch (infoType) {
                case GhostInfoTypes.HackNoticed:
                    Glitch.SetHackedKnows(true, senderId);
                    break;
                case GhostInfoTypes.HackOver:
                    _ = Glitch.HackedKnows.Remove(senderId);
                    break;
                case GhostInfoTypes.ArsonistDouse:
                    Arsonist.dousedPlayers.Add(Utils.PlayerById(reader.ReadByte()));
                    break;
                case GhostInfoTypes.PlaguebearerInfect:
                    Plaguebearer.InfectedPlayers.Add(Utils.PlayerById(reader.ReadByte()).PlayerId);
                    break;
                case GhostInfoTypes.BountyTarget:
                    BountyHunter.bounty = Utils.PlayerById(reader.ReadByte());
                    break;
                case GhostInfoTypes.NinjaMarked:
                    Ninja.ninjaMarked = Utils.PlayerById(reader.ReadByte());
                    break;
                case GhostInfoTypes.WarlockTarget:
                    Warlock.curseVictim = Utils.PlayerById(reader.ReadByte());
                    break;
                case GhostInfoTypes.MediumInfo:
                    string mediumInfo = reader.ReadString();
		             if (Utils.ShouldShowGhostInfo())
                    	FastDestroyableSingleton<HudManager>.Instance.Chat.AddChat(sender, mediumInfo);
                    break;
                case GhostInfoTypes.MysticInfo:
                    string mysticInfo = reader.ReadString();
		             if (Utils.ShouldShowGhostInfo())
                    	FastDestroyableSingleton<HudManager>.Instance.Chat.AddChat(sender, mysticInfo);
                    break;
                case GhostInfoTypes.OracleInfo:
                    string oracleInfo = reader.ReadString();
		             if (Utils.ShouldShowGhostInfo())
                    	FastDestroyableSingleton<HudManager>.Instance.Chat.AddChat(sender, oracleInfo);
                    break;
                case GhostInfoTypes.DetectiveOrMedicInfo:
                    string detectiveInfo = reader.ReadString();
                    if (Utils.ShouldShowGhostInfo())
		    	        FastDestroyableSingleton<HudManager>.Instance.Chat.AddChat(sender, detectiveInfo);
                    break;
                case GhostInfoTypes.BlankUsed:
                    Pursuer.blankedList.Remove(sender);
                    break;
                case GhostInfoTypes.PoisonerTimer:
                    poisonerKillButton.Timer = (float)reader.ReadByte();
                    break;
                case GhostInfoTypes.DeathReasonAndKiller:
                    GameHistory.CreateDeathReason(Utils.PlayerById(reader.ReadByte()), (DeadPlayer.CustomDeathReason)reader.ReadByte(), Utils.PlayerById(reader.ReadByte()));
                    break;
            }
        }

        public static void YoyoMarkLocation(byte[] buff) 
        {
            if (Yoyo.Player == null) return;
            Vector3 position = Vector3.zero;
            position.x = BitConverter.ToSingle(buff, 0 * sizeof(float));
            position.y = BitConverter.ToSingle(buff, 1 * sizeof(float));
            Yoyo.MarkLocation(position);
            new Silhouette(position, -1, false);
        }

        public static void YoyoBlink(bool isFirstJump, byte[] buff) 
        {
            if (Yoyo.Player == null || Yoyo.markedLocation == null) return;
            var markedPos = (Vector3)Yoyo.markedLocation;
            Yoyo.Player.NetTransform.SnapTo(markedPos);

            var markedSilhouette = Silhouette.silhouettes.FirstOrDefault(s => s.gameObject.transform.position.x == markedPos.x && s.gameObject.transform.position.y == markedPos.y);
            if (markedSilhouette != null)
                markedSilhouette.permanent = false;

            Vector3 position = Vector3.zero;
            position.x = BitConverter.ToSingle(buff, 0 * sizeof(float));
            position.y = BitConverter.ToSingle(buff, 1 * sizeof(float));
            // Create Silhoutte At Start Position:
            if (isFirstJump) {
                Yoyo.MarkLocation(position);
                new Silhouette(position, Yoyo.blinkDuration, true);
            } else {
                new Silhouette(position, 5, true);
                Yoyo.markedLocation = null;
            }
            if (Chameleon.Players.Any(x => x.PlayerId == Yoyo.Player.PlayerId)) // Make the Yoyo visible if chameleon!
                Chameleon.lastMoved[Yoyo.Player.PlayerId] = Time.time;            
        }

        public static void BreakArmor() 
        {
            if (Armored.Player == null || Armored.isBrokenArmor) return;
            Armored.isBrokenArmor = true;
            if (PlayerControl.LocalPlayer.Data.IsDead) 
            {
                Armored.Player.ShowFailedMurder();
            }
        }
    }

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.HandleRpc))]
    public static class HandleRpc
    {
        public static void Postfix([HarmonyArgument(0)] byte callId, [HarmonyArgument(1)] MessageReader reader)
        {
            switch ((CustomRPC) callId)
            {
                // Main Controls
                case CustomRPC.ResetVaribles:
                    RPCProcedure.ResetVariables();
                    break;
                case CustomRPC.ShareOptions:
                    RPCProcedure.HandleShareOptions(reader.ReadByte(), reader);
                    break;
                case CustomRPC.ForceEnd:
                    RPCProcedure.ForceEnd();
                    break; 
                case CustomRPC.WorkaroundSetRoles:
                    RPCProcedure.WorkaroundSetRoles(reader.ReadByte(), reader);
                    break;
                case CustomRPC.SetRole:
                    byte roleId = reader.ReadByte();
                    byte playerId = reader.ReadByte();
                    RPCProcedure.SetRole(roleId, playerId);
                    break;
               /* case CustomRPC.DraftModePickOrder:
                    Modules.RoleDraft.ReceivePickOrder(reader.ReadByte(), reader);
                    break;
                case CustomRPC.DraftModePick:
                    Modules.RoleDraft.ReceivePick(reader.ReadByte(), reader.ReadByte());
                    break;*/
                case CustomRPC.SetModifier:
                    byte modifierId = reader.ReadByte();
                    byte pId = reader.ReadByte();
                    byte flag = reader.ReadByte();
                    RPCProcedure.SetModifier(modifierId, pId, flag);
                    break;
                case CustomRPC.SetAbility:
                    byte abilityId = reader.ReadByte();
                    byte aId = reader.ReadByte();
                    byte aflag = reader.ReadByte();
                    RPCProcedure.SetAbility(abilityId, aId, aflag);
                    break;
                case CustomRPC.VersionHandshake:
                    byte major = reader.ReadByte();
                    byte minor = reader.ReadByte();
                    byte patch = reader.ReadByte();
                    float timer = reader.ReadSingle();
                    if (!AmongUsClient.Instance.AmHost && timer >= 0f) GameStartManagerPatch.timer = timer;
                    int versionOwnerId = reader.ReadPackedInt32();
                    byte revision = 0xFF;
                    Guid guid;
                    if (reader.Length - reader.Position >= 17) { // enough bytes left to read
                        revision = reader.ReadByte();
                        // GUID
                        byte[] gbytes = reader.ReadBytes(16);
                        guid = new Guid(gbytes);
                    } else {
                        guid = new Guid(new byte[16]);
                    }
                    RPCProcedure.VersionHandshake(major, minor, patch, revision == 0xFF ? -1 : revision, guid, versionOwnerId);
                    break;
                case CustomRPC.UseUncheckedVent:
                    int ventId = reader.ReadPackedInt32();
                    byte ventingPlayer = reader.ReadByte();
                    byte isEnter = reader.ReadByte();
                    RPCProcedure.UseUncheckedVent(ventId, ventingPlayer, isEnter);
                    break;
                case CustomRPC.UncheckedMurderPlayer:
                    byte source = reader.ReadByte();
                    byte target = reader.ReadByte();
                    byte showAnimation = reader.ReadByte();
                    RPCProcedure.UncheckedMurderPlayer(source, target, showAnimation);
                    break;
                case CustomRPC.UncheckedExilePlayer:
                    byte exileTarget = reader.ReadByte();
                    RPCProcedure.UncheckedExilePlayer(exileTarget);
                    break;
                case CustomRPC.UncheckedCmdReportDeadBody:
                    byte reportSource = reader.ReadByte();
                    byte reportTarget = reader.ReadByte();
                    RPCProcedure.UncheckedCmdReportDeadBody(reportSource, reportTarget);
                    break;
                case CustomRPC.DragBody:
                    RPCProcedure.DragBody(reader.ReadByte());
                    break;
                case CustomRPC.DropBody:
                    RPCProcedure.DropBody(reader.ReadByte());
                    break;
                case CustomRPC.DynamicMapOption:
                    byte mapId = reader.ReadByte();
                    RPCProcedure.DynamicMapOption(mapId);
                    break;
                case CustomRPC.SetGameStarting:
                    RPCProcedure.SetGameStarting();
                    break;

                // Role functionality

                case CustomRPC.EngineerFixLights:
                    RPCProcedure.EngineerFixLights();
                    break;
                case CustomRPC.EngineerFixSubmergedOxygen:
                    RPCProcedure.EngineerFixSubmergedOxygen();
                    break;
                case CustomRPC.RemoveBlackmail:
                    RPCProcedure.RemoveBlackmail();
                    break;
                case CustomRPC.EngineerUsedRepair:
                    RPCProcedure.EngineerUsedRepair();
                    break;
                case CustomRPC.CleanBody:
                    RPCProcedure.CleanBody(reader.ReadByte(), reader.ReadByte());
                    break;
                case CustomRPC.TimeMasterRewindTime:
                    RPCProcedure.TimeMasterRewindTime();
                    break;
                case CustomRPC.TimeMasterStopRewindTime:
                    RPCProcedure.TimeMasterStopRewindTime();
                    break;
                case CustomRPC.VeteranAlert:
                    RPCProcedure.VeteranAlert();
                    break;
                case CustomRPC.VeteranAlertKill:
                    RPCProcedure.VeteranAlertKill(reader.ReadByte());
                    break;
                case CustomRPC.PestilenceKill:
                    RPCProcedure.PestilenceKill(reader.ReadByte());
                    break;
                case CustomRPC.MedicSetShielded:
                    RPCProcedure.MedicSetShielded(reader.ReadByte());
                    break;
                case CustomRPC.Fortify:
                    RPCProcedure.Fortify(reader.ReadByte());
                    break;
                case CustomRPC.RomanticSetBeloved:
                    RPCProcedure.RomanticSetBeloved(reader.ReadByte());
                    break;
                case CustomRPC.ShieldedMurderAttempt:
                    RPCProcedure.ShieldedMurderAttempt();
                    break;
                case CustomRPC.FortifiedMurderAttempt:
                    RPCProcedure.FortifiedMurderAttempt();
                    break;
                case CustomRPC.Confess:
                    byte confessorId = reader.ReadByte();
                    Oracle.Confessor = Utils.PlayerById(confessorId);
                    if (Oracle.Confessor == null) break; // Ensure the confessor exists
                    // Read the revealed faction from the RPC
                    int factionId = reader.ReadInt32();
                    // Map the received integer to the correct Faction enum
                    if (Enum.IsDefined(typeof(Faction), factionId))
                    {
                        Oracle.RevealedFaction = (Faction)factionId;
                    }
                    else
                    {
                        TheSushiRolesPlugin.Logger.LogError($"Invalid faction ID received: {factionId}");
                        Oracle.RevealedFaction = Faction.Other; // Default to Other in case of error
                    }
                    break;
                case CustomRPC.SwapperSwap:
                    byte playerId1 = reader.ReadByte();
                    byte playerId2 = reader.ReadByte();
                    RPCProcedure.SwapperSwap(playerId1, playerId2);
                    break;
                case CustomRPC.MayorSetVoteTwice:
                    Mayor.voteTwice = reader.ReadBoolean();
                    break;
                case CustomRPC.Disperse:
                    byte teleports = reader.ReadByte();
                    Dictionary<byte, Vector2> coordinates = new Dictionary<byte, Vector2>();
                    for (int i = 0; i < teleports; i++)
                    {
                        byte playerId11 = reader.ReadByte();
                        Vector2 location = reader.ReadVector2();
                        coordinates.Add(playerId11, location);
                    }
                    RPCProcedure.StartTransportation(coordinates);
                    break;
                case CustomRPC.MorphlingMorph:
                    RPCProcedure.MorphlingMorph(reader.ReadByte());
                    break;
                case CustomRPC.GlitchMimic:
                    RPCProcedure.GlitchMimic(reader.ReadByte());
                    break;
                case CustomRPC.HitmanMorph:
                    RPCProcedure.HitmanMorph(reader.ReadByte());
                    break;
                case CustomRPC.CamouflagerCamouflage:
                    RPCProcedure.CamouflagerCamouflage();
                    break;
                case CustomRPC.GrenadierFlash:
                    RPCProcedure.GrenadierFlash();
                    break;
                case CustomRPC.WerewolfMaul:
                    RPCProcedure.WerewolfMaul();
                    break;
                case CustomRPC.PoisonerSetPoisoned:
                    byte poisonedId = reader.ReadByte();
                    byte reset = reader.ReadByte();
                    RPCProcedure.PoisonerSetPoisoned(poisonedId, reset);
                    break;
                case CustomRPC.TrackerUsedTracker:
                    RPCProcedure.TrackerUsedTracker(reader.ReadByte());
                    break;               
                case CustomRPC.GlitchUsedHacks:
                    RPCProcedure.GlitchUsedHacks(reader.ReadByte());
                    break;
                case CustomRPC.JackalCreatesSidekick:
                    RPCProcedure.JackalCreatesSidekick(reader.ReadByte());
                    break;
                case CustomRPC.BlackmailerBlackmail:
                    RPCProcedure.BlackmailerBlackmail(reader.ReadByte());
                    break;
                case CustomRPC.SidekickPromotes:
                    RPCProcedure.SidekickPromotes();
                    break;
                case CustomRPC.ErasePlayerRoles:
                    byte eraseTarget = reader.ReadByte();
                    RPCProcedure.ErasePlayerRoles(eraseTarget);
                    Eraser.alreadyErased.Add(eraseTarget);
                    break;
                case CustomRPC.SetFutureErased:
                    RPCProcedure.SetFutureErased(reader.ReadByte());
                    break;
                case CustomRPC.SetFutureShielded:
                    RPCProcedure.SetFutureShielded(reader.ReadByte());
                    break;
                case CustomRPC.PlaceNinjaTrace:
                    RPCProcedure.PlaceNinjaTrace(reader.ReadBytesAndSize());
                    break;
                case CustomRPC.PlacePortal:
                    RPCProcedure.PlacePortal(reader.ReadBytesAndSize());
                    break;
                case CustomRPC.PlaceMine:
                    RPCProcedure.PlaceMine(reader.ReadBytesAndSize());
                    break;
                case CustomRPC.UsePortal:
                    RPCProcedure.UsePortal(reader.ReadByte(), reader.ReadByte());
                    break;
                case CustomRPC.PlaceJackInTheBox:
                    RPCProcedure.PlaceJackInTheBox(reader.ReadBytesAndSize());
                    break;
                case CustomRPC.LightsOut:
                    RPCProcedure.LightsOut();
                    break;
                case CustomRPC.PlaceCamera:
                    RPCProcedure.PlaceCamera(reader.ReadBytesAndSize());
                    break;
                case CustomRPC.SealVent:
                    RPCProcedure.SealVent(reader.ReadPackedInt32());
                    break;
                case CustomRPC.ArsonistWin:
                    RPCProcedure.ArsonistWin();
                    break;
                case CustomRPC.Guesserhoot:
                    byte killerId = reader.ReadByte();
                    byte dyingTarget = reader.ReadByte();
                    byte guessedTarget = reader.ReadByte();
                    byte guessedRoleId = reader.ReadByte();
                    RPCProcedure.Guesserhoot(killerId, dyingTarget, guessedTarget, guessedRoleId);
                    break;
                case CustomRPC.LawyerSetTarget:
                    RPCProcedure.LawyerSetTarget(reader.ReadByte()); 
                    break;
                case CustomRPC.LawyerChangeRole:
                    RPCProcedure.LawyerChangeRole();
                    break;
                case CustomRPC.ProsecutorSetTarget:
                    RPCProcedure.ProsecutorSetTarget(reader.ReadByte()); 
                    break;
                case CustomRPC.ProsecutorChangeRole:
                    RPCProcedure.ProsecutorChangeRole();
                    break;
                case CustomRPC.RomanticChangeRole:
                    RPCProcedure.RomanticChangeRole();
                    break;
                case CustomRPC.TurnPestilence:
                    RPCProcedure.PlaguebearerTurnPestilence();
                    break;
                case CustomRPC.AgentTurnIntoHitman:
                    RPCProcedure.AgentTurnIntoHitman();
                    break;
                case CustomRPC.SetBlanked:
                    var pid = reader.ReadByte();
                    var blankedValue = reader.ReadByte();
                    RPCProcedure.SetBlanked(pid, blankedValue);
                    break;
                case CustomRPC.SetFutureSpelled:
                    RPCProcedure.SetFutureSpelled(reader.ReadByte());
                    break;
                case CustomRPC.SetFirstKill:
                    byte firstKill = reader.ReadByte();
                    RPCProcedure.SetFirstKill(firstKill);
                    break;
                case CustomRPC.SetTiebreak:
                    RPCProcedure.SetTiebreak();
                    break;
                case CustomRPC.AmnesiacRemember:
                    RPCProcedure.AmnesiacRemember(reader.ReadByte());
                    break;
                case CustomRPC.SetInvisible:
                    byte invisiblePlayer = reader.ReadByte();
                    byte invisibleFlag = reader.ReadByte();
                    RPCProcedure.SetInvisible(invisiblePlayer, invisibleFlag);
                    break;
                case CustomRPC.SetVanish:
                    RPCProcedure.SetVanish(reader.ReadByte(), reader.ReadByte());
                    break;
                case CustomRPC.SetTrap:
                    RPCProcedure.SetTrap(reader.ReadBytesAndSize());
                    break;
                case CustomRPC.TriggerTrap:
                    byte trappedPlayer = reader.ReadByte();
                    byte trapId = reader.ReadByte();
                    RPCProcedure.TriggerTrap(trappedPlayer, trapId);
                    break;
                case CustomRPC.StopStart:
                    RPCProcedure.StopStart(reader.ReadByte());
                    break;
                case CustomRPC.YoyoMarkLocation:
                    RPCProcedure.YoyoMarkLocation(reader.ReadBytesAndSize());
                    break;
                case CustomRPC.YoyoBlink:
                    RPCProcedure.YoyoBlink(reader.ReadByte() == byte.MaxValue, reader.ReadBytesAndSize());
                    break;
                case CustomRPC.BreakArmor:
                    RPCProcedure.BreakArmor();
                    break;

                // Game mode
                case CustomRPC.SetGuesser:
                    byte Guesser = reader.ReadByte();
                    RPCProcedure.SetGuesser(Guesser);
                    break;
                case CustomRPC.ShareGhostInfo:
                    RPCProcedure.ReceiveGhostInfo(reader.ReadByte(), reader);
                    break;
            }
        }
    }
} 
