using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TheSushiRoles.Roles
{
    public static class Arsonist 
    {
        public static PlayerControl Player;
        public static Color Color = new Color32(238, 112, 46, byte.MaxValue);

        public static float Cooldown = 30f;
        public static float Duration = 3f;
        public static bool IsArsonistWin = false;

        public static PlayerControl CurrentTarget;
        public static PlayerControl douseTarget;
        public static List<PlayerControl> dousedPlayers = new List<PlayerControl>();

        private static Sprite douseSprite;
        public static Sprite GetDouseSprite() 
        {
            if (douseSprite) return douseSprite;
            douseSprite = Utils.LoadSpriteFromResources("TheSushiRoles.Resources.DouseButton.png", 115f);
            return douseSprite;
        }

        private static Sprite igniteSprite;
        public static Sprite GetIgniteSprite() 
        {
            if (igniteSprite) return igniteSprite;
            igniteSprite = Utils.LoadSpriteFromResources("TheSushiRoles.Resources.IgniteButton.png", 115f);
            return igniteSprite;
        }

        public static bool DousedEveryoneAlive() 
        {
            return PlayerControl.AllPlayerControls.ToArray().All(x => { return x == Arsonist.Player || x.Data.IsDead || x.Data.Disconnected || Arsonist.dousedPlayers.Any(y => y.PlayerId == x.PlayerId); });
        }

        public static void ClearAndReload() 
        {
            Player = null;
            CurrentTarget = null;
            douseTarget = null; 
            IsArsonistWin = false;
            dousedPlayers = new List<PlayerControl>();
            foreach (PoolablePlayer p in MapOptions.BeanIcons.Values) 
            {
                if (p != null && p.gameObject != null) p.gameObject.SetActive(false);
            }
            Cooldown = CustomOptionHolder.arsonistCooldown.GetFloat();
            Duration = CustomOptionHolder.arsonistDuration.GetFloat();
        }
    }
}