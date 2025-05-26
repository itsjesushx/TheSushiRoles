using System;
using UnityEngine;

namespace TheSushiRoles.Roles.Modifiers
{
    public static class Mini 
    {
        public static PlayerControl Player;
        public static Color Color = new Color32(176, 161, 255, byte.MaxValue);
        public const float defaultColliderRadius = 0.2233912f;
        public const float defaultColliderOffset = 0.3636057f;
        public static Vector3 SizeFactor = new Vector3(0.4f, 0.4f, 1.0f);
        public static float Speed = 2f;
        public static void ClearAndReload()
        {
            Player = null;
            Speed = CustomOptionHolder.ModifierMiniSpeed.GetFloat();
        }
    }
}