using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class SceneLightProvider : ISceneLightProvider
{
    public IReadOnlyList<Light> SceneLights => _sceneLights;
    public IReadOnlyList<float> OriginalIntensities => _originalIntensities;
    public IReadOnlyList<Color> OriginalColors => _originalColors;

    private List<Light> _sceneLights = new();
    private List<float> _originalIntensities = new();
    private List<Color> _originalColors = new();

    public async UniTask InitializeAsync()
    {
        _sceneLights = Object.FindObjectsOfType<Light>().ToList();

        _originalIntensities.Clear();
        _originalColors.Clear();

        foreach (var light in _sceneLights)
        {
            _originalIntensities.Add(light.intensity);
            _originalColors.Add(light.color);
        }

        await UniTask.Yield();
    }
}