using System.Collections.Generic;
using UnityEngine;

namespace TheSushiRoles.Roles.Abilities
{
    public static class Guesser 
    {
        private static Sprite targetSprite;
        public static bool hasMultipleShotsPerMeeting = false;
        public static bool killsThroughShield = true;
        public static bool evilGuesserCanGuessSpy = true;
        public static int tasksToUnlock = Mathf.RoundToInt(CustomOptionHolder.CrewGuesserNumberOfTasks.GetFloat());
        private static List<Guessers> guessers = new List<Guessers>();
        private static Color color = new Color32(255, 255, 0, byte.MaxValue);
        public static Sprite GetTargetSprite() 
        {
            if (targetSprite) return targetSprite;
            targetSprite = Utils.LoadSprite("TheSushiRoles.Resources.TargetIcon.png", 150f);
            return targetSprite;
        }

        public static bool IsGuesser(byte playerId) 
        {
            return guessers.FindAll(x => x.guesser.PlayerId == playerId).Count > 0;
        }

        public static void Clear(byte playerId) 
        {
            var g = guessers.FindLast(x => x.guesser.PlayerId == playerId);
            if (g == null) return;
            g.guesser = null;
            g.shots = Mathf.RoundToInt(CustomOptionHolder.GuesserNumberOfShots.GetFloat());
            g.tasksToUnlock = Mathf.RoundToInt(CustomOptionHolder.CrewGuesserNumberOfTasks.GetFloat());

            guessers.Remove(g);
        }

        public static int RemainingShots(byte playerId, bool shoot = false) 
        {
            var g = guessers.FindLast(x => x.guesser.PlayerId == playerId);
            if (g == null) return 0;
            if (shoot) g.shots--;
            return g.shots;
        }

        public static void ClearAndReload() 
        {
            guessers = new List<Guessers>();
            hasMultipleShotsPerMeeting = CustomOptionHolder.GuesserHasMultipleShotsPerMeeting.GetBool();
            killsThroughShield = CustomOptionHolder.GuesserKillsThroughShield.GetBool();
            evilGuesserCanGuessSpy = CustomOptionHolder.GuesserEvilCanKillSpy.GetBool();
            tasksToUnlock = Mathf.RoundToInt(CustomOptionHolder.CrewGuesserNumberOfTasks.GetFloat());
        }

        public class Guessers
        {
            public PlayerControl guesser;
            public int shots = Mathf.RoundToInt(CustomOptionHolder.GuesserNumberOfShots.GetFloat());
            public int tasksToUnlock = Mathf.RoundToInt(CustomOptionHolder.CrewGuesserNumberOfTasks.GetFloat());

            public Guessers(PlayerControl player) 
            {
                guesser = player;
                guessers.Add(this);
            }
        }
    }
}
