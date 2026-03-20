using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class SimpleFPSController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float sprintSpeed = 8f;
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float gravity = -20f;

    [Header("Footsteps")]
    [SerializeField] private AudioSource footstepSource;
    [SerializeField] private AudioClip footstepClip;
    [SerializeField] private float walkFootstepPitch = 1f;
    [SerializeField] private float sprintFootstepPitch = 1.25f;

    [Header("Jump")]
    [SerializeField] private AudioSource jumpSource;
    [SerializeField] private AudioClip jumpClip;

    [Header("Landing")]
    [SerializeField] private AudioSource landingSource;
    [SerializeField] private AudioClip landingClip;

    private CharacterController controller;
    private Vector2 moveInput;
    private Vector3 velocity;
    private bool jumpPressed;
    private bool wasGrounded;

    public bool IsSprinting { get; private set; }
    public bool IsMoving => moveInput.magnitude > 0.1f && controller != null && controller.isGrounded;
    public Vector2 MoveInput => moveInput;
    public bool IsGrounded => controller != null && controller.isGrounded;
    public bool IsAirborne => !IsGrounded;
    public bool JustLanded { get; private set; }

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        if (footstepSource != null)
        {
            footstepSource.clip = footstepClip;
            footstepSource.loop = true;
            footstepSource.playOnAwake = false;
        }
    }

    private void Update()
    {
        if (PauseMenuManager.IsPaused)
            return;

        HandleMovement();
        HandleFootsteps();
    }

    private void HandleMovement()
    {
        bool isGrounded = controller.isGrounded;
        JustLanded = false;

        if (!wasGrounded && isGrounded)
        {
            JustLanded = true;

            if (landingSource != null && landingClip != null)
                landingSource.PlayOneShot(landingClip);
        }

        if (isGrounded && velocity.y < 0f)
            velocity.y = -2f;

        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;

        bool hasMoveInput = moveInput.magnitude > 0.1f;

        bool shiftHeld =
            Keyboard.current != null &&
            Keyboard.current.leftShiftKey.isPressed;

        IsSprinting = shiftHeld && hasMoveInput && isGrounded;

        float currentSpeed = IsSprinting ? sprintSpeed : walkSpeed;
        controller.Move(move * currentSpeed * Time.deltaTime);

        if (jumpPressed && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            if (jumpSource != null && jumpClip != null)
                jumpSource.PlayOneShot(jumpClip);
        }

        jumpPressed = false;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        wasGrounded = isGrounded;
    }

    private void HandleFootsteps()
    {
        bool isMoving = moveInput.magnitude > 0.1f;
        bool isGrounded = controller.isGrounded;

        if (footstepSource != null)
            footstepSource.pitch = IsSprinting ? sprintFootstepPitch : walkFootstepPitch;

        if (isMoving && isGrounded)
        {
            if (footstepSource != null && !footstepSource.isPlaying)
                footstepSource.Play();
        }
        else
        {
            if (footstepSource != null && footstepSource.isPlaying)
                footstepSource.Stop();
        }
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