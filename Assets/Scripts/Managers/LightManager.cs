using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Managers
{
    public class LightManager : MonoBehaviour
    {
        [Header("Timing")]
        [SerializeField, Min(0f)] private float timeBetweenIntensity = 0.1f;
        [SerializeField, Range(0f, 1f)] private float flickerRange = 0.5f;
        [SerializeField] private float transitionSpeed = 2f;

        [Header("Light Setup")]
        [SerializeField] private List<Light> sceneLights = new();

        [Header("Emotion Colors")]
        [SerializeField] private Color angerColor = Color.red;
        [SerializeField] private Color happinessColor = Color.yellow;
        [SerializeField] private Color regretColor = Color.blue;

        [Header("Emotion Thresholds")]
        [SerializeField, Range(0f, 1f)] private float angerThreshold = 0.7f;
        [SerializeField, Range(0f, 1f)] private float happinessThreshold = 0.7f;
        [SerializeField, Range(0f, 1f)] private float regretThreshold = 0.7f;
        [SerializeField, Range(0f, 1f)] private float suspicionThreshold = 0.7f;

        [Header("Color Intensity")]
        [SerializeField, Range(0f, 5f)] private float colorIntensityMultiplier = 1f;

        private readonly List<float> _originalIntensities = new();
        private readonly List<Color> _originalColors = new();

        private float _suspicionMultiplier = 0f;
        private float _currentTimer;
        private CancellationTokenSource _transitionCts;

        private async void Awake()
        {
            await CacheLightsInScene();
            await WaitForMemoryManager();
        }

        private async UniTask CacheLightsInScene()
        {
            sceneLights = FindObjectsOfType<Light>().ToList();

            _originalIntensities.Clear();
            _originalColors.Clear();

            foreach (var light in sceneLights)
            {
                _originalIntensities.Add(light.intensity);
                _originalColors.Add(light.color);
            }

            await UniTask.Yield();
        }

        private async UniTask WaitForMemoryManager()
        {
            await UniTask.WaitUntil(() => MemoryManager.Instance != null);
            
            Debug.Log("LightManager initialized");
        }

        private void OnDisable()
        {
            if (MemoryManager.Instance != null)
                MemoryManager.Instance.EmotionalStateChanged -= OnEmotionsChanged;

            _transitionCts?.Cancel();
            _transitionCts?.Dispose();
        }

        private void OnEmotionsChanged(EmotionalValue emotions)
        {
            Debug.Log("Emotional state changed");

            Color emotionColor = GetDominantEmotionColor(emotions, out float strength);
            _suspicionMultiplier = emotions.suspicion >= suspicionThreshold ? emotions.suspicion : 0f;

            _transitionCts?.Cancel();
            _transitionCts = new CancellationTokenSource();
            SmoothTransitionLights(emotionColor, strength, _transitionCts.Token).Forget();
        }

        private Color GetDominantEmotionColor(EmotionalValue emotions, out float strength)
        {
            strength = 0f;

            if (emotions.anger > angerThreshold &&
                emotions.anger >= emotions.happiness &&
                emotions.anger >= emotions.regret)
            {
                strength = emotions.anger;
                return angerColor;
            }

            if (emotions.happiness > happinessThreshold &&
                emotions.happiness >= emotions.anger &&
                emotions.happiness >= emotions.regret)
            {
                strength = emotions.happiness;
                return happinessColor;
            }

            if (emotions.regret > regretThreshold)
            {
                strength = emotions.regret;
                return regretColor;
            }

            return Color.black;
        }

        private async UniTaskVoid SmoothTransitionLights(Color emotionColor, float emotionStrength, CancellationToken token)
        {
            float t = 0f;

            while (t < 1f && !token.IsCancellationRequested)
            {
                t += Time.deltaTime * transitionSpeed;

                for (int i = 0; i < sceneLights.Count; i++)
                {
                    Light light = sceneLights[i];
                    Color baseColor = _originalColors[i];
                    float baseIntensity = _originalIntensities[i];

                    // Additive tint based on emotion and intensity multiplier
                    Color addedColor = emotionColor * emotionStrength * colorIntensityMultiplier;
                    Color targetColor = ClampColor(baseColor + addedColor);

                    float targetIntensity = baseIntensity * Mathf.Lerp(0.7f, 1.3f, emotionStrength);

                    light.color = Color.Lerp(light.color, targetColor, t);
                    light.intensity = Mathf.Lerp(light.intensity, targetIntensity, t);
                }

                await UniTask.Yield(PlayerLoopTiming.Update, token);
            }
        }

        private void Update()
        {
            if (_suspicionMultiplier > 0f)
                FlickerLights();
        }

        private void FlickerLights()
        {
            _currentTimer += Time.deltaTime;
            if (_currentTimer < timeBetweenIntensity) return;

            float scaledRange = flickerRange * _suspicionMultiplier;

            for (int i = 0; i < sceneLights.Count; i++)
            {
                float baseIntensity = _originalIntensities[i];
                float min = Mathf.Max(0f, baseIntensity - (scaledRange * baseIntensity));
                float max = baseIntensity + (scaledRange * baseIntensity);
                sceneLights[i].intensity = Random.Range(min, max);
            }

            _currentTimer = 0f;
        }

        private static Color ClampColor(Color color)
        {
            return new Color(
                Mathf.Clamp01(color.r),
                Mathf.Clamp01(color.g),
                Mathf.Clamp01(color.b),
                1f
            );
        }
    }
}
