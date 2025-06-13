using System.Collections.Generic;

namespace TheSushiRoles.Roles
{
    public static class Scavenger 
    {
        public static PlayerControl Player;
       
        public static Color Color = new Color32(139, 69, 19, byte.MaxValue);
        
        public static List<Arrow> localArrows = new();

        public static List<Vector3> DeadBodyPositions = new();

        public static int eatenBodies = 0;
        public static bool IsScavengerWin = false;
        public static float ScavengeTimer = 0f;
        public static void ClearAndReload()
        {
            Player = null;
            eatenBodies = 0;
            IsScavengerWin = false;
            ScavengeTimer = 0f;
            if (localArrows != null) 
            {
                foreach (Arrow arrow in localArrows)
                    if (arrow?.arrow != null)
                        UObject.Destroy(arrow.arrow);
            }
            DeadBodyPositions = new List<Vector3>();
        }
    }
}