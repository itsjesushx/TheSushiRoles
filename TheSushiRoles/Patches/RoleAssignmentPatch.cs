﻿using Hazel;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

namespace TheSushiRoles.Patches 
{
    [HarmonyPatch(typeof(RoleOptionsCollectionV08), nameof(RoleOptionsCollectionV08.GetNumPerGame))]
    class RoleOptionsDataGetNumPerGamePatch
    {
        public static void Postfix(ref int __result) 
        {
            if (GameOptionsManager.Instance.CurrentGameOptions.GameMode == GameModes.Normal) __result = 0; // Deactivate Vanilla Roles if the mod roles are active
        }
    }

    [HarmonyPatch(typeof(IGameOptionsExtensions), nameof(IGameOptionsExtensions.GetAdjustedNumImpostors))]
    class GameOptionsDataGetAdjustedNumImpostorsPatch 
    {
        public static void Postfix(ref int __result) 
        {
            if (GameOptionsManager.Instance.CurrentGameOptions.GameMode == GameModes.Normal) 
            {  // Ignore Vanilla impostor limits in TSR Games.
                __result = Mathf.Clamp(GameOptionsManager.Instance.CurrentGameOptions.NumImpostors, 1, 3);
            } 
        }
    }

    /*[HarmonyPatch(typeof(NormalGameOptionsV07), nameof(NormalGameOptionsV07.Validate))]
    class GameOptionsDataValidatePatch 
    {
        public static void Postfix(NormalGameOptionsV07 __instance) 
        {
            if (GameOptionsManager.Instance.CurrentGameOptions.GameMode != GameModes.Normal) return;
            __instance.NumImpostors = GameOptionsManager.Instance.CurrentGameOptions.NumImpostors;
        }
    }*/

    [HarmonyPatch(typeof(RoleManager), nameof(RoleManager.SelectRoles))]
    class RoleManagerSelectRolesPatch 
    {
        private static int crewValues;
        private static int impValues;
        private static List<Tuple<byte, byte>> playerRoleMap = new List<Tuple<byte, byte>>();
        public static void Postfix() 
        {
            Utils.StartRPC(CustomRPC.ResetVaribles);
            RPCProcedure.ResetVariables();
            if (GameOptionsManager.Instance.currentGameOptions.GameMode == GameModes.HideNSeek /*|| Modules.RoleDraft.isEnabled*/) return; // Don't assign Roles in Hide N Seek
            AssignRoles();
        }

        private static void AssignRoles() 
        {
            var data = getRoleAssignmentData();
            AssignEnsuredRoles(data); // Assign roles that should always be in the game next
            AssignChanceRoles(data); // Assign roles that may or may not be in the game last
            AssignRoleTargets(data); // Assign targets for Lawyer & Prosecutor
            AssignGuesser();
            AssignModifiers();
            AssignAbilties();
        }

