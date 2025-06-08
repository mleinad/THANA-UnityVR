using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Mouse Look")]
    public float mouseSensitivity = 2f;
    public Transform cameraTransform;

    [Header("Drag & Drop")]
    public float grabDistance = 3f;
    public float holdDistance = 2f;
    public LayerMask interactableLayer;

    [Header("Gravity")]
    public float gravity = -9.81f;
    public float groundedOffset = -0.1f;
    private float verticalVelocity = 0f;

    private CharacterController controller;
    private float xRotation = 0f;
    private Transform heldObject = null;

    [SerializeField] private GameObject uiText;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        uiText.SetActive(false);
    }

    void Update()
    {
        HandleMouseLook();
        HandleMovement();
        HandleDragAndDrop();

        if (heldObject != null)
        {
            var os = heldObject.GetComponent<ObjectSwap>();

            if (os != null)
            {
                uiText.SetActive(true);
                if (Input.GetKeyUp(KeyCode.Space))
                {
                    os.SwitchVariant();
                }
            }
        }
        else
        {
            uiText.SetActive(false);
        }
    }

    void HandleMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 move = transform.right * h + transform.forward * v;

        // Apply gravity
        if (controller.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f; // small downward force to stay grounded
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        move.y = verticalVelocity;

        controller.Move(move * moveSpeed * Time.deltaTime);
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleDragAndDrop()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (heldObject == null)
                TryGrab();
            else
                Drop();
        }

        if (heldObject != null)
        {
            Vector3 targetPos = cameraTransform.position + cameraTransform.forward * holdDistance;
            heldObject.position = Vector3.Lerp(heldObject.position, targetPos, Time.deltaTime * 10f);
        }
    }

    void TryGrab()
    {
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, grabDistance, interactableLayer))
        {
            heldObject = hit.transform;
            Rigidbody rb = heldObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.useGravity = false;
                rb.isKinematic = true;
            }
        }
    }

    void Drop()
    {
        if (heldObject != null)
        {
            Rigidbody rb = heldObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
            }

            heldObject = null;
        }
    }
}
