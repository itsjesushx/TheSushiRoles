using UnityEngine;

namespace TheSushiRoles.Roles
{
    public static class Hitman
    {
        public static PlayerControl Player;
        public static DeadBody BodyTarget;
        public static float Cooldown;
        public static float DragCooldown;
        public static float DragSpeed;
        public static float MorphCooldown;
        public static float MorphDuration;
        public static PlayerControl MorphTarget;
        public static PlayerControl SampledTarget;
        public static bool CanUseVents;
        public static PlayerControl CurrentTarget;
        public static float MorphTimer = 0f;
        public static Color Color = new Color32(69, 133, 140, byte.MaxValue);
        private static Sprite SampleSprite;
        private static Sprite MorphSprite;
        public static Sprite DragSprite1;
        public static Sprite DragSprite2;
        public static Sprite GetDragButtonSprite()
        {
            if (DragSprite1) return DragSprite1;
            DragSprite1 = Utils.LoadSprite("TheSushiRoles.Resources.Drag.png", 115f);
            return DragSprite1;
        }
        public static Sprite GetDropButtonSprite()
        {
            if (DragSprite2) return DragSprite2;
            DragSprite2 = Utils.LoadSprite("TheSushiRoles.Resources.Drop.png", 115f);
            return DragSprite2;
        }
        public static Sprite GetSampleSprite() 
        {
            if (SampleSprite) return SampleSprite;
            SampleSprite = Utils.LoadSprite("TheSushiRoles.Resources.SampleButton.png", 115f);
            return SampleSprite;
        }
        public static Sprite GetMorphSprite() 
        {
            if (MorphSprite) return MorphSprite;
            MorphSprite = Utils.LoadSprite("TheSushiRoles.Resources.MorphButton.png", 115f);
            return MorphSprite;
        }
        public static void ResetMorph() 
        {
            MorphTarget = null;
            MorphTimer = 0f;
            if (Player == null) return;
            Player.SetDefaultLook();
        }
        public static void ClearAndReload()
        {
            Player = null;
            MorphTarget = null;
            CurrentTarget = null;
            SampledTarget = null;
            BodyTarget = null;
            MorphTimer = 0f;
            Cooldown = CustomOptionHolder.HitmanCooldown.GetFloat();
            DragCooldown = CustomOptionHolder.HitmanDragCooldown.GetFloat();
            DragSpeed = CustomOptionHolder.HitmanDragSpeed.GetFloat();
            MorphCooldown = CustomOptionHolder.HitmanMorphCooldown.GetFloat();
            MorphDuration = CustomOptionHolder.HitmanMorphDuration.GetFloat();
            CanUseVents = CustomOptionHolder.HitmanCanUseVents.GetBool();
        }
    }
}