        public static RoleAssignmentData getRoleAssignmentData() 
        {
            // Get the players that we want to assign the roles to. Crewmate and Neutral roles are assigned to natural crewmates. Impostor roles to impostors.
            List<PlayerControl> crewmates = PlayerControl.AllPlayerControls.ToArray().ToList().OrderBy(x => Guid.NewGuid()).ToList();
            crewmates.RemoveAll(x => x.Data.Role.IsImpostor);
            List<PlayerControl> impostors = PlayerControl.AllPlayerControls.ToArray().ToList().OrderBy(x => Guid.NewGuid()).ToList();
            impostors.RemoveAll(x => !x.Data.Role.IsImpostor);

            var crewmateMin = CustomOptionHolder.crewmateRolesCountMin.GetSelection();
            var crewmateMax = CustomOptionHolder.crewmateRolesCountMax.GetSelection();

            var neutralMin = CustomOptionHolder.neutralRolesCountMin.GetSelection();
            var neutralMax = CustomOptionHolder.neutralRolesCountMax.GetSelection();

            var neutralKMin = CustomOptionHolder.neutralKillingRolesCountMin.GetSelection();
            var neutralKMax = CustomOptionHolder.neutralKillingRolesCountMax.GetSelection();
            
            var impostorMin = CustomOptionHolder.impostorRolesCountMin.GetSelection();
            var impostorMax = CustomOptionHolder.impostorRolesCountMax.GetSelection();

            // Make sure min is less or equal to max
            if (crewmateMin > crewmateMax) crewmateMin = crewmateMax;
            if (neutralMin > neutralMax) neutralMin = neutralMax;
            if (neutralKMin > neutralKMax) neutralKMin = neutralKMax;
            if (impostorMin > impostorMax) impostorMin = impostorMax;
           
            // Get the maximum allowed count of each role type based on the minimum and maximum option
            int crewCountSettings = rnd.Next(crewmateMin, crewmateMax + 1);
            int neutralCountSettings = rnd.Next(neutralMin, neutralMax + 1);
            int neutralKCountSettings = rnd.Next(neutralKMin, neutralKMax + 1);
            int impCountSettings = rnd.Next(impostorMin, impostorMax + 1);

            // Potentially lower the actual maximum to the assignable players
            int maxCrewmateRoles = Mathf.Min(crewmates.Count, crewCountSettings);
            int maxNeutralRoles = Mathf.Min(crewmates.Count, neutralCountSettings);
            int maxNeutralKRoles = Mathf.Min(crewmates.Count, neutralKCountSettings);
            int maxImpostorRoles = Mathf.Min(impostors.Count, impCountSettings);

            // Fill in the lists with the roles that should be assigned to players. Note that the special roles (like Mafia or Lovers) are NOT included in these lists
            Dictionary<byte, int> impSettings = new Dictionary<byte, int>();
            Dictionary<byte, int> neutralSettings = new Dictionary<byte, int>();
            Dictionary<byte, int> neutralKSettings = new Dictionary<byte, int>();
            Dictionary<byte, int> crewSettings = new Dictionary<byte, int>();
            
            impSettings.Add((byte)RoleId.Morphling, CustomOptionHolder.morphlingSpawnRate.GetSelection());
            impSettings.Add((byte)RoleId.Camouflager, CustomOptionHolder.camouflagerSpawnRate.GetSelection());
            impSettings.Add((byte)RoleId.Grenadier, CustomOptionHolder.GrenadierSpawnRate.GetSelection());
            impSettings.Add((byte)RoleId.Poisoner, CustomOptionHolder.poisonerSpawnRate.GetSelection());
            impSettings.Add((byte)RoleId.Miner, CustomOptionHolder.MinerSpawnRate.GetSelection());
            impSettings.Add((byte)RoleId.Blackmailer, CustomOptionHolder.BlackmailerSpawnRate.GetSelection());
            impSettings.Add((byte)RoleId.Eraser, CustomOptionHolder.eraserSpawnRate.GetSelection());
            impSettings.Add((byte)RoleId.Trickster, CustomOptionHolder.tricksterSpawnRate.GetSelection());
            impSettings.Add((byte)RoleId.Warlock, CustomOptionHolder.warlockSpawnRate.GetSelection());
            impSettings.Add((byte)RoleId.BountyHunter, CustomOptionHolder.bountyHunterSpawnRate.GetSelection());
            impSettings.Add((byte)RoleId.Witch, CustomOptionHolder.witchSpawnRate.GetSelection());
            impSettings.Add((byte)RoleId.Ninja, CustomOptionHolder.ninjaSpawnRate.GetSelection());
            impSettings.Add((byte)RoleId.Wraith, CustomOptionHolder.WraithSpawnRate.GetSelection());
            impSettings.Add((byte)RoleId.Undertaker, CustomOptionHolder.UndertakerSpawnRate .GetSelection());
            impSettings.Add((byte)RoleId.Yoyo, CustomOptionHolder.yoyoSpawnRate.GetSelection());

            neutralKSettings.Add((byte)RoleId.Jackal, CustomOptionHolder.jackalSpawnRate.GetSelection());
            neutralKSettings.Add((byte)RoleId.Plaguebearer, CustomOptionHolder.PlaguebearerSpawnRate.GetSelection());
            neutralKSettings.Add((byte)RoleId.Glitch, CustomOptionHolder.GlitchSpawnRate.GetSelection());
            neutralKSettings.Add((byte)RoleId.Werewolf, CustomOptionHolder.WerewolfSpawnRate.GetSelection());
            neutralKSettings.Add((byte)RoleId.Juggernaut, CustomOptionHolder.JuggernautSpawnRate.GetSelection());
            neutralKSettings.Add((byte)RoleId.Predator, CustomOptionHolder.PredatorSpawnRate.GetSelection());
            if (CustomOptionHolder.HitmanSpawnsWithNoAgent.GetBool()) // Hitman spawns with no Agent
                neutralKSettings.Add((byte)RoleId.Hitman, CustomOptionHolder.AgentSpawnRate.GetSelection());
            else
                neutralKSettings.Add((byte)RoleId.Agent, CustomOptionHolder.AgentSpawnRate.GetSelection());

            neutralSettings.Add((byte)RoleId.Jester, CustomOptionHolder.jesterSpawnRate.GetSelection());
            neutralSettings.Add((byte)RoleId.Romantic, CustomOptionHolder.RomanticSpawnChance.GetSelection());
            neutralSettings.Add((byte)RoleId.Arsonist, CustomOptionHolder.arsonistSpawnRate.GetSelection());
            neutralSettings.Add((byte)RoleId.Vulture, CustomOptionHolder.vultureSpawnRate.GetSelection());
            neutralSettings.Add((byte)RoleId.Amnesiac, CustomOptionHolder.AmnesiacSpawnRate.GetSelection());
            neutralSettings.Add((byte)RoleId.Prosecutor, CustomOptionHolder.ProsecutorSpawnRate.GetSelection());
            neutralSettings.Add((byte)RoleId.Lawyer, CustomOptionHolder.lawyerSpawnRate.GetSelection());

            crewSettings.Add((byte)RoleId.Mayor, CustomOptionHolder.mayorSpawnRate.GetSelection());
            crewSettings.Add((byte)RoleId.Veteran, CustomOptionHolder.VeteranSpawnRate.GetSelection());
            crewSettings.Add((byte)RoleId.Sheriff, CustomOptionHolder.sheriffSpawnRate.GetSelection());
            crewSettings.Add((byte)RoleId.Portalmaker, CustomOptionHolder.portalmakerSpawnRate.GetSelection());
            crewSettings.Add((byte)RoleId.Engineer, CustomOptionHolder.engineerSpawnRate.GetSelection());
            crewSettings.Add((byte)RoleId.Lighter, CustomOptionHolder.lighterSpawnRate.GetSelection());
            crewSettings.Add((byte)RoleId.Detective, CustomOptionHolder.detectiveSpawnRate.GetSelection());
            if (!SubmergedCompatibility.IsSubmerged)
            {
                crewSettings.Add((byte)RoleId.TimeMaster, CustomOptionHolder.timeMasterSpawnRate.GetSelection());
            }
            crewSettings.Add((byte)RoleId.Medic, CustomOptionHolder.medicSpawnRate.GetSelection());
            crewSettings.Add((byte)RoleId.Oracle, CustomOptionHolder.OracleSpawnRate.GetSelection());
            crewSettings.Add((byte)RoleId.Swapper,CustomOptionHolder.swapperSpawnRate.GetSelection());
            crewSettings.Add((byte)RoleId.Mystic, CustomOptionHolder.MysticSpawnRate.GetSelection());
            crewSettings.Add((byte)RoleId.Hacker, CustomOptionHolder.hackerSpawnRate.GetSelection());
            crewSettings.Add((byte)RoleId.Tracker, CustomOptionHolder.trackerSpawnRate.GetSelection());
            crewSettings.Add((byte)RoleId.Crusader, CustomOptionHolder.CrusaderSpawnRate.GetSelection());
            crewSettings.Add((byte)RoleId.Medium, CustomOptionHolder.mediumSpawnRate.GetSelection());
            crewSettings.Add((byte)RoleId.Trapper, CustomOptionHolder.trapperSpawnRate.GetSelection());
            if (impostors.Count > 1) 
            {
                // Only add Spy if more than 1 impostor as the spy role is otherwise useless
                crewSettings.Add((byte)RoleId.Spy, CustomOptionHolder.spySpawnRate.GetSelection());
                // Only add Cleaner if more than 1 impostor as the Cleaner role is otherwise useless
                impSettings.Add((byte)RoleId.Cleaner, CustomOptionHolder.cleanerSpawnRate.GetSelection());
            }
            crewSettings.Add((byte)RoleId.Vigilante, CustomOptionHolder.VigilanteSpawnRate.GetSelection());

            return new RoleAssignmentData 
            {
                crewmates = crewmates,
                impostors = impostors,
                crewSettings = crewSettings,
                neutralSettings = neutralSettings,
                neutralKSettings = neutralKSettings,
                impSettings = impSettings,
                maxCrewmateRoles = maxCrewmateRoles,
                maxNeutralRoles = maxNeutralRoles,
                maxNeutralKRoles = maxNeutralKRoles,
                maxImpostorRoles = maxImpostorRoles
            };
        }

