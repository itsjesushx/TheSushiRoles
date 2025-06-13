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
public static class PlayerPhysics_FixedUpdate
{
    public static void Postfix(PlayerPhysics __instance)
    {
        if (__instance.myPlayer == null) return;
        if (!__instance.AmOwner || !GameData.Instance || !__instance.myPlayer.CanMove) return;

        switch (__instance.myPlayer)
        {
            case var player when player == Undertaker.Player:
                if (Undertaker.CurrentTarget != null)
                    __instance.body.velocity *= CustomGameOptions.UndertakerDragSpeed;
                break;
            case var player when player == Hitman.Player:
                if (Hitman.BodyTarget != null)
                    __instance.body.velocity *= CustomGameOptions.HitmanDragSpeed;
                else if (Hitman.MorphTarget == Giant.Player && Hitman.MorphTimer > 0f)
                    __instance.body.velocity *= CustomGameOptions.ModifierGiantSpeed;
                break;
            case var player when player == Morphling.Player:
                if (Morphling.morphTarget == Giant.Player && Morphling.morphTimer > 0f)
                    __instance.body.velocity *= CustomGameOptions.ModifierGiantSpeed;
                else if (Morphling.morphTarget == Mini.Player && Morphling.morphTimer > 0f)
                    __instance.body.velocity *= CustomGameOptions.ModifierMiniSpeed;
                break;
            case var player when player == Glitch.Player:
                if (Glitch.MimicTarget == Giant.Player && Glitch.MimicTimer > 0f)
                    __instance.body.velocity *= CustomGameOptions.ModifierGiantSpeed;
                else if (Glitch.MimicTarget == Mini.Player && Glitch.MimicTimer > 0f)
                    __instance.body.velocity *= CustomGameOptions.ModifierMiniSpeed;
                break;
            case var player when player == Giant.Player:
                __instance.body.velocity *= CustomGameOptions.ModifierGiantSpeed;
                break;
            case var player when player == Mini.Player:
                __instance.body.velocity *= CustomGameOptions.ModifierMiniSpeed;
                break;
        }
    }
}