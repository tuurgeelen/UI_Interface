using UnityEngine;

[CreateAssetMenu(fileName = "WeaponObject", menuName = "Inventory/Create new Weapon")]
public class WeaponSO : ScriptableObject
{
    public enum WeaponType { Raygun, Pistol, Shotgun, AsaulRifle, Sniper }
    public enum ProjectileType { Raycast, PhysicalProjectile }

    [Header("Weapon type")]
    public WeaponType weaponType = WeaponType.Pistol;

    [Header("Weapon info")]
    public int maxAmmo = 12;
    public float fireDelay = 0.2f;
    public bool holdToFire = false;
    public float reloadTime = 1.5f;

    [Header("Damage")]
    public int projectileDamage = 20;

    [Header("Graphics and object")]
    public Sprite weaponSprite;
    public GameObject weaponPrefab;

    [Header("UI Settings")]
    public Vector2 uiScale = Vector2.one;

    [Header("Audio")]
    public AudioClip switchSound;
    [Range(0f, 1f)] public float switchVolume = 1f;

    public AudioClip fireSound;
    [Range(0f, 1f)] public float fireVolume = 1f;

    public AudioClip reloadSound;
    [Range(0f, 1f)] public float reloadVolume = 1f;

    [Header("Projectile settings")]
    public ProjectileType projectileType = ProjectileType.Raycast;
    public float maxRayDistance = 100f;
    public GameObject physicalProjectile;
    public float projectileSpeed = 40f;
    public float projectileLifeTime = 3f;

    [Header("Effects")]
    public GameObject hitEffectPrefab;
}