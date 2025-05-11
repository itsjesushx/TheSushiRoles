using UnityEngine;

namespace TheSushiRoles.Roles
{
    public static class Agent
    {
        public static PlayerControl Player;
        public static bool CanUseVents;
        public static Color Color = new Color32(0, 0, 255, byte.MaxValue);
        public static void ClearAndReload()
        {
            Player = null;
            CanUseVents = CustomOptionHolder.AgentCanUseVents.GetBool();
        }
    }
}