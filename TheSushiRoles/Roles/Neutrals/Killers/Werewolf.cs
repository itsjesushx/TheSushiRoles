namespace TheSushiRoles.Roles
{
    public static class Werewolf
    {
        public static Color Color = new Color32(168, 102, 41, byte.MaxValue);
        public static PlayerControl Player;
        public static PlayerControl CurrentTarget;
        public static void ClearAndReload()
        {
            CurrentTarget = null;
            Player = null;
        }
    }
}