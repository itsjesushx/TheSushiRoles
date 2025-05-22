using UnityEngine;

namespace TheSushiRoles.Patches;

[HarmonyPatch(typeof(PlayerPhysics), nameof(PlayerPhysics.Awake))]
public static class PlayerPhysiscs_Awake_Patch
{
    [HarmonyPostfix]
    public static void Postfix(PlayerPhysics __instance)
    {
        if (!__instance.body) return;
        __instance.body.interpolation = RigidbodyInterpolation2D.Interpolate;
    }
}
    [HarmonyPatch(typeof(PlayerPhysics), nameof(PlayerPhysics.FixedUpdate))]
    public static class UndertakerPlayerPhysics_FixedUpdate
    {
    public static void Postfix(PlayerPhysics __instance)
    {
        if (__instance.myPlayer == Undertaker.Player)
        {
            if (Undertaker.CurrentTarget != null)
                if (__instance.AmOwner && GameData.Instance && __instance.myPlayer.CanMove)
                    __instance.body.velocity *= Undertaker.DragSpeed;
        }
        else if (__instance.myPlayer == Hitman.Player)
        {
            if (Hitman.BodyTarget != null)
                if (__instance.AmOwner && GameData.Instance && __instance.myPlayer.CanMove)
                    __instance.body.velocity *= Hitman.DragSpeed;
        }
            else if (__instance.myPlayer == Giant.Player)
            {
                if (__instance.AmOwner && GameData.Instance && __instance.myPlayer.CanMove)
                    __instance.body.velocity *= Giant.Speed;
            }
        }
    }