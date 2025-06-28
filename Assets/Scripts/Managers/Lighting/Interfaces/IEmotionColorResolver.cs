using UnityEngine;

public interface IEmotionColorResolver
{
    Color GetDominantColor(EmotionalValue emotions, out float strength);
}