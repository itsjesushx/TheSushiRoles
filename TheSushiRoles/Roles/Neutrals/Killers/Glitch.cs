using System;
using System.Collections.Generic;
using Hazel;
using UnityEngine;

namespace TheSushiRoles.Roles
{
    public static class Glitch
    {
        public static PlayerControl Player;
        public static Color Color = Color.green;
        public static PlayerControl CurrentTarget;
        public static bool canEnterVents;
        public static float KillCooldown;
        public static List<byte> HackedPlayers = new List<byte>();
        public static float HackDuration;
        public static float remainingHacks;
        public static float HackCooldown;
        private static Sprite SampleSprite;
        private static Sprite MimicSprite;
        public static float MimicCooldown = 30f;
        public static float MimicDuration = 10f;
        public static PlayerControl sampledTarget;
        public static PlayerControl MimicTarget;
        public static float MimicTimer = 0f;
        public static Dictionary<byte, float> HackedKnows = new Dictionary<byte, float>();
        private static Sprite ButtonSprite;
        private static Sprite HackedSprite;
        
        public static Sprite GetButtonSprite()
        {
            if (ButtonSprite) return ButtonSprite;
            ButtonSprite = Utils.LoadSpriteFromResources("TheSushiRoles.Resources.Hack.png", 110f);
            return ButtonSprite;
        }
        public static Sprite GetHackedButtonSprite()
        {
            if (HackedSprite) return HackedSprite;
            HackedSprite = Utils.LoadSpriteFromResources("TheSushiRoles.Resources.Hack.png", 110f);
            return HackedSprite;
        }
        public static Sprite GetSampleSprite() 
        {
            if (SampleSprite) return SampleSprite;
            SampleSprite = Utils.LoadSpriteFromResources("TheSushiRoles.Resources.SampleButton.png", 115f);
            return SampleSprite;
        }
        public static Sprite GetMorphSprite() 
        {
            if (MimicSprite) return MimicSprite;
            MimicSprite = Utils.LoadSpriteFromResources("TheSushiRoles.Resources.MorphButton.png", 115f);
            return MimicSprite;
        }
        // Can be used to enable / disable the Hack effect on the target's buttons
        public static void SetHackedKnows(bool active = true, byte playerId = Byte.MaxValue)
        {
            if (playerId == Byte.MaxValue)
                playerId = PlayerControl.LocalPlayer.PlayerId;
            if (active && playerId == PlayerControl.LocalPlayer.PlayerId) 
            {
                Utils.StartRPC(CustomRPC.ShareGhostInfo, PlayerControl.LocalPlayer.PlayerId, (byte)GhostInfoTypes.HackNoticed);
            }
            if (active) 
            {
                HackedKnows.Add(playerId, HackDuration);
                HackedPlayers.RemoveAll(x => x == playerId);
           }
            if (playerId == PlayerControl.LocalPlayer.PlayerId) 
            {
                HudManagerStartPatch.SetAllButtonsHackedStatus(active);
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
            HudManagerStartPatch.SetAllButtonsHackedStatus(false, true);
            remainingHacks = CustomOptionHolder.GlitchNumberOfHacks.GetFloat();
            HackCooldown = CustomOptionHolder.GlitchHackCooldown.GetFloat();
            HackDuration = CustomOptionHolder.GlitchHackDuration.GetFloat();
            KillCooldown = CustomOptionHolder.GlitchKillCooldowm.GetFloat();
            canEnterVents = CustomOptionHolder.GlitchCanUseVents.GetBool();
            MimicCooldown = CustomOptionHolder.GlitchMimicCooldown.GetFloat();
            MimicDuration = CustomOptionHolder.GlitchMimicDuration.GetFloat();
        }
    }
}