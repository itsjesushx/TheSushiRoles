namespace TheSushiRoles.Roles
{
    public static class Hacker 
    {
        public static PlayerControl Player;
        public static Minigame vitals = null;
        public static Minigame doorLog = null;
        public static Color Color = new Color32(117, 250, 76, byte.MaxValue);
        public static int RechargeTasksNumber = 2;
        public static float hackerTimer = 0f;
        public static int RechargedTasks = 2;
        public static int chargesVitals = 1;
        public static float toolsNumber = 5f;
        public static int chargesAdminTable = 1;
        private static Sprite vitalsSprite;
        private static Sprite logSprite;
        private static Sprite adminSprite;

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
            UseButtonSettings button = FastDestroyableSingleton<HudManager>.Instance.UseButton.fastUseSettings[ImageNames.PolusAdminButton]; // Polus
            if (IsSkeld() || IsDleks()) button = FastDestroyableSingleton<HudManager>.Instance.UseButton.fastUseSettings[ImageNames.AdminMapButton]; // Skeld || Dleks
            else if (IsMira()) button = FastDestroyableSingleton<HudManager>.Instance.UseButton.fastUseSettings[ImageNames.MIRAAdminButton]; // Mira HQ
            else if (IsAirship()) button = FastDestroyableSingleton<HudManager>.Instance.UseButton.fastUseSettings[ImageNames.AirshipAdminButton]; // Airship
            else if (IsFungle()) button = FastDestroyableSingleton<HudManager>.Instance.UseButton.fastUseSettings[ImageNames.AdminMapButton];  // Hacker can Access the Admin panel on Fungle
            adminSprite = button.Image;
            return adminSprite;
        }

        public static void ClearAndReload() 
        {
            Player = null;
            vitals = null;
            doorLog = null;
            toolsNumber = CustomGameOptions.HackerToolsNumber;
            hackerTimer = 0f;
            adminSprite = null;
            RechargeTasksNumber = CustomGameOptions.HackerRechargeTasksNumber;
            RechargedTasks = CustomGameOptions.HackerRechargeTasksNumber;
            chargesVitals = CustomGameOptions.HackerToolsNumber / 2;
            chargesAdminTable = CustomGameOptions.HackerToolsNumber / 2;
        }
    }
}