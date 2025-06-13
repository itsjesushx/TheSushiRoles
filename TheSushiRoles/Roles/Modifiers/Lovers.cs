using System.Collections.Generic;

namespace TheSushiRoles.Roles.Modifiers
{
    public static class Lovers 
    {
        public static PlayerControl Lover1;
        public static PlayerControl Lover2;
        public static Color Color = new Color32(232, 57, 185, byte.MaxValue);
        public static readonly List<Arrow> LocalArrows = new();
        public static bool Existing()
        {
            return Lover1 != null && Lover2 != null && !Lover1.Data.Disconnected && !Lover2.Data.Disconnected;
        }
        public static bool IsLover(this PlayerControl player)
        {
            return player != null && (player == Lovers.Lover1 || player == Lovers.Lover2);
        }

        public static bool ExistingAndAlive() 
        {
            return Existing() && !Lover1.Data.IsDead && !Lover2.Data.IsDead;
        }

        public static PlayerControl OtherLover(PlayerControl oneLover) 
        {
            if (!ExistingAndAlive()) return null;
            if (oneLover == Lover1) return Lover2;
            if (oneLover == Lover2) return Lover1;
            return null;
        }

        public static bool ExistingWithKiller() 
        {
            return Existing() && (Lover1.IsNeutralKiller() || Lover2.IsNeutralKiller() || Lover1.Data.Role.IsImpostor || Lover2.Data.Role.IsImpostor);
        }

        public static bool HasAliveKillingLover(this PlayerControl player) 
        {
            if (!Lovers.ExistingAndAlive() || !ExistingWithKiller())
                return false;
            return (player != null && (player == Lover1 || player == Lover2));
        }

        public static void ClearAndReload()
        {
            Lover1 = null;
            Lover2 = null;
            ResetArrows();
        }
        public static void ResetArrows()
        {
            foreach (Arrow arrow in LocalArrows)
                UObject.Destroy(arrow.arrow);

            LocalArrows.Clear();
            Arrow arrow1 = new(Palette.ImpostorRed);
            arrow1.arrow.SetActive(false);
            Arrow arrow2 = new(Palette.ImpostorRed);
            arrow2.arrow.SetActive(false);

            LocalArrows.Add(arrow1);
            LocalArrows.Add(arrow2);
        }

        public static PlayerControl GetPartner(this PlayerControl player)
        {
            if (player == null)
                return null;
            if (Lover1 == player)
                return Lover2;
            if (Lover2 == player)
                return Lover1;
            return null;
        }
    }
}