using UnityEngine;

public class LightFlickerEffect : ILightFlickerEffect
{
    private readonly ISceneLightProvider _lightProvider;
    private readonly float _flickerRange;
    private readonly float _timeBetweenIntensity;
    private float _timer;

    public LightFlickerEffect(ISceneLightProvider provider, float flickerRange = 0.4f, float timeBetweenIntensity = 0.05f)
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
            float flickerOffset = Random.Range(-scaledRange, scaledRange);
            lights[i].intensity = Mathf.Max(0f, baseIntensity + flickerOffset);
        }

        _timer = 0f;
    }
}