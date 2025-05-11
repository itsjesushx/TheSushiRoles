using UnityEngine;

namespace TheSushiRoles.Roles
{
    public static class Sheriff 
    {
        public static PlayerControl Player;
        public static Color Color = new Color32(248, 205, 70, byte.MaxValue);
        public static float Cooldown = 30f;
        public static bool canKillNeutrals = false;
        public static bool spyCanDieToSheriff = false;

        public static PlayerControl CurrentTarget;
        public static void ClearAndReload() 
        {
            Player = null;
            CurrentTarget = null;
            Cooldown = CustomOptionHolder.sheriffCooldown.GetFloat();
            canKillNeutrals = CustomOptionHolder.sheriffCanKillNeutrals.GetBool();
            spyCanDieToSheriff = CustomOptionHolder.spyCanDieToSheriff.GetBool();
        }
    }
}