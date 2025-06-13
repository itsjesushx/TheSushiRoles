using System.Collections.Generic;

namespace TheSushiRoles.Roles
{
    public static class Glitch
    {
        public static PlayerControl Player;
        public static PlayerControl sampledTarget;
        public static PlayerControl MimicTarget;
        public static PlayerControl CurrentTarget;

        public static int remainingHacks = 0;

        public static Color Color = Color.green;

        public static List<byte> HackedPlayers = new List<byte>();        
        public static float MimicTimer = 0f;
        public static Dictionary<byte, float> HackedKnows = new Dictionary<byte, float>();
        // Can be used to enable / disable the Hack effect on the target's buttons
        public static void SetHackedKnows(bool active = true, byte playerId = Byte.MaxValue)
        {
            if (playerId == Byte.MaxValue)
                playerId = PlayerControl.LocalPlayer.PlayerId;
            if (active && playerId == PlayerControl.LocalPlayer.PlayerId) 
            {
                Utils.SendRPC(CustomRPC.ShareGhostInfo, PlayerControl.LocalPlayer.PlayerId, (byte)GhostInfoTypes.HackNoticed);
            }
            if (active)
            {
                HackedKnows.Add(playerId, CustomGameOptions.GlitchHackDuration);
                HackedPlayers.RemoveAll(x => x == playerId);
           }
            if (playerId == PlayerControl.LocalPlayer.PlayerId) 
            {
                CustomButtonLoader.SetAllButtonsHackedStatus(active);
                SoundEffectsManager.Play("deputyHandcuff");
		    }
	    }
        public static void ResetMimic() 
        {
            MimicTarget = null;
            MimicTimer = 0f;
            if (Player == null) return;
            Player.SetDefaultLook();
        }
        public static void ClearAndReload()
        {
            ResetMimic();
            Player = null;
            sampledTarget = null;
            MimicTarget = null;
            MimicTimer = 0f;
            CurrentTarget = null;
            HackedPlayers = new List<byte>();
            HackedKnows = new Dictionary<byte, float>();
            CustomButtonLoader.SetAllButtonsHackedStatus(false, true);
            remainingHacks = CustomGameOptions.GlitchNumberOfHacks;
        }
    }
}