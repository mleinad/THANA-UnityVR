using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Eflatun.SceneReference;
using Systems;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button exitButton;
    
    [SerializeField] private Button backButton;

    [Header("Settings Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject settingsPanel;

    [Header("Scene Settings")]
    [SerializeField] private SceneReference sceneToLoadReference;
    private void Awake()
    {
        startButton.onClick.AddListener(OnStartClicked);
        settingsButton.onClick.AddListener(OnSettingsClicked);
        exitButton.onClick.AddListener(OnExitClicked);
        backButton.onClick.AddListener(OnBackClicked);
    }

    private void OnBackClicked()
    {
        mainMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    private void OnEnable()
    {
        mainMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    private void OnStartClicked()
    {
        //DisableMenu();
      //  _= SceneLoader.Instance.LoadSceneGroup(mainSceneGroupIndex);
      SceneManager.LoadScene(sceneToLoadReference.Name);
    }

    private void OnSettingsClicked()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    private void OnExitClicked()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
