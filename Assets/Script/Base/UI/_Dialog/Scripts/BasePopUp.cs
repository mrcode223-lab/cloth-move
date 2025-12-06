using DG.Tweening;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BasePopUp : MonoBehaviour
{
    public enum ANIM_IN
    {
        NONE,
        FROM_TOP,
        FROM_LEFT,
        FROM_RIGHT,
        FROM_BOT,
        FADE_ALPHA,
        FROM_SCALE
    }

    public enum ANIM_OUT
    {
        NONE,
        TO_TOP,
        TO_LEFT,
        TO_RIGHT,
        TO_BOT,
        FADE_ALPHA,
        TO_SCALE
    }

    public GameObject popup;
    public float darkerAlpha = 0.8f;
    public Image darker;
    public ANIM_IN animIn = ANIM_IN.FROM_SCALE;
    public ANIM_OUT animOut = ANIM_OUT.TO_SCALE;
    public UnityAction OnAppearDone;
    public UnityAction actionCloseBase;
    public bool needClickDarkerClose = true;
    protected CanvasGroup canvasGroup;
    Vector3 firstScale = Vector3.one;
    protected bool isOpen = false;
    protected IDisposable delayClose;
    protected virtual void Awake()
    {
            
    }
    protected virtual void Start()
    {
        
    }
    private void Update()
    {
        if(isOpen)
        {
            float aspectRatio = (float)Screen.width / Screen.height;
            if (Helper.DeviceIsIpad())
            {
                popup.transform.localScale = aspectRatio > 1 ? (Vector3.one / aspectRatio) : (Vector3.one * 0.7f);
                firstScale = aspectRatio > 1 ? (Vector3.one / aspectRatio) : (Vector3.one * 0.7f);
            }
            else if (Helper.DeviceIsTablet())
            {
                popup.transform.localScale = aspectRatio > 1 ? (Vector3.one / aspectRatio) : (Vector3.one * 0.7f);
                firstScale = aspectRatio > 1 ? (Vector3.one / aspectRatio) : (Vector3.one * 0.7f);
            }
            else
            {
                popup.transform.localScale = Vector3.one;
                firstScale = Vector3.one;
            }
        }    
        
    }
    public virtual void Open(UnityAction action = null)
    {
        float aspectRatio = (float)Screen.width / Screen.height;
        if (Helper.DeviceIsIpad())
        {
            popup.transform.localScale = aspectRatio > 1 ? (Vector3.one / aspectRatio) : (Vector3.one * 0.7f);
            firstScale = aspectRatio > 1 ? (Vector3.one / aspectRatio) : (Vector3.one * 0.7f);
        }
        else if (Helper.DeviceIsTablet())
        {
            popup.transform.localScale = aspectRatio > 1 ? (Vector3.one / aspectRatio) : (Vector3.one * 0.7f);
            firstScale = aspectRatio > 1 ? (Vector3.one / aspectRatio) : (Vector3.one * 0.7f);
        }
        else
        {
            popup.transform.localScale = Vector3.one;
            firstScale = Vector3.one;
        }
        this.transform.SetSiblingIndex(99);
        darker.color = new Color(darker.color.r, darker.color.g, darker.color.b, 0);
        darker.gameObject.SetActive(true);
        darker.DOFade(darkerAlpha, 0.3f).SetEase(Ease.Linear);

        OnStartOpen();

        switch (animIn)
        {
            case ANIM_IN.FROM_TOP:
                popup.transform.localPosition = Vector3.up * Screen.height;
                popup.SetActive(true);
                popup.transform.DOLocalMoveY(0f, 2f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    delayClose = Observable.Timer(TimeSpan.FromSeconds(0.25f)).Subscribe(_ =>
                    {
                        if (needClickDarkerClose)
                        {
                            var button = darker.GetComponent<Button>();
                            if (button == null)
                            {
                                darker.gameObject.AddComponent<Button>();
                                button = darker.GetComponent<Button>();
                                ColorBlock cb = button.colors;
                                cb.pressedColor = button.colors.normalColor;
                                cb.disabledColor = button.colors.normalColor;
                                button.colors = cb;
                                button.onClick.AddListener(Close);
                            }
                        }
                    }).AddTo(this);
                    isOpen = true;
                    action?.Invoke();
                    OnAppearDone?.Invoke();
                    OnOpen();
                }).SetUpdate(true);
                break;
            case ANIM_IN.FROM_BOT:
                isOpen = true;
                break;
            case ANIM_IN.FROM_LEFT:
                isOpen = true;
                break;
            case ANIM_IN.FROM_RIGHT:
                isOpen = true;
                break;
            case ANIM_IN.FADE_ALPHA:
                canvasGroup = popup.GetComponent<CanvasGroup>();
                if (canvasGroup == null)
                {
                    canvasGroup = popup.AddComponent<CanvasGroup>();
                }

                canvasGroup.alpha = 0f;
                popup.SetActive(true);
                canvasGroup.DOFade(1f, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    delayClose = Observable.Timer(TimeSpan.FromSeconds(0.25f)).Subscribe(_ =>
                    {
                        if (needClickDarkerClose)
                        {
                            var button = darker.GetComponent<Button>();
                            if (button == null)
                            {
                                darker.gameObject.AddComponent<Button>();
                                button = darker.GetComponent<Button>();
                                ColorBlock cb = button.colors;
                                cb.pressedColor = button.colors.normalColor;
                                cb.disabledColor = button.colors.normalColor;
                                button.colors = cb;
                                button.onClick.AddListener(Close);
                            }
                        }
                    }).AddTo(this);

                    isOpen = true;
                    action?.Invoke();
                    OnAppearDone?.Invoke();
                    OnOpen();
                }).SetUpdate(true);
                break;
            case ANIM_IN.FROM_SCALE:
                popup.transform.localScale = Vector3.zero;
                popup.SetActive(true);
                popup.transform.DOScale(firstScale, 0.2f).SetEase(Ease.InExpo).OnComplete(() =>
                {
                    delayClose = Observable.Timer(TimeSpan.FromSeconds(0.25f)).Subscribe(_ =>
                    {
                        if (needClickDarkerClose)
                        {
                            var button = darker.GetComponent<Button>();
                            if (button == null)
                            {
                                darker.gameObject.AddComponent<Button>();
                                button = darker.GetComponent<Button>();
                                ColorBlock cb = button.colors;
                                cb.pressedColor = button.colors.normalColor;
                                cb.disabledColor = button.colors.normalColor;
                                button.colors = cb;
                                button.onClick.AddListener(Close);
                            }
                        }
                    }).AddTo(this);
                    isOpen = true;
                    action?.Invoke();
                    OnAppearDone?.Invoke();
                    OnOpen();
                }).SetUpdate(true);
                break;
            default:
                isOpen = true;
                popup.SetActive(true);
                break;
        }
    }

    public virtual void OnOpen()
    {
    }

    public virtual void OnStartOpen()
    {
    }
    public virtual void Close()
    {
        isOpen = false;
        delayClose?.Dispose();
        darker.DOFade(0, 0.3f).SetEase(Ease.Linear);
        if(needClickDarkerClose)
        {
           Destroy(darker.GetComponent<Button>());
        }    
        switch (animOut)
        {
            case ANIM_OUT.TO_TOP:
                popup.transform.DOLocalMoveY(Screen.height / 4f, 0.3f).SetEase(Ease.OutBack).OnComplete(() =>
                {
                    darker.gameObject.SetActive(false);
                    popup.SetActive(false);
                    actionCloseBase?.Invoke();
                    actionCloseBase = null;
                });
                break;
            case ANIM_OUT.TO_BOT:
                break;
            case ANIM_OUT.TO_LEFT:
                break;
            case ANIM_OUT.TO_RIGHT:
                break;
            case ANIM_OUT.FADE_ALPHA:
                break;
            case ANIM_OUT.TO_SCALE:
                popup.transform.DOScale(0, 0.2f).SetEase(Ease.OutExpo).OnComplete(() =>
                {
                    darker.gameObject.SetActive(false);
                    popup.SetActive(false);
                    actionCloseBase?.Invoke();
                    actionCloseBase = null;
                });
                break;
            default:
                darker.gameObject.SetActive(false);
                popup.SetActive(false);
                actionCloseBase?.Invoke();
                actionCloseBase = null;
                break;
        }
    }
}