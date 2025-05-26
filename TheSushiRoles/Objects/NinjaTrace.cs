using System;
using System.Collections.Generic;
using UnityEngine;

namespace TheSushiRoles.Objects 
{
    class AssassinTrace 
    {
        public static List<AssassinTrace> traces = new List<AssassinTrace>();
        private GameObject trace;
        private float timeRemaining;
        private static Sprite TraceSprite;
        public static Sprite GetTraceSprite() 
        {
            if (TraceSprite) return TraceSprite;
            TraceSprite = Utils.LoadSprite("TheSushiRoles.Resources.AssassinTraceW.png", 225f);
            return TraceSprite;
        }
        public AssassinTrace(Vector2 p, float Duration=1f) 
        {
            trace = new GameObject("AssassinTrace");
            trace.AddSubmergedComponent(SubmergedCompatibility.Classes.ElevatorMover);
            //Vector3 position = new Vector3(p.x, p.y, PlayerControl.LocalPlayer.transform.localPosition.z + 0.001f); // just behind player
            Vector3 position = new Vector3(p.x, p.y, p.y / 1000f + 0.01f);
            trace.transform.position = position;
            trace.transform.localPosition = position;
            
            var traceRenderer = trace.AddComponent<SpriteRenderer>();
            traceRenderer.sprite = GetTraceSprite();

            timeRemaining = Duration;

            // display the Assassins color in the trace
            float colorDuration = CustomOptionHolder.AssassinTraceColorTime.GetFloat();
            FastDestroyableSingleton<HudManager>.Instance.StartCoroutine(Effects.Lerp(colorDuration, new Action<float>((p) => {
                Color c = Palette.PlayerColors[(int)Assassin.Player.Data.DefaultOutfit.ColorId];
                if (Utils.IsLighterColor(Assassin.Player)) c = Color.white;
                else c = Palette.PlayerColors[6];
                Color g = Color.green; // Usual display color.
                Color combinedColor = Mathf.Clamp01(p) * g + Mathf.Clamp01(1 - p) * c;
                if (traceRenderer) traceRenderer.color = combinedColor;
            })));

            float fadeOutDuration = 1f;
            if (fadeOutDuration > Duration) fadeOutDuration = 0.5f * Duration;
            FastDestroyableSingleton<HudManager>.Instance.StartCoroutine(Effects.Lerp(Duration, new Action<float>((p) => {
                float interP = 0f;
                if (p < (Duration - fadeOutDuration) / Duration)
                    interP = 0f;
                else interP = (p * Duration + fadeOutDuration - Duration) / fadeOutDuration;
                if (traceRenderer) traceRenderer.color = new Color(traceRenderer.color.r, traceRenderer.color.g, traceRenderer.color.b, Mathf.Clamp01(1 - interP));
            })));

            trace.SetActive(true);
            traces.Add(this);
        }

        public static void ClearTraces() 
        {
            traces = new List<AssassinTrace>();
        }

        public static void UpdateAll() 
        {
            foreach (AssassinTrace traceCurrent in new List<AssassinTrace>(traces))
            {
                traceCurrent.timeRemaining -= Time.fixedDeltaTime;
                if (traceCurrent.timeRemaining < 0)
                {
                    traceCurrent.trace.SetActive(false);
                    UnityEngine.Object.Destroy(traceCurrent.trace);
                    traces.Remove(traceCurrent);
                }
            }
        }
    }
}
