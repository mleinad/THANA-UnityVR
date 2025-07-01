using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneBootstrapper 
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static async void Init()
    {
        Debug.Log("Bootstrapper Scene Loaded...");
        //await SceneManager.LoadSceneAsync("Bootstrapper", LoadSceneMode.Single);
    }
}