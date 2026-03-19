using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] private WeaponSO weaponToUnlock;

    [Header("Objective")]
    [SerializeField] private string completeObjectiveID;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip pickupSound;

    private void Awake()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    public void Pickup(WeaponManager weaponManager)
    {
        if (weaponManager == null || weaponToUnlock == null)
            return;

        if (!weaponManager.HasWeapon(weaponToUnlock))
        {
            weaponManager.UnlockWeapon(weaponToUnlock);

            if (!string.IsNullOrEmpty(completeObjectiveID) && ObjectiveManager.Instance != null)
            {
                ObjectiveManager.Instance.CompleteObjective(completeObjectiveID);
            }
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