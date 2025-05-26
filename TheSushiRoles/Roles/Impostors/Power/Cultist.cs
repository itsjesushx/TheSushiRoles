using System.Collections.Generic;
using UnityEngine;

namespace TheSushiRoles.Roles
{
    public static class Cultist
    {
        public static PlayerControl Player;
        public static PlayerControl CurrentTarget;
        public static float Cooldown = 25f;
        private static Sprite ButtonSprite;
        public static readonly List<Arrow> LocalArrows = new();
        public static bool HasFollower = false;
        public static bool FollowerGetsGuesser;
        public static Sprite GetButtonSprite()
        {
            if (ButtonSprite) return ButtonSprite;
            ButtonSprite = Utils.LoadSprite("TheSushiRoles.Resources.CultistButton.png", 115f);
            return ButtonSprite;
        }
        public static void ClearAndReload()
        {
            Player = null;
            CurrentTarget = null;
            Cooldown = CustomOptionHolder.CultistCooldown.GetFloat();
            FollowerGetsGuesser = CustomOptionHolder.FollowerGetsGuesser.GetBool();

            foreach (Arrow arrow in LocalArrows)
                Object.Destroy(arrow.arrow);

            LocalArrows.Clear();
            Arrow arrow1 = new(Palette.ImpostorRed);
            arrow1.arrow.SetActive(false);
            Arrow arrow2 = new(Palette.ImpostorRed);
            arrow2.arrow.SetActive(false);

            LocalArrows.Add(arrow1);
            LocalArrows.Add(arrow2);

            HasFollower = false;

            Follower.ClearAndReload();
        }

        public static RoleId SelectFollowerRole()
        {
            RoleId[] impostorRoles = new RoleId[]
            {
                RoleId.Assassin,
                RoleId.Blackmailer,
                RoleId.BountyHunter,
                RoleId.Painter,
                RoleId.Eraser,
                RoleId.Grenadier,
                RoleId.Janitor,
                RoleId.Miner,
                RoleId.Morphling,
                RoleId.Trickster,
                RoleId.Undertaker,
                RoleId.Viper,
                RoleId.Wraith,
                RoleId.Warlock,
                RoleId.Witch,
                RoleId.Yoyo
            };

            int randomIndex = Random.Range(0, impostorRoles.Length);
            return impostorRoles[randomIndex];
        }
    }
}