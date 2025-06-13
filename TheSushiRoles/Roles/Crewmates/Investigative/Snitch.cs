using System.Collections.Generic;

namespace TheSushiRoles.Roles
{
    public static class Snitch
    {
        public static PlayerControl Player;
        public static Color Color = new Color32(184, 251, 79, byte.MaxValue);
        public static readonly List<Arrow> LocalArrows = new();
        public static bool Active = false;
        public static bool KnowsRealKiller = false;
        public static bool ShouldSee = false;
        public static void ResetArrows()
        {
            foreach (Arrow arrow in LocalArrows)
                UObject.Destroy(arrow.arrow);
            LocalArrows.Clear();

            Arrow arrow1 = new(Palette.ImpostorRed);
            arrow1.arrow.SetActive(false);
            Arrow arrow2 = new(Color);
            arrow2.arrow.SetActive(false);

            LocalArrows.Add(arrow1);
            LocalArrows.Add(arrow2);
        }
        public static PlayerControl Target;
        public static void ClearAndReload()
        {
            Player = null;
            ShouldSee = false;
            Active = false;
            KnowsRealKiller = false;
            ResetArrows();
            Target = null;
        }
    }
}