        private static void AssignEnsuredRoles(RoleAssignmentData data) 
        {
            // Get all roles where the chance to occur is set to 100%
            List<byte> ensuredCrewmateRoles = data.crewSettings.Where(x => x.Value == 10).Select(x => x.Key).ToList();
            List<byte> ensuredNeutralRoles = data.neutralSettings.Where(x => x.Value == 10).Select(x => x.Key).ToList();
            List<byte> ensuredNeutralKRoles = data.neutralKSettings.Where(x => x.Value == 10).Select(x => x.Key).ToList();
            List<byte> ensuredImpostorRoles = data.impSettings.Where(x => x.Value == 10).Select(x => x.Key).ToList();

            // Assign roles until we run out of either players we can assign roles to or run out of roles we can assign to players
            while (
                (data.impostors.Count > 0 && data.maxImpostorRoles > 0 && ensuredImpostorRoles.Count > 0) || 
                (data.crewmates.Count > 0 && (
                    (data.maxCrewmateRoles > 0 && ensuredCrewmateRoles.Count > 0) || 
                    (data.maxNeutralRoles > 0 && ensuredNeutralRoles.Count > 0)|| 
                    (data.maxNeutralKRoles > 0 && ensuredNeutralKRoles.Count > 0)
                ))) {
                    
                Dictionary<RoleType, List<byte>> rolesToAssign = new Dictionary<RoleType, List<byte>>();
                if (data.crewmates.Count > 0 && data.maxCrewmateRoles > 0 && ensuredCrewmateRoles.Count > 0) rolesToAssign.Add(RoleType.Crewmate, ensuredCrewmateRoles);
                if (data.crewmates.Count > 0 && data.maxNeutralRoles > 0 && ensuredNeutralRoles.Count > 0) rolesToAssign.Add(RoleType.Neutral, ensuredNeutralRoles);
                if (data.crewmates.Count > 0 && data.maxNeutralKRoles > 0 && ensuredNeutralKRoles.Count > 0) rolesToAssign.Add(RoleType.NK, ensuredNeutralKRoles);
                if (data.impostors.Count > 0 && data.maxImpostorRoles > 0 && ensuredImpostorRoles.Count > 0) rolesToAssign.Add(RoleType.Impostor, ensuredImpostorRoles);
                
                // Randomly select a pool of roles to assign a role from next (Crewmate role, Neutral role or Impostor role) 
                // then select one of the roles from the selected pool to a player 
                // and remove the role (and any potentially blocked role pairings) from the pool(s)
                var roleType = rolesToAssign.Keys.ElementAt(rnd.Next(0, rolesToAssign.Keys.Count())); 
                var players = roleType == RoleType.Crewmate || roleType == RoleType.Neutral || roleType == RoleType.NK ? data.crewmates : data.impostors;
                var index = rnd.Next(0, rolesToAssign[roleType].Count);
                var roleId = rolesToAssign[roleType][index];
                SetRoleToRandomPlayer(rolesToAssign[roleType][index], players);
                rolesToAssign[roleType].RemoveAt(index);

                if (CustomOptionHolder.blockedRolePairings.ContainsKey(roleId)) 
                {
                    foreach(var blockedRoleId in CustomOptionHolder.blockedRolePairings[roleId]) 
                    {
                        // Set chance for the blocked roles to 0 for chances less than 100%
                        if (data.impSettings.ContainsKey(blockedRoleId)) data.impSettings[blockedRoleId] = 0;
                        if (data.neutralSettings.ContainsKey(blockedRoleId)) data.neutralSettings[blockedRoleId] = 0;
                        if (data.neutralKSettings.ContainsKey(blockedRoleId)) data.neutralKSettings[blockedRoleId] = 0;
                        if (data.crewSettings.ContainsKey(blockedRoleId)) data.crewSettings[blockedRoleId] = 0;
                        // Remove blocked roles even if the chance was 100%
                        foreach(var ensuredRolesList in rolesToAssign.Values) 
                        {
                            ensuredRolesList.RemoveAll(x => x == blockedRoleId);
                        }
                    }
                }

                // Adjust the role limit
                switch (roleType) 
                {
                    case RoleType.Crewmate: data.maxCrewmateRoles--; crewValues -= 10; break;
                    case RoleType.Neutral: data.maxNeutralRoles--; break;
                    case RoleType.NK: data.maxNeutralKRoles--; break;
                    case RoleType.Impostor: data.maxImpostorRoles--; impValues -= 10;  break;
                }
            }
        }
        private static void AssignChanceRoles(RoleAssignmentData data) 
        {
            // Get all roles where the chance to occur is set grater than 0% but not 100% and build a ticket pool based on their weight
            List<byte> crewmateTickets = data.crewSettings.Where(x => x.Value > 0 && x.Value < 10).Select(x => Enumerable.Repeat(x.Key, x.Value)).SelectMany(x => x).ToList();
            List<byte> neutralTickets = data.neutralSettings.Where(x => x.Value > 0 && x.Value < 10).Select(x => Enumerable.Repeat(x.Key, x.Value)).SelectMany(x => x).ToList();
            List<byte> neutralKTickets = data.neutralKSettings.Where(x => x.Value > 0 && x.Value < 10).Select(x => Enumerable.Repeat(x.Key, x.Value)).SelectMany(x => x).ToList();
            List<byte> impostorTickets = data.impSettings.Where(x => x.Value > 0 && x.Value < 10).Select(x => Enumerable.Repeat(x.Key, x.Value)).SelectMany(x => x).ToList();

            // Assign roles until we run out of either players we can assign roles to or run out of roles we can assign to players
            while (
                (data.impostors.Count > 0 && data.maxImpostorRoles > 0 && impostorTickets.Count > 0) || 
                (data.crewmates.Count > 0 && 
                (
                    (data.maxCrewmateRoles > 0 && crewmateTickets.Count > 0) || 
                    (data.maxNeutralRoles > 0 && neutralTickets.Count > 0)|| 
                    (data.maxNeutralKRoles > 0 && neutralKTickets.Count > 0)
                ))) 
                {
                
                Dictionary<RoleType, List<byte>> rolesToAssign = new Dictionary<RoleType, List<byte>>();
                if (data.crewmates.Count > 0 && data.maxCrewmateRoles > 0 && crewmateTickets.Count > 0) rolesToAssign.Add(RoleType.Crewmate, crewmateTickets);
                if (data.crewmates.Count > 0 && data.maxNeutralRoles > 0 && neutralTickets.Count > 0) rolesToAssign.Add(RoleType.Neutral, neutralTickets);
                if (data.crewmates.Count > 0 && data.maxNeutralKRoles > 0 && neutralKTickets.Count > 0) rolesToAssign.Add(RoleType.NK, neutralKTickets);
                if (data.impostors.Count > 0 && data.maxImpostorRoles > 0 && impostorTickets.Count > 0) rolesToAssign.Add(RoleType.Impostor, impostorTickets);
                
                // Randomly select a pool of role tickets to assign a role from next (Crewmate role, Neutral role or Impostor role) 
                // then select one of the roles from the selected pool to a player 
                // and remove all tickets of this role (and any potentially blocked role pairings) from the pool(s)
                var roleType = rolesToAssign.Keys.ElementAt(rnd.Next(0, rolesToAssign.Keys.Count()));
                var players = roleType == RoleType.Crewmate || roleType == RoleType.Neutral || roleType == RoleType.NK ? data.crewmates : data.impostors;
                var index = rnd.Next(0, rolesToAssign[roleType].Count);
                var roleId = rolesToAssign[roleType][index];
                SetRoleToRandomPlayer(roleId, players);
                rolesToAssign[roleType].RemoveAll(x => x == roleId);

                if (CustomOptionHolder.blockedRolePairings.ContainsKey(roleId)) 
                {
                    foreach(var blockedRoleId in CustomOptionHolder.blockedRolePairings[roleId]) 
                    {
                        // Remove tickets of blocked roles from all pools
                        crewmateTickets.RemoveAll(x => x == blockedRoleId);
                        neutralTickets.RemoveAll(x => x == blockedRoleId);
                        neutralKTickets.RemoveAll(x => x == blockedRoleId);
                        impostorTickets.RemoveAll(x => x == blockedRoleId);
                    }
                }

                // Adjust the role limit
                switch (roleType) 
                {
                    case RoleType.Crewmate: data.maxCrewmateRoles--; break;
                    case RoleType.Neutral: data.maxNeutralRoles--;break;
                    case RoleType.NK: data.maxNeutralKRoles--;break;
                    case RoleType.Impostor: data.maxImpostorRoles--;break;
                }
            }
        }

