using System;
using System.Collections.Generic;
using System.Linq;
using InnerNet;
using UnityEngine;

namespace TheSushiRoles.Patches
{
    [HarmonyPatch(typeof(KeyboardJoystick), nameof(KeyboardJoystick.Update))]
    public static class Keyboard_Joystick
    {
        private static bool GetKeysDown(params KeyCode[] keys)
        {
            if (keys.Any(Input.GetKeyDown) && keys.All(Input.GetKey))
            {
                TheSushiRolesPlugin.Logger.LogInfo($"Shortcut Key：{keys.First(Input.GetKeyDown)} in [{string.Join(",", keys)}]");
                return true;
            }
            return false;
        }
        public static void Postfix()
        {
            if (!AmongUsClient.Instance.AmHost && AmongUsClient.Instance?.GameState != InnerNetClient.GameStates.Started) return;

            if (GetKeysDown(KeyCode.LeftControl, KeyCode.LeftShift, KeyCode.L))
            {
                DevPatches.HostEndedGame = true;
            }

            if (GetKeysDown(KeyCode.LeftControl, KeyCode.LeftShift, KeyCode.M))
            {
                if (Utils.IsHideNSeek) return;

                // if in a meeting, close it, else start one.
                if (MeetingHud.Instance)
                {
                    foreach (var pva in MeetingHud.Instance.playerStates)
                    {
                        if (pva == null) continue;

                        if (pva.VotedFor < 253)
                            MeetingHud.Instance.RpcClearVote(pva.TargetPlayerId);
                    }
                    List<MeetingHud.VoterState> statesList = [];
                    MeetingHud.Instance.RpcVotingComplete(statesList.ToArray(), null, true);
                    MeetingHud.Instance.RpcClose();
                }
                else
                {
                    Utils.HandlePoisonedOnBodyReport(); // Manually call Viper handling, since the CmdReportDeadBody Prefix won't be called
                    RPCProcedure.UncheckedCmdReportDeadBody(PlayerControl.LocalPlayer.PlayerId, Byte.MaxValue);
                    Utils.SendRPC(CustomRPC.UncheckedCmdReportDeadBody, PlayerControl.LocalPlayer.PlayerId, Byte.MaxValue);
                }
            }
            if (GetKeysDown(KeyCode.LeftControl, KeyCode.LeftShift, KeyCode.E))
            {
                Utils.HostSuicide(PlayerControl.LocalPlayer);
            }
        }
    }
    // will use this class later but rn i doubt bc im busy
    [HarmonyPatch]
    public static class DevPatches
    {
        public static bool HostEndedGame = false;
    }
}