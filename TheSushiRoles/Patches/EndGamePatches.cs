using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.Text;
using Reactor.Utilities;
using System.Collections;

namespace TheSushiRoles.Patches 
{
    static class AdditionalTempData 
    {
        public static WinCondition WinCondition = WinCondition.Default;
        public static List<WinCondition> additionalWinConditions = new List<WinCondition>();
        public static List<PlayerRoleInfo> playerRoles = new List<PlayerRoleInfo>();
        public static float timer = 0;
        public static void Clear() 
        {
            playerRoles.Clear();
            additionalWinConditions.Clear();
            WinCondition = WinCondition.Default;
            timer = 0;
        }

        internal class PlayerRoleInfo 
        {
            public string PlayerName { get; set; }
            public List<RoleInfo> Roles { get; set; }
            public List<ModifierInfo> Modifiers { get; set; }
            public List<AbilityInfo> Abilities { get; set; }
            public string RoleNames { get; set; }
            public string ModifierNames { get; set; }
            public string DeathReasons { get; set; }
            public string IsRevived  { get; set; }
            public string GhostInfo { get; set; }
            public string AbilityNames { get; set; }
            public int TasksCompleted  {get;set;}
            public int TasksTotal  {get;set;}
            public bool IsGuesser {get; set;}
            public int? Kills {get; set;}
            public bool IsAlive { get; set; }
        }
    }


