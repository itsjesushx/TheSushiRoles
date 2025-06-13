namespace TheSushiRoles.Patches
{
    [HarmonyPatch]
    class DisableVanillaRolesPatch
    {
        [HarmonyPatch(typeof(GameStartManager), nameof(GameStartManager.BeginGame))]
        [HarmonyPrefix]
        public static bool Prefix()
        {
            if (AmongUsClient.Instance.AmHost)
            {
                Utils.SendRPC(CustomRPC.DisableVanillaRoles);
                RPCProcedure.DisableVanillaRoles();
            }
            return true;
        }
    }
}