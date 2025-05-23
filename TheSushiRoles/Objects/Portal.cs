using System;
using UnityEngine;
using System.Collections.Generic;

namespace TheSushiRoles.Objects 
{
    public class Portal 
    {
        public static Portal firstPortal = null;
        public static Portal secondPortal = null;
        public static bool bothPlacedAndEnabled = false;
        public static Sprite[] portalFgAnimationSprites = new Sprite[205];
        public static Sprite portalSprite;
        public static bool isTeleporting = false;
        public static float teleportDuration = 3.4166666667f;
        public string room;

        public struct TpLogEntry 
        {
            public byte playerId;
            public string name;
            public DateTime time;
            public TpLogEntry(byte playerId, string name, DateTime time) {
                this.playerId = playerId;
                this.time = time;
                this.name = name;
            }
        }

        public static List<TpLogEntry> teleportedPlayers;

        public static Sprite GetFgAnimationSprite(int index) 
        {
            if (portalFgAnimationSprites == null || portalFgAnimationSprites.Length == 0) return null;
            index = Mathf.Clamp(index, 0, portalFgAnimationSprites.Length - 1);
            if (portalFgAnimationSprites[index] == null)
                portalFgAnimationSprites[index] = (Utils.LoadSpriteFromResources($"TheSushiRoles.Resources.PortalAnimation.portal_{(index):000}.png", 115f));
            return portalFgAnimationSprites[index];
        }

        public static void StartTeleport(byte playerId, byte exit) 
        {
            if (firstPortal == null || secondPortal == null) return;
            isTeleporting = true;
            
            // Generate log info
            PlayerControl playerControl = Utils.PlayerById(playerId);
            bool flip = playerControl.cosmetics.currentBodySprite.BodySprite.flipX; // use the original player control here, not the morhpTarget.
            firstPortal.animationFgRenderer.flipX = flip;
            secondPortal.animationFgRenderer.flipX = flip;
            if (Morphling.Player != null && Morphling.morphTimer > 0) playerControl = Morphling.morphTarget;  // Will output info of morph-target instead
            if (Glitch.Player != null && Glitch.MimicTimer > 0) playerControl = Glitch.MimicTarget;  // Will output info of Mimic-target instead
            if (Hitman.Player != null && Hitman.MorphTimer > 0) playerControl = Hitman.MorphTarget;  // Will output info of Morph-target instead
            string playerNameDisplay = Gatekeeper.logOnlyHasColors ? "A player (" + (Utils.IsLighterColor(playerControl) ? "L" : "D") + ")" : playerControl.Data.PlayerName;

            int colorId = playerControl.Data.DefaultOutfit.ColorId;

            if (Painter.PaintTimer > 0 || Utils.MushroomSabotageActive()) 
            {
                playerNameDisplay = "An anonymous player";
                colorId = 6;
            }
            
            if (!playerControl.Data.IsDead)
                teleportedPlayers.Add(new TpLogEntry(playerId, playerNameDisplay, DateTime.UtcNow));
            
            FastDestroyableSingleton<HudManager>.Instance.StartCoroutine(Effects.Lerp(teleportDuration, new Action<float>((p) => {
                if (firstPortal != null && firstPortal.animationFgRenderer != null && secondPortal != null && secondPortal.animationFgRenderer != null) {
                    if (exit == 0 || exit == 1) firstPortal.animationFgRenderer.sprite = GetFgAnimationSprite((int)(p * portalFgAnimationSprites.Length));
                    if (exit == 0 || exit == 2) secondPortal.animationFgRenderer.sprite = GetFgAnimationSprite((int)(p * portalFgAnimationSprites.Length));
                    playerControl.SetPlayerMaterialColors(firstPortal.animationFgRenderer);
                    playerControl.SetPlayerMaterialColors(secondPortal.animationFgRenderer);
                    if (p == 1f) {
                        firstPortal.animationFgRenderer.sprite = null;
                        secondPortal.animationFgRenderer.sprite = null;
                        isTeleporting = false;
                    }
                }
            })));
        }

        public GameObject portalFgAnimationGameObject;
        public GameObject portalGameObject;
        private SpriteRenderer animationFgRenderer;
        private SpriteRenderer portalRenderer;

