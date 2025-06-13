namespace TheSushiRoles.Roles
{
    public static class Grenadier
    {
        public static PlayerControl Player;

        public static Color Color = Palette.ImpostorRed;

        public static bool Active;

        public static Il2CppSystem.Collections.Generic.List<PlayerControl> ClosestPlayers = null;
        public static Il2CppSystem.Collections.Generic.List<PlayerControl> FlashedPlayers = new Il2CppSystem.Collections.Generic.List<PlayerControl>();

        public static void ClearAndReload() 
        {
            Player = null;
            Active = false;
        }
    }
}