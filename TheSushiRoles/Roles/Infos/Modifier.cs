using System.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace TheSushiRoles.Roles.ModifierInfo
{
    public class ModifierInfo 
    {
        public Color Color;
        public string Name;
        public string IntroDescription;
        public string ShortDescription;
        public ModifierId ModifierId;
        public ModifierInfo(string name, Color Color, string IntroDescription, string ShortDescription, ModifierId ModifierId)
        {
            this.Color = Color;
            this.Name = name;
            this.IntroDescription = IntroDescription;
            this.ShortDescription = ShortDescription;
            this.ModifierId = ModifierId;
        }

        #region Modifiers
        public readonly static ModifierInfo lazy = new("Lazy", Color.yellow, "You don't get teleported", "You don't get teleported to meetings", ModifierId.Lazy);
        public readonly static ModifierInfo tiebreaker = new("Tiebreaker", Color.yellow, "Your vote breaks the tie", "Break the tie", ModifierId.Tiebreaker);
        public readonly static ModifierInfo bait = new("Bait", Color.yellow, "Bait your enemies", "Bait your enemies", ModifierId.Bait);
        public readonly static ModifierInfo sunglasses = new("Sunglasses", Color.yellow, "You got the sunglasses", "Your vision is reduced", ModifierId.Sunglasses);
        public readonly static ModifierInfo sleuth = new("Sleuth", Color.yellow, "Learn from your reports", "Get to know the role of who you report", ModifierId.Sleuth);
        public readonly static ModifierInfo lover = new("Lover", Lovers.Color, $"You are in love", "Stay alive until the end with your lover", ModifierId.Lover);
        public readonly static ModifierInfo mini = new("Mini", Color.yellow, "No one will harm you until you grow up", "No one will harm you", ModifierId.Mini);
        public readonly static ModifierInfo vip = new("VIP", Color.yellow, "You are the VIP", "Everyone is notified when you die", ModifierId.Vip);
        public readonly static ModifierInfo invert = new("Invert", Color.yellow, "Your movement is inverted", "Your movement is inverted", ModifierId.Invert);
        public readonly static ModifierInfo chameleon = new("Chameleon", Color.yellow, "You're hard to see when not moving", "You're hard to see when not moving", ModifierId.Chameleon);
        public readonly static ModifierInfo armored = new("Armored", Color.yellow, "You are protected from one murder attempt", "You are protected from one murder attempt", ModifierId.Armored);
        public readonly static ModifierInfo giant = new("Giant", Color.yellow, "You are bigger than anyone", "You are bigger than others", ModifierId.Giant);
        public readonly static ModifierInfo disperser = new("Disperser", Color.yellow, "Disperse the Crew to random vents", "Disperse the Crew", ModifierId.Disperser);
        public readonly static ModifierInfo sidekick = new("Sidekick", Sidekick.Color, "Help your Jackal to kill everyone", "Help your Jackal to kill everyone", ModifierId.Sidekick);
    
        #endregion

        // not used yet but might in the future
        public static List<ModifierInfo> allModifierInfos = new List<ModifierInfo>()
        {
            lazy,
            armored,
            bait,
            chameleon,
            disperser,
            giant,
            invert,
            lover,
            mini,
            sidekick,
            sleuth,
            sunglasses,
            tiebreaker,
            vip
        };
        public static List<ModifierInfo> GetModifierInfoForPlayer(PlayerControl player, bool showModifier = true)
        {
            List<ModifierInfo> infos = new List<ModifierInfo>();
            if (player == null) return infos;

            if (showModifier)
            {
                // Modifier
                if (!CustomOptionHolder.modifiersAreHidden.GetBool() || PlayerControl.LocalPlayer.Data.IsDead || AmongUsClient.Instance.GameState == InnerNet.InnerNetClient.GameStates.Ended)
                {
                    if (Bait.Players.Any(x => x.PlayerId == player.PlayerId)) infos.Add(bait);
                    if (Vip.Players.Any(x => x.PlayerId == player.PlayerId)) infos.Add(vip);
                }
                if (player == Lovers.Lover1 || player == Lovers.Lover2) infos.Add(lover);
                if (player == Tiebreaker.Player) infos.Add(tiebreaker);
                if (Lazy.Players.Any(x => x.PlayerId == player.PlayerId)) infos.Add(lazy);
                if (Sleuth.Players.Any(x => x.PlayerId == player.PlayerId)) infos.Add(sleuth);
                if (Sunglasses.Players.Any(x => x.PlayerId == player.PlayerId)) infos.Add(sunglasses);
                if (player == Mini.Player) infos.Add(mini);
                if (player == Disperser.Player) infos.Add(disperser);
                if (player == Sidekick.Player) infos.Add(sidekick);
                if (player == Giant.Player) infos.Add(giant);
                if (Invert.Players.Any(x => x.PlayerId == player.PlayerId)) infos.Add(invert);
                if (Chameleon.Players.Any(x => x.PlayerId == player.PlayerId)) infos.Add(chameleon);
                if (player == Armored.Player) infos.Add(armored);
            }
            return infos;
        }
        public static String GetModifiersString(PlayerControl player, bool useColors) 
        {
            string modifierName = String.Join(" ", GetModifierInfoForPlayer(player).Select(x => useColors ? Utils.ColorString(x.Color, x.Name) : x.Name).ToArray());
            
            if (modifierName == "") return "";
            
            if (useColors) modifierName = Utils.ColorString(Color.yellow, modifierName);
            
            return modifierName;
        }
    }
}