        public static void AssignRoleTargets(RoleAssignmentData data) 
        {
            // Set Lawyer or Prosecutor Target
            if (Lawyer.Player != null) 
            {
                var possibleTargets = new List<PlayerControl>();
                foreach (PlayerControl p in PlayerControl.AllPlayerControls) 
                {
                    if (!p.Data.IsDead && !p.Data.Disconnected && p != Lovers.Lover1 && p != Lovers.Lover2 && (p.IsKiller() || (Lawyer.targetCanBeJester && p == Jester.Player)))
                        possibleTargets.Add(p);
                }
                
                if (possibleTargets.Count == 0) 
                {
                    Utils.StartRPC(CustomRPC.LawyerChangeRole);
                    RPCProcedure.LawyerChangeRole();
                } 
                else 
                {
                    var target = possibleTargets[TheSushiRoles.rnd.Next(0, possibleTargets.Count)];
                    Utils.StartRPC(CustomRPC.LawyerSetTarget, target.PlayerId);
                    RPCProcedure.LawyerSetTarget(target.PlayerId);
                }
            }
            else if (Prosecutor.Player != null)
            {
                var possibleTargets = new List<PlayerControl>();
                foreach (PlayerControl p in PlayerControl.AllPlayerControls) 
                {
                    if (!p.Data.IsDead && !p.Data.Disconnected && p != Lovers.Lover1 && p != Lovers.Lover2 && p != Mini.Player && p.IsCrew() && p != Swapper.Player)
                        possibleTargets.Add(p);
                }
                if (possibleTargets.Count == 0) 
                {
                    Utils.StartRPC(CustomRPC.ProsecutorChangeRole);
                    RPCProcedure.ProsecutorChangeRole();
                }
                else 
                {
                    var target = possibleTargets[TheSushiRoles.rnd.Next(0, possibleTargets.Count)];
                    Utils.StartRPC(CustomRPC.ProsecutorSetTarget, target.PlayerId);
                    RPCProcedure.ProsecutorSetTarget(target.PlayerId);
                }
            }
        }

