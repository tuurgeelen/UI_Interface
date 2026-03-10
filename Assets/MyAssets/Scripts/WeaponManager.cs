using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private WeaponSO[] weapons;
    [SerializeField] private UIDataExample uiDataExample;

    private int index = 0;
    private WeaponSO selectedWeapon;

    private void Start()
    {
        if (weapons == null || weapons.Length == 0)
        {
            Debug.LogError("Geen wapens ingevuld in WeaponManager.");
            return;
        }

        SelectWeapon(index);
    }

    public void OnScrollWheel(InputValue value)
    {
        Vector2 scroll = value.Get<Vector2>();

        if (scroll.y > 0f)
        {
            index++;
            if (index >= weapons.Length)
                index = 0;

            SelectWeapon(index);
        }
        else if (scroll.y < 0f)
        {
            index--;
            if (index < 0)
                index = weapons.Length - 1;

            SelectWeapon(index);
        }
    }

    private void SelectWeapon(int newIndex)
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