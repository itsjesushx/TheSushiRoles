using System;

namespace TheSushiRoles 
{
    [HarmonyPatch]
    public static class TasksHandler 
    {
        public static Tuple<int, int> TaskInfo(NetworkedPlayerInfo playerInfo) 
        {
            int TotalTasks = 0;
            int CompletedTasks = 0;
            if (playerInfo != null && !playerInfo.Disconnected && playerInfo.Tasks != null &&
                playerInfo.Object &&
                playerInfo.Role && playerInfo.Role.TasksCountTowardProgress &&
                !playerInfo.Object.HasFakeTasks() && !playerInfo.Role.IsImpostor
                )
            {
                foreach (var playerInfoTask in playerInfo.Tasks.GetFastEnumerator())
                {
                    if (playerInfoTask.Complete) CompletedTasks++;
                    TotalTasks++;
                }
            }
            return Tuple.Create(CompletedTasks, TotalTasks);
        }

        [HarmonyPatch(typeof(GameData), nameof(GameData.RecomputeTaskCounts))]
        private static class GameDataRecomputeTaskCountsPatch 
        {
            private static bool Prefix(GameData __instance) 
            {
                var totalTasks = 0;
                var completedTasks = 0;
                
                foreach (var playerInfo in GameData.Instance.AllPlayers.GetFastEnumerator())
                {
                    if (playerInfo.Object
                        && playerInfo.Object.HasAliveKillingLover() // Tasks do not count if a Crewmate has an alive killing Lover
                        || playerInfo.PlayerId == Lawyer.Player?.PlayerId // Tasks of the Lawyer do not count
                        || playerInfo.PlayerId == Prosecutor.Player?.PlayerId // Tasks of the Prosecutor do not count
                        || playerInfo.PlayerId == Agent.Player?.PlayerId // Tasks of the Agent do not count
                        || (playerInfo.PlayerId == Survivor.Player?.PlayerId && Survivor.Player.Data.IsDead) // Tasks of the Survivor only count, if he's alive
                       )
                        continue;
                    var (playerCompleted, playerTotal) = TaskInfo(playerInfo);
                    totalTasks += playerTotal;
                    completedTasks += playerCompleted;
                }
                
                __instance.TotalTasks = totalTasks;
                __instance.CompletedTasks = completedTasks;
                return false;
            }
        }
        
    }
}
