using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace Managers
{
    public class MemoryButtonInteractor : MonoBehaviour
    {
        [Header("Controller Inputs")] [SerializeField]
        private InputActionReference buttonUp; // A (or X)

        [SerializeField] private InputActionReference buttonDown; // B (or Y)

        [Header("XR Setup")] [SerializeField] private XRDirectInteractor interactor;

        void OnEnable()
        {
            buttonUp.action.Enable();
            buttonDown.action.Enable();

            buttonUp.action.performed += OnUpPressed;
            buttonDown.action.performed += OnDownPressed;
        }

        void OnDisable()
        {
            buttonUp.action.Disable();
            buttonDown.action.Disable();

            buttonUp.action.performed -= OnUpPressed;
            buttonDown.action.performed -= OnDownPressed;
        }

        private void OnUpPressed(InputAction.CallbackContext ctx)
        {
            TrySwitchVariant(+1);
        }

        private void OnDownPressed(InputAction.CallbackContext ctx)
        {
            TrySwitchVariant(-1);
        }

        private void TrySwitchVariant(int direction)
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