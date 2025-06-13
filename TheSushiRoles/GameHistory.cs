using System.Collections.Generic;
using System.Linq;

namespace TheSushiRoles 
{
    public static class GameHistory 
    {
        public static List<Tuple<Vector3, bool>> LocalPlayerPositions = new List<Tuple<Vector3, bool>>();
        public static List<DeadPlayer> deadPlayers = new List<DeadPlayer>();
        public static readonly Dictionary<byte, List<RoleInfo>> RoleHistory = new();
        public static void ClearGameHistory() 
        {
            LocalPlayerPositions = new List<Tuple<Vector3, bool>>();
            deadPlayers = new List<DeadPlayer>();
            RoleHistory.Clear();
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
                var dp = new DeadPlayer(player, DateTime.UtcNow, DeathReason, killer);
                deadPlayers.Add(dp);
            }
        }
    }
}