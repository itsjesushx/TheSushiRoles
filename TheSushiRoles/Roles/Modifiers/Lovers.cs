using UnityEngine;

namespace TheSushiRoles.Roles.Modifiers
{
    public static class Lovers 
    {
        public static PlayerControl Lover1;
        public static PlayerControl Lover2;
        public static Color Color = new Color32(232, 57, 185, byte.MaxValue);
        public static bool bothDie = true;
        public static bool enableChat = true;
        // Lovers save if next to be exiled is a lover, because RPC of ending game comes before RPC of exiled
        public static bool notAckedExiledIsLover = false;
        public static bool Existing() 
        {
            return Lover1 != null && Lover2 != null && !Lover1.Data.Disconnected && !Lover2.Data.Disconnected;
        }

        public static bool ExistingAndAlive() 
        {
            return Existing() && !Lover1.Data.IsDead && !Lover2.Data.IsDead && !notAckedExiledIsLover; // ADD NOT ACKED IS LOVER
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
            notAckedExiledIsLover = false;
            bothDie = CustomOptionHolder.modifierLoverBothDie.GetBool() && !(Lover1 == Pestilence.Player || Lover2 == Pestilence.Player);
            enableChat = CustomOptionHolder.modifierLoverEnableChat.GetBool();
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