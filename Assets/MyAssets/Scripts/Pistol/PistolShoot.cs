using UnityEngine;

public class PistolShoot : MonoBehaviour
{
    [Header("References")]
    public Camera fpsCamera;
    public Animator animator;
    public WeaponMotion weaponMotion;

    [Header("Shoot Settings")]
    public float range = 100f;
    public int damage = 20;
    public float fireRate = 0.2f;

    [Header("Ammo")]
    public int magSize = 12;
    public int currentAmmo = 12;
    public float reloadTime = 1.5f;

    private bool isReloading = false;
    private float nextFireTime = 0f;

    void Update()
    {
        if (isReloading)
            return;

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartReload();
            return;
        }

        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        if (currentAmmo <= 0)
        {
            StartReload();
            return;
        }

        currentAmmo--;

        if (animator != null)
            animator.SetTrigger("Shoot");

        if (weaponMotion != null)
            weaponMotion.AddRecoil();

        Ray ray = new Ray(fpsCamera.transform.position, fpsCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, range))
        {
            Debug.Log("Hit: " + hit.collider.name);

            TargetHealth target = hit.collider.GetComponent<TargetHealth>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }
        }
    }

    void StartReload()
    {
        if (currentAmmo == magSize)
            return;

        isReloading = true;

        if (animator != null)
            animator.SetTrigger("Reload");

        Invoke(nameof(FinishReload), reloadTime);
    }

    void FinishReload()
    {
        currentAmmo = magSize;
        isReloading = false;
    }
}