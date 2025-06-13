namespace TheSushiRoles.Roles
{
    public static class Warlock 
    {
        public static PlayerControl Player;
        public static Color Color = Palette.ImpostorRed;

        public static PlayerControl CurrentTarget;
        public static PlayerControl curseVictim;
        public static PlayerControl curseVictimTarget;

        public static void ClearAndReload() 
        {
            Player = null;
            CurrentTarget = null;
            curseVictim = null;
            curseVictimTarget = null;
        }

        public static void ResetCurse() 
        {
            CustomButtonLoader.warlockCurseButton.Timer = CustomButtonLoader.warlockCurseButton.MaxTimer;
            CustomButtonLoader.warlockCurseButton.Sprite = Utils.GetSprite("CurseButton");
            CustomButtonLoader.warlockCurseButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
            CurrentTarget = null;
            curseVictim = null;
            curseVictimTarget = null;
        }
    }
}