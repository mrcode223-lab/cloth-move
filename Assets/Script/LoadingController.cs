using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class LoadingController : MonoBehaviour
{
    [SerializeField] private Image progressBar;
    [SerializeField] private float loadingTime = 3.0f;

    void Start()
    {
        Action actionDone = () =>
        {
            FadeLoadScene.Fade("Home", Color.black, 2f, null);
        };

        var dot = DOVirtual.Float(0, 1, loadingTime, t => { progressBar.fillAmount = t; })
            .OnComplete(() =>
            {
                progressBar.fillAmount = 1;
                actionDone.Invoke();
            });
    }
}