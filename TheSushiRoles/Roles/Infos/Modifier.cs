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

        public readonly static ModifierInfo lazy = new("Lazy", Lazy.Color, "You don't get teleported", "You don't get teleported to meetings", ModifierId.Lazy);
        public readonly static ModifierInfo tiebreaker = new("Tiebreaker", Tiebreaker.Color, "Your vote breaks the tie", "Break the tie", ModifierId.Tiebreaker);
        public readonly static ModifierInfo bait = new("Bait", Bait.Color, "Bait your enemies", "Bait your enemies", ModifierId.Bait);
        public readonly static ModifierInfo blind = new("Blind", Blind.Color, "You got the Blind", "Your vision is reduced", ModifierId.Blind);
        public readonly static ModifierInfo sleuth = new("Sleuth", Sleuth.Color, "Learn from your reports", "Get to know the role of who you report", ModifierId.Sleuth);
        public readonly static ModifierInfo lover = new("Lover", Lovers.Color, $"You are in love", "Stay alive until the end with your lover", ModifierId.Lover);
        public readonly static ModifierInfo mini = new("Mini", Mini.Color, "No one will harm you until you grow up", "No one will harm you", ModifierId.Mini);
        public readonly static ModifierInfo vip = new("VIP", Vip.Color, "You are the VIP", "Everyone is notified when you die", ModifierId.Vip);
        public readonly static ModifierInfo drunk = new("Drunk", Drunk.Color, "Your movement is inverted", "Your movement is inverted", ModifierId.Drunk);
        public readonly static ModifierInfo chameleon = new("Chameleon", Chameleon.Color, "You're hard to see when not moving", "You're hard to see when not moving", ModifierId.Chameleon);
        public readonly static ModifierInfo lucky = new("Lucky", Lucky.Color, "You are protected from one murder attempt", "You are protected from one murder attempt", ModifierId.Lucky);
        public readonly static ModifierInfo giant = new("Giant", Giant.Color, "You are bigger than anyone", "You are bigger than others", ModifierId.Giant);
        public readonly static ModifierInfo disperser = new("Disperser", Palette.ImpostorRed, "Disperse the Crew to random vents", "Disperse the Crew", ModifierId.Disperser);
        public readonly static ModifierInfo recruit = new("Recruit", Recruit.Color, "", "Help your Jackal to kill everyone", ModifierId.Recruit);
        public readonly static ModifierInfo follower = new("Follower", Palette.ImpostorRed, "", "Help the Cultist to kill everyone", ModifierId.Follower);

        // not used yet but might in the future
        public static List<ModifierInfo> allModifierInfos = new List<ModifierInfo>()
        {
            bait,
            blind,
            lazy,
            lucky,
            chameleon,
            disperser,
            follower,
            giant,
            drunk,
            lover,
            mini,
            recruit,
            sleuth,
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
                if (Blind.Players.Any(x => x.PlayerId == player.PlayerId)) infos.Add(blind);
                if (player == Mini.Player) infos.Add(mini);
                if (player == Follower.Player) infos.Add(follower);
                if (player == Disperser.Player) infos.Add(disperser);
                if (player == Recruit.Player) infos.Add(recruit);
                if (player == Giant.Player) infos.Add(giant);
                if (Drunk.Players.Any(x => x.PlayerId == player.PlayerId)) infos.Add(drunk);
                if (Chameleon.Players.Any(x => x.PlayerId == player.PlayerId)) infos.Add(chameleon);
                if (player == Lucky.Player) infos.Add(lucky);
            }
            return infos;
        }
        public static String GetModifiersString(PlayerControl player, bool useColors) 
        {
            string modifierName = String.Join(" ", GetModifierInfoForPlayer(player).Select(x => useColors ? Utils.ColorString(x.Color, x.Name) : x.Name).ToArray());
            
            if (modifierName == "") return "";
            var modifier = GetModifierInfoForPlayer(player).FirstOrDefault();
            
            if (useColors) modifierName = Utils.ColorString(modifier.Color, modifierName);
            
            return modifierName;
        }
    }
}