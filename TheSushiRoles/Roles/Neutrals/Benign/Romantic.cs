namespace TheSushiRoles.Roles
{
    public static class Romantic
    {
        public static PlayerControl Player;
        public static PlayerControl beloved;
        public static PlayerControl CurrentTarget;

        public static bool HasLover;
        public static Color Color = new Color32(255, 102, 204, byte.MaxValue);
        public static void ClearAndReload(bool ClearBeloved = true) 
        {
            Player = null;
            HasLover = false;
            if (ClearBeloved) 
            {
                beloved = null;
            }
        }
    }
}