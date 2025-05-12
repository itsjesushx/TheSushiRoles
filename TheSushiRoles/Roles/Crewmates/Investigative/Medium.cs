using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TheSushiRoles.Roles
{
    public static class Medium 
    {
        public static PlayerControl medium;
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
        enum SpecialMediumInfo 
        {
            SheriffSuicide,
            ActiveLoverDies,
            PassiveLoverSuicide,
            LawyerKilledByClient,
            JackalKillsSidekick,
            ImpostorTeamkill,
            SubmergedO2,
            WarlockSuicide,
            BodyCleaned,
        }

        public static Sprite GetSoulSprite() 
        {
            if (soulSprite) return soulSprite;
            soulSprite = Utils.LoadSpriteFromResources("TheSushiRoles.Resources.Soul.png", 500f);
            return soulSprite;
        }

        private static Sprite question;
        public static Sprite GetQuestionSprite() 
        {
            if (question) return question;
            question = Utils.LoadSpriteFromResources("TheSushiRoles.Resources.MediumButton.png", 115f);
            return question;
        }

        public static void ClearAndReload() 
        {
            medium = null;
            target = null;
            soulTarget = null;
            deadBodies = new List<Tuple<DeadPlayer, Vector3>>();
            futureDeadBodies = new List<Tuple<DeadPlayer, Vector3>>();
            souls = new List<SpriteRenderer>();
            meetingStartTime = DateTime.UtcNow;
            Cooldown = CustomOptionHolder.mediumCooldown.GetFloat();
            Duration = CustomOptionHolder.mediumDuration.GetFloat();
            oneTimeUse = CustomOptionHolder.mediumOneTimeUse.GetBool();
            chanceAdditionalInfo = CustomOptionHolder.mediumChanceAdditionalInfo.GetSelection() / 10f;
        }

        public static string GetInfo(PlayerControl target, PlayerControl killer, DeadPlayer.CustomDeathReason DeathReason) 
        {
            string msg = "";

            List<SpecialMediumInfo> infos = new List<SpecialMediumInfo>();
            // collect fitting death info types.
            // suicides:
            if (killer == target) 
            {
                if ((target == Sheriff.Player) && DeathReason != DeadPlayer.CustomDeathReason.LoverSuicide) infos.Add(SpecialMediumInfo.SheriffSuicide);
                if (target == Lovers.Lover1 || target == Lovers.Lover2) infos.Add(SpecialMediumInfo.PassiveLoverSuicide);
                if (target == Warlock.Player && DeathReason != DeadPlayer.CustomDeathReason.LoverSuicide) infos.Add(SpecialMediumInfo.WarlockSuicide);
            } 
            else 
            {
                if (target == Lovers.Lover1 || target == Lovers.Lover2) infos.Add(SpecialMediumInfo.ActiveLoverDies);
                if (target.Data.Role.IsImpostor && killer.Data.Role.IsImpostor) infos.Add(SpecialMediumInfo.ImpostorTeamkill);
            }
            if (target == Sidekick.Player && (killer == Jackal.Player || Jackal.formerJackals.Any(x => x.PlayerId == killer.PlayerId))) infos.Add(SpecialMediumInfo.JackalKillsSidekick);
            if (target == Lawyer.Player && killer == Lawyer.target) infos.Add(SpecialMediumInfo.LawyerKilledByClient);
            if (Medium.target.WasCleanedOrEaten) infos.Add(SpecialMediumInfo.BodyCleaned);
            
            if (infos.Count > 0) 
            {
                var selectedInfo = infos[rnd.Next(infos.Count)];
                switch (selectedInfo) 
                {
                    case SpecialMediumInfo.SheriffSuicide:
                        msg = "Yikes, that Sheriff shot backfired.";
                        break;
                    case SpecialMediumInfo.WarlockSuicide:
                        msg = "MAYBE I cursed the person next to me and killed myself. Oops.";
                        break;
                    case SpecialMediumInfo.ActiveLoverDies:
                        msg = "I wanted to get out of this toxic relationship anyways.";
                        break;
                    case SpecialMediumInfo.PassiveLoverSuicide:
                        msg = "The love of my life died, thus with a kiss I die.";
                        break;
                    case SpecialMediumInfo.LawyerKilledByClient:
                        msg = "My client killed me. Do I still get paid?";
                        break;
                    case SpecialMediumInfo.JackalKillsSidekick:
                        msg = "First they sidekicked me, then they killed me. At least I don't need to do tasks anymore.";
                        break;
                    case SpecialMediumInfo.ImpostorTeamkill:
                        msg = "I guess they confused me for the Spy, is there even one?";
                        break;
                    case SpecialMediumInfo.BodyCleaned:
                        msg = "Is my dead body some kind of art now or... aaand it's gone.";
                        break;
                }
            }
            else 
            {
                int randomNumber = rnd.Next(4);
                string typeOfColor = Utils.IsLighterColor(Medium.target.GetKiller) ? "lighter" : "darker";
                float timeSinceDeath = ((float)(Medium.meetingStartTime - Medium.target.DeathTime).TotalMilliseconds);
                var roleString = RoleInfo.GetRolesString(Medium.target.player, false);
                if (randomNumber == 0) {
                    if (!roleString.Contains("Impostor") && !roleString.Contains("Crewmate"))
                        msg = "If my role hasn't been saved, there's no " + roleString + " in the game anymore.";
                    else
                        msg = "I was a " + roleString + " without another role."; 
                } else if (randomNumber == 1) msg = "I'm not sure, but I guess a " + typeOfColor + " color killed me.";
                else if (randomNumber == 2) msg = "If I counted correctly, I died " + Math.Round(timeSinceDeath / 1000) + "s before the next meeting started.";
                else msg = "It seems like my killer is the " + RoleInfo.GetRolesString(Medium.target.GetKiller, false) + ".";
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

            return Medium.target.player.Data.PlayerName + "'s Soul:\n" + msg;
        }
    }
}