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
            TimeMaster.ClearAndReload();
            Medic.ClearAndReload();
            Portalmaker.ClearAndReload();
            Swapper.ClearAndReload();
            Tracker.ClearAndReload();
            Mystic.ClearAndReload();
            Hacker.ClearAndReload();
            Spy.ClearAndReload();
            Medium.ClearAndReload();
            Veteran.ClearAndReload();
            Oracle.ClearAndReload();
            Trapper.ClearAndReload();
            Crusader.ClearAndReload();

            // Neutral Roles
            Jester.ClearAndReload();
            Amnesiac.ClearAndReload();
            Vulture.ClearAndReload();
            Romantic.ClearAndReload();
            Arsonist.ClearAndReload();
            Lawyer.ClearAndReload();
            Prosecutor.ClearAndReload();
            Pursuer.ClearAndReload();

            // Neutral Killers
            Glitch.ClearAndReload();
            Juggernaut.ClearAndReload();
            VengefulRomantic.ClearAndReload();
            Jackal.ClearAndReload();
            Werewolf.ClearAndReload();
            Sidekick.ClearAndReload();
            Hitman.ClearAndReload();
            Predator.ClearAndReload();
            Agent.ClearAndReload();
            Plaguebearer.ClearAndReload();

            Pestilence.ClearAndReload();
        
            // Impostor Roles
            Blackmailer.ClearAndReload();
            Cleaner.ClearAndReload();
            Morphling.ClearAndReload();
            Poisoner.ClearAndReload();
            Camouflager.ClearAndReload();
            Eraser.ClearAndReload();
            Grenadier.ClearAndReload();
            Trickster.ClearAndReload();
            Miner.ClearAndReload();
            Undertaker.ClearAndReload();
            Warlock.ClearAndReload();
            Wraith.ClearAndReload();
            Witch.ClearAndReload();
            BountyHunter.ClearAndReload();
            Ninja.ClearAndReload();
            Yoyo.ClearAndReload();

            // Modifier
            Bait.ClearAndReload();
            Lazy.ClearAndReload();
            Tiebreaker.ClearAndReload();
            Sunglasses.ClearAndReload();
            Mini.ClearAndReload();
            Disperser.ClearAndReload();
            Vip.ClearAndReload();
            Giant.ClearAndReload();
            Invert.ClearAndReload();
            Chameleon.ClearAndReload();
            Armored.ClearAndReload();
            Lovers.ClearAndReload();
            Sleuth.ClearAndReload();

            // Abilities
            Guesser.ClearAndReload();
            Coward.ClearAndReload();
            Paranoid.ClearAndReload();

            // Other Clears and Reloads
            JackInTheBox.ClearJackInTheBoxes();
            NinjaTrace.ClearTraces();
            Silhouette.ClearSilhouettes();
            Portal.ClearPortals();
            Trap.ClearTraps();
            MinerVent.ClearMinerVents();
            Utils.ToggleZoom(reset : true);
            GameStartManagerPatch.GameStartManagerUpdatePatch.startingTimer = 0;
            SurveillanceMinigamePatch.nightVisionOverlays = null;
            MapBehaviourPatch.ClearAndReload();
            Modules.BetterMaps.BetterPolus.ClearAndReload();
            startTime = DateTime.UtcNow;
        }
    }
}
