namespace TheSushiRoles.Roles.Modifiers
{
    public static class Follower
    {
        public static PlayerControl Player;
        public static void ClearAndReload()
        {
            Player = null;
        }
    }
}