using UnityEngine;

namespace TheSushiRoles.Patches 
{
    [HarmonyPatch]
    public class DangerMeterPatch 
    {
        [HarmonyPatch(typeof(DangerMeter), nameof(DangerMeter.SetFirstNBarColors))]
        [HarmonyPrefix]
        public static void Prefix(DangerMeter __instance, ref Color color) 
        {
            if (PlayerControl.LocalPlayer != Tracker.Player) return;
            if (__instance == HudManager.Instance.DangerMeter) return;

            color = color.SetAlpha(0.5f);
        }
    }
}