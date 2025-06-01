using TMPro;
using UnityEngine;

namespace TheSushiRoles.Patches
{
    [HarmonyPatch]
    public static class CredentialsPatch 
    {
    public static string MainScreenText =
    $@"Created by <color=#B2FEFE>Jesushi</color>
<size=75%>Emotionally Helped by <color=#B2FEFE>döll</color>
Helped by <color=#B2FEFE>AlchlcDvl</color> & <color=#B2FEFE>50IQ</color>
Testing help by <color=#B2FEFE>Cake</color>";


        [HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.Start))]
        public static class LogoPatch
        {
            public static SpriteRenderer renderer;
            public static Sprite bannerSprite;
            private static PingTracker instance;

            static void Postfix(PingTracker __instance) 
            {
                var Logo = new GameObject("bannerLogo_TSR");
                Logo.transform.SetParent(GameObject.Find("RightPanel").transform, false);
                Logo.transform.localPosition = new Vector3(-0.4f, 0.5f, 5f);
                renderer = Logo.AddComponent<SpriteRenderer>();
                LoadSprites();
                renderer.sprite = Utils.LoadSprite("TheSushiRoles.Resources.TheSushiRolesBanner.png", 130f);
                instance = __instance;
                LoadSprites();
                renderer.sprite = bannerSprite;
                var CredentialObject = new GameObject("CredentialsTSR");
                var Credentials = CredentialObject.AddComponent<TextMeshPro>();
                Credentials.SetText($"v{TheSushiRolesPlugin.Version.ToString()}\n<size=30f%>\n</size>{MainScreenText}\n<size=30%>\n</size>");
                Credentials.alignment = TextAlignmentOptions.Center;
                Credentials.fontSize *= 0.05f;

                Credentials.transform.SetParent(Logo.transform);
                Credentials.transform.localPosition = Vector3.down * 1.45f;
            }

            public static void LoadSprites() 
            {
                if (bannerSprite == null) bannerSprite = Utils.LoadSprite("TheSushiRoles.Resources.TheSushiRolesBanner.png", 110f);
            }
        }
    }
}