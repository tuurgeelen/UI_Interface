using TMPro;
using UnityEngine;

public class UIDataExample : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ammoText;

    public void UpdateAmmo(int ammoCount, int maxAmmo)
    {
        if (ammoText == null)
            return;

        bool showAmmo = maxAmmo > 0;

        ammoText.gameObject.SetActive(showAmmo);

        if (showAmmo)
            ammoText.SetText($"{ammoCount:D2}/{maxAmmo}");
        else
            ammoText.text = "";
    }

    public void HideAmmo()
    {
        if (ammoText == null)
            return;

        ammoText.text = "";
        ammoText.gameObject.SetActive(false);
    }
}