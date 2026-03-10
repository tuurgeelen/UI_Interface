using System.Net.Http.Headers;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponObject", menuName = "Inventory/Create new Weapon")]
public class WeaponSO : ScriptableObject
{
    public enum WeaponType { ScopeRifle, Shotgun, PlasmaRifle, Grenadelauncher }
    [Header("Weapon type")]

    public WeaponType weaponType = WeaponType.ScopeRifle;
    [Header("Weapon info ")]

    public int maxAmmo;
    public int ammocount;
    public float fireDelay;
    [Header("Weapon grapics and object")]
    public Sprite waeponSprite;
    public GameObject weaponprefab;

    [Header("Projectile settings")]
    public int projectiledamage;
    public enum ProjectileType{Raycast, physicaProjectile}
    public ProjectileType projectileType = ProjectileType.Raycast;
    public float maxRayDistance = Mathf.Infinity;
    public GameObject physicaProjectile;
}