        public Portal(Vector2 p) 
        {
            portalGameObject = new GameObject("Portal"){ layer = 11 };
            //Vector3 position = new Vector3(p.x, p.y, PlayerControl.LocalPlayer.transform.position.z + 1f);
            Vector3 position = new Vector3(p.x, p.y, p.y / 1000f + 0.01f);

            // Create the portal            
            portalGameObject.transform.position = position;
            portalGameObject.AddSubmergedComponent(SubmergedCompatibility.Classes.ElevatorMover);
            portalRenderer = portalGameObject.AddComponent<SpriteRenderer>();
            portalRenderer.sprite = portalSprite;

            Vector3 fgPosition = new Vector3(0, 0, -1f);
            portalFgAnimationGameObject = new GameObject("PortalAnimationFG");
            portalFgAnimationGameObject.transform.SetParent(portalGameObject.transform);
            portalFgAnimationGameObject.AddSubmergedComponent(SubmergedCompatibility.Classes.ElevatorMover);
            portalFgAnimationGameObject.transform.localPosition = fgPosition;
            animationFgRenderer = portalFgAnimationGameObject.AddComponent<SpriteRenderer>();
            animationFgRenderer.material = FastDestroyableSingleton<HatManager>.Instance.PlayerMaterial;

            // Only render the inactive portals for the Gatekeeper
            bool playerSees = PlayerControl.LocalPlayer == Gatekeeper.Player  || PlayerControl.LocalPlayer.Data.IsDead;
            portalGameObject.SetActive(playerSees);
            portalFgAnimationGameObject.SetActive(true);

            if (firstPortal == null) firstPortal = this;
            else if (secondPortal == null) 
            {
                secondPortal = this;
            }
            var lastRoom = FastDestroyableSingleton<HudManager>.Instance?.roomTracker?.LastRoom?.RoomId;
            this.room = lastRoom != null ? DestroyableSingleton<TranslationController>.Instance.GetString((SystemTypes)lastRoom) : "Open Field";
        }

        public static bool LocationNearEntry(Vector2 p) 
        {
            if (!bothPlacedAndEnabled) return false;
            float maxDist = 0.25f;

            var dist1 = Vector2.Distance(p, firstPortal.portalGameObject.transform.position);
            var dist2 = Vector2.Distance(p, secondPortal.portalGameObject.transform.position);
            if (dist1 > maxDist && dist2 > maxDist) return false;
            return true;
        }

        public static Vector2 FindExit(Vector2 p) 
        {
            var dist1 = Vector2.Distance(p, firstPortal.portalGameObject.transform.position);
            var dist2 = Vector2.Distance(p, secondPortal.portalGameObject.transform.position);
            return dist1 < dist2 ? secondPortal.portalGameObject.transform.position : firstPortal.portalGameObject.transform.position;
        }

        public static Vector2 FindEntry(Vector2 p) 
        {
            var dist1 = Vector2.Distance(p, firstPortal.portalGameObject.transform.position);
            var dist2 = Vector2.Distance(p, secondPortal.portalGameObject.transform.position);
            return dist1 > dist2 ? secondPortal.portalGameObject.transform.position : firstPortal.portalGameObject.transform.position;
        }

        public static void MeetingEndsUpdate() 
        {
            // checkAndEnable
            if (secondPortal != null) 
            {
                firstPortal.portalGameObject.SetActive(true);
                secondPortal.portalGameObject.SetActive(true);
                bothPlacedAndEnabled = true;
                HudManagerStartPatch.GatekeeperButtonText2.text = "2. " + secondPortal.room;
            }

            // reset teleported players
            teleportedPlayers = new List<TpLogEntry>();
        }

        private static void PreloadSprites() 
        {
            for (int i = 0; i < portalFgAnimationSprites.Length; i++) 
            {
                GetFgAnimationSprite(i);
            }
            portalSprite = Utils.LoadSpriteFromResources("TheSushiRoles.Resources.PortalAnimation.plattform.png", 115f);
        }

        public static void ClearPortals() 
        {
            PreloadSprites();  // Force preload of sprites to avoid lag
            bothPlacedAndEnabled = false;
            firstPortal = null;
            secondPortal = null;
            isTeleporting = false;
            teleportedPlayers = new List<TpLogEntry>();
        }

    }

}
