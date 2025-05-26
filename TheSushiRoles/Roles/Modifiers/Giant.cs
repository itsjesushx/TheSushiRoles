using UnityEngine;

namespace TheSushiRoles.Roles.Modifiers
{
    public static class Giant
    {
        public static PlayerControl Player;
        public static Vector3 SizeFactor = new Vector3(1.0f, 1.0f, 1.0f);
        public static Color Color = new Color32(255, 178, 77, byte.MaxValue);
        public static float Speed;
        public static void ClearAndReload()
        {
            Player = null;
            Speed = CustomOptionHolder.ModifierGiantSpeed.GetFloat();
        }
    }
}
