using System.Collections.Generic;
using UnityEngine;

namespace TheSushiRoles.Roles
{
    public static class Eraser 
    {
        public static PlayerControl Player;
        public static Color Color = Palette.ImpostorRed;
        public static List<byte> alreadyErased = new List<byte>();
        public static List<PlayerControl> futureErased = new List<PlayerControl>();
        public static PlayerControl CurrentTarget;
        public static float Cooldown = 25f;
        public static bool canEraseAnyone = false;
        private static Sprite ButtonSprite;
        public static Sprite GetButtonSprite() 
        {
            if (ButtonSprite) return ButtonSprite;
            ButtonSprite = Utils.LoadSprite("TheSushiRoles.Resources.EraseButton.png", 115f);
            return ButtonSprite;
        }

        public static void ClearAndReload() 
        {
            Player = null;
            futureErased = new List<PlayerControl>();
            CurrentTarget = null;
            Cooldown = CustomOptionHolder.eraserCooldown.GetFloat();
            canEraseAnyone = CustomOptionHolder.eraserCanEraseAnyone.GetBool();
            alreadyErased = new List<byte>();
        }
    }
}