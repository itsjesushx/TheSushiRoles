using System.Collections.Generic;
using AmongUs.Data;
using UnityEngine;

namespace TheSushiRoles.Roles.Modifiers
{
    public static class Chameleon 
    {
        public static List<PlayerControl> Players = new List<PlayerControl>();
        public static float minVisibility = 0.2f;
        public static float holdDuration = 1f;
        public static float fadeDuration = 0.5f;
        public static Dictionary<byte, float> lastMoved;

        public static void ClearAndReload() 
        {
            Players = new List<PlayerControl>();
            lastMoved = new Dictionary<byte, float>();
            holdDuration = CustomOptionHolder.modifierChameleonHoldDuration.GetFloat();
            fadeDuration = CustomOptionHolder.modifierChameleonFadeDuration.GetFloat();
            minVisibility = CustomOptionHolder.modifierChameleonMinVisibility.GetSelection() / 10f;
        }

        public static float Visibility(byte playerId) 
        {
            float visibility = 1f;
            if (lastMoved != null && lastMoved.ContainsKey(playerId)) 
            {
                var tStill = Time.time - lastMoved[playerId];
                if (tStill > holdDuration) 
                {
                    if (tStill - holdDuration > fadeDuration) visibility = minVisibility;
                    else visibility = (1 - (tStill - holdDuration) / fadeDuration) * (1 - minVisibility) + minVisibility;
                }
            }
            if (PlayerControl.LocalPlayer.Data.IsDead && visibility < 0.1f) {  // Ghosts can always see!
                visibility = 0.1f;
            }
            return visibility;
        }

        public static void Update() 
        {
            foreach (var chameleonPlayer in Players) 
            {
                if (chameleonPlayer == Ninja.Player && Ninja.isInvisble) continue;  // Dont make Ninja visible...
                if (chameleonPlayer == Wraith.Player && Wraith.IsVanished) continue;  // Dont make Wraith visible...
                // check movement by animation
                PlayerPhysics playerPhysics = chameleonPlayer.MyPhysics;
                var currentPhysicsAnim = playerPhysics.Animations.Animator.GetCurrentAnimation();
                if (currentPhysicsAnim != playerPhysics.Animations.group.IdleAnim) {
                    lastMoved[chameleonPlayer.PlayerId] = Time.time;
                }
                // calculate and set visibility
                float visibility = Chameleon.Visibility(chameleonPlayer.PlayerId);
                float petVisibility = visibility;
                if (chameleonPlayer.Data.IsDead) 
                {
                    visibility = 0.5f;
                    petVisibility = 1f;
                }

                try 
                {  // Sometimes renderers are missing for weird reasons. Try catch to avoid exceptions
                    chameleonPlayer.cosmetics.currentBodySprite.BodySprite.color = chameleonPlayer.cosmetics.currentBodySprite.BodySprite.color.SetAlpha(visibility);
                    if (DataManager.Settings.Accessibility.ColorBlindMode) chameleonPlayer.cosmetics.colorBlindText.color = chameleonPlayer.cosmetics.colorBlindText.color.SetAlpha(visibility);
                    chameleonPlayer.SetHatAndVisorAlpha(visibility);
                    chameleonPlayer.cosmetics.skin.layer.color = chameleonPlayer.cosmetics.skin.layer.color.SetAlpha(visibility);
                    chameleonPlayer.cosmetics.nameText.color = chameleonPlayer.cosmetics.nameText.color.SetAlpha(visibility);
                    foreach (var rend in chameleonPlayer.cosmetics.currentPet.renderers)
                        rend.color = rend.color.SetAlpha(petVisibility);
                    foreach (var shadowRend in chameleonPlayer.cosmetics.currentPet.shadows)
                        shadowRend.color = shadowRend.color.SetAlpha(petVisibility);
                } 
                catch { }
            }
                
        }
    }
}