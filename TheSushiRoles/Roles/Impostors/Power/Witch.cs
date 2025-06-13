using System.Collections.Generic;
namespace TheSushiRoles.Roles
{
    public static class Witch 
    {
        public static PlayerControl Player;
        public static Color Color = Palette.ImpostorRed;

        public static List<PlayerControl> futureSpelled = new List<PlayerControl>();
        public static PlayerControl CurrentTarget;
        public static PlayerControl spellCastingTarget;

        public static float currentCooldownAddition = 0f;

        public static void ClearAndReload() 
        {
            Player = null;
            futureSpelled = new List<PlayerControl>();
            CurrentTarget = spellCastingTarget = null;
            currentCooldownAddition = 0f;
        }
    }
}