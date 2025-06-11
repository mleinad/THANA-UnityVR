using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

namespace Interactions
{
    public class LocomotionSettingsManager : MonoBehaviour
    {
        [Header("Turning")]
        [SerializeField] private ActionBasedSnapTurnProvider snapTurnProvider;
        [SerializeField] private ActionBasedContinuousTurnProvider continuousTurnProvider;

        [Header("Movement")]
        [SerializeField] private TeleportationProvider teleportationProvider;
        [SerializeField] private ActionBasedContinuousMoveProvider continuousMoveProvider;

        [Header("Teleport Interactor")]
        [SerializeField] private GameObject teleportInteractorObject; // Assign the Teleport Interactor GameObject here

        [Header("Snap Turn Settings")]
        [SerializeField] private int defaultSnapTurnDegrees = 45;

        [Header("Menu")]
        [SerializeField] private InputActionReference menuButtonAction; // Assign the Menu button action (e.g., from Input System)
        [SerializeField] private GameObject menuUI; // Assign your menu UI GameObject here

        public enum TurnMode { Snap, Continuous }
        public enum MoveMode { Teleport, Continuous }

        private TurnMode currentTurnMode = TurnMode.Snap;
        private MoveMode currentMoveMode = MoveMode.Continuous;

        private bool isMenuOpen = false;

        private void Start()
        {
            SetSnapTurnAmount(defaultSnapTurnDegrees);
            SetTurnMode((int)currentTurnMode);
            SetMoveMode((int)currentMoveMode);

            if (menuButtonAction != null)
            {
                menuButtonAction.action.performed += OnMenuButtonPressed;
                menuButtonAction.action.Enable();
            }

            UpdateTeleportInteractorVisibility();
            UpdateMenuUIVisibility();
        }

        private void OnDestroy()
        {
            if (menuButtonAction != null)
            {
                menuButtonAction.action.performed -= OnMenuButtonPressed;
                menuButtonAction.action.Disable();
            }
        }

        public void SetTurnMode(int mode)
        {
            currentTurnMode = (TurnMode)mode;

            bool useSnap = currentTurnMode == TurnMode.Snap;

            if (snapTurnProvider != null)
                snapTurnProvider.enabled = useSnap;

            if (continuousTurnProvider != null)
                continuousTurnProvider.enabled = !useSnap;
        }

        public void SetMoveMode(int mode)
        {
            currentMoveMode = (MoveMode)mode;

            bool useTeleport = currentMoveMode == MoveMode.Teleport;

            if (teleportationProvider != null)
                teleportationProvider.enabled = useTeleport;

            if (continuousMoveProvider != null)
                continuousMoveProvider.enabled = !useTeleport;

            UpdateTeleportInteractorVisibility();
        }

        public void ToggleTurnMode()
        {
            SetTurnMode(currentTurnMode == TurnMode.Snap ? 1 : 0);
        }

        public void ToggleMoveMode()
        {
            SetMoveMode(currentMoveMode == MoveMode.Teleport ? 1 : 0);
        }

        public void SetSnapTurnAmount(float degrees)
        {
            if (snapTurnProvider != null)
            {
                snapTurnProvider.turnAmount = Mathf.RoundToInt(degrees);
            }
        }

        private void UpdateTeleportInteractorVisibility()
        {
            if (teleportInteractorObject != null)
            {
                bool isTeleportEnabled = currentMoveMode == MoveMode.Teleport;
                teleportInteractorObject.SetActive(isTeleportEnabled);
            }
        }

        private void OnMenuButtonPressed(InputAction.CallbackContext ctx)
        {
            isMenuOpen = !isMenuOpen;
            UpdateMenuUIVisibility();

            // Optionally, pause gameplay or disable locomotion while menu is open
            // For example, disable locomotion providers while menu open:
            if (isMenuOpen)
            {
                DisableLocomotion();
            }
            else
            {
                EnableLocomotion();
            }
        }

        private void UpdateMenuUIVisibility()
        {
            if (menuUI != null)
            {
                menuUI.SetActive(isMenuOpen);
            }
        }

        private void DisableLocomotion()
        {
            if (snapTurnProvider != null) snapTurnProvider.enabled = false;
            if (continuousTurnProvider != null) continuousTurnProvider.enabled = false;
            if (teleportationProvider != null) teleportationProvider.enabled = false;
            if (continuousMoveProvider != null) continuousMoveProvider.enabled = false;

            if (teleportInteractorObject != null)
                teleportInteractorObject.SetActive(false);
        }

        private void EnableLocomotion()
        {
            // Restore locomotion based on current settings
            SetTurnMode((int)currentTurnMode);
            SetMoveMode((int)currentMoveMode);
        }
        
        public TurnMode CurrentTurnMode => currentTurnMode;
        public MoveMode CurrentMoveMode => currentMoveMode;
    }
}
