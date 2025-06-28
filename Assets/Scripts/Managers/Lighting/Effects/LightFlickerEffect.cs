using UnityEngine;

public class LightFlickerEffect : ILightFlickerEffect
{
    private readonly ISceneLightProvider _lightProvider;
    private readonly float _flickerRange;
    private readonly float _timeBetweenIntensity;

    private float _timer;

    public LightFlickerEffect(ISceneLightProvider provider, float flickerRange, float timeBetweenIntensity)
    {
        _lightProvider = provider;
        _flickerRange = flickerRange;
        _timeBetweenIntensity = timeBetweenIntensity;
    }

    public void FlickerLights(float suspicionMultiplier)
    {
        _timer += Time.deltaTime;
        if (_timer < _timeBetweenIntensity) return;

        var lights = _lightProvider.SceneLights;
        var intensities = _lightProvider.OriginalIntensities;
        float scaledRange = _flickerRange * suspicionMultiplier;

        for (int i = 0; i < lights.Count; i++)
        {
            float baseIntensity = intensities[i];
            float min = Mathf.Max(0f, baseIntensity - (scaledRange * baseIntensity));
            float max = baseIntensity + (scaledRange * baseIntensity);
            lights[i].intensity = Random.Range(min, max);
        }

        _timer = 0f;
    }
}