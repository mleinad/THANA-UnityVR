using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using MemoryLogic;
using Systems;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SlideShowManager : MonoBehaviour
{
        [SerializeField]
        List<GameObject> slideShows;
        
        [Header("Controller Inputs")]
        [SerializeField] private InputActionReference buttonUpL;    // Left controller up button
        [SerializeField] private InputActionReference buttonDownL;  // Left controller down button
        
        [SerializeField] private InputActionReference buttonUpR;    // Right controller up button
        [SerializeField] private InputActionReference buttonDownR;  // Right controller down button
        
        [SerializeField] private GameObject text;
        
        private int currentIndex = 0;
        void OnEnable()
        {
            buttonUpL.action.Enable();
            buttonDownL.action.Enable();
            buttonUpR.action.Enable();
            buttonDownR.action.Enable();
    
            buttonUpL.action.performed += PageUp;
            buttonDownL.action.performed += PageDown;
            buttonUpR.action.performed += PageUp;
            buttonDownR.action.performed += PageDown;

            ActivateSlide(currentIndex);
            text.SetActive(false);
        }

        void OnDisable()
        {
            buttonUpL.action.performed -= PageUp;
            buttonDownL.action.performed -= PageDown;
            buttonUpR.action.performed -= PageUp;
            buttonDownR.action.performed -= PageDown;
    
            buttonUpL.action.Disable();
            buttonDownL.action.Disable();
            buttonUpR.action.Disable();
            buttonDownR.action.Disable();
        }


        void PageUp(InputAction.CallbackContext ctx)
        {
            if (currentIndex < slideShows.Count - 1)
            {
                currentIndex++;
                ActivateSlide(currentIndex);
            }
            else if (currentIndex == slideShows.Count - 1)
            {
                currentIndex++;
                text.SetActive(true);
                _ = LoadSceneWithDelayAsync();
            }
        }


        private async UniTaskVoid LoadSceneWithDelayAsync()
        {
            // Begin loading the scene, but don't activate yet
            var asyncOp = SceneManager.LoadSceneAsync("Room1 Daniel");
          
            MemoryResultData.Instance.BeenLoaded = true;

            if (asyncOp != null)
            {
                asyncOp.allowSceneActivation = false;

                // Wait 3 seconds
                await UniTask.Delay(1000);

                // Activate the scene after delay
                asyncOp.allowSceneActivation = true;
            }
        }
        void PageDown(InputAction.CallbackContext ctx)
        {
            currentIndex--;
            if (currentIndex >= 0 && currentIndex < slideShows.Count)
            {
                text.SetActive(false); // Hide again
                ActivateSlide(currentIndex);
            }
            else
            {
                currentIndex = Mathf.Clamp(currentIndex, 0, slideShows.Count - 1);
            }
        }


        void ActivateSlide(int i)
        {
            for (int j = 0; j < slideShows.Count; j++)
            {
                slideShows[j].SetActive(j == i);
            }
        }
}
