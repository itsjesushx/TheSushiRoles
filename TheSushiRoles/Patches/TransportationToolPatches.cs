namespace TheSushiRoles.Patches 
{
    [HarmonyPatch]
    public static class TransportationToolPatches 
    {
        /* 
         * Moving Plattform / Zipline / Ladders move the player out of bounds, thus we want to disable functions of the mod if the player is currently using one of these.
         * Save the players Lazy position before using it.
        */
       
        public static bool IsUsingTransportation(PlayerControl pc) => pc.inMovingPlat || pc.onLadder;

        // Zipline:
        [HarmonyPrefix]
        [HarmonyPatch(typeof(ZiplineBehaviour), nameof(ZiplineBehaviour.Use), new Type[] {typeof(PlayerControl), typeof(bool)})]
        public static void prefix3(ZiplineBehaviour __instance, PlayerControl player, bool fromTop) 
        {
            Lazy.position = PlayerControl.LocalPlayer.transform.position;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(ZiplineBehaviour), nameof(ZiplineBehaviour.Use), new Type[] { typeof(PlayerControl), typeof(bool) })]
        public static void Postfix(ZiplineBehaviour __instance, PlayerControl player, bool fromTop) 
        {
            // Fix camo:
            __instance.StartCoroutine(Effects.Lerp(fromTop ? __instance.downTravelTime : __instance.upTravelTime, new System.Action<float>((p) => {
                HandZiplinePoolable hand;
                __instance.playerIdHands.TryGetValue(player.PlayerId, out hand);
                if (hand != null) 
                {
                    if (Painter.PaintTimer <= 0 && !Utils.MushroomSabotageActive()) 
                    {
                        if (player == Morphling.Player && Morphling.morphTimer > 0) 
                        {
                            hand.SetPlayerColor(Morphling.morphTarget.CurrentOutfit, PlayerMaterial.MaskType.None, 1f);
                            // Also set hat color, cause the line destroys it...
                            player.RawSetHat(Morphling.morphTarget.Data.DefaultOutfit.HatId, Morphling.morphTarget.Data.DefaultOutfit.ColorId);
                        }
                        if (player == Glitch.Player && Glitch.MimicTimer > 0) 
                        {
                            hand.SetPlayerColor(Glitch.MimicTarget.CurrentOutfit, PlayerMaterial.MaskType.None, 1f);
                            // Also set hat color, cause the line destroys it...
                            player.RawSetHat(Glitch.MimicTarget.Data.DefaultOutfit.HatId, Glitch.MimicTarget.Data.DefaultOutfit.ColorId);
                        }
                        if (player == Hitman.Player && Hitman.MorphTimer > 0)
                        {
                            hand.SetPlayerColor(Hitman.MorphTarget.CurrentOutfit, PlayerMaterial.MaskType.None, 1f);
                            // Also set hat color, cause the line destroys it...
                            player.RawSetHat(Hitman.MorphTarget.Data.DefaultOutfit.HatId, Hitman.MorphTarget.Data.DefaultOutfit.ColorId);
                        }
                        else
                        {
                            hand.SetPlayerColor(player.CurrentOutfit, PlayerMaterial.MaskType.None, 1f);
                        }
                    } 
                    else 
                    {
                        PlayerMaterial.SetColors(6, hand.handRenderer);
                    }
                }
            })));
        }

        // Save the position of the player prior to starting the climb / gap platform
        [HarmonyPrefix]
        [HarmonyPatch(typeof(PlayerPhysics), nameof(PlayerPhysics.ClimbLadder))]
        public static void Prefix() 
        {
            Lazy.position = PlayerControl.LocalPlayer.transform.position;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(PlayerPhysics), nameof(PlayerPhysics.ClimbLadder))]
        public static void Postfix2(PlayerPhysics __instance, Ladder source, byte climbLadderSid) 
        {
            // Fix camo:
            var player = __instance.myPlayer;
            __instance.StartCoroutine(Effects.Lerp(5.0f, new System.Action<float>((p) => 
            {
                if (Painter.PaintTimer <= 0 && !Utils.MushroomSabotageActive() && player == Morphling.Player && Morphling.morphTimer > 0.1f) 
                {
                    player.RawSetHat(Morphling.morphTarget.Data.DefaultOutfit.HatId, Morphling.morphTarget.Data.DefaultOutfit.ColorId);
                }
                else if (Painter.PaintTimer <= 0 && !Utils.MushroomSabotageActive() && player == Glitch.Player && Glitch.MimicTimer > 0.1f) 
                {
                    player.RawSetHat(Glitch.MimicTarget.Data.DefaultOutfit.HatId, Glitch.MimicTarget.Data.DefaultOutfit.ColorId);
                }
                else if (Painter.PaintTimer <= 0 && !Utils.MushroomSabotageActive() && player == Hitman.Player && Hitman.MorphTimer > 0.1f) 
                {
                    player.RawSetHat(Hitman.MorphTarget.Data.DefaultOutfit.HatId, Hitman.MorphTarget.Data.DefaultOutfit.ColorId);
                }
            })));
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(MovingPlatformBehaviour), nameof(MovingPlatformBehaviour.UsePlatform))]
        public static void Prefix2() 
        {
            Lazy.position = PlayerControl.LocalPlayer.transform.position;
        }
    }
    
    [HarmonyPatch(typeof(PlayerPhysics), nameof(PlayerPhysics.ClimbLadder))]
    public class SaveLadderPlayer
    {
        public static void Prefix(PlayerPhysics __instance, [HarmonyArgument(0)] Ladder source, [HarmonyArgument(1)] byte climbLadderSid)
        {
            if (PlayerControl.LocalPlayer == Landlord.Player)
                Landlord.UnteleportablePlayers.Add(__instance.myPlayer.PlayerId, DateTime.UtcNow);                
        }
    }

    [HarmonyPatch(typeof(MovingPlatformBehaviour), nameof(MovingPlatformBehaviour.Use), new Type[] { })]
    public class SavePlatformPlayer
    {
        public static void Prefix(MovingPlatformBehaviour __instance)
        {
            if (PlayerControl.LocalPlayer == Landlord.Player)
            {
                Landlord.UnteleportablePlayers.Add(PlayerControl.LocalPlayer.PlayerId, DateTime.UtcNow);
            }
            else
            {
                Utils.SendRPC(CustomRPC.SetUnteleportable, PlayerControl.LocalPlayer.PlayerId);
            }
        }
    }
}
