using System.Linq;
using System;
using Reactor.Utilities.Extensions;

namespace TheSushiRoles.Patches 
{
    [HarmonyPatch]
    public static class HauntMenuMinigamePatch 
    {

        // Show the role name instead of just Crewmate / Impostor
        [HarmonyPostfix]
        [HarmonyPatch(typeof(HauntMenuMinigame), nameof(HauntMenuMinigame.SetFilterText))]
        public static void Postfix(HauntMenuMinigame __instance) 
        {
            if (GameOptionsManager.Instance.currentGameOptions.GameMode != GameModes.Normal) return;
            var target = __instance.HauntTarget;
            var roleInfo = RoleInfo.GetRoleInfoForPlayer(target);
            string roleString = (roleInfo.Count > 0 && MapOptions.GhostsSeeEverything) ? roleInfo[0].Name : "";
            if (__instance.HauntTarget.Data.IsDead) 
            {
                __instance.FilterText.text = roleString + " Ghost";
                return;
            }
            __instance.FilterText.text = roleString;
            return;
        }

        // The impostor filter now includes neutral roles
        [HarmonyPostfix]
        [HarmonyPatch(typeof(HauntMenuMinigame), nameof(HauntMenuMinigame.MatchesFilter))]
        public static void MatchesFilterPostfix(HauntMenuMinigame __instance, PlayerControl pc, ref bool __result) 
        {
            if (GameOptionsManager.Instance.currentGameOptions.GameMode != GameModes.Normal) return;
            if (__instance.filterMode == HauntMenuMinigame.HauntFilters.Impostor) 
            {
                __result = !pc.IsCrew() && !pc.Data.IsDead;
            }
        }


        // Shows the "haunt evil roles button"
        [HarmonyPrefix]
        [HarmonyPatch(typeof(HauntMenuMinigame), nameof(HauntMenuMinigame.Start))]
        public static bool StartPrefix(HauntMenuMinigame __instance) 
        {
            if (GameOptionsManager.Instance.currentGameOptions.GameMode != GameModes.Normal || !MapOptions.GhostsSeeEverything) return true;
            __instance.FilterButtons[0].gameObject.SetActive(true);
            int numActive = 0;
            int numButtons = __instance.FilterButtons.Count((PassiveButton s) => s.isActiveAndEnabled);
            float edgeDist = 0.6f * (float)numButtons;
		    for (int i = 0; i< __instance.FilterButtons.Length; i++)
		    {
			    PassiveButton passiveButton = __instance.FilterButtons[i];
			    if (passiveButton.isActiveAndEnabled)
			    {
				    passiveButton.transform.SetLocalX(FloatRange.SpreadToEdges(-edgeDist, edgeDist, numActive, numButtons));
				    numActive++;
			    }
            }
            return false;
        }

        // Moves the haunt menu a bit further down
        [HarmonyPostfix]
        [HarmonyPatch(typeof(HauntMenuMinigame), nameof(HauntMenuMinigame.FixedUpdate))]
        public static void UpdatePostfix(HauntMenuMinigame __instance) 
        {
            if (GameOptionsManager.Instance.currentGameOptions.GameMode != GameModes.Normal) return;
            if (PlayerControl.LocalPlayer.Data.Role.IsImpostor && Poisoner.Player != PlayerControl.LocalPlayer)
                __instance.gameObject.transform.localPosition = new UnityEngine.Vector3(-6f, -1.1f, __instance.gameObject.transform.localPosition.z);
            return;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(AbilityButton), nameof(AbilityButton.Update))]
        public static void ShowOrHideAbilityButtonPostfix(AbilityButton __instance) 
        {
            bool isGameMode = GameOptionsManager.Instance.currentGameOptions.GameMode == GameModes.HideNSeek;
            if (PlayerControl.LocalPlayer.Data.IsDead && (CustomOptionHolder.finishTasksBeforeHauntingOrZoomingOut.GetBool() || isGameMode)) 
            {
                // player has haunt button.
                var (playerCompleted, playerTotal) = TasksHandler.TaskInfo(PlayerControl.LocalPlayer.Data);
                int numberOfLeftTasks = playerTotal - playerCompleted;
                if (numberOfLeftTasks <= 0 || isGameMode)
                    __instance.Show();
                else
                    __instance.Hide();
            }
            if (MapOptions.RevivedPlayers.Contains(PlayerControl.LocalPlayer.PlayerId))
            {
                __instance.Hide();
            }
        }
    }
}
