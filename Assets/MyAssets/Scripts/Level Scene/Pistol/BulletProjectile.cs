using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BulletProjectile : MonoBehaviour
{
    private int damage;
    private float speed;
    private float lifeTime;
    private GameObject hitEffectPrefab;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Initialize(int damageAmount, float projectileSpeed, float projectileLife, GameObject hitEffect)
    {
        damage = damageAmount;
        speed = projectileSpeed;
        lifeTime = projectileLife;
        hitEffectPrefab = hitEffect;

        rb.linearVelocity = transform.forward * speed;
        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        TargetHealth target = collision.collider.GetComponentInParent<TargetHealth>();
        if (target != null)
        {
            target.TakeDamage(damage);
        }

        if (hitEffectPrefab != null)
        {
            ContactPoint contact = collision.contacts[0];
            Instantiate(hitEffectPrefab, contact.point, Quaternion.LookRotation(contact.normal));
        }

        Destroy(gameObject);
    }
}