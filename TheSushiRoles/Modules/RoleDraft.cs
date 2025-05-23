using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Hazel;
using BepInEx.Unity.IL2CPP.Utils.Collections;
using System.Collections;
using UnityEngine.UI;
using Reactor.Utilities.Extensions;

namespace TheSushiRoles.Modules
{
    [HarmonyPatch]
    class RoleDraft
    {
        public static bool isEnabled => CustomOptionHolder.isDraftMode.GetBool();
        public static bool IsRunning = false;
        public static List<byte> pickOrder = new();
        private static bool picked = false;
        private static float timer = 0f;
        private static List<ActionButton> buttons = new List<ActionButton>();
        private static TMPro.TextMeshPro feedText;
        public static List<byte> alreadyPicked = new();
        public static IEnumerator CoSelectRoles(IntroCutscene __instance)
        {
            IsRunning = true;
            alreadyPicked.Clear();
            bool playedAlert = false;
            feedText = UnityEngine.Object.Instantiate(__instance.TeamTitle, __instance.transform);
            var aspectPosition = feedText.gameObject.AddComponent<AspectPosition>();
            aspectPosition.Alignment = AspectPosition.EdgeAlignments.LeftTop;
            aspectPosition.DistanceFromEdge = new Vector2(1.62f, 1.2f);
            aspectPosition.AdjustPosition();
            feedText.transform.localScale = new Vector3(0.6f, 0.6f, 1);
            feedText.text = "<size=200%>Player's Picks:</size>\n\n";
            feedText.alignment = TMPro.TextAlignmentOptions.TopLeft;
            feedText.autoSizeTextContainer = true;
            feedText.fontSize = 3f;
            feedText.enableAutoSizing = false;
            __instance.TeamTitle.transform.localPosition = __instance.TeamTitle.transform.localPosition + new Vector3(1f, 0f);
            __instance.TeamTitle.text = "Currently Picking:";
            __instance.BackgroundBar.enabled = false;
            __instance.TeamTitle.transform.localScale = new Vector3(0.25f, 0.25f, 1f);
            __instance.TeamTitle.autoSizeTextContainer = true;
            __instance.TeamTitle.enableAutoSizing = false;
            __instance.TeamTitle.fontSize = 5;
            __instance.TeamTitle.alignment = TMPro.TextAlignmentOptions.Top;
            __instance.ImpostorText.gameObject.SetActive(false);
            GameObject.Find("BackgroundLayer")?.SetActive(false);
            foreach (var player in UnityEngine.Object.FindObjectsOfType<PoolablePlayer>())
            {
                if (player.name.Contains("Dummy"))
                {
                    player.gameObject.SetActive(false);
                }
            }
            __instance.FrontMost.gameObject.SetActive(false);

            if (AmongUsClient.Instance.AmHost)
            {
                SendPickOrder();
            }

            while (pickOrder.Count == 0)
            {
                yield return null;
            }

            while (pickOrder.Count > 0)
            {
                picked = false;
                timer = 0;
                float maxTimer = CustomOptionHolder.draftModeTimeToChoose.GetFloat();
                string playerText = "";
                while (timer < maxTimer || !picked)
                {
                    if (pickOrder.Count == 0)
                        break;
                    // wait for pick
                    timer += Time.deltaTime;
                    if (PlayerControl.LocalPlayer.PlayerId == pickOrder[0])
                    {
                        if (!playedAlert)
                        {
                            playedAlert = true;
                            SoundManager.Instance.PlaySound(ShipStatus.Instance.SabotageSound, false, 1f, null);
                        }
                        // Animate beginning of choice, by changing background color
                        float min = 50 / 255f;
                        Color backGroundColor = new Color(min, min, min, 1);
                        if (timer < 1)
                        {
                            float max = 230 / 255f;
                            if (timer < 0.5f)
                            { // White flash                              
                                float p = timer / 0.5f;
                                float value = (float)Math.Pow(p, 2f) * max;
                                backGroundColor = new Color(value, value, value, 1);
                            } else
                            {
                                float p = (1 - timer) / 0.5f;
                                float value = (float)Math.Pow(p, 2f) * max + (1 - (float)Math.Pow(p, 2f)) * min;
                                backGroundColor = new Color(value, value, value, 1);
                            }

                        }
                        HudManager.Instance.FullScreen.color = backGroundColor;
                        GameObject.Find("BackgroundLayer")?.SetActive(false);

                        // enable pick, wait for pick
                        Color youColor = timer - (int)timer > 0.5 ? Color.red : Color.yellow;
                        playerText = Utils.ColorString(youColor, "You!");
                        // Available Roles:
                        List<RoleInfo> availableRoles = new();
                        foreach (RoleInfo roleInfo in RoleInfo.allRoleInfos) 
                        {
                            int impostorCount = PlayerControl.AllPlayerControls.ToArray().ToList().Where(x => x.Data.Role.IsImpostor).Count();
                            // Remove Impostor Roles
                            if (PlayerControl.LocalPlayer.Data.Role.IsImpostor && roleInfo.FactionId != Faction.Impostors) continue;
                            if (!PlayerControl.LocalPlayer.Data.Role.IsImpostor && roleInfo.FactionId == Faction.Impostors) continue;

                            RoleManagerSelectRolesPatch.RoleAssignmentData roleData = RoleManagerSelectRolesPatch.getRoleAssignmentData();
                            roleData.CrewSettings.Add((byte)RoleId.Sheriff, CustomOptionHolder.sheriffSpawnRate.GetSelection());
                            if (roleData.NeutralBenignSettings.ContainsKey((byte)roleInfo.RoleId) && roleData.NeutralBenignSettings[(byte)roleInfo.RoleId] == 0) continue;
                            else if (roleData.ImpSettings.ContainsKey((byte)roleInfo.RoleId) && roleData.ImpSettings[(byte)roleInfo.RoleId] == 0) continue;
                            else if (roleData.NeutralEvilSettings.ContainsKey((byte)roleInfo.RoleId) && roleData.NeutralEvilSettings[(byte)roleInfo.RoleId] == 0) continue;
                            else if (roleData.NeutralKillingSettings.ContainsKey((byte)roleInfo.RoleId) && roleData.NeutralKillingSettings[(byte)roleInfo.RoleId] == 0) continue;
                            else if (roleData.CrewSettings.ContainsKey((byte)roleInfo.RoleId) && roleData.CrewSettings[(byte)roleInfo.RoleId] == 0) continue;
                            if (roleInfo.RoleId == RoleId.Survivor) continue;
                            if (roleInfo.RoleId == RoleId.Spy && impostorCount < 2) continue;
                            if (alreadyPicked.Contains((byte)roleInfo.RoleId) && roleInfo.RoleId != RoleId.Crewmate) continue;
                            if (roleInfo.RoleId == RoleId.Crewmate) continue;

                            int impsPicked = alreadyPicked.Where(x => RoleInfo.RoleInfoById[(RoleId)x].isImpostor).Count();

                            // Hanlde forcing of 100% roles for impostors
                            if (PlayerControl.LocalPlayer.Data.Role.IsImpostor)
                            {
                                int impsMax = CustomOptionHolder.impostorRolesCountMax.GetSelection();
                                int impsMin = CustomOptionHolder.impostorRolesCountMin.GetSelection();
                                if (impsMin > impsMax) impsMin = impsMax;
                                int impsLeft = pickOrder.Where(x => Utils.PlayerById(x).Data.Role.IsImpostor).Count();
                                int imps100 = roleData.ImpSettings.Where(x => x.Value == 10).Count();
                                if (imps100 > impsMax) imps100 = impsMax;
                                int imps100Picked = alreadyPicked.Where(x => roleData.ImpSettings.GetValueSafe(x) == 10).Count();
                                if (imps100 - imps100Picked >= impsLeft && !(roleData.ImpSettings.Where(x => x.Value == 10 && x.Key == (byte)roleInfo.RoleId).Count() > 0)) continue;
                                if (impsMin - impsPicked >= impsLeft && roleInfo.RoleId == RoleId.Impostor) continue;
                                if (impsPicked >= impsMax && roleInfo.RoleId != RoleId.Impostor) continue;
                            }

                            // Player is no impostor! Handle forcing of 100% roles for crew and neutral
                            else 
                            {
                                // No more neutrals possible!
                                int neutralBenignsPicked = alreadyPicked.Where(x => RoleInfo.RoleInfoById[(RoleId)x].Alignment == RoleAlignment.NeutralBenign).Count();
                                int neutralEvilsPicked = alreadyPicked.Where(x => RoleInfo.RoleInfoById[(RoleId)x].Alignment == RoleAlignment.NeutralEvil).Count();
                                int neutralsKillersPicked = alreadyPicked.Where(x => RoleInfo.RoleInfoById[(RoleId)x].Alignment == RoleAlignment.NeutralKilling).Count();
                                int crewPicked = alreadyPicked.Where(x => RoleInfo.RoleInfoById[(RoleId)x].FactionId == Faction.Crewmates).Count();
                                
                                int neutralBenignsMax = CustomOptionHolder.MaxNeutralBenignRoles.GetSelection();
                                int neutralBenignsMin = CustomOptionHolder.MinNeutralBenignRoles.GetSelection();
                                int neutralEvilsMax = CustomOptionHolder.MaxNeutralEvilRoles.GetSelection();
                                int neutralEvilsMin = CustomOptionHolder.MinNeutralEvilRoles.GetSelection();
                                int neutralsKMin = CustomOptionHolder.neutralKillingRolesCountMin.GetSelection();
                                int neutralsKMax = CustomOptionHolder.neutralKillingRolesCountMax.GetSelection();

                                int neutralBenigns100 = roleData.NeutralBenignSettings.Where(x => x.Value == 10).Count();
                                int neutralEvils100 = roleData.NeutralEvilSettings.Where(x => x.Value == 10).Count();
                                int neutralsK100 = roleData.NeutralKillingSettings.Where(x => x.Value == 10).Count();

                                if (neutralsK100 > neutralsKMin) neutralsKMin = neutralsK100;
                                if (neutralsKMin > neutralsKMax) neutralsKMin = neutralsKMax;

                                if (neutralBenigns100 > neutralBenignsMin) neutralBenignsMin = neutralBenigns100;
                                if (neutralBenignsMin > neutralBenignsMax) neutralBenignsMin = neutralBenignsMax;

                                if (neutralEvils100 > neutralEvilsMin) neutralEvilsMin = neutralEvils100;
                                if (neutralEvilsMin > neutralEvilsMax) neutralEvilsMin = neutralEvilsMax;

                                // If crewmate fill disabled and crew picked the amount of allowed crewmates alreay: no more crewmate except vanilla crewmate allowed!
                                int crewLimit = PlayerControl.AllPlayerControls.Count - impostorCount - (neutralEvilsMin > neutralBenigns100 ? neutralBenignsMin : neutralBenigns100 > neutralBenignsMax ? neutralBenignsMax : neutralBenigns100) - (neutralEvilsMin > neutralEvils100 ? neutralEvilsMin : neutralEvils100 > neutralEvilsMax ? neutralEvilsMax : neutralEvils100) - (neutralsKMin > neutralsK100 ? neutralsKMin : neutralsK100 > neutralsKMax ? neutralsKMax : neutralsK100);
                                int maxCrew = crewLimit;
                                if (maxCrew > crewLimit)
                                    maxCrew = crewLimit;
                                if (crewPicked >= crewLimit && roleInfo.FactionId != Faction.Neutrals && roleInfo.Alignment != RoleAlignment.NeutralKilling && roleInfo.RoleId != RoleId.Crewmate) continue;
                                // no crewmates allowed!
                                if (roleInfo.RoleId == RoleId.Crewmate) continue;

                                bool allowAnyNeutralK = false;
                                if (neutralsKillersPicked >= neutralsKMax && roleInfo.Alignment == RoleAlignment.NeutralKilling) continue;
                                // More neutrals needed? Then no more crewmates! This takes precedence over crew roles set to 100%!
                                var crewmatesLeft1 = pickOrder.Count - pickOrder.Where(x => Utils.PlayerById(x).Data.Role.IsImpostor).Count();
                                var crewmatesLeft = pickOrder.Count - pickOrder.Where(x => Utils.PlayerById(x).Data.Role.IsImpostor).Count();

                                bool allowAnyNeutralBenign = false;
                                if (neutralBenignsPicked >= neutralBenignsMax && roleInfo.Alignment == RoleAlignment.NeutralBenign) continue;
                                crewmatesLeft = pickOrder.Count - pickOrder.Where(x => Utils.PlayerById(x).Data.Role.IsImpostor).Count();
                                
                                bool allowAnyNeutralEvil = false;
                                if (neutralBenignsPicked >= neutralBenignsMax && roleInfo.Alignment == RoleAlignment.NeutralBenign) continue;
                                crewmatesLeft = pickOrder.Count - pickOrder.Where(x => Utils.PlayerById(x).Data.Role.IsImpostor).Count();

                                if (crewmatesLeft <= neutralBenignsMin - neutralBenignsPicked && roleInfo.Alignment != RoleAlignment.NeutralBenign
                                || crewmatesLeft <= neutralsKMin - neutralsKillersPicked && roleInfo.Alignment != RoleAlignment.NeutralKilling
                                || crewmatesLeft <= neutralEvilsMin - neutralEvilsPicked && roleInfo.Alignment != RoleAlignment.NeutralEvil)
                                {
                                    continue;
                                }
                                else if (neutralBenignsMin - neutralBenigns100 > neutralBenignsPicked)
                                    allowAnyNeutralBenign = true;
                                else if (neutralEvilsMin - neutralEvils100 > neutralEvilsPicked)
                                    allowAnyNeutralEvil = true;
                                else if (neutralsKMin - neutralsK100 > neutralsKillersPicked)
                                    allowAnyNeutralK = true;
                                // Handle 100% Roles PER Faction.

                                int neutralEvils100Picked = alreadyPicked.Where(x => roleData.NeutralEvilSettings.GetValueSafe(x) == 10).Count();
                                if (neutralEvils100 > neutralEvilsMax) neutralEvils100 = neutralEvilsMax;

                                int neutralBenigns100Picked = alreadyPicked.Where(x => roleData.NeutralBenignSettings.GetValueSafe(x) == 10).Count();
                                if (neutralBenigns100 > neutralBenignsMax) neutralBenigns100 = neutralBenignsMax;

                                int neutralsK100Picked = alreadyPicked.Where(x => roleData.NeutralKillingSettings.GetValueSafe(x) == 10).Count();
                                if (neutralsK100 > neutralsKMax) neutralsK100 = neutralsKMax;

                                int crew100 = roleData.CrewSettings.Where(x => x.Value == 10).Count();
                                int crew100Picked = alreadyPicked.Where(x => roleData.CrewSettings.GetValueSafe(x) == 10).Count();
                                
                                if (neutralBenigns100 > neutralBenignsMax) neutralBenigns100 = neutralBenignsMax;
                                if (neutralEvils100 > neutralBenignsMax) neutralEvils100 = neutralEvilsMax;
                                if (neutralsK100 > neutralsKMax) neutralsK100 = neutralsKMax;

                                if (crew100 > maxCrew) crew100 = maxCrew;
                                if ((neutralEvils100 - neutralEvils100Picked >= crewmatesLeft || roleInfo.Alignment == RoleAlignment.NeutralEvil && neutralEvils100 - neutralEvils100Picked >= neutralEvilsMax - neutralEvilsPicked) && !(neutralEvils100Picked >= neutralEvilsMax) && !(roleData.NeutralEvilSettings.Any(x => x.Value == 10 && x.Key == (byte)roleInfo.RoleId))) continue;
                                if ((neutralsK100 - neutralsK100Picked >= crewmatesLeft || roleInfo.Alignment == RoleAlignment.NeutralKilling && neutralsK100 - neutralsK100Picked >= neutralsKMax - neutralsK100Picked) && !(neutralsK100Picked >= neutralsKMax) && !(roleData.NeutralKillingSettings.Any(x => x.Value == 10 && x.Key == (byte)roleInfo.RoleId))) continue;
                                if ((neutralBenigns100 - neutralBenigns100Picked >= crewmatesLeft || roleInfo.Alignment == RoleAlignment.NeutralBenign && neutralsK100 - neutralBenigns100Picked >= neutralBenignsMax - neutralBenignsPicked) && !(neutralBenigns100Picked >= neutralBenignsMax) && !(roleData.NeutralBenignSettings.Any(x => x.Value == 10 && x.Key == (byte)roleInfo.RoleId))) continue;

                                if (!(allowAnyNeutralBenign && roleInfo.Alignment == RoleAlignment.NeutralBenign) && crew100 - crew100Picked >= crewmatesLeft && !(roleData.CrewSettings.Where(x => x.Value == 10 && x.Key == (byte)roleInfo.RoleId).Count() > 0)) continue;
                                if (!(allowAnyNeutralEvil && roleInfo.Alignment == RoleAlignment.NeutralEvil) && crew100 - crew100Picked >= crewmatesLeft && !(roleData.CrewSettings.Where(x => x.Value == 10 && x.Key == (byte)roleInfo.RoleId).Count() > 0)) continue;
                                if (!(allowAnyNeutralK && roleInfo.Alignment == RoleAlignment.NeutralKilling) && crew100 - crew100Picked >= crewmatesLeft && !(roleData.CrewSettings.Where(x => x.Value == 10 && x.Key == (byte)roleInfo.RoleId).Count() > 0)) continue;

                                if (!(allowAnyNeutralEvil && roleInfo.Alignment == RoleAlignment.NeutralEvil) && neutralEvils100 + crew100 - neutralEvils100Picked - crew100Picked >= crewmatesLeft && !(roleData.CrewSettings.Where(x => x.Value == 10 && x.Key == (byte)roleInfo.RoleId).Count() > 0 || roleData.NeutralEvilSettings.Where(x => x.Value == 10 && x.Key == (byte)roleInfo.RoleId).Count() > 0)) continue;
                                if (!(allowAnyNeutralBenign && roleInfo.Alignment == RoleAlignment.NeutralBenign) && neutralBenigns100 + crew100 - neutralBenigns100Picked - crew100Picked >= crewmatesLeft && !(roleData.CrewSettings.Where(x => x.Value == 10 && x.Key == (byte)roleInfo.RoleId).Count() > 0 || roleData.NeutralBenignSettings.Where(x => x.Value == 10 && x.Key == (byte)roleInfo.RoleId).Count() > 0)) continue;
                                if (!(allowAnyNeutralK && roleInfo.Alignment == RoleAlignment.NeutralKilling) && neutralsK100 + crew100 - neutralsK100Picked - crew100Picked >= crewmatesLeft && !(roleData.CrewSettings.Where(x => x.Value == 10 && x.Key == (byte)roleInfo.RoleId).Count() > 0 || roleData.NeutralKillingSettings.Where(x => x.Value == 10 && x.Key == (byte)roleInfo.RoleId).Count() > 0)) continue;
                            }
                            // Handle role pairings that are blocked, e.g. Viper Warlock, Janitor Vulture etc.
                            bool blocked = false;
                            foreach (var blockedRoleId in CustomOptionHolder.blockedRolePairings) 
                            {
                                if (alreadyPicked.Contains(blockedRoleId.Key) && blockedRoleId.Value.ToList().Contains((byte)roleInfo.RoleId)) {
                                    blocked = true;
                                    break;
                                }
                            }
                            if (blocked) continue;


                            availableRoles.Add(roleInfo);
                        }

                        // Fallback for if all roles are somehow removed. (This is only the case if there is a bug, hence print a warning
                        if (availableRoles.Count == 0) 
                        {
                            if (PlayerControl.LocalPlayer.Data.Role.IsImpostor)
                                availableRoles.Add(RoleInfo.impostor);
                            else
                                availableRoles.Add(RoleInfo.crewmate);
                            TheSushiRolesPlugin.Logger.LogWarning("Draft Mode: Fallback triggered, because no roles were left. Forced addition of basegame Imp/Crewmate");
                        }

                        List<RoleInfo> originalAvailable = new(availableRoles);

                        // remove some roles, so that you can't always get the same roles:
                        if (availableRoles.Count > CustomOptionHolder.draftModeAmountOfChoices.GetFloat()) 
                        {
                            int countToRemove = availableRoles.Count - (int)CustomOptionHolder.draftModeAmountOfChoices.GetFloat();
                            while (countToRemove-- > 0) {
                                var toRemove = availableRoles.OrderBy(_ => Guid.NewGuid()).First();
                                availableRoles.Remove(toRemove);
                            }
                        }

                        if (timer >= maxTimer) 
                        {
                            SendPick((byte)originalAvailable.OrderBy(_ => Guid.NewGuid()).First().RoleId);
                        }


                        if (GameObject.Find("RoleButton") == null) 
                        {
                            SoundEffectsManager.Play("timemasterShield");
                            int i = 0;
                            int buttonsPerRow = 4;
                            int lastRow = availableRoles.Count / buttonsPerRow;
                            int buttonsInLastRow = availableRoles.Count % buttonsPerRow;

                            foreach (RoleInfo roleInfo in availableRoles) 
                            {
                                float row = i / buttonsPerRow;
                                float col = i % buttonsPerRow;
                                if (buttonsInLastRow != 0 && row == lastRow) 
                                {
                                    col += (buttonsPerRow - buttonsInLastRow) / 2f;
                                }
                                // planned rows: maximum of 4, hence the following calculation for rows as well:
                                row += (4 - lastRow - 1) / 2f;

                                ActionButton actionButton = UnityEngine.Object.Instantiate(HudManager.Instance.KillButton, __instance.TeamTitle.transform);
                                actionButton.gameObject.SetActive(true);
                                actionButton.gameObject.name = "RoleButton";
                                actionButton.transform.localPosition = new Vector3(-8.4f + col * 5.5f, -10 - row * 3f);
                                actionButton.transform.localScale = new Vector3(2f, 2f);
                                actionButton.SetCoolDown(0, 0);
                                GameObject textHolder = new GameObject("textHolder");
                                var text = textHolder.AddComponent<TMPro.TextMeshPro>();
                                text.text = roleInfo.Name.Replace(" ", "\n");
                                text.horizontalAlignment = TMPro.HorizontalAlignmentOptions.Center;
                                text.fontSize = 5;
                                textHolder.layer = actionButton.gameObject.layer;
                                text.outlineWidth = 0.1f;
                                text.outlineColor = Color.white;
                                text.color = roleInfo.Color;
                                textHolder.transform.SetParent(actionButton.transform, false);
                                textHolder.transform.localPosition = new Vector3(0, text.text.Contains("\n") ? -1.975f : -2.2f, -1);
                                GameObject actionButtonGameObject = actionButton.gameObject;
                                SpriteRenderer actionButtonRenderer = actionButton.graphic;
                                Material actionButtonMat = actionButtonRenderer.material;

                                PassiveButton button = actionButton.GetComponent<PassiveButton>();
                                button.OnClick = new Button.ButtonClickedEvent();
                                button.OnClick.AddListener((Action)(() => {
                                    SendPick((byte)roleInfo.RoleId);
                                }));
                                HudManager.Instance.StartCoroutine(Effects.Lerp(0.5f, new Action<float>((p) => {
                                    actionButton.OverrideText("");
                                })));
                                buttons.Add(actionButton);
                                i++;
                            }
                        }

                    }
                    else
                    {
                        int currentPick = PlayerControl.AllPlayerControls.Count - pickOrder.Count + 1;
                        playerText = $"Anonymous Player {currentPick}";
                        HudManager.Instance.FullScreen.color = Color.black;
                    }
                    __instance.TeamTitle.text = $"{Utils.ColorString(Color.white, "<size=280%>Role Draft! Get Ready!/size>")}\n\n\n<size=200%> Currently Picking:</size>\n\n\n<size=250%>{playerText}</size>";
                    int waitMore = pickOrder.IndexOf(PlayerControl.LocalPlayer.PlayerId);
                    string waitMoreText = "";
                    if (waitMore > 0) {
                        waitMoreText = $" ({waitMore} rounds until your turn)";
                    }
                    __instance.TeamTitle.text += $"\n\n{waitMoreText}\nRandom Selection In... {(int)(maxTimer + 1 - timer)}\n {(SoundManager.MusicVolume > -80 ? "♫ Music: Ultimate Superhero 3 - Kenët & Rez ♫" : "")}";
                    yield return null;
                }
            }
            HudManager.Instance.FullScreen.color = Color.black;
            __instance.FrontMost.gameObject.SetActive(true);
            GameObject.Find("BackgroundLayer")?.SetActive(true);
            if (AmongUsClient.Instance.AmHost)
            {
                RoleManagerSelectRolesPatch.AssignRoleTargets(null); // Assign targets for Lawyer & Prosecutor
                RoleManagerSelectRolesPatch.AssignGuesser();
                RoleManagerSelectRolesPatch.AssignModifiers(); // Assign modifier
            }

            float myTimer = 0f;
            while (myTimer < 3f)
            {
                myTimer += Time.deltaTime;
                Color c = new Color(0, 0, 0, myTimer / 3.0f);
                __instance.FrontMost.color = c;
                yield return null;
            }

            IsRunning = false;
            yield break;
        }

