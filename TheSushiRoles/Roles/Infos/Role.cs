using System.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace TheSushiRoles.Roles
{
    public class RoleInfo 
    {
        public static Dictionary<byte, List<RoleInfo>> RoleHistory = new();
        public Color Color;
        public string Name;
        public string IntroDescription;
        public string ShortDescription;
        public string RoleDescription;
        public RoleId RoleId;
        public RoleAlignment Alignment;
        public Factions FactionId;
        /*public bool isImpostor => Color == Palette.ImpostorRed && !(RoleId == RoleId.Spy);
        public static Dictionary<RoleId, RoleInfo> RoleInfoById = new();*/
        public RoleInfo(string name, Color Color, string IntroDescription, string ShortDescription, RoleId RoleId, Factions FactionId, RoleAlignment Alignment, string RoleDescription)
        {
            this.Color = Color;
            this.Name = name;
            this.IntroDescription = IntroDescription;
            this.ShortDescription = ShortDescription;
            this.RoleId = RoleId;
            this.FactionId = FactionId;
            this.Alignment = Alignment;
            this.RoleDescription = RoleDescription;
            //RoleInfoById.TryAdd(RoleId, this);
        }
        public static void AddToRoleHistory(byte playerId, RoleInfo role)
        {
            if (!RoleHistory.ContainsKey(playerId))
                RoleHistory[playerId] = new List<RoleInfo>();

            if (!RoleHistory[playerId].Contains(role))
                RoleHistory[playerId].Add(role);
        }

        #region Neutral passives
        public readonly static RoleInfo jester = new("Jester", Jester.Color, "Get voted out", "Get voted out", RoleId.Jester, Factions.Neutral, RoleAlignment.NeutralEvil, "As a Jester, your job is to get voted out by any means to win, if you don't get voted out, you lose.");
        public readonly static RoleInfo arsonist = new("Arsonist", Arsonist.Color, "Let them burn", "Let them burn", RoleId.Arsonist, Factions.Neutral, RoleAlignment.NeutralEvil, "The Arsonist needs to spread gasoline on every single player to win.");
        public readonly static RoleInfo vulture = new("Vulture", Vulture.Color, "Eat corpses to win", "Eat dead bodies", RoleId.Vulture, Factions.Neutral, RoleAlignment.NeutralEvil, $"The goal of the Vulture is to eat {Vulture.vultureNumberToWin - Vulture.eatenBodies} dead bodies to win.");
        public readonly static RoleInfo lawyer = new("Lawyer", Lawyer.Color, "Defend your client", "Defend your client", RoleId.Lawyer, Factions.Neutral, RoleAlignment.NeutralBenign, "The Lawyer's duty is to prevent their client from getting ejected, if they get voted out, you suicide, if they are killed, you become a Pursuer.");
        public readonly static RoleInfo amnesiac = new("Amnesiac", Amnesiac.Color, "Gain an identity from a corpse", "Find a dead body to remember a role", RoleId.Amnesiac, Factions.Neutral, RoleAlignment.NeutralBenign, "The Amnesiac has to find a dead body in order to gain a role as they are roleless at the begining. Once you find a body you can remember who you were and you will get the corpse's role. You may have arrows pointing to bodies depending on settings.");
        public readonly static RoleInfo prosecutor = new("Prosecutor", Lawyer.Color, "Vote out your target", "Vote out your target", RoleId.Prosecutor, Factions.Neutral, RoleAlignment.NeutralEvil, "The Prosecutor is the opposite of a Lawyer, they are also given a target, but instead of protecting them, you have to make them look guilty. If they are voted out you win. If they die you become a Pursuer.");
        public readonly static RoleInfo pursuer = new("Pursuer", Pursuer.Color, "Blank the Impostors", "Blank the Impostors", RoleId.Pursuer, Factions.Neutral, RoleAlignment.NeutralBenign, "As the Pursuer you were either a Lawyer, Romantic or Prosecutor which target die. All you have to do is stay alive to win, you may also blank players to prevent them from killing.");
        public readonly static RoleInfo romantic = new("Romantic", Romantic.Color, "Create a lover to win with you", "Create, protect and assist your lover", RoleId.Romantic, Factions.Neutral, RoleAlignment.NeutralBenign, "As the Romantic, you must pick a player to love, working together to ensure both of your survival. If your target dies, you will become a Pursuer.");
        #endregion

        #region Impostors
        public readonly static RoleInfo undertaker = new("Undertaker", Ninja.Color, "Drag dead bodies and hide them around the map", "Drag dead bodies", RoleId.Undertaker, Factions.Impostor, RoleAlignment.ImpConcealing, "The Undertaker is an Impostor who can drag dead bodies around the map. The Undertaker may vent during the drag, depending on settings.");
        public readonly static RoleInfo miner = new("Miner", Miner.Color, "Dig new vents around the map", "Create vents", RoleId.Miner, Factions.Impostor, RoleAlignment.ImpSupport, "The Miner can create new vents across all the map (if not too close to walls). The vents may be visible instantly, after meetings or after a set amount of time the Miner creates it. Any venter can use the Miner vents.");
        public readonly static RoleInfo morphling = new("Morphling", Morphling.Color, "Change your look to not get caught", "Change your look", RoleId.Morphling, Factions.Impostor, RoleAlignment.ImpConcealing, $"The Morphling can morph into the form of their fellow Crewmates, morphing changes the Morphling's look to make them not look sus. The morphling can only morph into a crewmate once every {Morphling.Cooldown} seconds and lasts for {Morphling.Duration} seconds.");
        public readonly static RoleInfo blackmailer = new("Blackmailer", Blackmailer.Color, "Silence everyone who oppose you", "Silence your enemies", RoleId.Blackmailer, Factions.Impostor, RoleAlignment.ImpSupport, $"The Blackmailer can blackmail other players into silence. The blackmailed player will be unable to speak in the next meeting.");
        public readonly static RoleInfo camouflager = new("Camouflager", Camouflager.Color, "Camouflage and kill the Crewmates", "Hide among others", RoleId.Camouflager, Factions.Impostor, RoleAlignment.ImpConcealing, $"The Camouflager can turn everyone gray making everyone unkown and nobody knows who is who for {Camouflager.Duration}s every {Camouflager.Cooldown}s.");
        public readonly static RoleInfo poisoner = new("Poisoner", Poisoner.Color, "Kill the Crewmates with your bites", "Bite your enemies", RoleId.Poisoner, Factions.Impostor,  RoleAlignment.ImpPower, $"The Poisoner can bite a player every {Poisoner.Cooldown} seconds, after {Poisoner.delay} seconds the player die. Players with protection can't be killed by the Poisoner. If the Poisoner is alive in the last 4, they will directly kill instead of bitting.");
        public readonly static RoleInfo eraser = new("Eraser", Eraser.Color, "Kill the Crewmates and erase their roles", "Erase the roles of your enemies", RoleId.Eraser, Factions.Impostor, RoleAlignment.ImpSupport, "The Eraser can delete player's role for the rest of the game, making them become regular crewmate. They may be able to erase Neutral killers depending on settings.");
        public readonly static RoleInfo trickster = new("Trickster", Trickster.Color, "Use your jack-in-the-boxes to surprise others", "Surprise your enemies", RoleId.Trickster, Factions.Impostor, RoleAlignment.ImpSupport, "The trickster can place boxes around the map which works like a vent, only the Trickster may use them. They can also manually sabotage lights, at any time, with any sabotage on but lights.");
        public readonly static RoleInfo cleaner = new("Cleaner", Cleaner.Color, "Kill everyone and leave no traces", "Clean up dead bodies", RoleId.Cleaner, Factions.Impostor, RoleAlignment.ImpSupport, "The Cleaner is an Impostor that can clean up bodies. Both their Kill and Clean ability have a shared Cooldown, meaning they have to choose which one they want to use.");
        public readonly static RoleInfo grenadier = new("Grenadier", Grenadier.Color, "Blind players to get sneaky kills", "Blind other players", RoleId.Grenadier, Factions.Impostor, RoleAlignment.ImpSupport, "The Grenadier is an Impostor that can flashbang other players, making them blind for a set amount of time, Impostors and dead people won't be affected by this, neither will people outside of the Grenadier radius, which is also set in settings. The Spy, other impostors and dead people won't be affected by the Grenadier flash.");
        public readonly static RoleInfo warlock = new("Warlock", Warlock.Color, "Curse other players and kill everyone", "Curse and kill everyone", RoleId.Warlock, Factions.Impostor, RoleAlignment.ImpPower, "The Warlock is an Impostor, that can curse another player (the cursed player doesn't get notified). If the cursed person stands next to another player, the Warlock is able to kill that player (no matter how far away they are).");
        public readonly static RoleInfo bountyHunter = new("Bounty Hunter", BountyHunter.Color, "Hunt your bounty down", "Hunt your bounty down", RoleId.BountyHunter, Factions.Impostor,  RoleAlignment.ImpPower, "As the Bounty Hunter, you are given a target, which your task is to eliminate them, killing your target gives you a short Cooldown, else will give you a long penalty Cooldown.");
        public readonly static RoleInfo impostor = new("Impostor", Palette.ImpostorRed, Utils.ColorString(Palette.ImpostorRed, "Sabotage and kill everyone"), "Sabotage and kill everyone", RoleId.Impostor, Factions.Impostor, RoleAlignment.ImpSpecial, "Just a regular Impostor");
        public readonly static RoleInfo witch = new("Witch", Witch.Color, "Cast a spell upon your foes", "Cast a spell upon your foes", RoleId.Witch, Factions.Impostor, RoleAlignment.ImpPower, "The Witch is an Impostor who has the ability to cast a spell on other players. During the next meeting, the spellbound player will be highlighted and they'll die right after the meeting. There are multiple options listed down below with which you can configure to fit your taste. Similar to the Poisoner, Shields and blanks will be checked twice (at the end of casting the spell on the player and at the end of the meeting, when the spell will be activated).");
        public readonly static RoleInfo ninja = new("Ninja", Ninja.Color, "Surprise and assassinate your foes", "Surprise and assassinate your foes", RoleId.Ninja, Factions.Impostor, RoleAlignment.ImpConcealing, "The Ninja is an Impostor who has the ability to kill another player all over the map. You can mark a player with your ability and by using the ability again, you jump to the position of the marked player and kill them.");
        public readonly static RoleInfo wraith = new("Wraith", Wraith.Color, "Vanish to kill your foes", "Become invisible", RoleId.Wraith, Factions.Impostor, RoleAlignment.ImpSupport, "The Wraith is an Impostor role that can go invisible for a set amount of time (settings) they can NOT vent at all, and may just kill, sabotage and go invisible.");
        public readonly static RoleInfo yoyo = new("Yo-Yo", Yoyo.Color, "Blink to a marked location and Back", "Blink to a location", RoleId.Yoyo, Factions.Impostor, RoleAlignment.ImpConcealing, "The Yo-Yo is an Impostor who has the ability mark a position and later blink (teleport) to this position. After the initial blink, the Yo-Yo has a fixed amount of time (option) to do whatever they want, before automatically blinking back to the starting point of the first blink. Each blink leaves behind a silhouette with configurable transparency. The silhouette is very hard to see.The Yo-Yo may also have access to a mobile admin table, depending on the settings.");

        #endregion

        #region Crewmates
        public readonly static RoleInfo crewmate = new("Crewmate", Palette.CrewmateBlue, "Find the Impostors", "Find the Impostors", RoleId.Crewmate, Factions.Crewmate, RoleAlignment.CrewSpecial, "Just a regular Crewmate.");
        public readonly static RoleInfo lighter = new("Lighter", Lighter.Color, "Your light never goes out", "Your light never goes out", RoleId.Lighter, Factions.Crewmate, RoleAlignment.CrewSupport, "As the lighter, you have the ability to temporarly enhance your vision.");
        public readonly static RoleInfo detective = new("Detective", Detective.Color, "Find the <color=#FF1919FF>Impostors</color> by examining footprints", "Examine footprints", RoleId.Detective, Factions.Crewmate,  RoleAlignment.CrewInvest, "The Detective can see footprints that other players leave behind. The Detective's other feature shows when they report a corpse: they receive clues about the killer's identity. The type of information they get is based on the time it took them to find the corpse.");
        public readonly static RoleInfo hacker = new("Hacker", Hacker.Color, "Hack systems to find the <color=#FF1919FF>Impostors</color>", "Hack to find the Impostors", RoleId.Hacker, Factions.Crewmate, RoleAlignment.CrewInvest, "If the Hacker activates the Hacker mode, the Hacker gets more information than others from the admin table and vitals for a set Duration. Otherwise they see the same information as everyone else. The Hacker can see the colors (or Color types) of the players on the table. They can also see how long dead players have been dead for. The Hacker can access his mobile gadgets (vitals & admin table), with a maximum of Charges (uses) and a configurable amount of tasks needed to recharge.");
        public readonly static RoleInfo tracker = new("Tracker", Tracker.Color, "Track the <color=#FF1919FF>Impostors</color> down", "Track the Impostors down", RoleId.Tracker, Factions.Crewmate, RoleAlignment.CrewInvest, $"The Tracker is able to track the movements of other players. The Arrow's Color will be the tracked players Color. The arrow will update the position of the player every {Tracker.updateIntervall} seconds. The Arrows will reset depending on settings after each meeting. They can track dead bodies depending on settings as well.");
        public readonly static RoleInfo crusader = new("Crusader", Crusader.Color, "Fortify a Crewmate to Eliminate the <color=#FF1919FF>Impostors</color>", "Fortify a Crewmate", RoleId.Crusader, Factions.Crewmate, RoleAlignment.CrewProtect, "The Crusader can fortify a player in order to protect them from being touched. If somebody tries to kill the fortified player the killer will die. If a non killing role interacts with them, nothing will happen. The Crusader can Fortify one player per round.");
        public readonly static RoleInfo spy = new("Spy", Spy.Color, "Confuse the <color=#FF1919FF>Impostors</color>", "Confuse the Impostors", RoleId.Spy, Factions.Crewmate, RoleAlignment.CrewSupport, "The Spy appears as another Impostor when there's more than 2 Impostors, they may vent or be able to die by the Sheriff, your job is to confuse the impostors into killing themselves.");
        public readonly static RoleInfo vigilante = new("Vigilante", Vigilante.Color, "Seal vents and place cameras", "Seal vents and place cameras", RoleId.Vigilante, Factions.Crewmate, RoleAlignment.CrewSupport, "The Vigilante is a Crewmate that has a certain number of screws that they can use for either sealing vents or for placing new cameras. bPlacing a new camera and sealing vents takes a configurable amount of screws. The total number of screws that a Vigilante has can also be configured. The new camera will be visible after the next meeting and accessible by everyone. The vents will be sealed after the next meeting, players can't enter or exit sealed vents, but they can still move to them underground.");
        public readonly static RoleInfo mayor = new("Mayor", Mayor.Color, "Your vote counts twice", "Your vote counts twice", RoleId.Mayor, Factions.Crewmate, RoleAlignment.CrewPower, "The Mayor leads the Crewmates by having a vote that counts twice. The Mayor can always use their meeting, even if the maximum number of meetings was reached. The Mayor has a portable Meeting Button, depending on the options. The Mayor can see the vote colors after completing a configurable amount of tasks, depending on the options. The Mayor has the option to vote with only one vote instead of two (via a button in the meeting screen), depending on the settings.");
        public readonly static RoleInfo portalmaker = new("Portalmaker", Portalmaker.Color, "You can create portals", "You can create portals", RoleId.Portalmaker, Factions.Crewmate, RoleAlignment.CrewSupport, "The Portalmaker is a Crewmate that can place two portals on the map. These two portals are connected to each other. Those portals will be visible after the next meeting and can be used by everyone. Additionally to that, the Portalmaker gets information about who used the portals and when in the chat during each meeting, depending on the options. The Portalmaker can teleport themself to their placed portals from anywhere if the setting is enabled.");
        public readonly static RoleInfo veteran = new("Veteran", Veteran.Color, "Alert to murder evil players who touch you", "Alert to kill the <color=#FF1919FF>Evildoers</color>", RoleId.Veteran, Factions.Crewmate, RoleAlignment.CrewPower, $"The Veteran is able to alert, Alerting makes the Veteran Unkillable and will kill anyone who interacts with them. At the start of the game the Veteran can alert a maximum of " + Veteran.Charges + " times.");
        public readonly static RoleInfo engineer = new("Engineer",  Engineer.Color, "Maintain important systems on the ship", "Repair the ship", RoleId.Engineer, Factions.Crewmate, RoleAlignment.CrewSupport, $"The Engineer is able to vent around the map and fix sabotages. The Engineer can fix a maximum of " + Engineer.remainingFixes + " sabotages.");
        public readonly static RoleInfo sheriff = new("Sheriff", Sheriff.Color, "Shoot the <color=#FF1919FF>Impostors</color>", "Shoot the Impostors", RoleId.Sheriff, Factions.Crewmate, RoleAlignment.CrewPower, "The Sheriff is able to kill players during rounds, if the player they kill is an impostor, or Neutral Killer, the Sheriff will survive. If the player they kill is a crewmate, the Sheriff will die.");
        public readonly static RoleInfo medium = new("Medium", Medium.Color, "Question the souls of the dead to gain information", "Question the souls", RoleId.Medium, Factions.Crewmate, RoleAlignment.CrewInvest, "The medium is a crewmate who can ask the souls of dead players for information. Like the Mystic, the medium will see the souls of the players who have died (after the next meeting) and can question them. They then gets random information about the soul or the killer in the chat. The souls only stay for one round, i.e. until the next meeting. Depending on the options, the souls can only be questioned once and then disappear.");
        public readonly static RoleInfo trapper = new("Trapper", Trapper.Color, "Place traps to find the Impostors", "Place traps", RoleId.Trapper, Factions.Crewmate, RoleAlignment.CrewInvest, "The Tracker can select one player to track. Depending on the options the Tracker can track a different person after each meeting or the Tracker tracks the same person for the whole game. An arrow points to the last tracked position of the player. The arrow updates its position every few seconds (configurable). By an option, the arrow can be replaced or combined with the Proximity Tracker from Hide N Seek. Depending on the options, the Tracker has another ability: They can track all corpses on the map for a set amount of time. They will keep tracking corpses, even if they were cleaned or eaten by the Vulture.");
        public readonly static RoleInfo timeMaster = new("Time Master", TimeMaster.Color, "Rewind Time To Screw the Killers", "Rewind Time", RoleId.TimeMaster, Factions.Crewmate, RoleAlignment.CrewSupport, "The Time Master can rewind the time, making players go back the exact steps they walked but in reverse, if a player was killed within the same amount of time the rewind lasts and if the options allows it, the dead players will get revived.");
        public readonly static RoleInfo medic = new("Medic", Medic.Color, "Protect someone with your Shield", "Protect other players", RoleId.Medic, Factions.Crewmate, RoleAlignment.CrewProtect, "The Medic can Shield (highlighted by an outline around the player) one player per game, which makes the player unkillable. The Shield is also shown in the meeting as brackets around the Shielded player's name. The Shielded player can still be voted out and might also be an Impostor. If set in the options, the Shielded player and/or the Medic will get a red flash on their screen if someone (Impostor, Sheriff, ...) tried to murder them. If the Medic dies, the Shield disappears with them. The Sheriff will not die if they try to kill a Shielded Crewmate and won't perform a kill if they try to kill a Shielded Impostor. Depending on the options, guesses from the Guesser will be blocked by the Shield and the Shielded player/medic might be notified. The Medic's other feature shows when they report a corpse: they will see how long ago the player died.");
        public readonly static RoleInfo swapper = new("Swapper", Swapper.Color, "Swap votes to exile the <color=#FF1919FF>Impostors</color>", "Swap votes", RoleId.Swapper, Factions.Crewmate, RoleAlignment.CrewPower, "During meetings the Swapper can exchange votes that two people get (i.e. all votes that player A got will be given to player B and vice versa). Because of the Swapper's strength in meetings, they might not start emergency meetings and can't fix lights and comms. The Swapper now has initial swap Charges and can recharge those Charges after completing a configurable amount of tasks.");
        public readonly static RoleInfo oracle = new("Oracle", Oracle.Color, "Make the <color=#FF1919FF>Impostors</color> confess their sins", "Get another player to confess on your passing", RoleId.Oracle, Factions.Crewmate, RoleAlignment.CrewInvest, $"The Oracle can compel another player to confess their secrets upon death. The oracle will get information about 3 players being possibly evil each meeting. The Oracle can only make a player confess once per meeting. When the Oracle dies, the player they made confess will reveal their faction with a probability of {Oracle.Accuracy}% of being right.");
        public readonly static RoleInfo mystic = new("Mystic", Mystic.Color, "You will see players die", "You will see players die", RoleId.Mystic, Factions.Crewmate, RoleAlignment.CrewInvest, "The Mystic gets a list of the possible roles that the examined player can be in meetings. The Mystic has more abilities (one can activate one of them or both in the options). The Mystic sees the souls of players that died a round earlier, the souls slowly fade away. The Mystic gets a blue flash on their screen, if a player dies somewhere on the map.");

        #endregion

        #region Neutral Killers
        public readonly static RoleInfo jackal = new("Jackal", Jackal.Color, "Kill all Crewmates and <color=#FF1919FF>Impostors</color> to win", "Kill everyone", RoleId.Jackal, Factions.Neutral, RoleAlignment.NeutralKilling, "The Jackal is a Neutral role with its own win condition. The Jackal can pick a Sidekick. Creating a Sidekick removes all tasks of the Sidekick and adds them to the team Jackal. The Create Sidekick Action can only be used once per Jackal or once per game. The Jackal can also promote Impostors to be their Sidekick, but depending on the options the Impostor will either really turn into the Sidekick and leave the team Impostors or they will just look like the Sidekick to the Jackal and remain as they were. Also if a Spy or Impostor gets sidekicked, they still will appear red to the Impostors.");
        public readonly static RoleInfo plaguebearer = new("Plaguebearer", Plaguebearer.Color, "Infect all players to become Pestilence", "Infect to become Pestilence", RoleId.Pestilence, Factions.Neutral, RoleAlignment.NeutralKilling, "The Plaguebearer is a Neutral role with its own win condition, as well as an ability to transform into another role. The Plaguebearer has one ability, which allows them to infect other players. Once all players are infected, the Plaguebearer becomes Pestilence.");
        public readonly static RoleInfo pestilence = new("Pestilence", Pestilence.Color, "", "Kill with your unstoppable abilities", RoleId.Pestilence, Factions.Neutral, RoleAlignment.NeutralKilling, "The Pestilence is a unkillable force which can only be killed by being voted out or them guessing wrong. The Pestilence needs to be the last killer alive to win the game.");
        public readonly static RoleInfo juggernaut = new("Juggernaut", Juggernaut.Color, "Kill all your <color=#FF1919FF>Enemies</color> to win", "Each kill makes you more dangerous", RoleId.Juggernaut, Factions.Neutral, RoleAlignment.NeutralKilling, "The Juggernaut is a Neutral role with its own win condition. The Juggernaut's special ability is that their kill Cooldown reduces with each kill. This means in theory the Juggernaut can have a 0 second kill Cooldown!. The Juggernaut needs to be the last killer alive to win the game.");
        public readonly static RoleInfo sidekick = new("Sidekick", Sidekick.Color, "Help your Jackal to kill everyone", "Help your Jackal to kill everyone", RoleId.Sidekick, Factions.Neutral, RoleAlignment.NeutralKilling, "Gets assigned to a player during the game by the Create Sidekick Action of the Jackal and joins the Jackal in their quest to eliminate all other players. Upon the death of the Jackal (depending on the options), they might get promoted to Jackal themself and potentially even assign a Sidekick of their own.");
        public readonly static RoleInfo agent = new("Agent", Agent.Color, "Finish your duties to start the dirty work", "Finish your tasks", RoleId.Agent, Factions.Neutral, RoleAlignment.NeutralKilling, "The Agent is a Neutral killer role with its own win condition. They need to finish tasks in order to gain new abilities. Depending on settings they may be able to vent so they finish tasks faster.");
        public readonly static RoleInfo hitman = new("Hitman", Hitman.Color, "Kill your enemies to win", "Kill your enemies", RoleId.Hitman, Factions.Neutral, RoleAlignment.NeutralKilling, "The Hitman is a Neutral role with its own win condition. The Hitman's aim is to kill win alone. The Hitman is able to kill players, morph into them like a Morphling or a Glitch for a set amount of time. They can also drag dead bodies just like an Undertaker. They may be able to vent depending on settings.");
        public readonly static RoleInfo predator = new("Predator", Predator.Color, "Terminate to make everyone die", "Murder everyone when stabbing", RoleId.Predator, Factions.Neutral, RoleAlignment.NeutralKilling, "The Predator is a Neutral role with its own win condition. The Predator has an invisible kill button, but they can't use it unless they are stabbing. Once the Predator rampages they gain Impostor vision and the ability to kill. However, unlike most killers their kill Cooldown is really short. The Predator needs to be the last killer alive to win the game.");
        public readonly static RoleInfo glitch = new("Glitch", Glitch.Color, "Hack, Kill and Mimic your <color=#FF1919FF>enemies</color>", "Hack, Kill and Mimic your <color=#FF1919FF>enemies</color>", RoleId.Glitch, Factions.Neutral, RoleAlignment.NeutralKilling, "Glitch is a Neutral role with its own win condition. Glitch's aim is to kill everyone and be the last person standing. Glitch can Hack players, resulting in them being unable to report bodies and do tasks. Hacking prevents the hacked player from doing anything but walk around the map. Glitch can Mimic someone, which results in them looking exactly like the other person.");
        public readonly static RoleInfo vromantic = new("Vengeful Romantic", Romantic.Color, "", "Avenge your lover", RoleId.VengefulRomantic, Factions.Neutral, RoleAlignment.NeutralKilling, "As the Vengeful Romantic you were once a Romantic with a lover, but they died somehow. Now you are mad for revenge and will murder everyone in order to avenge your lover, if you win your dead lover also does.");
        public readonly static RoleInfo werewolf = new("Werewolf", Werewolf.Color, "Maul and eliminate your <color=#FF1919FF>enemies</color>", "Maul to eliminate your <color=#FF1919FF>enemies</color>", RoleId.Werewolf, Factions.Neutral, RoleAlignment.NeutralKilling, "The Werewolf can kill all players within a certain radius.");

        #endregion

        public static List<RoleInfo> allRoleInfos = new List<RoleInfo>() 
        {
            // Impostors
            blackmailer,
            bountyHunter,
            camouflager,
            cleaner,
            eraser,
            grenadier,
            miner,
            morphling,
            ninja,
            trickster,
            undertaker,
            poisoner,
            wraith,
            warlock,
            witch,
            yoyo,

            // Crewmates
            crewmate,
            crusader,
            detective,
            engineer,
            hacker,
            lighter,
            mayor,
            medic,
            medium,
            mystic,
            oracle,
            portalmaker,
            sheriff,
            spy,
            swapper,
            timeMaster,
            tracker,
            trapper,
            vigilante,
            veteran,

            // Neutrals
            amnesiac,
            arsonist,
            jester,
            lawyer,
            prosecutor,
            pursuer,
            romantic,
            vulture,

            // Neutral Killers
            agent,
            glitch,
            hitman,
            jackal,
            juggernaut,
            pestilence,
            plaguebearer,
            predator,
            sidekick,
            vromantic,
            werewolf,
        };

        public static List<RoleInfo> GetRoleInfoForPlayer(PlayerControl player) 
        {
            List<RoleInfo> infos = new List<RoleInfo>();
            if (player == null) return infos;

            // Special roles

            if (player == Jester.Player) infos.Add(jester);
            if (player == Pestilence.Player) infos.Add(pestilence);
            if (player == Plaguebearer.Player) infos.Add(plaguebearer);
            if (player == Mayor.Player) infos.Add(mayor);
            if (player == Portalmaker.Player) infos.Add(portalmaker);
            if (player == Engineer.Player) infos.Add(engineer);
            if (player == Sheriff.Player) infos.Add(sheriff);
            if (player == Romantic.Player) infos.Add(romantic);
            if (player == Juggernaut.Player) infos.Add(juggernaut);
            if (player == Crusader.Player) infos.Add(crusader);
            if (player == Miner.Player) infos.Add(miner);
            if (player == Undertaker.Player) infos.Add(undertaker);
            if (player == VengefulRomantic.Player) infos.Add(vromantic);
            if (player == Glitch.Player) infos.Add(glitch);
            if (player == Blackmailer.Player) infos.Add(blackmailer);
            if (player == Predator.Player) infos.Add(predator);
            if (player == Oracle.Player) infos.Add(oracle);
            if (player == Werewolf.Player) infos.Add(werewolf);
            if (player == Veteran.Player) infos.Add(veteran);
            if (player == Lighter.Player) infos.Add(lighter);
            if (player == Morphling.Player) infos.Add(morphling);
            if (player == Camouflager.Player) infos.Add(camouflager);
            if (player == Poisoner.Player) infos.Add(poisoner);
            if (player == Eraser.Player) infos.Add(eraser);
            if (player == Trickster.Player) infos.Add(trickster);
            if (player == Grenadier.Player) infos.Add(grenadier);
            if (player == Cleaner.Player) infos.Add(cleaner);
            if (player == Warlock.Player) infos.Add(warlock);
            if (player == Witch.Player) infos.Add(witch);
            if (player == Wraith.Player) infos.Add(wraith);
            if (player == Ninja.Player) infos.Add(ninja);
            if (player == Yoyo.Player) infos.Add(yoyo);
            if (player == Amnesiac.Player) infos.Add(amnesiac);
            if (player == Detective.Player) infos.Add(detective);
            if (player == TimeMaster.Player) infos.Add(timeMaster);
            if (player == Medic.Player) infos.Add(medic);
            if (player == Hitman.Player) infos.Add(hitman);
            if (player == Agent.Player) infos.Add(agent);
            if (player == Swapper.Player) infos.Add(swapper);
            if (player == Mystic.Player) infos.Add(mystic);
            if (player == Hacker.Player) infos.Add(hacker);
            if (player == Tracker.Player) infos.Add(tracker);
            if (player == Jackal.Player || (Jackal.formerJackals != null && Jackal.formerJackals.Any(x => x.PlayerId == player.PlayerId))) infos.Add(jackal);
            if (player == Sidekick.Player) infos.Add(sidekick);
            if (player == Spy.Player) infos.Add(spy);
            if (player == Vigilante.Player) infos.Add(vigilante);
            if (player == Arsonist.Player) infos.Add(arsonist);
            if (player == BountyHunter.Player) infos.Add(bountyHunter);
            if (player == Vulture.Player) infos.Add(vulture);
            if (player == Medium.medium) infos.Add(medium);
            if (player == Lawyer.Player) infos.Add(lawyer);
            if (player == Prosecutor.Player) infos.Add(prosecutor);
            if (player == Trapper.Player) infos.Add(trapper);
            if (player == Pursuer.Player) infos.Add(pursuer);

            // Default roles (just impostor, just crewmate, or hunter / hunted for hide n seek, prop hunt prop ...
            if (!infos.Any()) 
            {
                infos.Add(player.Data.Role.IsImpostor ? impostor : crewmate);
            }

            return infos;
        }

        public static String GetRolesString(PlayerControl player, bool useColors) 
        {
            string roleName;
            roleName = String.Join(" ", GetRoleInfoForPlayer(player).Select(x => useColors ? Utils.ColorString(x.Color, x.Name) : x.Name).ToArray());
            return roleName;
        }
        public static String GetGhostInfoString(PlayerControl player)
        {
            string msg = "";
            if (player == Vulture.Player && (PlayerControl.LocalPlayer == Vulture.Player || Utils.ShouldShowGhostInfo()))
                msg += Utils.ColorString(Vulture.Color, $" ({Vulture.vultureNumberToWin - Vulture.eatenBodies} left) ");
            
            if (Utils.ShouldShowGhostInfo())
            {
                if (Eraser.futureErased.Contains(player))
                    msg += Utils.ColorString(Color.gray, " (Erased)");
                if (Poisoner.Player != null && !Poisoner.Player.Data.IsDead && Poisoner.poisoned == player && !player.Data.IsDead)
                    msg += Utils.ColorString(Poisoner.Color, $" (Poisoned {(int)HudManagerStartPatch.poisonerKillButton.Timer + 1})");
                if (Glitch.HackedPlayers.Contains(player.PlayerId))
                    msg += Utils.ColorString(Color.gray, " (Hacked)");
                if (Glitch.HackedKnows.ContainsKey(player.PlayerId))  // Active cuff
                    msg += Utils.ColorString(Glitch.Color, " (Hacked)");
                if (player == Warlock.curseVictim)
                    msg += Utils.ColorString(Warlock.Color, " (Cursed)");
                if (player == Ninja.ninjaMarked)
                    msg += Utils.ColorString(Ninja.Color, " (Marked)");
                if (player == Crusader.FortifiedPlayer)
                    msg += Utils.ColorString(Crusader.Color, " (Fortified)");
                if (Pursuer.blankedList.Contains(player) && !player.Data.IsDead)
                    msg += Utils.ColorString(Pursuer.Color, " (Blanked)");
                if (Witch.futureSpelled.Contains(player) && !MeetingHud.Instance) // This is already displayed in meetings!
                    msg += Utils.ColorString(Witch.Color, " (☆)");
                if (BountyHunter.bounty == player)
                    msg += Utils.ColorString(BountyHunter.Color, " (⦿)");
                if (Plaguebearer.InfectedPlayers.Contains(player.PlayerId))
                    msg += Utils.ColorString(Plaguebearer.Color, " (⦿)");
                if (Arsonist.dousedPlayers.Contains(player))
                    msg += Utils.ColorString(Arsonist.Color, " (♨)");
                if (player == Arsonist.Player)
                    msg += Utils.ColorString(Arsonist.Color, $" ({PlayerControl.AllPlayerControls.ToArray().Count(x => { return x != Arsonist.Player && !x.Data.IsDead && !x.Data.Disconnected && !Arsonist.dousedPlayers.Any(y => y.PlayerId == x.PlayerId); })} left)");
            }
            return msg;
        }
        public static String GetDeathReasonString(PlayerControl player)
        {
            string msg = "";
            // Death Reason on Ghosts
            if (player.Data.IsDead) 
            {
                string deathReasonString = "";
                var deadPlayer = GameHistory.deadPlayers.FirstOrDefault(x => x.player.PlayerId == player.PlayerId);
                Color killerColor = new();
                if (deadPlayer != null && deadPlayer.killerIfExisting != null) 
                {
                    killerColor = GetRoleInfoForPlayer(deadPlayer.killerIfExisting).FirstOrDefault().Color;
                }
                if (deadPlayer != null) 
                {
                    switch (deadPlayer.deathReason) 
                    {
                        case DeadPlayer.CustomDeathReason.Disconnect:
                            deathReasonString = " | Disconnected";
                            break;
                        case DeadPlayer.CustomDeathReason.Exile:
                                deathReasonString = " | Voted out";
                            break;
                        case DeadPlayer.CustomDeathReason.Kill:
                            deathReasonString = $" | Killed by {Utils.ColorString(killerColor, deadPlayer.killerIfExisting.Data.PlayerName)}";
                            break;
                        case DeadPlayer.CustomDeathReason.Guess:
                            if (deadPlayer.killerIfExisting.Data.PlayerName == player.Data.PlayerName)
                                deathReasonString = $" | Misguessed";
                            else
                                deathReasonString = $" | Guessed by {Utils.ColorString(killerColor, deadPlayer.killerIfExisting.Data.PlayerName)}";
                            break;
                        case DeadPlayer.CustomDeathReason.WitchExile:
                            deathReasonString = $" | {Utils.ColorString(Witch.Color, "Witched")} by {Utils.ColorString(killerColor, deadPlayer.killerIfExisting.Data.PlayerName)}";
                            break;
                        case DeadPlayer.CustomDeathReason.Maul:
                            deathReasonString = $" | {Utils.ColorString(Werewolf.Color, "Mauled")} by {Utils.ColorString(killerColor, deadPlayer.killerIfExisting.Data.PlayerName)}";
                            break;
                        case DeadPlayer.CustomDeathReason.LoverSuicide:
                            deathReasonString = $" | {Utils.ColorString(Lovers.Color, "Lover Suicide")}";
                            break;
                        case DeadPlayer.CustomDeathReason.LawyerSuicide:
                            deathReasonString = $" | {Utils.ColorString(Lawyer.Color, "Client Voted Out")}";
                            break;
                        case DeadPlayer.CustomDeathReason.Arson:
                            deathReasonString = $" | Ignited by {Utils.ColorString(killerColor, deadPlayer.killerIfExisting.Data.PlayerName)}";
                            break;
                    }
                    msg = msg + deathReasonString;
                }
            }
            return msg;
        }
    }
}