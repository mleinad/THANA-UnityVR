using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public interface ISceneLightProvider
{
        UniTask InitializeAsync();
        IReadOnlyList<Light> SceneLights { get; }
        IReadOnlyList<float> OriginalIntensities { get; }
        IReadOnlyList<Color> OriginalColors { get; }
}
