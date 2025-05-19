using UnityEngine;

namespace TheSushiRoles.Roles.Modifiers
{
    public static class Sidekick 
    {
        public static PlayerControl Player;
        public static Color Color = new Color32(0, 180, 235, byte.MaxValue);
        public static PlayerControl CurrentTarget;
        public static float Cooldown = 30f;
        public static void ClearAndReload() 
        {
            Player = null;
            CurrentTarget = null;
            Cooldown = CustomOptionHolder.jackalKillCooldown.GetFloat();
        }
    }
}