using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class LightTransitionEffect : ILightTransitionEffect
{
    private readonly ISceneLightProvider _lightProvider;
    private readonly float _transitionSpeed;
    private readonly float _intensityMultiplier;

    public LightTransitionEffect(ISceneLightProvider provider, float transitionSpeed, float intensityMultiplier)
    {
        _lightProvider = provider;
        _transitionSpeed = transitionSpeed;
        _intensityMultiplier = intensityMultiplier;
    }

    public async UniTask SmoothTransitionAsync(Color emotionColor, float strength, CancellationToken token)
    {
        var lights = _lightProvider.SceneLights;
        var baseColors = _lightProvider.OriginalColors;
        var baseIntensities = _lightProvider.OriginalIntensities;

        while (!token.IsCancellationRequested)
        {
            for (int i = 0; i < lights.Count; i++)
            {
                var light = lights[i];

                Color targetColor = Color.Lerp(baseColors[i], emotionColor, strength);
                //float targetIntensity = Mathf.Lerp(baseIntensities[i], baseIntensities[i] * _intensityMultiplier, strength);

                light.color = Color.Lerp(light.color, targetColor, Time.deltaTime * _transitionSpeed);
               // light.intensity = Mathf.Lerp(light.intensity, baseIntensities[i], Time.deltaTime * _transitionSpeed);
            }

            await UniTask.Yield(PlayerLoopTiming.Update, token);
        }
    }
}