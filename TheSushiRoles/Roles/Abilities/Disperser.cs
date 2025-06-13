namespace TheSushiRoles.Roles.Abilities
{
    public static class Disperser
    {
        public static PlayerControl Player;
        public static int Charges;

        public static int RechargeKillsCount;
        public static Sprite ButtonSprite;
        public static Sprite GetButtonSprite()
        {
            if (ButtonSprite) return ButtonSprite;
            ButtonSprite = Utils.LoadSprite("TheSushiRoles.Resources.DisperseButton.png", 135f);
            return ButtonSprite;
        }
        public static void ClearAndReload()
        {
            Player = null;
            Charges = CustomGameOptions.ModifierDisperserCharges;
            RechargeKillsCount = CustomGameOptions.ModifierDisperserKillCharges;
        }
    }
}