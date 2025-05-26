using UnityEngine;

namespace TheSushiRoles.Roles
{
    public static class Undertaker
    {
        public static PlayerControl Player;
        public static float Cooldown;
        public static float DragSpeed;
        public static DeadBody CurrentTarget;
        public static Sprite ButtonSprite;
        public static Sprite GetFirstButtonSprite()
        {
            if (ButtonSprite) return ButtonSprite;
            ButtonSprite = Utils.LoadSprite("TheSushiRoles.Resources.Drag.png", 115f);
            return ButtonSprite;
        }
        public static Sprite ButtonSprite2;
        public static Sprite GetSecondButtonSprite()
        {
            if (ButtonSprite2) return ButtonSprite2;
            ButtonSprite2 = Utils.LoadSprite("TheSushiRoles.Resources.Drop.png", 115f);
            return ButtonSprite2;
        }
        public static void ClearAndReload()
        {
            Player = null;
            CurrentTarget = null;
            Cooldown = CustomOptionHolder.UndertakerCooldown.GetFloat();
            DragSpeed = CustomOptionHolder.UndertakerDragSpeed.GetFloat();
        }
    }
}