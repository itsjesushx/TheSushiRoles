using System.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace TheSushiRoles.Roles.AbilityInfo
{
    public class AbilityInfo 
    {
        public Color Color;
        public string Name;
        public string Description;
        public AbilityId AbilityId;
        public AbilityInfo(string name, Color Color, string Description, AbilityId AbilityId)
        {
            this.Color = Color;
            this.Name = name;
            this.Description = Description;
            this.AbilityId = AbilityId;
        }

        public static readonly AbilityInfo coward = new("Coward", Coward.Color, "Call a meeting from anywhere!", AbilityId.Coward);
        public readonly static AbilityInfo paranoid = new("Paranoid", Paranoid.Color, "Know when someone is close to you", AbilityId.Paranoid);
        public readonly static AbilityInfo flashlight = new("FlashLight", FlashLight.Color, "Hide N' Seek Vision!", AbilityId.Lighter);
        
        // not used yet but might in the future
        public static List<AbilityInfo> allAbilityInfos = new List<AbilityInfo>()
        {
            coward,
            flashlight,
            paranoid
        };
        public static List<AbilityInfo> GetAbilityInfoForPlayer(PlayerControl player)
        {
            List<AbilityInfo> infos = new List<AbilityInfo>();
            if (player == null) return infos;

            if (player == Coward.Player) infos.Add(coward);
            if (player == Paranoid.Player) infos.Add(paranoid);
            if (player == FlashLight.Player) infos.Add(flashlight);

            return infos;
        }
        public static String GetAbilitiesString(PlayerControl player, bool useColors) 
        {
            string abilityName = String.Join(" ", GetAbilityInfoForPlayer(player).Select(x => useColors ? Utils.ColorString(x.Color, x.Name) : x.Name).ToArray());
            
            if (abilityName == "") return "";
            
            if (useColors) abilityName = Utils.ColorString(Color.yellow, abilityName);
            
            return abilityName;
        }
    }
}