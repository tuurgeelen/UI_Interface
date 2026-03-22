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
        Debug.Log("ShowWeaponIcon called for: " + weaponType);

        switch (weaponType)
        {
            case WeaponSO.WeaponType.Knife:
                Debug.Log("Show KNIFE image");
                if (knifeImage != null)
                    knifeImage.gameObject.SetActive(true);
                else
                    Debug.LogWarning("knifeImage is NULL");
                break;

            case WeaponSO.WeaponType.Pistol:
                Debug.Log("Show PISTOL image");
                if (pistolImage != null)
                    pistolImage.gameObject.SetActive(true);
                else
                    Debug.LogWarning("pistolImage is NULL");
                break;

            case WeaponSO.WeaponType.Grenade:
                Debug.Log("Show GRENADE image");
                if (grenadeImage != null)
                    grenadeImage.gameObject.SetActive(true);
                else
                    Debug.LogWarning("grenadeImage is NULL");
                break;

            default:
                Debug.LogWarning("Weapon type not handled: " + weaponType);
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