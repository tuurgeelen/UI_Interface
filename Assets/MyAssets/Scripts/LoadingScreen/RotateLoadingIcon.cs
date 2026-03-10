using UnityEngine;

public class RotateLoadingIcon : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 180f;

    private Vector3 pivot;

    void Start()
    {
        pivot = transform.position;
    }

    void Update()
    {
        transform.RotateAround(pivot, Vector3.forward, -rotationSpeed * Time.deltaTime);
    }
}