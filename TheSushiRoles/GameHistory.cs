using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

namespace TheSushiRoles 
{
    public class DeadPlayer
    {
        public enum CustomDeathReason 
        {
            Exile,
            Kill,
            Disconnect,
            Guess,
            Maul,
            LawyerSuicide,
            LoverSuicide,
            WitchExile,
            Arson,
        };

        public PlayerControl player;
        public DateTime timeOfDeath;
        public CustomDeathReason deathReason;
        public PlayerControl killerIfExisting;
        public bool wasCleaned;

        public DeadPlayer(PlayerControl player, DateTime timeOfDeath, CustomDeathReason deathReason, PlayerControl killerIfExisting) 
        {
            this.player = player;
            this.timeOfDeath = timeOfDeath;
            this.deathReason = deathReason;
            this.killerIfExisting = killerIfExisting;
            this.wasCleaned = false;
        }
    }

    static class GameHistory 
    {
        public static List<Tuple<Vector3, bool>> localPlayerPositions = new List<Tuple<Vector3, bool>>();
        public static List<DeadPlayer> deadPlayers = new List<DeadPlayer>();
        public static DateTime KillTime { get; set; }
        public static void ClearGameHistory() 
        {
            localPlayerPositions = new List<Tuple<Vector3, bool>>();
            deadPlayers = new List<DeadPlayer>();
        }

        public static void OverrideDeathReasonAndKiller(PlayerControl player, DeadPlayer.CustomDeathReason deathReason, PlayerControl killer = null) 
        {
            var target = deadPlayers.FirstOrDefault(x => x.player.PlayerId == player.PlayerId);
            if (target != null) {
                target.deathReason = deathReason;
                if (killer != null) {
                    target.killerIfExisting = killer;
                }
            } 
            else if (player != null) 
            {  // Create dead player if needed:
                var dp = new DeadPlayer(player, DateTime.UtcNow, deathReason, killer);
                deadPlayers.Add(dp);
            }
        }
    }
}