        public static void AssignAbilties() 
        {
            var modifierMin = CustomOptionHolder.abilitiesCountMin.GetSelection();
            var modifierMax = CustomOptionHolder.abilitiesCountMax.GetSelection();
            if (modifierMin > modifierMax) modifierMin = modifierMax;
            int modifierCountSettings = rnd.Next(modifierMin, modifierMax + 1);
            List<PlayerControl> players = PlayerControl.AllPlayerControls.ToArray().ToList();
            if (!CustomOptionHolder.GuesserHaveModifier.GetBool())
                players.RemoveAll(x => Guesser.IsGuesser(x.PlayerId));
            int modifierCount = Mathf.Min(players.Count, modifierCountSettings);

            if (modifierCount == 0) return;

            List<AbilityId> allAbilities = new List<AbilityId>();
            List<AbilityId> ensuredAbilities = new List<AbilityId>();
            List<AbilityId> chanceAbilities = new List<AbilityId>();
            allAbilities.AddRange(new List<AbilityId> 
            {
                AbilityId.Disperser,
                AbilityId.Coward,
                AbilityId.Paranoid
            });

            foreach (AbilityId m in allAbilities) 
            {
                if (GetSelectionForAbilityId(m) == 10) ensuredAbilities.AddRange(Enumerable.Repeat(m, GetSelectionForAbilityId(m, true) / 10));
                else chanceAbilities.AddRange(Enumerable.Repeat(m, GetSelectionForAbilityId(m, true)));
            }

            AssignAbilitiesToPlayers(ensuredAbilities, players, modifierCount); // Assign ensured ability

            modifierCount -= ensuredAbilities.Count;
            if (modifierCount <= 0) return;
            int chanceModifierCount = Mathf.Min(modifierCount, chanceAbilities.Count);
            List<AbilityId> chanceAbilityToAssign = new List<AbilityId>();
            while (chanceModifierCount > 0 && chanceAbilities.Count > 0)
            {
                var index = rnd.Next(0, chanceAbilities.Count);
                AbilityId AbilityId = chanceAbilities[index];
                chanceAbilityToAssign.Add(AbilityId);

                int Abilitieselection = GetSelectionForAbilityId(AbilityId);
                while (Abilitieselection > 0) 
                {
                    chanceAbilities.Remove(AbilityId);
                    Abilitieselection--;
                }
                chanceModifierCount--;
            }

            AssignAbilitiesToPlayers(chanceAbilityToAssign, players, modifierCount); // Assign chance ability
        }

