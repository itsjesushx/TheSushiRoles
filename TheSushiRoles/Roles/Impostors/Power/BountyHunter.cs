using UnityEngine;

namespace TheSushiRoles.Roles
{
    public static class BountyHunter 
    {
        public static PlayerControl Player;
        public static Color Color = Palette.ImpostorRed;
        public static Arrow arrow;
        public static float bountyDuration = 25f;
        public static bool showArrow = true;
        public static float bountyKillCooldown = 0f;
        public static float punishmentTime = 15f;
        public static float arrowUpdateIntervall = 10f;

        public static float arrowUpdateTimer = 0f;
        public static float bountyUpdateTimer = 0f;
        public static PlayerControl bounty;
        public static TMPro.TextMeshPro CooldownText;

        public static void ClearAndReload() 
        {
            arrow = new Arrow(Color);
            Player = null;
            bounty = null;
            arrowUpdateTimer = 0f;
            bountyUpdateTimer = 0f;
            if (arrow != null && arrow.arrow != null) UnityEngine.Object.Destroy(arrow.arrow);
            arrow = null;
            if (CooldownText != null && CooldownText.gameObject != null) UnityEngine.Object.Destroy(CooldownText.gameObject);
            CooldownText = null;
            foreach (PoolablePlayer p in MapOptions.BeanIcons.Values) 
            {
                if (p != null && p.gameObject != null) p.gameObject.SetActive(false);
            }


            bountyDuration = CustomOptionHolder.bountyHunterBountyDuration.GetFloat();
            bountyKillCooldown = CustomOptionHolder.bountyHunterReducedCooldown.GetFloat();
            punishmentTime = CustomOptionHolder.bountyHunterPunishmentTime.GetFloat();
            showArrow = CustomOptionHolder.bountyHunterShowArrow.GetBool();
            arrowUpdateIntervall = CustomOptionHolder.bountyHunterArrowUpdateIntervall.GetFloat();
        }
    }
}