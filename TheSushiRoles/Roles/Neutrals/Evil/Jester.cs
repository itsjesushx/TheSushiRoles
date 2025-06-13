namespace TheSushiRoles.Roles
{
    public class Jester
    {
        public static PlayerControl Player;

        public static Color Color = new Color32(255, 191, 204, byte.MaxValue);

        public static bool IsJesterWin = false;
        public static void ClearAndReload() 
        {
            Player = null;
            IsJesterWin = false;
        }
    }
}