using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float interactDistance = 3f;
    [SerializeField] private LayerMask interactLayer;

    private PlayerKeyInventory inventory;
    private WeaponManager weaponManager;

    private void Awake()
    {
        inventory = GetComponent<PlayerKeyInventory>();
        weaponManager = GetComponentInChildren<WeaponManager>();
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

            WeaponPickup weaponPickup = hit.collider.GetComponentInParent<WeaponPickup>();
            if (weaponPickup != null)
            {
                weaponPickup.Pickup(weaponManager);
                return;
            }

            Door door = hit.collider.GetComponentInParent<Door>();
            if (door != null)
            {
                door.TryOpen(inventory);
                return;
            }
        }
    }
}