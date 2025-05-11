using Discord;

namespace TheSushiRoles.Patches;

[HarmonyPatch]
internal class DiscordStatusManager
{
    [HarmonyPatch(typeof(ActivityManager), nameof(ActivityManager.UpdateActivity))]
    [HarmonyPrefix]
    public static void Prefix([HarmonyArgument(0)] Activity activity)
    {
        activity.Details += $" TheSushiRoles v" + TheSushiRolesPlugin.VersionString;
    }
}