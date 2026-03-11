using UnityEngine;

[CreateAssetMenu(fileName = "WeaponObject", menuName = "Inventory/Create new Weapon")]
public class WeaponSO : ScriptableObject
{
    public enum WeaponType { Raygun, Pistol, Shotgun, AsaulRifle, Sniper }

    [Header("Weapon type")]
    public WeaponType weaponType = WeaponType.Raygun;

    [Header("Weapon info")]
    public int maxAmmo;
    public int ammocount;
    public float fireDelay = 0.2f;

    [Header("Fire Settings")]
    public bool holdToFire = false;

    [Header("Weapon graphics and object")]
    public Sprite waeponSprite;
    public GameObject weaponprefab;

    [Header("UI Settings")]
    public Vector2 uiScale = Vector2.one;

    [Header("Audio")]
    public AudioClip switchSound;
    [Range(0f,1f)] public float switchVolume = 1f;

    public AudioClip fireSound;
    [Range(0f,1f)] public float fireVolume = 1f;

    public AudioClip reloadSound;
    [Range(0f,1f)] public float reloadVolume = 1f;

    [Header("Reload Settings")]
    public float reloadTime = 1.5f;

    [Header("Projectile settings")]
    public int projectiledamage;

    public enum ProjectileType { Raycast, physicaProjectile }
    public ProjectileType projectileType = ProjectileType.Raycast;

    public float maxRayDistance = Mathf.Infinity;
    public GameObject physicaProjectile;
}