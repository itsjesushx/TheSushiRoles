using UnityEngine;

namespace TheSushiRoles.Roles
{
    public static class Vigilante 
    {
        public static PlayerControl Player;
        public static Color Color = new Color32(195, 178, 95, byte.MaxValue);
        public static float Cooldown = 30f;
        public static int remainingScrews = 7;
        public static int totalScrews = 7;
        public static int ventPrice = 1;
        public static int camPrice = 2;
        public static int placedCameras = 0;
        public static float Duration = 10f;
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
            closeVentButtonSprite = Utils.LoadSpriteFromResources("TheSushiRoles.Resources.CloseVentButton.png", 115f);
            return closeVentButtonSprite;
        }

        private static Sprite placeCameraButtonSprite;
        public static Sprite GetPlaceCameraButtonSprite() 
        {
            if (placeCameraButtonSprite) return placeCameraButtonSprite;
            placeCameraButtonSprite = Utils.LoadSpriteFromResources("TheSushiRoles.Resources.PlaceCameraButton.png", 115f);
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
            animatedVentSealedSprite = Utils.LoadSpriteFromResources("TheSushiRoles.Resources.AnimatedVentSealed.png", ppu);
            return animatedVentSealedSprite;
        }

        private static Sprite staticVentSealedSprite;
        public static Sprite GetStaticVentSealedSprite() 
        {
            if (staticVentSealedSprite) return staticVentSealedSprite;
            staticVentSealedSprite = Utils.LoadSpriteFromResources("TheSushiRoles.Resources.StaticVentSealed.png", 160f);
            return staticVentSealedSprite;
        }

        private static Sprite fungleVentSealedSprite;
        public static Sprite GetFungleVentSealedSprite() 
        {
            if (fungleVentSealedSprite) return fungleVentSealedSprite;
            fungleVentSealedSprite = Utils.LoadSpriteFromResources("TheSushiRoles.Resources.FungleVentSealed.png", 160f);
            return fungleVentSealedSprite;
        }


        private static Sprite submergedCentralUpperVentSealedSprite;
        public static Sprite GetSubmergedCentralUpperSealedSprite() 
        {
            if (submergedCentralUpperVentSealedSprite) return submergedCentralUpperVentSealedSprite;
            submergedCentralUpperVentSealedSprite = Utils.LoadSpriteFromResources("TheSushiRoles.Resources.CentralUpperBlocked.png", 145f);
            return submergedCentralUpperVentSealedSprite;
        }

        private static Sprite submergedCentralLowerVentSealedSprite;
        public static Sprite GetSubmergedCentralLowerSealedSprite() 
        {
            if (submergedCentralLowerVentSealedSprite) return submergedCentralLowerVentSealedSprite;
            submergedCentralLowerVentSealedSprite = Utils.LoadSpriteFromResources("TheSushiRoles.Resources.CentralLowerBlocked.png", 145f);
            return submergedCentralLowerVentSealedSprite;
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
            Duration = CustomOptionHolder.VigilanteCamDuration.GetFloat();
            maxCharges = Mathf.RoundToInt(CustomOptionHolder.VigilanteCamMaxCharges.GetFloat());
            RechargeTasksNumber = Mathf.RoundToInt(CustomOptionHolder.VigilanteCamRechargeTasksNumber.GetFloat());
            RechargedTasks = Mathf.RoundToInt(CustomOptionHolder.VigilanteCamRechargeTasksNumber.GetFloat());
            Charges = Mathf.RoundToInt(CustomOptionHolder.VigilanteCamMaxCharges.GetFloat()) /2;
            placedCameras = 0;
            Cooldown = CustomOptionHolder.VigilanteCooldown.GetFloat();
            totalScrews = remainingScrews = Mathf.RoundToInt(CustomOptionHolder.VigilanteTotalScrews.GetFloat());
            camPrice = Mathf.RoundToInt(CustomOptionHolder.VigilanteCamPrice.GetFloat());
            ventPrice = Mathf.RoundToInt(CustomOptionHolder.VigilanteVentPrice.GetFloat());
        }
    }
}