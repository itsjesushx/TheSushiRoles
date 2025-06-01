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
                GameOptionsManager.Instance.currentNormalGameOptions.RoleOptions.SetRoleRate(RoleTypes.Scientist, 0, 0);
                GameOptionsManager.Instance.currentNormalGameOptions.RoleOptions.SetRoleRate(RoleTypes.Engineer, 0, 0);
                GameOptionsManager.Instance.currentNormalGameOptions.RoleOptions.SetRoleRate(RoleTypes.GuardianAngel, 0, 0);
                GameOptionsManager.Instance.currentNormalGameOptions.RoleOptions.SetRoleRate(RoleTypes.Tracker, 0, 0);
                GameOptionsManager.Instance.currentNormalGameOptions.RoleOptions.SetRoleRate(RoleTypes.Noisemaker, 0, 0);
                GameOptionsManager.Instance.currentNormalGameOptions.RoleOptions.SetRoleRate(RoleTypes.Shapeshifter, 0, 0);
                GameOptionsManager.Instance.currentNormalGameOptions.RoleOptions.SetRoleRate(RoleTypes.Phantom, 0, 0);
                Utils.SendRPC(CustomRPC.DisableVanillaRoles);
            }
            return true;
        }
    }
}