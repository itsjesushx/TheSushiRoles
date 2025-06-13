namespace TheSushiRoles.Roles
{
    public static class Lawyer 
    {
        public static PlayerControl Player;
        public static PlayerControl target;
        
        public static Color Color = new Color32(112, 185, 141, byte.MaxValue);

        public static bool targetWasGuessed = false;
        public static void ClearAndReload(bool ClearTarget = true)
        {
            Player = null;
            if (ClearTarget)
            {
                target = null;
                targetWasGuessed = false;
            }
        }
    }
}