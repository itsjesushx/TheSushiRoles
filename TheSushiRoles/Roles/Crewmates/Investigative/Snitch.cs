using System.Collections.Generic;
using UnityEngine;

namespace TheSushiRoles.Roles
{
    public static class Snitch
    {
        public static PlayerControl Player;
        public static Color Color = new Color32(184, 251, 79, byte.MaxValue);
        public static readonly List<Arrow> LocalArrows = new();
        public static float Cooldown = 25f;
        public static float Duration = 5f;
        public static bool SeesInMeetings = false;
        public static bool Active = false;
        public static float Accuracy;
        public static bool KnowsRealKiller = false;
        public static bool ShouldSee = false;
        public static void ResetArrows()
        {
            foreach (Arrow arrow in LocalArrows)
                Object.Destroy(arrow.arrow);
            LocalArrows.Clear();

            Arrow arrow1 = new(Palette.ImpostorRed);
            arrow1.arrow.SetActive(false);
            Arrow arrow2 = new(Color);
            arrow2.arrow.SetActive(false);

            LocalArrows.Add(arrow1);
            LocalArrows.Add(arrow2);
        }
        public static bool ShouldHaveButton = false;
        public static PlayerControl Target;
        private static Sprite ButtonSprite;
        public static Sprite GetButtonSprite()
        {
            if (ButtonSprite) return ButtonSprite;
            ButtonSprite = Utils.LoadSprite("TheSushiRoles.Resources.SnitchButton.png", 115f);
            return ButtonSprite;
        }
        public static void ClearAndReload()
        {
            Player = null;
            ShouldSee = false;
            Active = false;
            KnowsRealKiller = false;
            ShouldHaveButton = false;
            ResetArrows();
            Target = null;
            Cooldown = CustomOptionHolder.SnitchCooldown.GetFloat();
            Accuracy = CustomOptionHolder.SnitchAccuracy.GetFloat();
            SeesInMeetings = CustomOptionHolder.SnitchSeesInMeetings.GetBool();
        }
    }
}