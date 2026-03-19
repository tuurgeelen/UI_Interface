using UnityEngine;

public class WeaponMotion : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SimpleFPSController fpsController;

    [Header("Sway")]
    public float swayAmount = 0.02f;
    public float maxSwayAmount = 0.06f;
    public float swaySmooth = 8f;

    [Header("Rotation Sway")]
    public float rotationSwayAmount = 4f;
    public float maxRotationSwayAmount = 8f;
    public float rotationSmooth = 10f;

    [Header("Walk Bobbing")]
    public float bobFrequency = 7f;
    public float bobHorizontalAmplitude = 0.03f;
    public float bobVerticalAmplitude = 0.02f;
    public float bobSmooth = 10f;

    [Header("Sprint Bobbing")]
    public float sprintBobFrequency = 11f;
    public float sprintBobHorizontalAmplitude = 0.05f;
    public float sprintBobVerticalAmplitude = 0.035f;

    [Header("Idle Movement")]
    public float idleFrequency = 1.5f;
    public float idleHorizontalAmplitude = 0.005f;
    public float idleVerticalAmplitude = 0.01f;
    public float idleRotationAmount = 1.2f;

    [Header("Recoil")]
    public float recoilKickBack = 0.08f;
    public float recoilRotationX = -8f;
    public float recoilRotationY = 3f;
    public float recoilRotationZ = 2f;
    public float recoilReturnSpeed = 8f;
    public float recoilSnappiness = 14f;

    [Header("Movement Input")]
    public bool useRawInput = true;

    private Vector3 initialLocalPosition;
    private Quaternion initialLocalRotation;

    private Vector3 currentRecoilPosition;
    private Vector3 targetRecoilPosition;

    private Vector3 currentRecoilRotation;
    private Vector3 targetRecoilRotation;

    private float bobTime;
    private float idleTime;

    void Start()
    {
        initialLocalPosition = transform.localPosition;
        initialLocalRotation = transform.localRotation;

        if (fpsController == null)
            fpsController = GetComponentInParent<SimpleFPSController>();
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        float moveX;
        float moveY;
        bool isSprinting = false;
        bool isMoving = false;

        if (fpsController != null)
        {
            Vector2 input = fpsController.MoveInput;
            moveX = input.x;
            moveY = input.y;
            isSprinting = fpsController.IsSprinting;
            isMoving = input.magnitude > 0.1f;
        }
        else
        {
            moveX = useRawInput ? Input.GetAxisRaw("Horizontal") : Input.GetAxis("Horizontal");
            moveY = useRawInput ? Input.GetAxisRaw("Vertical") : Input.GetAxis("Vertical");
            isMoving = new Vector2(moveX, moveY).magnitude > 0.1f;
        }

        Vector3 swayPosition = CalculateSwayPosition(mouseX, mouseY);
        Quaternion swayRotation = CalculateSwayRotation(mouseX, mouseY);

        Vector3 movementOffset = CalculateMovementOffset(moveX, moveY, isMoving, isSprinting);
        Quaternion idleRotation = CalculateIdleRotation(isMoving);

        HandleRecoil();

        Vector3 finalPosition =
            initialLocalPosition +
            swayPosition +
            movementOffset +
            currentRecoilPosition;

        Quaternion finalRotation =
            initialLocalRotation *
            swayRotation *
            idleRotation *
            Quaternion.Euler(currentRecoilRotation);

        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            finalPosition,
            Time.deltaTime * swaySmooth
        );

        transform.localRotation = Quaternion.Slerp(
            transform.localRotation,
            finalRotation,
            Time.deltaTime * rotationSmooth
        );
    }

    Vector3 CalculateSwayPosition(float mouseX, float mouseY)
    {
        float invertX = -mouseX * swayAmount;
        float invertY = -mouseY * swayAmount;

        invertX = Mathf.Clamp(invertX, -maxSwayAmount, maxSwayAmount);
        invertY = Mathf.Clamp(invertY, -maxSwayAmount, maxSwayAmount);

        return new Vector3(invertX, invertY, 0f);
    }

    Quaternion CalculateSwayRotation(float mouseX, float mouseY)
    {
        float rotX = Mathf.Clamp(-mouseY * rotationSwayAmount, -maxRotationSwayAmount, maxRotationSwayAmount);
        float rotY = Mathf.Clamp(mouseX * rotationSwayAmount, -maxRotationSwayAmount, maxRotationSwayAmount);

        return Quaternion.Euler(rotX, rotY, 0f);
    }

    Vector3 CalculateMovementOffset(float moveX, float moveY, bool isMoving, bool isSprinting)
    {
        if (isMoving)
        {
            float frequency = isSprinting ? sprintBobFrequency : bobFrequency;
            float horizontalAmp = isSprinting ? sprintBobHorizontalAmplitude : bobHorizontalAmplitude;
            float verticalAmp = isSprinting ? sprintBobVerticalAmplitude : bobVerticalAmplitude;

            bobTime += Time.deltaTime * frequency;

            float bobX = Mathf.Cos(bobTime) * horizontalAmp;
            float bobY = Mathf.Sin(bobTime * 2f) * verticalAmp;

            return new Vector3(bobX, bobY, 0f);
        }
        else
        {
            bobTime = 0f;

            idleTime += Time.deltaTime * idleFrequency;

            float idleX = Mathf.Sin(idleTime) * idleHorizontalAmplitude;
            float idleY = Mathf.Cos(idleTime * 2f) * idleVerticalAmplitude;

            return new Vector3(idleX, idleY, 0f);
        }
    }

    Quaternion CalculateIdleRotation(bool isMoving)
    {
        if (isMoving)
            return Quaternion.identity;

        float rotZ = Mathf.Sin(idleTime) * idleRotationAmount;
        float rotX = Mathf.Cos(idleTime * 0.5f) * (idleRotationAmount * 0.35f);

        return Quaternion.Euler(rotX, 0f, rotZ);
    }

    void HandleRecoil()
    {
        targetRecoilPosition = Vector3.Lerp(targetRecoilPosition, Vector3.zero, recoilReturnSpeed * Time.deltaTime);
        currentRecoilPosition = Vector3.Lerp(currentRecoilPosition, targetRecoilPosition, recoilSnappiness * Time.deltaTime);

        targetRecoilRotation = Vector3.Lerp(targetRecoilRotation, Vector3.zero, recoilReturnSpeed * Time.deltaTime);
        currentRecoilRotation = Vector3.Lerp(currentRecoilRotation, targetRecoilRotation, recoilSnappiness * Time.deltaTime);
    }

    public void AddRecoil()
    {
        targetRecoilPosition += new Vector3(0f, 0f, -recoilKickBack);

        targetRecoilRotation += new Vector3(
            recoilRotationX,
            Random.Range(-recoilRotationY, recoilRotationY),
            Random.Range(-recoilRotationZ, recoilRotationZ)
        );
    }
}