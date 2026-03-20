using UnityEngine;
using UnityEngine.EventSystems;

public class HoverArrows : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private RectTransform leftArrow;
    [SerializeField] private RectTransform rightArrow;

    [SerializeField] private float speed = 5f;
    [SerializeField] private float distance = 10f;

    private Vector2 leftStartPos;
    private Vector2 rightStartPos;

    private bool hovering = false;

    private void Start()
    {
        if (leftArrow != null)
            leftStartPos = leftArrow.anchoredPosition;

        if (rightArrow != null)
            rightStartPos = rightArrow.anchoredPosition;

        SetArrows(false);
    }

    private void Update()
    {
        if (!hovering)
            return;

        float move = Mathf.Sin(Time.unscaledTime * speed) * distance;

        if (leftArrow != null)
            leftArrow.anchoredPosition = leftStartPos + new Vector2(-move, 0);

        if (rightArrow != null)
            rightArrow.anchoredPosition = rightStartPos + new Vector2(move, 0);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hovering = true;
        SetArrows(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hovering = false;
        SetArrows(false);

        if (leftArrow != null)
            leftArrow.anchoredPosition = leftStartPos;

        if (rightArrow != null)
            rightArrow.anchoredPosition = rightStartPos;
    }

    private void SetArrows(bool state)
    {
        if (leftArrow != null) leftArrow.gameObject.SetActive(state);
        if (rightArrow != null) rightArrow.gameObject.SetActive(state);
    }
}