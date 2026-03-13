using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Lock Settings")]
    [SerializeField] private bool isLocked = true;
    [SerializeField] private KeyID requiredKey;

    [Header("Door Movement")]
    [SerializeField] private SlidingDoor slidingDoor;

    private void Awake()
    {
        if (slidingDoor == null)
            slidingDoor = GetComponent<SlidingDoor>();
    }

    public void TryOpen(PlayerKeyInventory inventory)
    {
        if (slidingDoor == null)
        {
            Debug.LogWarning($"Geen SlidingDoor gevonden op {gameObject.name}");
            return;
        }

        if (!isLocked)
        {
            slidingDoor.ToggleDoor();
            return;
        }

        if (inventory != null && inventory.HasKey(requiredKey))
        {
            Debug.Log("Juiste key gevonden: " + requiredKey);
            isLocked = false;
            slidingDoor.ToggleDoor();
            return;
        }

        Debug.Log("Deur is op slot. Vereiste key: " + requiredKey);
    }
}