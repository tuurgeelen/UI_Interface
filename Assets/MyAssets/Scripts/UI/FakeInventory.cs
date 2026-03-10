using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class NewMonoBehaviourScript : MonoBehaviour
{
    //[SerializeField] WeaponUI[] weapon; //class is een reference type
    [SerializeField] WeaponSO[] WeaponSO;
    [SerializeField] WeaponSO selectedweapon;
    [SerializeField] int index;

    [Header("UI References")]
    [SerializeField] UIDataExample uIDataExample;

    void Start()
    {
        index = 0;
        selectedweapon = WeaponSO[index];
        uIDataExample.OnInitializeSO(selectedweapon);
    }

    void OnScrollWheel(InputValue value)
{
    Debug.Log("Scrolling detected");

    float scrollDirection = value.Get<float>();
    index += (int)scrollDirection;

    index = index % WeaponSO.Length;
    index = index < 0 ? WeaponSO.Length - 1 : index;

    selectedweapon = WeaponSO[index];

    uIDataExample.UpdateUI(
        selectedweapon.ammocount,
        selectedweapon.maxAmmo,
        selectedweapon.waeponSprite);
}
}
#region Class & Struct
[Serializable]
public class WeaponUI
{
    public int maxAmmo;
    public int ammocount;
    public Sprite waeponSprite;
    public float firedelay;


}
[Serializable]
public struct WeaponUIStruct //Struct is een Value type
{
    public int maxAmmo;
    public int ammocount;
    public Sprite waeponSprite;
    public float firedelay;

}
#endregion Class & Struct