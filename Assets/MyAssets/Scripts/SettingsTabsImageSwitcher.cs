using UnityEngine;
using UnityEngine.UI;

public class SettingsTabsImageSwitcher : MonoBehaviour
{
    [Header("Content")]
    [SerializeField] private Image settingsContentImage;   // de image in je ScrollView Content
    [SerializeField] private ScrollRect scrollRect;        // je DisplayScrollView (of algemene scrollview)

    [Header("Tab Sprites")]
    [SerializeField] private Sprite displaySprite;
    [SerializeField] private Sprite controlsSprite;
    [SerializeField] private Sprite audioSprite;
    [SerializeField] private Sprite voiceSprite;
    [SerializeField] private Sprite bindingsSprite;
    [SerializeField] private Sprite miscSprite;

    [Header("Optional Tab Visuals")]
    [SerializeField] private Image displayTabImage;
    [SerializeField] private Image controlsTabImage;
    [SerializeField] private Image audioTabImage;
    [SerializeField] private Image voiceTabImage;
    [SerializeField] private Image bindingsTabImage;
    [SerializeField] private Image miscTabImage;

    [SerializeField] private Color normalTabColor = new Color32(5, 18, 18, 255);
    [SerializeField] private Color selectedTabColor = new Color32(70, 224, 208, 255);

    private void Start()
    {
        OpenDisplayTab(); // start standaard op DISPLAY
    }

    public void OpenDisplayTab() => SetTab(displaySprite, displayTabImage);
    public void OpenControlsTab() => SetTab(controlsSprite, controlsTabImage);
    public void OpenAudioTab() => SetTab(audioSprite, audioTabImage);
    public void OpenVoiceTab() => SetTab(voiceSprite, voiceTabImage);
    public void OpenBindingsTab() => SetTab(bindingsSprite, bindingsTabImage);
    public void OpenMiscTab() => SetTab(miscSprite, miscTabImage);

    private void SetTab(Sprite sprite, Image selectedTab)
    {
        if (settingsContentImage == null || sprite == null) return;

        // Sprite wisselen
        settingsContentImage.sprite = sprite;

        // Zorg dat image niet clickable/drag-target is (optioneel, maar handig)
        settingsContentImage.raycastTarget = false;

        // Breedte laten meegaan met parent, hoogte behouden via aspect
        settingsContentImage.preserveAspect = true;

        // Scroll terug naar boven
        Canvas.ForceUpdateCanvases();
        if (scrollRect != null)
            scrollRect.verticalNormalizedPosition = 1f;

        // Tab colors updaten (optioneel)
        ResetTabColors();
        if (selectedTab != null)
            selectedTab.color = selectedTabColor;
    }

    private void ResetTabColors()
    {
        if (displayTabImage != null) displayTabImage.color = normalTabColor;
        if (controlsTabImage != null) controlsTabImage.color = normalTabColor;
        if (audioTabImage != null) audioTabImage.color = normalTabColor;
        if (voiceTabImage != null) voiceTabImage.color = normalTabColor;
        if (bindingsTabImage != null) bindingsTabImage.color = normalTabColor;
        if (miscTabImage != null) miscTabImage.color = normalTabColor;
    }
}