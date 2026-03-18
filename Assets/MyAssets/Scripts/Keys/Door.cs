using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Lock Settings")]
    [SerializeField] private bool isLocked = true;
    [SerializeField] private KeyID requiredKey;

    [Header("Door Movement")]
    [SerializeField] private SlidingDoor slidingDoor;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip doorSound;

    private void Awake()
    {
        if (slidingDoor == null)
            slidingDoor = GetComponent<SlidingDoor>();

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
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
            ToggleDoor();
            return;
        }

        if (inventory != null && inventory.HasKey(requiredKey))
        {
            Debug.Log("Juiste key gevonden: " + requiredKey);
            isLocked = false;
            ToggleDoor();
            return;
        }

        Debug.Log("Deur is op slot. Vereiste key: " + requiredKey);
    }

    private void ToggleDoor()
    {
        slidingDoor.ToggleDoor();

        if (audioSource != null && doorSound != null)
        {
            audioSource.PlayOneShot(doorSound);
        }
    }
}