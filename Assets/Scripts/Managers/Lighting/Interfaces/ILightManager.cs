using Cysharp.Threading.Tasks;


public interface ILightManager
{
    UniTask InitializeAsync(ISceneLightProvider lightProvider,
        IEmotionColorResolver emotionColorResolver,
        ILightTransitionEffect transitionEffect,
        ILightFlickerEffect flickerEffect);
}
