using System.Collections.Generic;
using System.Linq;
using AmongUs.Data;
using Assets.CoreScripts;
using Reactor.Networking.Extensions;
using System.Collections;

namespace TheSushiRoles
{
    public static class RPCProcedure 
    {
        #region Main Controls
        public static void ResetVariables() 
        {
            MapOptions.ClearAndReloadMapOptions();
            Utils.GlobalClearAndReload();
            GameHistory.ClearGameHistory();
            CustomButtonLoader.SetCustomButtonCooldowns();
            CustomButton.ReloadHotkeys();
            MapOptions.ReloadPluginOptions();
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
                TheSushiRoles.Logger.LogError("Error while deserializing options: " + e.Message);
            }
        }
        public static void WorkaroundSetRoles(byte numberOfRoles, MessageReader reader)
        {
            for (int i = 0; i < numberOfRoles; i++)
            {                   
                byte playerId = (byte) reader.ReadPackedUInt32();
                byte roleId = (byte) reader.ReadPackedUInt32();
                try
                {
                    SetRole(roleId, playerId);
                }
                catch (Exception e)
                {
                    TheSushiRoles.Logger.LogError("Error while deserializing roles: " + e.Message);
                }
            }
        }

        public static void StopStart(byte playerId) 
        {
            if (!CustomGameOptions.EveryoneCanStopStart) return;

            SoundManagerInstance().StopSound(GameStartManager.Instance.gameStartSound);
            if (AmongUsClient.Instance.AmHost) 
            {
                GameStartManager.Instance.ResetStartState();
                PlayerControl.LocalPlayer.RpcSendChat($"{Utils.GetPlayerById(playerId).Data.PlayerName} stopped the game countdown!");
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
                            GameHistory.AddToRoleHistory(player.PlayerId, RoleInfo.jester);
                            break;
                        case RoleId.Mayor:
                            Mayor.Player = player;
                            GameHistory.AddToRoleHistory(player.PlayerId, RoleInfo.mayor);
                            break;
                        case RoleId.Gatekeeper:
                            Gatekeeper.Player = player;
                            GameHistory.AddToRoleHistory(player.PlayerId, RoleInfo.gatekeeper);
                            break;
                        case RoleId.Engineer:
                            Engineer.Player = player;
                            GameHistory.AddToRoleHistory(player.PlayerId, RoleInfo.engineer);
                            break;
                        case RoleId.Sheriff:
                            Sheriff.Player = player;
                            GameHistory.AddToRoleHistory(player.PlayerId, RoleInfo.sheriff);
                            break;
                        case RoleId.VengefulRomantic:
                            VengefulRomantic.Player = player;
                            GameHistory.AddToRoleHistory(player.PlayerId, RoleInfo.vromantic);
                            break;
                        case RoleId.Blackmailer:
                            Blackmailer.Player = player;
                            GameHistory.AddToRoleHistory(player.PlayerId, RoleInfo.blackmailer);
                            break;
                        case RoleId.Glitch:
                            Glitch.Player = player;
                            GameHistory.AddToRoleHistory(player.PlayerId, RoleInfo.glitch);
                            break;
                        case RoleId.Werewolf:
                            Werewolf.Player = player;
                            GameHistory.AddToRoleHistory(player.PlayerId, RoleInfo.werewolf);
                            break;
                        case RoleId.Agent:
                            Agent.Player = player;
                            GameHistory.AddToRoleHistory(player.PlayerId, RoleInfo.agent);
                            break;
                        case RoleId.Hitman:
                            Hitman.Player = player;
                            GameHistory.AddToRoleHistory(player.PlayerId, RoleInfo.hitman);
                            break;
                        case RoleId.Undertaker:
                            Undertaker.Player = player;
                            GameHistory.AddToRoleHistory(player.PlayerId, RoleInfo.undertaker);
                            break;
                        case RoleId.Oracle:
                            Oracle.Player = player;
                            GameHistory.AddToRoleHistory(player.PlayerId, RoleInfo.oracle);
                            break;
                        case RoleId.Amnesiac:
                            Amnesiac.Player = player;
                            GameHistory.AddToRoleHistory(player.PlayerId, RoleInfo.amnesiac);
                            break;
                        case RoleId.Plaguebearer:
                            Plaguebearer.Player = player;
                            GameHistory.AddToRoleHistory(player.PlayerId, RoleInfo.plaguebearer);
                            break;
                        case RoleId.Pestilence:
                            Pestilence.Player = player;
                            GameHistory.AddToRoleHistory(player.PlayerId, RoleInfo.pestilence);
                            break;
                        case RoleId.Detective:
                            Detective.Player = player;
                            GameHistory.AddToRoleHistory(player.PlayerId, RoleInfo.detective);
                            break;
                        case RoleId.Chronos:
                            Chronos.Player = player;
                            GameHistory.AddToRoleHistory(player.PlayerId, RoleInfo.chronos);
                            break;
                        case RoleId.Veteran:
                            Veteran.Player = player;
                            GameHistory.AddToRoleHistory(player.PlayerId, RoleInfo.veteran);
                            break;
                        case RoleId.Medic:
                            Medic.Player = player;
                            GameHistory.AddToRoleHistory(player.PlayerId, RoleInfo.medic);
                            break;
                        case RoleId.Crusader:
                            Crusader.Player = player;
                            GameHistory.AddToRoleHistory(player.PlayerId, RoleInfo.crusader);
                            break;
                        case RoleId.Miner:
                            Miner.Player = player;
                            GameHistory.AddToRoleHistory(player.PlayerId, RoleInfo.miner);
                            break;
                        case RoleId.Mystic:
                            Mystic.Player = player;
                            GameHistory.AddToRoleHistory(player.PlayerId, RoleInfo.mystic);
                            break;
                        case RoleId.Juggernaut:
                            Juggernaut.Player = player;
                            GameHistory.AddToRoleHistory(player.PlayerId, RoleInfo.juggernaut);
                            break;
                        case RoleId.Morphling:
                            Morphling.Player = player;
                            GameHistory.AddToRoleHistory(player.PlayerId, RoleInfo.morphling);
                            break;
                        case RoleId.Painter:
                            Painter.Player = player;
                            GameHistory.AddToRoleHistory(player.PlayerId, RoleInfo.painter);
                            break;
                        case RoleId.Snitch:
                            Snitch.Player = player;
                            GameHistory.AddToRoleHistory(player.PlayerId, RoleInfo.snitch);
                            break;
                        case RoleId.Predator:
                            Predator.Player = player;
                            GameHistory.AddToRoleHistory(player.PlayerId, RoleInfo.predator);
                            break;
                        case RoleId.Hacker:
                            Hacker.Player = player;
                            GameHistory.AddToRoleHistory(player.PlayerId, RoleInfo.hacker);
                            break;
                        case RoleId.Tracker:
                            Tracker.Player = player;
                            GameHistory.AddToRoleHistory(player.PlayerId, RoleInfo.tracker);
                            break;
                        case RoleId.Viper:
                            Viper.Player = player;
                            GameHistory.AddToRoleHistory(player.PlayerId, RoleInfo.viper);
                            break;
                        case RoleId.Romantic:
                            Romantic.Player = player;
                            GameHistory.AddToRoleHistory(player.PlayerId, RoleInfo.romantic);
                            break;
                        case RoleId.Spy:
                            Spy.Player = player;
                            GameHistory.AddToRoleHistory(player.PlayerId, RoleInfo.spy);
                            break;
                        case RoleId.Deputy:
                            Deputy.Player = player;
                            GameHistory.AddToRoleHistory(player.PlayerId, RoleInfo.deputy);
                            break;
                        case RoleId.Trickster:
                            Trickster.Player = player;
                            GameHistory.AddToRoleHistory(player.PlayerId, RoleInfo.trickster);
                            break;
                        case RoleId.Janitor:
                            Janitor.Player = player;
                            GameHistory.AddToRoleHistory(player.PlayerId, RoleInfo.janitor);
                            break;
                        case RoleId.Warlock:
                            Warlock.Player = player;
                            GameHistory.AddToRoleHistory(player.PlayerId, RoleInfo.warlock);
                            break;
                        case RoleId.Monarch:
                            Monarch.Player = player;
                            GameHistory.AddToRoleHistory(player.PlayerId, RoleInfo.monarch);
                            break;
                        case RoleId.Grenadier:
                            Grenadier.Player = player;
                            GameHistory.AddToRoleHistory(player.PlayerId, RoleInfo.grenadier);
                            break;
                        case RoleId.Vigilante:
                            Vigilante.Player = player;
                            GameHistory.AddToRoleHistory(player.PlayerId, RoleInfo.vigilante);
                            break;
                        case RoleId.Arsonist:
                            Arsonist.Player = player;
                            GameHistory.AddToRoleHistory(player.PlayerId, RoleInfo.arsonist);
                            break;
                        case RoleId.BountyHunter:
                            BountyHunter.Player = player;
                            GameHistory.AddToRoleHistory(player.PlayerId, RoleInfo.bountyHunter);
                            break;
                        case RoleId.Scavenger:
                            Scavenger.Player = player;
                            GameHistory.AddToRoleHistory(player.PlayerId, RoleInfo.scavenger);
                            break;
                        case RoleId.Psychic:
                            Psychic.Player = player;
                            GameHistory.AddToRoleHistory(player.PlayerId, RoleInfo.psychic);
                            break;
                        case RoleId.Landlord:
                            Landlord.Player = player;
                            GameHistory.AddToRoleHistory(player.PlayerId, RoleInfo.landlord);
                            break;
                        case RoleId.Trapper:
                            Trapper.Player = player;
                            GameHistory.AddToRoleHistory(player.PlayerId, RoleInfo.trapper);
                            break;
                        case RoleId.Lawyer:
                            Lawyer.Player = player;
                            GameHistory.AddToRoleHistory(player.PlayerId, RoleInfo.lawyer);
                            break;
                        case RoleId.Prosecutor:
                            Prosecutor.Player = player;
                            GameHistory.AddToRoleHistory(player.PlayerId, RoleInfo.prosecutor);
                            break;
                        case RoleId.Survivor:
                            Survivor.Player = player;
                            GameHistory.AddToRoleHistory(player.PlayerId, RoleInfo.survivor);
                            break;
                        case RoleId.Witch:
                            Witch.Player = player;
                            GameHistory.AddToRoleHistory(player.PlayerId, RoleInfo.witch);
                            break;
                        case RoleId.Assassin:
                            Assassin.Player = player;
                            GameHistory.AddToRoleHistory(player.PlayerId, RoleInfo.assassin);
                            break;
                        case RoleId.Wraith:
                            Wraith.Player = player;
                            GameHistory.AddToRoleHistory(player.PlayerId, RoleInfo.wraith);
                            break;
                        case RoleId.Yoyo:
                            Yoyo.Player = player;
                            GameHistory.AddToRoleHistory(player.PlayerId, RoleInfo.yoyo);
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
            PlayerControl player = Utils.GetPlayerById(playerId);
            switch ((AbilityId)abilityId)
            {
                case AbilityId.Coward:
                    Coward.Player = player;
                    break;
                case AbilityId.Paranoid:
                    Paranoid.Player = player;
                    break;
                case AbilityId.Lighter:
                    FlashLight.Player = player;
                    break;
            }
        }

        public static void SetModifier(byte modifierId, byte playerId, byte flag) 
        {
            PlayerControl player = Utils.GetPlayerById(playerId); 
            switch ((ModifierId)modifierId) 
            {
                case ModifierId.Bait:
                    Bait.Players.Add(player);
                    break;
                case ModifierId.Disperser:
                    Disperser.Player = player;
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
                case ModifierId.Blind:
                    Blind.Players.Add(player);
                    break;
                case ModifierId.Mini:
                    Mini.Player = player;
                    break;
                case ModifierId.Giant:
                    Giant.Player = player;
                    break;
                case ModifierId.Vip:
                    Vip.Players.Add(player);
                    break;
                case ModifierId.Drunk:
                    Drunk.Players.Add(player);
                    break;
                case ModifierId.Chameleon:
                    Chameleon.Players.Add(player);
                    break;
                case ModifierId.Lucky:
                    Lucky.Player = player;
                    break;
            }
        }

        public static void VersionHandshake(int major, int minor, int build, int revision, Guid guid, int clientId) 
        {
            Version ver;
            if (revision < 0) 
                ver = new Version(major, minor, build);
            else 
                ver = new Version(major, minor, build, revision);
            GameStartManagerPatch.playerVersions[clientId] = new GameStartManagerPatch.PlayerVersion(ver, guid);
        }

        public static void UseUncheckedVent(int ventId, byte playerId, byte isEnter)
        {
            PlayerControl player = Utils.GetPlayerById(playerId);
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

        public static void UncheckedCmdReportDeadBody(byte sourceId, byte targetId) 
        {
            PlayerControl source = Utils.GetPlayerById(sourceId);
            var t = targetId == Byte.MaxValue ? null : Utils.GetPlayerById(targetId).Data;
            if (source != null) source.ReportDeadBody(t);
        }

        public static void UncheckedExilePlayer(byte targetId) 
        {
            PlayerControl target = Utils.GetPlayerById(targetId);
            if (target != null) target.Exiled();
        }

        public static void RandomMapOption(byte mapId) 
        {
           GameOptionsManager.Instance.currentNormalGameOptions.MapId = mapId;
        }

        public static void SetGameStarting() 
        {
            GameStartManagerPatch.GameStartManagerUpdatePatch.startingTimer = 5f;
        }
        
        #endregion

        #region  Role functionality
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
                Utils.ShowFlash(Engineer.Color);
                SoundEffectsManager.Play("engineerRepair");
            }
        }
        public static void LandlordHandleTeleport(HudManager __instance)
        {
            Landlord.Charges--;
            if (!Landlord.UnteleportablePlayers.ContainsKey(Landlord.FirstTarget.PlayerId) && !Landlord.UnteleportablePlayers.ContainsKey(Landlord.SecondTarget.PlayerId) && !Lazy.Players.Any(x => x.PlayerId == Landlord.SecondTarget.PlayerId) && !Lazy.Players.Any(x => x.PlayerId == Landlord.FirstTarget.PlayerId))
            {
                Coroutines.Start(LandlordTransportPlayers(Landlord.FirstTarget.PlayerId, Landlord.SecondTarget.PlayerId, false));
                Utils.SendRPC(CustomRPC.LandlordTeleport, Landlord.FirstTarget.PlayerId, Landlord.SecondTarget.PlayerId, false);
            }
            else
            {
                __instance.StartCoroutine(Effects.SwayX(CustomButtonLoader.LandlordButton.actionButton.transform));
                if (Landlord.Player == PlayerControl.LocalPlayer)
                {
                    SoundManagerInstance().PlaySound(ShipStatus.Instance.SabotageSound, false, 1f, null);
                    Utils.SendMessage(Utils.ColorString(Landlord.Color, "One (or both) of your chosen players cannot be teleported"));
                }
            }
        }
        public static IEnumerator LandlordOpenSecondMenu()
        {
            try
            {
                Patches.ShapeShifterMenu.Singleton.Menu.ForceClose();
            }
            catch
            {

            }
            yield return (object)new WaitForSeconds(0.05f);
            Landlord.SwappingMenus = false;
            if (MeetingHud.Instance || PlayerControl.LocalPlayer != Landlord.Player) yield break;
            List<byte> transportTargets = new List<byte>();
            foreach (var player in PlayerControl.AllPlayerControls)
            {
                if (!player.Data.Disconnected && player != Landlord.FirstTarget)
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
            var pk = new ShapeShifterMenu(CustomButtonLoader.LandlordButton, (x) =>
            {
                Landlord.SecondTarget = x;
                LandlordHandleTeleport(HudManager.Instance);
            }, (y) =>
            {
                return transporttargetIDs.Contains(y.PlayerId);
            });
            Coroutines.Start(pk.Open(0f, true));
        }        
        public static IEnumerator LandlordTransportPlayers(byte player1, byte player2, bool die)
        {
            var Target1 = Utils.GetPlayerById(player1);
            var Target2 = Utils.GetPlayerById(player2);
            var deadBodies = UObject.FindObjectsOfType<DeadBody>();
            DeadBody Player1Body = null;
            DeadBody Player2Body = null;
            if (Target1.Data.IsDead)
            {
                foreach (var body in deadBodies) if (body.ParentId == Target1.PlayerId) Player1Body = body;
                if (Player1Body == null) yield break;
            }
            if (Target2.Data.IsDead)
            {
                foreach (var body in deadBodies) if (body.ParentId == Target2.PlayerId) Player2Body = body;
                if (Player2Body == null) yield break;
            }

            if (Target1.inVent && PlayerControl.LocalPlayer.PlayerId == Target1.PlayerId)
            {
                while (SubmergedCompatibility.GetInTransition())
                {
                    yield return null;
                }
                Target1.MyPhysics.ExitAllVents();
            }
            if (Target2.inVent && PlayerControl.LocalPlayer.PlayerId == Target2.PlayerId)
            {
                while (SubmergedCompatibility.GetInTransition())
                {
                    yield return null;
                }
                Target2.MyPhysics.ExitAllVents();
            }

            if (Player1Body == null && Player2Body == null)
            {
                Target1.MyPhysics.ResetMoveState();
                Target2.MyPhysics.ResetMoveState();
                var Target1Position = Target1.GetTruePosition();
                Target1Position = new Vector2(Target1Position.x, Target1Position.y + 0.3636f);
                var Target2Position = Target2.GetTruePosition();
                Target2Position = new Vector2(Target2Position.x, Target2Position.y + 0.3636f);
                if (Target1.transform.localScale == new Vector3(0.4f, 0.4f, 1.0f))
                {
                    Target1Position = new Vector2(Target1Position.x, Target1Position.y + 0.2233912f * 0.75f);
                    Target2Position = new Vector2(Target2Position.x, Target2Position.y - 0.2233912f * 0.75f);
                }
                if (Target2.transform.localScale == new Vector3(0.4f, 0.4f, 1.0f))
                {
                    Target1Position = new Vector2(Target1Position.x, Target1Position.y - 0.2233912f * 0.75f);
                    Target2Position = new Vector2(Target2Position.x, Target2Position.y + 0.2233912f * 0.75f);
                }

                Target1.transform.position = Target2Position;
                Target1.NetTransform.SnapTo(Target2Position);
                if (die) Utils.MurderPlayer(Target1, Target2, true);
                else
                {
                    Target2.transform.position = Target1Position;
                    Target2.NetTransform.SnapTo(Target1Position);
                }

                if (SubmergedCompatibility.IsSubmerged)
                {
                    if (PlayerControl.LocalPlayer.PlayerId == Target1.PlayerId)
                    {
                        SubmergedCompatibility.ChangeFloor(Target1.GetTruePosition().y > -7);
                        SubmergedCompatibility.CheckOutOfBoundsElevator(PlayerControl.LocalPlayer);
                    }
                    if (PlayerControl.LocalPlayer.PlayerId == Target2.PlayerId)
                    {
                        SubmergedCompatibility.ChangeFloor(Target2.GetTruePosition().y > -7);
                        SubmergedCompatibility.CheckOutOfBoundsElevator(PlayerControl.LocalPlayer);
                    }
                }

            }
            else if (Player1Body != null && Player2Body == null)
            {
                DropBody(Player1Body.ParentId);
                HitmanDropBody(Player1Body.ParentId);
                Target2.MyPhysics.ResetMoveState();
                var Target1Position = Player1Body.TruePosition;
                Target1Position = new Vector2(Target1Position.x, Target1Position.y + 0.3636f);
                var Target2Position = Target2.GetTruePosition();
                Target2Position = new Vector2(Target2Position.x, Target2Position.y + 0.3636f);
                if (Target2.transform.localScale == new Vector3(0.4f, 0.4f, 1.0f))
                {
                    Target1Position = new Vector2(Target1Position.x, Target1Position.y - 0.2233912f * 0.75f);
                    Target2Position = new Vector2(Target2Position.x, Target2Position.y + 0.2233912f * 0.75f);
                }

                Player1Body.transform.position = Target2Position;
                Target2.transform.position = Target1Position;
                Target2.NetTransform.SnapTo(Target1Position);

                if (SubmergedCompatibility.IsSubmerged)
                {
                    if (PlayerControl.LocalPlayer.PlayerId == Target2.PlayerId)
                    {
                        SubmergedCompatibility.ChangeFloor(Target2.GetTruePosition().y > -7);
                        SubmergedCompatibility.CheckOutOfBoundsElevator(PlayerControl.LocalPlayer);
                    }
                }
            }
            else if (Player1Body == null && Player2Body != null)
            {
                DropBody(Player2Body.ParentId);
                HitmanDropBody(Player2Body.ParentId);
                Target1.MyPhysics.ResetMoveState();
                var Target1Position = Target1.GetTruePosition();
                Target1Position = new Vector2(Target1Position.x, Target1Position.y + 0.3636f);
                var Target2Position = Player2Body.TruePosition;
                Target2Position = new Vector2(Target2Position.x, Target2Position.y + 0.3636f);
                if (Target1.transform.localScale == new Vector3(0.4f, 0.4f, 1.0f))
                {
                    Target1Position = new Vector2(Target1Position.x, Target1Position.y + 0.2233912f * 0.75f);
                    Target2Position = new Vector2(Target2Position.x, Target2Position.y - 0.2233912f * 0.75f);
                }
                
                Player2Body.transform.position = Target1Position;
                Target1.transform.position = Target2Position;
                Target1.NetTransform.SnapTo(Target2Position);

                if (SubmergedCompatibility.IsSubmerged)
                {
                    if (PlayerControl.LocalPlayer.PlayerId == Target1.PlayerId)
                    {
                        SubmergedCompatibility.ChangeFloor(Target1.GetTruePosition().y > -7);
                        SubmergedCompatibility.CheckOutOfBoundsElevator(PlayerControl.LocalPlayer);
                    }
                }
            }
            else if (Player1Body != null && Player2Body != null)
            {
                DropBody(Player2Body.ParentId);
                HitmanDropBody(Player2Body.ParentId);
                var TempPosition = Player1Body.TruePosition;
                Player1Body.transform.position = Player2Body.TruePosition;
                Player2Body.transform.position = TempPosition;
            }

            if (PlayerControl.LocalPlayer.PlayerId == Target1.PlayerId ||
                PlayerControl.LocalPlayer.PlayerId == Target2.PlayerId)
            {
                Utils.ShowFlash(Landlord.Color);
                if (TaskPanelInstance) TaskPanelInstance.Close();
            }

            Target1.moveable = true;
            Target2.moveable = true;
            Target1.Collider.enabled = true;
            Target2.Collider.enabled = true;
            Target1.NetTransform.enabled = true;
            Target2.NetTransform.enabled = true;
        }

        public static void CleanBody(byte playerId, byte cleaningPlayerId)
        {
            if (Psychic.futureDeadBodies != null)
            {
            var deadBody = Psychic.futureDeadBodies.Find(x => x.Item1.player.PlayerId == playerId).Item1;
            if (deadBody != null) deadBody.WasCleanedOrEaten = true;
            }

            DeadBody[] array = UObject.FindObjectsOfType<DeadBody>();
            for (int i = 0; i < array.Length; i++)
            {
                if (GameData.Instance.GetPlayerById(array[i].ParentId).PlayerId == playerId)
                {
                    UObject.Destroy(array[i].gameObject);
                }
            }
            if (Scavenger.Player != null && cleaningPlayerId == Scavenger.Player.PlayerId)
            {
                Scavenger.eatenBodies++;
                if (Scavenger.eatenBodies == CustomGameOptions.ScavengerNumberToWin)
                {
                    Scavenger.IsScavengerWin = true;
                }
                else if (Scavenger.eatenBodies == CustomGameOptions.ScavengerNumberToWin - 1)
                {
                    foreach (var player in PlayerControl.AllPlayerControls)
                    {
                        if (player != null && !player.Data.Disconnected)
                        {
                            if (player == PlayerControl.LocalPlayer) Utils.ShowFlash(Scavenger.Color, PlaySound: true);
                        }
                    }
                }
            }
        }

        public static void ChronosRewindTime()
        {
            Chronos.isRewinding = true;
            Chronos.Charges--;

            Utils.ShowFlash(Chronos.Color, CustomGameOptions.ChronosRewindTime);

            if (MapBehaviour.Instance) MapBehaviour.Instance.Close();
            if (TaskPanelInstance) TaskPanelInstance.ForceClose();
            PlayerControl.LocalPlayer.moveable = false;
        }
        public static void VeteranAlert() 
        {
            Veteran.AlertActive = true;
            FastDestroyableSingleton<HudManager>.Instance.StartCoroutine(Effects.Lerp(CustomGameOptions.VeteranDuration, new Action<float>((p) => 
            {
                if (p == 1f) Veteran.AlertActive = false;
            })));
        }
        public static void MedicSetShielded(byte ShieldedId) 
        {
            Medic.usedShield = true;
            Medic.Shielded = Utils.GetPlayerById(ShieldedId);
            Medic.futureShielded = null;
        }
        public static void RomanticSetBeloved(byte belovedId) 
        {
            Romantic.HasLover = true;
            Romantic.beloved = Utils.GetPlayerById(belovedId);
        }
        public static void WerewolfMaul()
        {
            var nearbyPlayers = Utils.GetClosestPlayers(Werewolf.Player.GetTruePosition(), CustomGameOptions.WerewolfMaulRadius);

            foreach (var player in nearbyPlayers)
            {
                if (player == Werewolf.Player || player.Data.IsDead
                || (player == Lucky.Player && !Lucky.IsUnlucky)
                || player == Medic.Shielded || player == Crusader.FortifiedPlayer
                || player == MapOptions.FirstPlayerKilled) continue;

                // had to do all these checks because CheckMurderAndKill was giving multiple kill animations???
                Utils.RpcMurderPlayer(Werewolf.Player, player, false);

                Utils.SendRPC(CustomRPC.ShareGhostInfo,
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
                Utils.ShowFlash(Palette.ImpostorRed);
                SoundEffectsManager.Play("morphlingMorph");
                if (TaskPanelInstance)
                {
                    try
                    {
                        TaskPanelInstance.Close();
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
                PlayerControl player = Utils.GetPlayerById(key);
                foreach (var lazy in Lazy.Players)
                {
                    if (player != lazy) player.transform.position = value;
                    if (PlayerControl.LocalPlayer == player && PlayerControl.LocalPlayer != lazy) PlayerControl.LocalPlayer.NetTransform.RpcSnapTo(value);
                }
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
            List<PlayerControl> targets = AllPlayerControls.Where(player => !player.Data.IsDead && !player.Data.Disconnected).ToList();

            HashSet<Vent> vents = UObject.FindObjectsOfType<Vent>().ToHashSet();

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
            Crusader.FortifiedPlayer = Utils.GetPlayerById(fortifiedId);
        }

        public static void MonarchKnight(byte knightedId)
        {
            Monarch.Charges--;
            Monarch.KnightedPlayers.Add(Utils.GetPlayerById(knightedId));
        }

        public static void Confess(byte confessorId)
        {
            if (Oracle.Player == null || Oracle.Player.Data.IsDead) return;

            Oracle.Confessor = Utils.GetPlayerById(confessorId);
            if (Oracle.Confessor == null) return;

            RoleInfo roleInfo = RoleInfo.GetRoleInfoForPlayer(Oracle.Confessor).FirstOrDefault();
            if (roleInfo == null) return;

            bool showsCorrectFaction = UnityEngine.Random.RandomRangeInt(1, 101) <= CustomGameOptions.OracleAccuracy;
            Faction revealedFaction;

            if (showsCorrectFaction)
            {
                revealedFaction = roleInfo.FactionId;
            }
            else
            {
                // Get possible factions
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
            Utils.SendRPC(CustomRPC.Confess, Oracle.Confessor.PlayerId, (int)revealedFaction);

            // Ghost Info
            Utils.SendRPC(CustomRPC.ShareGhostInfo, Oracle.Confessor.PlayerId, (byte)GhostInfoTypes.OracleInfo, results);
        }

        public static void GrenadierFlash() 
        {
            if (Grenadier.Player == null) return;

            foreach (PlayerControl player in PlayerControl.AllPlayerControls)
            {
                if (player == null || player.Data == null) continue;
                Grenadier.ClosestPlayers = Utils.GetClosestPlayers(Grenadier.Player.GetTruePosition(), CustomGameOptions.GrenadeRadius);
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

                        hud.StartCoroutine(Effects.Lerp(CustomGameOptions.GrenadeDuration, (Action<float>)(p =>
                        {
                            hud.FullScreen.color = Color.Lerp(hud.FullScreen.color, targetColor, p);
                        })));
                    }

                        hud.FullScreen.enabled = true;
                        hud.FullScreen.gameObject.SetActive(true);
                        hud.StartCoroutine(Effects.Lerp(CustomGameOptions.GrenadeDuration, (Action<float>)(p =>
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
            if (CustomGameOptions.GrenadeDuration > 0.5f)
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
            
            bool isShieldedAndShow = Medic.Shielded == PlayerControl.LocalPlayer && CustomGameOptions.MedicShowMurderAttempt == NotificationOptions.Shielded;
            isShieldedAndShow = isShieldedAndShow && (Medic.meetingAfterShielding || !Medic.showShieldAfterMeeting);  // Dont show attempt, if Shield is not shown yet
            bool isMedicAndShow = Medic.Player == PlayerControl.LocalPlayer && CustomGameOptions.MedicShowMurderAttempt == NotificationOptions.Medic;

            if (isShieldedAndShow || isMedicAndShow || Utils.ShouldShowGhostInfo()) Utils.ShowFlash(Palette.ImpostorRed, PlaySound: true);
        }

        public static void FortifiedMurderAttempt() 
        {
            if (Crusader.FortifiedPlayer == null || Crusader.Player == null) return;

            if (Crusader.FortifiedPlayer == PlayerControl.LocalPlayer || Crusader.Player == PlayerControl.LocalPlayer || Utils.ShouldShowGhostInfo()) Utils.ShowFlash(Palette.ImpostorRed, PlaySound: true);
        }

        public static void MorphlingMorph(byte playerId) 
        {
            PlayerControl target = Utils.GetPlayerById(playerId);
            if (Morphling.Player == null || target == null) return;

            Morphling.morphTimer = CustomGameOptions.MorphlingDuration;
            Morphling.morphTarget = target;
            if (Painter.PaintTimer <= 0f)
                Morphling.Player.SetLook(target.Data.PlayerName, target.Data.DefaultOutfit.ColorId, target.Data.DefaultOutfit.HatId, target.Data.DefaultOutfit.VisorId, target.Data.DefaultOutfit.SkinId, target.Data.DefaultOutfit.PetId);
        }

        public static void GlitchMimic(byte playerId) 
        {
            PlayerControl target = Utils.GetPlayerById(playerId);
            if (Glitch.Player == null || target == null) return;

            Glitch.MimicTimer = CustomGameOptions.GlitchMimicDuration;
            Glitch.MimicTarget = target;
            if (Painter.PaintTimer <= 0f)
                Glitch.Player.SetLook(target.Data.PlayerName, target.Data.DefaultOutfit.ColorId, target.Data.DefaultOutfit.HatId, target.Data.DefaultOutfit.VisorId, target.Data.DefaultOutfit.SkinId, target.Data.DefaultOutfit.PetId);
        }

        public static void HitmanMorph(byte playerId) 
        {
            PlayerControl target = Utils.GetPlayerById(playerId);
            if (Hitman.Player == null || target == null) return;

            Hitman.MorphTimer = CustomGameOptions.HitmanMorphDuration;
            Hitman.MorphTarget = target;
            if (Painter.PaintTimer <= 0f)
                Hitman.Player.SetLook(target.Data.PlayerName, target.Data.DefaultOutfit.ColorId, target.Data.DefaultOutfit.HatId, target.Data.DefaultOutfit.VisorId, target.Data.DefaultOutfit.SkinId, target.Data.DefaultOutfit.PetId);
        }

        public static void PainterPaint() 
        {
            if (Painter.Player == null) return;

            Painter.PaintTimer = CustomGameOptions.PainterDuration;
            if (Utils.MushroomSabotageActive()) return; // Dont overwrite the fungle "camo"

            List<int> availableColors = Enumerable.Range(0, Palette.PlayerColors.Count).ToList();
            System.Random rng = new System.Random();
            availableColors = availableColors.OrderBy(x => rng.Next()).ToList();
            int index = 0;

            int randomColorId = rng.Next(Palette.PlayerColors.Count); // full color range
            foreach (PlayerControl player in PlayerControl.AllPlayerControls)
            {
                // Assign a different color to each player
                int randomColor = availableColors[index % availableColors.Count];
                player.SetLook("", randomColor, "", "", "", "");
                index++;
            }
        }

        public static void ViperSetPoisoned(byte targetId, byte performReset) 
        {
            if (performReset != 0) 
            {
                Viper.poisoned = null;
                return;
            }

            if (Viper.Player == null) return;
            foreach (PlayerControl player in PlayerControl.AllPlayerControls) 
            {
                if (player.PlayerId == targetId && !player.Data.IsDead) 
                {
                        Viper.poisoned = player;
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
            PlayerControl player = Utils.GetPlayerById(targetId);
            Blackmailer.BlackmailedPlayer = player;
        }

        public static void GlitchUsedHacks(byte targetId)
        {
            Glitch.remainingHacks--;
            Glitch.HackedPlayers.Add(targetId);
        }
        
        public static void BecomeCrewmate(PlayerControl player)
        {
            RPCProcedure.ErasePlayerRoles(player.PlayerId);
            player.roleAssigned = false;
            DestroyableSingleton<RoleManager>.Instance.SetRole(player, RoleTypes.Crewmate);
            player.Data.Role.TeamType = RoleTeamTypes.Crewmate;
            player.roleAssigned = true;
            foreach (PlayerControl otherPlayer in PlayerControl.AllPlayerControls.GetFastEnumerator())
                if (!otherPlayer.Data.Role.IsImpostor && PlayerControl.LocalPlayer.Data.Role.IsImpostor)
                    player.cosmetics.nameText.color = Palette.White;
        }
        
        // Taken from Town of Us https://github.com/eDonnes124/Town-Of-Us-R/blob/master/source/Patches/NeutralRoles/AmnesiacMod/PerformKillButton.cs Licensed under GPLv3
        public static void BecomeImpostor(PlayerControl player)
        {
            player.Data.Role.TeamType = RoleTeamTypes.Impostor;
            RoleManager.Instance.SetRole(player, RoleTypes.Impostor);
            player.SetKillTimer(GameOptionsManager.Instance.currentNormalGameOptions.KillCooldown);
            foreach (var player2 in PlayerControl.AllPlayerControls)
            {
                if (player2.Data.Role.IsImpostor && PlayerControl.LocalPlayer.Data.Role.IsImpostor)
                {
                    player.cosmetics.nameText.color = Palette.ImpostorRed;
                }
            }
        }
        
        public static void ErasePlayerRoles(byte playerId)
        {
            PlayerControl player = Utils.GetPlayerById(playerId);
            if (player == null) return;

            // Crewmate roles
            if (player == Mayor.Player) Mayor.ClearAndReload();
            if (player == Gatekeeper.Player) Gatekeeper.ClearAndReload();
            if (player == Engineer.Player) Engineer.ClearAndReload();
            if (player == Sheriff.Player) Sheriff.ClearAndReload();
            if (player == Oracle.Player) Oracle.ClearAndReload();
            if (player == Snitch.Player) Snitch.ClearAndReload();
            if (player == Monarch.Player) Monarch.ClearAndReload();
            if (player == Detective.Player) Detective.ClearAndReload();
            if (player == Chronos.Player) Chronos.ClearAndReload();
            if (player == Veteran.Player) Veteran.ClearAndReload();
            if (player == Deputy.Player) Deputy.ClearAndReload();
            if (player == Medic.Player) Medic.ClearAndReload();
            if (player == Mystic.Player) Mystic.ClearAndReload();
            if (player == Hacker.Player) Hacker.ClearAndReload();
            if (player == Tracker.Player) Tracker.ClearAndReload();
            if (player == Landlord.Player) Landlord.ClearAndReload();
            if (player == Spy.Player) Spy.ClearAndReload();
            if (player == Crusader.Player) Crusader.ClearAndReload();
            if (player == Vigilante.Player) Vigilante.ClearAndReload();
            if (player == Psychic.Player) Psychic.ClearAndReload();
            if (player == Trapper.Player) Trapper.ClearAndReload();

            // Impostor roles
            if (player == Morphling.Player) Morphling.ClearAndReload();
            if (player == Painter.Player) Painter.ClearAndReload();
            if (player == Viper.Player) Viper.ClearAndReload();
            if (player == Trickster.Player) Trickster.ClearAndReload();
            if (player == Undertaker.Player) Undertaker.ClearAndReload();
            if (player == Janitor.Player) Janitor.ClearAndReload();
            if (player == Blackmailer.Player) Blackmailer.ClearAndReload();
            if (player == Warlock.Player) Warlock.ClearAndReload();
            if (player == Witch.Player) Witch.ClearAndReload();
            if (player == Miner.Player) Miner.ClearAndReload();
            if (player == Assassin.Player) Assassin.ClearAndReload();
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

            // Passive Neutral Roles
            if (player == Jester.Player) Jester.ClearAndReload();
            if (player == Scavenger.Player) Scavenger.ClearAndReload();
            if (player == Lawyer.Player) Lawyer.ClearAndReload();
            if (player == Arsonist.Player) Arsonist.ClearAndReload();
            if (player == Amnesiac.Player) Amnesiac.ClearAndReload();
            if (player == Romantic.Player) Romantic.ClearAndReload();
            if (player == Survivor.Player) Survivor.ClearAndReload();

            Utils.ClearAllRoleTexts();
            PlayerControlFixedUpdatePatch.UpdatePlayerInfoText(player);
        }

        public static void SetFutureShielded(byte playerId) 
        {
            Medic.futureShielded = Utils.GetPlayerById(playerId);
            Medic.usedShield = true;
        }

        public static void SetFutureSpelled(byte playerId) 
        {
            PlayerControl player = Utils.GetPlayerById(playerId);
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
        
        public static void RemoveMedicShield() 
        {
            Medic.Shielded = null;
            Medic.futureShielded = null;
            Medic.usedShield = false;
        }

        public static void PlaceAssassinTrace(byte[] buff)
        {
            Vector3 position = Vector3.zero;
            position.x = BitConverter.ToSingle(buff, 0 * sizeof(float));
            position.y = BitConverter.ToSingle(buff, 1 * sizeof(float));
            new AssassinTrace(position, CustomGameOptions.AssassinTraceTime);
            if (PlayerControl.LocalPlayer != Assassin.Player)
                Assassin.AssassinMarked = null;
        }

        public static void SetVanish(byte playerId, byte flag)
        {
            PlayerControl target = Utils.GetPlayerById(playerId);
            if (target == null) return;
            if (flag == byte.MaxValue)
            {
                target.cosmetics.currentBodySprite.BodySprite.color = Color.white;
                target.cosmetics.colorBlindText.gameObject.SetActive(DataManager.Settings.Accessibility.ColorBlindMode);
                target.cosmetics.colorBlindText.color = target.cosmetics.colorBlindText.color.SetAlpha(1f);

                if (Painter.PaintTimer <= 0 && !Utils.MushroomSabotageActive()) target.SetDefaultLook();
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
            Wraith.VanishTimer = CustomGameOptions.WraithDuration;
            Wraith.IsVanished = true;
        }

        public static void SetInvisible(byte playerId, byte flag)
        {
            PlayerControl target = Utils.GetPlayerById(playerId);
            if (target == null) return;
            if (flag == byte.MaxValue)
            {
                target.cosmetics.currentBodySprite.BodySprite.color = Color.white;
                target.cosmetics.colorBlindText.gameObject.SetActive(DataManager.Settings.Accessibility.ColorBlindMode);
                target.cosmetics.colorBlindText.color = target.cosmetics.colorBlindText.color.SetAlpha(1f);

                if (Painter.PaintTimer <= 0 && !Utils.MushroomSabotageActive()) target.SetDefaultLook();
                Assassin.isInvisble = false;
                return;
            }

            target.SetLook("", 6, "", "", "", "");
            Color color = Color.clear;
            bool canSee = PlayerControl.LocalPlayer.Data.Role.IsImpostor || PlayerControl.LocalPlayer.Data.IsDead;
            if (canSee) color.a = 0.1f;
            target.cosmetics.currentBodySprite.BodySprite.color = color;
            target.cosmetics.colorBlindText.gameObject.SetActive(false);
            target.cosmetics.colorBlindText.color = target.cosmetics.colorBlindText.color.SetAlpha(canSee ? 0.1f : 0f);
            Assassin.invisibleTimer = CustomGameOptions.AssassinInvisibleDuration;
            Assassin.isInvisble = true;
        }

        public static void PlacePortal(byte[] buff) 
        {
            Vector3 position = Vector2.zero;
            position.x = BitConverter.ToSingle(buff, 0 * sizeof(float));
            position.y = BitConverter.ToSingle(buff, 1 * sizeof(float));
            _ = new Portal(position);
        }

        public static void PlaceMine(byte[] buff) 
        {
            Vector3 position = Vector2.zero;
            position.x = BitConverter.ToSingle(buff, 0 * sizeof(float));
            position.y = BitConverter.ToSingle(buff, 1 * sizeof(float));
            _ = new MinerVent(position);
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
            Trickster.lightsOutTimer = CustomGameOptions.TricksterLightsOutDuration;
            // If the local player is impostor indicate lights out
            if(Utils.HasImpVision(GameData.Instance.GetPlayerById(PlayerControl.LocalPlayer.PlayerId)))
            {
                _ = new CustomMessage("Lights are out", CustomGameOptions.TricksterLightsOutDuration);
            }
        }

        public static void PlaceCamera(byte[] buff) 
        {
            var referenceCamera = UObject.FindObjectOfType<SurvCamera>(); 
            if (referenceCamera == null) return; // Mira HQ

            Vigilante.remainingScrews -= Vigilante.camPrice;
            Vigilante.placedCameras++;

            Vector3 position = Vector3.zero;
            position.x = BitConverter.ToSingle(buff, 0*sizeof(float));
            position.y = BitConverter.ToSingle(buff, 1*sizeof(float));

            var camera = UObject.Instantiate<SurvCamera>(referenceCamera);
            camera.transform.position = new Vector3(position.x, position.y, referenceCamera.transform.position.z - 1f);
            camera.CamName = $"Security Camera {Vigilante.placedCameras}";
            camera.Offset = new Vector3(0f, 0f, camera.Offset.z);
            if (GameOptionsManager.Instance.currentNormalGameOptions.MapId == 2 || GameOptionsManager.Instance.currentNormalGameOptions.MapId == 4) camera.transform.localRotation = new Quaternion(0, 0, 1, 1); // Polus and Airship 

            if (SubmergedCompatibility.IsSubmerged) 
            {
                // remove 2d box collider of console, so that no barrier can be created. (irrelevant for now, but who knows... maybe we need it later)
                var fixConsole = camera.transform.FindChild("FixConsole");
                if (fixConsole != null)
                {
                    var boxCollider = fixConsole.GetComponent<BoxCollider2D>();
                    if (boxCollider != null) UObject.Destroy(boxCollider);
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
                Sprite newSprite = animator == null ? Utils.GetSprite("StaticVentSealed", 160f) : Vigilante.GetAnimatedVentSealedSprite();
                SpriteRenderer rend = vent.myRend;
                if (IsFungle())
                {
                    newSprite = Utils.GetSprite("FungleVentSealed", 160f);  
                    rend = vent.transform.GetChild(3).GetComponent<SpriteRenderer>();
                    animator = vent.transform.GetChild(3).GetComponent<PowerTools.SpriteAnim>();
                }
                animator?.Stop();
                rend.sprite = newSprite;
                if (SubmergedCompatibility.IsSubmerged && vent.Id == 0) vent.myRend.sprite = Utils.GetSprite("CentralUpperBlocked", 145f);
                if (SubmergedCompatibility.IsSubmerged && vent.Id == 14) vent.myRend.sprite = Utils.GetSprite("CentralLowerBlocked", 145f);
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
            Lawyer.target = Utils.GetPlayerById(playerId);
        }

        public static void LawyerChangeRole() 
        {
            PlayerControl player = Lawyer.Player;
            PlayerControl client = Lawyer.target;
            Lawyer.ClearAndReload(false);

            Survivor.Player = player;
            var newRole = RoleInfo.GetRoleInfoForPlayer(player).FirstOrDefault();
            if (newRole != null && newRole != RoleInfo.survivor) GameHistory.AddToRoleHistory(player.PlayerId, newRole);
        }
        public static void ProsecutorSetTarget(byte playerId) 
        {
            Prosecutor.target = Utils.GetPlayerById(playerId);
        }

        public static void ProsecutorChangeRole()
        {
            PlayerControl player = Prosecutor.Player;
            Prosecutor.ClearAndReload();

            RoleInfo newRoleInfo = null;

            switch (CustomGameOptions.ProsecutorBecomeEnum)
            {
                case ProsecutorOnTargetDeath.Jester:
                    Jester.Player = player;
                    newRoleInfo = RoleInfo.jester;
                    break;
                case ProsecutorOnTargetDeath.Amnesiac:
                    Amnesiac.Player = player;
                    newRoleInfo = RoleInfo.amnesiac;
                    break;
                default:
                    Survivor.Player = player;
                    newRoleInfo = RoleInfo.survivor;
                    break;
            }

            var currentRole = RoleInfo.GetRoleInfoForPlayer(player).FirstOrDefault();
            if (newRoleInfo != null && currentRole != newRoleInfo)
                GameHistory.AddToRoleHistory(player.PlayerId, newRoleInfo);
        }

        public static void PlaguebearerTurnPestilence() 
        {
            PlayerControl player = Plaguebearer.Player;
            Plaguebearer.ClearAndReload();

            Pestilence.Player = player;
            
            var newRole = RoleInfo.GetRoleInfoForPlayer(player).FirstOrDefault();
            if (newRole != null && newRole != RoleInfo.plaguebearer) GameHistory.AddToRoleHistory(player.PlayerId, newRole);
            
            if (player == PlayerControl.LocalPlayer)
            {
                Utils.ShowFlash(Pestilence.Color, PlaySound: true);
                Utils.ShowTextToast("You just transformed into the Pestilence!", 2.5f);
            }
        }

        public static void AgentTurnIntoHitman() 
        {
            PlayerControl player = Agent.Player;
            Agent.ClearAndReload();

            Hitman.Player = player;
            
            var newRole = RoleInfo.GetRoleInfoForPlayer(player).FirstOrDefault();
            if (newRole != null && newRole != RoleInfo.agent) GameHistory.AddToRoleHistory(player.PlayerId, newRole);

            if (player == PlayerControl.LocalPlayer)
            {
                Utils.ShowFlash(Hitman.Color, PlaySound: true);
                Utils.ShowTextToast("You just became the Hitman!", 2.5f);
            }
        }
        public static void DragBody(byte BodyId)
        {
            DeadBody[] array = UObject.FindObjectsOfType<DeadBody>();
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
            DeadBody[] array = UObject.FindObjectsOfType<DeadBody>();
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
            Romantic.ClearAndReload();

            VengefulRomantic.Player = player;
            var newRole = RoleInfo.GetRoleInfoForPlayer(player).FirstOrDefault();
            if (newRole != null && newRole != RoleInfo.romantic) GameHistory.AddToRoleHistory(player.PlayerId, newRole);
            VengefulRomantic.Lover = target;
        }
        public static void DeputyShoot(byte killerId, byte dyingTargetId) 
        {
            Deputy.CanExecute = false; // only once per meeting
            foreach (var button in Deputy.ExecuteButtons)
            {
                UObject.Destroy(button);
            }
            Deputy.ExecuteButtons.Clear();
            Deputy.Charges--;

            PlayerControl dyingTarget = Utils.GetPlayerById(dyingTargetId);
            if (dyingTarget == null ) return;
            if (Lawyer.target != null && dyingTarget == Lawyer.target) Lawyer.targetWasGuessed = true;  // Lawyer shouldn't be exiled with the client for executes
            PlayerControl dyingLoverPartner = CustomGameOptions.ModifierLoverBothDie ? dyingTarget.GetPartner() : null; // Lover check
            if (Lawyer.target != null && dyingLoverPartner == Lawyer.target) Lawyer.targetWasGuessed = true;  // Lawyer shouldn't be exiled with the client for executes

            PlayerControl deputy = Utils.GetPlayerById(killerId);
            if (dyingTarget.IsKiller() || dyingTarget.IsNeutralEvil() && CustomGameOptions.DeputyCanKillNeutralEvil || dyingTarget.IsNeutralBenign() && CustomGameOptions.DeputyCanKillNeutralBenign)
            {
                dyingTarget.Exiled();
            }
            else
            {
                deputy.Exiled();
                dyingTarget = deputy;
            }

            GameHistory.CreateDeathReason(dyingTarget, DeadPlayer.CustomDeathReason.Execute, deputy);
            byte partnerId = dyingLoverPartner != null ? dyingLoverPartner.PlayerId : dyingTargetId;

            Guesser.RemainingShots(killerId, true);
            if (Constants.ShouldPlaySfx()) SoundManagerInstance().PlaySound(dyingTarget.KillSfx, false, 0.8f);
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
                    }

                    //Give players back their vote if target is shot dead
                    if (pva.VotedFor != dyingTargetId && pva.VotedFor != partnerId) continue;
                    pva.UnsetVote();
                    var voteAreaPlayer = Utils.GetPlayerById(pva.TargetPlayerId);
                    if (!voteAreaPlayer.AmOwner) continue;
                    MeetingHud.Instance.ClearVote();

                }

                if (AmongUsClient.Instance.AmHost) 
                    MeetingHud.Instance.CheckForEndVoting();
            }
            if (FastDestroyableSingleton<HudManager>.Instance != null && deputy != null)
                if (PlayerControl.LocalPlayer == dyingTarget) 
                {
                    FastDestroyableSingleton<HudManager>.Instance.KillOverlay.ShowKillAnimation(deputy.Data, dyingTarget.Data);
                    if (MeetingHudPatch.GuesserUI != null) MeetingHudPatch.GuesserUIExitButton.OnClick.Invoke();
                }
                else if (dyingLoverPartner != null && PlayerControl.LocalPlayer == dyingLoverPartner) 
                {
                    FastDestroyableSingleton<HudManager>.Instance.KillOverlay.ShowKillAnimation(dyingLoverPartner.Data, dyingLoverPartner.Data);
                    if (MeetingHudPatch.GuesserUI != null) MeetingHudPatch.GuesserUIExitButton.OnClick.Invoke();
                }

            // remove shoot button from targets for all Guesser and close their GuesserUI
            if (Guesser.IsGuesser(PlayerControl.LocalPlayer.PlayerId) && !PlayerControl.LocalPlayer.Data.IsDead && Guesser.RemainingShots(PlayerControl.LocalPlayer.PlayerId) > 0 && MeetingHud.Instance)
            {
                MeetingHud.Instance.playerStates.ToList().ForEach(x => { if (x.TargetPlayerId == dyingTarget.PlayerId && x.transform.FindChild("DeputyButton") != null) UObject.Destroy(x.transform.FindChild("DeputyButton").gameObject); });
                if (dyingLoverPartner != null)
                    MeetingHud.Instance.playerStates.ToList().ForEach(x => { if (x.TargetPlayerId == dyingLoverPartner.PlayerId && x.transform.FindChild("DeputyButton") != null) UObject.Destroy(x.transform.FindChild("DeputyButton").gameObject); });

                if (MeetingHudPatch.GuesserUI != null && MeetingHudPatch.GuesserUIExitButton != null)
                {
                    if (MeetingHudPatch.guesserCurrentTarget == dyingTarget.PlayerId)
                        MeetingHudPatch.GuesserUIExitButton.OnClick.Invoke();
                    else if (dyingLoverPartner != null && MeetingHudPatch.guesserCurrentTarget == dyingLoverPartner.PlayerId)
                        MeetingHudPatch.GuesserUIExitButton.OnClick.Invoke();
                }
            }

            if (PlayerControl.LocalPlayer.Data.IsDead && dyingTarget != null && deputy != null) 
            {
                bool isWrong = dyingTarget.IsCrew() ? true : false;
                string msg = isWrong? $"{deputy.Data.PlayerName} tried to execute {dyingTarget.Data.PlayerName} and failed!": $"{deputy.Data.PlayerName} executed {dyingTarget.Data.PlayerName} correctly!";
              
                if (AmongUsClient.Instance.AmClient && FastDestroyableSingleton<HudManager>.Instance)
                    FastDestroyableSingleton<HudManager>.Instance.Chat.AddChat(deputy, msg);
                if (msg.IndexOf("who", StringComparison.OrdinalIgnoreCase) >= 0)
                    FastDestroyableSingleton<UnityTelemetry>.Instance.SendWho();
            }
        }
        public static void GuesserShoot(byte killerId, byte dyingTargetId, byte guessedTargetId, byte guessedRoleId)
        {
            PlayerControl dyingTarget = Utils.GetPlayerById(dyingTargetId);
            if (dyingTarget == null ) return;
            if (Lawyer.target != null && dyingTarget == Lawyer.target) Lawyer.targetWasGuessed = true;  // Lawyer shouldn't be exiled with the client for guesses
            PlayerControl dyingLoverPartner = CustomGameOptions.ModifierLoverBothDie ? dyingTarget.GetPartner() : null; // Lover check
            if (Lawyer.target != null && dyingLoverPartner == Lawyer.target) Lawyer.targetWasGuessed = true;  // Lawyer shouldn't be exiled with the client for guesses

            PlayerControl guesser = Utils.GetPlayerById(killerId);
            dyingTarget.Exiled();
            GameHistory.CreateDeathReason(dyingTarget, DeadPlayer.CustomDeathReason.Guess, guesser);
            byte partnerId = dyingLoverPartner != null ? dyingLoverPartner.PlayerId : dyingTargetId;

            Guesser.RemainingShots(killerId, true);
            if (Constants.ShouldPlaySfx()) SoundManagerInstance().PlaySound(dyingTarget.KillSfx, false, 0.8f);
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
                    }

                    //Give players back their vote if target is shot dead
                    if (pva.VotedFor != dyingTargetId && pva.VotedFor != partnerId) continue;
                    pva.UnsetVote();
                    var voteAreaPlayer = Utils.GetPlayerById(pva.TargetPlayerId);
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
                MeetingHud.Instance.playerStates.ToList().ForEach(x => { if (x.TargetPlayerId == dyingTarget.PlayerId && x.transform.FindChild("ShootButton") != null) UObject.Destroy(x.transform.FindChild("ShootButton").gameObject); });
                if (dyingLoverPartner != null)
                    MeetingHud.Instance.playerStates.ToList().ForEach(x => { if (x.TargetPlayerId == dyingLoverPartner.PlayerId && x.transform.FindChild("ShootButton") != null) UObject.Destroy(x.transform.FindChild("ShootButton").gameObject); });

                if (MeetingHudPatch.GuesserUI != null && MeetingHudPatch.GuesserUIExitButton != null) 
                {
                    if (MeetingHudPatch.guesserCurrentTarget == dyingTarget.PlayerId)
                        MeetingHudPatch.GuesserUIExitButton.OnClick.Invoke();
                    else if (dyingLoverPartner != null && MeetingHudPatch.guesserCurrentTarget == dyingLoverPartner.PlayerId)
                        MeetingHudPatch.GuesserUIExitButton.OnClick.Invoke();
                }
            }

            if (dyingTarget == Deputy.Player) 
            {
                Deputy.CanExecute = false;
                foreach (var button in Deputy.ExecuteButtons)
                {
                    UObject.Destroy(button);
                }
                Deputy.ExecuteButtons.Clear();
            }

            PlayerControl guessedTarget = Utils.GetPlayerById(guessedTargetId);
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
        public static void DisableVanillaRoles()
        {
            GameOptionsManager.Instance.currentNormalGameOptions.RoleOptions.SetRoleRate(RoleTypes.Scientist, 0, 0);
            GameOptionsManager.Instance.currentNormalGameOptions.RoleOptions.SetRoleRate(RoleTypes.Engineer, 0, 0);
            GameOptionsManager.Instance.currentNormalGameOptions.RoleOptions.SetRoleRate(RoleTypes.GuardianAngel, 0, 0);
            GameOptionsManager.Instance.currentNormalGameOptions.RoleOptions.SetRoleRate(RoleTypes.Tracker, 0, 0);
            GameOptionsManager.Instance.currentNormalGameOptions.RoleOptions.SetRoleRate(RoleTypes.Noisemaker, 0, 0);
            GameOptionsManager.Instance.currentNormalGameOptions.RoleOptions.SetRoleRate(RoleTypes.Shapeshifter, 0, 0);
            GameOptionsManager.Instance.currentNormalGameOptions.RoleOptions.SetRoleRate(RoleTypes.Phantom, 0, 0);
        }

        public static void SetBlanked(byte playerId, byte value)
        {
            PlayerControl target = Utils.GetPlayerById(playerId);
            if (target == null) return;
            Survivor.blankedList.RemoveAll(x => x.PlayerId == playerId);
            if (value > 0) Survivor.blankedList.Add(target);
        }
        public static void SetFirstKill(byte playerId) 
        {
            PlayerControl target = Utils.GetPlayerById(playerId);
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

            PlayerControl target = Utils.GetPlayerById(targetId);
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
                    Utils.BecomeCrewmate(target);
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
                    Utils.BecomeCrewmate(target);
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

                case RoleId.Gatekeeper:
                    Gatekeeper.ClearAndReload();
                    Gatekeeper.Player = AmnesiacPlayer;
                    Amnesiac.ClearAndReload();
                    Amnesiac.Player = target;
                    break;
                
                case RoleId.Snitch:
                    Snitch.ClearAndReload();
                    Snitch.Player = AmnesiacPlayer;
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

                case RoleId.Detective:
                    Detective.ClearAndReload();
                    Detective.Player = AmnesiacPlayer;
                    Amnesiac.ClearAndReload();
                    Amnesiac.Player = target;
                    break;

                case RoleId.Chronos:
                    Chronos.ClearAndReload();
                    Chronos.Player = AmnesiacPlayer;
                    Amnesiac.ClearAndReload();
                    Amnesiac.Player = target;
                    break;

                case RoleId.Veteran:
                    Veteran.ClearAndReload();
                    Veteran.Player = AmnesiacPlayer;
                    Amnesiac.ClearAndReload();
                    Amnesiac.Player = target;
                    break;

                case RoleId.Deputy:
                    Deputy.ClearAndReload();
                    Deputy.Player = AmnesiacPlayer;
                    Amnesiac.ClearAndReload();
                    Amnesiac.Player = target;
                    break;

                case RoleId.Medic:
                    Medic.ClearAndReload();
                    Medic.Player = AmnesiacPlayer;
                    Amnesiac.ClearAndReload();
                    Amnesiac.Player = target;
                    break;

                case RoleId.Grenadier:
                    Utils.BecomeImpostor(Amnesiac.Player);
                    Utils.BecomeCrewmate(target);
                    Grenadier.ClearAndReload();
                    Grenadier.Player = AmnesiacPlayer;
                    Amnesiac.ClearAndReload();
                    Amnesiac.Player = target;
                    break;

                case RoleId.Blackmailer:
                    Utils.BecomeImpostor(Amnesiac.Player);
                    Utils.BecomeCrewmate(target);
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
                    Utils.BecomeCrewmate(target);
                    Morphling.ClearAndReload();
                    Morphling.Player = AmnesiacPlayer;
                    Amnesiac.ClearAndReload();
                    Amnesiac.Player = target;
                    break;

                case RoleId.Yoyo:
                    Utils.BecomeImpostor(Amnesiac.Player);
                    Utils.BecomeCrewmate(target);
                    Yoyo.ClearAndReload();
                    Yoyo.Player = AmnesiacPlayer;
                    Amnesiac.ClearAndReload();
                    Amnesiac.Player = target;
                    break;

                case RoleId.Painter:
                    Utils.BecomeImpostor(Amnesiac.Player);
                    Utils.BecomeCrewmate(target);
                    Painter.ClearAndReload();
                    Painter.Player = AmnesiacPlayer;
                    Amnesiac.ClearAndReload();
                    Amnesiac.Player = target;
                    break;

                case RoleId.Hacker:
                    Hacker.ClearAndReload();
                    Hacker.Player = AmnesiacPlayer;
                    Amnesiac.ClearAndReload();
                    Amnesiac.Player = target;
                    break;

                case RoleId.Landlord:
                    Landlord.ClearAndReload();
                    Landlord.Player = AmnesiacPlayer;
                    Amnesiac.ClearAndReload();
                    Amnesiac.Player = target;
                    break;

                case RoleId.Tracker:
                    Tracker.ClearAndReload();
                    Tracker.Player = AmnesiacPlayer;
                    Amnesiac.ClearAndReload();
                    Amnesiac.Player = target;
                    break;

                case RoleId.Viper:
                    Utils.BecomeImpostor(Amnesiac.Player);
                    Utils.BecomeCrewmate(target);
                    Viper.ClearAndReload();
                    Viper.Player = AmnesiacPlayer;
                    Amnesiac.ClearAndReload();
                    Amnesiac.Player = target;
                    break;

                case RoleId.Glitch:
                    Glitch.ClearAndReload();
                    Glitch.Player = AmnesiacPlayer;
                    Amnesiac.ClearAndReload();
                    Amnesiac.Player = target;
                    break;
                
                case RoleId.Monarch:
                    Monarch.ClearAndReload();
                    Monarch.Player = AmnesiacPlayer;
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
                    Utils.BecomeCrewmate(target);
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

                case RoleId.Spy:
                    Spy.ClearAndReload();
                    Spy.Player = AmnesiacPlayer;
                    Amnesiac.ClearAndReload();
                    Amnesiac.Player = target;
                    break;

                case RoleId.Trickster:
                    Utils.BecomeImpostor(Amnesiac.Player);
                    Utils.BecomeCrewmate(target);
                    Trickster.ClearAndReload();
                    Trickster.Player = AmnesiacPlayer;
                    Amnesiac.ClearAndReload();
                    Amnesiac.Player = target;
                    break;

                case RoleId.Janitor:
                    Utils.BecomeImpostor(Amnesiac.Player);
                    Utils.BecomeCrewmate(target);
                    Janitor.ClearAndReload();
                    Janitor.Player = AmnesiacPlayer;
                    Amnesiac.ClearAndReload();
                    Amnesiac.Player = target;
                    break;

                case RoleId.Warlock:
                    Utils.BecomeImpostor(Amnesiac.Player);
                    Utils.BecomeCrewmate(target);
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
                            if (MapOptions.BeanIcons.ContainsKey(p.PlayerId) && p != Arsonist.Player)
                            {
                                //Arsonist.poolIcons.Add(p);
                                if (Arsonist.dousedPlayers.Contains(p))
                                {
                                    MapOptions.BeanIcons[p.PlayerId].SetSemiTransparent(false);
                                }
                                else
                                {
                                    MapOptions.BeanIcons[p.PlayerId].SetSemiTransparent(true);
                                }

                                MapOptions.BeanIcons[p.PlayerId].transform.localPosition = bottomLeft + new Vector3(-0.25f, -0.25f, 0) + (Vector3.right * playerCounter++ * 0.35f);
                                MapOptions.BeanIcons[p.PlayerId].transform.localScale = Vector3.one * 0.2f;
                                MapOptions.BeanIcons[p.PlayerId].gameObject.SetActive(true);
                            }
                        }
                    }
                    break;

                case RoleId.BountyHunter:
                    Utils.BecomeImpostor(Amnesiac.Player);
                    Utils.BecomeCrewmate(target);
                    BountyHunter.ClearAndReload();
                    BountyHunter.Player = AmnesiacPlayer;
                    Amnesiac.ClearAndReload();
                    Amnesiac.Player = target;

                    BountyHunter.bountyUpdateTimer = 0f;
                    if (PlayerControl.LocalPlayer == BountyHunter.Player)
                    {
                        Vector3 bottomLeft = new Vector3(-FastDestroyableSingleton<HudManager>.Instance.UseButton.transform.localPosition.x, FastDestroyableSingleton<HudManager>.Instance.UseButton.transform.localPosition.y, FastDestroyableSingleton<HudManager>.Instance.UseButton.transform.localPosition.z) + new Vector3(-0.25f, 1f, 0);
                        BountyHunter.CooldownText = UObject.Instantiate(FastDestroyableSingleton<HudManager>.Instance.KillButton.cooldownTimerText, FastDestroyableSingleton<HudManager>.Instance.transform);
                        BountyHunter.CooldownText.alignment = TextAlignmentOptions.Center;
                        BountyHunter.CooldownText.transform.localPosition = bottomLeft + new Vector3(0f, -1f, -1f);
                        BountyHunter.CooldownText.gameObject.SetActive(true);

                        foreach (PlayerControl p in PlayerControl.AllPlayerControls)
                        {
                            if (MapOptions.BeanIcons.ContainsKey(p.PlayerId))
                            {
                                MapOptions.BeanIcons[p.PlayerId].SetSemiTransparent(false);
                                MapOptions.BeanIcons[p.PlayerId].transform.localPosition = bottomLeft + new Vector3(0f, -1f, 0);
                                MapOptions.BeanIcons[p.PlayerId].transform.localScale = Vector3.one * 0.4f;
                                MapOptions.BeanIcons[p.PlayerId].gameObject.SetActive(false);
                            }
                        }
                    }
                    break;

                case RoleId.Scavenger:
                    Scavenger.ClearAndReload();
                    Scavenger.Player = AmnesiacPlayer;
                    Amnesiac.ClearAndReload();
                    Amnesiac.Player = target;
                    break;

                case RoleId.Psychic:
                    Psychic.ClearAndReload();
                    Psychic.Player = AmnesiacPlayer;
                    Amnesiac.ClearAndReload();
                    Amnesiac.Player = target;
                    break;

                case RoleId.Lawyer:
                    Lawyer.Player = AmnesiacPlayer;
                    Amnesiac.ClearAndReload();
                    Amnesiac.Player = target;
                    break;

                case RoleId.Survivor:
                    Survivor.ClearAndReload();
                    Survivor.Player = AmnesiacPlayer;
                    Amnesiac.ClearAndReload();
                    Amnesiac.Player = target;
                    break;

                case RoleId.Witch:
                    Utils.BecomeImpostor(Amnesiac.Player);
                    Utils.BecomeCrewmate(target);
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

                case RoleId.Assassin:
                    Utils.BecomeImpostor(Amnesiac.Player);
                    Utils.BecomeCrewmate(target);
                    Assassin.ClearAndReload();
                    Assassin.Player = AmnesiacPlayer;
                    Amnesiac.ClearAndReload();
                    Amnesiac.Player = target;
                    break;

                case RoleId.Wraith:
                    Utils.BecomeImpostor(Amnesiac.Player);
                    Utils.BecomeCrewmate(target);
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
                Utils.ShowFlash(newRole.Color, PlaySound: true);
            }
            if (newRole != null && newRole != RoleInfo.amnesiac)
            {
                GameHistory.AddToRoleHistory(AmnesiacPlayer.PlayerId, newRole);
            }
            if (newRole3 != null)
            {
                GameHistory.AddToRoleHistory(target.PlayerId, newRole3);
            }
        }
        public static void SetTrap(byte[] buff) 
        {
            if (Trapper.Player == null) return;
            Trapper.Charges -= 1;
            Vector3 position = Vector3.zero;
            position.x = BitConverter.ToSingle(buff, 0 * sizeof(float));
            position.y = BitConverter.ToSingle(buff, 1 * sizeof(float));
            _ = new Trap(position);
        }

        public static void SetBlindTrap(byte[] buff) 
        {
            if (Viper.Player == null) return;

            Vector3 position = Vector3.zero;
            position.x = BitConverter.ToSingle(buff, 0 * sizeof(float));
            position.y = BitConverter.ToSingle(buff, 1 * sizeof(float));
            _ = new BlindTrap(position);
        }
        public static void VeteranAlertKill(byte targetId)
        {
            if (PlayerControl.LocalPlayer == Veteran.Player)
            {
                PlayerControl player = Utils.GetPlayerById(targetId);
                Utils.CheckMurderAttempt(Veteran.Player, player);
            }
        }
        public static void PestilenceKill(byte targetId)
        {
            if (PlayerControl.LocalPlayer == Pestilence.Player)
            {
                PlayerControl player = Utils.GetPlayerById(targetId);
                Utils.CheckMurderAttempt(Pestilence.Player, player);
            }
        }

        public static void HostSuicide(PlayerControl player)
        {
            if (player == null) return;

            if (Deputy.ExecuteButtons != null && player == Deputy.Player)
            {
                foreach (var button in Deputy.ExecuteButtons)
                {
                    UObject.Destroy(button);
                }
                Deputy.ExecuteButtons.Clear();
            }
            if (Guesser.IsGuesser(player.PlayerId)) Utils.RemoveGuessButtonForPlayer(player);
            player.Exiled();

            GameHistory.CreateDeathReason(player, DeadPlayer.CustomDeathReason.HostSuicide, player);

            SoundEffectsManager.Play("DeadSound");

            if (FastDestroyableSingleton<HudManager>.Instance != null && PlayerControl.LocalPlayer == player)
            {
                FastDestroyableSingleton<HudManager>.Instance.KillOverlay.ShowKillAnimation(player.Data, player.Data);
                if (MeetingHudPatch.GuesserUI != null) MeetingHudPatch.GuesserUIExitButton.OnClick.Invoke();
            }

            if (MeetingHud.Instance)
            {
                var voteArea = MeetingHud.Instance.playerStates.FirstOrDefault(x => x.TargetPlayerId == player.PlayerId);
                if (voteArea != null)
                {
                    voteArea.SetDead(voteArea.DidReport, true);
                    voteArea.Overlay.gameObject.SetActive(true);
                }
                foreach (PlayerVoteArea pva in MeetingHud.Instance.playerStates)
                {
                    if (pva.VotedFor == player.PlayerId)
                    {
                        pva.UnsetVote();
                        var voteAreaPlayer = Utils.GetPlayerById(pva.TargetPlayerId);
                        if (voteAreaPlayer != null && voteAreaPlayer.AmOwner)
                            MeetingHud.Instance.ClearVote();
                    }
                }
                if (AmongUsClient.Instance.AmHost) MeetingHud.Instance.CheckForEndVoting();
            }

            if (player != null)
            {
                string msg = $"{player.Data.PlayerName}, (Lobby Host) has committed suicide!";
                if (AmongUsClient.Instance.AmClient && FastDestroyableSingleton<HudManager>.Instance)
                    FastDestroyableSingleton<HudManager>.Instance.Chat.AddChat(player, msg);
            }
        }

        public static void TriggerTrap(byte playerId, byte trapId)
        {
            Trap.TriggerTrap(playerId, trapId);
        }

        public static void TriggerBlindTrap(byte playerId, byte trapId) 
        {
            BlindTrap.TriggerTrap(playerId, trapId);
        }

        public static void SetGuesser(byte playerId)
        {
            PlayerControl target = Utils.GetPlayerById(playerId);
            if (target == null || target == Deputy.Player) return;
            _ = new Guesser.Guessers(target);
        }

        public static void ReceiveGhostInfo (byte senderId, MessageReader reader)
        {
            PlayerControl sender = Utils.GetPlayerById(senderId);

            GhostInfoTypes infoType = (GhostInfoTypes)reader.ReadByte();
            switch (infoType)
            {
                case GhostInfoTypes.HackNoticed:
                    Glitch.SetHackedKnows(true, senderId);
                    break;
                case GhostInfoTypes.HackOver:
                    _ = Glitch.HackedKnows.Remove(senderId);
                    break;
                case GhostInfoTypes.ArsonistDouse:
                    Arsonist.dousedPlayers.Add(reader.ReadPlayer());
                    break;
                case GhostInfoTypes.PlaguebearerInfect:
                    Plaguebearer.InfectedPlayers.Add(reader.ReadPlayer());
                    break;
                case GhostInfoTypes.BountyTarget:
                    BountyHunter.bounty = reader.ReadPlayer();
                    break;
                case GhostInfoTypes.AssassinMarked:
                    Assassin.AssassinMarked = reader.ReadPlayer();
                    break;
                case GhostInfoTypes.WarlockTarget:
                    Warlock.curseVictim = reader.ReadPlayer();
                    break;
                case GhostInfoTypes.PsychicInfo:
                    string PsychicInfo = reader.ReadString();
		             if (Utils.ShouldShowGhostInfo())
                    	FastDestroyableSingleton<HudManager>.Instance.Chat.AddChat(sender, PsychicInfo);
                    break;
                case GhostInfoTypes.MysticInfo:
                    string mysticInfo = reader.ReadString();
		             if (Utils.ShouldShowGhostInfo())
                    	FastDestroyableSingleton<HudManager>.Instance.Chat.AddChat(sender, mysticInfo);
                    break;
                case GhostInfoTypes.SnitchInfo:
                    string SnitchInfo = reader.ReadString();
		             if (Utils.ShouldShowGhostInfo())
                    	FastDestroyableSingleton<HudManager>.Instance.Chat.AddChat(sender, SnitchInfo);
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
                    Survivor.blankedList.Remove(sender);
                    break;
                case GhostInfoTypes.ViperTimer:
                    CustomButtonLoader.ViperKillButton.Timer = (float)reader.ReadByte();
                    break;
                case GhostInfoTypes.DeathReasonAndKiller:
                    GameHistory.CreateDeathReason(reader.ReadPlayer(), (DeadPlayer.CustomDeathReason)reader.ReadByte(), reader.ReadPlayer());
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
            if (isFirstJump)
            {
                Yoyo.MarkLocation(position);
                new Silhouette(position, CustomGameOptions.YoyoBlinkDuration, true);
            }
            else
            {
                new Silhouette(position, 5, true);
                Yoyo.markedLocation = null;
            }
            if (Chameleon.Players.Any(x => x.PlayerId == Yoyo.Player.PlayerId)) // Make the Yoyo visible if chameleon!
                Chameleon.lastMoved[Yoyo.Player.PlayerId] = Time.time;
        }

        public static void LuckyBecomeUnlucky() 
        {
            if (Lucky.Player == null || Lucky.IsUnlucky) return;
            Lucky.IsUnlucky = true;
            if (PlayerControl.LocalPlayer.Data.IsDead) 
            {
                Lucky.Player.ShowFailedMurder();
            }
        }
    }

    #endregion

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.HandleRpc))]
    public static class HandleRpc
    {
        public static PlayerControl ReadPlayer(this MessageReader reader)
        {
            return Utils.GetPlayerById(reader.ReadByte());
        }
        public static void Postfix([HarmonyArgument(0)] byte callId, [HarmonyArgument(1)] MessageReader reader)
        {
            switch ((CustomRPC)callId)
            {
                // Main Controls
                case CustomRPC.ResetVaribles:
                    RPCProcedure.ResetVariables();
                    break;
                case CustomRPC.ShareOptions:
                    RPCProcedure.HandleShareOptions(reader.ReadByte(), reader);
                    break;
                case CustomRPC.SetRole:
                    byte roleId = reader.ReadByte();
                    byte playerId = reader.ReadByte();
                    RPCProcedure.SetRole(roleId, playerId);
                    break;
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
                    if (reader.Length - reader.Position >= 17)
                    { // enough bytes left to read
                        revision = reader.ReadByte();
                        // GUID
                        byte[] gbytes = reader.ReadBytes(16);
                        guid = new Guid(gbytes);
                    }
                    else
                    {
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
                case CustomRPC.WorkaroundSetRoles:
                    RPCProcedure.WorkaroundSetRoles(reader.ReadByte(), reader);
                    break;
                case CustomRPC.DisableVanillaRoles:
                    RPCProcedure.DisableVanillaRoles();
                    break;
                case CustomRPC.BypassKill:
                    var killer = reader.ReadPlayer();
                    var target = reader.ReadPlayer();
                    Utils.MurderPlayer(killer, target, reader.ReadBoolean());
                    break;
                case CustomRPC.BypassMultiKill:
                    var killer2 = reader.ReadPlayer();
                    var target2 = reader.ReadPlayer();
                    Utils.MurderPlayer(killer2, target2, false);
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
                case CustomRPC.RandomMapOption:
                    byte mapId = reader.ReadByte();
                    RPCProcedure.RandomMapOption(mapId);
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
                case CustomRPC.CheckMurder:
                    var murderKiller = reader.ReadPlayer();
                    var murderTarget = reader.ReadPlayer();
                    murderKiller.CheckMurder(murderTarget);
                    break;
                case CustomRPC.RemoveBlackmail:
                    RPCProcedure.RemoveBlackmail();
                    break;
                case CustomRPC.RemoveMedicShield:
                    RPCProcedure.RemoveMedicShield();
                    break;
                case CustomRPC.EngineerUsedRepair:
                    RPCProcedure.EngineerUsedRepair();
                    break;
                case CustomRPC.CleanBody:
                    RPCProcedure.CleanBody(reader.ReadByte(), reader.ReadByte());
                    break;
                case CustomRPC.ChronosRewindTime:
                    RPCProcedure.ChronosRewindTime();
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
                case CustomRPC.MonarchKnight:
                    RPCProcedure.MonarchKnight(reader.ReadByte());
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
                case CustomRPC.LandlordTeleport:
                    Coroutines.Start(RPCProcedure.LandlordTransportPlayers(reader.ReadByte(), reader.ReadByte(), reader.ReadBoolean()));
                    break;
                case CustomRPC.SetUnteleportable:
                    if (PlayerControl.LocalPlayer == Landlord.Player)
                    {
                        Landlord.UnteleportablePlayers.Add(reader.ReadByte(), DateTime.UtcNow);
                    }
                    break;
                case CustomRPC.Confess:
                    byte confessorId = reader.ReadByte();
                    Oracle.Confessor = Utils.GetPlayerById(confessorId);
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
                        TheSushiRoles.Logger.LogError($"Invalid faction ID received: {factionId}");
                        Oracle.RevealedFaction = Faction.Other; // Default to Other in case of error
                    }
                    break;
                case CustomRPC.BecomeCrewmate:
                    RPCProcedure.BecomeCrewmate(reader.ReadPlayer());
                    break;
                case CustomRPC.BecomeImpostor:
                    RPCProcedure.BecomeImpostor(reader.ReadPlayer());
                    break;
                case CustomRPC.MayorSetVoteTwice:
                    Mayor.voteTwice = reader.ReadBoolean();
                    break;
                case CustomRPC.Disperse:
                    byte teleports = reader.ReadByte();
                    Dictionary<byte, Vector2> coordinates = new Dictionary<byte, Vector2>();
                    for (int i = 0; i < teleports; i++)
                    {
                        byte Swap11 = reader.ReadByte();
                        Vector2 location = reader.ReadVector2();
                        coordinates.Add(Swap11, location);
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
                case CustomRPC.PainterPaint:
                    RPCProcedure.PainterPaint();
                    break;
                case CustomRPC.GrenadierFlash:
                    RPCProcedure.GrenadierFlash();
                    break;
                case CustomRPC.WerewolfMaul:
                    RPCProcedure.WerewolfMaul();
                    break;
                case CustomRPC.HostSuicide:
                    RPCProcedure.HostSuicide(reader.ReadPlayer());
                    break;
                case CustomRPC.ViperSetPoisoned:
                    byte poisonedId = reader.ReadByte();
                    byte reset = reader.ReadByte();
                    RPCProcedure.ViperSetPoisoned(poisonedId, reset);
                    break;
                case CustomRPC.TrackerUsedTracker:
                    RPCProcedure.TrackerUsedTracker(reader.ReadByte());
                    break;
                case CustomRPC.GlitchUsedHacks:
                    RPCProcedure.GlitchUsedHacks(reader.ReadByte());
                    break;
                case CustomRPC.BlackmailerBlackmail:
                    RPCProcedure.BlackmailerBlackmail(reader.ReadByte());
                    break;
                case CustomRPC.SetFutureShielded:
                    RPCProcedure.SetFutureShielded(reader.ReadByte());
                    break;
                case CustomRPC.PlaceAssassinTrace:
                    RPCProcedure.PlaceAssassinTrace(reader.ReadBytesAndSize());
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
                case CustomRPC.GuesserShoot:
                    byte killerId = reader.ReadByte();
                    byte dyingTarget = reader.ReadByte();
                    byte guessedTarget = reader.ReadByte();
                    byte guessedRoleId = reader.ReadByte();
                    RPCProcedure.GuesserShoot(killerId, dyingTarget, guessedTarget, guessedRoleId);
                    break;
                case CustomRPC.DeputyShoot:
                    byte killerId1 = reader.ReadByte();
                    byte dyingTarget1 = reader.ReadByte();
                    RPCProcedure.DeputyShoot(killerId1, dyingTarget1);
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
                case CustomRPC.SetBlindTrap:
                    RPCProcedure.SetBlindTrap(reader.ReadBytesAndSize());
                    break;
                case CustomRPC.TriggerTrap:
                    byte trappedPlayer = reader.ReadByte();
                    byte trapId = reader.ReadByte();
                    RPCProcedure.TriggerTrap(trappedPlayer, trapId);
                    break;
                case CustomRPC.TriggerBlindTrap:
                    byte trappedPlayer2 = reader.ReadByte();
                    byte trapId2 = reader.ReadByte();
                    RPCProcedure.TriggerBlindTrap(trappedPlayer2, trapId2);
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
                case CustomRPC.LuckyBecomeUnlucky:
                    RPCProcedure.LuckyBecomeUnlucky();
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
