using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI; 

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Canvas canvas; 
    public Image trashCan; 

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector3 originalPosition;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        originalPosition = transform.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPosition = transform.position;
        canvasGroup.alpha = 0.6f; 
        canvasGroup.blocksRaycasts = false; 
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 newPos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(canvas.transform as RectTransform, eventData.position, eventData.pressEventCamera, out newPos))
        {
            rectTransform.position = newPos;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f; 
        canvasGroup.blocksRaycasts = true; 

        RectTransform trashRect = trashCan.GetComponent<RectTransform>();
        if (RectTransformUtility.RectangleContainsScreenPoint(trashRect, eventData.position, eventData.pressEventCamera))
        {
            Destroy(gameObject); 
        }
        else
        {
            rectTransform.position = originalPosition;
        }
    }
}



