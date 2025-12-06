using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class FixedTouchField : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public UnityEvent onDownEvent;
    public UnityEvent onUpEvent;
    public Vector2 TouchDist { get; private set; }
    private Vector2 pointerOld;
    private bool pressed;
    public bool Pressed=>pressed;
    [Header("Tốc độ xoay")]
    public float sensitivity = 0.01f; // càng lớn thì xoay càng nhanh

    void Update()
    {
        if (!pressed)
        {
            TouchDist = Vector2.zero;
        }
    }
    public void ResetTouch()
    {
        pressed = false;
        TouchDist = Vector2.zero;
        onUpEvent?.Invoke();
    }    
    public void OnPointerDown(PointerEventData eventData)
    {
        if (pressed) return; // chỉ nhận duy nhất 1 touch trên UI này
        pressed = true;
        pointerOld = eventData.position;
        TouchDist = Vector2.zero;
        onDownEvent?.Invoke();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!pressed) return;

        Vector2 pointerNew = eventData.position;
        Vector2 delta = pointerNew - pointerOld;

        if (delta == Vector2.zero)
        {
            // Ngón tay không hề di chuyển
            TouchDist = Vector2.zero;
        }
        else
        {
            TouchDist = delta * sensitivity;
            pointerOld = pointerNew;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pressed = false;
        TouchDist = Vector2.zero;
        onUpEvent?.Invoke();
    }
}
