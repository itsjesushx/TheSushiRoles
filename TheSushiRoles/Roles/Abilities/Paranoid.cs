using UnityEngine;

namespace TheSushiRoles.Roles.Abilities
{
    public static class Paranoid
    {
        public static PlayerControl Player;
        public static PlayerControl ClosestPlayer;
        public static Arrow Arrow = new(Color);
        public static Color Color = new Color32(234, 0, 255, byte.MaxValue);
        public static void ResetArrows()
        {
            ClosestPlayer = null;
            if (Arrow?.arrow != null) Object.Destroy(Arrow.arrow);
            Arrow = new Arrow(Color);
            if (Arrow.arrow != null) Arrow.arrow.SetActive(false);
        }
        public static void ClearAndReload()
        {
            ResetArrows();
            Player = null;
            ClosestPlayer = null;
        }
    }
}