using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class FPSRotation : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float mouseSensitivity = 0.2f;
    [SerializeField] private Vector2 lookLimits = new Vector2(-90f, 90f);

    private float xRotation = 0f;
    private Vector2 lookInput;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "GameScene")
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name != "GameScene")
            return;

        float mouseX = lookInput.x * mouseSensitivity;
        float mouseY = lookInput.y * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, lookLimits.x, lookLimits.y);

        if (cameraTransform != null)
        {
            cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }
    }

    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }
}