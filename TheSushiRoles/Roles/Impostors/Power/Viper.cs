using System.Collections.Generic;
using UnityEngine;

namespace TheSushiRoles.Roles
{
    public static class Viper 
    {
        public static PlayerControl Player;
        public static Color Color = Palette.ImpostorRed;
        public static HashSet<byte> BlindedPlayers = new HashSet<byte>();
        public static float delay = 3f;
        public static float Cooldown = 30f;
        public static float BlindCooldown = 30f;
        public static float BlindDuration = 6f;

        public static PlayerControl CurrentTarget;
        public static PlayerControl poisoned; 

        private static Sprite ButtonSprite;
        public static Sprite GetButtonSprite() 
        {
            if (ButtonSprite) return ButtonSprite;
            ButtonSprite = Utils.LoadSpriteFromResources("TheSushiRoles.Resources.PoisonButton.png", 115f);
            return ButtonSprite;
        }

        private static Sprite ButtonSprite2;
        public static Sprite GetBlindSprite() 
        {
            if (ButtonSprite2) return ButtonSprite2;
            ButtonSprite2 = Utils.LoadSpriteFromResources("TheSushiRoles.Resources.BlindTrapButton.png", 115f);
            return ButtonSprite2;
        }

        public static void ClearAndReload() 
        {
            Player = null;
            poisoned = null;
            CurrentTarget = null;
            delay = CustomOptionHolder.ViperKillDelay.GetFloat();
            Cooldown = CustomOptionHolder.ViperCooldown.GetFloat();
            BlindCooldown = CustomOptionHolder.BlindCooldown.GetFloat();
            BlindDuration = CustomOptionHolder.BlindDuration.GetFloat();
            BlindedPlayers = new HashSet<byte>();
        }
    }
}