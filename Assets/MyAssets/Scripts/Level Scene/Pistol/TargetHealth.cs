using UnityEngine;

public class TargetHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private bool useRobotTargetManager = true;

    [Header("Hit Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] hitSounds;

    private int currentHealth;
    private bool isDead = false;

    private void Awake()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        ResetHealth();
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        isDead = false;
    }

    public void TakeDamage(int amount)
    {
        if (isDead)
            return;

        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0);

        PlayRandomHitSound();

        Debug.Log(gameObject.name + " kreeg damage: " + amount + " | health left: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void PlayRandomHitSound()
    {
        if (audioSource == null || hitSounds == null || hitSounds.Length == 0)
            return;

        int randomIndex = Random.Range(0, hitSounds.Length);
        AudioClip clip = hitSounds[randomIndex];

        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    private void Die()
    {
        if (isDead)
            return;

        isDead = true;

        if (useRobotTargetManager && RobotTargetManager.Instance != null)
        {
            bool handled = RobotTargetManager.Instance.TryHitRobot(transform);

            if (handled)
                return;
        }

        Destroy(gameObject);
    }
}