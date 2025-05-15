using System;
using AmongUs.Data;
using InnerNet;
using TheSushiRoles.Modules.Debugger.Embedded.ReactorImGui;
using UnityEngine;

namespace TheSushiRoles.Modules.Debugger.Components;
public class Debugger : MonoBehaviour
{
    [HideFromIl2Cpp]
    public DragWindow Window { get; }
    public bool WindowEnabled { get; set; } = true;
    public Debugger(System.IntPtr ptr) : base(ptr)
    {
        Window = new(new(20, 20, 0, 0), "TheSushiRoles Debugger", () =>
        {
            GUILayout.Label($"Name: {DataManager.Player.customization.Name} - PRESS F3 TO HIDE WINDOW");

            var mouse = Input.mousePosition;
            GUILayout.Label($"Mouse Position\nx: {mouse.x:00.00} y: {mouse.y:00.00} z: {mouse.z:00.00}");

            if (PlayerControl.LocalPlayer)
            {
                GUILayout.Label($"Name: {PlayerControl.LocalPlayer.CurrentOutfit.PlayerName}");
                var position = PlayerControl.LocalPlayer.gameObject.transform.position;
                GUILayout.Label($"Your Position\nx: {position.x:00.00} y: {position.y:00.00} z: {position.z:00.00}");

                if (!PlayerControl.LocalPlayer.Data.IsDead && TheSushiRolesPlugin.DebuggerLoaded)
                {
                    PlayerControl.LocalPlayer.Collider.enabled = GUILayout.Toggle(PlayerControl.LocalPlayer.Collider.enabled, "Enable Player Collider");
                }
            }

            if (!TheSushiRolesPlugin.DebuggerLoaded || !PlayerControl.LocalPlayer)
            {
                GUILayout.Label("DEBUGGER ONLY WORKS ON LOCAL-HOSTED GAMES");
                return;
            }

            if (!(AmongUsClient.Instance?.GameState == InnerNetClient.GameStates.Joined || AmongUsClient.Instance?.GameState == InnerNetClient.GameStates.Started
            || GameManager.Instance?.GameHasStarted == true && AmongUsClient.Instance.GameState != InnerNetClient.GameStates.Ended))
                return;

            if (GUILayout.Button($"Spawn Bot ({InstanceControlPatches.Clients.Count}/15)"))
            {
                Keyboard_Joystick.CreatePlayer();
            }

            if (GUILayout.Button("Remove Last Bot"))
                InstanceControlPatches.RemovePlayer((byte)InstanceControlPatches.Clients.Count);

            if (GUILayout.Button("Remove All Bots"))
                InstanceControlPatches.RemoveAllPlayers();

            if (GUILayout.Button("Next Player"))
                Keyboard_Joystick.Switch(true);

            if (GUILayout.Button("Previous Player"))
                Keyboard_Joystick.Switch(false);

            if (GUILayout.Button("End Game"))
                GameManager.Instance.RpcEndGame(GameOverReason.CrewmatesByTask, false);

            if (GUILayout.Button("Turn Impostor"))
            {
                PlayerControl.LocalPlayer.Data.Role.TeamType = RoleTeamTypes.Impostor;
                if (!PlayerControl.LocalPlayer.Data.IsDead)
                {
                    RoleManager.Instance.SetRole(PlayerControl.LocalPlayer, RoleTypes.Impostor);
                    DestroyableSingleton<HudManager>.Instance.KillButton.gameObject.SetActive(true);
                    PlayerControl.LocalPlayer.SetKillTimer(GameOptionsManager.Instance.currentNormalGameOptions.KillCooldown);
                }
                else
                {
                    RoleManager.Instance.SetRole(PlayerControl.LocalPlayer, RoleTypes.ImpostorGhost);
                }
            }

            if (GUILayout.Button("Turn Crewmate"))
            {
                PlayerControl.LocalPlayer.Data.Role.TeamType = RoleTeamTypes.Crewmate;
                if (!PlayerControl.LocalPlayer.Data.IsDead)
                    RoleManager.Instance.SetRole(PlayerControl.LocalPlayer, RoleTypes.Crewmate);
                else
                    RoleManager.Instance.SetRole(PlayerControl.LocalPlayer, RoleTypes.CrewmateGhost);
            }

            if (GUILayout.Button("Complete Tasks"))
                foreach (var task in PlayerControl.LocalPlayer.myTasks)
                {
                    PlayerControl.LocalPlayer.RpcCompleteTask(task.Id);
                }

            if (GUILayout.Button("Complete Everyone's Tasks"))
                foreach (var player in PlayerControl.AllPlayerControls)
                {
                    foreach (var task in player.myTasks)
                    {
                        player.RpcCompleteTask(task.Id);
                    }
                }
            
            if (GUILayout.Button("Remove Cooldowns"))
            {
                foreach (var button in CustomButton.buttons)
                {
                    button.Timer = 0;
                }
                PlayerControl.LocalPlayer.SetKillTimer(0f);
            }

            if (GUILayout.Button("Redo Intro Sequence"))
            {
                FastDestroyableSingleton<HudManager>.Instance.StartCoroutine(FastDestroyableSingleton<HudManager>.Instance.CoFadeFullScreen(Color.clear, Color.black));
                FastDestroyableSingleton<HudManager>.Instance.StartCoroutine(FastDestroyableSingleton<HudManager>.Instance.CoShowIntro());
            }

            if (!MeetingHud.Instance && GUILayout.Button("Start Meeting"))
            {
                PlayerControl.LocalPlayer.RemainingEmergencies++;
                RPCProcedure.UncheckedCmdReportDeadBody(PlayerControl.LocalPlayer.PlayerId, Byte.MaxValue);
                Utils.StartRPC(CustomRPC.UncheckedCmdReportDeadBody, PlayerControl.LocalPlayer.PlayerId, Byte.MaxValue);
            }

            if (GUILayout.Button("End Meeting") && MeetingHud.Instance)
                MeetingHud.Instance.RpcClose();

            if (GUILayout.Button("Kill Self"))
                PlayerControl.LocalPlayer.RpcMurderPlayer(PlayerControl.LocalPlayer, true);

            if (GUILayout.Button("Kill All"))
            foreach (var player in PlayerControl.AllPlayerControls)
            {
                player.RpcMurderPlayer(player, true);
            }

            if (GUILayout.Button("Revive Self"))
                Utils.Revive(PlayerControl.LocalPlayer);

            if (GUILayout.Button("Revive All"))
            foreach (var player in PlayerControl.AllPlayerControls)
            {
                Utils.Revive(player);
            }
        });
    }

    public void OnGUI()
    {
        if (WindowEnabled) Window.OnGUI();
    }

    public void Toggle()
    {
        WindowEnabled = !WindowEnabled;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.F3)) Toggle();
    }

    private void Start()
    {
        WindowEnabled = false;
    }
}