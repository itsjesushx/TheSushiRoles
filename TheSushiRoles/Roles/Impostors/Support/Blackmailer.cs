namespace TheSushiRoles.Roles
{
    public static class Blackmailer
    {
        public static PlayerControl Player;
        public static PlayerControl CurrentTarget;
        public static PlayerControl BlackmailedPlayer;
        
        public static Color Color = Palette.ImpostorRed;

        public static bool IsBlackmailed(this PlayerControl player)
        {
            return BlackmailedPlayer != null &&
               !BlackmailedPlayer.Data.IsDead &&
               BlackmailedPlayer.PlayerId == player.PlayerId && Player != null;
        }
        public static bool CanSeeBlackmailed(byte playerId)
        {
            return !CustomGameOptions.BlackmailInvisible || BlackmailedPlayer?.PlayerId == playerId || Player.PlayerId == playerId || Utils.GetPlayerById(playerId).Data.IsDead;
        }
        public static bool ShouldShowBlackmail(PlayerControl player)
        {
            return BlackmailedPlayer != null && !BlackmailedPlayer.Data.IsDead && CanSeeBlackmailed(player.PlayerId);
        }
        public static void ClearAndReload()
        {
            Player = null;
            CurrentTarget = null;
            BlackmailedPlayer = null;
        }
    }
}