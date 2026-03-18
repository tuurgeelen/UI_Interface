using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    [SerializeField] private KeyID keyID;

    [Header("Objective")]
    [SerializeField] private string completeObjectiveID;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip pickupSound;

    public KeyID Key => keyID;

    private void Awake()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    public void Pickup(PlayerKeyInventory inventory)
    {
        inventory.AddKey(keyID);

        if (!string.IsNullOrEmpty(completeObjectiveID) && ObjectiveManager.Instance != null)
        {
            ObjectiveManager.Instance.CompleteObjective(completeObjectiveID);
        }

        if (audioSource != null && pickupSound != null)
        {
            audioSource.PlayOneShot(pickupSound);
            Destroy(gameObject, pickupSound.length);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}