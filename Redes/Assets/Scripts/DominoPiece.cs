using UnityEngine;

using UnityEngine;
using UnityEngine.EventSystems;

public class DominoPiece : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int leftValue;
    public int rightValue;

    private Vector3 startPosition;

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Camera.main.ScreenToWorldPoint(eventData.position) + Vector3.forward * 10;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Aqui você verifica se a peça foi colocada em posição válida
        // Se não, volta pra posição inicial
        if (!GameManager.Instance.TryPlacePiece(this, transform.position))
        {
            transform.position = startPosition;
        }
    }
}
