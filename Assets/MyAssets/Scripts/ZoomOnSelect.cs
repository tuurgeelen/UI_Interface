using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class ZoomOnSelect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    Vector3 startScale;
    Vector3 startPosition;
    [SerializeField] float scale = 1.1f;
    [SerializeField] float offset = 50f;
    [SerializeField] float wiggleAngle = 5f;

    void Start()
    {
        startScale = transform.localScale;
        startPosition = transform.position;
    }

    void SelectionTween()
    {

        /*
            transform.DORotate(new Vector3(0, 0, 25f), 0.2f, RotateMode.Fast).SetEase(Ease.InOutBounce).OnComplete
            (() => transform.DORotate(new Vector3(0, 0, 0f), 0.2f, RotateMode.Fast).SetEase(Ease.InOutBounce));*/
        #region uitleg
        //() => is een lambda expressie, hiermee kunnen we een naamloze 
        //functie aanmaken die direct wordt uitgevoerd nadat de eerste animatie klaar is.
        //  In dit geval zorgt het ervoor dat de tweede rotatie begint nadat de eerste rotatie is voltooid.
        #endregion
        #region uitleg
        //DoTween Sequence. Met een sequence kunnen we
        //verschillende animaties stacken oftewel achter elkaar afspelen.
        //Normaalgezien speelt DowTween animations die regel per regel staan tegelijk af als 1 combined animatie.
        //Maar met een sequence hebben we controle over hoe dit gebeurt, en kunnen we dus ook wachttijden maken.
        #endregion
        print(transform.name);
        Sequence selectSequence = DOTween.Sequence();
        selectSequence.Append(transform.DOScale(startScale * scale, 0.1f));
        selectSequence.Append(transform.DORotate(new Vector3(0, 0, -wiggleAngle), 0.1f, RotateMode.Fast).SetEase(Ease.OutBounce));
        selectSequence.Append(transform.DORotate(new Vector3(0, 0, wiggleAngle), 0.1f, RotateMode.Fast).SetEase(Ease.OutBounce));
        selectSequence.Append(transform.DORotate(new Vector3(0, 0, 0f), 0.2f, RotateMode.Fast).SetEase(Ease.OutBounce));

        selectSequence.Kill();

    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        transform.DOMoveY(startPosition.y + offset, 0.15f);
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        // if (!pointerEventData.pointerEnter == gameObject)
        transform.DOScale(startScale, 0.15f);
        transform.DOMoveY(startPosition.y, 0.15f);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SelectionTween();
    }

}