        public static void ReceivePick(byte playerId, byte RoleId)
        {
            if (!isEnabled) return;
            RPCProcedure.SetRole(RoleId, playerId);
            alreadyPicked.Add(RoleId);
            try
            {
                pickOrder.Remove(playerId);
                timer = 0;
                picked = true;                
                RoleInfo roleInfo = RoleInfo.allRoleInfos.First(x => (byte)x.RoleId == RoleId);
                string roleString = Utils.ColorString(roleInfo.Color, roleInfo.Name);
                int roleLength = roleInfo.Name.Length;  // Not used for now, but stores the amount of charactes of the roleString.
                if (!CustomOptionHolder.draftModeShowRoles.GetBool() && !(playerId == PlayerControl.LocalPlayer.PlayerId)) 
                {
                    roleString = "Unknown Role";
                    roleLength = roleString.Length;
                }                   
                else if (CustomOptionHolder.draftModeHideImpRoles.GetBool() && roleInfo.isImpostor && !(playerId == PlayerControl.LocalPlayer.PlayerId)) 
                {
                    roleString = Utils.ColorString(Palette.ImpostorRed, "Impostor Role");
                    roleLength = "Impostor Role".Length;
                }                    
                else if (CustomOptionHolder.draftModeHideNeutralRoles.GetBool() && roleInfo.Alignment == RoleAlignment.NeutralEvil && !(playerId == PlayerControl.LocalPlayer.PlayerId)) 
                {
                    roleString = Utils.ColorString(Palette.Blue, "Neutral Evil Role");
                    roleLength = "Neutral Role".Length;
                }
                else if (CustomOptionHolder.draftModeHideNeutralRoles.GetBool() && roleInfo.Alignment == RoleAlignment.NeutralBenign && !(playerId == PlayerControl.LocalPlayer.PlayerId)) 
                {
                    roleString = Utils.ColorString(Palette.Blue, "Neutral Benign Role");
                    roleLength = "Neutral Role".Length;
                }
                else if (CustomOptionHolder.draftModeHideNeutralKRoles.GetBool() && roleInfo.Alignment == RoleAlignment.NeutralKilling && !(playerId == PlayerControl.LocalPlayer.PlayerId))
                {
                    roleString = Utils.ColorString(Palette.Blue, "Neutral Killer Role");
                    roleLength = "Neutral Role".Length;
                }
                string line = $"{(playerId == PlayerControl.LocalPlayer.PlayerId ? "You" : alreadyPicked.Count)}:";
                line = line + string.Concat(Enumerable.Repeat(" ", 6 - line.Length)) + roleString;
                feedText.text += line + "\n";
                SoundEffectsManager.Play("select");
            }
            catch (Exception e) { TheSushiRolesPlugin.Logger.LogError(e); }
        }

