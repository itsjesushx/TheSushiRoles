using System.Collections.Generic;
using UnityEngine;

namespace TheSushiRoles.Roles.Abilities
{
    public static class Paranoid
    {
        public static PlayerControl Player;
        public static PlayerControl ClosestPlayer;
        public static Arrow Arrow = new(Color.yellow);
        public static void ResetArrows() 
        {
            ClosestPlayer = null;
            if (Arrow?.arrow != null) UnityEngine.Object.Destroy(Arrow.arrow);
            Arrow = new Arrow(Color.yellow);
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