using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] private WeaponSO weaponToUnlock;

    [Header("Objective")]
    [SerializeField] private string completeObjectiveID;
    [SerializeField] private string requiredObjectiveIDToPickup;

    [Header("HUD")]
    [SerializeField] private WeaponHUDManager weaponHUDManager;

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
        if (ObjectiveManager.Instance != null && !string.IsNullOrEmpty(requiredObjectiveIDToPickup))
        {
            string currentObjective = ObjectiveManager.Instance.GetCurrentObjectiveID();

            if (currentObjective != requiredObjectiveIDToPickup)
            {
                Debug.Log("Je kan dit wapen nog niet oppakken. Vereiste objective: " + requiredObjectiveIDToPickup);
                return;
            }
        }

        if (weaponToUnlock == null)
            return;

        if (weaponToUnlock.weaponType == WeaponSO.WeaponType.Grenade)
        {
            if (weaponHUDManager != null)
            {
                weaponHUDManager.ShowWeaponIcon(weaponToUnlock.weaponType);
            }

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

            return;
        }

        if (weaponManager == null)
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