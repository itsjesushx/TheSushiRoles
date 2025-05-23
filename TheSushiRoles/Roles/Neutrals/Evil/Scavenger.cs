using System.Collections.Generic;
using UnityEngine;

namespace TheSushiRoles.Roles
{
    public static class Scavenger 
    {
        public static PlayerControl Player;
        public static Color Color = new Color32(139, 69, 19, byte.MaxValue);
        public static List<Arrow> localArrows = new List<Arrow>();
        public static float Cooldown = 30f;
        public static int ScavengerNumberToWin = 4;
        public static List<Vector3> DeadBodyPositions = new();
        public static int eatenBodies = 0;
        public static bool IsScavengerWin = false;
        public static bool canUseVents = true;
        public static float ScavengeCooldown = 30f;
        public static float ScavengeDuration = 5f;
        public static float ScavengeTimer = 0f;
        private static Sprite ButtonSprite;
        public static Sprite GetButtonSprite() 
        {
            if (ButtonSprite) return ButtonSprite;
            ButtonSprite = Utils.LoadSpriteFromResources("TheSushiRoles.Resources.ScavengerButton.png", 115f);
            return ButtonSprite;
        }
        private static Sprite ButtonSprite2;
        public static Sprite GetScavengeSprite() 
        {
            if (ButtonSprite2) return ButtonSprite2;
            ButtonSprite2 = Utils.LoadSpriteFromResources("TheSushiRoles.Resources.ScavengeButton.png", 115f);
            return ButtonSprite2;
        }

        public static void ClearAndReload()
        {
            Player = null;
            ScavengerNumberToWin = Mathf.RoundToInt(CustomOptionHolder.ScavengerNumberToWin.GetFloat());
            eatenBodies = 0;
            Cooldown = CustomOptionHolder.ScavengerCooldown.GetFloat();
            IsScavengerWin = false;
            canUseVents = CustomOptionHolder.ScavengerCanUseVents.GetBool();
            ScavengeTimer = 0f;
            ScavengeCooldown = CustomOptionHolder.ScavengerScavengeCooldown.GetFloat();
            ScavengeDuration = CustomOptionHolder.ScavengerScavengeDuration.GetFloat();
            if (localArrows != null)
            {
                foreach (Arrow arrow in localArrows)
                    if (arrow?.arrow != null)
                        UnityEngine.Object.Destroy(arrow.arrow);
            }
            localArrows = new List<Arrow>();
            DeadBodyPositions = new List<Vector3>();
        }
    }
}