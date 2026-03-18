using UnityEngine;

public class WeaponMotion : MonoBehaviour
{
    [Header("Sway")]
    public float swayAmount = 0.02f;
    public float maxSwayAmount = 0.06f;
    public float swaySmooth = 8f;

    [Header("Rotation Sway")]
    public float rotationSwayAmount = 4f;
    public float maxRotationSwayAmount = 8f;
    public float rotationSmooth = 10f;

    [Header("Bobbing")]
    public float bobFrequency = 7f;
    public float bobHorizontalAmplitude = 0.03f;
    public float bobVerticalAmplitude = 0.02f;
    public float bobSmooth = 10f;

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

    void Start()
    {
        initialLocalPosition = transform.localPosition;
        initialLocalRotation = transform.localRotation;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        float moveX = useRawInput ? Input.GetAxisRaw("Horizontal") : Input.GetAxis("Horizontal");
        float moveY = useRawInput ? Input.GetAxisRaw("Vertical") : Input.GetAxis("Vertical");

        Vector3 swayPosition = CalculateSwayPosition(mouseX, mouseY);
        Quaternion swayRotation = CalculateSwayRotation(mouseX, mouseY);

        Vector3 bobOffset = CalculateBobOffset(moveX, moveY);

        HandleRecoil();

        Vector3 finalPosition =
            initialLocalPosition +
            swayPosition +
            bobOffset +
            currentRecoilPosition;

        Quaternion finalRotation =
            initialLocalRotation *
            swayRotation *
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

    Vector3 CalculateBobOffset(float moveX, float moveY)
    {
        float moveAmount = new Vector2(moveX, moveY).magnitude;

        if (moveAmount > 0.1f)
        {
            bobTime += Time.deltaTime * bobFrequency;
            float bobX = Mathf.Cos(bobTime) * bobHorizontalAmplitude;
            float bobY = Mathf.Sin(bobTime * 2f) * bobVerticalAmplitude;

            Vector3 targetBob = new Vector3(bobX, bobY, 0f);
            return Vector3.Lerp(Vector3.zero, targetBob, Time.deltaTime * bobSmooth);
        }
        else
        {
            bobTime = 0f;
            return Vector3.Lerp(Vector3.zero, Vector3.zero, Time.deltaTime * bobSmooth);
        }
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