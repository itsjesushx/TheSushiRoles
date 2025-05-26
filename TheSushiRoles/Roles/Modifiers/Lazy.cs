using System.Collections.Generic;
using UnityEngine;

namespace TheSushiRoles.Roles.Modifiers
{
    public static class Lazy
    {
        public static List<PlayerControl> Players = new List<PlayerControl>();
        public static Vector3 position;
        public static Color Color = new Color32(114, 136, 176, byte.MaxValue);
        public static void ClearAndReload() 
        {
            Players = new List<PlayerControl>();
            position = Vector3.zero;
        }
        public static void SetPosition() 
        {
            if (position == Vector3.zero) return;  // Check if this has been set, otherwise first spawn on submerged will fail
            if (Players.FindAll(x => x.PlayerId == PlayerControl.LocalPlayer.PlayerId).Count > 0) 
            {
                PlayerControl.LocalPlayer.NetTransform.RpcSnapTo(position);
                if (SubmergedCompatibility.IsSubmerged) 
                {
                    SubmergedCompatibility.ChangeFloor(position.y > -7);
                }
            }
        }
    }
}