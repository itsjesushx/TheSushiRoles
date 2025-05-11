using UnityEngine;

namespace TheSushiRoles.Roles
{
    public static class Medic 
    {
        public static PlayerControl Player;
        public static PlayerControl Shielded;
        public static PlayerControl futureShielded;
        
        public static Color Color = new Color32(126, 251, 194, byte.MaxValue);
        public static bool usedShield;

        public static int showShielded = 0;
        public static bool showAttemptToShielded = false;
        public static bool showAttemptToMedic = false;
        public static bool setShieldAfterMeeting = false;
        public static bool showShieldAfterMeeting = false;
        public static bool meetingAfterShielding = false;

        public static Color ShieldedColor = new Color32(0, 221, 255, byte.MaxValue);
        public static PlayerControl CurrentTarget;

        private static Sprite ButtonSprite;
        public static Sprite GetButtonSprite() 
        {
            if (ButtonSprite) return ButtonSprite;
            ButtonSprite = Utils.LoadSpriteFromResources("TheSushiRoles.Resources.ShieldButton.png", 115f);
            return ButtonSprite;
        }

        public static bool ShieldVisible(PlayerControl target) 
        {
            bool hasVisibleShield = false;

            bool isMorphedMorphling = target == Morphling.Player && Morphling.morphTarget != null && Morphling.morphTimer > 0f;
            bool isMimicGlitch = target == Glitch.Player && Glitch.MimicTarget != null && Glitch.MimicTimer > 0f;
            if (Medic.Shielded != null && ((target == Medic.Shielded && !isMorphedMorphling) || (target == Medic.Shielded && !isMimicGlitch) || (isMorphedMorphling && Morphling.morphTarget == Medic.Shielded) || (isMimicGlitch && Glitch.MimicTarget == Medic.Shielded))) 
            {
                hasVisibleShield =  Utils.ShouldShowGhostInfo() // Ghost info
                    || (Medic.showShielded == 0 && (PlayerControl.LocalPlayer == Medic.Shielded || PlayerControl.LocalPlayer == Medic.Player)) // Shielded + Medic
                    || (Medic.showShielded == 1 && PlayerControl.LocalPlayer == Medic.Player) // Medic only
                    || (Medic.showShielded == 2 && PlayerControl.LocalPlayer == Medic.Shielded); // Shielded only
                // Make Shield invisible till after the next meeting if the option is set (the medic can already see the Shield)
                hasVisibleShield = hasVisibleShield && (Medic.meetingAfterShielding || !Medic.showShieldAfterMeeting || PlayerControl.LocalPlayer == Medic.Player || Utils.ShouldShowGhostInfo());
            }
            return hasVisibleShield;
        }

        public static void ClearAndReload() 
        {
            Player = null;
            Shielded = null;
            futureShielded = null;
            CurrentTarget = null;
            usedShield = false;
            showShielded = CustomOptionHolder.medicShowShielded.GetSelection();
            showAttemptToShielded = CustomOptionHolder.medicShowAttemptToShielded.GetBool();
            showAttemptToMedic = CustomOptionHolder.medicShowAttemptToMedic.GetBool();
            setShieldAfterMeeting = CustomOptionHolder.medicSetOrShowShieldAfterMeeting.GetSelection() == 2;
            showShieldAfterMeeting = CustomOptionHolder.medicSetOrShowShieldAfterMeeting.GetSelection() == 1;
            meetingAfterShielding = false;
        }
    }
}