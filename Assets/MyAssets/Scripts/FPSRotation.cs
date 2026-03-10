using UnityEngine;

public class FPSRotation : MonoBehaviour
{
    [SerializeField] Transform cameraTransform;
    [SerializeField] float mouseSensitivity = 200f;
    [SerializeField] Vector2 lookLimits = new Vector2(-90f, 90f);

    private float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, lookLimits.x, lookLimits.y);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}