    [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.OnGameEnd))]
    public static class OnGameEndPatch 
    {
        public static GameOverReason gameOverReason = GameOverReason.CrewmatesByTask;
        public static void Prefix(AmongUsClient __instance, [HarmonyArgument(0)]ref EndGameResult endGameResult) 
        {
            gameOverReason = endGameResult.GameOverReason;
            if ((int)endGameResult.GameOverReason >= 10) endGameResult.GameOverReason = GameOverReason.ImpostorsByKill;

            // Reset zoomed out ghosts
            Utils.ToggleZoom(reset: true);
        }

        public static void Postfix(AmongUsClient __instance, [HarmonyArgument(0)]ref EndGameResult endGameResult) 
        {
            AdditionalTempData.Clear();
            
            foreach(var playerControl in PlayerControl.AllPlayerControls) 
            {
                var roles = RoleInfo.GetRoleInfoForPlayer(playerControl);
                var modifiers = ModifierInfo.GetModifierInfoForPlayer(playerControl);
                var abilities = AbilityInfo.GetAbilityInfoForPlayer(playerControl);
                var (tasksCompleted, tasksTotal) = TasksHandler.TaskInfo(playerControl.Data);
                bool isGuesser = Guesser.IsGuesser(playerControl.PlayerId);
                int? killCount = GameHistory.deadPlayers.FindAll(x => x.GetKiller != null && x.GetKiller.PlayerId == playerControl.PlayerId).Count;
                if (killCount == 0 && !(new List<RoleInfo>() { RoleInfo.sheriff, RoleInfo.veteran}.Contains(RoleInfo.GetRoleInfoForPlayer(playerControl).FirstOrDefault()) || playerControl.IsKiller())) 
                {
                    killCount = null;
                }
                string roleString = GameHistory.RoleHistory.TryGetValue(playerControl.PlayerId, out var history) && history.Count > 0
                ? string.Join(" -> ", history.Select(r => Utils.ColorString(r.Color, r.Name)))
                : RoleInfo.GetRolesString(playerControl, true);

                string modifierString = ModifierInfo.GetModifiersString(playerControl, true);
                string abilityString = AbilityInfo.GetAbilitiesString(playerControl, true);
                string reasonsString = RoleInfo.GetDeathReasonString(playerControl);
                string ghostInfo = RoleInfo.GetGhostInfoString(playerControl);
                string wasRevived = MapOptions.RevivedPlayers.Contains(playerControl.PlayerId) ? " | <color=#82fbff>Revived</color>" : "";
                AdditionalTempData.playerRoles.Add(new AdditionalTempData.PlayerRoleInfo() 
                { 
                PlayerName = playerControl.Data.PlayerName, 
                Roles = roles, Modifiers = modifiers, 
                ModifierNames = modifierString,
                DeathReasons = reasonsString,
                GhostInfo = ghostInfo,
                IsRevived = wasRevived,
                Abilities = abilities, 
                AbilityNames = abilityString, 
                RoleNames = roleString, 
                TasksTotal = tasksTotal, 
                TasksCompleted = tasksCompleted, 
                IsGuesser = isGuesser, 
                Kills = killCount,
                IsAlive = !playerControl.Data.IsDead });
            }

            // Remove Jester, Arsonist, Scavenger, Jackal, former Jackals from winners (if they win, they'll be readded)
            List<PlayerControl> notWinners = new List<PlayerControl>();
            if (Jester.Player != null) notWinners.Add(Jester.Player);
            if (Jackal.Player != null) notWinners.Add(Jackal.Player);
            if (Glitch.Player != null) notWinners.Add(Glitch.Player);
            if (Werewolf.Player != null) notWinners.Add(Werewolf.Player);
            if (Amnesiac.Player != null) notWinners.Add(Amnesiac.Player);
            if (Predator.Player != null) notWinners.Add(Predator.Player);
            if (Arsonist.Player != null) notWinners.Add(Arsonist.Player);
            if (Scavenger.Player != null) notWinners.Add(Scavenger.Player);
            if (Lawyer.Player != null) notWinners.Add(Lawyer.Player);
            if (Survivor.Player != null) notWinners.Add(Survivor.Player);
            if (Romantic.Player != null) notWinners.Add(Romantic.Player);
            if (Juggernaut.Player != null) notWinners.Add(Juggernaut.Player);
            if (VengefulRomantic.Player != null) notWinners.Add(VengefulRomantic.Player);
            if (Plaguebearer.Player != null) notWinners.Add(Plaguebearer.Player);
            if (Pestilence.Player != null) notWinners.Add(Pestilence.Player);
            if (Agent.Player != null) notWinners.Add(Agent.Player);
            if (Hitman.Player != null) notWinners.Add(Hitman.Player);

            List<CachedPlayerData> winnersToRemove = new List<CachedPlayerData>();
            foreach (CachedPlayerData winner in EndGameResult.CachedWinners.GetFastEnumerator()) 
            {
                if (notWinners.Any(x => x.Data.PlayerName == winner.PlayerName)) winnersToRemove.Add(winner);
            }
            foreach (var winner in winnersToRemove) EndGameResult.CachedWinners.Remove(winner);

            bool jesterWin = Jester.Player != null && gameOverReason == (GameOverReason)CustomGameOverReason.JesterWin;
            bool arsonistWin = Arsonist.Player != null && gameOverReason == (GameOverReason)CustomGameOverReason.ArsonistWin;
            bool glitchWin = Glitch.Player != null && gameOverReason == (GameOverReason)CustomGameOverReason.GlitchWin;
            bool wwWin = Werewolf.Player != null && gameOverReason == (GameOverReason)CustomGameOverReason.WerewolfWin;
            bool SKWin = Predator.Player != null && gameOverReason == (GameOverReason)CustomGameOverReason.PredatorWin;
            bool RomanticWin = VengefulRomantic.Player != null && gameOverReason == (GameOverReason)CustomGameOverReason.VRomanticWin;
            bool JuggernautWin = Juggernaut.Player != null && gameOverReason == (GameOverReason)CustomGameOverReason.JuggernautWin;
            bool loversWin = Lovers.ExistingAndAlive() && (gameOverReason == (GameOverReason)CustomGameOverReason.LoversWin || (GameManager.Instance.DidHumansWin(gameOverReason) && !Lovers.ExistingWithKiller())); // Either they win if they are among the last 3 players, or they win if they are both Crewmates and both alive and the Crew wins (Team Imp/Jackal Lovers can only win solo wins)
            bool teamJackalWin = gameOverReason == (GameOverReason)CustomGameOverReason.TeamJackalWin && ((Jackal.Player != null && !Jackal.Player.Data.IsDead) || (Recruit.Player != null && !Recruit.Player.Data.IsDead));
            bool ScavengerWin = Scavenger.Player != null && gameOverReason == (GameOverReason)CustomGameOverReason.ScavengerWin;
            bool prosecutorWin = Prosecutor.Player != null && gameOverReason == (GameOverReason)CustomGameOverReason.ProsecutorWin;
            bool PlaguebearerWin = Plaguebearer.Player != null && gameOverReason == (GameOverReason)CustomGameOverReason.PlaguebearerWin;
            bool PestilenceWin = Pestilence.Player != null && gameOverReason == (GameOverReason)CustomGameOverReason.PestilenceWin;
            bool HitmanWin = Hitman.Player != null && gameOverReason == (GameOverReason)CustomGameOverReason.HitmanWin;
            bool AgentWin = Agent.Player != null && gameOverReason == (GameOverReason)CustomGameOverReason.AgentWin;

            // Hitman Win
            if (HitmanWin)
            {
                EndGameResult.CachedWinners = new Il2CppSystem.Collections.Generic.List<CachedPlayerData>();
                CachedPlayerData wpd = new CachedPlayerData(Hitman.Player.Data);
                EndGameResult.CachedWinners.Add(wpd);
                AdditionalTempData.WinCondition = WinCondition.HitmanWin;
            }

            // Agent Win
            else if (AgentWin)
            {
                EndGameResult.CachedWinners = new Il2CppSystem.Collections.Generic.List<CachedPlayerData>();
                CachedPlayerData wpd = new CachedPlayerData(Agent.Player.Data);
                EndGameResult.CachedWinners.Add(wpd);
                AdditionalTempData.WinCondition = WinCondition.AgentWin;
            }

            // Jester win
            else if (jesterWin)
            {
                EndGameResult.CachedWinners = new Il2CppSystem.Collections.Generic.List<CachedPlayerData>();
                CachedPlayerData wpd = new CachedPlayerData(Jester.Player.Data);
                EndGameResult.CachedWinners.Add(wpd);
                AdditionalTempData.WinCondition = WinCondition.JesterWin;
            }

            // Glitch win
            else if (glitchWin)
            {
                EndGameResult.CachedWinners = new Il2CppSystem.Collections.Generic.List<CachedPlayerData>();
                CachedPlayerData wpd = new CachedPlayerData(Glitch.Player.Data);
                EndGameResult.CachedWinners.Add(wpd);
                AdditionalTempData.WinCondition = WinCondition.GlitchWin;
            }

            // Vengeful Romantic win
            else if (RomanticWin)
            {
                EndGameResult.CachedWinners = new Il2CppSystem.Collections.Generic.List<CachedPlayerData>();
                CachedPlayerData wpd = new CachedPlayerData(VengefulRomantic.Player.Data);
                EndGameResult.CachedWinners.Add(wpd);
                AdditionalTempData.WinCondition = WinCondition.VRomanticWin;
            }

            // Plaguebearer Win
            else if (PlaguebearerWin)
            {
                EndGameResult.CachedWinners = new Il2CppSystem.Collections.Generic.List<CachedPlayerData>();
                CachedPlayerData wpd = new CachedPlayerData(Plaguebearer.Player.Data);
                EndGameResult.CachedWinners.Add(wpd);
                AdditionalTempData.WinCondition = WinCondition.PlaguebearerWin;
            }

            // Pestilence Win
            else if (PestilenceWin)
            {
                EndGameResult.CachedWinners = new Il2CppSystem.Collections.Generic.List<CachedPlayerData>();
                CachedPlayerData wpd = new CachedPlayerData(Pestilence.Player.Data);
                EndGameResult.CachedWinners.Add(wpd);
                AdditionalTempData.WinCondition = WinCondition.PestilenceWin;
            }

            // Werewolf win
            else if (wwWin)
            {
                EndGameResult.CachedWinners = new Il2CppSystem.Collections.Generic.List<CachedPlayerData>();
                CachedPlayerData wpd = new CachedPlayerData(Werewolf.Player.Data);
                EndGameResult.CachedWinners.Add(wpd);
                AdditionalTempData.WinCondition = WinCondition.WerewolfWin;
            }

            // Jugg win
            else if (JuggernautWin)
            {
                EndGameResult.CachedWinners = new Il2CppSystem.Collections.Generic.List<CachedPlayerData>();
                CachedPlayerData wpd = new CachedPlayerData(Juggernaut.Player.Data);
                EndGameResult.CachedWinners.Add(wpd);
                AdditionalTempData.WinCondition = WinCondition.JuggernautWin;
            }

            // Predator win
            else if (SKWin)
            {
                EndGameResult.CachedWinners = new Il2CppSystem.Collections.Generic.List<CachedPlayerData>();
                CachedPlayerData wpd = new CachedPlayerData(Predator.Player.Data);
                EndGameResult.CachedWinners.Add(wpd);
                AdditionalTempData.WinCondition = WinCondition.PredatorWin;
            }

            else if (DevPatches.HostEndedGame)
            {
                AdditionalTempData.WinCondition = WinCondition.HostEndedGame;
                // No winners because host manually ended the game
            }

            // Arsonist win
            else if (arsonistWin)
            {
                EndGameResult.CachedWinners = new Il2CppSystem.Collections.Generic.List<CachedPlayerData>();
                CachedPlayerData wpd = new CachedPlayerData(Arsonist.Player.Data);
                EndGameResult.CachedWinners.Add(wpd);
                AdditionalTempData.WinCondition = WinCondition.ArsonistWin;
            }

            // Scavenger win
            else if (ScavengerWin)
            {
                EndGameResult.CachedWinners = new Il2CppSystem.Collections.Generic.List<CachedPlayerData>();
                CachedPlayerData wpd = new CachedPlayerData(Scavenger.Player.Data);
                EndGameResult.CachedWinners.Add(wpd);
                AdditionalTempData.WinCondition = WinCondition.ScavengerWin;
            }

            // Prosecutor win
            else if (prosecutorWin)
            {
                EndGameResult.CachedWinners = new Il2CppSystem.Collections.Generic.List<CachedPlayerData>();
                CachedPlayerData wpd = new CachedPlayerData(Prosecutor.Player.Data);
                EndGameResult.CachedWinners.Add(wpd);
                AdditionalTempData.WinCondition = WinCondition.ProsecutorWin;
            }

            // Lovers win conditions
            else if (loversWin)
            {
                // Double win for lovers, crewmates also win
                if (!Lovers.ExistingWithKiller())
                {
                    AdditionalTempData.WinCondition = WinCondition.LoversTeamWin;
                    EndGameResult.CachedWinners = new Il2CppSystem.Collections.Generic.List<CachedPlayerData>();
                    foreach (PlayerControl p in PlayerControl.AllPlayerControls)
                    {
                        if (p == null) continue;
                        if (p == Lovers.Lover1 || p == Lovers.Lover2)
                            EndGameResult.CachedWinners.Add(new CachedPlayerData(p.Data));
                        else if (p == Survivor.Player && !Survivor.Player.Data.IsDead)
                            EndGameResult.CachedWinners.Add(new CachedPlayerData(p.Data));
                        else if (p.IsCrew())
                            EndGameResult.CachedWinners.Add(new CachedPlayerData(p.Data));
                    }
                }
                // Lovers solo win
                else
                {
                    AdditionalTempData.WinCondition = WinCondition.LoversSoloWin;
                    EndGameResult.CachedWinners = new Il2CppSystem.Collections.Generic.List<CachedPlayerData>();
                    EndGameResult.CachedWinners.Add(new CachedPlayerData(Lovers.Lover1.Data));
                    EndGameResult.CachedWinners.Add(new CachedPlayerData(Lovers.Lover2.Data));
                }
            }

            // Jackal win condition (should be implemented using a proper GameOverReason in the future)
            else if (teamJackalWin)
            {
                // Jackal wins if nobody except jackal is alive
                AdditionalTempData.WinCondition = WinCondition.JackalWin;
                EndGameResult.CachedWinners = new Il2CppSystem.Collections.Generic.List<CachedPlayerData>();
                CachedPlayerData wpd = new CachedPlayerData(Jackal.Player.Data);
                wpd.IsImpostor = false;
                EndGameResult.CachedWinners.Add(wpd);
                // If there is a Recruit. The Recruit also wins
                if (Recruit.Player != null)
                {
                    CachedPlayerData wpdRecruit = new CachedPlayerData(Recruit.Player.Data);
                    wpdRecruit.IsImpostor = false;
                    EndGameResult.CachedWinners.Add(wpdRecruit);
                }
            }

            // Possible Additional winner: Lawyer
            if (Lawyer.Player != null && Lawyer.target != null && (!Lawyer.target.Data.IsDead || Lawyer.target == Jester.Player)) {
                CachedPlayerData winningClient = null;
                foreach (CachedPlayerData winner in EndGameResult.CachedWinners.GetFastEnumerator()) 
                {
                    if (winner.PlayerName == Lawyer.target.Data.PlayerName)
                        winningClient = winner;
                }
                if (winningClient != null) 
                { // The Lawyer wins if the client is winning (and alive, but if he wasn't the Lawyer shouldn't exist anymore)
                    if (!EndGameResult.CachedWinners.ToArray().Any(x => x.PlayerName == Lawyer.Player.Data.PlayerName))
                        EndGameResult.CachedWinners.Add(new CachedPlayerData(Lawyer.Player.Data));
                    AdditionalTempData.additionalWinConditions.Add(WinCondition.AdditionalLawyerBonusWin); // The Lawyer wins together with the client
                } 
            }

            // Possible Additional winner: Romantic
            if (Romantic.Player != null && Romantic.beloved != null && (!Romantic.beloved.Data.IsDead || Romantic.beloved == Jester.Player)) 
            {
                CachedPlayerData winningClient = null;
                foreach (CachedPlayerData winner in EndGameResult.CachedWinners.GetFastEnumerator()) 
                {
                    if (winner.PlayerName == Romantic.beloved.Data.PlayerName)
                        winningClient = winner;
                }
                if (winningClient != null) 
                { // The Romantic wins if the beloved is winning (but if they weren't the Romantic shouldn't exist anymore)
                    if (!EndGameResult.CachedWinners.ToArray().Any(x => x.PlayerName == Romantic.Player.Data.PlayerName))
                        EndGameResult.CachedWinners.Add(new CachedPlayerData(Romantic.Player.Data));
                    AdditionalTempData.additionalWinConditions.Add(WinCondition.AdditionalRomanticBonusWin); // The Romantic wins together with the beloved
                } 
            }

            // Possible Additional winner: Beloved
            if (VengefulRomantic.Lover != null && VengefulRomantic.Player != null && !VengefulRomantic.Player.Data.IsDead) 
            {
                // Add the Vengeful Romantic to the winners list if not already present
                if (!EndGameResult.CachedWinners.ToArray().Any(x => x.PlayerName == VengefulRomantic.Player.Data.PlayerName))
                {
                    EndGameResult.CachedWinners.Add(new CachedPlayerData(VengefulRomantic.Player.Data));
                }

                // Add the lover to the winners list if not already present
                if (!EndGameResult.CachedWinners.ToArray().Any(x => x.PlayerName == VengefulRomantic.Lover.Data.PlayerName))
                {
                    EndGameResult.CachedWinners.Add(new CachedPlayerData(VengefulRomantic.Lover.Data));
                }

                // Add the win condition for both the Vengeful Romantic and their lover
                AdditionalTempData.additionalWinConditions.Add(WinCondition.AdditionalBelovedBonusWin); // The beloved wins together with the romantic
            }

            // Possible Additional winner: Survivor
            if (Survivor.Player != null && !Survivor.Player.Data.IsDead) 
            {
                if (!EndGameResult.CachedWinners.ToArray().Any(x => x.PlayerName == Survivor.Player.Data.PlayerName))
                    EndGameResult.CachedWinners.Add(new CachedPlayerData(Survivor.Player.Data));
                AdditionalTempData.additionalWinConditions.Add(WinCondition.AdditionalAliveSurvivorWin);
            }

            // If one lover wins: the other also does
            if (Lovers.Existing() && Lovers.Lover1 != null && Lovers.Lover2 != null)
            {
                if (EndGameResult.CachedWinners.ToArray().Any(x => x.PlayerName == Lovers.Lover1.Data.PlayerName) && !EndGameResult.CachedWinners.ToArray().Any(x => x.PlayerName == Lovers.Lover2.Data.PlayerName))
                {
                    EndGameResult.CachedWinners.Add(new CachedPlayerData(Lovers.Lover2.Data));
                    AdditionalTempData.additionalWinConditions.Add(WinCondition.AdditionalLoversPartnerWin);
                }
                else if (!EndGameResult.CachedWinners.ToArray().Any(x => x.PlayerName == Lovers.Lover1.Data.PlayerName) && EndGameResult.CachedWinners.ToArray().Any(x => x.PlayerName == Lovers.Lover2.Data.PlayerName))
                {
                    EndGameResult.CachedWinners.Add(new CachedPlayerData(Lovers.Lover1.Data));
                    AdditionalTempData.additionalWinConditions.Add(WinCondition.AdditionalLoversPartnerWin);
                }
            }
            AdditionalTempData.timer = ((float)(DateTime.UtcNow - TheSushiRolesPlugin.startTime).TotalMilliseconds) / 1000;
            RPCProcedure.ResetVariables();
        }
    }

    [HarmonyPatch(typeof(EndGameManager), nameof(EndGameManager.SetEverythingUp))]
    public class EndGameManagerSetUpPatch 
    {
        public static void Postfix(EndGameManager __instance)
        {
            // Delete and readd PoolablePlayers always showing the name and role of the player
            foreach (PoolablePlayer pb in __instance.transform.GetComponentsInChildren<PoolablePlayer>())
            {
                UnityEngine.Object.Destroy(pb.gameObject);
            }
            int num = Mathf.CeilToInt(7.5f);
            List<CachedPlayerData> list = EndGameResult.CachedWinners.ToArray().ToList().OrderBy(delegate (CachedPlayerData b)
            {
                if (!b.IsYou)
                {
                    return 0;
                }
                return -1;
            }).ToList<CachedPlayerData>();
            for (int i = 0; i < list.Count; i++)
            {
                CachedPlayerData CachedPlayerData2 = list[i];
                int num2 = (i % 2 == 0) ? -1 : 1;
                int num3 = (i + 1) / 2;
                float num4 = (float)num3 / (float)num;
                float num5 = Mathf.Lerp(1f, 0.75f, num4);
                float num6 = (float)((i == 0) ? -8 : -1);
                PoolablePlayer poolablePlayer = UnityEngine.Object.Instantiate<PoolablePlayer>(__instance.PlayerPrefab, __instance.transform);
                poolablePlayer.transform.localPosition = new Vector3(1f * (float)num2 * (float)num3 * num5, FloatRange.SpreadToEdges(-1.125f, 0f, num3, num), num6 + (float)num3 * 0.01f) * 0.9f;
                float num7 = Mathf.Lerp(1f, 0.65f, num4) * 0.9f;
                Vector3 vector = new Vector3(num7, num7, 1f);
                poolablePlayer.transform.localScale = vector;
                if (CachedPlayerData2.IsDead)
                {
                    poolablePlayer.SetBodyAsGhost();
                    poolablePlayer.SetDeadFlipX(i % 2 == 0);
                }
                else
                {
                    poolablePlayer.SetFlipX(i % 2 == 0);
                }
                poolablePlayer.UpdateFromPlayerOutfit(CachedPlayerData2.Outfit, PlayerMaterial.MaskType.None, CachedPlayerData2.IsDead, true);

                poolablePlayer.cosmetics.nameText.color = Color.white;
                poolablePlayer.cosmetics.nameText.transform.localScale = new Vector3(1f / vector.x, 1f / vector.y, 1f / vector.z);
                poolablePlayer.cosmetics.nameText.transform.localPosition = new Vector3(poolablePlayer.cosmetics.nameText.transform.localPosition.x, poolablePlayer.cosmetics.nameText.transform.localPosition.y, -15f);
                poolablePlayer.cosmetics.nameText.text = CachedPlayerData2.PlayerName;

                foreach (var data in AdditionalTempData.playerRoles)
                {
                    if (data.PlayerName != CachedPlayerData2.PlayerName) continue;
                    var roles =
                    poolablePlayer.cosmetics.nameText.text += $"\n{string.Join("\n", data.Roles.Select(x => Utils.ColorString(x.Color, x.Name)))}";
                }
            }

            // Additional code
            GameObject bonusText = UnityEngine.Object.Instantiate(__instance.WinText.gameObject);
            bonusText.transform.position = new Vector3(__instance.WinText.transform.position.x, __instance.WinText.transform.position.y - 0.5f, __instance.WinText.transform.position.z);
            bonusText.transform.localScale = new Vector3(0.7f, 0.7f, 1f);
            TMPro.TMP_Text textRenderer = bonusText.GetComponent<TMPro.TMP_Text>();
            textRenderer.text = "";

            switch (AdditionalTempData.WinCondition)
            {
                case WinCondition.JesterWin:
                    textRenderer.text = "Jester Wins!";
                    textRenderer.color = Jester.Color;
                    __instance.BackgroundBar.material.SetColor("_Color", Jester.Color);
                    break;

                case WinCondition.ArsonistWin:
                    textRenderer.text = "Arsonist Wins!";
                    textRenderer.color = Arsonist.Color;
                    __instance.BackgroundBar.material.SetColor("_Color", Arsonist.Color);
                    break;

                case WinCondition.PlaguebearerWin:
                    textRenderer.text = "Plaguebearer Wins!";
                    textRenderer.color = Plaguebearer.Color;
                    __instance.BackgroundBar.material.SetColor("_Color", Plaguebearer.Color);
                    break;

                case WinCondition.PestilenceWin:
                    textRenderer.text = "Pestilence Wins!";
                    textRenderer.color = Pestilence.Color;
                    __instance.BackgroundBar.material.SetColor("_Color", Pestilence.Color);
                    break;

                case WinCondition.ScavengerWin:
                    textRenderer.text = "Scavenger Wins!";
                    textRenderer.color = Scavenger.Color;
                    __instance.BackgroundBar.material.SetColor("_Color", Scavenger.Color);
                    break;

                case WinCondition.AgentWin:
                    textRenderer.text = "Agent Wins!";
                    textRenderer.color = Agent.Color;
                    __instance.BackgroundBar.material.SetColor("_Color", Agent.Color);
                    break;

                case WinCondition.HitmanWin:
                    textRenderer.text = "Hitman Wins!";
                    textRenderer.color = Hitman.Color;
                    __instance.BackgroundBar.material.SetColor("_Color", Hitman.Color);
                    break;

                case WinCondition.ProsecutorWin:
                    textRenderer.text = "Prosecutor Wins!";
                    textRenderer.color = Prosecutor.Color;
                    __instance.BackgroundBar.material.SetColor("_Color", Prosecutor.Color);
                    break;

                case WinCondition.LoversTeamWin:
                    textRenderer.text = "Lovers And Crewmates Win!";
                    textRenderer.color = Lovers.Color;
                    __instance.BackgroundBar.material.SetColor("_Color", Lovers.Color);
                    break;

                case WinCondition.JuggernautWin:
                    textRenderer.text = "Juggernaut Wins!";
                    textRenderer.color = Juggernaut.Color;
                    __instance.BackgroundBar.material.SetColor("_Color", Juggernaut.Color);
                    break;
                case WinCondition.HostEndedGame:
                    textRenderer.text = "Host Aborted The Game!";
                    textRenderer.color = Color.white;
                    __instance.BackgroundBar.material.SetColor("_Color", Color.white);
                    __instance.WinText.text = ""; // no victory or defeat bc of no winners...
                    break;

                case WinCondition.LoversSoloWin:
                    textRenderer.text = "Lovers Win!";
                    textRenderer.color = Lovers.Color;
                    __instance.BackgroundBar.material.SetColor("_Color", Lovers.Color);
                    break;

                case WinCondition.VRomanticWin:
                    textRenderer.text = "Vengeful Romantic Wins!";
                    textRenderer.color = Romantic.Color;
                    __instance.BackgroundBar.material.SetColor("_Color", Romantic.Color);
                    break;

                case WinCondition.JackalWin:
                    textRenderer.text = "Team Jackal Wins!";
                    textRenderer.color = Jackal.Color;
                    __instance.BackgroundBar.material.SetColor("_Color", Jackal.Color);
                    break;

                case WinCondition.GlitchWin:
                    textRenderer.text = "Glitch Wins!";
                    textRenderer.color = Glitch.Color;
                    __instance.BackgroundBar.material.SetColor("_Color", Glitch.Color);
                    break;

                case WinCondition.PredatorWin:
                    textRenderer.text = "Predator Wins!";
                    textRenderer.color = Predator.Color;
                    __instance.BackgroundBar.material.SetColor("_Color", Predator.Color);
                    break;

                case WinCondition.WerewolfWin:
                    textRenderer.text = "Werewolf Wins!";
                    textRenderer.color = Werewolf.Color;
                    __instance.BackgroundBar.material.SetColor("_Color", Werewolf.Color);
                    break;

                case WinCondition.Default:
                    switch (OnGameEndPatch.gameOverReason)
                    {
                        case GameOverReason.ImpostorDisconnect:
                            textRenderer.text = "Crewmates Disconnected. Impostors Win!";
                            textRenderer.color = Palette.ImpostorRed;
                            __instance.BackgroundBar.material.SetColor("_Color", Palette.ImpostorRed);
                            break;

                        case GameOverReason.ImpostorsByKill:
                            textRenderer.text = "Impostors Win!";
                            textRenderer.color = Palette.ImpostorRed;
                            __instance.BackgroundBar.material.SetColor("_Color", Palette.ImpostorRed);
                            break;

                        case GameOverReason.ImpostorsBySabotage:
                            textRenderer.text = "Impostors Win!";
                            textRenderer.color = Palette.ImpostorRed;
                            __instance.BackgroundBar.material.SetColor("_Color", Palette.ImpostorRed);
                            break;

                        case GameOverReason.ImpostorsByVote:
                            textRenderer.text = "Impostors Win!";
                            textRenderer.color = Palette.ImpostorRed;
                            __instance.BackgroundBar.material.SetColor("_Color", Palette.ImpostorRed);
                            break;

                        case GameOverReason.CrewmatesByTask:
                            textRenderer.text = "Crewmates Win!";
                            textRenderer.color = Palette.CrewmateBlue;
                            __instance.BackgroundBar.material.SetColor("_Color", Palette.CrewmateBlue);
                            break;

                        case GameOverReason.CrewmateDisconnect:
                            textRenderer.text = "Impostors Disconnected. Crewmates Win!";
                            textRenderer.color = Palette.CrewmateBlue;
                            __instance.BackgroundBar.material.SetColor("_Color", Palette.CrewmateBlue);
                            break;

                        case GameOverReason.CrewmatesByVote:
                            textRenderer.text = "Crewmates Win!";
                            textRenderer.color = Palette.CrewmateBlue;
                            __instance.BackgroundBar.material.SetColor("_Color", Palette.CrewmateBlue);
                            break;
                    }
                    break;
            }

            foreach (WinCondition cond in AdditionalTempData.additionalWinConditions)
            {
                if (cond == WinCondition.AdditionalLawyerBonusWin)
                {
                    textRenderer.text += $"\n{Utils.ColorString(Lawyer.Color, "The Lawyer wins with the client.")}";
                }
                else if (cond == WinCondition.AdditionalRomanticBonusWin)
                {
                    textRenderer.text += $"\n{Utils.ColorString(Romantic.Color, "The Romantic wins with their lover.")}";
                }
                else if (cond == WinCondition.AdditionalBelovedBonusWin)
                {
                    textRenderer.text += $"\n{Utils.ColorString(Romantic.Color, "The Beloved wins with their lover.")}";
                }
                else if (cond == WinCondition.AdditionalAliveSurvivorWin)
                {
                    textRenderer.text += $"\n{Utils.ColorString(Survivor.Color, "The Survivor is alive. They also win.")}";
                }
                else if (cond == WinCondition.AdditionalLoversPartnerWin)
                {
                    textRenderer.text += $"\n{Utils.ColorString(Lovers.Color, "The Lover wins with their partner.")}";
                }
            }

            var position = Camera.main.ViewportToWorldPoint(new Vector3(0f, 1f, Camera.main.nearClipPlane));
            GameObject roleSummary = UnityEngine.Object.Instantiate(__instance.WinText.gameObject);
            roleSummary.transform.position = new Vector3(__instance.Navigation.ExitButton.transform.position.x + 0.1f, position.y - 0.1f, -214f);
            roleSummary.transform.localScale = new Vector3(1f, 1f, 1f);

            var roleSummaryText = new StringBuilder();
            var winnersText = new StringBuilder();
            var winnersCache = new StringBuilder();
            var losersText = new StringBuilder();
            var winnerCount = 0;
            var loserCount = 0;
            int minutes = (int)AdditionalTempData.timer / 60;
            int seconds = (int)AdditionalTempData.timer % 60;
            roleSummaryText.AppendLine($"<size=125%><u><b><color=#FAD934FF>Match Duration: {minutes:00}:{seconds:00}</color></b></u></size> \n");

            roleSummaryText.AppendLine("<size=125%><u><b>Game Stats:</b></u></size>");
            roleSummaryText.AppendLine();
            winnersText.AppendLine("<size=105%><color=#00FF00FF><b>★ - Winners List - ★</b></color></size>");
            losersText.AppendLine("<size=105%><color=#FF0000FF><b>◆ - Losers List - ◆</b></color></size>");

            foreach (var data in AdditionalTempData.playerRoles)
            {
                var summaryText = new List<string>();

                // Name
                // white if alive else light gray
                string name = Utils.ColorString(data.IsAlive ? Color.white : new Color(.7f, .7f, .7f), data.PlayerName);
                summaryText.Add(name);

                // Role names
                if (!string.IsNullOrEmpty(data.RoleNames)) summaryText.Add($" - {data.RoleNames}");

                // Modifiers
                if (!string.IsNullOrEmpty(data.ModifierNames)) summaryText.Add($" ({data.ModifierNames})");

                // Abilities
                if (!string.IsNullOrEmpty(data.AbilityNames)) summaryText.Add($" [{data.AbilityNames}]");

                // Guessers
                if (data.IsGuesser) summaryText.Add($" [{Utils.ColorString(Color.red, "Guesser")}]");

                // Tasks
                if (data.TasksTotal > 0) summaryText.Add($" | Tasks: <color=#FAD934FF>({data.TasksCompleted}/{data.TasksTotal})</color>");

                // Kills
                if (data.Kills != null) summaryText.Add($" | <color=#FF0000FF>Kills: {data.Kills}</color>");

                // Ghost Info
                if (!string.IsNullOrEmpty(data.GhostInfo)) summaryText.Add(data.GhostInfo);

                // Is revived?
                if (!string.IsNullOrEmpty(data.IsRevived)) summaryText.Add(data.IsRevived);

                // Role targets
                if (PlayerControl.LocalPlayer.IsProssecutorTarget()) summaryText.Add($" | {Utils.ColorString(Prosecutor.Color, $"[X]")}");
                if (PlayerControl.LocalPlayer.IsLawyerClient()) summaryText.Add($" | {Utils.ColorString(Lawyer.Color, $"[⦿]")}");
                if (PlayerControl.LocalPlayer.IsShielded()) summaryText.Add($" | {Utils.ColorString(Medic.Color, $"[<b>+</b>]")}");
                if (Monarch.Player != null && Monarch.KnightedPlayers.Contains(PlayerControl.LocalPlayer)) summaryText.Add($" | {Utils.ColorString(Monarch.Color, $"[★]")}");
                if (PlayerControl.LocalPlayer.IsBeloved()) summaryText.Add($" | {Utils.ColorString(Romantic.Color, $"[♥]")}");

                // Death reasons
                if (!string.IsNullOrEmpty(data.DeathReasons)) summaryText.Add(data.DeathReasons);

                // everything together
                string dataString = $"<size=70%>{string.Join("", summaryText)}</size>";

                if (data.PlayerName.IsWinner())
                {
                    winnersText.AppendLine(dataString);
                    winnerCount++;
                }
                else
                {
                    losersText.AppendLine(dataString);
                    loserCount++;
                }
            }

            roleSummaryText.Append(winnersText);
            roleSummaryText.AppendLine();
            roleSummaryText.Append(losersText);

            var roleSummaryTextMesh = roleSummary.GetComponent<TMPro.TMP_Text>();
            roleSummaryTextMesh.alignment = TMPro.TextAlignmentOptions.TopLeft;
            roleSummaryTextMesh.color = Color.white;
            roleSummaryTextMesh.fontSizeMin = 1f;
            roleSummaryTextMesh.fontSizeMax = 1f;
            roleSummaryTextMesh.fontSize = 1f;
            roleSummaryTextMesh.text = $"{roleSummaryText}";
            roleSummaryTextMesh.GetComponent<RectTransform>().anchoredPosition = new(position.x + 3.5f, position.y - 0.1f);
            Utils.PreviousEndGameSummary = $"<size=110%>{roleSummaryText.ToString()}</size>";
            AdditionalTempData.Clear();
        }
    }

    // I had to delay this because RPC of ending game came before RPC of exiled, which causes issues with some roles
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.Die))]
    class CheckEndCriteriaPatch
    {
        public static void Postfix()
        {
            if (TheSushiRolesPlugin.NoEndGame || !AmongUsClient.Instance.AmHost || Utils.IsHideNSeek || AmongUsClient.Instance.GameState != InnerNet.InnerNetClient.GameStates.Started) return;

            Coroutines.Start(CheckForEndGame());
        }
        public static IEnumerator CheckForEndGame()
        {
            yield return new WaitForSeconds(1.5f);

            var statistics = new PlayerStatistics();
            if (CheckAndEndGameForJesterWin()) yield break;
            if (CheckAndEndGameForArsonistWin()) yield break;
            if (CheckAndEndGameForScavengerWin()) yield break;
            if (CheckAndEndGameForTaskWin()) yield break;
            if (CheckAndEndGameForProsecutorWin()) yield break;
            if (CheckAndEndGameForLoverWin(statistics)) yield break;
            if (CheckAndEndGameForJackalWin(statistics)) yield break;
            if (CheckAndEndGameForGlitchWin(statistics)) yield break;
            if (CheckAndEndGameForPestilenceWin(statistics)) yield break;
            if (CheckAndEndGameForPlaguebearerWin(statistics)) yield break;
            if (CheckAndEndGameForWerewolfWin(statistics)) yield break;
            if (CheckAndEndGameForRomanticWin(statistics)) yield break;
            if (CheckAndEndGameForPredatorWin(statistics)) yield break;
            if (CheckAndEndGameForJuggernautWin(statistics)) yield break;
            if (CheckAndEndGameForImpostorWin(statistics)) yield break;
            if (CheckAndEndGameForCrewmateWin(statistics)) yield break;
            if (CheckAndEndGameForHitmanWin(statistics)) yield break;
            if (CheckAndEndGameForAgentWin(statistics)) yield break;
        }

        private static bool CheckAndEndGameForJesterWin()
        {
            if (Jester.IsJesterWin)
            {
                Utils.EndGame((GameOverReason)CustomGameOverReason.JesterWin);
                return true;
            }
            return false;
        }
        private static bool CheckAndEndGameForArsonistWin()
        {
            if (Arsonist.IsArsonistWin)
            {
                Utils.EndGame((GameOverReason)CustomGameOverReason.ArsonistWin);
                return true;
            }
            return false;
        }

        private static bool CheckAndEndGameForScavengerWin()
        {
            if (Scavenger.IsScavengerWin)
            {
                Utils.EndGame((GameOverReason)CustomGameOverReason.ScavengerWin);
                return true;
            }
            return false;
        }

        private static bool CheckAndEndGameForTaskWin()
        {
            if (GameData.Instance.TotalTasks > 0 && GameData.Instance.TotalTasks <= GameData.Instance.CompletedTasks)
            {

                Utils.EndGame(GameOverReason.CrewmatesByTask);
                return true;
            }
            return false;
        }

        private static bool CheckAndEndGameForProsecutorWin()
        {
            if (Prosecutor.IsProsecutorWin)
            {
                Utils.EndGame((GameOverReason)CustomGameOverReason.ProsecutorWin);
                return true;
            }
            return false;
        }

        private static bool CheckAndEndGameForLoverWin(PlayerStatistics statistics)
        {
            if (statistics.TeamLoversAlive == 2 && statistics.TotalAlive <= 3)
            {
                Utils.EndGame((GameOverReason)CustomGameOverReason.LoversWin);
                return true;
            }
            return false;
        }

        private static bool CheckAndEndGameForJackalWin(PlayerStatistics statistics)
        {
            if (statistics.TeamJackalAlive >= statistics.TotalAlive - statistics.TeamJackalAlive
            && statistics.TeamImpostorsAlive == 0
            && statistics.GlitchAlive == 0
            && statistics.WerewolfAlive == 0
            && statistics.PredatorAlive == 0
            && statistics.VengefulRomanticAlive == 0
            && statistics.HitmanAlive == 0
            && statistics.AgentAlive == 0
            && statistics.PestilenceAlive == 0
            && statistics.PlaguebearerAlive == 0
            && statistics.CrewPowerAlive == 0
            && statistics.JuggernautAlive == 0
            && !(statistics.TeamJackalHasAliveLover
            && statistics.TeamLoversAlive == 2))
            {
                Utils.EndGame((GameOverReason)CustomGameOverReason.TeamJackalWin);
                return true;
            }
            return false;
        }

        private static bool CheckAndEndGameForPestilenceWin(PlayerStatistics statistics)
        {
            if (statistics.PestilenceAlive >= statistics.TotalAlive - statistics.PestilenceAlive
            && statistics.TeamImpostorsAlive == 0
            && statistics.GlitchAlive == 0
            && statistics.WerewolfAlive == 0
            && statistics.TeamJackalAlive == 0
            && statistics.PredatorAlive == 0
            && statistics.HitmanAlive == 0
            && statistics.AgentAlive == 0
            && statistics.VengefulRomanticAlive == 0
            && statistics.CrewPowerAlive == 0
            && statistics.JuggernautAlive == 0
            && !(statistics.PestilenceAliveHasLover
            && statistics.TeamLoversAlive == 2))
            {
                Utils.EndGame((GameOverReason)CustomGameOverReason.PestilenceWin);
                return true;
            }
            return false;
        }

        private static bool CheckAndEndGameForPlaguebearerWin(PlayerStatistics statistics)
        {
            if (statistics.PlaguebearerAlive >= statistics.TotalAlive - statistics.PlaguebearerAlive
            && statistics.TeamImpostorsAlive == 0
            && statistics.GlitchAlive == 0
            && statistics.WerewolfAlive == 0
            && statistics.TeamJackalAlive == 0
            && statistics.PredatorAlive == 0
            && statistics.HitmanAlive == 0
            && statistics.AgentAlive == 0
            && statistics.VengefulRomanticAlive == 0
            && statistics.CrewPowerAlive == 0
            && statistics.JuggernautAlive == 0
            && !(statistics.PlaguebearerAliveHasLover
            && statistics.TeamLoversAlive == 2))
            {
                Utils.EndGame((GameOverReason)CustomGameOverReason.PlaguebearerWin);
                return true;
            }
            return false;
        }

        private static bool CheckAndEndGameForJuggernautWin(PlayerStatistics statistics)
        {
            if (statistics.JuggernautAlive >= statistics.TotalAlive - statistics.JuggernautAlive
            && statistics.TeamImpostorsAlive == 0
            && statistics.GlitchAlive == 0
            && statistics.HitmanAlive == 0
            && statistics.AgentAlive == 0
            && statistics.WerewolfAlive == 0
            && statistics.PredatorAlive == 0
            && statistics.VengefulRomanticAlive == 0
            && statistics.PestilenceAlive == 0
            && statistics.PlaguebearerAlive == 0
            && statistics.CrewPowerAlive == 0
            && statistics.TeamJackalAlive == 0
            && !(statistics.JuggernautHasAliveLover
            && statistics.TeamLoversAlive == 2))
            {

                Utils.EndGame((GameOverReason)CustomGameOverReason.JuggernautWin);
                return true;
            }
            return false;
        }

        private static bool CheckAndEndGameForRomanticWin(PlayerStatistics statistics)
        {
            if (statistics.VengefulRomanticAlive >= statistics.TotalAlive - statistics.VengefulRomanticAlive
            && statistics.TeamImpostorsAlive == 0
            && statistics.GlitchAlive == 0
            && statistics.WerewolfAlive == 0
            && statistics.PestilenceAlive == 0
            && statistics.PlaguebearerAlive == 0
            && statistics.PredatorAlive == 0
            && statistics.HitmanAlive == 0
            && statistics.AgentAlive == 0
            && statistics.TeamJackalAlive == 0
            && statistics.JuggernautAlive == 0
            && statistics.CrewPowerAlive == 0
            && !(statistics.VengefulRomanticHasAliveLover
            && statistics.TeamLoversAlive == 2))
            {

                Utils.EndGame((GameOverReason)CustomGameOverReason.VRomanticWin);
                return true;
            }
            return false;
        }

        private static bool CheckAndEndGameForWerewolfWin(PlayerStatistics statistics)
        {
            if (statistics.WerewolfAlive >= statistics.TotalAlive - statistics.WerewolfAlive
            && statistics.TeamImpostorsAlive == 0
            && statistics.GlitchAlive == 0
            && statistics.TeamJackalAlive == 0
            && statistics.PredatorAlive == 0
            && statistics.CrewPowerAlive == 0
            && statistics.PestilenceAlive == 0
            && statistics.HitmanAlive == 0
            && statistics.AgentAlive == 0
            && statistics.PlaguebearerAlive == 0
            && statistics.JuggernautAlive == 0
            && statistics.VengefulRomanticAlive == 0
            && !(statistics.WerewolfAliveHasLover
            && statistics.TeamLoversAlive == 2))
            {
                Utils.EndGame((GameOverReason)CustomGameOverReason.WerewolfWin);
                return true;
            }
            return false;
        }

        private static bool CheckAndEndGameForPredatorWin(PlayerStatistics statistics)
        {
            if (statistics.PredatorAlive >= statistics.TotalAlive - statistics.PredatorAlive
            && statistics.TeamImpostorsAlive == 0
            && statistics.CrewPowerAlive == 0
            && statistics.TeamJackalAlive == 0
            && statistics.WerewolfAlive == 0
            && statistics.GlitchAlive == 0
            && statistics.PestilenceAlive == 0
            && statistics.PlaguebearerAlive == 0
            && statistics.HitmanAlive == 0
            && statistics.AgentAlive == 0
            && statistics.JuggernautAlive == 0
            && statistics.VengefulRomanticAlive == 0
            && !(statistics.PredatorHasLover
            && statistics.TeamLoversAlive == 2))
            {

                Utils.EndGame((GameOverReason)CustomGameOverReason.PredatorWin);
                return true;
            }
            return false;
        }

        private static bool CheckAndEndGameForHitmanWin(PlayerStatistics statistics)
        {
            if (statistics.HitmanAlive >= statistics.TotalAlive - statistics.HitmanAlive
            && statistics.TeamImpostorsAlive == 0
            && statistics.CrewPowerAlive == 0
            && statistics.TeamJackalAlive == 0
            && statistics.WerewolfAlive == 0
            && statistics.GlitchAlive == 0
            && statistics.PestilenceAlive == 0
            && statistics.PlaguebearerAlive == 0
            && statistics.PredatorAlive == 0
            && statistics.JuggernautAlive == 0
            && !(statistics.HitmanHasAliveLover
            && statistics.TeamLoversAlive == 2))
            {

                Utils.EndGame((GameOverReason)CustomGameOverReason.HitmanWin);
                return true;
            }
            return false;
        }
        private static bool CheckAndEndGameForAgentWin(PlayerStatistics statistics)
        {
            if (statistics.AgentAlive >= statistics.TotalAlive - statistics.AgentAlive
            && statistics.TeamImpostorsAlive == 0
            && statistics.CrewPowerAlive == 0
            && statistics.TeamJackalAlive == 0
            && statistics.WerewolfAlive == 0
            && statistics.GlitchAlive == 0
            && statistics.PestilenceAlive == 0
            && statistics.PlaguebearerAlive == 0
            && statistics.PredatorAlive == 0
            && statistics.JuggernautAlive == 0
            && !(statistics.AgentHasAliveLover
            && statistics.TeamLoversAlive == 2))
            {

                Utils.EndGame((GameOverReason)CustomGameOverReason.AgentWin);
                return true;
            }
            return false;
        }

        private static bool CheckAndEndGameForGlitchWin(PlayerStatistics statistics)
        {
            if (statistics.GlitchAlive >= statistics.TotalAlive - statistics.GlitchAlive
            && statistics.TeamImpostorsAlive == 0
            && statistics.CrewPowerAlive == 0
            && statistics.TeamJackalAlive == 0
            && statistics.WerewolfAlive == 0
            && statistics.PredatorAlive == 0
            && statistics.HitmanAlive == 0
            && statistics.AgentAlive == 0
            && statistics.PestilenceAlive == 0
            && statistics.PlaguebearerAlive == 0
            && statistics.JuggernautAlive == 0
            && statistics.VengefulRomanticAlive == 0
            && !(statistics.GlitchHasLoverAlive
            && statistics.TeamLoversAlive == 2))
            {

                Utils.EndGame((GameOverReason)CustomGameOverReason.GlitchWin);
                return true;
            }
            return false;
        }

        private static bool CheckAndEndGameForImpostorWin(PlayerStatistics statistics)
        {
            if (statistics.TeamImpostorsAlive >= statistics.TotalAlive - statistics.TeamImpostorsAlive
            && statistics.TeamJackalAlive == 0
            && statistics.GlitchAlive == 0
            && statistics.WerewolfAlive == 0
            && statistics.PredatorAlive == 0
            && statistics.HitmanAlive == 0
            && statistics.AgentAlive == 0
            && statistics.PestilenceAlive == 0
            && statistics.PlaguebearerAlive == 0
            && statistics.CrewPowerAlive == 0
            && statistics.VengefulRomanticAlive == 0
            && statistics.JuggernautAlive == 0
            && !(statistics.TeamImpostorHasAliveLover
            && statistics.TeamLoversAlive == 2))
            {
                GameOverReason endReason;
                switch (GameData.LastDeathReason)
                {
                    case DeathReason.Exile:
                        endReason = GameOverReason.ImpostorsByVote;
                        break;
                    case DeathReason.Kill:
                        endReason = GameOverReason.ImpostorsByKill;
                        break;
                    default:
                        endReason = GameOverReason.ImpostorsByVote;
                        break;
                }
                Utils.EndGame(endReason);
                return true;
            }
            return false;
        }

        private static bool CheckAndEndGameForCrewmateWin(PlayerStatistics statistics)
        {
            if (statistics.TeamImpostorsAlive == 0
            && statistics.TeamJackalAlive == 0
            && statistics.PredatorAlive == 0
            && statistics.VengefulRomanticAlive == 0
            && statistics.PestilenceAlive == 0
            && statistics.PlaguebearerAlive == 0
            && statistics.HitmanAlive == 0
            && statistics.AgentAlive == 0
            && statistics.JuggernautAlive == 0
            && statistics.WerewolfAlive == 0
            && statistics.GlitchAlive == 0)
            {
                Utils.EndGame(GameOverReason.CrewmatesByTask);
                return true;
            }
            return false;
        }
    }

    internal class PlayerStatistics 
    {
        public int TeamImpostorsAlive {get;set;}
        public int TeamJackalAlive {get;set;}
        public int GlitchAlive {get;set;}
        public int HitmanAlive {get;set;}
        public int AgentAlive {get;set;}
        public int PlaguebearerAlive {get;set;}
        public int PestilenceAlive {get;set;}
        public int WerewolfAlive {get;set;}
        public int VengefulRomanticAlive {get;set;}
        public int PredatorAlive {get;set;}
        public int JuggernautAlive {get;set;}
        public int CrewPowerAlive {get;set;}
        public int TeamLoversAlive {get;set;}
        public int TotalAlive {get;set;}
        public bool TeamImpostorHasAliveLover {get;set;}
        public bool TeamJackalHasAliveLover {get;set;}
        public bool VengefulRomanticHasAliveLover {get;set;}
        public bool HitmanHasAliveLover {get;set;}
        public bool AgentHasAliveLover {get;set;}
        public bool WerewolfAliveHasLover {get;set;}
        public bool PestilenceAliveHasLover {get;set;}
        public bool PlaguebearerAliveHasLover {get;set;}
        public bool GlitchHasLoverAlive {get;set;}
        public bool JuggernautHasAliveLover {get;set;}
        public bool PredatorHasLover {get;set;}

        public PlayerStatistics() 
        {
            GetPlayerCounts();
        }

        private static bool IsLover(NetworkedPlayerInfo p) 
        {
            return (Lovers.Lover1 != null && Lovers.Lover1.PlayerId == p.PlayerId) || (Lovers.Lover2 != null && Lovers.Lover2.PlayerId == p.PlayerId);
        }

        private void GetPlayerCounts() 
        {
            int numJackalAlive = 0;
            int numGlitchAlive = 0;
            int numImpostorsAlive = 0;
            int numberPestiAlive = 0;
            int numberPlagueAlive = 0;
            int numJuggAlive = 0;
            int numVRomanticsAlive = 0;
            int numWerewolfAlive = 0;
            int numPredatorAlive = 0;
            int numHitmanAlive = 0;
            int numAgentAlive = 0;
            int numCrewPowerAlive = 0;
            int numLoversAlive = 0;
            int numTotalAlive = 0;
            bool impLover = false;
            bool RomanticLover = false;
            bool glitchLover = false;
            bool PbLover = false;
            bool PestilenceLover = false;
            bool SKLover = false;
            bool jackalLover = false;
            bool JuggLover = false;
            bool WerewolfLover = false;
            bool hitmanLover = false;
            bool agentLover = false;

            foreach (var playerInfo in GameData.Instance.AllPlayers.GetFastEnumerator())
            {
                if (!playerInfo.Disconnected)
                {
                    if (!playerInfo.IsDead)
                    {
                        numTotalAlive++;

                        bool lover = IsLover(playerInfo);
                        if (lover) numLoversAlive++;

                        if (playerInfo.Role.IsImpostor && !Utils.IsJackal(Utils.GetPlayerById(playerInfo.PlayerId)))
                        {
                            numImpostorsAlive++;
                            if (lover) impLover = true;
                        }
                        if (Utils.IsJackal(Utils.GetPlayerById(playerInfo.PlayerId)))
                        {
                            numJackalAlive++;
                            if (lover) jackalLover = true;
                        }
                        if (Hitman.Player != null && Hitman.Player.PlayerId == playerInfo.PlayerId && !Utils.IsJackal(Utils.GetPlayerById(playerInfo.PlayerId)))
                        {
                            numHitmanAlive++;
                            if (lover) hitmanLover = true;
                        }
                        if (Agent.Player != null && Agent.Player.PlayerId == playerInfo.PlayerId && !Utils.IsJackal(Utils.GetPlayerById(playerInfo.PlayerId)))
                        {
                            numAgentAlive++;
                            if (lover) agentLover = true;
                        }
                        if (VengefulRomantic.Player != null && VengefulRomantic.Player.PlayerId == playerInfo.PlayerId && !Utils.IsJackal(Utils.GetPlayerById(playerInfo.PlayerId)))
                        {
                            numVRomanticsAlive++;
                            if (lover) RomanticLover = true;
                        }
                        if (Juggernaut.Player != null && Juggernaut.Player.PlayerId == playerInfo.PlayerId && !Utils.IsJackal(Utils.GetPlayerById(playerInfo.PlayerId)))
                        {
                            numJuggAlive++;
                            if (lover) JuggLover = true;
                        }
                        if (Pestilence.Player != null && Pestilence.Player.PlayerId == playerInfo.PlayerId && !Utils.IsJackal(Utils.GetPlayerById(playerInfo.PlayerId)))
                        {
                            numberPestiAlive++;
                            if (lover) PestilenceLover = true;
                        }
                        if (Plaguebearer.Player != null && Plaguebearer.Player.PlayerId == playerInfo.PlayerId && !Utils.IsJackal(Utils.GetPlayerById(playerInfo.PlayerId)))
                        {
                            numberPlagueAlive++;
                            if (lover) PbLover = true;
                        }
                        if (Sheriff.Player != null && Sheriff.Player.PlayerId == playerInfo.PlayerId && !Utils.IsJackal(Utils.GetPlayerById(playerInfo.PlayerId)))
                        {
                            numCrewPowerAlive++;
                        }
                        if (Mayor.Player != null && Mayor.Player.PlayerId == playerInfo.PlayerId && !Utils.IsJackal(Utils.GetPlayerById(playerInfo.PlayerId)))
                        {
                            numCrewPowerAlive++;
                        }
                        if (Veteran.Player != null && Veteran.Charges > 0 && Veteran.Player.PlayerId == playerInfo.PlayerId && !Utils.IsJackal(Utils.GetPlayerById(playerInfo.PlayerId)))
                        {
                            numCrewPowerAlive++;
                        }
                        if (Swapper.Player != null && Swapper.Charges > 0 && Swapper.Player.PlayerId == playerInfo.PlayerId && !Utils.IsJackal(Utils.GetPlayerById(playerInfo.PlayerId)))
                        {
                            numCrewPowerAlive++;
                        }
                        if (Tiebreaker.Player != null && Tiebreaker.Player.PlayerId == playerInfo.PlayerId && Tiebreaker.Player.IsCrew() && !Utils.IsJackal(Utils.GetPlayerById(playerInfo.PlayerId)))
                        {
                            numCrewPowerAlive++;
                        }
                        if (Predator.Player != null && Predator.Player.PlayerId == playerInfo.PlayerId && !Utils.IsJackal(Utils.GetPlayerById(playerInfo.PlayerId)))
                        {
                            numPredatorAlive++;
                            if (lover) SKLover = true;
                        }
                        if (Glitch.Player != null && Glitch.Player.PlayerId == playerInfo.PlayerId && !Utils.IsJackal(Utils.GetPlayerById(playerInfo.PlayerId)))
                        {
                            numGlitchAlive++;
                            if (lover) glitchLover = true;
                        }
                        if (Werewolf.Player != null && Werewolf.Player.PlayerId == playerInfo.PlayerId && !Utils.IsJackal(Utils.GetPlayerById(playerInfo.PlayerId)))
                        {
                            numWerewolfAlive++;
                            if (lover) WerewolfLover = true;
                        }
                    }
                }
            }

            TeamJackalAlive = numJackalAlive;
            JuggernautAlive = numJuggAlive;
            HitmanAlive = numHitmanAlive;
            AgentAlive = numAgentAlive;
            PlaguebearerAlive = numberPlagueAlive;
            PestilenceAlive = numberPestiAlive;
            GlitchAlive = numGlitchAlive;
            VengefulRomanticAlive = numVRomanticsAlive;
            PredatorAlive = numPredatorAlive;
            WerewolfAlive = numWerewolfAlive;
            CrewPowerAlive = numCrewPowerAlive;
            TeamImpostorsAlive = numImpostorsAlive;
            TeamLoversAlive = numLoversAlive;
            TotalAlive = numTotalAlive;
            TeamImpostorHasAliveLover = impLover;
            TeamJackalHasAliveLover = jackalLover;
            GlitchHasLoverAlive = glitchLover;
            PlaguebearerAliveHasLover = PbLover;
            PestilenceAliveHasLover = PestilenceLover;
            HitmanHasAliveLover = hitmanLover;
            AgentHasAliveLover = agentLover;
            VengefulRomanticHasAliveLover = RomanticLover;
            PredatorHasLover = SKLover;
            JuggernautHasAliveLover = JuggLover;
            WerewolfAliveHasLover = WerewolfLover;
        }
    }
    [HarmonyPatch]
    public static class CheckForSabotageWin
    {
        [HarmonyPatch(typeof(LogicGameFlowNormal), nameof(LogicGameFlowNormal.CheckEndCriteria))]
        public static bool Prefix(LogicGameFlowNormal __instance)
        {
            if (Utils.IsHideNSeek) return true;
            if (AmongUsClient.Instance.GameState != InnerNet.InnerNetClient.GameStates.Started) return false;
            if (Utils.TwoPlayersAlive()) return false;
            if (!AmongUsClient.Instance.AmHost) return false;
            if (ShipStatus.Instance.Systems != null)
            {
                if (ShipStatus.Instance.Systems.ContainsKey(SystemTypes.LifeSupp))
                {
                    var lifeSuppSystemType = ShipStatus.Instance.Systems[SystemTypes.LifeSupp].Cast<LifeSuppSystemType>();
                    if (lifeSuppSystemType.Countdown < 0f)
                    {
                        Utils.EndGame(GameOverReason.ImpostorsBySabotage);
                        return false;
                    }
                }

                if (ShipStatus.Instance.Systems.ContainsKey(SystemTypes.Laboratory))
                {
                    var reactorSystemType = ShipStatus.Instance.Systems[SystemTypes.Laboratory].Cast<ReactorSystemType>();
                    if (reactorSystemType.Countdown < 0f)
                    {
                        Utils.EndGame(GameOverReason.ImpostorsBySabotage);
                        return false;
                    }
                }

                if (ShipStatus.Instance.Systems.ContainsKey(SystemTypes.Reactor))
                {
                    var reactorSystemType = ShipStatus.Instance.Systems[SystemTypes.Reactor].Cast<ICriticalSabotage>();
                    if (reactorSystemType.Countdown < 0f)
                    {
                        Utils.EndGame(GameOverReason.ImpostorsBySabotage);
                        return false;
                    }
                }
            }
            return false;
        }
    }
}