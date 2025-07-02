using Interactions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace Managers
{
    public class MemoryButtonInteractor : MonoBehaviour
    {
        [Header("Controller Inputs")]
        [SerializeField] private InputActionReference buttonUpL;    // Left controller up button
        [SerializeField] private InputActionReference buttonDownL;  // Left controller down button
        
        [SerializeField] private InputActionReference buttonUpR;    // Right controller up button
        [SerializeField] private InputActionReference buttonDownR;  // Right controller down button

        [Header("XR Setup")] 
        [SerializeField] private XRDirectInteractor interactorL;
        [SerializeField] private XRDirectInteractor interactorR;

        private Transform _currentlyHoldingTransform;
        private void OnEnable()
        {
            buttonUpL.action.Enable();
            buttonDownL.action.Enable();
            buttonUpR.action.Enable();
            buttonDownR.action.Enable();

            buttonUpL.action.performed += OnUpPressedLeft;
            buttonDownL.action.performed += OnDownPressedLeft;
            buttonUpR.action.performed += OnUpPressedRight;
            buttonDownR.action.performed += OnDownPressedRight;

            interactorL.selectEntered.AddListener(OnGrabbedObject);
            interactorR.selectEntered.AddListener(OnGrabbedObject);
            
            interactorL.hoverEntered.AddListener(OnHoverEnter);
            interactorR.hoverEntered.AddListener(OnHoverEnter);
            
            interactorL.hoverExited.AddListener(OnHoverExit);
            interactorR.hoverExited.AddListener(OnHoverExit);

            
            
            interactorL.selectExited.AddListener(OnReleasedObject);
            interactorR.selectExited.AddListener(OnReleasedObject);

        }


        private void OnDisable()
        {
            buttonUpL.action.performed -= OnUpPressedLeft;
            buttonDownL.action.performed -= OnDownPressedLeft;
            buttonUpR.action.performed -= OnUpPressedRight;
            buttonDownR.action.performed -= OnDownPressedRight;

            buttonUpL.action.Disable();
            buttonDownL.action.Disable();
            buttonUpR.action.Disable();
            buttonDownR.action.Disable();
            
            interactorR.selectEntered.RemoveListener(OnGrabbedObject);
            interactorL.selectEntered.RemoveListener(OnGrabbedObject);
            
            
            interactorL.selectExited.RemoveListener(OnReleasedObject);
            interactorR.selectExited.RemoveListener(OnReleasedObject);
            
            
            interactorL.hoverEntered.RemoveListener(OnHoverEnter);
            interactorR.hoverEntered.AddListener(OnHoverEnter);
            
            interactorL.hoverExited.RemoveListener(OnHoverExit);
            interactorR.hoverExited.RemoveListener(OnHoverExit);

        }
        
        
        
        
        private void OnGrabbedObject(SelectEnterEventArgs args)
        {
            _currentlyHoldingTransform = args.interactableObject.transform;
            TryDisableHighlight();
        }
        
        private void OnReleasedObject(SelectExitEventArgs args)
        {
            _currentlyHoldingTransform = args.interactableObject.transform;
            TryDisableHighlight(true);
            _currentlyHoldingTransform = null;
            
        }


        private void OnHoverEnter(HoverEnterEventArgs args)
        {
            var mv = args.interactableObject.transform.GetComponent<MeshHighlightVisual>();
            if (mv == null) return;
            if(mv.isBeingHeld) return;
            
            mv.Show();
            mv.SetInRange(true);
        }

        private void OnHoverExit(HoverExitEventArgs args)
        {
            var mv = args.interactableObject.transform.GetComponent<MeshHighlightVisual>();
            if (mv == null) return;
            mv.SetInRange(false);
        }
        
        
        void TryDisableHighlight(bool force = false)
        {
             var mv =  _currentlyHoldingTransform.GetComponent<MeshHighlightVisual>();
             if (mv == null) return;
             if (mv != null)
             {
                 mv.isBeingHeld = !force;
             }
        }
        

        private void OnUpPressedLeft(InputAction.CallbackContext ctx)
        {
            TrySwitchVariant(interactorL, +1);
        }

        private void OnDownPressedLeft(InputAction.CallbackContext ctx)
        {
            TrySwitchVariant(interactorL, -1);
        }

        private void OnUpPressedRight(InputAction.CallbackContext ctx)
        {
            TrySwitchVariant(interactorR, +1);
        }

        private void OnDownPressedRight(InputAction.CallbackContext ctx)
        {
            TrySwitchVariant(interactorR, -1);
        }

        private void TrySwitchVariant(XRDirectInteractor interactor, int direction)
        {
            if (interactor.hasSelection)
            {
                var heldObject = interactor.GetOldestInteractableSelected();
                if (heldObject == null) return;

                var swapper = heldObject.transform.GetComponentInChildren<ObjectSwap>();
                if (swapper != null)
                {
                    swapper.SwitchVariant(direction);
                }
            }
        }
        
        
    }
}
