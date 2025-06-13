namespace TheSushiRoles.Roles
{
    public static class BountyHunter 
    {
        public static PlayerControl Player;
        public static Color Color = Palette.ImpostorRed;
        public static Arrow arrow;

        public static float arrowUpdateTimer = 0f;
        public static float bountyUpdateTimer = 0f;
        public static PlayerControl bounty;
        public static TextMeshPro CooldownText;

        public static void ClearAndReload() 
        {
            arrow = new Arrow(Color);
            Player = null;
            bounty = null;
            arrowUpdateTimer = 0f;
            bountyUpdateTimer = 0f;
            if (arrow != null && arrow.arrow != null) UObject.Destroy(arrow.arrow);
            arrow = null;
            if (CooldownText != null && CooldownText.gameObject != null) UObject.Destroy(CooldownText.gameObject);
            CooldownText = null;
            foreach (PoolablePlayer p in MapOptions.BeanIcons.Values)
            {
                if (p != null && p.gameObject != null) p.gameObject.SetActive(false);
            }
        }
    }
}