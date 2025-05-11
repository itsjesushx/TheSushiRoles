namespace TheSushiRoles.Patches
{
    [HarmonyPatch(typeof(PlayerPurchasesData), nameof(PlayerPurchasesData.GetPurchase))]
    public static class GetPurchasePatch
    {
        public static bool Prefix(ref bool __result)
        {
            __result = true;
            return false;
        }
    }

    [HarmonyPatch(typeof(PlayerPurchasesData), nameof(PlayerPurchasesData.SetPurchased))]
    public static class SetPurchasedPatch
    {
        public static bool Prefix() => false;
    }
}