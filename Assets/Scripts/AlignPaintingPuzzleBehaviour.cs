using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ignore this script for now
public class AlignPaintingPuzzleBehaviour : MonoBehaviour
{
    public float rotationSpeed = 50f; // speed of painting being "rotated" by the mouse -> also adjustable in the inspector
    private bool isDragging = false;
    private float targetAngle;

    private void OnMouseDown()
    {
        isDragging = true;
    }

    private void OnMouseUp()
    {
        isDragging = false;
    }

    void Update()
    {
        // This will have to appear through a "screen" if issues pop up with player camera movement
        if (isDragging)
        {
            float rotationInput = Input.GetAxis("Mouse X"); // This gets the mouse movement
            transform.Rotate(Vector3.left, rotationInput * rotationSpeed * Time.deltaTime); // Mouse Movement Rotation
        }
    }
}
