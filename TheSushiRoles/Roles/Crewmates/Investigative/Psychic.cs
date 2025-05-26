using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TheSushiRoles.Roles
{
    public static class Psychic 
    {
        public static PlayerControl Player;
        public static DeadPlayer target;
        public static DeadPlayer soulTarget;
        public static Color Color = new Color32(98, 120, 115, byte.MaxValue);
        public static List<Tuple<DeadPlayer, Vector3>> deadBodies = new List<Tuple<DeadPlayer, Vector3>>();
        public static List<Tuple<DeadPlayer, Vector3>> futureDeadBodies = new List<Tuple<DeadPlayer, Vector3>>();
        public static List<SpriteRenderer> souls = new List<SpriteRenderer>();
        public static DateTime meetingStartTime = DateTime.UtcNow;
        public static float Cooldown = 30f;
        public static float Duration = 3f;
        public static bool oneTimeUse = false;
        public static float chanceAdditionalInfo = 0f;
        private static Sprite soulSprite;
        enum SpecialPsychicInfo 
        {
            SheriffSuicide,
            ActiveLoverDies,
            PassiveLoverSuicide,
            LawyerKilledByClient,
            RomanticKilledByBeloved,
            JackalKillsRecruit,
            ImpostorTeamkill,
            SubmergedO2,
            WarlockSuicide,
            BodyCleaned,
        }

        public static Sprite GetSoulSprite() 
        {
            if (soulSprite) return soulSprite;
            soulSprite = Utils.LoadSprite("TheSushiRoles.Resources.Soul.png", 500f);
            return soulSprite;
        }

        private static Sprite question;
        public static Sprite GetQuestionSprite() 
        {
            if (question) return question;
            question = Utils.LoadSprite("TheSushiRoles.Resources.PsychicButton.png", 115f);
            return question;
        }

        public static void ClearAndReload() 
        {
            Player = null;
            target = null;
            soulTarget = null;
            deadBodies = new List<Tuple<DeadPlayer, Vector3>>();
            futureDeadBodies = new List<Tuple<DeadPlayer, Vector3>>();
            souls = new List<SpriteRenderer>();
            meetingStartTime = DateTime.UtcNow;
            Cooldown = CustomOptionHolder.PsychicCooldown.GetFloat();
            Duration = CustomOptionHolder.PsychicDuration.GetFloat();
            oneTimeUse = CustomOptionHolder.PsychicOneTimeUse.GetBool();
            chanceAdditionalInfo = CustomOptionHolder.PsychicChanceAdditionalInfo.GetSelection() / 10f;
        }

        public static string GetInfo(PlayerControl target, PlayerControl killer, DeadPlayer.CustomDeathReason DeathReason) 
        {
            string msg = "";

            List<SpecialPsychicInfo> infos = new List<SpecialPsychicInfo>();
            // collect fitting death info types.
            // suicides:
            if (killer == target) 
            {
                if ((target == Sheriff.Player) && DeathReason != DeadPlayer.CustomDeathReason.LoverSuicide) infos.Add(SpecialPsychicInfo.SheriffSuicide);
                if (target == Lovers.Lover1 || target == Lovers.Lover2) infos.Add(SpecialPsychicInfo.PassiveLoverSuicide);
                if (target == Warlock.Player && DeathReason != DeadPlayer.CustomDeathReason.LoverSuicide) infos.Add(SpecialPsychicInfo.WarlockSuicide);
            }
            else
            {
                if (target == Lovers.Lover1 || target == Lovers.Lover2) infos.Add(SpecialPsychicInfo.ActiveLoverDies);
                if (target.Data.Role.IsImpostor && killer.Data.Role.IsImpostor) infos.Add(SpecialPsychicInfo.ImpostorTeamkill);
            }
            if (target == Recruit.Player && (killer == Jackal.Player)) infos.Add(SpecialPsychicInfo.JackalKillsRecruit);
            if (target == Lawyer.Player && killer == Lawyer.target) infos.Add(SpecialPsychicInfo.LawyerKilledByClient);
            if (target == Romantic.Player && killer == Romantic.beloved) infos.Add(SpecialPsychicInfo.RomanticKilledByBeloved);
            if (Psychic.target.WasCleanedOrEaten) infos.Add(SpecialPsychicInfo.BodyCleaned);
            
            if (infos.Count > 0) 
            {
                var selectedInfo = infos[rnd.Next(infos.Count)];
                switch (selectedInfo) 
                {
                    case SpecialPsychicInfo.SheriffSuicide:
                        msg = "Yikes, that Sheriff shot backfired.";
                        break;
                    case SpecialPsychicInfo.WarlockSuicide:
                        msg = "MAYBE I cursed the person next to me and killed myself. Oops.";
                        break;
                    case SpecialPsychicInfo.ActiveLoverDies:
                        msg = "I wanted to get out of this toxic relationship anyways.";
                        break;
                    case SpecialPsychicInfo.RomanticKilledByBeloved:
                        msg = "Why would my own beloved murder me? It must've been a mistake...I hope!";
                        break;
                    case SpecialPsychicInfo.PassiveLoverSuicide:
                        msg = "The love of my life died, thus with a kiss I die.";
                        break;
                    case SpecialPsychicInfo.LawyerKilledByClient:
                        msg = "My client killed me. Do I still get paid?";
                        break;
                    case SpecialPsychicInfo.JackalKillsRecruit:
                        msg = "First they Recruited me, then they killed me. At least I don't need to do tasks anymore.";
                        break;
                    case SpecialPsychicInfo.ImpostorTeamkill:
                        msg = "I guess they confused me for the Spy, is there even one?";
                        break;
                    case SpecialPsychicInfo.BodyCleaned:
                        msg = "Is my dead body some kind of art now or... aaand it's gone.";
                        break;
                }
            }
            else
            {
                int randomNumber = rnd.Next(4);
                string typeOfColor = Utils.IsLighterColor(Psychic.target.GetKiller) ? "lighter" : "darker";
                float timeSinceDeath = (float)(Psychic.meetingStartTime - Psychic.target.DeathTime).TotalMilliseconds;
                var roleString = RoleInfo.GetRolesString(Psychic.target.player, false);
                if (randomNumber == 0)
                {
                    if (!roleString.Contains("Impostor") && !roleString.Contains("Crewmate"))
                        msg = "If my role hasn't been saved, there's no " + roleString + " in the game anymore.";
                    else
                        msg = "I was " + roleString + " without another role."; 
                }
                else if (randomNumber == 1) msg = "I'm not sure, but I guess a " + typeOfColor + " color killed me.";
                else if (randomNumber == 2) msg = "If I counted correctly, I died " + Math.Round(timeSinceDeath / 1000) + "s before the next meeting started.";
                else msg = "It seems like my killer is the " + RoleInfo.GetRolesString(Psychic.target.GetKiller, false) + ".";
            }

            if (rnd.NextDouble() < chanceAdditionalInfo) 
            {
                int count = 0;
                string condition = "";
                var alivePlayersList = PlayerControl.AllPlayerControls.ToArray().Where(pc => !pc.Data.IsDead);
                switch (rnd.Next(3)) 
                {
                    case 0:
                        count = alivePlayersList.Where(pc => pc.Data.Role.IsImpostor || pc.IsNeutralKiller() || new List<RoleInfo>() { RoleInfo.sheriff, RoleInfo.veteran}.Contains(RoleInfo.GetRoleInfoForPlayer(pc).FirstOrDefault())).Count();
                        condition = "killer" + (count == 1 ? "" : "s");
                        break;
                    case 1:
                        count = alivePlayersList.Where(Utils.IsVenter).Count();
                        condition = "player" + (count == 1 ? "" : "s") + " who can use vents";
                        break;
                    case 2:
                        count = alivePlayersList.Where(pc => Utils.IsPassiveNeutral(pc)).Count();
                        condition = "player" + (count == 1 ? "" : "s") + " who " + (count == 1 ? "is" : "are") + " a passive neutral role";
                        break;
                    case 3:
                        break;               
                }
                msg += $"\nWhen you asked, {count} " + condition + (count == 1 ? " was" : " were") + " still alive";
            }

            return Psychic.target.player.Data.PlayerName + "'s Soul:\n" + msg;
        }
    }
}