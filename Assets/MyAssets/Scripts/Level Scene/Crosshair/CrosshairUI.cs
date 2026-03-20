using UnityEngine;
using UnityEngine.UI;

public class CrosshairUI : MonoBehaviour
{
    [SerializeField] private Image crosshairImage;
    [SerializeField] private WeaponManager weaponManager;

    private void Awake()
    {
        if (crosshairImage == null)
            crosshairImage = GetComponent<Image>();
    }

    private void Update()
    {
        if (crosshairImage == null || weaponManager == null)
            return;

        bool hasWeapon = weaponManager.GetCurrentWeapon() != null;
        bool shouldShow = hasWeapon && !PauseMenuManager.IsPaused;

        crosshairImage.enabled = shouldShow;
    }
}