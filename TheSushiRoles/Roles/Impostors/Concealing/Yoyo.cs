namespace TheSushiRoles.Roles
{
    public static class Yoyo 
    {
        public static PlayerControl Player = null;

        public static Color Color = Palette.ImpostorRed;
        public static float SilhouetteVisibility => (CustomGameOptions.YoyoSilhouetteVisibility == 0 && (PlayerControl.LocalPlayer == Player || PlayerControl.LocalPlayer.Data.IsDead)) ? 0.1f : CustomGameOptions.YoyoSilhouetteVisibility;
        public static Vector3? markedLocation = null;
        public static void MarkLocation(Vector3 position) 
        {
            markedLocation = position;
        }

        public static void ClearAndReload() 
        {
            markedLocation = null;
        }
    }
}