using System;

namespace TheSushiRoles
{
    [HarmonyPatch]
    public static class TheSushiRoles
    {
        public static Random rnd = new Random((int)DateTime.Now.Ticks);
        public static DateTime startTime = DateTime.UtcNow;
        public static void GlobalClearAndReload() 
        {
            // Crew Roles
            Mayor.ClearAndReload();
            Engineer.ClearAndReload();
            Sheriff.ClearAndReload();
            Lighter.ClearAndReload();
            Detective.ClearAndReload();
            Landlord.ClearAndReload();
            Monarch.ClearAndReload();
            Chronos.ClearAndReload();
            Medic.ClearAndReload();
            Gatekeeper.ClearAndReload();
            Swapper.ClearAndReload();
            Tracker.ClearAndReload();
            Mystic.ClearAndReload();
            Hacker.ClearAndReload();
            Spy.ClearAndReload();
            Psychic.ClearAndReload();
            Veteran.ClearAndReload();
            Oracle.ClearAndReload();
            Trapper.ClearAndReload();
            Crusader.ClearAndReload();

            // Neutral Roles
            Jester.ClearAndReload();
            Amnesiac.ClearAndReload();
            Scavenger.ClearAndReload();
            Romantic.ClearAndReload();
            Arsonist.ClearAndReload();
            Lawyer.ClearAndReload();
            Prosecutor.ClearAndReload();
            Survivor.ClearAndReload();

            // Neutral Killers
            Glitch.ClearAndReload();
            Juggernaut.ClearAndReload();
            VengefulRomantic.ClearAndReload();
            Jackal.ClearAndReload();
            Werewolf.ClearAndReload();
            Hitman.ClearAndReload();
            Predator.ClearAndReload();
            Agent.ClearAndReload();
            Plaguebearer.ClearAndReload();
            Pestilence.ClearAndReload();
        
            // Impostor Roles
            Blackmailer.ClearAndReload();
            Janitor.ClearAndReload();
            Morphling.ClearAndReload();
            Cultist.ClearAndReload();
            Viper.ClearAndReload();
            Painter.ClearAndReload();
            Eraser.ClearAndReload();
            Grenadier.ClearAndReload();
            Trickster.ClearAndReload();
            Miner.ClearAndReload();
            Undertaker.ClearAndReload();
            Warlock.ClearAndReload();
            Wraith.ClearAndReload();
            Witch.ClearAndReload();
            BountyHunter.ClearAndReload();
            Assassin.ClearAndReload();
            Yoyo.ClearAndReload();

            // Modifier
            Bait.ClearAndReload();
            Lazy.ClearAndReload();
            Tiebreaker.ClearAndReload();
            Blind.ClearAndReload();
            Mini.ClearAndReload();
            Disperser.ClearAndReload();
            Vip.ClearAndReload();
            Giant.ClearAndReload();
            Drunk.ClearAndReload();
            Chameleon.ClearAndReload();
            Lucky.ClearAndReload();
            Recruit.ClearAndReload();
            Lovers.ClearAndReload();
            Sleuth.ClearAndReload();

            // Abilities
            Guesser.ClearAndReload();
            Coward.ClearAndReload();
            Paranoid.ClearAndReload();

            // Other Clears and reloads
            JackInTheBox.ClearJackInTheBoxes();
            AssassinTrace.ClearTraces();
            Silhouette.ClearSilhouettes();
            Portal.ClearPortals();
            Trap.ClearTraps();
            BlindTrap.ClearTraps();
            MinerVent.ClearMinerVents();
            Utils.ToggleZoom(reset : true);
            GameStartManagerPatch.GameStartManagerUpdatePatch.startingTimer = 0;
            SurveillanceMinigamePatch.nightVisionOverlays = null;
            MapBehaviourPatch.ClearAndReload();
            //RoleInfo.RoleTexts.Clear();
            Modules.BetterMaps.BetterPolus.ClearAndReload();
            startTime = DateTime.UtcNow;
        }
    }
}
