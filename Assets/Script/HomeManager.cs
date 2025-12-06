using TMPro;
using UnityEngine;

public class HomeManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI coinTxt;
    public void LoadToGame()
    {
        FadeLoadScene.Fade("Game", Color.black, 2f, null);
    }
    private void LateUpdate()
    {
        coinTxt.text = DataManager.Coin.ToString();
    }
    public void ShowSetting()
    {
        PopupManager.Instance.ShowSettingPopup(false);
    }    
}
