using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TheSushiRoles.Roles
{
    public static class Plaguebearer
    {
        public static PlayerControl Player;
        public static PlayerControl InfectTarget;
        public static Color Color = new Color32(200, 225, 150, byte.MaxValue);
        public static PlayerControl CurrentTarget;
        public static float Cooldown = 30f;
        public static List<byte> InfectedPlayers = new List<byte>();
        public static bool CanTransform()
        {
            var alivePlayerIds = PlayerControl.AllPlayerControls
                 .ToArray().Where(x => !x.Data.IsDead && !x.Data.Disconnected && x != Player) // Get all alive players except PB
                .Select(x => x.PlayerId) // Convert to Player IDs
                .ToList();

            return alivePlayerIds.All(playerId => InfectedPlayers.Contains(playerId)); // Compare IDs
        }
        private static Sprite ButtonSprite;
        public static Sprite GetButtonSprite() 
        {
            if (ButtonSprite) return ButtonSprite;
            ButtonSprite = Utils.LoadSpriteFromResources("TheSushiRoles.Resources.Infect.png", 115f);
            return ButtonSprite;
        }
        public static bool IsInfected(PlayerControl target) => InfectedPlayers.Contains(target.PlayerId);
        public static void ClearAndReload()
        {
            Player = null;
            Cooldown = CustomOptionHolder.PlaguebearerCooldown.GetFloat();
            CurrentTarget = null;
            InfectedPlayers = new List<byte>();
            InfectTarget = null;
        }
    }
}