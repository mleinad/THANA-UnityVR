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

        float t = 0f;

        while (t < 1f && !token.IsCancellationRequested)
        {
            t += Time.deltaTime * _transitionSpeed;

            for (int i = 0; i < lights.Count; i++)
            {
                var baseColor = baseColors[i];
                var baseIntensity = baseIntensities[i];

                var addedColor = emotionColor * strength * _intensityMultiplier;
                var targetColor = ClampColor(baseColor + addedColor);
                var targetIntensity = baseIntensity * Mathf.Lerp(0.7f, 1.3f, strength);

                lights[i].color = Color.Lerp(lights[i].color, targetColor, t);
                lights[i].intensity = Mathf.Lerp(lights[i].intensity, targetIntensity, t);
            }

            await UniTask.Yield(PlayerLoopTiming.Update, token);
        }
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