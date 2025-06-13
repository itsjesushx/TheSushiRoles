namespace TheSushiRoles.Roles
{
    public static class Assassin 
    {
        public static PlayerControl Player;
        public static Color Color = Palette.ImpostorRed;
        public static PlayerControl AssassinMarked;
        public static PlayerControl CurrentTarget;

        public static float invisibleTimer = 0f;
        public static Arrow arrow = new Arrow(Color.black);

        public static bool isInvisble = false;

        public static void ClearAndReload()
        {
            Player = null;
            CurrentTarget = AssassinMarked = null;
            invisibleTimer = 0f;
            isInvisble = false;
            if (arrow?.arrow != null) UObject.Destroy(arrow.arrow);
            arrow = new Arrow(Color.black);
            if (arrow.arrow != null) arrow.arrow.SetActive(false);
        }
    }
}