        public static void AssignModifiers() 
        {
            var modifierMin = CustomOptionHolder.modifiersCountMin.GetSelection();
            var modifierMax = CustomOptionHolder.modifiersCountMax.GetSelection();
            if (modifierMin > modifierMax) modifierMin = modifierMax;
            int modifierCountSettings = rnd.Next(modifierMin, modifierMax + 1);
            List<PlayerControl> players = PlayerControl.AllPlayerControls.ToArray().ToList();
            if (!CustomOptionHolder.GuesserHaveModifier.GetBool())
                players.RemoveAll(x => Guesser.IsGuesser(x.PlayerId));
            int modifierCount = Mathf.Min(players.Count, modifierCountSettings);

            if (modifierCount == 0) return;

            List<ModifierId> allModifiers = new List<ModifierId>();
            List<ModifierId> ensuredModifiers = new List<ModifierId>();
            List<ModifierId> chanceModifiers = new List<ModifierId>();
            allModifiers.AddRange(new List<ModifierId> 
            {
                ModifierId.Tiebreaker,
                ModifierId.Mini,
                ModifierId.Bait,
                ModifierId.Lazy,
                ModifierId.Sleuth,
                ModifierId.Sunglasses,
                ModifierId.Vip,
                ModifierId.Invert,
                ModifierId.Chameleon,
                ModifierId.Armored
            });

            if (rnd.Next(1, 101) <= CustomOptionHolder.modifierLover.GetSelection() * 10) 
            { 
                // Assign lover
                bool isEvilLover = rnd.Next(1, 101) <= CustomOptionHolder.modifierLoverImpLoverRate.GetSelection() * 10;
                byte firstLoverId;
                List<PlayerControl> evilPlayer = new List<PlayerControl>(players);
                List<PlayerControl> crewPlayer = new List<PlayerControl>(players);
                evilPlayer.RemoveAll(x => !x.IsKiller());
                crewPlayer.RemoveAll(x => !x.IsCrew());

                if (isEvilLover) firstLoverId = SetModifierToRandomPlayer((byte)ModifierId.Lover, evilPlayer);
                else firstLoverId = SetModifierToRandomPlayer((byte)ModifierId.Lover, crewPlayer);
                byte secondLoverId = SetModifierToRandomPlayer((byte)ModifierId.Lover, crewPlayer, 1);

                players.RemoveAll(x => x.PlayerId == firstLoverId || x.PlayerId == secondLoverId);
                modifierCount--;

                // Ensure players with Lover modifier cannot have another modifier
                allModifiers.RemoveAll(x => x == ModifierId.Lover);
                players.RemoveAll(x => x.PlayerId == firstLoverId || x.PlayerId == secondLoverId);
            }

            foreach (ModifierId m in allModifiers) 
            {
                if (GetSelectionForModifierId(m) == 10) ensuredModifiers.AddRange(Enumerable.Repeat(m, GetSelectionForModifierId(m, true) / 10));
                else chanceModifiers.AddRange(Enumerable.Repeat(m, GetSelectionForModifierId(m, true)));
            }

            AssignModifiersToPlayers(ensuredModifiers, players, modifierCount); // Assign ensured modifier

            modifierCount -= ensuredModifiers.Count;
            if (modifierCount <= 0) return;
            int chanceModifierCount = Mathf.Min(modifierCount, chanceModifiers.Count);
            List<ModifierId> chanceModifierToAssign = new List<ModifierId>();
            while (chanceModifierCount > 0 && chanceModifiers.Count > 0) 
            {
                var index = rnd.Next(0, chanceModifiers.Count);
                ModifierId modifierId = chanceModifiers[index];
                chanceModifierToAssign.Add(modifierId);

                int modifierSelection = GetSelectionForModifierId(modifierId);
                while (modifierSelection > 0)
                {
                    chanceModifiers.Remove(modifierId);
                    modifierSelection--;
                }
                chanceModifierCount--;
            }

            AssignModifiersToPlayers(chanceModifierToAssign, players, modifierCount); // Assign chance modifier
        }

        public static void AssignGuesser() 
        {
            List<PlayerControl> impPlayer = PlayerControl.AllPlayerControls.ToArray().ToList().OrderBy(x => Guid.NewGuid()).ToList();
            List<PlayerControl> neutralPlayer = PlayerControl.AllPlayerControls.ToArray().ToList().OrderBy(x => Guid.NewGuid()).ToList();
            List<PlayerControl> crewPlayer = PlayerControl.AllPlayerControls.ToArray().ToList().OrderBy(x => Guid.NewGuid()).ToList();
            impPlayer.RemoveAll(x => !x.Data.Role.IsImpostor);
            neutralPlayer.RemoveAll(x => !x.IsNeutralKiller());
            crewPlayer.RemoveAll(x => !x.IsCrew());
            AssignGuesserToPlayers(crewPlayer, Mathf.RoundToInt(CustomOptionHolder.GuesserCrewNumber.GetFloat()));
            AssignGuesserToPlayers(neutralPlayer, Mathf.RoundToInt(CustomOptionHolder.GuesserNeutralNumber.GetFloat()));
            AssignGuesserToPlayers(impPlayer, Mathf.RoundToInt(CustomOptionHolder.GuesserImpNumber.GetFloat()));
        }

