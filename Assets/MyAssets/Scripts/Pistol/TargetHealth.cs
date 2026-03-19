using UnityEngine;

public class TargetHealth : MonoBehaviour
{
    [SerializeField] private int health = 100;

    public void TakeDamage(int amount)
    {
        health -= amount;
        Debug.Log(gameObject.name + " damage: " + amount + " | health left: " + health);

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}