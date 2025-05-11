using System;
using System.Collections.Generic;
using UnityEngine;

namespace TheSushiRoles.Patches
{
    [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.OnDestroy))]
    class RandomSpawnsPatch
    {
        public static System.Random rnd = new System.Random((int)DateTime.Now.Ticks);

        public static void Prefix(IntroCutscene __instance)
        {
            if (CustomOptionHolder.RandomSpawns.GetBool())
            {
                List<Vector3> skeldSpawn = new List<Vector3>()
                {
                    new Vector3(-2.2f, 2.2f, 0f), //cafeteria. botton. top left.
                    new Vector3(0.7f, 2.2f, 0f), //caffeteria. button. top right.
                    new Vector3(-2.2f, -0.2f, 0f), //caffeteria. button. bottom left.
                    new Vector3(0.7f, -0.2f, 0f), //caffeteria. button. bottom right.
                    new Vector3(10.0f, 3.0f, 0f), //weapons top
                    new Vector3(9.0f, 1.0f, 0f), //weapons bottom
                    new Vector3(6.5f, -3.5f, 0f), //O2
                    new Vector3(11.5f, -3.5f, 0f), //O2-nav hall
                    new Vector3(17.0f, -3.5f, 0f), //navigation top
                    new Vector3(18.2f, -5.7f, 0f), //navigation bottom
                    new Vector3(11.5f, -6.5f, 0f), //nav-Shields top
                    new Vector3(9.5f, -8.5f, 0f), //nav-Shields bottom
                    new Vector3(9.2f, -12.2f, 0f), //Shields top
                    new Vector3(8.0f, -14.3f, 0f), //Shields bottom
                    new Vector3(2.5f, -16f, 0f), //coms left
                    new Vector3(4.2f, -16.4f, 0f), //coms middle
                    new Vector3(5.5f, -16f, 0f), //coms right
                    new Vector3(-1.5f, -10.0f, 0f), //storage top
                    new Vector3(-1.5f, -15.5f, 0f), //storage bottom
                    new Vector3(-4.5f, -12.5f, 0f), //storrage left
                    new Vector3(0.3f, -12.5f, 0f), //storrage right
                    new Vector3(4.5f, -7.5f, 0f), //admin top
                    new Vector3(4.5f, -9.5f, 0f), //admin bottom
                    new Vector3(-9.0f, -8.0f, 0f), //elec top left
                    new Vector3(-6.0f, -8.0f, 0f), //elec top right
                    new Vector3(-8.0f, -11.0f, 0f), //elec bottom
                    new Vector3(-12.0f, -13.0f, 0f), //elec-lower hall
                    new Vector3(-17f, -10f, 0f), //lower engine top
                    new Vector3(-17.0f, -13.0f, 0f), //lower engine bottom
                    new Vector3(-21.5f, -3.0f, 0f), //reactor top
                    new Vector3(-21.5f, -8.0f, 0f), //reactor bottom
                    new Vector3(-13.0f, -3.0f, 0f), //security top
                    new Vector3(-12.6f, -5.6f, 0f), // security bottom
                    new Vector3(-17.0f, 2.5f, 0f), //upper engibe top
                    new Vector3(-17.0f, -1.0f, 0f), //upper engine bottom
                    new Vector3(-10.5f, 1.0f, 0f), //upper-mad hall
                    new Vector3(-10.5f, -2.0f, 0f), //medbay top
                    new Vector3(-6.5f, -4.5f, 0f) //medbay bottom
                };

                List<Vector3> miraSpawn = new List<Vector3>()
                {
                    new Vector3(-4.5f, 3.5f, 0f), //launchpad top
                    new Vector3(-4.5f, -1.4f, 0f), //launchpad bottom
                    new Vector3(8.5f, -1f, 0f), //launchpad- med hall
                    new Vector3(14f, -1.5f, 0f), //medbay
                    new Vector3(16.5f, 3f, 0f), // comms
                    new Vector3(10f, 5f, 0f), //lockers
                    new Vector3(6f, 1.5f, 0f), //locker room
                    new Vector3(2.5f, 13.6f, 0f), //reactor
                    new Vector3(6f, 12f, 0f), //reactor middle
                    new Vector3(9.5f, 13f, 0f), //lab
                    new Vector3(15f, 9f, 0f), //bottom left cross
                    new Vector3(17.9f, 11.5f, 0f), //middle cross
                    new Vector3(14f, 17.3f, 0f), //office
                    new Vector3(19.5f, 21f, 0f), //admin
                    new Vector3(14f, 24f, 0f), //greenhouse left
                    new Vector3(22f, 24f, 0f), //greenhouse right
                    new Vector3(21f, 8.5f, 0f), //bottom right cross
                    new Vector3(28f, 3f, 0f), //caf right
                    new Vector3(22f, 3f, 0f), //caf left
                    new Vector3(19f, 4f, 0f), //storage
                    new Vector3(22f, -2f, 0f), //balcony
                };

                List<Vector3> polusSpawn = new List<Vector3>()
                {
                    new Vector3(16.6f, -1f, 0f), //dropship top
                    new Vector3(16.6f, -5f, 0f), //dropship bottom
                    new Vector3(20f, -9f, 0f), //above storrage
                    new Vector3(22f, -7f, 0f), //right fuel
                    new Vector3(25.5f, -6.9f, 0f), //drill
                    new Vector3(29f, -9.5f, 0f), //lab lockers
                    new Vector3(29.5f, -8f, 0f), //lab weather notes
                    new Vector3(35f, -7.6f, 0f), //lab table
                    new Vector3(40.4f, -8f, 0f), //lab scan
                    new Vector3(33f, -10f, 0f), //lab toilet
                    new Vector3(39f, -15f, 0f), //specimen hall top
                    new Vector3(36.5f, -19.5f, 0f), //specimen top
                    new Vector3(36.5f, -21f, 0f), //specimen bottom
                    new Vector3(28f, -21f, 0f), //specimen hall bottom
                    new Vector3(24f, -20.5f, 0f), //admin tv
                    new Vector3(22f, -25f, 0f), //admin books
                    new Vector3(16.6f, -17.5f, 0f), //office coffe
                    new Vector3(22.5f, -16.5f, 0f), //office projector
                    new Vector3(24f, -17f, 0f), //office figure
                    new Vector3(27f, -16.5f, 0f), //office lifelines
                    new Vector3(32.7f, -15.7f, 0f), //lavapool
                    new Vector3(31.5f, -12f, 0f), //snowmad below lab
                    new Vector3(10f, -14f, 0f), //below storrage
                    new Vector3(21.5f, -12.5f, 0f), //storrage vent
                    new Vector3(19f, -11f, 0f), //storrage toolrack
                    new Vector3(12f, -7f, 0f), //left fuel
                    new Vector3(5f, -7.5f, 0f), //above elec
                    new Vector3(10f, -12f, 0f), //elec fence
                    new Vector3(9f, -9f, 0f), //elec lockers
                    new Vector3(5f, -9f, 0f), //elec window
                    new Vector3(4f, -11.2f, 0f), //elec tapes
                    new Vector3(5.5f, -16f, 0f), //elec-O2 hall
                    new Vector3(1f, -17.5f, 0f), //O2 tree hayball
                    new Vector3(3f, -21f, 0f), //O2 middle
                    new Vector3(2f, -19f, 0f), //O2 gas
                    new Vector3(1f, -24f, 0f), //O2 water
                    new Vector3(7f, -24f, 0f), //under O2
                    new Vector3(9f, -20f, 0f), //right outside of O2
                    new Vector3(7f, -15.8f, 0f), //snowman under elec
                    new Vector3(11f, -17f, 0f), //comms table
                    new Vector3(12.7f, -15.5f, 0f), //coms antenna pult
                    new Vector3(13f, -24.5f, 0f), //weapons window
                    new Vector3(15f, -17f, 0f), //between coms-office
                    new Vector3(17.5f, -25.7f, 0f), //snowman under office
                };

                List<Vector3> fungleSpawn = new List<Vector3>()
                {
                    new Vector3(-10.68f, 13.32f, 0.1f), //dropship top
                    new Vector3(-08.33f, 10.69f, 0.1f), //dropship bottom
                    new Vector3(-4.55f, -1.35f, 0f), //meeting room near button
                    new Vector3(3.19f, -1.27f, 0f), //meeting room weapons task
                    new Vector3(-2.85f, -11.56f, -0.1f), //outside lab hallway
                    new Vector3(-2.79f, -9.95f, -0.1f), //lab
                    new Vector3(14.37f, -16.32f, -0.2f), //jungle vent
                    new Vector3(-13.42f, -3.91f, 0f), //kitchen door
                    new Vector3(-13.19f, -9.22f, -0.1f), //kitchen cook fish task
                    new Vector3(-23.52f, -6.87f, -0.1f), //dock
                    new Vector3(-16.36f, -1.81f, 0f), //splash zone room
                    new Vector3(-19.92f, -02.04f, 0f), //splash zone 1
                    new Vector3(-18.08f, 3.07f, 0f), //splash zone 2
                    new Vector3(-14.21f, 7.48f, 0.1f), //caf water task
                    new Vector3(-17.40f, 5.05f, 0.1f), //caf trash task
                    new Vector3(02.92f, 1.91f, 0f), //below storage
                    new Vector3(02.73f, 4.24f, 0f), //storage water task
                    new Vector3(0.96f, 6.80f, 0.1f), //storage plant task
                    new Vector3(17.81f, 13.40f, 0.1f), //top zipline
                    new Vector3(23.12f, 14.39f, 0.1f), //comms
                    new Vector3(21.31f, 10.92f, 0.1f), //top comms ladder
                    new Vector3(20.00f, 7.42f, 0.1f), //bottom comms ladder (middle part)
                    new Vector3(15.48f, 3.58f, 0f), //mining pit exit (behind the gem)
                    new Vector3(11.81f, 10.58f, 0.1f), //mining pit
                    new Vector3(6.31f, 3.61f, 0f), //lookout
                    new Vector3(22.98f, -6.94f, -0.1f), //reactor
                    new Vector3(15.28f, -5.85f, -0.1f), //outside reactor
                    new Vector3(19.01f, -10.60f, -0.1f), //below reactor (small joke hehe)
                    new Vector3(15.63f, -14.59f, -0.1f), //nearby greenhouse
                    new Vector3(08.73f, -11.65f, -0.1f), //bottom greenhouse
                    new Vector3(08.99f, -9.58f, -0.1f), //top greenhouse
                    new Vector3(11.24f, -5.70f, -0.1f), //ladder outside greenhouse
                    new Vector3(11.38f, -0.72f, -0f), //top greenhouse ladder
                    new Vector3(15.99f, 0.78f, 0f), //below the big ass gem
                    new Vector3(0.66f, -6.02f, 0.1f), //middle path between greenhouse lab and meeting room
                    new Vector3(-5.85f, -14.51f, -0.1f), //lab hallway 2
                    new Vector3(-10.98f, -5.84f, -0.1f), //lab hall 3
                    new Vector3(-11.31f, 1.36f, 0f), //normal spawn lmao
                    new Vector3(-8.31f, 1.36f, 0f), //normal spawn 2 lmao
                    new Vector3(-9.84f, 7.28f, 0.1f), //a little bit up from normal spawn

                    // yes i manually did this and it hurt my brain
                };

                List<Vector3> dleksSpawn = new List<Vector3>()
                {
                    new Vector3(2.2f, 2.2f, 0f), //cafeteria. botton. top left.
                    new Vector3(-0.7f, 2.2f, 0f), //caffeteria. button. top right.
                    new Vector3(2.2f, -0.2f, 0f), //caffeteria. button. bottom left.
                    new Vector3(-0.7f, -0.2f, 0f), //caffeteria. button. bottom right.
                    new Vector3(-10.0f, 3.0f, 0f), //weapons top
                    new Vector3(-9.0f, 1.0f, 0f), //weapons bottom
                    new Vector3(-6.5f, -3.5f, 0f), //O2
                    new Vector3(-11.5f, -3.5f, 0f), //O2-nav hall
                    new Vector3(-17.0f, -3.5f, 0f), //navigation top
                    new Vector3(-18.2f, -5.7f, 0f), //navigation bottom
                    new Vector3(-11.5f, -6.5f, 0f), //nav-Shields top
                    new Vector3(-9.5f, -8.5f, 0f), //nav-Shields bottom
                    new Vector3(-9.2f, -12.2f, 0f), //Shields top
                    new Vector3(-8.0f, -14.3f, 0f), //Shields bottom
                    new Vector3(-2.5f, -16f, 0f), //coms left
                    new Vector3(-4.2f, -16.4f, 0f), //coms middle
                    new Vector3(-5.5f, -16f, 0f), //coms right
                    new Vector3(1.5f, -10.0f, 0f), //storage top
                    new Vector3(1.5f, -15.5f, 0f), //storage bottom
                    new Vector3(4.5f, -12.5f, 0f), //storrage left
                    new Vector3(-0.3f, -12.5f, 0f), //storrage right
                    new Vector3(-4.5f, -7.5f, 0f), //admin top
                    new Vector3(-4.5f, -9.5f, 0f), //admin bottom
                    new Vector3(9.0f, -8.0f, 0f), //elec top left
                    new Vector3(6.0f, -8.0f, 0f), //elec top right
                    new Vector3(8.0f, -11.0f, 0f), //elec bottom
                    new Vector3(12.0f, -13.0f, 0f), //elec-lower hall
                    new Vector3(17f, -10f, 0f), //lower engine top
                    new Vector3(17.0f, -13.0f, 0f), //lower engine bottom
                    new Vector3(21.5f, -3.0f, 0f), //reactor top
                    new Vector3(21.5f, -8.0f, 0f), //reactor bottom
                    new Vector3(13.0f, -3.0f, 0f), //security top
                    new Vector3(12.6f, -5.6f, 0f), // security bottom
                    new Vector3(17.0f, 2.5f, 0f), //upper engibe top
                    new Vector3(17.0f, -1.0f, 0f), //upper engine bottom
                    new Vector3(10.5f, 1.0f, 0f), //upper-mad hall
                    new Vector3(10.5f, -2.0f, 0f), //medbay top
                    new Vector3(6.5f, -4.5f, 0f) //medbay bottom
                };

                List<Vector3> airshipSpawn = new List<Vector3>() {}; //no spawns since it already has random spawns

                foreach (var player in PlayerControl.AllPlayerControls)
                {
                    if (player.Data.Disconnected || player.Data.IsDead)
                        continue;
                        
                    var map = GameOptionsManager.Instance.currentNormalGameOptions.MapId;

                    switch (map)
                    {
                        case 0:
                            player.transform.position = skeldSpawn[rnd.Next(skeldSpawn.Count)];
                            break;

                        case 1:
                            player.transform.position = miraSpawn[rnd.Next(miraSpawn.Count)];
                            break;

                        case 2:
                            player.transform.position = polusSpawn[rnd.Next(polusSpawn.Count)];
                            break;

                        case 3:
                            player.transform.position = dleksSpawn[rnd.Next(dleksSpawn.Count)];
                            break;

                        case 4:
                            player.transform.position = airshipSpawn[rnd.Next(airshipSpawn.Count)];
                            break;
                        
                        case 5:
                            player.transform.position = fungleSpawn[rnd.Next(fungleSpawn.Count)];
                            break;
                    }
                }
            }
        }
    }

    [HarmonyPatch]
    static class ExileControllerWrapUpPatch2
    {
        public static System.Random rnd = new System.Random((int)DateTime.Now.Ticks);

        [HarmonyPatch(typeof(ExileController), nameof(ExileController.WrapUp))]
        static class BaseExileControllerPatch
        {
            public static void Postfix(ExileController __instance)
            {
                WrapUpPostfix(__instance.initData.networkedPlayer);
            }
        }

        [HarmonyPatch(typeof(AirshipExileController), nameof(AirshipExileController.WrapUpAndSpawn))]
        static class AirshipExileControllerPatch
        {
            public static void Postfix(AirshipExileController __instance)
            {
                WrapUpPostfix(__instance.initData.networkedPlayer);
            }
        }

        static void WrapUpPostfix(NetworkedPlayerInfo exiled)
        {
            if (CustomOptionHolder.RandomSpawns.GetBool())
            {
                List<Vector3> skeldSpawn = new List<Vector3>()
                {
                    new Vector3(-2.2f, 2.2f, 0f), //cafeteria. botton. top left.
                    new Vector3(0.7f, 2.2f, 0f), //caffeteria. button. top right.
                    new Vector3(-2.2f, -0.2f, 0f), //caffeteria. button. bottom left.
                    new Vector3(0.7f, -0.2f, 0f), //caffeteria. button. bottom right.
                    new Vector3(10.0f, 3.0f, 0f), //weapons top
                    new Vector3(9.0f, 1.0f, 0f), //weapons bottom
                    new Vector3(6.5f, -3.5f, 0f), //O2
                    new Vector3(11.5f, -3.5f, 0f), //O2-nav hall
                    new Vector3(17.0f, -3.5f, 0f), //navigation top
                    new Vector3(18.2f, -5.7f, 0f), //navigation bottom
                    new Vector3(11.5f, -6.5f, 0f), //nav-Shields top
                    new Vector3(9.5f, -8.5f, 0f), //nav-Shields bottom
                    new Vector3(9.2f, -12.2f, 0f), //Shields top
                    new Vector3(8.0f, -14.3f, 0f), //Shields bottom
                    new Vector3(2.5f, -16f, 0f), //coms left
                    new Vector3(4.2f, -16.4f, 0f), //coms middle
                    new Vector3(5.5f, -16f, 0f), //coms right
                    new Vector3(-1.5f, -10.0f, 0f), //storage top
                    new Vector3(-1.5f, -15.5f, 0f), //storage bottom
                    new Vector3(-4.5f, -12.5f, 0f), //storrage left
                    new Vector3(0.3f, -12.5f, 0f), //storrage right
                    new Vector3(4.5f, -7.5f, 0f), //admin top
                    new Vector3(4.5f, -9.5f, 0f), //admin bottom
                    new Vector3(-9.0f, -8.0f, 0f), //elec top left
                    new Vector3(-6.0f, -8.0f, 0f), //elec top right
                    new Vector3(-8.0f, -11.0f, 0f), //elec bottom
                    new Vector3(-12.0f, -13.0f, 0f), //elec-lower hall
                    new Vector3(-17f, -10f, 0f), //lower engine top
                    new Vector3(-17.0f, -13.0f, 0f), //lower engine bottom
                    new Vector3(-21.5f, -3.0f, 0f), //reactor top
                    new Vector3(-21.5f, -8.0f, 0f), //reactor bottom
                    new Vector3(-13.0f, -3.0f, 0f), //security top
                    new Vector3(-12.6f, -5.6f, 0f), // security bottom
                    new Vector3(-17.0f, 2.5f, 0f), //upper engibe top
                    new Vector3(-17.0f, -1.0f, 0f), //upper engine bottom
                    new Vector3(-10.5f, 1.0f, 0f), //upper-mad hall
                    new Vector3(-10.5f, -2.0f, 0f), //medbay top
                    new Vector3(-6.5f, -4.5f, 0f) //medbay bottom
                };

                List<Vector3> miraSpawn = new List<Vector3>()
                {
                    new Vector3(-4.5f, 3.5f, 0f), //launchpad top
                    new Vector3(-4.5f, -1.4f, 0f), //launchpad bottom
                    new Vector3(8.5f, -1f, 0f), //launchpad- med hall
                    new Vector3(14f, -1.5f, 0f), //medbay
                    new Vector3(16.5f, 3f, 0f), // comms
                    new Vector3(10f, 5f, 0f), //lockers
                    new Vector3(6f, 1.5f, 0f), //locker room
                    new Vector3(2.5f, 13.6f, 0f), //reactor
                    new Vector3(6f, 12f, 0f), //reactor middle
                    new Vector3(9.5f, 13f, 0f), //lab
                    new Vector3(15f, 9f, 0f), //bottom left cross
                    new Vector3(17.9f, 11.5f, 0f), //middle cross
                    new Vector3(14f, 17.3f, 0f), //office
                    new Vector3(19.5f, 21f, 0f), //admin
                    new Vector3(14f, 24f, 0f), //greenhouse left
                    new Vector3(22f, 24f, 0f), //greenhouse right
                    new Vector3(21f, 8.5f, 0f), //bottom right cross
                    new Vector3(28f, 3f, 0f), //caf right
                    new Vector3(22f, 3f, 0f), //caf left
                    new Vector3(19f, 4f, 0f), //storage
                    new Vector3(22f, -2f, 0f), //balcony
                };

                List<Vector3> fungleSpawn = new List<Vector3>()
                {
                    new Vector3(-10.68f, 13.32f, 0.1f), //dropship top
                    new Vector3(-08.33f, 10.69f, 0.1f), //dropship bottom
                    new Vector3(-4.55f, -1.35f, 0f), //meeting room near button
                    new Vector3(3.19f, -1.27f, 0f), //meeting room weapons task
                    new Vector3(-2.85f, -11.56f, -0.1f), //outside lab hallway
                    new Vector3(-2.79f, -9.95f, -0.1f), //lab
                    new Vector3(14.37f, -16.32f, -0.2f), //jungle vent
                    new Vector3(-13.42f, -3.91f, 0f), //kitchen door
                    new Vector3(-13.19f, -9.22f, -0.1f), //kitchen cook fish task
                    new Vector3(-23.52f, -6.87f, -0.1f), //dock
                    new Vector3(-16.36f, -1.81f, 0f), //splash zone room
                    new Vector3(-19.92f, -02.04f, 0f), //splash zone 1
                    new Vector3(-18.08f, 3.07f, 0f), //splash zone 2
                    new Vector3(-14.21f, 7.48f, 0.1f), //caf water task
                    new Vector3(-17.40f, 5.05f, 0.1f), //caf trash task
                    new Vector3(02.92f, 1.91f, 0f), //below storage
                    new Vector3(02.73f, 4.24f, 0f), //storage water task
                    new Vector3(0.96f, 6.80f, 0.1f), //storage plant task
                    new Vector3(17.81f, 13.40f, 0.1f), //top zipline
                    new Vector3(23.12f, 14.39f, 0.1f), //comms
                    new Vector3(21.31f, 10.92f, 0.1f), //top comms ladder
                    new Vector3(20.00f, 7.42f, 0.1f), //bottom comms ladder (middle part)
                    new Vector3(15.48f, 3.58f, 0f), //mining pit exit (behind the gem)
                    new Vector3(11.81f, 10.58f, 0.1f), //mining pit
                    new Vector3(6.31f, 3.61f, 0f), //lookout
                    new Vector3(22.98f, -6.94f, -0.1f), //reactor
                    new Vector3(15.28f, -5.85f, -0.1f), //outside reactor
                    new Vector3(19.01f, -10.60f, -0.1f), //below reactor (small joke hehe)
                    new Vector3(15.63f, -14.59f, -0.1f), //nearby greenhouse
                    new Vector3(08.73f, -11.65f, -0.1f), //bottom greenhouse
                    new Vector3(08.99f, -9.58f, -0.1f), //top greenhouse
                    new Vector3(11.24f, -5.70f, -0.1f), //ladder outside greenhouse
                    new Vector3(11.38f, -0.72f, -0f), //top greenhouse ladder
                    new Vector3(15.99f, 0.78f, 0f), //below the big ass gem
                    new Vector3(0.66f, -6.02f, 0.1f), //middle path between greenhouse lab and meeting room
                    new Vector3(-5.85f, -14.51f, -0.1f), //lab hallway 2
                    new Vector3(-10.98f, -5.84f, -0.1f), //lab hall 3
                    new Vector3(-11.31f, 1.36f, 0f), //normal spawn lmao
                    new Vector3(-8.31f, 1.36f, 0f), //normal spawn 2 lmao
                    new Vector3(-9.84f, 7.28f, 0.1f), //a little bit up from normal spawn
                };

                List<Vector3> polusSpawn = new List<Vector3>()
                {
                    new Vector3(16.6f, -1f, 0f), //dropship top
                    new Vector3(16.6f, -5f, 0f), //dropship bottom
                    new Vector3(20f, -9f, 0f), //above storrage
                    new Vector3(22f, -7f, 0f), //right fuel
                    new Vector3(25.5f, -6.9f, 0f), //drill
                    new Vector3(29f, -9.5f, 0f), //lab lockers
                    new Vector3(29.5f, -8f, 0f), //lab weather notes
                    new Vector3(35f, -7.6f, 0f), //lab table
                    new Vector3(40.4f, -8f, 0f), //lab scan
                    new Vector3(33f, -10f, 0f), //lab toilet
                    new Vector3(39f, -15f, 0f), //specimen hall top
                    new Vector3(36.5f, -19.5f, 0f), //specimen top
                    new Vector3(36.5f, -21f, 0f), //specimen bottom
                    new Vector3(28f, -21f, 0f), //specimen hall bottom
                    new Vector3(24f, -20.5f, 0f), //admin tv
                    new Vector3(22f, -25f, 0f), //admin books
                    new Vector3(16.6f, -17.5f, 0f), //office coffe
                    new Vector3(22.5f, -16.5f, 0f), //office projector
                    new Vector3(24f, -17f, 0f), //office figure
                    new Vector3(27f, -16.5f, 0f), //office lifelines
                    new Vector3(32.7f, -15.7f, 0f), //lavapool
                    new Vector3(31.5f, -12f, 0f), //snowmad below lab
                    new Vector3(10f, -14f, 0f), //below storrage
                    new Vector3(21.5f, -12.5f, 0f), //storrage vent
                    new Vector3(19f, -11f, 0f), //storrage toolrack
                    new Vector3(12f, -7f, 0f), //left fuel
                    new Vector3(5f, -7.5f, 0f), //above elec
                    new Vector3(10f, -12f, 0f), //elec fence
                    new Vector3(9f, -9f, 0f), //elec lockers
                    new Vector3(5f, -9f, 0f), //elec window
                    new Vector3(4f, -11.2f, 0f), //elec tapes
                    new Vector3(5.5f, -16f, 0f), //elec-O2 hall
                    new Vector3(1f, -17.5f, 0f), //O2 tree hayball
                    new Vector3(3f, -21f, 0f), //O2 middle
                    new Vector3(2f, -19f, 0f), //O2 gas
                    new Vector3(1f, -24f, 0f), //O2 water
                    new Vector3(7f, -24f, 0f), //under O2
                    new Vector3(9f, -20f, 0f), //right outside of O2
                    new Vector3(7f, -15.8f, 0f), //snowman under elec
                    new Vector3(11f, -17f, 0f), //comms table
                    new Vector3(12.7f, -15.5f, 0f), //coms antenna pult
                    new Vector3(13f, -24.5f, 0f), //weapons window
                    new Vector3(15f, -17f, 0f), //between coms-office
                    new Vector3(17.5f, -25.7f, 0f), //snowman under office
                };

                List<Vector3> dleksSpawn = new List<Vector3>()
                {
                    new Vector3(2.2f, 2.2f, 0f), //cafeteria. botton. top left.
                    new Vector3(-0.7f, 2.2f, 0f), //caffeteria. button. top right.
                    new Vector3(2.2f, -0.2f, 0f), //caffeteria. button. bottom left.
                    new Vector3(-0.7f, -0.2f, 0f), //caffeteria. button. bottom right.
                    new Vector3(-10.0f, 3.0f, 0f), //weapons top
                    new Vector3(-9.0f, 1.0f, 0f), //weapons bottom
                    new Vector3(-6.5f, -3.5f, 0f), //O2
                    new Vector3(-11.5f, -3.5f, 0f), //O2-nav hall
                    new Vector3(-17.0f, -3.5f, 0f), //navigation top
                    new Vector3(-18.2f, -5.7f, 0f), //navigation bottom
                    new Vector3(-11.5f, -6.5f, 0f), //nav-Shields top
                    new Vector3(-9.5f, -8.5f, 0f), //nav-Shields bottom
                    new Vector3(-9.2f, -12.2f, 0f), //Shields top
                    new Vector3(-8.0f, -14.3f, 0f), //Shields bottom
                    new Vector3(-2.5f, -16f, 0f), //coms left
                    new Vector3(-4.2f, -16.4f, 0f), //coms middle
                    new Vector3(-5.5f, -16f, 0f), //coms right
                    new Vector3(1.5f, -10.0f, 0f), //storage top
                    new Vector3(1.5f, -15.5f, 0f), //storage bottom
                    new Vector3(4.5f, -12.5f, 0f), //storrage left
                    new Vector3(-0.3f, -12.5f, 0f), //storrage right
                    new Vector3(-4.5f, -7.5f, 0f), //admin top
                    new Vector3(-4.5f, -9.5f, 0f), //admin bottom
                    new Vector3(9.0f, -8.0f, 0f), //elec top left
                    new Vector3(6.0f, -8.0f, 0f), //elec top right
                    new Vector3(8.0f, -11.0f, 0f), //elec bottom
                    new Vector3(12.0f, -13.0f, 0f), //elec-lower hall
                    new Vector3(17f, -10f, 0f), //lower engine top
                    new Vector3(17.0f, -13.0f, 0f), //lower engine bottom
                    new Vector3(21.5f, -3.0f, 0f), //reactor top
                    new Vector3(21.5f, -8.0f, 0f), //reactor bottom
                    new Vector3(13.0f, -3.0f, 0f), //security top
                    new Vector3(12.6f, -5.6f, 0f), // security bottom
                    new Vector3(17.0f, 2.5f, 0f), //upper engibe top
                    new Vector3(17.0f, -1.0f, 0f), //upper engine bottom
                    new Vector3(10.5f, 1.0f, 0f), //upper-mad hall
                    new Vector3(10.5f, -2.0f, 0f), //medbay top
                    new Vector3(6.5f, -4.5f, 0f) //medbay bottom
                };

                List<Vector3> airshipSpawn = new List<Vector3>() {}; //no spawns since it already has random spawns

                foreach (var player in PlayerControl.AllPlayerControls)
                {
                    if (player.Data.Disconnected || player.Data.IsDead || Lazy.Players.FindAll(x => x.PlayerId == player.PlayerId).Count > 0)
                        continue;

                    var map = GameOptionsManager.Instance.currentNormalGameOptions.MapId;

                    switch (map)
                    {
                        case 0:
                            player.transform.position = skeldSpawn[rnd.Next(skeldSpawn.Count)];
                            break;

                        case 1:
                            player.transform.position = miraSpawn[rnd.Next(miraSpawn.Count)];
                            break;

                        case 2:
                            player.transform.position = polusSpawn[rnd.Next(polusSpawn.Count)];
                            break;

                        case 3:
                            player.transform.position = dleksSpawn[rnd.Next(dleksSpawn.Count)];
                            break;

                        case 4:
                            player.transform.position = airshipSpawn[rnd.Next(airshipSpawn.Count)];
                            break;
                        
                        case 5:
                            player.transform.position = fungleSpawn[rnd.Next(fungleSpawn.Count)];
                            break;
                    }
                }
            }
        }
    }
}