        private static void AssignGuesserToPlayers(List<PlayerControl> playerList, int count) 
        {
            for (int i = 0; i < count && playerList.Count > 0; i++) 
            {
                var index = rnd.Next(0, playerList.Count);
                byte playerId = playerList[index].PlayerId;
                playerList.RemoveAt(index);

                Utils.StartRPC(CustomRPC.SetGuesser, playerId);
                RPCProcedure.SetGuesser(playerId);
            }
        }

        private static byte SetRoleToRandomPlayer(byte roleId, List<PlayerControl> playerList, bool removePlayer = true) 
        {
            var index = rnd.Next(0, playerList.Count);
            byte playerId = playerList[index].PlayerId;
            if (removePlayer) playerList.RemoveAt(index);

            playerRoleMap.Add(new Tuple<byte, byte>(playerId, roleId));

            Utils.StartRPC(CustomRPC.SetRole, roleId, playerId);
            RPCProcedure.SetRole(roleId, playerId);
            return playerId;
        }

        private static byte SetModifierToRandomPlayer(byte modifierId, List<PlayerControl> playerList, byte flag = 0) 
        {
            if (playerList.Count == 0) return Byte.MaxValue;
            var index = rnd.Next(0, playerList.Count);
            byte playerId = playerList[index].PlayerId;
            playerList.RemoveAt(index);

            Utils.StartRPC(CustomRPC.SetModifier, modifierId, playerId, flag);
            RPCProcedure.SetModifier(modifierId, playerId, flag);
            return playerId;
        }

        private static byte SetAbilityToRandomPlayer(byte abilityId, List<PlayerControl> playerList, byte flag = 0) 
        {
            if (playerList.Count == 0) return Byte.MaxValue;
            var index = rnd.Next(0, playerList.Count);
            byte playerId = playerList[index].PlayerId;
            playerList.RemoveAt(index);

            Utils.StartRPC(CustomRPC.SetAbility, abilityId, playerId, flag);
            RPCProcedure.SetAbility(abilityId, playerId, flag);
            return playerId;
        }
        private static void AssignAbilitiesToPlayers(List<AbilityId> abilities, List<PlayerControl> playerList, int abilityCount) 
        {
            abilities = abilities.OrderBy(x => rnd.Next()).ToList(); // randomize list

            while (abilityCount < abilities.Count) 
            {
                var index = rnd.Next(0, abilities.Count);
                abilities.RemoveAt(index);
            }

            byte playerId;

            List<PlayerControl> crewPlayer = new List<PlayerControl>(playerList);
            List<PlayerControl> impPlayer = new List<PlayerControl>(playerList);
            impPlayer.RemoveAll(x => !x.Data.Role.IsImpostor);
            crewPlayer.RemoveAll(x => !x.IsCrew());

            // Remove players with the Guesser ability from the list
            crewPlayer.RemoveAll(x => Guesser.IsGuesser(x.PlayerId));
            impPlayer.RemoveAll(x => Guesser.IsGuesser(x.PlayerId));
            playerList.RemoveAll(x => Guesser.IsGuesser(x.PlayerId));

            if (abilities.Contains(AbilityId.Coward)) 
            {
                var crewPlayerC = new List<PlayerControl>(crewPlayer);
                crewPlayerC.RemoveAll(x => x == Mayor.Player);
                playerId = SetAbilityToRandomPlayer((byte)AbilityId.Coward, crewPlayerC);
                crewPlayer.RemoveAll(x => x.PlayerId == playerId);
                playerList.RemoveAll(x => x.PlayerId == playerId);
                abilities.RemoveAll(x => x == AbilityId.Coward);
            }

            if (abilities.Contains(AbilityId.Disperser)) 
            {
                playerId = SetModifierToRandomPlayer((byte)AbilityId.Disperser, impPlayer);
                impPlayer.RemoveAll(x => x.PlayerId == playerId);
                playerList.RemoveAll(x => x.PlayerId == playerId);
                abilities.RemoveAll(x => x == AbilityId.Disperser);
            }

            foreach (AbilityId ability in abilities)
            {
                if (playerList.Count == 0) break;
                playerId = SetAbilityToRandomPlayer((byte)ability, playerList);
                playerList.RemoveAll(x => x.PlayerId == playerId);
            }
        }

