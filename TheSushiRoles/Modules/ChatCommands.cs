using System;
using System.Collections.Generic;
using System.Linq;

namespace TheSushiRoles.Modules
{
    [HarmonyPatch]
    public static class ChatCommands 
    {
        public static bool IsLover(this PlayerControl player) => !(player == null) && (player == Lovers.Lover1 || player == Lovers.Lover2);

        [HarmonyPatch(typeof(ChatController), nameof(ChatController.SendChat))]
        private static class SendChatPatch 
        {
            static bool Prefix(ChatController __instance) 
            {
                string text = __instance.freeChatField.Text;
                bool handled = false;
                if (AmongUsClient.Instance.GameState != InnerNet.InnerNetClient.GameStates.Started) 
                {
                    if (text.ToLower().StartsWith("/kick ")) 
                    {
                        string playerName = text.Substring(6);
                        PlayerControl target = PlayerControl.AllPlayerControls.ToArray().FirstOrDefault(x => x.Data.PlayerName.Equals(playerName));
                        if (target != null && AmongUsClient.Instance != null && AmongUsClient.Instance.CanBan()) {
                            var client = AmongUsClient.Instance.GetClient(target.OwnerId);
                            if (client != null) 
                            
                            {
                                AmongUsClient.Instance.KickPlayer(client.Id, false);
                                handled = true;
                            }
                        }
                    } 
                    else if (text.ToLower().StartsWith("/ban ")) 
                    {
                        string playerName = text.Substring(5);
                        PlayerControl target = PlayerControl.AllPlayerControls.ToArray().FirstOrDefault(x => x.Data.PlayerName.Equals(playerName));
                        if (target != null && AmongUsClient.Instance != null && AmongUsClient.Instance.CanBan()) {
                            var client = AmongUsClient.Instance.GetClient(target.OwnerId);
                            if (client != null) 
                            {
                                AmongUsClient.Instance.KickPlayer(client.Id, true);
                                handled = true;
                            }
                        }
                    }
                }

                if (text.ToLower().StartsWith("/alignments"))
                {
                    handled = true;
                    var roles = new List<RoleInfo>
                    {
                        RoleInfo.crewmate, RoleInfo.cultist, RoleInfo.lighter, RoleInfo.deputy, RoleInfo.detective, RoleInfo.hacker, RoleInfo.tracker, RoleInfo.crusader, RoleInfo.spy, RoleInfo.vigilante, RoleInfo.mayor,
                        RoleInfo.gatekeeper, RoleInfo.veteran, RoleInfo.engineer, RoleInfo.sheriff, RoleInfo.psychic, RoleInfo.trapper, RoleInfo.chronos, RoleInfo.medic, RoleInfo.swapper,
                        RoleInfo.oracle, RoleInfo.mystic,
                        RoleInfo.impostor, RoleInfo.morphling, RoleInfo.blackmailer, RoleInfo.painter, RoleInfo.viper, RoleInfo.eraser, RoleInfo.trickster, RoleInfo.janitor,
                        RoleInfo.grenadier, RoleInfo.warlock, RoleInfo.bountyHunter, RoleInfo.assassin, RoleInfo.witch, RoleInfo.wraith, RoleInfo.yoyo,
                        RoleInfo.jester, RoleInfo.arsonist, RoleInfo.monarch, RoleInfo.scavenger, RoleInfo.lawyer, RoleInfo.amnesiac, RoleInfo.prosecutor, RoleInfo.survivor, RoleInfo.romantic,
                        RoleInfo.jackal, RoleInfo.plaguebearer, RoleInfo.pestilence, RoleInfo.juggernaut, RoleInfo.agent, RoleInfo.hitman, RoleInfo.predator,
                        RoleInfo.glitch, RoleInfo.vromantic, RoleInfo.werewolf
                    };

                    var groupedRoles = roles.GroupBy(role => role.AlignmentText())
                        .OrderBy(group => group.Key)
                        .Select(group => $"{group.Key}:\n" +
                                         string.Join(", ", group.Select(role => Utils.ColorString(role.Color, $"{role.Name}"))));

                    string alignments = string.Join("\n\n", groupedRoles);
                    __instance.AddChat(PlayerControl.LocalPlayer, alignments);
                }
                
                if (AmongUsClient.Instance.NetworkMode == NetworkModes.FreePlay) 
                {
                    if (text.ToLower().Equals("/murder")) 
                    {
                        PlayerControl.LocalPlayer.Exiled();
                        FastDestroyableSingleton<HudManager>.Instance.KillOverlay.ShowKillAnimation(PlayerControl.LocalPlayer.Data, PlayerControl.LocalPlayer.Data);
                        handled = true;
                    } 
                    else if (text.ToLower().StartsWith("/color ")) 
                    {
                        handled = true;
                        int col;
                        if (!Int32.TryParse(text.Substring(7), out col)) 
                        {
                            __instance.AddChat(PlayerControl.LocalPlayer, "Unable to parse color id\nUsage: /color {id}");
                        }
                        col = Math.Clamp(col, 0, Palette.PlayerColors.Length - 1);
                        PlayerControl.LocalPlayer.SetColor(col);
                        __instance.AddChat(PlayerControl.LocalPlayer, "Changed color succesfully");
                    }
                }

                if (text.ToLower().StartsWith("/tp ") && PlayerControl.LocalPlayer.Data.IsDead) 
                {
                    string playerName = text.Substring(4).ToLower();
                    PlayerControl target = PlayerControl.AllPlayerControls.ToArray().FirstOrDefault(x => x.Data.PlayerName.ToLower().Equals(playerName));
                    if (target != null) 
                    {
                        PlayerControl.LocalPlayer.transform.position = target.transform.position;
                        handled = true;
                    }
                }

                if (handled) 
                {
                    __instance.freeChatField.Clear();
                    __instance.quickChatMenu.Clear();
                }
                return !handled;
            }
        }
        [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
        public static class EnableChat 
        {
            public static void Postfix(HudManager __instance) 
            {
                if (!__instance.Chat.isActiveAndEnabled && (AmongUsClient.Instance.NetworkMode == NetworkModes.FreePlay || PlayerControl.LocalPlayer.IsLover()))
                    __instance.Chat.SetVisible(true);
            }
        }

        [HarmonyPatch(typeof(ChatBubble), nameof(ChatBubble.SetName))]
        public static class SetBubbleName 
        { 
            public static void Postfix(ChatBubble __instance, [HarmonyArgument(0)] string playerName) 
            {
                PlayerControl sourcePlayer = PlayerControl.AllPlayerControls.ToArray().ToList().FirstOrDefault(x => x.Data != null && x.Data.PlayerName.Equals(playerName));
                if (sourcePlayer != null && PlayerControl.LocalPlayer != null && PlayerControl.LocalPlayer.Data?.Role?.IsImpostor == true && (Spy.Player != null && sourcePlayer.PlayerId == Spy.Player.PlayerId) && __instance != null) __instance.NameText.color = Palette.ImpostorRed;
            }
        }

        [HarmonyPatch(typeof(ChatController), nameof(ChatController.AddChat))]
        public static class AddChat 
        {
            public static bool Prefix(ChatController __instance, [HarmonyArgument(0)] PlayerControl sourcePlayer) 
            {
                if (__instance != FastDestroyableSingleton<HudManager>.Instance.Chat)
                    return true;
                PlayerControl localPlayer = PlayerControl.LocalPlayer;
                return localPlayer == null || MeetingHud.Instance != null || LobbyBehaviour.Instance != null || localPlayer.Data.IsDead || localPlayer.IsLover() || (int)sourcePlayer.PlayerId == (int)PlayerControl.LocalPlayer.PlayerId;

            }
        }
    }
}
