using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class LightManager : MonoBehaviour
    {
        [SerializeField] private List<Light> scenelights;

        [SerializeField, Min(0f)] private float timeBetweenIntensity = 0.1f;
        [SerializeField, Range(0f, 1f)] private float flickerRange = 0.5f; // % of original intensity

        public static LightManager Instance;

        private float _currentTimer;
        private float _suspicionMultiplier = 0f;

        private readonly List<float> _originalIntensities = new List<float>();
        private readonly List<Color> _originalColors = new List<Color>();
        
        [Header("Emotion Light Colors")]
        [SerializeField] private Color angerColor = Color.red;
        [SerializeField] private Color happinessColor = Color.yellow;
        [SerializeField] private Color regretColor = Color.blue;

        [Header("Emotion Thresholds")]
        [SerializeField, Range(0f, 1f)] private float angerThreshold = 0.7f;
        [SerializeField, Range(0f, 1f)] private float happinessThreshold = 0.7f;
        [SerializeField, Range(0f, 1f)] private float regretThreshold = 0.7f;

        
        private void Awake()
        {
            Instance = this;

            foreach (var light in scenelights)
            {
                _originalIntensities.Add(light.intensity);
                _originalColors.Add(light.color);

            }
        }

        public void FlickerLights(float amount)
        {
            _suspicionMultiplier = amount;

            if (amount == 0f)
            {
                for (int i = 0; i < scenelights.Count; i++)
                {
                    scenelights[i].intensity = _originalIntensities[i];
                }
            }
        }

        private void UpdateFlickerLights()
        {
            _currentTimer += Time.deltaTime;
            if (_currentTimer < timeBetweenIntensity) return;

            float scaledFlickerRange = flickerRange * _suspicionMultiplier;

            for (int i = 0; i < scenelights.Count; i++)
            {
                float original = _originalIntensities[i];
                float min = Mathf.Max(0f, original - (scaledFlickerRange * original));
                float max = original + (scaledFlickerRange * original);
                scenelights[i].intensity = Random.Range(min, max);
            }

            _currentTimer = 0;
        }

        public void UpdateLightColor(float anger, float happiness, float regret)
        {
            Color? newColor = null;

            if (anger > angerThreshold)
                newColor = angerColor;
            else if (happiness > happinessThreshold)
                newColor = happinessColor;
            else if (regret > regretThreshold)
                newColor = regretColor;

            if (newColor.HasValue)
            {
                foreach (var light in scenelights)
                {
                    light.color = newColor.Value;
                }
            }
            else
            {
                for (int i = 0; i < scenelights.Count; i++)
                {
                    scenelights[i].color = _originalColors[i];
                }
            }
        }

        private void Update()
        {
            if (_suspicionMultiplier > 0) ;
            {
                UpdateFlickerLights();
            }
        }
    }
}