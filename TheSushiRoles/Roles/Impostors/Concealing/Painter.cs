namespace TheSushiRoles.Roles
{
    public static class Painter 
    {
        public static PlayerControl Player;

        public static Color Color = Palette.ImpostorRed;

        public static float PaintTimer = 0f;
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
        }
    }
}