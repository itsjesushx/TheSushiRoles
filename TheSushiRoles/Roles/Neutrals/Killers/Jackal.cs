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
        public static float createSidekickCooldown;
        public static bool canUseVents = true;
        public static bool canCreateSidekick = true;
        public static Sprite ButtonSprite;
        public static bool canCreateSidekickFromImpostor = true;

        public static Sprite getSidekickButtonSprite() 
        {
            if (ButtonSprite) return ButtonSprite;
            ButtonSprite = Utils.LoadSpriteFromResources("TheSushiRoles.Resources.SidekickButton.png", 115f);
            return ButtonSprite;
        }

        public static void ClearAndReload() 
        {
            Player = null;
            CurrentTarget = null;
            Cooldown = CustomOptionHolder.jackalKillCooldown.GetFloat();
            createSidekickCooldown = CustomOptionHolder.jackalCreateSidekickCooldown.GetFloat();
            canUseVents = CustomOptionHolder.jackalCanUseVents.GetBool();
            canCreateSidekick = CustomOptionHolder.jackalCanCreateSidekick.GetBool();
        }
    }
}