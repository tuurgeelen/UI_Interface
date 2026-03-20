using UnityEngine;
using UnityEngine.UI;

public class WeaponHUDManager : MonoBehaviour
{
    [Header("Weapon Images")]
    [SerializeField] private Image knifeImage;
    [SerializeField] private Image pistolImage;
    [SerializeField] private Image grenadeImage;

    private void Start()
    {
        HideAllWeaponImages();
    }

    public void ShowWeaponIcon(WeaponSO.WeaponType weaponType)
    {
        switch (weaponType)
        {
            case WeaponSO.WeaponType.Knife:
                if (knifeImage != null)
                    knifeImage.gameObject.SetActive(true);
                break;

            case WeaponSO.WeaponType.Pistol:
                if (pistolImage != null)
                    pistolImage.gameObject.SetActive(true);
                break;

            case WeaponSO.WeaponType.Grenade:
                if (grenadeImage != null)
                    grenadeImage.gameObject.SetActive(true);
                break;
        }
    }

    public void HideAllWeaponImages()
    {
        if (knifeImage != null)
            knifeImage.gameObject.SetActive(false);

        if (pistolImage != null)
            pistolImage.gameObject.SetActive(false);

        if (grenadeImage != null)
            grenadeImage.gameObject.SetActive(false);
    }
}