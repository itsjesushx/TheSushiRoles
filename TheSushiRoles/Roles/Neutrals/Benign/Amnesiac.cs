using System.Collections.Generic;
using UnityEngine;

namespace TheSushiRoles.Roles
{
    public static class Amnesiac
    {
        public static PlayerControl Player;
        public static PlayerControl ToRemember;
        public static readonly List<GameObject> Buttons = new List<GameObject>();
        public static readonly List<bool> ListOfActives = new List<bool>();
        private static Sprite ButtonSprite;
        private static Sprite ButtonSprite2;
        public static Sprite GetSelectActiveSprite()
        {
            if (ButtonSprite) return ButtonSprite;
            ButtonSprite = Utils.LoadSpriteFromResources("TheSushiRoles.Resources.SelecActive.png", 150f);
            return ButtonSprite;
        }
        public static Sprite GetSelectInActiveSprite()
        {
            if (ButtonSprite2) return ButtonSprite2;
            ButtonSprite2 = Utils.LoadSpriteFromResources("TheSushiRoles.Resources.SelecInActive.png", 150f);
            return ButtonSprite2;
        }
        public static List<RoleId> RolesToRemember = new List<RoleId>
        {
            RoleId.Jester, RoleId.Mayor, RoleId.Portalmaker, RoleId.Grenadier, RoleId.Oracle, RoleId.Miner,
            RoleId.Engineer, RoleId.Sheriff, RoleId.Juggernaut, RoleId.Glitch, RoleId.Lighter, RoleId.Agent,
            RoleId.Hitman, RoleId.Veteran, RoleId.Detective, RoleId.Predator, RoleId.TimeMaster, RoleId.Medic,
            RoleId.Romantic, RoleId.Swapper, RoleId.Mystic, RoleId.Morphling, RoleId.Camouflager, RoleId.Hacker,
            RoleId.Crusader, RoleId.Tracker, RoleId.Poisoner, RoleId.Jackal, RoleId.Sidekick, RoleId.Eraser,
            RoleId.Spy, RoleId.VengefulRomantic, RoleId.Trickster, RoleId.Cleaner, RoleId.Blackmailer, RoleId.Warlock,
            RoleId.Werewolf, RoleId.Vigilante, RoleId.Arsonist, RoleId.BountyHunter, RoleId.Wraith, RoleId.Vulture,
            RoleId.Medium, RoleId.Pestilence, RoleId.Plaguebearer, RoleId.Trapper, RoleId.Undertaker, RoleId.Lawyer,
            RoleId.Prosecutor, RoleId.Pursuer, RoleId.Witch, RoleId.Ninja, RoleId.Yoyo, RoleId.Amnesiac,
            RoleId.Crewmate, RoleId.Impostor
        };
        public static Color Color = new Color32(34, 255, 255, byte.MaxValue);
        public static bool Remembered;
        public static void ClearAndReload()
        {
            Player = null;
            ToRemember = null;
            Remembered = false;
        }
    }
}