using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UniRx;
using UnityBase.Base.Controller;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : ProcessController
{
    public delegate void Full();
    public Full fullEvent;
    public Transform ui;
    public Image staminaBar;
    public Image realStamina;
    public TextMeshProUGUI staminaStatus;
    Transform target;
    public bool startWithFull = false;
    public bool needFollowCamera = true;
    public bool useDOTween = false;
    float delayFull = 0;
    private Queue<float> expQueue = new Queue<float>();
    private bool isProcessingExp = false;
    public bool IsFull => currentsValue >= max;
    public float max => (float)maxValue;
    public float Stamina => currentsValue;
    float speedFill;
    private void Awake()
    {
        if (startWithFull) currentsValue = maxValue;
    }
    protected virtual void Start()
    {
        //healthBar = ui.GetChild(0).GetComponent<Image>();
        //realHP = healthBar.transform.GetChild(0).GetComponent<Image>();
        //healthBar.enabled = false;
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    public void SetDelayFull(float time)
    {
        delayFull = time;
    }
    public void SetTarget(Transform tfTarget)
    {
        this.target = tfTarget;
    }
    public void ChangeMaxHp(float newHp)
    {
        maxValue = newHp;
        EditValue(currentsValue);
    }
    public void PlusMax(int value)
    {
        maxValue += value;
        if (currentsValue > maxValue)
        {
            EditValue(maxValue);
        }
        else
        {
            EditValue(currentsValue += value);
        }
    }
    public void SetMaxStamina(float newHp)
    {
        maxValue = newHp;
        currentsValue = newHp;
        EditValue(currentsValue);
    }
    public void SetCurrentStamina(float changeValue)
    {
        EditValue(changeValue);
    }
    public virtual void ChangeStamina(float changeHp, bool needPlusRedundency = true)
    {
        expQueue.Enqueue(changeHp);
        if (!isProcessingExp)
        {
            if (useDOTween)
            {
                ProcessExpQueueDOTween(needPlusRedundency);
            }
            else
            {
                ProcessExpQueue(needPlusRedundency);
            }

        }
    }
    private void ProcessExpQueue(bool needPlusRedundency)
    {
        isProcessingExp = true;
        while (expQueue.Count > 0)
        {
            float changeHp = expQueue.Dequeue();
            float stamina = currentsValue + changeHp;
            currentsValue = Mathf.Clamp(stamina, 0f, max);
            if (stamina >= max)
            {
                float redundency = stamina - max;

                if (fullEvent != null)
                {
                    if (needPlusRedundency) ChangeStamina(redundency);
                    fullEvent();
                }
            }
        }
        isProcessingExp = false;
    }
    Tween _tweenFill;
    private void ProcessExpQueueDOTween(bool needPlusRedundency)
    {
        isProcessingExp = true;

        while (expQueue.Count > 0)
        {
            float changeHp = expQueue.Dequeue();
            float stamina = currentsValue + changeHp;
            _tweenFill?.Kill();
            _tweenFill = DOTween.To(() => currentsValue, x => currentsValue = x, stamina, 0f)
                .OnUpdate(() =>
                {
                    staminaBar.fillAmount = currentsValue / max;
                })
                .OnComplete(() =>
                {
                    currentsValue = Mathf.Clamp(stamina, 0f, max);
                    if (stamina >= max)
                    {
                        float redundency = stamina - max;

                        if (fullEvent != null)
                        {
                            if (needPlusRedundency) ChangeStamina(redundency);
                            fullEvent();
                        }
                    }
                    isProcessingExp = false;
                }).SetEase(Ease.Linear);
        }
    }
    //private IEnumerator ActionFull(System.Action onComplete)
    //{
    //    yield return new WaitForSeconds(0);
    //    onComplete?.Invoke();
    //}
    protected override void OnUpdate(float value)
    {
        if (realStamina != null) realStamina.fillAmount = (currentsValue) / maxValue;
    }
    private void OnDestroy()
    {
        _tweenFill?.Kill();
    }
    //private void Update()
    //{

    //}
    private void LateUpdate()
    {
        if (!useDOTween && staminaBar != null) staminaBar.fillAmount = Mathf.Lerp(staminaBar.fillAmount, (currentsValue) / maxValue, Time.deltaTime * 10);
        if (staminaStatus) staminaStatus.text = $"{Helper.CurrencyString(currentsValue)} / {Helper.CurrencyString(maxValue)}";
        if (needFollowCamera && Camera.main != null) transform.forward = Camera.main.transform.forward;
        if (target)
        {
            transform.position = target.position;
        }
    }
}