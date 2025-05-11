using System.Collections.Generic;
using UnityEngine;

namespace TheSushiRoles.Roles
{
    public static class Witch 
    {
        public static PlayerControl Player;
        public static Color Color = Palette.ImpostorRed;

        public static List<PlayerControl> futureSpelled = new List<PlayerControl>();
        public static PlayerControl CurrentTarget;
        public static PlayerControl spellCastingTarget;
        public static float Cooldown = 30f;
        public static float spellCastingDuration = 2f;
        public static float CooldownAddition = 10f;
        public static float currentCooldownAddition = 0f;
        public static bool canSpellAnyone = false;
        public static bool triggerBothCooldowns = true;
        public static bool witchVoteSavesTargets = true;

        private static Sprite ButtonSprite;
        public static Sprite GetButtonSprite() 
        {
            if (ButtonSprite) return ButtonSprite;
            ButtonSprite = Utils.LoadSpriteFromResources("TheSushiRoles.Resources.SpellButton.png", 115f);
            return ButtonSprite;
        }

        private static Sprite spelledOverlaySprite;
        public static Sprite GetSpelledOverlaySprite() 
        {
            if (spelledOverlaySprite) return spelledOverlaySprite;
            spelledOverlaySprite = Utils.LoadSpriteFromResources("TheSushiRoles.Resources.SpellButtonMeeting.png", 225f);
            return spelledOverlaySprite;
        }


        public static void ClearAndReload() 
        {
            Player = null;
            futureSpelled = new List<PlayerControl>();
            CurrentTarget = spellCastingTarget = null;
            Cooldown = CustomOptionHolder.witchCooldown.GetFloat();
            CooldownAddition = CustomOptionHolder.witchAdditionalCooldown.GetFloat();
            currentCooldownAddition = 0f;
            canSpellAnyone = CustomOptionHolder.witchCanSpellAnyone.GetBool();
            spellCastingDuration = CustomOptionHolder.witchSpellCastingDuration.GetFloat();
            triggerBothCooldowns = CustomOptionHolder.witchTriggerBothCooldowns.GetBool();
            witchVoteSavesTargets = CustomOptionHolder.witchVoteSavesTargets.GetBool();
        }
    }
}