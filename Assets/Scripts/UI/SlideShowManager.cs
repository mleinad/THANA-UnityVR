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
                ActivateSlide(currentIndex);
               buttonUpL.action.performed += PageUp;
               buttonDownL.action.performed += PageDown;
               
               buttonUpR.action.performed += PageUp;
               buttonDownR.action.performed += PageDown;
               text.SetActive(false);
        }

        private void OnDisable()
        {
                
            buttonUpL.action.performed -= PageUp;
            buttonDownL.action.performed -= PageDown;
               
            buttonUpR.action.performed -= PageUp;
            buttonDownR.action.performed -= PageDown;
        }

        void PageUp(InputAction.CallbackContext ctx)
        {
            currentIndex++;
            ActivateSlide(currentIndex);

            if (currentIndex > slideShows.Count)
            {
                text.SetActive(true);
                
                _ = LoadSceneWithDelayAsync(); // Fire-and-forget
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
                if(currentIndex < 0)
                    ActivateSlide(currentIndex);
        }


        void ActivateSlide(int i)
        {
            for (int j = 0; j < slideShows.Count; j++)
            {
                slideShows[j].SetActive(i == j);
            }
        }
}
