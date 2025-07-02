using Cysharp.Threading.Tasks;

public interface IMemoryManager
{
        UniTask InitializeAsync();

        public void RecalculateEmotions();

        public EmotionalValue GetFinalValue();
}
