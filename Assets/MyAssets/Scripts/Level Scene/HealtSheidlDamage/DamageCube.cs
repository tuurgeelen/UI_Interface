using UnityEngine;

public class DamageCube : MonoBehaviour
{
    [SerializeField] private int damageAmount = 10;

    private void OnTriggerEnter(Collider other)
    {
        PlayerHealthShield player = other.GetComponent<PlayerHealthShield>();

        if (player != null)
        {
            player.TakeDamage(damageAmount);
        }
    }
}