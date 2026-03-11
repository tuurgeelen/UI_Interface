using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class SimpleFPSController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float gravity = -20f;

    private CharacterController controller;
    private Vector2 moveInput;
    private Vector3 velocity;
    private bool jumpPressed;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (PauseMenuManager.IsPaused)
            return;

        HandleMovement();
    }

    private void HandleMovement()
    {
        bool isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0f)
            velocity.y = -2f;

        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(move * moveSpeed * Time.deltaTime);

        if (jumpPressed && isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        jumpPressed = false;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    public void OnMove(InputValue value)
    {
        if (PauseMenuManager.IsPaused)
            return;

        moveInput = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        if (PauseMenuManager.IsPaused)
            return;

        if (value.isPressed)
            jumpPressed = true;
    }
}