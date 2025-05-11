namespace TheSushiRoles.Roles.Modifiers
{
    public static class Tiebreaker 
    {
        public static PlayerControl Player;
        public static bool isTiebreak = false;
        public static void ClearAndReload() 
        {
            Player = null;
            isTiebreak = false;
        }
    }
}