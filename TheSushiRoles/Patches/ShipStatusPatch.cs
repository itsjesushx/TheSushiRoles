using UnityEngine;

namespace TheSushiRoles.Patches 
{

    [HarmonyPatch(typeof(ShipStatus))]
    public class ShipStatusPatch 
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.CalculateLightRadius))]
        public static bool Prefix(ref float __result, ShipStatus __instance, [HarmonyArgument(0)] NetworkedPlayerInfo player) {
            if ((!__instance.Systems.ContainsKey(SystemTypes.Electrical) && !Utils.IsFungle()) || GameOptionsManager.Instance.currentGameOptions.GameMode == GameModes.HideNSeek) return true;

                // If player is a role which has Impostor vision
            if (Utils.HasImpVision(player)) {
                //__result = __instance.MaxLightRadius * GameOptionsManager.Instance.currentNormalGameOptions.ImpostorLightMod;
                __result = GetNeutralLightRadius(__instance, true);
                return false;
            }

            // If player is Lighter with ability active
            if (Lighter.Player != null && Lighter.Player.PlayerId == player.PlayerId) {
                float unlerped = Mathf.InverseLerp(__instance.MinLightRadius, __instance.MaxLightRadius, GetNeutralLightRadius(__instance, false));
                __result = Mathf.Lerp(__instance.MaxLightRadius * Lighter.lighterModeLightsOffVision, __instance.MaxLightRadius * Lighter.lighterModeLightsOnVision, unlerped);
            }

            // If there is a Trickster with their ability active
            else if (Trickster.Player != null && Trickster.lightsOutTimer > 0f) {
                float lerpValue = 1f;
                if (Trickster.lightsOutDuration - Trickster.lightsOutTimer < 0.5f) {
                    lerpValue = Mathf.Clamp01((Trickster.lightsOutDuration - Trickster.lightsOutTimer) * 2);
                } else if (Trickster.lightsOutTimer < 0.5) {
                    lerpValue = Mathf.Clamp01(Trickster.lightsOutTimer * 2);
                }

                __result = Mathf.Lerp(__instance.MinLightRadius, __instance.MaxLightRadius, 1 - lerpValue) * GameOptionsManager.Instance.currentNormalGameOptions.CrewLightMod;
            }

            // If player is Lawyer, apply Lawyer vision modifier
            else if (Lawyer.Player != null && Lawyer.Player.PlayerId == player.PlayerId) 
            {
                float unlerped = Mathf.InverseLerp(__instance.MinLightRadius, __instance.MaxLightRadius, GetNeutralLightRadius(__instance, false));
                __result = Mathf.Lerp(__instance.MinLightRadius, __instance.MaxLightRadius * Lawyer.vision, unlerped);
                return false;
            }

            // If player is Prosecutor, apply Prosecutor vision modifier
            else if (Prosecutor.Player != null && Prosecutor.Player.PlayerId == player.PlayerId) 
            {
                float unlerped = Mathf.InverseLerp(__instance.MinLightRadius, __instance.MaxLightRadius, GetNeutralLightRadius(__instance, false));
                __result = Mathf.Lerp(__instance.MinLightRadius, __instance.MaxLightRadius * Prosecutor.vision, unlerped);
                return false;
            }

            // Default light radius
            else {
                __result = GetNeutralLightRadius(__instance, false);
            }
            if (Sunglasses.Players.FindAll(x => x.PlayerId == player.PlayerId).Count > 0) // Sunglasses
                __result *= 1f - Sunglasses.vision * 0.1f;

            return false;
        }

        public static float GetNeutralLightRadius(ShipStatus shipStatus, bool isImpostor) {
            if (SubmergedCompatibility.IsSubmerged) {
                return SubmergedCompatibility.GetSubmergedNeutralLightRadius(isImpostor);
            }

            if (isImpostor) return shipStatus.MaxLightRadius * GameOptionsManager.Instance.currentNormalGameOptions.ImpostorLightMod;
            float lerpValue = 1.0f;
            try {
                SwitchSystem switchSystem = MapUtilities.Systems[SystemTypes.Electrical].CastFast<SwitchSystem>();
                lerpValue = switchSystem.Value / 255f;
            } catch { }

            return Mathf.Lerp(shipStatus.MinLightRadius, shipStatus.MaxLightRadius, lerpValue) * GameOptionsManager.Instance.currentNormalGameOptions.CrewLightMod;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(LogicGameFlowNormal), nameof(LogicGameFlowNormal.IsGameOverDueToDeath))]
        public static void Postfix2(ShipStatus __instance, ref bool __result)
        {
            __result = false;
        }

        private static int originalNumCommonTasksOption = 0;
        private static int originalNumShortTasksOption = 0;
        private static int originalNumLongTasksOption = 0;
        public static float originalNumCrewVisionOption = 0;
        public static float originalNumImpVisionOption = 0;
        public static float originalNumKillCooldownOption = 0;

        [HarmonyPrefix]
        [HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.Begin))]
        public static bool Prefix(ShipStatus __instance)
        {
            originalNumCommonTasksOption = GameOptionsManager.Instance.currentNormalGameOptions.NumCommonTasks;
            originalNumShortTasksOption = GameOptionsManager.Instance.currentNormalGameOptions.NumShortTasks;
            originalNumLongTasksOption = GameOptionsManager.Instance.currentNormalGameOptions.NumLongTasks;

            var commonTaskCount = __instance.CommonTasks.Count;
            var normalTaskCount = __instance.ShortTasks.Count;
            var longTaskCount = __instance.LongTasks.Count;


            if (GameOptionsManager.Instance.currentNormalGameOptions.NumCommonTasks > commonTaskCount) GameOptionsManager.Instance.currentNormalGameOptions.NumCommonTasks = commonTaskCount;
            if (GameOptionsManager.Instance.currentNormalGameOptions.NumShortTasks > normalTaskCount) GameOptionsManager.Instance.currentNormalGameOptions.NumShortTasks = normalTaskCount;
            if (GameOptionsManager.Instance.currentNormalGameOptions.NumLongTasks > longTaskCount) GameOptionsManager.Instance.currentNormalGameOptions.NumLongTasks = longTaskCount;

            MapBehaviourPatch.VentNetworks.Clear();
            return true;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.Begin))]
        public static void Postfix3(ShipStatus __instance)
        {
            // Restore original settings after the tasks have been selected
            GameOptionsManager.Instance.currentNormalGameOptions.NumCommonTasks = originalNumCommonTasksOption;
            GameOptionsManager.Instance.currentNormalGameOptions.NumShortTasks = originalNumShortTasksOption;
            GameOptionsManager.Instance.currentNormalGameOptions.NumLongTasks = originalNumLongTasksOption;
        }

        public static void resetVanillaSettings() {
            GameOptionsManager.Instance.currentNormalGameOptions.ImpostorLightMod = originalNumImpVisionOption;
            GameOptionsManager.Instance.currentNormalGameOptions.CrewLightMod = originalNumCrewVisionOption;
            GameOptionsManager.Instance.currentNormalGameOptions.KillCooldown = originalNumKillCooldownOption;
        }
    }
}
