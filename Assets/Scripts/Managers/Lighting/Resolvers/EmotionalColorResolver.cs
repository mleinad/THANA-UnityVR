using UnityEngine;

public class EmotionColorResolver : IEmotionColorResolver
{
    private readonly float _angerThreshold;
    private readonly float _happinessThreshold;
    private readonly float _regretThreshold;

    private readonly Color _angerColor;
    private readonly Color _happinessColor;
    private readonly Color _regretColor;

    public EmotionColorResolver(
        float angerThreshold, float happinessThreshold, float regretThreshold,
        Color angerColor, Color happinessColor, Color regretColor)
    {
        _angerThreshold = angerThreshold;
        _happinessThreshold = happinessThreshold;
        _regretThreshold = regretThreshold;

        _angerColor = angerColor;
        _happinessColor = happinessColor;
        _regretColor = regretColor;
    }

    public Color GetDominantColor(EmotionalValue emotions, out float strength)
    {
        strength = 0f;

        switch (emotions.GetDominantEmotion())
        {
            case 1:
                strength = emotions.anger;
                return _angerColor * strength;
                break;
            case 2:
                strength = emotions.regret;
                return _regretColor * strength;
                break;
            case 0:
                strength = emotions.happiness;
                return _happinessColor * strength;
                break;
        }

        return Color.black;
    }
}