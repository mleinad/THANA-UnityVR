using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Interactions.Interfaces;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Interactions
{
    public class ProximityHighlighterController : MonoBehaviour, IProximityHighlightController
    {
        [SerializeField] private InteractableLocator _locator;
        private HashSet<MeshHighlightVisual> _previousTargets = new();
        private const float UpdateIntervalSeconds = 0.2f;
        
        [Header("Input")]
        [SerializeField] private InputActionProperty rightTouch;
        [SerializeField] private InputActionProperty leftTouch;

        
        private bool _isTouching;
        private CancellationTokenSource _highlightCts;

        private void OnEnable()
        {
            rightTouch.action.Enable();
            rightTouch.action.performed += OnTouchEnter;
            rightTouch.action.canceled += OnTouchExit;
            
            leftTouch.action.performed += OnTouchEnter;
            leftTouch.action.canceled += OnTouchExit;

        }

        private void OnDisable()
        {
            rightTouch.action.performed -= OnTouchEnter;
            rightTouch.action.canceled -= OnTouchExit;
            
            leftTouch.action.performed -= OnTouchEnter;
            leftTouch.action.canceled -= OnTouchExit;
            
            rightTouch.action.Disable();
            
            
            StopHighlighting();
        }
        
      //  private void Start() => RunHighlightLoop().Forget();


      
      private void OnTouchEnter(InputAction.CallbackContext ctx)
      {
          StartHighlighting();
      }

      private void OnTouchExit(InputAction.CallbackContext ctx)
      {
          StopHighlighting();
      }
      
      private void StartHighlighting()
      {
          if (_highlightCts != null && !_highlightCts.IsCancellationRequested)
          {
              return;
          }

          _isTouching = true;
    
          _highlightCts?.Dispose();

          _highlightCts = new CancellationTokenSource();
          RunHighlightLoop(_highlightCts.Token).Forget();

          //Debug.Log("Starting highlighting");
      }

      private void StopHighlighting()
      {
          _isTouching = false;

          if (_highlightCts != null && !_highlightCts.IsCancellationRequested)
          {
              _highlightCts.Cancel();
              _highlightCts.Dispose();
              _highlightCts = null;
          }

          foreach (var target in _previousTargets)
          {
              var h = target;
              if (h != null) h.Hide();
          }

          _previousTargets.Clear();

          //Debug.Log("Stopped highlighting");
      }

      
      
      private async UniTaskVoid RunHighlightLoop(CancellationToken token)
      { 
      
          while (!token.IsCancellationRequested)
            {
                var currentTargets = new HashSet<MeshHighlightVisual>(_locator.GetLeftHandInteractableProximityList());

                float count = currentTargets.Count;
                int i = 0;

                // Disable highlights for targets no longer in range
                foreach (var oldTarget in _previousTargets)
                {
                    if (!currentTargets.Contains(oldTarget))
                    {
                        var h = oldTarget;
                        if (h != null) h.Hide();
                    }
                }

                // Update and show highlights for current targets
                foreach (var target in currentTargets)
                {
                    var h = target;
                    if (h == null) continue;
                    
                    if(h.isBeingHeld) continue;
                    
                    if (!h.beenInitialized)
                        h.Initialize();

                    float priority = 1f - (i / count);
                    h.UpdateVisual(priority);
                    h.Show();
                    i++;
                }

                _previousTargets = currentTargets;

                await UniTask.Delay(TimeSpan.FromSeconds(UpdateIntervalSeconds), cancellationToken: token);
            }
      }

        
        public Transform GetClosestObjectLeft()
        {
            throw new NotImplementedException();
        }

        public Transform GetClosestObjectRight()
        {
            throw new NotImplementedException();
        }
    }
}