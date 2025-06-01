using System.Collections.Generic;
using UnityEngine;

namespace TheSushiRoles.Roles
{
    public static class Survivor 
    {        
        public static PlayerControl Player;
        public static PlayerControl target;
        public static Color Color = new Color32(255, 227, 105, byte.MaxValue);
        public static List<PlayerControl> blankedList = new List<PlayerControl>();
        public static int blanks = 0;
        public static Sprite blank;
        public static float Cooldown = 25f;
        public static int blanksNumber = 5;
        public static bool IsAdditionalAliveSurvivorWin = false;
        public static Sprite GetTargetSprite()
        {
            if (blank) return blank;
            blank = Utils.LoadSprite("TheSushiRoles.Resources.SurvivorButton.png", 115f);
            return blank;
        }

        public static void ClearAndReload() 
        {
            Player = null;
            target = null;
            blankedList = new List<PlayerControl>();
            blanks = 0;
            Cooldown = CustomOptionHolder.SurvivorCooldown.GetFloat();
            IsAdditionalAliveSurvivorWin = false;
            blanksNumber = Mathf.RoundToInt(CustomOptionHolder.SurvivorBlanksNumber.GetFloat());
        }
    }
}