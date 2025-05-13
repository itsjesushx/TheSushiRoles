using System;
using UnityEngine;

namespace TheSushiRoles.Roles.Modifiers
{
    public static class Giant
    {
        public static PlayerControl Player;
        public static Vector3 SizeFactor = new Vector3(1.0f, 1.0f, 1.0f);
        public static void ClearAndReload()
        {
            Player = null;
        }
    }
}
