using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class SimpleFPSController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float gravity = -20f;

    [Header("Footsteps")]
    [SerializeField] private AudioSource footstepSource;
    [SerializeField] private AudioClip footstepClip;

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

        // Landing detectie
        if (!wasGrounded && isGrounded)
        {
            if (landingSource != null && landingClip != null)
                landingSource.PlayOneShot(landingClip);
        }

        if (isGrounded && velocity.y < 0f)
            velocity.y = -2f;

        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(move * moveSpeed * Time.deltaTime);

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

        if (isMoving && isGrounded)
        {
            if (!footstepSource.isPlaying)
                footstepSource.Play();
        }
        else
        {
            if (footstepSource.isPlaying)
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