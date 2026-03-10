using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Sprites;
using System;
using JetBrains.Annotations;
public class UIDataExample : MonoBehaviour
{
    [SerializeField] Slider getSlider;
    [SerializeField] TextMeshProUGUI getText;
    [SerializeField] Image GetImage;


    public void OnInitializeSO(WeaponSO _weaponSO)
    {
        if (_weaponSO == null)
        {
            Debug.LogError("WeaponSO is null, cannot initialize UI values");
        }
        UpdateUI(_weaponSO.ammocount, _weaponSO.maxAmmo, _weaponSO.waeponSprite);

    }

   public void UpdateUI(int _ammoCount, int _maxAmmo, Sprite _weaponSprite)
    {

        UpdateAmmoCountUI(_ammoCount, _maxAmmo);
        if (_weaponSprite != null) GetImage.sprite = _weaponSprite;
    }


    public void UpdateAmmoCountUI(int _ammoCount, int _maxAmmo)
    {
        string CombineText = $"{_ammoCount:D2}/{_maxAmmo}";
        getText.SetText(CombineText);
        getSlider.value = _ammoCount;
        getSlider.maxValue = _maxAmmo;

    }
}
