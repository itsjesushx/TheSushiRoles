using System;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;
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



            var buttonGithub = UnityEngine.Object.Instantiate(template, template.transform.parent);
            buttonGithub.transform.localScale = new Vector3(0.42f, 0.84f, 0.84f);
            buttonGithub.GetComponent<AspectPosition>().anchorPoint = new Vector2(0.542f, 0.5f);

            var textGithub = buttonGithub.transform.GetComponentInChildren<TMPro.TMP_Text>();
            __instance.StartCoroutine(Effects.Lerp(0.5f, new System.Action<float>((p) => {
                textGithub.SetText("TSR Github");
            })));
            PassiveButton passiveButtonGithub = buttonGithub.GetComponent<PassiveButton>();
            
            passiveButtonGithub.OnClick = new Button.ButtonClickedEvent();
            passiveButtonGithub.OnClick.AddListener((System.Action)(() => Application.OpenURL("https://github.com/itsjesushx/TheSushiRoles")));


            
            // TSR credits button
            if (template == null) return;
            var creditsButton = Object.Instantiate(template, template.transform.parent);

            creditsButton.transform.localScale = new Vector3(0.42f, 0.84f, 0.84f);
            creditsButton.GetComponent<AspectPosition>().anchorPoint = new Vector2(0.462f, 0.5f);

            var textCreditsButton = creditsButton.transform.GetComponentInChildren<TMPro.TMP_Text>();
            __instance.StartCoroutine(Effects.Lerp(0.5f, new System.Action<float>((p) => {
                textCreditsButton.SetText("TSR Credits");
            })));
            PassiveButton passiveCreditsButton = creditsButton.GetComponent<PassiveButton>();

            passiveCreditsButton.OnClick = new Button.ButtonClickedEvent();

            passiveCreditsButton.OnClick.AddListener((System.Action)delegate {
                // do stuff
                if (popUp != null) Object.Destroy(popUp);
                var popUpTemplate = Object.FindObjectOfType<AnnouncementPopUp>(true);
                if (popUpTemplate == null) 
                {
                    TheSushiRolesPlugin.Logger.LogError("couldnt show credits, popUp is null");
                    return;
                }
                popUp = Object.Instantiate(popUpTemplate);

                popUp.gameObject.SetActive(true);
                string creditsString = @$"<align=""center"">Special Thanks:
[https://github.com/TheOtherRolesAU/TheOtherRoles]TheOtherRoles[] - Original Codebase

";
                creditsString += $@"<size=60%> Other Credits & Resources:
TownOfUs-Reactivated - Added many roles and features from the mod.
myDragonBreath - for the MCI debugger.
OxygenFilter - For the versions v2.3.0 to v2.6.1, we were using the OxygenFilter for automatic deobfuscation
Reactor - The framework used for all versions before v2.0.0, and again since 4.2.0
BepInEx - Used to hook game functions
Essentials - Custom game options by DorCoMaNdO:
Before v1.6: We used the default Essentials release
v1.6-v1.8: We slightly changed the default Essentials.
v2.0.0 and later: As we were not using Reactor anymore, we are using our own implementation, inspired by the one from DorCoMaNdO
Jackal and Sidekick - Original idea for the Jackal and Sidekick came from Dhalucard
Among-Us-Love-Couple-Mod - Idea for the Lovers modifier comes from Woodi-dev
Jester - Idea for the Jester role came from Maartii
ExtraRolesAmongUs - Idea for the Engineer and Medic role came from NotHunter101. Also some code snippets from their implementation were used.
Among-Us-Sheriff-Mod - Idea for the Sheriff role came from Woodi-dev
TooManyRolesMods - Idea for the Detective and Chronos roles comes from Hardel-DW. Also some code snippets from their implementation were used.
TownOfUs - Idea for the Swapper, Arsonist and a similar Mayor role came from Slushiegoose
Ottomated - Idea for the Morphling and Painter role came from Ottomated
Crowded-Mod - Our implementation for 10+ player lobbies was inspired by the one from the Crowded Mod Team
Goose-Goose-Duck - Idea for the Scavenger role came from Slushiegoose
TheEpicRoles - Idea for the first kill Shield (partly) and the tabbed option menu (fully + some code), by LaicosVK DasMonschta Nova
ugackMiner53 - Idea and core code for the Prop Hunt game mode</size>";
                creditsString += "</align>";

                Assets.InnerNet.Announcement creditsAnnouncement = new() {
                    Id = "torCredits",
                    Language = 0,
                    Number = 500,
                    Title = "The Sushi Roles\nCredits & Resources",
                    ShortTitle = "TSR Credits",
                    SubTitle = "",
                    PinState = false,
                    Date = "01.07.2021",
                    Text = creditsString,
                };
                __instance.StartCoroutine(Effects.Lerp(0.1f, new Action<float>((p) => {
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
