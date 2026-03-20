using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIDataExample : MonoBehaviour
{
    [Header("Main UI")]
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private Image weaponImage;

    [Header("Extra Objects (knife, pistol icons, etc.)")]
    [SerializeField] private GameObject[] extraObjectsToHideWhenNoWeapon;

    public void UpdateUI(int ammoCount, int maxAmmo, Sprite weaponSprite, Vector2 scale)
    {
        bool hasWeapon = weaponSprite != null && maxAmmo > 0;

        if (!hasWeapon)
        {
            ShowEmptyState();
            return;
        }

        // Ammo text
        if (ammoText != null)
        {
            ammoText.gameObject.SetActive(true);
            ammoText.SetText($"{ammoCount:D2}/{maxAmmo}");
        }

        // Weapon image
        if (weaponImage != null)
        {
            weaponImage.gameObject.SetActive(true);
            weaponImage.enabled = true;
            weaponImage.sprite = weaponSprite;
            weaponImage.rectTransform.localScale = scale;
        }

        SetExtraObjectsActive(true);
    }

    public void ShowEmptyState()
    {
        // Ammo weg
        if (ammoText != null)
        {
            ammoText.text = "";
            ammoText.gameObject.SetActive(false);
        }

        // Image weg
        if (weaponImage != null)
        {
            weaponImage.sprite = null;
            weaponImage.enabled = false;
            weaponImage.gameObject.SetActive(false);
        }

        // Extra visuals weg (knife, pistol icons etc.)
        SetExtraObjectsActive(false);
    }

    private void SetExtraObjectsActive(bool state)
    {
        if (extraObjectsToHideWhenNoWeapon == null)
            return;

        for (int i = 0; i < extraObjectsToHideWhenNoWeapon.Length; i++)
        {
            if (extraObjectsToHideWhenNoWeapon[i] != null)
            {
                extraObjectsToHideWhenNoWeapon[i].SetActive(state);
            }
        }
    }
}