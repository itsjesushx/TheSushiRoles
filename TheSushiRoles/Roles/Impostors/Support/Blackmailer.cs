using UnityEngine;

namespace TheSushiRoles.Roles
{
    public static class Blackmailer
    {
        public static PlayerControl Player;
        public static PlayerControl CurrentTarget;
        public static PlayerControl BlackmailedPlayer;
        public static Color Color = Palette.ImpostorRed;
        public static float Cooldown;
        public static bool BlackmailInvisible;
        public static bool IsBlackmailed(PlayerControl player)
        {
            return BlackmailedPlayer != null &&
               !BlackmailedPlayer.Data.IsDead &&
               BlackmailedPlayer.PlayerId == player.PlayerId && Player != null;
        }
        public static bool CanSeeBlackmailed(byte playerId)
        {
            return !BlackmailInvisible || BlackmailedPlayer?.PlayerId == playerId || Player.PlayerId == playerId || Utils.PlayerById(playerId).Data.IsDead;
        }
        public static bool ShouldShowBlackmail(PlayerControl player)
        {
            return BlackmailedPlayer != null && !BlackmailedPlayer.Data.IsDead && CanSeeBlackmailed(player.PlayerId);
        }
        public static Sprite ButtonSprite2;
        public static Sprite GetBlackmailLetter()
        {
            if (ButtonSprite2 == null)
            {
                ButtonSprite2 = Utils.LoadSprite("TheSushiRoles.Resources.BlackmailLetter.png", 150f);
            }
            return ButtonSprite2;
        }
        public static Sprite ButtonSprite3;
        public static Sprite GetBlackmailOverlay()
        {
            if (ButtonSprite3 == null)
            {
                ButtonSprite3 = Utils.LoadSprite("TheSushiRoles.Resources.BlackmailOverlay.png", 100f);
            }
            return ButtonSprite3;
        }
        public static Sprite ButtonSprite;
        public static Sprite GetButtonSprite()
        {
            if (ButtonSprite == null)
            {
                ButtonSprite = Utils.LoadSprite("TheSushiRoles.Resources.BlackmailButton.png", 115f);
            }
            return ButtonSprite;
        }
        public static void ClearAndReload()
        {
            Player = null;
            CurrentTarget = null;
            BlackmailedPlayer = null;
            Cooldown = CustomOptionHolder.BlackmailCooldown.GetFloat();
            BlackmailInvisible = CustomOptionHolder.BlackmailInvisible.GetBool();
        }
    }
}