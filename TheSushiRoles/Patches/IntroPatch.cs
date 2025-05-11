using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Hazel;
using TheSushiRoles.Modules.BetterMaps;
using static TheSushiRoles.SubmergedCompatibility;

namespace TheSushiRoles.Patches 
{
    [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.OnDestroy))]
    class IntroCutsceneOnDestroyPatch
    {
        public static PoolablePlayer playerPrefab;
        public static Vector3 bottomLeft;
        public static void Prefix(IntroCutscene __instance) 
        {
            // Generate and initialize player icons
            int playerCounter = 0;
            if (PlayerControl.LocalPlayer != null && FastDestroyableSingleton<HudManager>.Instance != null) 
            {
                float aspect = Camera.main.aspect;
                float safeOrthographicSize = CameraSafeArea.GetSafeOrthographicSize(Camera.main);
                float xpos = 1.75f - safeOrthographicSize * aspect * 1.70f;
                float ypos = 0.15f - safeOrthographicSize * 1.7f;
                bottomLeft = new Vector3(xpos / 2, ypos/2, -61f);

                foreach (PlayerControl p in PlayerControl.AllPlayerControls) 
                {
                    NetworkedPlayerInfo data = p.Data;
                    PoolablePlayer player = UnityEngine.Object.Instantiate<PoolablePlayer>(__instance.PlayerPrefab, FastDestroyableSingleton<HudManager>.Instance.transform);
                    playerPrefab = __instance.PlayerPrefab;
                    p.SetPlayerMaterialColors(player.cosmetics.currentBodySprite.BodySprite);
                    player.SetSkin(data.DefaultOutfit.SkinId, data.DefaultOutfit.ColorId);
                    player.cosmetics.SetHat(data.DefaultOutfit.HatId, data.DefaultOutfit.ColorId);
                   // PlayerControl.SetPetImage(data.DefaultOutfit.PetId, data.DefaultOutfit.ColorId, player.PetSlot);
                    player.cosmetics.nameText.text = data.PlayerName;
                    player.SetFlipX(true);
                    MapOptions.BeanIcons[p.PlayerId] = player;
                    player.gameObject.SetActive(false);

                    if (PlayerControl.LocalPlayer == Arsonist.Player && p != Arsonist.Player) 
                    {
                        player.transform.localPosition = bottomLeft + new Vector3(-0.25f, -0.25f, 0) + Vector3.right * playerCounter++ * 0.35f;
                        player.transform.localScale = Vector3.one * 0.2f;
                        player.SetSemiTransparent(true);
                        player.gameObject.SetActive(true);
                    }
                    else 
                    {   //  This can be done for all players not just for the bounty hunter as it was before
                        player.transform.localPosition = bottomLeft;
                        player.transform.localScale = Vector3.one * 0.4f;
                        player.gameObject.SetActive(false);
                    }
                }
            }

            // Force Bounty Hunter to load a new Bounty when the Intro is over
            if (BountyHunter.bounty != null && PlayerControl.LocalPlayer == BountyHunter.Player) 
            {
                BountyHunter.bountyUpdateTimer = 0f;
                if (FastDestroyableSingleton<HudManager>.Instance != null) 
                {
                    BountyHunter.CooldownText = UnityEngine.Object.Instantiate<TMPro.TextMeshPro>(FastDestroyableSingleton<HudManager>.Instance.KillButton.cooldownTimerText, FastDestroyableSingleton<HudManager>.Instance.transform);
                    BountyHunter.CooldownText.alignment = TMPro.TextAlignmentOptions.Center;
                    BountyHunter.CooldownText.transform.localPosition = bottomLeft + new Vector3(0f, -0.35f, -62f);
                    BountyHunter.CooldownText.transform.localScale = Vector3.one * 0.4f;
                    BountyHunter.CooldownText.gameObject.SetActive(true);
                }
            }

            // First kill
            if (AmongUsClient.Instance.AmHost && MapOptions.ShieldFirstKill && MapOptions.FirstKillName != "")
            {
                PlayerControl target = PlayerControl.AllPlayerControls.ToArray().ToList().FirstOrDefault(x => x.Data.PlayerName.Equals(MapOptions.FirstKillName));
                if (target != null) 
                {
                    Utils.StartRPC(CustomRPC.SetFirstKill, target.PlayerId);
                    RPCProcedure.SetFirstKill(target.PlayerId);
                }
            }

            if (Utils.IsPolus() && MapOptions.BPVentImprovements) 
            {
                var list = GameObject.FindObjectsOfType<Vent>().ToList();
                var adminVent = list.FirstOrDefault(x => x.gameObject.name == "AdminVent");
                var bathroomVent = list.FirstOrDefault(x => x.gameObject.name == "BathroomVent");
                BetterPolus.SpecimenVent = UnityEngine.Object.Instantiate<Vent>(adminVent);
                BetterPolus.SpecimenVent.gameObject.AddSubmergedComponent(Classes.ElevatorMover);
                BetterPolus.SpecimenVent.transform.position = new Vector3(36.55068f, -21.5168f, -0.0215168f);
                BetterPolus.SpecimenVent.Left = adminVent;
                BetterPolus.SpecimenVent.Right = bathroomVent;
                BetterPolus.SpecimenVent.Center = null;
                BetterPolus.SpecimenVent.Id = ShipStatus.Instance.AllVents.Select(x => x.Id).Max() + 1;
                var allVentsList = ShipStatus.Instance.AllVents.ToList();
                allVentsList.Add(BetterPolus.SpecimenVent);
                ShipStatus.Instance.AllVents = allVentsList.ToArray();
                BetterPolus.SpecimenVent.gameObject.SetActive(true);
                BetterPolus.SpecimenVent.name = "newVent_" + BetterPolus.SpecimenVent.Id;

                adminVent.Center = BetterPolus.SpecimenVent;
                bathroomVent.Center = BetterPolus.SpecimenVent;
            }
            MapOptions.FirstKillName = "";
        }
    }

    [HarmonyPatch]
    class IntroPatch 
    {
        public static void SetupIntroTeamIcons(IntroCutscene __instance, ref  Il2CppSystem.Collections.Generic.List<PlayerControl> yourTeam) 
        {
            // Intro solo teams
            if (PlayerControl.LocalPlayer.IsNeutral() || PlayerControl.LocalPlayer.IsNeutralKiller()) 
            {
                var soloTeam = new Il2CppSystem.Collections.Generic.List<PlayerControl>();
                soloTeam.Add(PlayerControl.LocalPlayer);
                yourTeam = soloTeam;
            }

            // Add the Spy to the Impostor team (for the Impostors)
            if (Spy.Player != null && PlayerControl.LocalPlayer.Data.Role.IsImpostor) 
            {
                List<PlayerControl> players = PlayerControl.AllPlayerControls.ToArray().ToList().OrderBy(x => Guid.NewGuid()).ToList();
                var fakeImpostorTeam = new Il2CppSystem.Collections.Generic.List<PlayerControl>(); // The local player always has to be the first one in the list (to be displayed in the center)
                fakeImpostorTeam.Add(PlayerControl.LocalPlayer);
                foreach (PlayerControl p in players) 
                {
                    if (PlayerControl.LocalPlayer != p && (p == Spy.Player || p.Data.Role.IsImpostor))
                        fakeImpostorTeam.Add(p);
                }
                yourTeam = fakeImpostorTeam;
            }
        }

        public static void SetupIntroTeam(IntroCutscene __instance, ref  Il2CppSystem.Collections.Generic.List<PlayerControl> yourTeam) 
        {
            List<RoleInfo> infos = RoleInfo.GetRoleInfoForPlayer(PlayerControl.LocalPlayer);
            RoleInfo roleInfo = infos.Where(info => info.FactionId != Factions.Other).FirstOrDefault();
            var neutralColor = new Color32(76, 84, 78, 255);
            if (roleInfo == null) return;
            /*if (roleInfo == null || roleInfo == RoleInfo.crewmate) 
            {
                if (Modules.RoleDraft.isEnabled && CustomOptionHolder.neutralRolesCountMax.GetSelection() > 0) 
                {
                    __instance.TeamTitle.text = "<size=60%>Crewmate" + Utils.ColorString(Color.white, " / ") + Utils.ColorString(neutralColor, "Neutral") + "</size>";
                }
                return;
            }*/
            if (roleInfo.FactionId == Factions.Neutral) 
            {
                __instance.BackgroundBar.material.color = neutralColor;
                __instance.TeamTitle.text = "Neutral";
                __instance.TeamTitle.color = neutralColor;
            }
        }

        public static IEnumerator<WaitForSeconds> EndShowRole(IntroCutscene __instance) 
        {
            yield return new WaitForSeconds(5f);
            __instance.YouAreText.gameObject.SetActive(false);
            __instance.RoleText.gameObject.SetActive(false);
            __instance.RoleBlurbText.gameObject.SetActive(false);
            __instance.ourCrewmate.gameObject.SetActive(false);
           
        }

        [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.CreatePlayer))]
        class CreatePlayerPatch 
        {
            public static void Postfix(IntroCutscene __instance, bool impostorPositioning, ref PoolablePlayer __result) 
            {
                if (impostorPositioning) __result.SetNameColor(Palette.ImpostorRed);
            }
        }


        [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.ShowRole))]
        class SetUpRoleTextPatch 
        {
            static int seed = 0;
            static public void SetRoleTexts(IntroCutscene __instance) 
            {
                // Don't override the intro of the vanilla roles
                List<RoleInfo> infos = RoleInfo.GetRoleInfoForPlayer(PlayerControl.LocalPlayer);
                RoleInfo roleInfo = infos.Where(info => info.FactionId != Factions.Other).FirstOrDefault();

                List<ModifierInfo> infos2 = ModifierInfo.GetModifierInfoForPlayer(PlayerControl.LocalPlayer);

                __instance.RoleBlurbText.text = "";
                if (roleInfo != null) 
                {
                    __instance.RoleText.text = roleInfo.Name;
                    __instance.YouAreText.color = roleInfo.Color;
                    __instance.RoleText.color = roleInfo.Color;
                    __instance.RoleBlurbText.text = roleInfo.IntroDescription;
                    __instance.RoleBlurbText.color = roleInfo.Color;
                }
                if (infos2 != null) 
                {
                    foreach (var modifierInfo in infos2)
                    {
                        if (modifierInfo.ModifierId != ModifierId.Lover)
                        {
                            __instance.RoleBlurbText.text += Utils.ColorString(modifierInfo.Color, $"\n{modifierInfo.IntroDescription}");
                        }
                        else
                        {
                            PlayerControl otherLover = PlayerControl.LocalPlayer == Lovers.Lover1 ? Lovers.Lover2 : Lovers.Lover1;
                            __instance.RoleBlurbText.text += Utils.ColorString(Lovers.Color, $"\n♥ You are in love with {otherLover?.Data?.PlayerName ?? ""} ♥");
                        }
                    }
                }
            }
            public static bool Prefix(IntroCutscene __instance) 
            {
                seed = rnd.Next(5000);
                FastDestroyableSingleton<HudManager>.Instance.StartCoroutine(Effects.Lerp(1f, new Action<float>((p) => 
                {
                    SetRoleTexts(__instance);
                })));
                return true;
            }
        }

        [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.BeginCrewmate))]
        class BeginCrewmatePatch 
        {
            public static void Prefix(IntroCutscene __instance, ref  Il2CppSystem.Collections.Generic.List<PlayerControl> teamToDisplay) 
            {
                SetupIntroTeamIcons(__instance, ref teamToDisplay);
            }

            public static void Postfix(IntroCutscene __instance, ref  Il2CppSystem.Collections.Generic.List<PlayerControl> teamToDisplay) 
            {
                SetupIntroTeam(__instance, ref teamToDisplay);
            }
        }

        [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.BeginImpostor))]
        class BeginImpostorPatch 
        {
            public static void Prefix(IntroCutscene __instance, ref  Il2CppSystem.Collections.Generic.List<PlayerControl> yourTeam) 
            {
                SetupIntroTeamIcons(__instance, ref yourTeam);
            }

            public static void Postfix(IntroCutscene __instance, ref  Il2CppSystem.Collections.Generic.List<PlayerControl> yourTeam) 
            {
                SetupIntroTeam(__instance, ref yourTeam);
            }
        }
    }
}