        public static void SendPick(byte RoleId)
        {
            SoundEffectsManager.Stop("chronosShield");
            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.DraftModePick, SendOption.Reliable, -1);
            writer.Write(PlayerControl.LocalPlayer.PlayerId);
            writer.Write(RoleId);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            ReceivePick(PlayerControl.LocalPlayer.PlayerId, RoleId);

            // destroy all the buttons:
            foreach (var button in buttons)
            {
                button?.gameObject?.Destroy();
            }
            buttons.Clear();
        }


        public static void SendPickOrder()
        {
            pickOrder = PlayerControl.AllPlayerControls.ToArray().Select(x => x.PlayerId).OrderBy(_ => Guid.NewGuid()).ToList().ToList();
            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.DraftModePickOrder, SendOption.Reliable, -1);
            writer.Write((byte)pickOrder.Count);
            foreach (var item in pickOrder)
            {
                writer.Write(item);
            }
            AmongUsClient.Instance.FinishRpcImmediately(writer);
        }


        public static void ReceivePickOrder(int amount, MessageReader reader)
        {
            pickOrder.Clear();
            for (int i = 0; i < amount; i++)
            {
                pickOrder.Add(reader.ReadByte());
            }
        }

        class PatchedEnumerator() : IEnumerable
        {
            public IEnumerator enumerator;
            public IEnumerator Postfix;
            public IEnumerator GetEnumerator()
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current;
                }
                while (Postfix.MoveNext())
                    yield return Postfix.Current;
            }
        }


        [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.ShowTeam))]
        class ShowRolePatch
        {
            [HarmonyPostfix]
            public static void Postfix(IntroCutscene __instance, ref Il2CppSystem.Collections.IEnumerator __result)
            {
                if (!isEnabled) return;
                var newEnumerator = new PatchedEnumerator()
                {
                    enumerator = __result.WrapToManaged(),
                    Postfix = CoSelectRoles(__instance)
                };
                __result = newEnumerator.GetEnumerator().WrapToIl2Cpp();
            }
        }
    }
}