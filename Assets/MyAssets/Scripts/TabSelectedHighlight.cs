using UnityEngine;
using UnityEngine.UI;

public class TabSelectedHighlight : MonoBehaviour
{
    [SerializeField] private Image[] tabBackgrounds;

    [Header("Colors")]
    [SerializeField] private Color normalColor = new Color32(5, 18, 18, 255);
    [SerializeField] private Color selectedColor = new Color32(70, 224, 208, 255);

    private Image _currentSelected;

    private void Start()
    {
        // Optioneel: zet de eerste tab standaard selected
        if (tabBackgrounds != null && tabBackgrounds.Length > 0)
            SelectTab(tabBackgrounds[0]);
    }

    public void SelectTab(Image tabBg)
    {
        if (tabBg == null) return;

        // Reset alles naar normal
        foreach (var img in tabBackgrounds)
        {
            if (img != null) img.color = normalColor;
        }

        // Selected tab kleur
        tabBg.color = selectedColor;
        _currentSelected = tabBg;
    }
}