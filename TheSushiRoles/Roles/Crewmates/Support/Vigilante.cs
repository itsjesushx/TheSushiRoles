namespace TheSushiRoles.Roles
{
    public static class Vigilante 
    {
        public static PlayerControl Player;
        public static Color Color = new Color32(195, 178, 95, byte.MaxValue);
        public static int remainingScrews = 7;
        public static int totalScrews = 7;
        public static int ventPrice = 1;
        public static int camPrice = 2;
        public static int placedCameras = 0;
        public static int maxCharges = 5;
        public static int RechargeTasksNumber = 3;
        public static int RechargedTasks = 3;
        public static int Charges = 1;
        public static Vent ventTarget = null;
        public static Minigame minigame = null;

        private static Sprite closeVentButtonSprite;
        public static Sprite GetCloseVentButtonSprite() 
        {
            if (closeVentButtonSprite) return closeVentButtonSprite;
            closeVentButtonSprite = Utils.LoadSprite("TheSushiRoles.Resources.CloseVentButton.png", 115f);
            return closeVentButtonSprite;
        }

        private static Sprite placeCameraButtonSprite;
        public static Sprite GetPlaceCameraButtonSprite() 
        {
            if (placeCameraButtonSprite) return placeCameraButtonSprite;
            placeCameraButtonSprite = Utils.LoadSprite("TheSushiRoles.Resources.PlaceCameraButton.png", 115f);
            return placeCameraButtonSprite;
        }

        private static Sprite animatedVentSealedSprite;
        private static float lastPPU;
        public static Sprite GetAnimatedVentSealedSprite() 
        {
            float ppu = 185f;
            if (SubmergedCompatibility.IsSubmerged) ppu = 120f;
            if (lastPPU != ppu) 
            {
                animatedVentSealedSprite = null;
                lastPPU = ppu;
            }
            if (animatedVentSealedSprite) return animatedVentSealedSprite;
            animatedVentSealedSprite = Utils.LoadSprite("TheSushiRoles.Resources.AnimatedVentSealed.png", ppu);
            return animatedVentSealedSprite;
        }

        private static Sprite camSprite;
        public static Sprite GetCamSprite() 
        {
            if (camSprite) return camSprite;
            camSprite = FastDestroyableSingleton<HudManager>.Instance.UseButton.fastUseSettings[ImageNames.CamsButton].Image;
            return camSprite;
        }

        private static Sprite logSprite;
        public static Sprite GetLogSprite() 
        {
            if (logSprite) return logSprite;
            logSprite = FastDestroyableSingleton<HudManager>.Instance.UseButton.fastUseSettings[ImageNames.DoorLogsButton].Image;
            return logSprite;
        }

        public static void ClearAndReload() 
        {
            Player = null;
            ventTarget = null;
            minigame = null;
            maxCharges = CustomGameOptions.VigilanteCamMaxCharges;
            RechargeTasksNumber = CustomGameOptions.VigilanteCamRechargeTasksNumber;
            RechargedTasks = CustomGameOptions.VigilanteCamRechargeTasksNumber;
            Charges = CustomGameOptions.VigilanteCamMaxCharges /2;
            placedCameras = 0;
            totalScrews = remainingScrews = CustomGameOptions.VigilanteTotalScrews;
            camPrice = CustomGameOptions.VigilanteCamPrice;
            ventPrice = CustomGameOptions.VigilanteVentPrice;
        }
    }
}