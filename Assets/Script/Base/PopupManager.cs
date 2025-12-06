using DG.Tweening;
using System.Collections.Generic;
using UnityBase.DesignPattern;
using UnityEngine;
using UnityEngine.Events;

public class PopupManager : SingletonMonoBehavier<PopupManager>
{
    [SerializeField] private Transform popupHolder;
    public Transform PopupHolder => popupHolder;
    private readonly Dictionary<string, BasePopUp> _availablePopups = new();
    [HideInInspector] public SettingsDialog settingsDialog;
    void Start()
    {
    }
    public void ShowSettingPopup(bool isInGame)
    {
        PopupManager.Instance.OpenSettingPopup();
    }
    public void CloseAllPopup()
    {
        foreach (var item in _availablePopups.Values) item.Close();
    }
    public void OpenSettingPopup(UnityAction callback = null)
    {
        if (settingsDialog != null)
            settingsDialog.Open(callback);
        else
        {
            var obj = Resources.Load<SettingsDialog>("UI/" + PopUpName.SettingPopup);
            var gameObj = Instantiate(obj, popupHolder);
            var script = gameObj.GetComponent<SettingsDialog>();
            settingsDialog = script;
            script.Open(callback);
            _availablePopups.Add(PopUpName.SettingPopup, script);
        }
    }
}

public class PopUpName
{
    public static string SettingPopup = "SettingPopup";
}