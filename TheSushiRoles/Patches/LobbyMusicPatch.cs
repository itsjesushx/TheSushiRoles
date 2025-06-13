namespace TheSushiRoles.Patches
{
    // Class from: https://github.com/SuperNewRoles/SuperNewRoles
    [HarmonyPatch(typeof(LobbyBehaviour))]
    public class LobbyBehaviourPatch
    {
        [HarmonyPatch(nameof(LobbyBehaviour.Update)), HarmonyPostfix]
        public static void Update_Postfix(LobbyBehaviour __instance)    
        {
            Func<ISoundPlayer, bool> uglyMusic = x => x.Name.Equals("MapTheme");
            ISoundPlayer LobbyUglyMusic = SoundManagerInstance().soundPlayers.Find(uglyMusic);
            if (MapOptions.DisableLobbyMusic)
            {
                if (LobbyUglyMusic == null) return;
                SoundManagerInstance().StopNamedSound("MapTheme");
            }
            else
            {
                if (LobbyUglyMusic != null) return;
                SoundManagerInstance().CrossFadeSound("MapTheme", __instance.MapTheme, 0.5f);
            }
        }
    }
}