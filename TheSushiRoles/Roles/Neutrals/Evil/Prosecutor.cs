namespace TheSushiRoles.Roles
{
    public static class Prosecutor
    {
        public static PlayerControl Player;
        public static PlayerControl target;

        public static Color Color = new Color32(166, 131, 212, byte.MaxValue);
        
        public static bool targetWasGuessed = false;
        public static bool IsProsecutorWin = false;

        public static void ClearAndReload(bool clearTarget = true) 
        {
            Player = null;
            if (clearTarget) 
            {
                target = null;
                targetWasGuessed = false;
            }
            IsProsecutorWin = false;
        }
    }
}