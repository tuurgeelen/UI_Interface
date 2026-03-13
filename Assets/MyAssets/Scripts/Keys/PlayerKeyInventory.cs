using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float interactDistance = 3f;
    [SerializeField] private LayerMask interactLayer;

    private PlayerKeyInventory inventory;

    private void Awake()
    {
        inventory = GetComponent<PlayerKeyInventory>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteract();
        }
    }

    private void TryInteract()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, interactLayer))
        {
            KeyPickup key = hit.collider.GetComponentInParent<KeyPickup>();
            if (key != null)
            {
                key.Pickup(inventory);
                return;
            }

            Door door = hit.collider.GetComponentInParent<Door>();
            if (door != null)
            {
                door.TryOpen(inventory);
                return;
            }

            // NIET DOEN:
            // SlidingDoor doorSlide = hit.collider.GetComponentInParent<SlidingDoor>();
            // if (doorSlide != null) doorSlide.ToggleDoor();
        }
    }
}