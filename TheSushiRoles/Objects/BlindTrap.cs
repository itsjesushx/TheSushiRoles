using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TheSushiRoles.Objects
{
    class BlindTrap
    {
        public static List<BlindTrap> traps = new List<BlindTrap>();
        private static int instanceCounter = 0;
        public int instanceId = 0;
        public GameObject trap;
        public SpriteRenderer trapRenderer;
        public bool triggerable = false;
        private static Sprite trapSprite;

        public static Sprite GetTrapSprite()
        {
            if (trapSprite) return trapSprite;
            trapSprite = Utils.LoadSpriteFromResources("TheSushiRoles.Resources.BlindTrap.png", 300f);
            return trapSprite;
        }

        public BlindTrap(Vector2 p)
        {
            trap = new GameObject("Trap") { layer = 11 };
            trap.AddSubmergedComponent(SubmergedCompatibility.Classes.ElevatorMover);
            Vector3 position = new Vector3(p.x, p.y, p.y / 1000 + 0.001f);
            trap.transform.position = position;

            trapRenderer = trap.AddComponent<SpriteRenderer>();
            trapRenderer.sprite = GetTrapSprite();

            trap.SetActive(false);
            instanceId = ++instanceCounter;
            triggerable = true;
            traps.Add(this);

            if (PlayerControl.LocalPlayer.PlayerId == Viper.Player.PlayerId || PlayerControl.LocalPlayer.Data.IsDead)
            {
                trap.SetActive(true);
                SetAlpha(0.5f);
            }
        }

        private void SetAlpha(float alpha)
        {
            if (trapRenderer != null)
            {
                Color c = trapRenderer.color;
                trapRenderer.color = new Color(c.r, c.g, c.b, alpha);
            }
        }

        public static void ClearTraps()
        {
            foreach (BlindTrap t in traps)
            {
                UnityEngine.Object.Destroy(t.trap);
            }
            traps.Clear();
            instanceCounter = 0;
        }

        public static void TriggerTrap(byte playerId, byte trapId)
        {
            BlindTrap t = traps.FirstOrDefault(x => x.instanceId == (int)trapId);
            PlayerControl player = Utils.PlayerById(playerId);
            if (t == null || player == null) return;
            if (player.IsImpostor()) return;

            t.triggerable = false;

            if (playerId == PlayerControl.LocalPlayer.PlayerId)
            {
                t.trap.SetActive(true);
                t.SetAlpha(1f);
                SoundEffectsManager.Play("trapperTrap");
                Viper.BlindedPlayers.Add(playerId);
            }
            else if (PlayerControl.LocalPlayer.PlayerId == Viper.Player.PlayerId)
            {
                t.trap.SetActive(true);
                t.SetAlpha(0.5f);
            }

            FastDestroyableSingleton<HudManager>.Instance.StartCoroutine(Effects.Lerp(Viper.BlindDuration, new Action<float>((p) =>
            {
                if (p == 1f)
                {
                    Viper.BlindedPlayers.Remove(player.PlayerId);
                }
            })));

            t.triggerable = true;
        }

        public static void Update()
        {
            var player = PlayerControl.LocalPlayer;
            if (player == null || player.Data == null) return;

            float triggerDistance = MapUtilities.CachedShipStatus.AllVents[0]?.UsableDistance / 2 ?? 0.4f;
            BlindTrap target = null;
            float closestDistance = float.MaxValue;

            foreach (BlindTrap trap in traps)
            {
                if (!trap.triggerable || player.inVent || !player.CanMove) continue;
                if (player.IsImpostor()) continue;

                float distance = Vector2.Distance(trap.trap.transform.position, player.GetTruePosition());

                if (distance <= triggerDistance && distance < closestDistance)
                {
                    closestDistance = distance;
                    target = trap;
                }
            }

            if (target != null && !player.Data.IsDead)
            {
                Utils.StartRPC(CustomRPC.TriggerBlindTrap, player.PlayerId, target.instanceId);
                RPCProcedure.TriggerBlindTrap(player.PlayerId, (byte)target.instanceId);
            }

            if (player.Data.IsDead)
            {
                foreach (BlindTrap trap in traps)
                {
                    if (!trap.trap.active)
                    {
                        trap.trap.SetActive(true);
                        trap.SetAlpha(0.5f);
                    }
                }
            }
        }
    }
}