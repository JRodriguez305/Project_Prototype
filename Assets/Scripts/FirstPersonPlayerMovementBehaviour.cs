using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonPlayerMovementBehaviour : MonoBehaviour
{
    public float moveSpeed = 5f; // movement speed also changable in the inspector
    public float mouseSensitivity = 100f; // change to 1500 in the inspector
    public float mouseLookDelay = 0.1f; // mini delay

    private CharacterController characterController;
    private Transform cameraTransform;
    private float xRotation = 0f;

    private float mouseLookTimer = 0f;
    private bool mouseLookEnabled = false;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        mouseLookTimer = mouseLookDelay;
        mouseLookEnabled = false;
    }

    void Update()
    {
        // Adds delay to avoid instant snapping
        if (!mouseLookEnabled)
        {
            mouseLookTimer -= Time.deltaTime;
            mouseLookEnabled = mouseLookTimer <= 0f ? true : mouseLookEnabled;
        }

        if (mouseLookEnabled)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;


            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f); // This should prevent "flipping"

            cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);
        }

        // Player Movement
        float xInput = Input.GetAxis("Horizontal");
        float zInput = Input.GetAxis("Vertical");

        Vector3 moveDirection = transform.right * xInput + transform.forward * zInput;
        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
    }
}
