using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIHoverFeedback : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Targets")]
    [SerializeField] TextMeshProUGUI targetText;
    [SerializeField] Image targetImage;
    [SerializeField] Shadow targetShadow;
    [SerializeField] Outline targetOutline;

    [Header("Text")]
    [SerializeField] bool affectTextColor = true;
    [SerializeField] Color normalTextColor = new(232/255f, 240/255f, 240/255f, 1f);
    [SerializeField] Color hoverTextColor  = new(70/255f, 224/255f, 208/255f, 1f);

    [Header("Image")]
    [SerializeField] bool affectImageColor = false;
    [SerializeField] Color normalImageColor = new(5/255f, 18/255f, 18/255f, 1f);
    [SerializeField] Color hoverImageColor  = new(70/255f, 224/255f, 208/255f, 1f);

    [Header("Shadow")]
    [SerializeField] bool useShadowGlow = false;
    [SerializeField] Color normalShadowColor = new(0,0,0,0);
    [SerializeField] Color hoverShadowColor  = new(0.2f, 1f, 0.9f, 0.55f);
    [SerializeField] Vector2 normalShadowDistance = Vector2.zero;
    [SerializeField] Vector2 hoverShadowDistance  = new(2f, -2f);

    [Header("Outline")]
    [SerializeField] bool useOutlineGlow = false;
    [SerializeField] Color normalOutlineColor = new(0,0,0,0);
    [SerializeField] Color hoverOutlineColor  = new(0.2f, 1f, 0.9f, 0.35f);

    [Header("Audio")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip hoverClip, clickClip;
    [SerializeField, Range(0f, 1f)] float hoverVolume = 1f, clickVolume = 1f;

    [Header("Tab Selection")]
    [SerializeField] bool playOnlyIfNotSelected = false;
    [SerializeField] bool isSelected = false;

    void Reset()
    {
        targetText ??= GetComponentInChildren<TextMeshProUGUI>();
        targetImage ??= GetComponent<Image>();
        targetShadow ??= GetComponentInChildren<Shadow>();
        targetOutline ??= GetComponentInChildren<Outline>();
        audioSource ??= FindFirstObjectByType<AudioSource>();
    }

    void Awake() => Apply(false);

    public void SetSelected(bool selected) { isSelected = selected; Apply(selected); }

    public void OnPointerEnter(PointerEventData e)
    {
        if (!(playOnlyIfNotSelected && isSelected)) Play(hoverClip, hoverVolume);
        if (!isSelected) Apply(true);
    }

    public void OnPointerExit(PointerEventData e) { if (!isSelected) Apply(false); }

    public void OnPointerClick(PointerEventData e) => Play(clickClip, clickVolume);

    void Play(AudioClip clip, float vol) { if (audioSource && clip) audioSource.PlayOneShot(clip, vol); }

    void Apply(bool hover)
    {
        if (affectTextColor && targetText) targetText.color = hover ? hoverTextColor : normalTextColor;
        if (affectImageColor && targetImage) targetImage.color = hover ? hoverImageColor : normalImageColor;

        if (useShadowGlow && targetShadow)
        {
            targetShadow.effectColor = hover ? hoverShadowColor : normalShadowColor;
            targetShadow.effectDistance = hover ? hoverShadowDistance : normalShadowDistance;
        }

        if (useOutlineGlow && targetOutline)
            targetOutline.effectColor = hover ? hoverOutlineColor : normalOutlineColor;
    }
}