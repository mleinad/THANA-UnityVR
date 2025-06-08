using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controller_test : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float lookSpeed = 2f;
    public bool enableMouseLook = true;

    float yaw = 0f;
    float pitch = 0f;

    void Update()
    {
        // Movement
        float horizontal = Input.GetAxis("Horizontal"); // A/D
        float vertical = Input.GetAxis("Vertical");     // W/S

        Vector3 move = transform.right * horizontal + transform.forward * vertical;
        transform.position += move * moveSpeed * Time.deltaTime;

        // Mouse Look
       
            yaw += Input.GetAxis("Mouse X") * lookSpeed;
            pitch -= Input.GetAxis("Mouse Y") * lookSpeed;
            pitch = Mathf.Clamp(pitch, -90f, 90f);

            transform.eulerAngles = new Vector3(pitch, yaw, 0f);
       
    }
}
