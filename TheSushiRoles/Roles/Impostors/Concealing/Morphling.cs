namespace TheSushiRoles.Roles
{
    public static class Morphling 
    {
        public static PlayerControl Player;
        public static PlayerControl sampledTarget;
        public static PlayerControl morphTarget;
        public static PlayerControl CurrentTarget;

        public static Color Color = Palette.ImpostorRed;
        
        public static float morphTimer = 0f;
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
        }

    }
}