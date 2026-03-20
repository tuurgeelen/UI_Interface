using UnityEngine;

public class SlidingDoor : MonoBehaviour
{
    [Header("Door Settings")]
    [SerializeField] private Vector3 openOffset = new Vector3(2f, 0f, 0f);
    [SerializeField] private float openSpeed = 3f;

    private Vector3 closedPosition;
    private Vector3 openPosition;

    private bool isOpen = false;
    private bool isMoving = false;

    private void Start()
    {
        closedPosition = transform.position;
        openPosition = closedPosition + openOffset;
    }

    public void ToggleDoor()
    {
        if (!isMoving)
        {
            StartCoroutine(MoveDoor());
        }
    }

    private System.Collections.IEnumerator MoveDoor()
    {
        isMoving = true;

        Vector3 target = isOpen ? closedPosition : openPosition;

        while (Vector3.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                target,
                openSpeed * Time.deltaTime
            );

            yield return null;
        }

        transform.position = target;
        isOpen = !isOpen;
        isMoving = false;
    }
}