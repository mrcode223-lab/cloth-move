using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Mybutton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IEndDragHandler
{
    public UnityEvent onDownEvent;
    public UnityEvent onUpEvent;
    public UnityEvent onDownEventStart;
    public UnityEvent oneDownEvent;
    public Image btnImage;
    [SerializeField] Color normalColor;
    [SerializeField] Color disableColor;
    public float pressedTime = 0.5f;
    public float frameEvent = 0.05f;
    public bool interactable = true;
    public bool isPressed = false;
    public bool useEndDrag = false;
    float timePressed, timeInvokeEvent;
    IDisposable pressedDisposable;
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!interactable) return;
        onDownEvent?.Invoke();
        oneDownEvent?.Invoke();
        isPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        onUpEvent?.Invoke();
        isPressed = false;
        pressedDisposable?.Dispose();
        pressedDisposable = null;
        timePressed = 0;
    }
    private void Update()
    {
        if(!interactable)
        {
            isPressed = false;
            pressedDisposable?.Dispose();
            pressedDisposable = null;
            timePressed = 0;
        }   
        else
        {
            if (isPressed)
            {
                timePressed += Time.deltaTime;
                if (timePressed >= pressedTime && pressedDisposable == null)
                {
                    onDownEventStart?.Invoke();
                    pressedDisposable = Observable.Interval(TimeSpan.FromSeconds(frameEvent)).Subscribe(_ =>
                    {
                        //Debug.LogError("pressed");
                        if (isPressed && interactable)
                        {
                            onDownEvent?.Invoke();
                        }
                    }).AddTo(this);
                }
            }
            else
            {
                pressedDisposable?.Dispose();
            }    
        }

        if (btnImage != null) btnImage.color = interactable ? normalColor : disableColor;
    }
    public void StopPressed()
    {
        isPressed = false;
        pressedDisposable?.Dispose();
        pressedDisposable = null;
        timePressed = 0;
    }
    private void OnDisable()
    {
        StopPressed();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(useEndDrag)
        {
            onUpEvent?.Invoke();
            isPressed = false;
            pressedDisposable?.Dispose();
            pressedDisposable = null;
            timePressed = 0;
        }    
    }
}
