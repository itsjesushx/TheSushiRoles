namespace TheSushiRoles.Patches
{
    [HarmonyPatch(typeof(MedScanMinigame), nameof(MedScanMinigame.FixedUpdate))]
    class MedScanMinigameFixedUpdatePatch 
    {
        static void Prefix(MedScanMinigame __instance) 
        {
            if (CustomGameOptions.AllowParallelMedBayScans) 
            {
                __instance.medscan.CurrentUser = PlayerControl.LocalPlayer.PlayerId;
                __instance.medscan.UsersList.Clear();
            }
        }
    }
    [HarmonyPatch(typeof(MedScanMinigame))]
    class MedScanMinigameBeginPatch
    {
        [HarmonyPatch(nameof(MedScanMinigame.Begin))]
        [HarmonyPostfix]
        private static void BeginPostfix(MedScanMinigame __instance)
        {
            // Update medical details for Giant and mini
            if (PlayerControl.LocalPlayer == Giant.Player)
            {
                __instance.completeString = __instance.completeString.Replace("3' 6\"", "5' 3\"").Replace("92lb", "184lb");
            }
            if (PlayerControl.LocalPlayer == Mini.Player)
            {
                __instance.completeString = __instance.completeString.Replace("3' 6\"", "2' 4\"").Replace("92lb", "45lb");
            }
        }
    }
    [HarmonyPatch(typeof(MedScanMinigame._WalkToOffset_d__15), nameof(MedScanMinigame._WalkToPad_d__16.MoveNext))]
    class DisableMedbayAnimation
    {
        static bool Prefix(MedScanMinigame._WalkToPad_d__16 __instance)
        {
            if (CustomGameOptions.DisableMedbayAnimation)
            {
                MedScanMinigame medScanMinigame = __instance.__4__this;
                switch (__instance.__1__state)
                {
                    case 0:
                        __instance.__1__state = -1;
                        medScanMinigame.state = MedScanMinigame.PositionState.WalkingToPad;
                        __instance.__1__state = 1;
                        return true;
                    case 1:
                        __instance.__1__state = -1;
                        __instance.__2__current = new WaitForSeconds(0.1f);
                        __instance.__1__state = 2;
                        return true;
                    case 2:
                        __instance.__1__state = -1;
                        medScanMinigame.walking = null;
                        return false;
                    default:
                        return false;
                }
            }
            return false;
        }
    }
    [HarmonyPatch(typeof(MedScanMinigame._WalkToOffset_d__15), nameof(MedScanMinigame._WalkToOffset_d__15.MoveNext))]
    class DisableMedbayAnimation2
    {
        static bool Prefix(MedScanMinigame._WalkToOffset_d__15 __instance)
        {
            if (CustomGameOptions.DisableMedbayAnimation)
            {
                MedScanMinigame medScanMinigame = __instance.__4__this;
                switch (__instance.__1__state)
                {
                    case 0:
                        __instance.__1__state = -1;
                        medScanMinigame.state = MedScanMinigame.PositionState.WalkingToOffset;
                        __instance.__1__state = 1;
                        return true;
                    case 1:
                        __instance.__1__state = -1;
                        __instance.__2__current = new WaitForSeconds(0.1f);
                        __instance.__1__state = 2;
                        return true;
                    case 2:
                        __instance.__1__state = -1;
                        medScanMinigame.walking = null;
                        return false;
                    default:
                        return false;
                }
            }
            return false;
        }
    }
}