using UnityEngine;

namespace TheSushiRoles.Roles
{
    public static class Spy 
    {
        public static PlayerControl Player;
        public static Color Color = Palette.ImpostorRed;
        public static bool impostorsCanKillAnyone = true;
        public static bool canEnterVents = false;
        public static bool hasImpostorVision = false;

        public static void ClearAndReload() 
        {
            Player = null;
            impostorsCanKillAnyone = CustomOptionHolder.spyImpostorsCanKillAnyone.GetBool();
            canEnterVents = CustomOptionHolder.spyCanEnterVents.GetBool();
            hasImpostorVision = CustomOptionHolder.spyHasImpostorVision.GetBool();
        }
    }
}