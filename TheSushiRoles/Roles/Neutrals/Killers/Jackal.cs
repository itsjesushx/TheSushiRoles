using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TheSushiRoles.Roles
{
    public static class Jackal
    {
        public static PlayerControl Player;
        public static Color Color = new Color32(0, 180, 235, byte.MaxValue);
        public static PlayerControl CurrentTarget;
        
        public static float Cooldown;
        public static float createRecruitCooldown;
        public static bool canUseVents = true;
        public static bool canCreateRecruit = true;
        public static Sprite ButtonSprite;
        public static bool canCreateRecruitFromImpostor = true;
        public static bool HasRecruit;

        public static Sprite getRecruitButtonSprite()
        {
            if (ButtonSprite) return ButtonSprite;
            ButtonSprite = Utils.LoadSprite("TheSushiRoles.Resources.RecruitButton.png", 115f);
            return ButtonSprite;
        }

        public static void ClearAndReload() 
        {
            Player = null;
            CurrentTarget = null;
            HasRecruit = false;
            Cooldown = CustomOptionHolder.jackalKillCooldown.GetFloat();
            createRecruitCooldown = CustomOptionHolder.jackalCreateRecruitCooldown.GetFloat();
            canUseVents = CustomOptionHolder.jackalCanUseVents.GetBool();
            canCreateRecruit = CustomOptionHolder.jackalCanCreateRecruit.GetBool();
        }
    }
}