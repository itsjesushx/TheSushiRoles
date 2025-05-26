using UnityEngine;

namespace TheSushiRoles.Roles
{
    public static class Morphling 
    {
        public static PlayerControl Player;
        public static Color Color = Palette.ImpostorRed;
        private static Sprite sampleSprite;
        private static Sprite morphSprite;
        public static float Cooldown = 30f;
        public static float Duration = 10f;
        public static PlayerControl sampledTarget;
        public static PlayerControl morphTarget;
        public static float morphTimer = 0f;
        public static PlayerControl CurrentTarget;

        public static void ResetMorph() 
        {
            morphTarget = null;
            morphTimer = 0f;
            if (Player == null) return;
            Player.SetDefaultLook();
        }

        public static void ClearAndReload() 
        {
            ResetMorph();
            Player = null;
            CurrentTarget = null;
            sampledTarget = null;
            morphTarget = null;
            morphTimer = 0f;
            Cooldown = CustomOptionHolder.morphlingCooldown.GetFloat();
            Duration = CustomOptionHolder.morphlingDuration.GetFloat();
        }

        public static Sprite GetSampleSprite() 
        {
            if (sampleSprite) return sampleSprite;
            sampleSprite = Utils.LoadSprite("TheSushiRoles.Resources.SampleButton.png", 115f);
            return sampleSprite;
        }

        public static Sprite GetMorphSprite() 
        {
            if (morphSprite) return morphSprite;
            morphSprite = Utils.LoadSprite("TheSushiRoles.Resources.MorphButton.png", 115f);
            return morphSprite;
        }
    }
}