using Interactions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        }

        private void Update()
        {
            if (facePlayer && playerCamera != null)
            {
                FacePlayer();
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
            lookDirection.y = 0; // Optional: ignore vertical tilt
            if (lookDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * smoothSpeed);
            }
        }
    }
}
