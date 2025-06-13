using UnityEngine.UI;
using AmongUs.Data;
using Assets.InnerNet;

namespace TheSushiRoles.Modules 
{
    [HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.Start))]
    public class MainMenuPatch 
    {
        private static AnnouncementPopUp popUp;
        private static void Prefix(MainMenuManager __instance) 
        {
            // Force Reload of SoundEffectHolder
            SoundEffectsManager.Load();

            Application.targetFrameRate = 165;
            
            var template = GameObject.Find("ExitGameButton");
            var template2 = GameObject.Find("CreditsButton");
            if (template == null || template2 == null) return;
            template.transform.localScale = new Vector3(0.42f, 0.84f, 0.84f);
            template.GetComponent<AspectPosition>().anchorPoint = new Vector2(0.625f, 0.5f);
            template.transform.FindChild("FontPlacer").transform.localScale = new Vector3(1.8f, 0.9f, 0.9f);
            template.transform.FindChild("FontPlacer").transform.localPosition = new Vector3(-1.1f, 0f, 0f);

            template2.transform.localScale = new Vector3(0.42f, 0.84f, 0.84f);
            template2.GetComponent<AspectPosition>().anchorPoint = new Vector2(0.378f, 0.5f);
            template2.transform.FindChild("FontPlacer").transform.localScale = new Vector3(1.8f, 0.9f, 0.9f);
            template2.transform.FindChild("FontPlacer").transform.localPosition = new Vector3(-1.1f, 0f, 0f);



            var buttonGithub = UObject.Instantiate(template, template.transform.parent);
            buttonGithub.transform.localScale = new Vector3(0.42f, 0.84f, 0.84f);
            buttonGithub.GetComponent<AspectPosition>().anchorPoint = new Vector2(0.542f, 0.5f);

            var textGithub = buttonGithub.transform.GetComponentInChildren<TMP_Text>();
            __instance.StartCoroutine(Effects.Lerp(0.5f, new System.Action<float>((p) => {
                textGithub.SetText("Mod Github");
            })));
            PassiveButton passiveButtonGithub = buttonGithub.GetComponent<PassiveButton>();
            
            passiveButtonGithub.OnClick = new Button.ButtonClickedEvent();
            passiveButtonGithub.OnClick.AddListener((Action)(() => Application.OpenURL("https://github.com/itsjesushx/TheSushiRoles")));
            
            // TSR credits button
            if (template == null) return;
            var creditsButton = UObject.Instantiate(template, template.transform.parent);

            creditsButton.transform.localScale = new Vector3(0.42f, 0.84f, 0.84f);
            creditsButton.GetComponent<AspectPosition>().anchorPoint = new Vector2(0.462f, 0.5f);

            var textCreditsButton = creditsButton.transform.GetComponentInChildren<TMP_Text>();
            __instance.StartCoroutine(Effects.Lerp(0.5f, new Action<float>((p) =>
            {
                textCreditsButton.SetText("Mod Credits");
            })));
            PassiveButton passiveCreditsButton = creditsButton.GetComponent<PassiveButton>();

            passiveCreditsButton.OnClick = new Button.ButtonClickedEvent();

            passiveCreditsButton.OnClick.AddListener((Action)delegate
            {
                // do stuff
                if (popUp != null) UObject.Destroy(popUp);
                var popUpTemplate = UObject.FindObjectOfType<AnnouncementPopUp>(true);
                if (popUpTemplate == null) 
                {
                    TheSushiRoles.Logger.LogError("couldnt show credits, popUp is null");
                    return;
                }
                popUp = UObject.Instantiate(popUpTemplate);

                popUp.gameObject.SetActive(true);
                string creditsString = @$"<align=""center"">Special Thanks:
[https://github.com/TheOtherRolesAU/TheOtherRoles]TheOtherRoles[] - Original Codebase

";
                creditsString += $@"<size=60%>
Thanks to <link=""https://github.com/TheOtherRolesAU/TheOtherRoles""><color=#00BFFF>TheOtherRoles</color></link> for providing the mod's original codebase!

<link=""https://github.com/myDragonBreath/AmongUs.MultiClientInstancing"">MyDragonBreath's AmongUs MultiClientInstancing</link> - for TheSushiRoles Debugger.
<link=""https://github.com/NuclearPowered/Reactor"">Reactor</link> - The framework used.
<link=""https://github.com/BepInEx"">BepInEx</link> - Used to hook to game functions.
<link=""https://github.com/mr-fluuff/StellarRolesAU"">StellarRoles</link> - code snippets for the MinerVent class.
<link=""https://github.com/Woodi-dev/Among-Us-Love-Couple-Mod"">Among-Us-Love-Couple-Mod</link> - Idea for the Lovers modifier came from <b>Woodi-dev</b>
<link=""https://github.com/Maartii/Jester"">Jester</link> - Idea for the Jester role came from <b>Maartii</b>
<link=""https://github.com/NotHunter101/ExtraRolesAmongUs"">ExtraRolesAmongUs</link> - Idea for the Engineer and Medic role came from <b>NotHunter101</b>. Also some code snippets from their implementation were used.
<link=""https://github.com/Woodi-dev/Among-Us-Sheriff-Mod"">Among-Us-Sheriff-Mod</link> - Idea for the Sheriff role came from <b>Woodi-dev</b>
<link=""https://github.com/Hardel-DW/TooManyRolesMods"">TooManyRolesMods</link> - Idea for the Detective and Chronos roles came from <b>Hardel-DW</b>.
<link=""https://github.com/slushiegoose/Town-Of-Us"">TownOfUs</link> - Idea for the Arsonist and a similar Mayor role came from <b>Slushiegoose</b>
<link=""https://twitter.com/ottomated_"">Ottomated</link> - Idea for the Morphling and Painter role came from <b>Ottomated</b>
<link=""https://github.com/CrowdedMods/CrowdedMod"">Crowded-Mod</link> - Our implementation for 10+ player lobbies were inspired by the one from the <b>Crowded Mod Team</b>
<link=""https://store.steampowered.com/app/1568590/Goose_Goose_Duck"">Goose-Goose-Duck</link> - Idea for the Scavenger role came from <b>Slushiegoose</b>
<link=""https://github.com/LaicosVK/TheEpicRoles"">TheEpicRoles</link> - Idea for the first kill shield and the tabbed option menu, by <b>LaicosVK</b> <b>DasMonschta</b> <b>Nova</b>
<link=""https://github.com/Brybry16/BetterPolus"">Brybry16</link> - For the Better Polus Implementation.
<link=""https://github.com/eDonnes124/Town-Of-Us-R"">Town of Us-R</link> - Idea for the Veteran, Undertaker, Juggernaut, Plaguebearer & Pestilence, Glitch, Oracle, Predator and a similar Amnesiac role came from <b>eDonnes124</b>
</size>";
                creditsString += "</align>";

                Announcement creditsAnnouncement = new()
                {
                    Id = "ModCredits",
                    Language = 0,
                    Number = 500,
                    Title = "The Sushi Roles\nCredits & Resources",
                    ShortTitle = "TSR Credits",
                    SubTitle = "",
                    PinState = false,
                    Date = "01.07.2021",
                    Text = creditsString,
                };
                __instance.StartCoroutine(Effects.Lerp(0.1f, new Action<float>((p) =>
                {
                    if (p == 1) {
                        var backup = DataManager.Player.Announcements.allAnnouncements;
                        DataManager.Player.Announcements.allAnnouncements = new();
                        popUp.Init(false);
                        DataManager.Player.Announcements.SetAnnouncements(new Announcement[] { creditsAnnouncement });
                        popUp.CreateAnnouncementList();
                        popUp.UpdateAnnouncementText(creditsAnnouncement.Number);
                        popUp.visibleAnnouncements._items[0].PassiveButton.OnClick.RemoveAllListeners();
                        DataManager.Player.Announcements.allAnnouncements = backup;
                    }
                })));
            });
        }
    }
}
