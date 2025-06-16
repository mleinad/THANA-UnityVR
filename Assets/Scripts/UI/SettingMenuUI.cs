using Interactions;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

namespace UI
{
    public class SettingMenuUI : MonoBehaviour
    {
        [Header("Manager Reference")]
        [SerializeField] private LocomotionSettingsManager locomotionManager;

        [Header("UI Elements")]
        [SerializeField] private Button turnModeButton;
        [SerializeField] private TMP_Text turnModeButtonText;

        [SerializeField] private Button moveModeButton;
        [SerializeField] private TMP_Text moveModeButtonText;

        [Header("Facing Settings")]
        [SerializeField] private bool facePlayer = true;
        [SerializeField] private float smoothSpeed = 5f;

        [Header("References")]
        [SerializeField] private InputAction leftTriggerAction;
        [SerializeField] private InputAction rightTriggerAction;

        [SerializeField] private XRRayInteractor leftRayInteractor;
        [SerializeField] private XRRayInteractor rightRayInteractor;

        private Transform playerCamera;

        private void Start()
        {
            if (locomotionManager == null)
            {
                Debug.LogError("LocomotionSettingsManager reference is missing.");
                enabled = false;
                return;
            }

            turnModeButton.onClick.AddListener(ToggleTurnMode);
            moveModeButton.onClick.AddListener(ToggleMoveMode);

            playerCamera = Camera.main?.transform;
            UpdateButtonTexts();

            leftTriggerAction.performed += ctx => TryClickHoveredButton(leftRayInteractor);
            rightTriggerAction.performed += ctx => TryClickHoveredButton(rightRayInteractor);

            leftTriggerAction.Enable();
            rightTriggerAction.Enable();
        }

        private void OnDestroy()
        {
            leftTriggerAction.performed -= ctx => TryClickHoveredButton(leftRayInteractor);
            rightTriggerAction.performed -= ctx => TryClickHoveredButton(rightRayInteractor);
        }

        private void Update()
        {
            if (facePlayer && playerCamera != null)
            {
                FacePlayer();
            }
        }

        private void TryClickHoveredButton(XRRayInteractor interactor)
        {
            if (interactor.TryGetCurrentUIRaycastResult(out var result))
            {
                Button button = result.gameObject.GetComponent<Button>();
                if (button != null)
                {
                    button.onClick.Invoke();
                }
            }
        }

        private void ToggleTurnMode()
        {
            locomotionManager.ToggleTurnMode();
            UpdateButtonTexts();
        }

        private void ToggleMoveMode()
        {
            locomotionManager.ToggleMoveMode();
            UpdateButtonTexts();
        }

        private void UpdateButtonTexts()
        {
            string turnText = locomotionManager.CurrentTurnMode == LocomotionSettingsManager.TurnMode.Snap
                ? "Turn: Snap"
                : "Turn: Smooth";
            turnModeButtonText.text = turnText;

            string moveText = locomotionManager.CurrentMoveMode == LocomotionSettingsManager.MoveMode.Teleport
                ? "Move: Teleport"
                : "Move: Smooth";
            moveModeButtonText.text = moveText;
        }

        private void FacePlayer()
        {
            Vector3 lookDirection = playerCamera.position - transform.position;
            lookDirection.y = 0;
            if (lookDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * smoothSpeed);
            }
        }
    }
}
