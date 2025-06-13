using System.Collections.Generic;
using System.Linq;

namespace TheSushiRoles.Roles
{
    public static class Plaguebearer
    {
        public static PlayerControl Player;
        public static PlayerControl InfectTarget;
        public static PlayerControl CurrentTarget;
        public static List<PlayerControl> InfectedPlayers = new List<PlayerControl>();

        public static Color Color = new Color32(200, 225, 150, byte.MaxValue);

        public static bool CanTransform()
        {
            var alivePlayerIds = PlayerControl.AllPlayerControls
                 .ToArray().Where(x => !x.Data.IsDead && !x.Data.Disconnected && x != Player).ToList();

            return alivePlayerIds.All(player => InfectedPlayers.Contains(player));
        }

        private static Sprite ButtonSprite;
        public static Sprite GetButtonSprite() 
        {
            if (ButtonSprite) return ButtonSprite;
            ButtonSprite = Utils.LoadSprite("TheSushiRoles.Resources.Infect.png", 115f);
            return ButtonSprite;
        }
        public static void ClearAndReload()
        {
            Player = null;
            CurrentTarget = null;
            InfectedPlayers = new List<PlayerControl>();
            InfectTarget = null;
        }
    }
}