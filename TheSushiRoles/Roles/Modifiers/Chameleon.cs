using System.Collections.Generic;
using AmongUs.Data;
namespace TheSushiRoles.Roles.Modifiers
{
    public static class Chameleon 
    {
        public static List<PlayerControl> Players = new List<PlayerControl>();
        public static Color Color = new Color32(237, 221, 142, byte.MaxValue);
        public static Dictionary<byte, float> lastMoved;

        public static void ClearAndReload() 
        {
            Players = new List<PlayerControl>();
            lastMoved = new Dictionary<byte, float>();
        }

        public static float Visibility(byte playerId) 
        {
            float visibility = 1f;
            if (lastMoved != null && lastMoved.ContainsKey(playerId)) 
            {
                var tStill = Time.time - lastMoved[playerId];
                if (tStill > CustomGameOptions.ModifierChameleonHoldDuration) 
                {
                    if (tStill - CustomGameOptions.ModifierChameleonHoldDuration > CustomGameOptions.ModifierChameleonFadeDuration) visibility = CustomGameOptions.ModifierChameleonMinVisibility;
                    else visibility = (1 - (tStill - CustomGameOptions.ModifierChameleonHoldDuration) / CustomGameOptions.ModifierChameleonFadeDuration) * (1 - CustomGameOptions.ModifierChameleonMinVisibility) + CustomGameOptions.ModifierChameleonMinVisibility;
                }
            }
            if (PlayerControl.LocalPlayer.Data.IsDead && visibility < 0.1f)
            {  // Ghosts can always see!
                visibility = 0.1f;
            }
            return visibility;
        }

        public static void Update() 
        {
            foreach (var chameleonPlayer in Players) 
            {
                if (chameleonPlayer == Assassin.Player && Assassin.isInvisble) continue;  // Dont make Assassin visible...
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