namespace TheSushiRoles.Patches
{
    [HarmonyPatch(typeof(ChatController), nameof(ChatController.Awake))]
    public static class QuickChatDisablePatch
    {
        private static void Postfix(ChatController __instance)
        {
            __instance.quickChatButton.gameObject.SetActive(false);
            __instance.openKeyboardButton.gameObject.SetActive(false);
        }
    }
}