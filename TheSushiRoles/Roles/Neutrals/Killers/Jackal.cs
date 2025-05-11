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
        public static List<PlayerControl> formerJackals = new List<PlayerControl>();
        
        public static float Cooldown;
        public static float createSidekickCooldown;
        public static bool canUseVents = true;
        public static bool canCreateSidekick = true;
        public static Sprite ButtonSprite;
        public static bool jackalPromotedFromSidekickCanCreateSidekick = true;
        public static bool canCreateSidekickFromImpostor = true;
        public static bool wasTeamRed;
        public static bool wasImpostor;
        public static bool wasSpy;

        public static Sprite getSidekickButtonSprite() 
        {
            if (ButtonSprite) return ButtonSprite;
            ButtonSprite = Utils.LoadSpriteFromResources("TheSushiRoles.Resources.SidekickButton.png", 115f);
            return ButtonSprite;
        }

        public static void RemoveCurrentJackal() 
        {
            if (!formerJackals.Any(x => x.PlayerId == Player.PlayerId)) formerJackals.Add(Player);
            Player = null;
            CurrentTarget = null;
            Cooldown = CustomOptionHolder.jackalKillCooldown.GetFloat();
            createSidekickCooldown = CustomOptionHolder.jackalCreateSidekickCooldown.GetFloat();
        }

        public static void ClearAndReload() 
        {
            Player = null;
            CurrentTarget = null;
            Cooldown = CustomOptionHolder.jackalKillCooldown.GetFloat();
            createSidekickCooldown = CustomOptionHolder.jackalCreateSidekickCooldown.GetFloat();
            canUseVents = CustomOptionHolder.jackalCanUseVents.GetBool();
            canCreateSidekick = CustomOptionHolder.jackalCanCreateSidekick.GetBool();
            jackalPromotedFromSidekickCanCreateSidekick = CustomOptionHolder.jackalPromotedFromSidekickCanCreateSidekick.GetBool();
            canCreateSidekickFromImpostor = CustomOptionHolder.jackalCanCreateSidekickFromImpostor.GetBool();
            formerJackals.Clear();
            wasTeamRed = wasImpostor = wasSpy = false;
        }
    }
}