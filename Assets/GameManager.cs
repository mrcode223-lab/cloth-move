using DG.Tweening;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] GameObject losePopup, winPopup;
    [SerializeField] ParticleSystem winFx;
    private void Awake()
    {
        Instance = this;
        Application.targetFrameRate = 120;
    }
    private void Start()
    {
        int levelIndex = ((DataManager.CurrentLevel - 1) % 10) + 1;

        var level = Instantiate(Resources.Load<GameObject>($"Levels/Level {levelIndex}"));
    }
    public void ShowLosePopup()
    {
        if (winPopup.activeInHierarchy) return;
        losePopup.gameObject.SetActive(true);
    }
    public void ShowWinPopup()
    {
        if (losePopup.activeInHierarchy) return;
        winFx.Play();
        DOVirtual.DelayedCall(2f, () =>
        {
            winPopup.gameObject.SetActive(true);
        });
        DataManager.CurrentLevel++;
    }
    public void LoadGameScene()
    {
        
    }   
    public void LoadHomeScene()
    {

    }    
}
