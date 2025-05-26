using UnityEngine;

namespace TheSushiRoles.Roles
{
    public static class Hacker 
    {
        public static PlayerControl Player;
        public static Minigame vitals = null;
        public static Minigame doorLog = null;
        public static Color Color = new Color32(117, 250, 76, byte.MaxValue);
        public static float Cooldown = 30f;
        public static float Duration = 10f;
        public static float toolsNumber = 5f;
        public static bool onlyColorType = false;
        public static float hackerTimer = 0f;
        public static int RechargeTasksNumber = 2;
        public static int RechargedTasks = 2;
        public static int chargesVitals = 1;
        public static int chargesAdminTable = 1;
        private static Sprite ButtonSprite;
        private static Sprite vitalsSprite;
        private static Sprite logSprite;
        private static Sprite adminSprite;

        public static Sprite GetButtonSprite() 
        {
            if (ButtonSprite) return ButtonSprite;
            ButtonSprite = Utils.LoadSprite("TheSushiRoles.Resources.HackerButton.png", 115f);
            return ButtonSprite;
        }

        public static Sprite GetVitalsSprite() 
        {
            if (vitalsSprite) return vitalsSprite;
            vitalsSprite = FastDestroyableSingleton<HudManager>.Instance.UseButton.fastUseSettings[ImageNames.VitalsButton].Image;
            return vitalsSprite;
        }

        public static Sprite GetLogSprite() 
        {
            if (logSprite) return logSprite;
            logSprite = FastDestroyableSingleton<HudManager>.Instance.UseButton.fastUseSettings[ImageNames.DoorLogsButton].Image;
            return logSprite;
        }

        public static Sprite GetAdminSprite() 
        {
            byte mapId = GameOptionsManager.Instance.currentNormalGameOptions.MapId;
            UseButtonSettings button = FastDestroyableSingleton<HudManager>.Instance.UseButton.fastUseSettings[ImageNames.PolusAdminButton]; // Polus
            if (Utils.IsSkeld() || mapId == 3) button = FastDestroyableSingleton<HudManager>.Instance.UseButton.fastUseSettings[ImageNames.AdminMapButton]; // Skeld || Dleks
            else if (Utils.IsMira()) button = FastDestroyableSingleton<HudManager>.Instance.UseButton.fastUseSettings[ImageNames.MIRAAdminButton]; // Mira HQ
            else if (Utils.IsAirship()) button = FastDestroyableSingleton<HudManager>.Instance.UseButton.fastUseSettings[ImageNames.AirshipAdminButton]; // Airship
            else if (Utils.IsFungle()) button = FastDestroyableSingleton<HudManager>.Instance.UseButton.fastUseSettings[ImageNames.AdminMapButton];  // Hacker can Access the Admin panel on Fungle
            adminSprite = button.Image;
            return adminSprite;
        }

        public static void ClearAndReload() 
        {
            Player = null;
            vitals = null;
            doorLog = null;
            hackerTimer = 0f;
            adminSprite = null;
            Cooldown = CustomOptionHolder.hackerCooldown.GetFloat();
            Duration = CustomOptionHolder.hackerHackeringDuration.GetFloat();
            onlyColorType = CustomOptionHolder.hackerOnlyColorType.GetBool();
            toolsNumber = CustomOptionHolder.hackerToolsNumber.GetFloat();
            RechargeTasksNumber = Mathf.RoundToInt(CustomOptionHolder.hackerRechargeTasksNumber.GetFloat());
            RechargedTasks = Mathf.RoundToInt(CustomOptionHolder.hackerRechargeTasksNumber.GetFloat());
            chargesVitals = Mathf.RoundToInt(CustomOptionHolder.hackerToolsNumber.GetFloat()) / 2;
            chargesAdminTable = Mathf.RoundToInt(CustomOptionHolder.hackerToolsNumber.GetFloat()) / 2;
        }
    }
}