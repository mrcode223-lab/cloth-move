using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityBase.Base.Controller;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : ProcessController
{
    public delegate void Die();
    public Die dieEvent;
    public delegate void Full();
    public Full fullEvent;
    public Transform ui;
    public Image healthBar;
    public Image realHP;
    public TextMeshProUGUI hpStatus, nameTxt;
    Transform target;
    public bool startWithFull = false;
    public bool needFollowCamera = true;
    public bool IsFull => currentsValue >= max;
    public float max => (float)maxValue;
    public float HP => currentsValue;

    private void Awake()
    {
        if(startWithFull) currentsValue = maxValue;
    }
    protected virtual void Start()
    {
        //healthBar = ui.GetChild(0).GetComponent<Image>();
        //realHP = healthBar.transform.GetChild(0).GetComponent<Image>();
        //healthBar.enabled = false;
    }
    public void SetTarget(Transform target)
    {
        this.target = target;
    }
    public void PlusMax(int value)
    {
        maxValue += value;
        if(currentsValue > maxValue)
        {
            EditValue(maxValue);
        }
        else
        {
            EditValue(currentsValue += value);
        }
    }
    public void SetMaxHp(float newHp)
    {
        maxValue = newHp;
        currentsValue = newHp;
        EditValue(currentsValue);
    }
    public void ChangeMaxHp(float newHp)
    {
        maxValue = newHp;
        EditValue(currentsValue);
    }
    public void SetCurrentHp(float changeValue)
    {
        EditValue(changeValue);
    }
    public void ChangeHp(float changeHp)
    {
       
        currentsValue -= changeHp;
        if (currentsValue < 1f)
        {
            currentsValue = 0;
            dieEvent?.Invoke();
        }
        EditValue(currentsValue, () =>
        {
            
        });
    }

    protected override void OnUpdate(float value)
    {
        realHP.fillAmount = (currentsValue) / maxValue;
        if (value <= 1f) realHP.fillAmount = 0;
    }

    private void Update()
    {
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, (currentsValue) / maxValue, Time.deltaTime * 10f);
        if(hpStatus) hpStatus.text = $"{Helper.CurrencyString(currentsValue)}";
    }
    private void LateUpdate()
    {
        if(needFollowCamera && Camera.main != null) transform.forward = Camera.main.transform.forward;
        if (target)
        {
            transform.position = target.position;
        }
    }
}
