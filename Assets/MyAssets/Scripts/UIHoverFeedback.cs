using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIHoverFeedback : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Optional Visual Targets")]
    [SerializeField] private TextMeshProUGUI targetText;
    [SerializeField] private Image targetImage;               // bv tab achtergrond
    [SerializeField] private Shadow targetShadow;             // standaard UI Shadow component
    [SerializeField] private Outline targetOutline;           // standaard UI Outline component

    [Header("Text Colors")]
    [SerializeField] private bool affectTextColor = true;
    [SerializeField] private Color normalTextColor = new Color32(232, 240, 240, 255);
    [SerializeField] private Color hoverTextColor = new Color32(70, 224, 208, 255);

    [Header("Image Colors")]
    [SerializeField] private bool affectImageColor = false;
    [SerializeField] private Color normalImageColor = new Color32(5, 18, 18, 255);
    [SerializeField] private Color hoverImageColor = new Color32(70, 224, 208, 255);

    [Header("Glow / Shadow")]
    [SerializeField] private bool useShadowGlow = false;
    [SerializeField] private Color normalShadowColor = new Color(0f, 0f, 0f, 0f);
    [SerializeField] private Color hoverShadowColor = new Color(0.2f, 1f, 0.9f, 0.55f);
    [SerializeField] private Vector2 normalShadowDistance = new Vector2(0f, 0f);
    [SerializeField] private Vector2 hoverShadowDistance = new Vector2(2f, -2f);

    [Header("Outline (optional)")]
    [SerializeField] private bool useOutlineGlow = false;
    [SerializeField] private Color normalOutlineColor = new Color(0f, 0f, 0f, 0f);
    [SerializeField] private Color hoverOutlineColor = new Color(0.2f, 1f, 0.9f, 0.35f);

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;     // kan gedeeld worden
    [SerializeField] private AudioClip hoverClip;
    [SerializeField, Range(0f, 1f)] private float hoverVolume = 1f;
    [SerializeField] private bool playOnlyIfNotSelected = false;

    [Header("Selection (for tabs)")]
    [SerializeField] private bool isSelected = false;

    private void Reset()
    {
        if (targetText == null) targetText = GetComponentInChildren<TextMeshProUGUI>();
        if (targetImage == null) targetImage = GetComponent<Image>();
        if (targetShadow == null) targetShadow = GetComponentInChildren<Shadow>();
        if (targetOutline == null) targetOutline = GetComponentInChildren<Outline>();
        if (audioSource == null) audioSource = FindFirstObjectByType<AudioSource>();
    }

    private void Awake()
    {
        ApplyNormal();
    }

    public void SetSelected(bool selected)
    {
        isSelected = selected;
        if (isSelected) ApplyHoverVisual();
        else ApplyNormal();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!(playOnlyIfNotSelected && isSelected))
        {
            PlayHoverSound();
        }

        if (!isSelected)
            ApplyHoverVisual();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isSelected)
            ApplyNormal();
    }

    private void PlayHoverSound()
    {
        if (audioSource == null || hoverClip == null) return;
        audioSource.PlayOneShot(hoverClip, hoverVolume);
    }

    private void ApplyNormal()
    {
        if (affectTextColor && targetText != null)
            targetText.color = normalTextColor;

        if (affectImageColor && targetImage != null)
            targetImage.color = normalImageColor;

        if (useShadowGlow && targetShadow != null)
        {
            targetShadow.effectColor = normalShadowColor;
            targetShadow.effectDistance = normalShadowDistance;
        }

        if (useOutlineGlow && targetOutline != null)
        {
            targetOutline.effectColor = normalOutlineColor;
        }
    }

    private void ApplyHoverVisual()
    {
        if (affectTextColor && targetText != null)
            targetText.color = hoverTextColor;

        if (affectImageColor && targetImage != null)
            targetImage.color = hoverImageColor;

        if (useShadowGlow && targetShadow != null)
        {
            targetShadow.effectColor = hoverShadowColor;
            targetShadow.effectDistance = hoverShadowDistance;
        }

        if (useOutlineGlow && targetOutline != null)
        {
            targetOutline.effectColor = hoverOutlineColor;
        }
    }
}