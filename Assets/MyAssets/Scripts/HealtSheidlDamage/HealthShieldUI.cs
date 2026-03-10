using UnityEngine;
using UnityEngine.UI;

public class HealthShieldUI : MonoBehaviour
{
    [SerializeField] private Image[] shieldBlocks;
    [SerializeField] private Image[] healthBlocks;

    [SerializeField] private Color shieldOnColor = Color.cyan;
    [SerializeField] private Color shieldOffColor = new Color(0.15f, 0.15f, 0.15f, 0.5f);

    [SerializeField] private Color healthOnColor = Color.green;
    [SerializeField] private Color healthOffColor = new Color(0.15f, 0.15f, 0.15f, 0.5f);

    public void UpdateShield(int currentShield)
    {
        int activeBlocks = Mathf.CeilToInt(currentShield / 10f);

        for (int i = 0; i < shieldBlocks.Length; i++)
        {
            if (shieldBlocks[i] != null)
                shieldBlocks[i].color = i < activeBlocks ? shieldOnColor : shieldOffColor;
        }
    }

    public void UpdateHealth(int currentHealth)
    {
        int activeBlocks = Mathf.CeilToInt(currentHealth / 10f);

        for (int i = 0; i < healthBlocks.Length; i++)
        {
            if (healthBlocks[i] != null)
                healthBlocks[i].color = i < activeBlocks ? healthOnColor : healthOffColor;
        }
    }

    public void UpdateUI(int currentHealth, int currentShield)
    {
        UpdateHealth(currentHealth);
        UpdateShield(currentShield);
    }
}