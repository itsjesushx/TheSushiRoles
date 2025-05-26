using UnityEngine;

namespace TheSushiRoles.Roles
{
    public static class Miner
    {
        public static PlayerControl Player;
        public static float Cooldown;
        public static Color Color = Palette.ImpostorRed;
        public static float Delay;
        public static int MineVisibility;
        private static Sprite ButtonSprite;
        public static Sprite GetMineSprite() 
        {
            if (ButtonSprite) return ButtonSprite;
            ButtonSprite = Utils.LoadSprite("TheSushiRoles.Resources.MineButton.png", 115f);
            return ButtonSprite;
        }
        public static void ClearAndReload() 
        {
            Player = null;
            Cooldown = CustomOptionHolder.MinerCooldown.GetFloat();
            Delay = CustomOptionHolder.MineDelay.GetFloat();
            MineVisibility = CustomOptionHolder.MineVisible.GetSelection();
        }
    }
}