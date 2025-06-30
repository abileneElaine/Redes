using UnityEngine;
using UnityEngine.EventSystems;

public class DominoPiece : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int leftValue;
    public int rightValue;

    private Vector3 startPosition;
    private Transform originalParent;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!GameManager.Instance.CanPlayerMove(this)) return;

        startPosition = transform.position;
        originalParent = transform.parent;
        transform.SetParent(null);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!GameManager.Instance.CanPlayerMove(this)) return;

        Vector3 pos = Camera.main.ScreenToWorldPoint(eventData.position);
        pos.z = 0;
        transform.position = pos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!GameManager.Instance.CanPlayerMove(this)) return;

        if (!GameManager.Instance.TryPlacePiece(this, transform.position))
        {
            transform.position = startPosition;
            transform.SetParent(originalParent);
        }
    }
}