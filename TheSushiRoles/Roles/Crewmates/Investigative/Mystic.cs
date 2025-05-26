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
            ButtonSprite = Utils.LoadSprite("TheSushiRoles.Resources.Mystic.png", 115f);
            return ButtonSprite;
        }
        public static string GetInfo(PlayerControl target)
        {
            var roles = RoleInfo.GetRoleInfoForPlayer(target);
            if (target == null || roles == null) return "";

            string message = "";

            foreach (RoleInfo roleInfo in roles)
            {
                var id = roleInfo.RoleId;

                if (id.In(
                    RoleId.Jester, RoleId.Mayor, RoleId.Agent, RoleId.Lawyer,
                    RoleId.Prosecutor, RoleId.Monarch, RoleId.Yoyo, RoleId.Landlord))
                {
                    message = "I twist truth and law with charm and deception. My games govern hearts and halls. \n\n(Jester, Mayor, Agent, Lawyer, Prosecutor, Monarch, Yoyo, Landlord)";
                }
                else if (id.In(
                    RoleId.Wraith, RoleId.Assassin, RoleId.Witch, RoleId.Blackmailer,
                    RoleId.Trickster, RoleId.Swapper, RoleId.Eraser, RoleId.Spy))
                {
                    message = "I cloak my steps, whisper secrets, and shift the board without a trace. \n\n(Wraith, Assassin, Witch, Blackmailer, Trickster, Swapper, Eraser, Spy)";
                }
                else if (id.In(
                    RoleId.Detective, RoleId.Hacker, RoleId.Oracle, RoleId.Mystic,
                    RoleId.Psychic, RoleId.Chronos, RoleId.Painter, RoleId.Morphling))
                {
                    message = "I peer through time and disguise, revealing fates and faces unknown. \n\n(Detective, Hacker, Oracle, Mystic, Psychic, Chronos, Painter, Morphling)";
                }
                else if (id.In(
                    RoleId.Engineer, RoleId.Miner, RoleId.Janitor, RoleId.Scavenger,
                    RoleId.Undertaker, RoleId.Trapper, RoleId.Grenadier, RoleId.Sheriff))
                {
                    message = "I wield tools of war and work, cleaning, building, burying, or blasting. \n\n(Engineer, Miner, Janitor, Scavenger, Undertaker, Trapper, Grenadier, Sheriff)";
                }
                else if (id.In(
                    RoleId.Veteran, RoleId.BountyHunter, RoleId.Juggernaut, RoleId.Arsonist,
                    RoleId.Warlock, RoleId.Predator, RoleId.Viper, RoleId.Werewolf))
                {
                    message = "I am the storm. Fire, fang, and fury — nothing survives my wrath. \n\n(Veteran, Bounty Hunter, Juggernaut, Arsonist, Warlock, Predator, Viper, Werewolf)";
                }
                else if (id.In(
                    RoleId.Medic, RoleId.Crusader, RoleId.Romantic, RoleId.VengefulRomantic,
                    RoleId.Survivor))
                {
                    message = "I guard, love, endure — or avenge. Compassion and fury walk with me. \n\n(Medic, Crusader, Romantic, Vengeful Romantic, Survivor)";
                }
                else if (id.In(
                    RoleId.Plaguebearer, RoleId.Pestilence, RoleId.Glitch, RoleId.Jackal))
                {
                    message = "I am neither ally nor enemy — I am the anomaly, spreading decay or carving a lone path. \n\n(Plaguebearer, Pestilence, Glitch, Jackal)";
                }
                else if (id.In(
                    RoleId.Hitman, RoleId.Tracker, RoleId.Crewmate, RoleId.Impostor))
                {
                    message = "From humble light to lethal aim, I walk among you — familiar or fatal. \n\n(Hitman, Tracker, Crewmate, Impostor)";
                }
                else if (id.In(
                    RoleId.Gatekeeper, RoleId.Amnesiac, RoleId.Painter, RoleId.Oracle))
                {
                    message = "Some guard the gates, others forget themselves — but all touch the threads of fate. \n\n(Gatekeeper, Amnesiac, Painter, Oracle)";
                }
                else
                {
                    message = "I am the unknown, the undefined... yet part of the great design. \n\n(Unclassified)";
                }

                if (!string.IsNullOrEmpty(message)) break;
            }
            return target.Data.PlayerName + "'s Mind:\n" + message;
        }


        public static int Charges;
        public static int RechargeTasksNumber;
        public static int RechargedTasks;

        private static Sprite soulSprite;
        public static Sprite GetSoulSprite() 
        {
            if (soulSprite) return soulSprite;
            soulSprite = Utils.LoadSprite("TheSushiRoles.Resources.Soul.png", 500f);
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