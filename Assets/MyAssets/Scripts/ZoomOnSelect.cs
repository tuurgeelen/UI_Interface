using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class ZoomOnSelect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Target")]
    [SerializeField] private RectTransform target;

    [Header("Animation")]
    [SerializeField] private float hoverScale = 1.1f;
    [SerializeField] private float clickScale = 1.15f;
    [SerializeField] private float moveOffset = 20f;
    [SerializeField] private float wiggleAngle = 5f;

    private Vector3 startScale;
    private Vector2 startAnchoredPos;

    private void Awake()
    {
        if (target == null)
            target = transform as RectTransform;

        startScale = target.localScale;
        startAnchoredPos = target.anchoredPosition;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        target.DOKill();

        target.DOScale(startScale * hoverScale, 0.15f)
            .SetUpdate(true);

        target.DOAnchorPosY(startAnchoredPos.y + moveOffset, 0.15f)
            .SetUpdate(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        target.DOKill();

        target.DOScale(startScale, 0.15f)
            .SetUpdate(true);

        target.DOAnchorPosY(startAnchoredPos.y, 0.15f)
            .SetUpdate(true);

        target.DORotate(Vector3.zero, 0.15f, RotateMode.Fast)
            .SetUpdate(true);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        target.DOKill();

        Sequence seq = DOTween.Sequence().SetUpdate(true);

        seq.Append(target.DOScale(startScale * clickScale, 0.08f));
        seq.Append(target.DORotate(new Vector3(0, 0, -wiggleAngle), 0.08f, RotateMode.Fast));
        seq.Append(target.DORotate(new Vector3(0, 0, wiggleAngle), 0.08f, RotateMode.Fast));
        seq.Append(target.DORotate(Vector3.zero, 0.12f, RotateMode.Fast));
        seq.Join(target.DOScale(startScale * hoverScale, 0.12f));
    }
}