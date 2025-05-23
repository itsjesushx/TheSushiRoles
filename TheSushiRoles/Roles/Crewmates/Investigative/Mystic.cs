using System.Collections.Generic;
using UnityEngine;

namespace TheSushiRoles.Roles
{
    public static class Mystic 
    {
        public static bool Investigated;
        public static PlayerControl Player;
        public static Color Color = new Color32(77, 154, 230, byte.MaxValue);
        public static List<Vector3> deadBodyPositions = new List<Vector3>();
        public static PlayerControl CurrentTarget;
        public static float Cooldown;
        public static float soulDuration = 15f;
        public static bool limitSoulDuration = false;
        public static int mode = 0;
        private static Sprite ButtonSprite;
        public static Sprite GetButtonSprite() 
        {
            if (ButtonSprite) return ButtonSprite;
            ButtonSprite = Utils.LoadSpriteFromResources("TheSushiRoles.Resources.Mystic.png", 115f);
            return ButtonSprite;
        }
        public static string GetInfo(PlayerControl target)
        {
            var role = RoleInfo.GetRoleInfoForPlayer(target);
            if (target == null) return "";
            if (role == null) return "";

            string message = "";
            foreach (RoleInfo roleInfo in role) 
            {
                if (roleInfo.RoleId == RoleId.Jester || roleInfo.RoleId == RoleId.Prosecutor || roleInfo.RoleId == RoleId.Agent || roleInfo.RoleId == RoleId.Monarch || roleInfo.RoleId == RoleId.Mayor || roleInfo.RoleId == RoleId.Lawyer)
                {
                    message = "I dance on the edge of chaos, hold secrets, and strike with precision. \n\n(Jester, Prosecutor, Agent, Monarch, Mayor, or Lawyer)";
                }
                else if (roleInfo.RoleId == RoleId.Wraith || roleInfo.RoleId == RoleId.Swapper || roleInfo.RoleId == RoleId.Witch || roleInfo.RoleId == RoleId.Blackmailer || roleInfo.RoleId == RoleId.Tracker)
                {
                    message = "I bend reality, manipulate, and uncover hidden truths. \n\n(Wraith, Swapper, Witch, Blackmailer, or Tracker)";
                }
                else if (roleInfo.RoleId == RoleId.Detective || roleInfo.RoleId == RoleId.Hacker || roleInfo.RoleId == RoleId.Morphling || roleInfo.RoleId == RoleId.Medium || roleInfo.RoleId == RoleId.Hitman)
                {
                    message = "I uncover secrets, blur identities, and execute plans. \n\n(Detective, Hacker, Morphling, Medium, or Hitman)";
                }
                else if (roleInfo.RoleId == RoleId.Engineer || roleInfo.RoleId == RoleId.Vulture || roleInfo.RoleId == RoleId.Miner || roleInfo.RoleId == RoleId.Undertaker || roleInfo.RoleId == RoleId.Janitor || roleInfo.RoleId == RoleId.Sheriff)
                {
                    message = "I build, adapt, and ensure order prevails. \n\n(Engineer, Vulture, Undertaker, Miner, Janitor, or Sheriff)";
                }
                else if (roleInfo.RoleId == RoleId.Veteran || roleInfo.RoleId == RoleId.BountyHunter || roleInfo.RoleId == RoleId.Warlock || roleInfo.RoleId == RoleId.Werewolf || roleInfo.RoleId == RoleId.Juggernaut)
                {
                    message = "I stand ready, wield dark power, and leave destruction in my wake. \n\n(Veteran, Bounty Hunter, Warlock, Werewolf, or Juggernaut)";
                }
                else 
                {
                    message = "Error";
                }
            }
            return CurrentTarget.Data.PlayerName + "'s Mind:\n" + message;
        }

        public static int Charges;
        public static int RechargeTasksNumber;
        public static int RechargedTasks;

        private static Sprite soulSprite;
        public static Sprite GetSoulSprite() 
        {
            if (soulSprite) return soulSprite;
            soulSprite = Utils.LoadSpriteFromResources("TheSushiRoles.Resources.Soul.png", 500f);
            return soulSprite;
        }

        public static void ClearAndReload() 
        {
            Player = null;
            CurrentTarget = null;
            Investigated = false;
            deadBodyPositions = new List<Vector3>();
            Cooldown = CustomOptionHolder.MysticCooldown.GetFloat();
            limitSoulDuration = CustomOptionHolder.MysticLimitSoulDuration.GetBool();
            soulDuration = CustomOptionHolder.MysticSoulDuration.GetFloat();
            mode = CustomOptionHolder.MysticMode.GetSelection();
            Charges = Mathf.RoundToInt(CustomOptionHolder.MysticCharges.GetFloat());
            RechargeTasksNumber = Mathf.RoundToInt(CustomOptionHolder.MysticRechargeTasksNumber.GetFloat());
            RechargedTasks = Mathf.RoundToInt(CustomOptionHolder.MysticRechargeTasksNumber.GetFloat());
        }
    }
}