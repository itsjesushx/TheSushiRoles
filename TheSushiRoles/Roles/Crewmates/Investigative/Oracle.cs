using System.Collections.Generic;
using System.Linq;

namespace TheSushiRoles.Roles
{
    public static class Oracle
    {
        public static Color Color = new Color32(228, 116, 148, byte.MaxValue);
        public static PlayerControl Player;
        public static PlayerControl Confessor;
        public static PlayerControl CurrentTarget;

        public static Faction RevealedFaction;

        public static bool Investigated; 
        public static int Charges;
        public static int RechargeTasksNumber;
        public static int RechargedTasks;
        public static string GetInfo(PlayerControl target)
        {
            string msg = "";

            var neutralEvilRoles = new List<PlayerControl> { Jester.Player, Scavenger.Player, Arsonist.Player, Prosecutor.Player };

            var neutralBenignRoles = new List<PlayerControl> { Romantic.Player, Lawyer.Player, Amnesiac.Player };

            var evilPlayers = PlayerControl.AllPlayerControls.ToArray().Where(x => !x.Data.IsDead && !x.Data.Disconnected && x.IsImpostor() 
                || x.IsNeutralKiller() || (neutralEvilRoles.Contains(x) && CustomGameOptions.NeutralEvilShowsEvil)
                || (neutralBenignRoles.Contains(x) && CustomGameOptions.NeutralBenignShowsEvil)).ToList();

            var allPlayers = PlayerControl.AllPlayerControls.ToArray().Where(x => !x.Data.IsDead && !x.Data.Disconnected && x != Oracle.Player && x != target).ToList();

            if (target.Data.IsDead || target.Data.Disconnected)
            {
                msg = Utils.ColorString(Color.red, "Your confessor failed to survive so you received no confession");
            }

            else if (allPlayers.Count < 2)
            {
                msg = "Too few people alive to receive a confessional";
            }
                
            if (evilPlayers.Count == 0)
            {
                msg = $"{target.Data.PlayerName} " + Utils.ColorString(Palette.CrewmateBlue, "confesses to knowing that there are no more evil players!"); 
            }

            allPlayers.Shuffle();
            evilPlayers.Shuffle();

            var secondPlayer = allPlayers[0];
            var firstTwoEvil = false;

            foreach (var evilPlayer in evilPlayers)
            {
                if (evilPlayer == target || evilPlayer == secondPlayer) firstTwoEvil = true;
            }
                
            if (firstTwoEvil)
            {
                var thirdPlayer = allPlayers[1];
                msg = $"{target.Data.PlayerName} confesses to knowing that they, {secondPlayer.Data.PlayerName} and/or {thirdPlayer.Data.PlayerName} is evil!";
            }
            else
            {
                var thirdPlayer = evilPlayers[0];
                msg =  $"{target.Data.PlayerName} confesses to knowing that they, {secondPlayer.Data.PlayerName} and/or {thirdPlayer.Data.PlayerName} is evil!";
            }

            return msg;
        }
        public static void ClearAndReload()
        {
            Player = null;
            Confessor = null;
            RevealedFaction = Faction.Other;
            CurrentTarget = null;
            Investigated = false;
            Charges = CustomGameOptions.OracleCharges;
            RechargeTasksNumber = CustomGameOptions.OracleRechargeTasksNumber;
            RechargedTasks = CustomGameOptions.OracleRechargeTasksNumber;
        }
    }
}