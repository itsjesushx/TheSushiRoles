using UnityEngine;

namespace TheSushiRoles.Roles
{
    public static class Predator
    {
        public static PlayerControl Player;
        public static PlayerControl CurrentTarget;
        public static bool HasImpostorVision;
        public static Color Color = new Color32(51, 110, 255, byte.MaxValue);
        public static float TerminateCooldown;
        public static float TerminateDuration;
        public static float TerminateKillCooldown;
        public static bool CanUseVents;
        public static bool Terminating;
        private static Sprite ButtonSprite;
        public static Sprite GetButtonSprite() 
        {
        if (ButtonSprite) return ButtonSprite;
            ButtonSprite = Utils.LoadSpriteFromResources("TheSushiRoles.Resources.Terminate.png", 115f);
        return ButtonSprite;
        }            
        public static void ClearAndReload()
        {
            CurrentTarget = null;
            Player = null;
            Terminating = false;
            HasImpostorVision = false;
            TerminateCooldown = CustomOptionHolder.PredatorTerminateCooldown.GetFloat();
            TerminateDuration = CustomOptionHolder.PredatorTerminateDuration.GetFloat();
            TerminateKillCooldown = CustomOptionHolder.PredatorTerminateKillCooldown.GetFloat();
            CanUseVents = CustomOptionHolder.PredatorCanUseVents.GetBool();
        }
    }
}