        private static void AssignModifiersToPlayers(List<ModifierId> modifiers, List<PlayerControl> playerList, int modifierCount) 
        {
            modifiers = modifiers.OrderBy(x => rnd.Next()).ToList(); // randomize list

            while (modifierCount < modifiers.Count) 
            {
                var index = rnd.Next(0, modifiers.Count);
                modifiers.RemoveAt(index);
            }

            byte playerId;

            List<PlayerControl> crewPlayer = new List<PlayerControl>(playerList);
            List<PlayerControl> impPlayer = new List<PlayerControl>(playerList);
            impPlayer.RemoveAll(x => !x.Data.Role.IsImpostor);
            crewPlayer.RemoveAll(x => !x.IsCrew());
            if (modifiers.Contains(ModifierId.Sunglasses)) 
            {
                int sunglassesCount = 0;
                while (sunglassesCount < modifiers.FindAll(x => x == ModifierId.Sunglasses).Count) 
                {
                    playerId = SetModifierToRandomPlayer((byte)ModifierId.Sunglasses, crewPlayer);
                    crewPlayer.RemoveAll(x => x.PlayerId == playerId);
                    playerList.RemoveAll(x => x.PlayerId == playerId);
                    sunglassesCount++;
                }
                modifiers.RemoveAll(x => x == ModifierId.Sunglasses);
            }

            foreach (ModifierId modifier in modifiers) 
            {
                if (playerList.Count == 0) break;
                playerId = SetModifierToRandomPlayer((byte)modifier, playerList);
                playerList.RemoveAll(x => x.PlayerId == playerId);
            }
        }

        private static int GetSelectionForModifierId(ModifierId modifierId, bool multiplyQuantity = false) 
        {
            int selection = 0;
            switch (modifierId) 
            {
                case ModifierId.Lover:
                    selection = CustomOptionHolder.modifierLover.GetSelection(); break;
                case ModifierId.Tiebreaker:
                    selection = CustomOptionHolder.modifierTieBreaker.GetSelection(); break;
                case ModifierId.Mini:
                    selection = CustomOptionHolder.modifierMini.GetSelection(); break;
                case ModifierId.Bait:
                    selection = CustomOptionHolder.modifierBait.GetSelection();
                    if (multiplyQuantity) selection *= CustomOptionHolder.modifierBaitQuantity.GetQuantity();
                    break;
                case ModifierId.Lazy:
                    selection = CustomOptionHolder.modifierLazy.GetSelection();
                    if (multiplyQuantity) selection *= CustomOptionHolder.modifierLazyQuantity.GetQuantity();
                    break;
                case ModifierId.Sunglasses:
                    selection = CustomOptionHolder.modifierSunglasses.GetSelection();
                    if (multiplyQuantity) selection *= CustomOptionHolder.modifierSunglassesQuantity.GetQuantity();
                    break;
                case ModifierId.Vip:
                    selection = CustomOptionHolder.modifierVip.GetSelection();
                    if (multiplyQuantity) selection *= CustomOptionHolder.modifierVipQuantity.GetQuantity();
                    break;
                case ModifierId.Invert:
                    selection = CustomOptionHolder.modifierInvert.GetSelection();
                    if (multiplyQuantity) selection *= CustomOptionHolder.modifierInvertQuantity.GetQuantity();
                    break;
                case ModifierId.Chameleon:
                    selection = CustomOptionHolder.modifierChameleon.GetSelection();
                    if (multiplyQuantity) selection *= CustomOptionHolder.modifierChameleonQuantity.GetQuantity();
                    break;
                case ModifierId.Sleuth:
                    selection = CustomOptionHolder.ModifierSleuth.GetSelection();
                    if (multiplyQuantity) selection *= CustomOptionHolder.ModifierSleuthQuantity.GetQuantity();
                    break;
                case ModifierId.Armored:
                    selection = CustomOptionHolder.modifierArmored.GetSelection();
                    break;
            }
                 
            return selection;
        }
        private static int GetSelectionForAbilityId(AbilityId AbilityId, bool multiplyQuantity = false) 
        {
            int selection = 0;
            switch (AbilityId) 
            {
                case AbilityId.Coward:
                    selection = CustomOptionHolder.AbilityCoward.GetSelection();
                    break;
                case AbilityId.Disperser:
                    selection = CustomOptionHolder.AbilityDisperser.GetSelection();
                    break;
                case AbilityId.Paranoid:
                    selection = CustomOptionHolder.AbilityParanoid.GetSelection();
                    break;
            }
                 
            return selection;
        }

        private static void SetRolesAgain()
        {

            while (playerRoleMap.Any())
            {
                byte amount = (byte)Math.Min(playerRoleMap.Count, 20);
                var writer = AmongUsClient.Instance!.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.WorkaroundSetRoles, SendOption.Reliable, -1);
                writer.Write(amount);
                for (int i = 0; i < amount; i++)
                {
                    var option = playerRoleMap[0];
                    playerRoleMap.RemoveAt(0);
                    writer.WritePacked((uint)option.Item1);
                    writer.WritePacked((uint)option.Item2);
                }
                AmongUsClient.Instance.FinishRpcImmediately(writer);
            }
        }

        public class RoleAssignmentData 
        {
            public List<PlayerControl> crewmates {get;set;}
            public List<PlayerControl> impostors {get;set;}
            public Dictionary<byte, int> impSettings = new Dictionary<byte, int>();
            public Dictionary<byte, int> neutralSettings = new Dictionary<byte, int>();
            public Dictionary<byte, int> neutralKSettings = new Dictionary<byte, int>();
            public Dictionary<byte, int> crewSettings = new Dictionary<byte, int>();
            public int maxCrewmateRoles {get;set;}
            public int maxNeutralRoles {get;set;}
            public int maxNeutralKRoles {get;set;}
            public int maxImpostorRoles {get;set;}
        }
        
        private enum RoleType 
        {
            Crewmate = 0,
            Neutral = 1,
            Impostor = 2,
            NK = 3
        }

    }
}
