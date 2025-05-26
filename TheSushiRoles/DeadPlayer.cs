using System;

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
            WrongRecruit,
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
}