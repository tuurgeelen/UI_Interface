using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class WeaponManager : MonoBehaviour
{
    [Header("Weapons")]
    [SerializeField] private WeaponSO[] weapons;
    [SerializeField] private UIDataExample uiDataExample;

    [Header("Settings")]
    [SerializeField] private float scrollThreshold = 0.05f;
    [SerializeField] private float switchCooldown = 0.25f;

    private int index = 0;
    private WeaponSO selectedWeapon;

    private AudioSource audioSource;

    private float nextSwitchTime;
    private float nextFireTime;

    private bool isReloading;
    private float reloadFinishTime;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (weapons == null || weapons.Length == 0)
        {
            Debug.LogError("Geen wapens ingevuld in WeaponManager.");
            return;
        }

        index = Mathf.Clamp(index, 0, weapons.Length - 1);
        SelectWeapon(index, false);
    }

    private void Update()
    {
        if (PauseMenuManager.IsPaused)
            return;

        HandleScrollSwitch();
        HandleFire();
        HandleReload();
    }

    private void HandleScrollSwitch()
    {
        if (Time.time < nextSwitchTime)
            return;

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll > scrollThreshold)
        {
            index++;
            if (index >= weapons.Length)
                index = 0;

            SelectWeapon(index, true);
            nextSwitchTime = Time.time + switchCooldown;
        }
        else if (scroll < -scrollThreshold)
        {
            index--;
            if (index < 0)
                index = weapons.Length - 1;

            SelectWeapon(index, true);
            nextSwitchTime = Time.time + switchCooldown;
        }
    }

    private void HandleFire()
    {
        if (isReloading)
            return;

        if (selectedWeapon == null)
            return;

        if (Time.time < nextFireTime)
            return;

        bool wantsToFire = selectedWeapon.holdToFire
            ? Input.GetMouseButton(0)
            : Input.GetMouseButtonDown(0);

        if (!wantsToFire)
            return;

        if (selectedWeapon.ammocount <= 0)
        {
            Debug.Log("Geen ammo meer in " + selectedWeapon.weaponType);
            return;
        }

        selectedWeapon.ammocount--;

        if (selectedWeapon.fireSound != null)
        {
            audioSource.PlayOneShot(
                selectedWeapon.fireSound,
                selectedWeapon.fireVolume
            );
        }

        UpdateWeaponUI();

        nextFireTime = Time.time + selectedWeapon.fireDelay;

        Debug.Log("Geschoten met: " + selectedWeapon.weaponType + " | Ammo: " + selectedWeapon.ammocount);
    }

    private void HandleReload()
    {
        if (selectedWeapon == null)
            return;

        if (Input.GetKeyDown(KeyCode.R) && !isReloading)
        {
            if (selectedWeapon.ammocount >= selectedWeapon.maxAmmo)
                return;

            StartReload();
        }

        if (isReloading && Time.time >= reloadFinishTime)
        {
            FinishReload();
        }
    }

    private void StartReload()
    {
        isReloading = true;
        reloadFinishTime = Time.time + selectedWeapon.reloadTime;

        if (selectedWeapon.reloadSound != null)
        {
            audioSource.PlayOneShot(
                selectedWeapon.reloadSound,
                selectedWeapon.reloadVolume
            );
        }

        Debug.Log("Reloading " + selectedWeapon.weaponType);
    }

    private void FinishReload()
    {
        isReloading = false;
        selectedWeapon.ammocount = selectedWeapon.maxAmmo;

        UpdateWeaponUI();

        Debug.Log("Reload complete");
    }

    private void SelectWeapon(int newIndex, bool playSound)
    {
        if (weapons == null || weapons.Length == 0)
            return;

        if (newIndex < 0 || newIndex >= weapons.Length)
            return;

        index = newIndex;
        selectedWeapon = weapons[index];

        if (selectedWeapon == null)
            return;

        isReloading = false;

        if (playSound && selectedWeapon.switchSound != null)
        {
            audioSource.PlayOneShot(
                selectedWeapon.switchSound,
                selectedWeapon.switchVolume
            );
        }

        UpdateWeaponUI();

        Debug.Log("Geselecteerd wapen: " + selectedWeapon.weaponType);
    }

    private void UpdateWeaponUI()
    {
        if (uiDataExample != null && selectedWeapon != null)
        {
            uiDataExample.UpdateUI(
                selectedWeapon.ammocount,
                selectedWeapon.maxAmmo,
                selectedWeapon.waeponSprite,
                selectedWeapon.uiScale
            );
        }
    }
}