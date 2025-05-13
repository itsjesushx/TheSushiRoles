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
            WrongSidekick,
            LoverSuicide,
            WitchExile,
            Arson,
        };

        public PlayerControl player;
        public DateTime DeathTime;
        public CustomDeathReason DeathReason;
        public PlayerControl GetKiller;
        public bool WasCleanedOrEaten;

        public DeadPlayer(PlayerControl player, DateTime DeathTime, CustomDeathReason DeathReason, PlayerControl GetKiller) 
        {
            this.player = player;
            this.DeathTime = DeathTime;
            this.DeathReason = DeathReason;
            this.GetKiller = GetKiller;
            this.WasCleanedOrEaten = false;
        }
    }

    static class GameHistory 
    {
        public static List<Tuple<Vector3, bool>> LocalPlayerPositions = new List<Tuple<Vector3, bool>>();
        public static List<DeadPlayer> deadPlayers = new List<DeadPlayer>();
        public static Dictionary<byte, List<RoleInfo>> RoleHistory = new();
        public static DateTime KillTime { get; set; }
        public static void ClearGameHistory() 
        {
            LocalPlayerPositions = new List<Tuple<Vector3, bool>>();
            deadPlayers = new List<DeadPlayer>();
            RoleHistory = new Dictionary<byte, List<RoleInfo>>();
        }
        public static void AddToRoleHistory(byte playerId, RoleInfo role)
        {
            if (!RoleHistory.ContainsKey(playerId))
                RoleHistory[playerId] = new List<RoleInfo>();

            if (!RoleHistory[playerId].Contains(role))
                RoleHistory[playerId].Add(role);
        }

        public static void CreateDeathReason(PlayerControl player, DeadPlayer.CustomDeathReason DeathReason, PlayerControl killer = null) 
        {
            var target = deadPlayers.FirstOrDefault(x => x.player.PlayerId == player.PlayerId);
            if (target != null) 
            {
                target.DeathReason = DeathReason;
                if (killer != null) 
                {
                    target.GetKiller = killer;
                }
            } 
            else if (player != null) 
            {  
                // Create dead player if needed:
                var dp = new DeadPlayer(player, DateTime.UtcNow, DeathReason, killer);
                deadPlayers.Add(dp);
            }
        }
    }
}