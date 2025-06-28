using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public interface ILightTransitionEffect
{
    UniTask SmoothTransitionAsync(Color emotionColor, float strength, CancellationToken token);
}