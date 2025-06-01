using Hazel;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using TMPro;
using System.Collections;
using Reactor.Utilities;

namespace TheSushiRoles.Patches 
{
    [HarmonyPatch]
    class MeetingHudPatch 
    {
        static bool[] selections;
        static SpriteRenderer[] renderers;
        private static NetworkedPlayerInfo target = null;
        private const float scale = 0.65f;
        private static TMPro.TextMeshPro meetingExtraButtonText;
        private static PassiveButton[] swapperButtonList;
        private static TMPro.TextMeshPro meetingExtraButtonLabel;
        private static PlayerVoteArea swapped1 = null;
        private static PlayerVoteArea swapped2 = null;

        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.CheckForEndVoting))]
        class MeetingCalculateVotesPatch 
        {
            private static Dictionary<byte, int> CalculateVotes(MeetingHud __instance) 
            {
                Dictionary<byte, int> dictionary = new Dictionary<byte, int>();
                for (int i = 0; i < __instance.playerStates.Length; i++) 
                {
                    PlayerVoteArea playerVoteArea = __instance.playerStates[i];
                    if (playerVoteArea.VotedFor != 252 && playerVoteArea.VotedFor != 255 && playerVoteArea.VotedFor != 254) 
                    {
                        PlayerControl player = Utils.GetPlayerById((byte)playerVoteArea.TargetPlayerId);
                        if (player == null || player.Data == null || player.Data.IsDead || player.Data.Disconnected) continue;

                        int currentVotes;
                        int additionalVotes = 1;
                        if (Mayor.Player != null && Mayor.Player.PlayerId == playerVoteArea.TargetPlayerId && Mayor.voteTwice)
                        {
                            additionalVotes = 2;
                        }
                        PlayerControl votedFor = Utils.GetPlayerById(playerVoteArea.VotedFor);
                        if (votedFor != null && Monarch.Player != null && Monarch.KnightedPlayers.Contains(votedFor))
                        {
                            additionalVotes += 1;
                        }
                        if (dictionary.TryGetValue(playerVoteArea.VotedFor, out currentVotes))
                            dictionary[playerVoteArea.VotedFor] = currentVotes + additionalVotes;
                        else
                            dictionary[playerVoteArea.VotedFor] = additionalVotes;
                    }
                }
                // Swapper swap votes
                if (Swapper.Player != null && !Swapper.Player.Data.IsDead) 
                {
                    swapped1 = null;
                    swapped2 = null;
                    foreach (PlayerVoteArea playerVoteArea in __instance.playerStates) 
                    {
                        if (playerVoteArea.TargetPlayerId == Swapper.Swap1) swapped1 = playerVoteArea;
                        if (playerVoteArea.TargetPlayerId == Swapper.Swap2) swapped2 = playerVoteArea;
                    }

                    if (swapped1 != null && swapped2 != null) 
                    {
                        if (!dictionary.ContainsKey(swapped1.TargetPlayerId)) dictionary[swapped1.TargetPlayerId] = 0;
                        if (!dictionary.ContainsKey(swapped2.TargetPlayerId)) dictionary[swapped2.TargetPlayerId] = 0;
                        int tmp = dictionary[swapped1.TargetPlayerId];
                        dictionary[swapped1.TargetPlayerId] = dictionary[swapped2.TargetPlayerId];
                        dictionary[swapped2.TargetPlayerId] = tmp;
                    }
                }



                return dictionary;
            }


            static bool Prefix(MeetingHud __instance) 
            {
                if (__instance.playerStates.All((PlayerVoteArea ps) => ps.AmDead || ps.DidVote)) 
                {
                    // If skipping is disabled, replace skipps/no-votes with self vote
                    if (target == null && MapOptions.SkipButtonDisable != SkipButtonOptions.No && MapOptions.noVoteIsSelfVote) 
                    {
                        foreach (PlayerVoteArea playerVoteArea in __instance.playerStates)
                        {
                            if (playerVoteArea.VotedFor == byte.MaxValue - 1) playerVoteArea.VotedFor = playerVoteArea.TargetPlayerId; // TargetPlayerId
                        }
                    }

			        Dictionary<byte, int> self = CalculateVotes(__instance);
                    bool tie;
			        KeyValuePair<byte, int> max = self.MaxPair(out tie);
                    NetworkedPlayerInfo exiled = GameData.Instance.AllPlayers.ToArray().FirstOrDefault(v => !tie && v.PlayerId == max.Key && !v.IsDead);

                    // TieBreaker 
                    List<NetworkedPlayerInfo> potentialExiled = new List<NetworkedPlayerInfo>();
                    bool skipIsTie = false;
                    if (self.Count > 0)
                    {
                        Tiebreaker.isTiebreak = false;
                        int maxVoteValue = self.Values.Max();
                        PlayerVoteArea tb = null;
                        if (Tiebreaker.Player != null)
                            tb = __instance.playerStates.ToArray().FirstOrDefault(x => x.TargetPlayerId == Tiebreaker.Player.PlayerId);
                        bool isTiebreakerSkip = tb == null || tb.VotedFor == 253;
                        if (tb != null && tb.AmDead) isTiebreakerSkip = true;

                        foreach (KeyValuePair<byte, int> pair in self) 
                        {
                            if (pair.Value != maxVoteValue || isTiebreakerSkip) continue;
                            if (pair.Key != 253)
                                potentialExiled.Add(GameData.Instance.AllPlayers.ToArray().FirstOrDefault(x => x.PlayerId == pair.Key));
                            else 
                                skipIsTie = true;
                        }
                    }

                    MeetingHud.VoterState[] array = new MeetingHud.VoterState[__instance.playerStates.Length];
                    for (int i = 0; i < __instance.playerStates.Length; i++)
                    {
                        PlayerVoteArea playerVoteArea = __instance.playerStates[i];
                        array[i] = new MeetingHud.VoterState 
                        {
                            VoterId = playerVoteArea.TargetPlayerId,
                            VotedForId = playerVoteArea.VotedFor
                        };

                        if (Tiebreaker.Player == null || playerVoteArea.TargetPlayerId != Tiebreaker.Player.PlayerId) continue;

                        byte tiebreakerVote = playerVoteArea.VotedFor;
                        if (swapped1 != null && swapped2 != null) 
                        {
                            if (tiebreakerVote == swapped1.TargetPlayerId) tiebreakerVote = swapped2.TargetPlayerId;
                            else if (tiebreakerVote == swapped2.TargetPlayerId) tiebreakerVote = swapped1.TargetPlayerId;
                        }

                        if (potentialExiled.FindAll(x => x != null && x.PlayerId == tiebreakerVote).Count > 0 && (potentialExiled.Count > 1 || skipIsTie))
                        {
                            exiled = potentialExiled.ToArray().FirstOrDefault(v => v.PlayerId == tiebreakerVote);
                            tie = false;

                            Utils.SendRPC(CustomRPC.SetTiebreak);
                            RPCProcedure.SetTiebreak();
                        }
                    }

                    // RPCVotingComplete
                    __instance.RpcVotingComplete(array, exiled, tie);
                }
                return false;
            }
        }

        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.BloopAVoteIcon))]
        class MeetingHudBloopAVoteIconPatch 
        {
            public static bool Prefix(MeetingHud __instance, NetworkedPlayerInfo voterPlayer, int index, Transform parent) 
            {
                var spriteRenderer = UObject.Instantiate<SpriteRenderer>(__instance.PlayerVotePrefab);
                var showVoteColors = !GameManager.Instance.LogicOptions.GetAnonymousVotes() ||
                                      (PlayerControl.LocalPlayer.Data.IsDead && MapOptions.ghostsSeeVotes) || 
                                      (Mayor.Player != null && Mayor.Player == PlayerControl.LocalPlayer && Mayor.canSeeVoteColors && TasksHandler.TaskInfo(PlayerControl.LocalPlayer.Data).Item1 >= Mayor.tasksNeededToSeeVoteColors);
                if (showVoteColors)
                {
                    PlayerMaterial.SetColors(voterPlayer.DefaultOutfit.ColorId, spriteRenderer);
                }
                else
                {
                    PlayerMaterial.SetColors(Palette.DisabledGrey, spriteRenderer);
                }

                var transform = spriteRenderer.transform;
                transform.SetParent(parent);
                transform.localScale = Vector3.zero;
                var component = parent.GetComponent<PlayerVoteArea>();
                if (component != null)
                {
                    spriteRenderer.material.SetInt(PlayerMaterial.MaskLayer, component.MaskLayer);
                }

                __instance.StartCoroutine(Effects.Bloop(index * 0.3f, transform));
                parent.GetComponent<VoteSpreader>().AddVote(spriteRenderer);
                return false;
            }
        }

        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.PopulateResults))]
        class MeetingHudPopulateVotesPatch
        {

            private static bool Prefix(MeetingHud __instance, Il2CppStructArray<MeetingHud.VoterState> states)
            {
                // Swapper swap
                PlayerVoteArea swapped1 = null;
                PlayerVoteArea swapped2 = null;
                foreach (PlayerVoteArea playerVoteArea in __instance.playerStates)
                {
                    if (playerVoteArea.TargetPlayerId == Swapper.Swap1) swapped1 = playerVoteArea;
                    if (playerVoteArea.TargetPlayerId == Swapper.Swap2) swapped2 = playerVoteArea;
                }
                bool doSwap = swapped1 != null && swapped2 != null && Swapper.Player != null && !Swapper.Player.Data.IsDead;
                if (doSwap)
                {
                    __instance.StartCoroutine(Effects.Slide3D(swapped1.transform, swapped1.transform.localPosition, swapped2.transform.localPosition, 1.5f));
                    __instance.StartCoroutine(Effects.Slide3D(swapped2.transform, swapped2.transform.localPosition, swapped1.transform.localPosition, 1.5f));
                }


                __instance.TitleText.text = FastDestroyableSingleton<TranslationController>.Instance.GetString(StringNames.MeetingVotingResults, new Il2CppReferenceArray<Il2CppSystem.Object>(0));
                int num = 0;
                for (int i = 0; i < __instance.playerStates.Length; i++)
                {
                    PlayerVoteArea playerVoteArea = __instance.playerStates[i];
                    byte targetPlayerId = playerVoteArea.TargetPlayerId;
                    // Swapper change playerVoteArea that gets the votes
                    if (doSwap && playerVoteArea.TargetPlayerId == swapped1.TargetPlayerId) playerVoteArea = swapped2;
                    else if (doSwap && playerVoteArea.TargetPlayerId == swapped2.TargetPlayerId) playerVoteArea = swapped1;

                    playerVoteArea.ClearForResults();
                    int num2 = 0;
                    for (int j = 0; j < states.Length; j++)
                    {
                        MeetingHud.VoterState voterState = states[j];
                        NetworkedPlayerInfo playerById = GameData.Instance.GetPlayerById(voterState.VoterId);
                        if (playerById == null || playerById.IsDead) continue;

                        int extraVotes = 1;

                        if (Mayor.Player != null && voterState.VoterId == Mayor.Player.PlayerId && Mayor.voteTwice)
                            extraVotes++;

                        if (Monarch.Player != null && Monarch.KnightedPlayers.Any(p => p.PlayerId == voterState.VoterId))
                            extraVotes++;

                        for (int k = 0; k < extraVotes; k++)
                        {
                            if (i == 0 && voterState.SkippedVote)
                            {
                                __instance.BloopAVoteIcon(playerById, num, __instance.SkippedVoting.transform);
                                num++;
                            }
                            else if (voterState.VotedForId == targetPlayerId)
                            {
                                __instance.BloopAVoteIcon(playerById, num2, playerVoteArea.transform);
                                num2++;
                            }
                        }
                    }
                }
                return false;
            }
        }
        

        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.VotingComplete))]
        class MeetingHudVotingCompletedPatch 
        {
            static void Postfix(MeetingHud __instance, [HarmonyArgument(0)]byte[] states, [HarmonyArgument(1)]NetworkedPlayerInfo exiled, [HarmonyArgument(2)]bool tie)
            {
                // Reset swapper values
                Swapper.Swap1 = Byte.MaxValue;
                Swapper.Swap2 = Byte.MaxValue;
            }
        }


        static void SwapperOnClick(int i, MeetingHud __instance) 
        {
            if (__instance.state == MeetingHud.VoteStates.Results || Swapper.Charges <= 0) return;
            if (__instance.playerStates[i].AmDead) return;

            int selectedCount = selections.Where(b => b).Count();
            SpriteRenderer renderer = renderers[i];

            if (selectedCount == 0) 
            {
                renderer.color = Color.yellow;
                selections[i] = true;
            } 
            else if (selectedCount == 1) 
            {
                if (selections[i]) 
                {
                    renderer.color = Color.red;
                    selections[i] = false;
                } 
                else 
                {
                    selections[i] = true;
                    renderer.color = Color.yellow;
                    meetingExtraButtonLabel.text = Utils.ColorString(Color.yellow, "Confirm Swap");
                }
            } 
            else if (selectedCount == 2) 
            {
                if (selections[i]) 
                {
                    renderer.color = Color.red;
                    selections[i] = false;
                    meetingExtraButtonLabel.text = Utils.ColorString(Color.red, "Confirm Swap");
                }
            }
        }

        static void SwapperConfirm(MeetingHud __instance) 
        {
            __instance.playerStates[0].Cancel();  // This will stop the underlying buttons of the template from showing up
            if (__instance.state == MeetingHud.VoteStates.Results) return;
            if (selections.Where(b => b).Count() != 2) return;
            if (Swapper.Charges <= 0 || Swapper.Swap1 != Byte.MaxValue) return;

            PlayerVoteArea firstPlayer = null;
            PlayerVoteArea secondPlayer = null;
            for (int A = 0; A < selections.Length; A++) 
            {
                if (selections[A]) {
                    if (firstPlayer == null) {
                        firstPlayer = __instance.playerStates[A];
                    } else {
                        secondPlayer = __instance.playerStates[A];
                    }
                    renderers[A].color = Color.green;
                } else if (renderers[A] != null) {
                    renderers[A].color = Color.gray;
                    }
                if (swapperButtonList[A] != null) swapperButtonList[A].OnClick.RemoveAllListeners();  // Swap buttons can't be clicked / changed anymore
            }
            if (firstPlayer != null && secondPlayer != null) 
            {
                Utils.SendRPC(CustomRPC.SwapperSwap, (byte)firstPlayer.TargetPlayerId, (byte)secondPlayer.TargetPlayerId);
                RPCProcedure.SwapperSwap((byte)firstPlayer.TargetPlayerId, (byte)secondPlayer.TargetPlayerId);

                meetingExtraButtonLabel.text = Utils.ColorString(Color.green, "Swapping!");
                Swapper.Charges--;
                meetingExtraButtonText.text = $"Swaps: {Swapper.Charges}";
            }
        }

        public static void SwapperCheckAndReturnSwap(MeetingHud __instance, byte dyingPlayerId) 
        {
            // someone was guessed, Executed or dced in the meeting, check if this affects the swapper.
            if (Swapper.Player == null || __instance.state == MeetingHud.VoteStates.Results) return;

            // reset swap.
            bool reset = false;
            if (dyingPlayerId == Swapper.Swap1 || dyingPlayerId == Swapper.Swap2) 
            {
                reset = true;
                Swapper.Swap1 = Swapper.Swap2 = byte.MaxValue;
            }
            

            // Only for the swapper: Reset all the buttons and Charges value to their original state.
            if (PlayerControl.LocalPlayer != Swapper.Player) return;


            // check if dying player was a selected player (but not confirmed yet)
            for (int i = 0; i < __instance.playerStates.Count; i++) 
            {
                reset = reset || selections[i] && __instance.playerStates[i].TargetPlayerId == dyingPlayerId;
                if (reset) break;
            }

            if (!reset) return;


            for (int i = 0; i < selections.Length; i++) 
            {
                selections[i] = false;
                PlayerVoteArea playerVoteArea = __instance.playerStates[i];
                if (playerVoteArea.AmDead || (playerVoteArea.TargetPlayerId == Swapper.Player.PlayerId && Swapper.canOnlySwapOthers)) continue;
                renderers[i].color = Color.red;
                Swapper.Charges++;
                int copyI = i;
                swapperButtonList[i].OnClick.RemoveAllListeners();
                swapperButtonList[i].OnClick.AddListener((System.Action)(() => SwapperOnClick(copyI, __instance)));
            }
            meetingExtraButtonText.text = $"Swaps: {Swapper.Charges}";
            meetingExtraButtonLabel.text = Utils.ColorString(Color.red, "Confirm Swap");

        }

        static void MayorToggleVoteTwice(MeetingHud __instance) 
        {
            __instance.playerStates[0].Cancel();  // This will stop the underlying buttons of the template from showing up
            if (__instance.state == MeetingHud.VoteStates.Results || Mayor.Player.Data.IsDead) return;
            if (Mayor.mayorChooseSingleVote == Mayor.ChooseSingleVote.BeforeVoting) 
            { // Only accept changes until the mayor voted
                var mayorPVA = __instance.playerStates.FirstOrDefault(x => x.TargetPlayerId == Mayor.Player.PlayerId);
                if (mayorPVA != null && mayorPVA.DidVote) {
                    SoundEffectsManager.Play("fail");
                    return;
                }
            }

            Mayor.voteTwice = !Mayor.voteTwice;
            Utils.SendRPC(CustomRPC.MayorSetVoteTwice, Mayor.voteTwice);

            meetingExtraButtonLabel.text = Utils.ColorString(Mayor.Color, "Double Vote: " + (Mayor.voteTwice ? Utils.ColorString(Color.green, "On ") : Utils.ColorString(Color.red, "Off")));
        }

        public static byte deputyCurrentTarget;
        static void DeputyOnClick(int buttonTarget, MeetingHud __instance)
        {
            if (!(__instance.state == MeetingHud.VoteStates.Voted || __instance.state == MeetingHud.VoteStates.NotVoted) || !Deputy.CanExecute) return;
            __instance.playerStates.ToList().ForEach(x => x.gameObject.SetActive(false));

            PlayerControl focusedTarget = Utils.GetPlayerById((byte)__instance.playerStates[buttonTarget].TargetPlayerId);

            deputyCurrentTarget = __instance.playerStates[buttonTarget].TargetPlayerId;
            if (focusedTarget != null && !PlayerControl.LocalPlayer.Data.IsDead && !focusedTarget.Data.IsDead)
            {
                // Check for shield/fortified
                if (!Deputy.KillsThroughShield && focusedTarget == Medic.Shielded)
                {
                    __instance.playerStates.ToList().ForEach(x => x.gameObject.SetActive(true));

                    Utils.SendRPC(CustomRPC.ShieldedMurderAttempt);
                    RPCProcedure.ShieldedMurderAttempt();
                    SoundEffectsManager.Play("fail");
                    return;
                }

                if (focusedTarget == Crusader.FortifiedPlayer)
                {
                    __instance.playerStates.ToList().ForEach(x => x.gameObject.SetActive(true));
                    Utils.SendRPC(CustomRPC.FortifiedMurderAttempt);
                    RPCProcedure.FortifiedMurderAttempt();
                    SoundEffectsManager.Play("fail");
                    return;
                }

                // Murder the focused target
                __instance.playerStates.ToList().ForEach(x => x.gameObject.SetActive(true));

                Utils.SendRPC(CustomRPC.DeputyShoot, PlayerControl.LocalPlayer.PlayerId, focusedTarget.PlayerId);
                RPCProcedure.DeputyShoot(PlayerControl.LocalPlayer.PlayerId, focusedTarget.PlayerId);
            }
        }

        public static GameObject GuesserUI;
        public static PassiveButton GuesserUIExitButton;
        public static byte guesserCurrentTarget;
        static void GuesserOnClick(int buttonTarget, MeetingHud __instance)
        {
            if (GuesserUI != null || !(__instance.state == MeetingHud.VoteStates.Voted || __instance.state == MeetingHud.VoteStates.NotVoted)) return;
            __instance.playerStates.ToList().ForEach(x => x.gameObject.SetActive(false));

            Transform PhoneUI = UObject.FindObjectsOfType<Transform>().FirstOrDefault(x => x.name == "PhoneUI");
            Transform container = UObject.Instantiate(PhoneUI, __instance.transform);
            container.transform.localPosition = new Vector3(0, 0, -5f);
            GuesserUI = container.gameObject;

            int i = 0;
            var buttonTemplate = __instance.playerStates[0].transform.FindChild("votePlayerBase");
            var maskTemplate = __instance.playerStates[0].transform.FindChild("MaskArea");
            var smallButtonTemplate = __instance.playerStates[0].Buttons.transform.Find("CancelButton");
            var textTemplate = __instance.playerStates[0].NameText;

            guesserCurrentTarget = __instance.playerStates[buttonTarget].TargetPlayerId;

            Transform exitButtonParent = (new GameObject()).transform;
            exitButtonParent.SetParent(container);
            Transform exitButton = UObject.Instantiate(buttonTemplate.transform, exitButtonParent);
            Transform exitButtonMask = UObject.Instantiate(maskTemplate, exitButtonParent);
            exitButton.gameObject.GetComponent<SpriteRenderer>().sprite = smallButtonTemplate.GetComponent<SpriteRenderer>().sprite;
            exitButtonParent.transform.localPosition = new Vector3(2.725f, 2.1f, -5);
            exitButtonParent.transform.localScale = new Vector3(0.217f, 0.9f, 1);
            GuesserUIExitButton = exitButton.GetComponent<PassiveButton>();
            GuesserUIExitButton.OnClick.RemoveAllListeners();
            GuesserUIExitButton.OnClick.AddListener((System.Action)(() => 
            {
                __instance.playerStates.ToList().ForEach(x => 
                {
                    x.gameObject.SetActive(true);
                    if (PlayerControl.LocalPlayer.Data.IsDead && Guesser.GuessButtons != null) Utils.RemoveGuessButtons();
                });
                UObject.Destroy(container.gameObject);
            }));

            List<Transform> buttons = new List<Transform>();
            Transform selectedButton = null;

            foreach (RoleInfo roleInfo in RoleInfo.allRoleInfos) 
            {
                var guesserRole = RoleInfo.GetRoleInfoForPlayer(PlayerControl.LocalPlayer).FirstOrDefault();
                if (roleInfo.RoleId == guesserRole.RoleId) continue; // Not guessable roles
                if (PlayerControl.LocalPlayer.Data.Role.IsImpostor && !Guesser.evilGuesserCanGuessSpy && roleInfo.RoleId == RoleId.Spy) continue;
                // remove all roles that cannot spawn due to the settings from the ui.
                RoleManagerSelectRolesPatch.RoleAssignmentData roleData = RoleManagerSelectRolesPatch.GetRoleAssignmentData();
                if (roleData.NeutralEvilSettings.ContainsKey((byte)roleInfo.RoleId) && roleData.NeutralEvilSettings[(byte)roleInfo.RoleId] == 0) continue;
                else if (roleData.ImpSettings.ContainsKey((byte)roleInfo.RoleId) && roleData.ImpSettings[(byte)roleInfo.RoleId] == 0) continue;
                else if (roleData.NeutralBenignSettings.ContainsKey((byte)roleInfo.RoleId) && roleData.NeutralBenignSettings[(byte)roleInfo.RoleId] == 0) continue;
                else if (roleData.NeutralKillingSettings.ContainsKey((byte)roleInfo.RoleId) && roleData.NeutralKillingSettings[(byte)roleInfo.RoleId] == 0) continue;
                else if (roleData.CrewSettings.ContainsKey((byte)roleInfo.RoleId) && roleData.CrewSettings[(byte)roleInfo.RoleId] == 0) continue;
                else if (roleInfo.RoleId == RoleId.Pestilence) continue;
                if (roleInfo.RoleId == RoleId.Survivor && CustomOptionHolder.lawyerSpawnRate.GetSelection() == 0) continue;
                if (roleInfo.RoleId == RoleId.Spy && roleData.Impostors.Count <= 1) continue;
                if (Snitch.Player != null)
                {
                    var (playerCompleted, playerTotal) = TasksHandler.TaskInfo(Snitch.Player.Data);
                    int numberOfLeftTasks = playerTotal - playerCompleted;
                    if (numberOfLeftTasks == 0 && roleInfo.RoleId == RoleId.Snitch && Snitch.KnowsRealKiller) continue;
                }

                Transform buttonParent = (new GameObject()).transform;
                buttonParent.SetParent(container);
                Transform button = UObject.Instantiate(buttonTemplate, buttonParent);
                Transform buttonMask = UObject.Instantiate(maskTemplate, buttonParent);
                TMPro.TextMeshPro label = UObject.Instantiate(textTemplate, button);
                button.GetComponent<SpriteRenderer>().sprite = ShipStatus.Instance.CosmeticsCache.GetNameplate("nameplate_NoPlate").Image;
                buttons.Add(button);
                int row = i/5, col = i%5;
                buttonParent.localPosition = new Vector3(-3.47f + 1.75f * col, 1.5f - 0.45f * row, -5);
                buttonParent.localScale = new Vector3(0.55f, 0.55f, 1f);
                label.text = Utils.ColorString(roleInfo.Color, roleInfo.Name);
                label.alignment = TMPro.TextAlignmentOptions.Center;
                label.transform.localPosition = new Vector3(0, 0, label.transform.localPosition.z);
                label.transform.localScale *= 1.7f;
                int copiedIndex = i;

                button.GetComponent<PassiveButton>().OnClick.RemoveAllListeners();
                if (!PlayerControl.LocalPlayer.Data.IsDead && !Utils.GetPlayerById((byte)__instance.playerStates[buttonTarget].TargetPlayerId).Data.IsDead) button.GetComponent<PassiveButton>().OnClick.AddListener((System.Action)(() => 
                {
                    if (selectedButton != button) 
                    {
                        selectedButton = button;
                        buttons.ForEach(x => x.GetComponent<SpriteRenderer>().color = x == selectedButton ? Color.red : Color.white);
                    } 
                    else 
                    {
                        PlayerControl focusedTarget = Utils.GetPlayerById((byte)__instance.playerStates[buttonTarget].TargetPlayerId);
                        if (!(__instance.state == MeetingHud.VoteStates.Voted || __instance.state == MeetingHud.VoteStates.NotVoted) || focusedTarget == null || Guesser.RemainingShots(PlayerControl.LocalPlayer.PlayerId) <= 0 ) return;

                        if (!Guesser.killsThroughShield && focusedTarget == Medic.Shielded) 
                        {
                            // Depending on the options, shooting the Shielded player will not allow the guess, notifiy everyone about the kill attempt and close the window
                            __instance.playerStates.ToList().ForEach(x => x.gameObject.SetActive(true)); 
                            UObject.Destroy(container.gameObject);

                            Utils.SendRPC(CustomRPC.ShieldedMurderAttempt);
                            RPCProcedure.ShieldedMurderAttempt();
                            SoundEffectsManager.Play("fail");
                            return;
                        }

                        if (focusedTarget == Crusader.FortifiedPlayer) 
                        {
                            // Shooting the fortified player will not allow the guess, notifiy everyone about the kill attempt and close the window
                            __instance.playerStates.ToList().ForEach(x => x.gameObject.SetActive(true)); 
                            UObject.Destroy(container.gameObject);

                            Utils.SendRPC(CustomRPC.FortifiedMurderAttempt);
                            RPCProcedure.FortifiedMurderAttempt();
                            SoundEffectsManager.Play("fail");
                            return;
                        }

                        var mainRoleInfo = RoleInfo.GetRoleInfoForPlayer(focusedTarget).FirstOrDefault();
                        if (mainRoleInfo == null) return;

                        PlayerControl dyingTarget = (mainRoleInfo == roleInfo) ? focusedTarget : PlayerControl.LocalPlayer;

                        // Reset the GUI
                        __instance.playerStates.ToList().ForEach(x => x.gameObject.SetActive(true));
                        UObject.Destroy(container.gameObject);
                        if (Guesser.hasMultipleShotsPerMeeting && Guesser.RemainingShots(PlayerControl.LocalPlayer.PlayerId) > 1 && dyingTarget != PlayerControl.LocalPlayer)
                        __instance.playerStates.ToList().ForEach(x => { if (x.TargetPlayerId == dyingTarget.PlayerId && x.transform.FindChild("ShootButton") != null) UObject.Destroy(x.transform.FindChild("ShootButton").gameObject); });
                        else
                        {
                            __instance.playerStates.ToList().ForEach(x => { if (Guesser.GuessButtons != null) Utils.RemoveGuessButtons(); });
                        }

                        // Shoot player and send chat info if activated
                        Utils.SendRPC(CustomRPC.GuesserShoot, PlayerControl.LocalPlayer.PlayerId, dyingTarget.PlayerId, focusedTarget.PlayerId, (byte)roleInfo.RoleId);
                        RPCProcedure.GuesserShoot(PlayerControl.LocalPlayer.PlayerId, dyingTarget.PlayerId, focusedTarget.PlayerId, (byte)roleInfo.RoleId);
                    }
                }));

                i++;
            }
            container.transform.localScale *= 0.75f;
        }

        [HarmonyPatch(typeof(PlayerVoteArea), nameof(PlayerVoteArea.Select))]
        class PlayerVoteAreaSelectPatch 
        {
            static bool Prefix(MeetingHud __instance) 
            {
                return !(PlayerControl.LocalPlayer != null && Guesser.IsGuesser(PlayerControl.LocalPlayer.PlayerId) && GuesserUI != null);
            }
        }

        static void PopulateButtonsPostfix(MeetingHud __instance)
        {
            // Add Swapper Buttons
            bool addSwapperButtons = Swapper.Player != null && PlayerControl.LocalPlayer == Swapper.Player && !Swapper.Player.Data.IsDead;
            bool addMayorButton = Mayor.Player != null && PlayerControl.LocalPlayer == Mayor.Player && !Mayor.Player.Data.IsDead && Mayor.mayorChooseSingleVote != Mayor.ChooseSingleVote.Off;
            if (addSwapperButtons)
            {
                selections = new bool[__instance.playerStates.Length];
                renderers = new SpriteRenderer[__instance.playerStates.Length];
                swapperButtonList = new PassiveButton[__instance.playerStates.Length];

                for (int i = 0; i < __instance.playerStates.Length; i++)
                {
                    PlayerVoteArea playerVoteArea = __instance.playerStates[i];
                    if (playerVoteArea.AmDead || (playerVoteArea.TargetPlayerId == Swapper.Player.PlayerId && Swapper.canOnlySwapOthers)) continue;

                    GameObject template = playerVoteArea.Buttons.transform.Find("CancelButton").gameObject;
                    GameObject checkbox = UObject.Instantiate(template);
                    checkbox.transform.SetParent(playerVoteArea.transform);
                    checkbox.transform.position = template.transform.position;
                    checkbox.transform.localPosition = new Vector3(-0.5f, 0.03f, -1.3f);
                    SpriteRenderer renderer = checkbox.GetComponent<SpriteRenderer>();
                    renderer.sprite = Swapper.GetCheckSprite();
                    renderer.color = Color.red;

                    if (Swapper.Charges <= 0) renderer.color = Color.gray;

                    PassiveButton button = checkbox.GetComponent<PassiveButton>();
                    swapperButtonList[i] = button;
                    button.OnClick.RemoveAllListeners();
                    int copiedIndex = i;
                    button.OnClick.AddListener((System.Action)(() => SwapperOnClick(copiedIndex, __instance)));

                    selections[i] = false;
                    renderers[i] = renderer;
                }
            }

            // Add meeting extra button, i.e. Swapper Confirm Button or Mayor Toggle Double Vote Button. Swapper Button uses ExtraButtonText on the Left of the Button. (Future meeting buttons can easily be added here)
            if (addSwapperButtons || addMayorButton)
            {
                Transform meetingUI = UObject.FindObjectsOfType<Transform>().FirstOrDefault(x => x.name == "PhoneUI");

                var buttonTemplate = __instance.playerStates[0].transform.FindChild("votePlayerBase");
                var maskTemplate = __instance.playerStates[0].transform.FindChild("MaskArea");
                var textTemplate = __instance.playerStates[0].NameText;
                Transform meetingExtraButtonParent = (new GameObject()).transform;
                meetingExtraButtonParent.SetParent(meetingUI);
                Transform meetingExtraButton = UObject.Instantiate(buttonTemplate, meetingExtraButtonParent);

                Transform infoTransform = __instance.playerStates[0].NameText.transform.parent.FindChild("Info");
                TMPro.TextMeshPro meetingInfo = infoTransform != null ? infoTransform.GetComponent<TMPro.TextMeshPro>() : null;
                meetingExtraButtonText = UObject.Instantiate(__instance.playerStates[0].NameText, meetingExtraButtonParent);
                meetingExtraButtonText.text = addSwapperButtons ? $"Swaps: {Swapper.Charges}" : "";
                meetingExtraButtonText.enableWordWrapping = false;
                meetingExtraButtonText.transform.localScale = Vector3.one * 1.7f;
                meetingExtraButtonText.transform.localPosition = new Vector3(-2.5f, 0f, 0f);

                Transform meetingExtraButtonMask = UObject.Instantiate(maskTemplate, meetingExtraButtonParent);
                meetingExtraButtonLabel = UObject.Instantiate(textTemplate, meetingExtraButton);
                meetingExtraButton.GetComponent<SpriteRenderer>().sprite = ShipStatus.Instance.CosmeticsCache.GetNameplate("nameplate_NoPlate").Image;

                meetingExtraButtonParent.localPosition = new Vector3(0, -2.225f, -5);
                meetingExtraButtonParent.localScale = new Vector3(0.55f, 0.55f, 1f);
                meetingExtraButtonLabel.alignment = TMPro.TextAlignmentOptions.Center;
                meetingExtraButtonLabel.transform.localPosition = new Vector3(0, 0, meetingExtraButtonLabel.transform.localPosition.z);
                if (addSwapperButtons)
                {
                    meetingExtraButtonLabel.transform.localScale *= 1.7f;
                    meetingExtraButtonLabel.text = Utils.ColorString(Color.red, "Confirm Swap");
                }
                else if (addMayorButton)
                {
                    meetingExtraButtonLabel.transform.localScale = new Vector3(meetingExtraButtonLabel.transform.localScale.x * 1.5f, meetingExtraButtonLabel.transform.localScale.x * 1.7f, meetingExtraButtonLabel.transform.localScale.x * 1.7f);
                    meetingExtraButtonLabel.text = Utils.ColorString(Mayor.Color, "Double Vote: " + (Mayor.voteTwice ? Utils.ColorString(Color.green, "On ") : Utils.ColorString(Color.red, "Off")));
                }
                PassiveButton passiveButton = meetingExtraButton.GetComponent<PassiveButton>();
                passiveButton.OnClick.RemoveAllListeners();
                if (!PlayerControl.LocalPlayer.Data.IsDead)
                {
                    if (addSwapperButtons)
                        passiveButton.OnClick.AddListener((Action)(() => SwapperConfirm(__instance)));
                    else if (addMayorButton)
                        passiveButton.OnClick.AddListener((Action)(() => MayorToggleVoteTwice(__instance)));
                }
                meetingExtraButton.parent.gameObject.SetActive(false);
                __instance.StartCoroutine(Effects.Lerp(7.27f, new Action<float>((p) =>
                { // Button appears delayed, so that its visible in the voting screen only!
                    if (p == 1f)
                    {
                        meetingExtraButton.parent.gameObject.SetActive(true);
                    }
                })));
            }


            bool isGuesser = Guesser.IsGuesser(PlayerControl.LocalPlayer.PlayerId);

            // Add overlay for spelled players
            if (Witch.Player != null && Witch.futureSpelled != null)
            {
                foreach (PlayerVoteArea pva in __instance.playerStates)
                {
                    if (Witch.futureSpelled.Any(x => x.PlayerId == pva.TargetPlayerId))
                    {
                        SpriteRenderer rend = (new GameObject()).AddComponent<SpriteRenderer>();
                        rend.transform.SetParent(pva.transform);
                        rend.gameObject.layer = pva.Megaphone.gameObject.layer;
                        rend.transform.localPosition = new Vector3(-0.725f, -0.15f, -1f);
                        rend.sprite = Witch.GetSpelledOverlaySprite();
                    }
                }
            }

            // Add Guesser Buttons
            int remainingShots = Guesser.RemainingShots(PlayerControl.LocalPlayer.PlayerId);
            var (playerCompleted, playerTotal) = TasksHandler.TaskInfo(PlayerControl.LocalPlayer.Data);

            if (isGuesser && !PlayerControl.LocalPlayer.Data.IsDead && remainingShots > 0)
            {
                for (int i = 0; i < __instance.playerStates.Length; i++)
                {
                    PlayerVoteArea playerVoteArea = __instance.playerStates[i];
                    PlayerControl targetPlayer = GameData.Instance.GetPlayerById(playerVoteArea.TargetPlayerId)?.Object;
                    if (playerVoteArea.TargetPlayerId == PlayerControl.LocalPlayer.PlayerId || playerVoteArea.AmDead) continue;
                    if (targetPlayer != null && targetPlayer.IsImpostor() && PlayerControl.LocalPlayer.IsImpostor()) continue;
                    if (targetPlayer != null && targetPlayer.IsImpostor() && PlayerControl.LocalPlayer == Recruit.Player) continue;
                    if (targetPlayer != null && targetPlayer == Monarch.Player && Monarch.KnightedPlayers.Contains(PlayerControl.LocalPlayer)) continue;
                    if (PlayerControl.LocalPlayer != null && PlayerControl.LocalPlayer == Eraser.Player && Eraser.alreadyErased.Contains(playerVoteArea.TargetPlayerId)) continue;
                    if (PlayerControl.LocalPlayer != null && PlayerControl.LocalPlayer.IsCrew() && playerCompleted < Guesser.tasksToUnlock) continue;

                    GameObject template = playerVoteArea.Buttons.transform.Find("CancelButton").gameObject;
                    GameObject targetBox = UObject.Instantiate(template, playerVoteArea.transform);
                    targetBox.name = "ShootButton";
                    targetBox.transform.localPosition = new Vector3(-0.95f, 0.03f, -1.3f);
                    SpriteRenderer renderer = targetBox.GetComponent<SpriteRenderer>();
                    renderer.sprite = Guesser.GetTargetSprite();
                    PassiveButton button = targetBox.GetComponent<PassiveButton>();
                    button.OnClick.RemoveAllListeners();
                    int copiedIndex = i;
                    button.OnClick.AddListener((Action)(() => GuesserOnClick(copiedIndex, __instance)));
                    Guesser.GuessButtons.Add(targetBox);
                }
            }
            
            if (Deputy.Player == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead && Deputy.Charges > 0 && Deputy.CanExecute) 
            {
                for (int i = 0; i < __instance.playerStates.Length; i++)
                {
                    PlayerVoteArea playerVoteArea = __instance.playerStates[i];
                    PlayerControl targetPlayer = GameData.Instance.GetPlayerById(playerVoteArea.TargetPlayerId)?.Object;
                    if (playerVoteArea.TargetPlayerId == PlayerControl.LocalPlayer.PlayerId || playerVoteArea.AmDead) continue;
                    if (targetPlayer != null && targetPlayer == Jackal.Player && PlayerControl.LocalPlayer == Recruit.Player) continue;
                    if (targetPlayer != null && targetPlayer == Monarch.Player && Monarch.KnightedPlayers.Contains(PlayerControl.LocalPlayer)) continue;

                    GameObject template = playerVoteArea.Buttons.transform.Find("CancelButton").gameObject;
                    GameObject targetBox = UObject.Instantiate(template, playerVoteArea.transform);
                    targetBox.name = "DeputyButton";
                    targetBox.transform.localPosition = new Vector3(-0.5f, 0.03f, -1.3f);
                    SpriteRenderer renderer = targetBox.GetComponent<SpriteRenderer>();
                    renderer.sprite = Guesser.GetTargetSprite();
                    PassiveButton button = targetBox.GetComponent<PassiveButton>();
                    button.OnClick.RemoveAllListeners();
                    int copiedIndex = i;
                    button.OnClick.AddListener((Action)(() => DeputyOnClick(copiedIndex, __instance)));
                    Deputy.ExecuteButtons.Add(targetBox);
                }
            }
        }

        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.ServerStart))]
        class MeetingServerStartPatch 
        {
            static void Postfix(MeetingHud __instance)
            {
                PopulateButtonsPostfix(__instance);
            }
        }

        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Deserialize))]
        class MeetingDeserializePatch 
        {
            static void Postfix(MeetingHud __instance, [HarmonyArgument(0)]MessageReader reader, [HarmonyArgument(1)]bool initialState)
            {
                // Add swapper buttons
                if (initialState) 
                {
                    PopulateButtonsPostfix(__instance);
                }
            }
        }

        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.StartMeeting))]
        class StartMeetingPatch 
        {
            public static void Prefix(PlayerControl __instance, [HarmonyArgument(0)]NetworkedPlayerInfo meetingTarget) 
            {
                // Resett Bait list
                Bait.active = new Dictionary<DeadPlayer, float>();
                // Save Lazy position, if the player is able to move (i.e. not on a ladder or a gap thingy)
                if (PlayerControl.LocalPlayer.MyPhysics.enabled && (PlayerControl.LocalPlayer.moveable || PlayerControl.LocalPlayer.inVent
                    || HudManagerStartPatch.hackerVitalsButton.isEffectActive || HudManagerStartPatch.hackerAdminTableButton.isEffectActive || HudManagerStartPatch.VigilanteCamButton.isEffectActive
                    || Portal.isTeleporting && Portal.teleportedPlayers.Last().playerId == PlayerControl.LocalPlayer.PlayerId)) {
                    if (!PlayerControl.LocalPlayer.inMovingPlat)
                        Lazy.position = PlayerControl.LocalPlayer.transform.position;
                }

                // Psychic meeting start time
                Psychic.meetingStartTime = DateTime.UtcNow;
                // Reset Viper poisoned
                Viper.poisoned = null;
                // Count meetings
                if (meetingTarget == null) MapOptions.meetingsCount++;
                // Save the meeting target
                target = meetingTarget;

                // Add Portal info into Gatekeeper Chat:
                if (Gatekeeper.Player != null && (PlayerControl.LocalPlayer == Gatekeeper.Player || Utils.ShouldShowGhostInfo()) && !Gatekeeper.Player.Data.IsDead) {
                    if (Portal.teleportedPlayers.Count > 0)
                    {
                        string msg = "Portal Log:\n";
                        foreach (var entry in Portal.teleportedPlayers)
                        {
                            float timeBeforeMeeting = ((float)(DateTime.UtcNow - entry.time).TotalMilliseconds) / 1000;
                            msg += Gatekeeper.logShowsTime ? $"{(int)timeBeforeMeeting}s ago: " : "";
                            msg = msg + $"{entry.name} used the teleporter\n";
                        }
                        FastDestroyableSingleton<HudManager>.Instance.Chat.AddChat(Gatekeeper.Player, $"{msg}");
                    }
                }

                // Add trapped Info into Trapper chat
                if (Trapper.Player != null && (PlayerControl.LocalPlayer == Trapper.Player || Utils.ShouldShowGhostInfo()) && !Trapper.Player.Data.IsDead) 
                {
                    if (Trap.traps.Any(x => x.revealed))
                        FastDestroyableSingleton<HudManager>.Instance.Chat.AddChat(Trapper.Player, "Trap Logs:");
                    foreach (Trap trap in Trap.traps) 
                    {
                        if (!trap.revealed) continue;
                        string message = $"Trap {trap.instanceId}: \n";
                        trap.trappedPlayer = trap.trappedPlayer.OrderBy(x => TheSushiRolesPlugin.rnd.Next()).ToList();
                        foreach (byte playerId in trap.trappedPlayer) 
                        {
                            PlayerControl p = Utils.GetPlayerById(playerId);
                            if (Trapper.infoType == 0) message += RoleInfo.GetRolesString(p, false) + "\n";
                            else if (Trapper.infoType == 1) 
                            {
                                if (!p.IsCrew()) message += "Evil Role \n";
                                else message += "Good Role \n";
                            }
                            else message += p.Data.PlayerName + "\n";
                        }
                        FastDestroyableSingleton<HudManager>.Instance.Chat.AddChat(Trapper.Player, $"{message}");
                    }
                }

                Trapper.playersOnMap = new();

                // Remove revealed traps
                Trap.ClearRevealedTraps();

                // Remove the blind traps
                BlindTrap.ClearTraps();

                // Reset zoomed out ghosts
                Utils.ToggleZoom(reset: true);

                // Stop all playing sounds
                SoundEffectsManager.StopAll();

                // Close In-Game Settings Display if open
                HudManagerUpdate.CloseSettings();
            }
        }

        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Update))]
        class MeetingHudUpdatePatch 
        {
            static void Postfix(MeetingHud __instance) 
            {
                // Deactivate skip Button if skipping on emergency meetings is disabled
                // Deactivate skip Button if skipping on emergency meetings is disabled
                if ((target == null && MapOptions.SkipButtonDisable == SkipButtonOptions.Emergency) || (MapOptions.SkipButtonDisable == SkipButtonOptions.Always))
                {
                    __instance.SkipVoteButton.gameObject.SetActive(false);
                }

                if (__instance.state >= MeetingHud.VoteStates.Discussion)
                {
                    // Remove first kill Shield
                    MapOptions.FirstPlayerKilled = null;
                    // No Longer First Round
                    MapOptions.IsFirstRound = false;
                }
            }
        }

        [HarmonyPatch(typeof(OverlayKillAnimation), nameof(OverlayKillAnimation.CoShow))]
        public static class KillAnimationPatches
        {
            [HarmonyPrefix]
            public static void SetKillAnimationMaskInteraction(OverlayKillAnimation __instance)
            {
                if (MeetingHud.Instance)
                {
                    __instance.GetComponentsInChildren<SpriteRenderer>(true)
                        .ToList().ForEach(x => x.maskInteraction = SpriteMaskInteraction.None);
                }
            }
        }

        [HarmonyPatch]
        public class ShowHost 
        {
            private static TextMeshPro Text = null;
            [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Start))]
            [HarmonyPostfix]
            public static void Setup(MeetingHud __instance)
            {
                if (AmongUsClient.Instance.NetworkMode != NetworkModes.OnlineGame) return;

                __instance.ProceedButton.gameObject.transform.localPosition = new(-2.5f, 2.2f, 0);
                __instance.ProceedButton.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                __instance.ProceedButton.GetComponent<PassiveButton>().enabled = false;
                __instance.HostIcon.gameObject.SetActive(true);
                __instance.ProceedButton.gameObject.SetActive(true);
            }

            [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Update))]
            [HarmonyPostfix]

            public static void Postfix(MeetingHud __instance) 
            {
                var host = GameData.Instance.GetHost();

                if (host != null)
                {
                    PlayerMaterial.SetColors(host.DefaultOutfit.ColorId, __instance.HostIcon);
                    if (Text == null) Text = __instance.ProceedButton.gameObject.GetComponentInChildren<TextMeshPro>();
                    Text.text = $"LOBBY HOST: {host.PlayerName}";
                }
            }
        }
    }

    public class BlackmailMeetingUpdate
    {
        public const float LetterXOffset = 0.22f;
        public const float LetterYOffset = -0.32f;
        public static bool shookAlready = false;
        public static Sprite prevXMark = null;
        public static Sprite prevOverlay = null;

        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Start))]
        public class MeetingHudStartPatch
        {
            public static void Postfix(MeetingHud __instance)
            {
                shookAlready = false;

                if (Blackmailer.IsBlackmailed(PlayerControl.LocalPlayer))
                {
                    Coroutines.Start(ShowBlackmailShhh());
                }

                if (Blackmailer.ShouldShowBlackmail(PlayerControl.LocalPlayer))
                {
                    HighlightBlackmailedPlayer(__instance);
                }
            }

            private static void HighlightBlackmailedPlayer(MeetingHud meetingHud)
            {
                var playerState = meetingHud.playerStates.FirstOrDefault(x => x.TargetPlayerId == Blackmailer.BlackmailedPlayer.PlayerId);

                if (playerState != null)
                {
                    playerState.XMark.gameObject.SetActive(true);

                    if (prevXMark == null)
                        prevXMark = playerState.XMark.sprite;

                    playerState.XMark.sprite = Blackmailer.GetBlackmailLetter();
                    playerState.XMark.transform.localScale *= 0.75f;
                    playerState.XMark.transform.localPosition += new Vector3(LetterXOffset, LetterYOffset, 0);
                }
            }
            private static IEnumerator ShowBlackmailShhh()
            {
                var hudManager = FastDestroyableSingleton<HudManager>.Instance;
                yield return hudManager.CoFadeFullScreen(Color.clear, new Color(0f, 0f, 0f, 0.98f));

                var emblem = hudManager.shhhEmblem;
                var originalPosition = emblem.transform.localPosition;
                var originalDuration = emblem.HoldDuration;

                emblem.transform.localPosition = new Vector3(emblem.transform.localPosition.x, emblem.transform.localPosition.y, hudManager.FullScreen.transform.position.z + 1f);
                emblem.TextImage.text = "YOU ARE BLACKMAILED!";
                emblem.HoldDuration = 2.5f;

                yield return hudManager.ShowEmblem(true);

                emblem.transform.localPosition = originalPosition;
                emblem.HoldDuration = originalDuration;

                yield return hudManager.CoFadeFullScreen(new Color(0f, 0f, 0f, 0.98f), Color.clear);
            }
        }

        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Update))]
        public class MeetingHudUpdatePatch
        {
            public static void Postfix(MeetingHud __instance)
            {
                if (Blackmailer.ShouldShowBlackmail(PlayerControl.LocalPlayer))
                {
                    UpdateBlackmailedPlayer(__instance);
                }
            }

            private static void UpdateBlackmailedPlayer(MeetingHud meetingHud)
            {
                var playerState = meetingHud.playerStates.FirstOrDefault(x => x.TargetPlayerId == Blackmailer.BlackmailedPlayer.PlayerId);

                if (playerState != null)
                {
                    playerState.Overlay.gameObject.SetActive(true);

                    if (prevOverlay == null)
                        prevOverlay = playerState.Overlay.sprite;

                    playerState.Overlay.sprite = Blackmailer.GetBlackmailOverlay();

                    if (meetingHud.state != MeetingHud.VoteStates.Animating && !shookAlready)
                    {
                        shookAlready = true;
                        (meetingHud as MonoBehaviour).StartCoroutine(Effects.SwayX(playerState.transform));
                    }
                }
            }
        }

        [HarmonyPatch(typeof(TextBoxTMP), nameof(TextBoxTMP.SetText))]
        public class StopChattingPatch
        {
            public static bool Prefix(TextBoxTMP __instance)
            {
                return !Blackmailer.IsBlackmailed(PlayerControl.LocalPlayer);
            }
        }
    }
}
