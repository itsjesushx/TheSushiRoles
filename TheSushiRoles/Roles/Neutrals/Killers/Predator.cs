namespace TheSushiRoles.Roles
{
    public static class Predator
    {
        public static PlayerControl Player;
        public static PlayerControl CurrentTarget;
        public static Color Color = new Color32(51, 110, 255, byte.MaxValue);
        public static bool Terminating;
        public static bool HasImpostorVision;
        public static void ClearAndReload()
        {
            CurrentTarget = null;
            Player = null;
            Terminating = false;
            HasImpostorVision = false;
        }
    }
}