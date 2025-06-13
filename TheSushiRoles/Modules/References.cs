namespace TheSushiRoles.Modules
{
    public static class References
    {
        public static Minigame TaskPanelInstance => Minigame.Instance;
        public static SoundManager SoundManagerInstance() => SoundManager.Instance;
        public static ExileController ExiledInstance() => ExileController.Instance;
        public static PlayerControl[] AllPlayerControls => PlayerControl.AllPlayerControls.ToArray();
        public static bool IsMira() => GameOptionsManager.Instance.CurrentGameOptions.MapId == 1;
        public static bool IsAirship() => GameOptionsManager.Instance.CurrentGameOptions.MapId == 4;
        public static bool IsSkeld() => GameOptionsManager.Instance.CurrentGameOptions.MapId == 0;
        public static bool IsDleks() => GameOptionsManager.Instance.CurrentGameOptions.MapId == 3;
        public static bool IsPolus() => GameOptionsManager.Instance.CurrentGameOptions.MapId == 2;
        public static bool IsFungle() => GameOptionsManager.Instance.CurrentGameOptions.MapId == 5;
        
    }
}