using System.Collections.Generic;

namespace TheSushiRoles.Roles.Abilities
{
    public static class Guesser 
    {
        private static Sprite targetSprite;

        public static List<GameObject> GuessButtons = new List<GameObject>();
        private static List<Guessers> guessers = new List<Guessers>();
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
            g.shots = CustomGameOptions.GuesserNumberOfShots;
            g.tasksToUnlock = CustomGameOptions.CrewGuesserNumberOfTasks;

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
        }

        public class Guessers
        {
            public PlayerControl guesser;
            public int shots = CustomGameOptions.GuesserNumberOfShots;
            public int tasksToUnlock = CustomGameOptions.CrewGuesserNumberOfTasks;

            public Guessers(PlayerControl player) 
            {
                guesser = player;
                guessers.Add(this);
            }
        }
    }
}
