using UnityEngine;

namespace TheSushiRoles.Roles
{
    public static class Painter 
    {
        public static PlayerControl Player;
        public static Color Color = Palette.ImpostorRed;
    
        public static float Cooldown = 30f;
        public static float Duration = 10f;
        public static float PaintTimer = 0f;

        private static Sprite ButtonSprite;
        public static Sprite GetButtonSprite() 
        {
            if (ButtonSprite) return ButtonSprite;
            ButtonSprite = Utils.LoadSprite("TheSushiRoles.Resources.CamoButton.png", 115f);
            return ButtonSprite;
        }

        public static void ResetPaint() 
        {
            PaintTimer = 0f;
            foreach (PlayerControl p in PlayerControl.AllPlayerControls) 
            {
                if (p == Assassin.Player && Assassin.isInvisble) continue;
                if (p == Wraith.Player && Wraith.IsVanished) continue;
                p.SetDefaultLook();
            }
        }

        public static void ClearAndReload() 
        {
            ResetPaint();
            Player = null;
            PaintTimer = 0f;
            Cooldown = CustomOptionHolder.PainterCooldown.GetFloat();
            Duration = CustomOptionHolder.PainterDuration.GetFloat();
        }
    }
}