using UnityEngine;

public class PlayerHealthShield : MonoBehaviour
{
    [Header("Stats")]
    public int maxHealth = 100;
    public int maxShield = 50;

    public int currentHealth;
    public int currentShield;

    [Header("UI")]
    [SerializeField] private HealthShieldUI healthShieldUI;

    private void Start()
    {
        currentHealth = maxHealth;
        currentShield = maxShield;
        UpdateUI();
    }

    public void TakeDamage(int damage)
    {
        if (currentShield > 0)
        {
            currentShield -= damage;

            if (currentShield < 0)
            {
                int remainingDamage = -currentShield;
                currentShield = 0;
                currentHealth -= remainingDamage;
            }
        }
        else
        {
            currentHealth -= damage;
        }

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        currentShield = Mathf.Clamp(currentShield, 0, maxShield);

        UpdateUI();

        Debug.Log("Damage genomen: " + damage + " | Shield: " + currentShield + " | Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Debug.Log("Player is dead");
        }
    }

    private void UpdateUI()
    {
        if (healthShieldUI != null)
            healthShieldUI.UpdateUI(currentHealth, currentShield);
    }
}