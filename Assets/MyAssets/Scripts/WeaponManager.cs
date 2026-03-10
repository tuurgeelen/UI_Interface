using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] WeaponSO[] weapons;
    [SerializeField] UIDataExample uiDataExample;

    private int index = 0;
    private WeaponSO selectedWeapon;

    void Start()
    {
        if (weapons == null || weapons.Length == 0)
        {
            Debug.LogError("Geen wapens ingevuld in WeaponManager.");
            return;
        }

        SelectWeapon(index);
    }

    void Update()
    {
        HandleScroll();
    }

    void HandleScroll()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll > 0f)
        {
            index++;
            if (index >= weapons.Length)
                index = 0;

            SelectWeapon(index);
        }
        else if (scroll < 0f)
        {
            index--;
            if (index < 0)
                index = weapons.Length - 1;

            SelectWeapon(index);
        }
    }

    void SelectWeapon(int newIndex)
    {
        selectedWeapon = weapons[newIndex];

        if (uiDataExample != null)
        {
            uiDataExample.UpdateUI(
                selectedWeapon.ammocount,
                selectedWeapon.maxAmmo,
                selectedWeapon.waeponSprite
            );
        }

        Debug.Log("Geselecteerd wapen: " + selectedWeapon.name);
    }
}