using UnityEngine.XR.Interaction.Toolkit;

namespace Interactions
{
    using UnityEngine;
    using UnityEngine.InputSystem;
    using UnityEngine.XR.Interaction.Toolkit;

    public class RayToggleInteractor : MonoBehaviour
    {
        [Header("Input")]
        [SerializeField] private InputActionReference triggerAction; // Button-type action

        [Header("Interactors")]
        [SerializeField] private XRRayInteractor rayInteractor;
        [SerializeField] private XRDirectInteractor directInteractor;

        private bool rayModeActive = false;

        private void OnEnable()
        {
            if (triggerAction != null)
            {
                triggerAction.action.performed += OnTriggerPressed;
                triggerAction.action.canceled += OnTriggerReleased;
                triggerAction.action.Enable();
            }
        }

        private void OnDisable()
        {
            if (triggerAction != null)
            {
                triggerAction.action.performed -= OnTriggerPressed;
                triggerAction.action.canceled -= OnTriggerReleased;
                triggerAction.action.Disable();
            }
        }

        private void OnTriggerPressed(InputAction.CallbackContext ctx)
        {
            EnableRayMode(true);
        }

        private void OnTriggerReleased(InputAction.CallbackContext ctx)
        {
            EnableRayMode(false);
        }

        private void EnableRayMode(bool enable)
        {
            if (rayModeActive == enable)
                return;

            rayModeActive = enable;

            if (rayInteractor != null)
                rayInteractor.enabled = enable;

            if (directInteractor != null)
                directInteractor.enabled = !enable;
        }
    }
}
