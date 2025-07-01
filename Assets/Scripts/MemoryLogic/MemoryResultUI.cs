using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MemoryLogic
{
    public class MemoryResultUI : MonoBehaviour
    {
        [SerializeField] private GameObject initialInfo;
        [SerializeField] private GameObject endInfo;

        [SerializeField] private GameObject happy;
        [SerializeField] private GameObject sus;
        [SerializeField] private GameObject sad;

        private async void OnEnable()
        {
            // Wait until MemoryResultData.Instance is not null
            await UniTask.WaitUntil(() => MemoryResultData.Instance != null);

            var data = MemoryResultData.Instance;

            if (data.BeenLoaded)
            {
                initialInfo.SetActive(false);
                endInfo.SetActive(true);
                SetEndingUI(data.Ending);
            }
            else
            {
                initialInfo.SetActive(true);
                endInfo.SetActive(false);
            }
        }


        private void SetEndingUI(int ending)
        {
            happy.SetActive(ending == 0);
            sus.SetActive(ending == 1);
            sad.SetActive(ending == 2);

            Debug.Log($"Ending {ending + 1} displayed.");
        }
    }
}