using System.Collections.Generic;
using System.Linq;
namespace TheSushiRoles.Roles
{
    public static class Arsonist 
    {
        public static PlayerControl Player;
        
        public static Color Color = new Color32(238, 112, 46, byte.MaxValue);

        public static bool IsArsonistWin = false;

        public static PlayerControl CurrentTarget;
        public static PlayerControl douseTarget;
        public static List<PlayerControl> dousedPlayers = new List<PlayerControl>();

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
        }
    }
}