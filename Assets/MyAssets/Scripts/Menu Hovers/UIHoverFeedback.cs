using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIHoverFeedback : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Targets")]
    [SerializeField] private TextMeshProUGUI targetText;
    [SerializeField] private Image targetImage;
    [SerializeField] private Shadow targetShadow;
    [SerializeField] private Outline targetOutline;

    [Header("Text")]
    [SerializeField] private bool affectTextColor = true;
    [SerializeField] private Color normalTextColor = new(232 / 255f, 240 / 255f, 240 / 255f, 1f);
    [SerializeField] private Color hoverTextColor = new(70 / 255f, 224 / 255f, 208 / 255f, 1f);

    [Header("Image")]
    [SerializeField] private bool affectImageColor = false;
    [SerializeField] private Color normalImageColor = new(5 / 255f, 18 / 255f, 18 / 255f, 1f);
    [SerializeField] private Color hoverImageColor = new(70 / 255f, 224 / 255f, 208 / 255f, 1f);

    [Header("Shadow")]
    [SerializeField] private bool useShadowGlow = false;
    [SerializeField] private Color normalShadowColor = new(0, 0, 0, 0);
    [SerializeField] private Color hoverShadowColor = new(0.2f, 1f, 0.9f, 0.55f);
    [SerializeField] private Vector2 normalShadowDistance = Vector2.zero;
    [SerializeField] private Vector2 hoverShadowDistance = new(2f, -2f);

    [Header("Outline")]
    [SerializeField] private bool useOutlineGlow = false;
    [SerializeField] private Color normalOutlineColor = new(0, 0, 0, 0);
    [SerializeField] private Color hoverOutlineColor = new(0.2f, 1f, 0.9f, 0.35f);

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip hoverClip;
    [SerializeField] private AudioClip clickClip;
    [SerializeField, Range(0f, 1f)] private float hoverVolume = 1f;
    [SerializeField, Range(0f, 1f)] private float clickVolume = 1f;

    [Header("Tab Selection")]
    [SerializeField] private bool playOnlyIfNotSelected = false;
    [SerializeField] private bool isSelected = false;

    private void Awake()
    {
        if (targetText == null) targetText = GetComponentInChildren<TextMeshProUGUI>();
        if (targetImage == null) targetImage = GetComponent<Image>();
        if (targetShadow == null) targetShadow = GetComponentInChildren<Shadow>();
        if (targetOutline == null) targetOutline = GetComponentInChildren<Outline>();

        Apply(false);
    }

    public void SetSelected(bool selected)
    {
        isSelected = selected;
        Apply(selected);
    }

    public void OnPointerEnter(PointerEventData e)
    {
        Debug.Log("Hover on: " + gameObject.name);

        if (!(playOnlyIfNotSelected && isSelected))
            Play(hoverClip, hoverVolume);

        if (!isSelected)
            Apply(true);
    }

    public void OnPointerExit(PointerEventData e)
    {
        if (!isSelected)
            Apply(false);
    }

    public void OnPointerClick(PointerEventData e)
    {
        Debug.Log("Click on: " + gameObject.name);
        Play(clickClip, clickVolume);
    }

    private void Play(AudioClip clip, float volume)
    {
        if (audioSource == null)
        {
            Debug.LogWarning("UIHoverFeedback: geen AudioSource ingevuld op " + gameObject.name);
            return;
        }

        if (clip == null)
        {
            Debug.LogWarning("UIHoverFeedback: geen AudioClip ingevuld op " + gameObject.name);
            return;
        }

        audioSource.PlayOneShot(clip, volume);
    }

    private void Apply(bool hover)
    {
        if (affectTextColor && targetText != null)
            targetText.color = hover ? hoverTextColor : normalTextColor;

        if (affectImageColor && targetImage != null)
            targetImage.color = hover ? hoverImageColor : normalImageColor;

        if (useShadowGlow && targetShadow != null)
        {
            targetShadow.effectColor = hover ? hoverShadowColor : normalShadowColor;
            targetShadow.effectDistance = hover ? hoverShadowDistance : normalShadowDistance;
        }

        if (useOutlineGlow && targetOutline != null)
            targetOutline.effectColor = hover ? hoverOutlineColor : normalOutlineColor;
    }
}