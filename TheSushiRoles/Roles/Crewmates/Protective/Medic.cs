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

        public static ShieldOptions showShielded;
        public static NotificationOptions ShowMurderAttempt;
        public enum ShieldOptions
        {
            ShieldedAndMedic = 0,
            Medic = 1,
            Shielded = 2
        }
        public enum NotificationOptions
        {
            Medic = 0,
            Shielded = 1,
            Nobody = 2
        }
        public static bool setShieldAfterMeeting = false;
        public static bool showShieldAfterMeeting = false;
        public static bool meetingAfterShielding = false;

        public static Color ShieldedColor = new Color32(0, 221, 255, byte.MaxValue);
        public static PlayerControl CurrentTarget;

        private static Sprite ButtonSprite;
        public static Sprite GetButtonSprite() 
        {
            if (ButtonSprite) return ButtonSprite;
            ButtonSprite = Utils.LoadSprite("TheSushiRoles.Resources.ShieldButton.png", 115f);
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
                    || (Medic.showShielded == ShieldOptions.ShieldedAndMedic && (PlayerControl.LocalPlayer == Medic.Shielded || PlayerControl.LocalPlayer == Medic.Player)) // Shielded + Medic
                    || (Medic.showShielded == ShieldOptions.Medic && PlayerControl.LocalPlayer == Medic.Player) // Medic only
                    || (Medic.showShielded == ShieldOptions.Shielded && PlayerControl.LocalPlayer == Medic.Shielded); // Shielded only
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
            showShielded = (ShieldOptions)CustomOptionHolder.medicShowShielded.GetSelection();
            ShowMurderAttempt = (NotificationOptions)CustomOptionHolder.medicShowAttemptToMedic.GetSelection();
            setShieldAfterMeeting = CustomOptionHolder.medicSetOrShowShieldAfterMeeting.GetSelection() == 2;
            showShieldAfterMeeting = CustomOptionHolder.medicSetOrShowShieldAfterMeeting.GetSelection() == 1;
            meetingAfterShielding = false;
        }
    }
}