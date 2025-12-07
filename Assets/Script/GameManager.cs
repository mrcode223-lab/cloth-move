using DG.Tweening;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] GameObject losePopup, winPopup;
    [SerializeField] ParticleSystem winFx;
    [SerializeField] SimpleCloth cloth;
    [SerializeField] TextMeshProUGUI loseRewardTxt, winRewardTxt, coinTxt, levelTxt;
    bool endGame = false;
    private void Awake()
    {
        Instance = this;
        Application.targetFrameRate = 120;
    }
    private void Start()
    {
        int levelIndex = ((DataManager.CurrentLevel - 1) % 10) + 1;

        var level = Instantiate(Resources.Load<GameObject>($"Levels/Level {levelIndex}"));
        cloth.collisionColliders = level.GetComponentsInChildren<Collider>();
        levelTxt.text = $"Level-{DataManager.CurrentLevel}";
    }
    private void LateUpdate()
    {
        coinTxt.text = DataManager.Coin.ToString();
    }
    public void ShowLosePopup()
    {
        if (winPopup.activeInHierarchy && !endGame) return;
        endGame = true;
        var reward = Random.Range(5, 10);
        loseRewardTxt.text = "+" + reward.ToString();
        DataManager.Coin += reward;
        losePopup.gameObject.SetActive(true);
    }
    public void ShowWinPopup()
    {
        if (losePopup.activeInHierarchy) return;
        winFx.Play();
        var reward = Random.Range(20, 50);
        winRewardTxt.text = "+" + reward.ToString();
        DataManager.Coin += reward;
        DOVirtual.DelayedCall(2f, () =>
        {
            winPopup.gameObject.SetActive(true);
        });
        DataManager.CurrentLevel++;
    }
    public void ShowSetting()
    {
        PopupManager.Instance.ShowSettingPopup(false);
    }
    public void LoadGameScene()
    {
        FadeLoadScene.Fade("Game", Color.black, 2f, null);
    }   
    public void LoadHomeScene()
    {
        FadeLoadScene.Fade("Home", Color.black, 2f, null);
    }    
}
