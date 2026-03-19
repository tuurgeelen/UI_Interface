using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class WeaponManager : MonoBehaviour
{
    [System.Serializable]
    public class WeaponEntry
    {
        public WeaponSO weaponData;
        public GameObject weaponObject;
        public Animator animator;
    }

    [Header("Weapons")]
    [SerializeField] private WeaponEntry[] weapons;
    [SerializeField] private UIDataExample uiDataExample;

    [Header("References")]
    [SerializeField] private Camera fpsCamera;
    [SerializeField] private WeaponMotion weaponMotion;
    [SerializeField] private Transform projectileSpawnPoint;

    [Header("Switch Settings")]
    [SerializeField] private float scrollThreshold = 0.05f;
    [SerializeField] private float switchCooldown = 0.25f;

    private AudioSource audioSource;

    private bool[] unlockedWeapons;
    private int[] currentAmmo;

    private int currentIndex = -1;
    private WeaponEntry currentWeapon;

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

        unlockedWeapons = new bool[weapons.Length];
        currentAmmo = new int[weapons.Length];

        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i].weaponData != null)
            {
                currentAmmo[i] = weapons[i].weaponData.maxAmmo;
            }

            if (weapons[i].weaponObject != null)
            {
                weapons[i].weaponObject.SetActive(false);
            }
        }

        currentIndex = -1;
        currentWeapon = null;
        ClearWeaponUI();
    }

    private void Update()
    {
        if (PauseMenuManager.IsPaused)
            return;

        HandleReload();

        if (currentWeapon == null)
            return;

        HandleScrollSwitch();
        HandleFire();
    }

    private void HandleScrollSwitch()
    {
        if (Time.time < nextSwitchTime)
            return;

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll > scrollThreshold)
        {
            SelectNextWeapon(1);
        }
        else if (scroll < -scrollThreshold)
        {
            SelectNextWeapon(-1);
        }
    }

    private void SelectNextWeapon(int direction)
    {
        if (weapons == null || weapons.Length == 0)
            return;

        int startIndex = currentIndex < 0 ? 0 : currentIndex;
        int checkIndex = startIndex;

        for (int i = 0; i < weapons.Length; i++)
        {
            checkIndex += direction;

            if (checkIndex >= weapons.Length)
                checkIndex = 0;
            else if (checkIndex < 0)
                checkIndex = weapons.Length - 1;

            if (unlockedWeapons[checkIndex])
            {
                SelectWeapon(checkIndex, true);
                nextSwitchTime = Time.time + switchCooldown;
                return;
            }
        }
    }

    private void HandleFire()
    {
        if (isReloading)
            return;

        if (currentWeapon == null || currentWeapon.weaponData == null)
            return;

        if (Time.time < nextFireTime)
            return;

        WeaponSO data = currentWeapon.weaponData;

        bool wantsToFire = data.holdToFire
            ? Input.GetMouseButton(0)
            : Input.GetMouseButtonDown(0);

        if (!wantsToFire)
            return;

        if (currentAmmo[currentIndex] <= 0)
        {
            StartReload();
            return;
        }

        currentAmmo[currentIndex]--;

        if (currentWeapon.animator != null)
            currentWeapon.animator.SetTrigger("Shoot");

        if (weaponMotion != null)
            weaponMotion.AddRecoil();

        if (data.fireSound != null)
        {
            audioSource.PlayOneShot(data.fireSound, data.fireVolume);
        }

        FireWeapon(data);

        UpdateWeaponUI();
        nextFireTime = Time.time + data.fireDelay;
    }

    private void FireWeapon(WeaponSO data)
    {
        if (data.projectileType == WeaponSO.ProjectileType.Raycast)
        {
            FireRaycast(data);
        }
        else
        {
            FireProjectile(data);
        }
    }

    private void FireRaycast(WeaponSO data)
    {
        if (fpsCamera == null)
        {
            Debug.LogWarning("fpsCamera ontbreekt in WeaponManager.");
            return;
        }

        Ray ray = new Ray(fpsCamera.transform.position, fpsCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, data.maxRayDistance))
        {
            Debug.Log("Hit: " + hit.collider.name);

            TargetHealth target = hit.collider.GetComponentInParent<TargetHealth>();
            if (target != null)
            {
                target.TakeDamage(data.projectileDamage);
            }

            if (data.hitEffectPrefab != null)
            {
                Quaternion rot = Quaternion.LookRotation(hit.normal);
                Instantiate(data.hitEffectPrefab, hit.point, rot);
            }
        }
    }

    private void FireProjectile(WeaponSO data)
    {
        if (data.physicalProjectile == null)
        {
            Debug.LogWarning("Geen physical projectile prefab ingesteld op " + data.weaponType);
            return;
        }

        Transform spawnPoint = projectileSpawnPoint != null
            ? projectileSpawnPoint
            : fpsCamera != null ? fpsCamera.transform : transform;

        GameObject bullet = Instantiate(
            data.physicalProjectile,
            spawnPoint.position,
            spawnPoint.rotation
        );

        BulletProjectile projectile = bullet.GetComponent<BulletProjectile>();
        if (projectile != null)
        {
            projectile.Initialize(
                data.projectileDamage,
                data.projectileSpeed,
                data.projectileLifeTime,
                data.hitEffectPrefab
            );
        }
        else
        {
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = spawnPoint.forward * data.projectileSpeed;
            }

            Destroy(bullet, data.projectileLifeTime);
        }
    }

    private void HandleReload()
    {
        if (currentWeapon == null || currentWeapon.weaponData == null)
            return;

        if (Input.GetKeyDown(KeyCode.R) && !isReloading)
        {
            if (currentAmmo[currentIndex] >= currentWeapon.weaponData.maxAmmo)
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
        if (currentWeapon == null || currentWeapon.weaponData == null)
            return;

        isReloading = true;
        reloadFinishTime = Time.time + currentWeapon.weaponData.reloadTime;

        if (currentWeapon.animator != null)
            currentWeapon.animator.SetTrigger("Reload");

        if (currentWeapon.weaponData.reloadSound != null)
        {
            audioSource.PlayOneShot(
                currentWeapon.weaponData.reloadSound,
                currentWeapon.weaponData.reloadVolume
            );
        }
    }

    private void FinishReload()
    {
        if (currentWeapon == null || currentWeapon.weaponData == null)
            return;

        isReloading = false;
        currentAmmo[currentIndex] = currentWeapon.weaponData.maxAmmo;
        UpdateWeaponUI();
    }

    public void UnlockWeapon(WeaponSO weaponToUnlock)
    {
        if (weaponToUnlock == null)
        {
            Debug.LogWarning("weaponToUnlock is null");
            return;
        }

        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i].weaponData == weaponToUnlock)
            {
                bool wasAlreadyUnlocked = unlockedWeapons[i];
                unlockedWeapons[i] = true;

                if (!wasAlreadyUnlocked)
                {
                    currentAmmo[i] = weaponToUnlock.maxAmmo;
                }

                SelectWeapon(i, !wasAlreadyUnlocked);
                Debug.Log("Weapon unlocked: " + weaponToUnlock.weaponType);
                return;
            }
        }

        Debug.LogWarning("WeaponSO niet gevonden in WeaponManager: " + weaponToUnlock.name);
    }

    private void SelectWeapon(int newIndex, bool playSound)
    {
        if (newIndex < 0 || newIndex >= weapons.Length)
            return;

        if (!unlockedWeapons[newIndex])
            return;

        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i].weaponObject != null)
            {
                weapons[i].weaponObject.SetActive(i == newIndex);
            }
        }

        currentIndex = newIndex;
        currentWeapon = weapons[newIndex];
        isReloading = false;

        if (playSound && currentWeapon.weaponData != null && currentWeapon.weaponData.switchSound != null)
        {
            audioSource.PlayOneShot(
                currentWeapon.weaponData.switchSound,
                currentWeapon.weaponData.switchVolume
            );
        }

        UpdateWeaponUI();
    }

    private void UpdateWeaponUI()
    {
        if (uiDataExample == null)
            return;

        if (currentWeapon == null || currentWeapon.weaponData == null)
        {
            uiDataExample.ShowEmptyState();
            return;
        }

        uiDataExample.UpdateUI(
            currentAmmo[currentIndex],
            currentWeapon.weaponData.maxAmmo,
            currentWeapon.weaponData.weaponSprite,
            currentWeapon.weaponData.uiScale
        );
    }

    private void ClearWeaponUI()
    {
        if (uiDataExample != null)
        {
            uiDataExample.ShowEmptyState();
        }
    }

    public bool HasWeapon(WeaponSO weaponToCheck)
    {
        if (weaponToCheck == null)
            return false;

        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i].weaponData == weaponToCheck)
            {
                return unlockedWeapons[i];
            }
        }

        return false;
    }

    public WeaponSO GetCurrentWeapon()
    {
        if (currentWeapon == null)
            return null;

        return currentWeapon.weaponData;
    }

    public int GetCurrentAmmo()
    {
        if (currentIndex < 0 || currentIndex >= currentAmmo.Length)
            return 0;

        return currentAmmo[currentIndex];
    }

    public bool IsReloading()
    {
        return isReloading;
    }
}