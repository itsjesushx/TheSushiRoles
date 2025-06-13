namespace TheSushiRoles.Roles
{
    public static class Hitman
    {
        public static PlayerControl Player;
        public static PlayerControl MorphTarget;
        public static PlayerControl SampledTarget;
        public static PlayerControl CurrentTarget;

        public static DeadBody BodyTarget;

        public static float MorphTimer = 0f;

        public static Color Color = new Color32(69, 133, 140, byte.MaxValue);
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
        }
    }
}