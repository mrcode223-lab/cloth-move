using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsDialog : BasePopUp
{
    //private const string SOUND_ON = "SOUND_ON";
    //private const string MUSIC_ON = "MUSIC_ON";

    [SerializeField] private Toggle soundToggle;
    [SerializeField] private Toggle musicToggle;
    [SerializeField] private Toggle vibrationToggle;

    [SerializeField] GameObject soundToggleOffGraphic;
    [SerializeField] GameObject soundToggleOnGraphic;
    [SerializeField] GameObject musicToggleOffGraphic;
    [SerializeField] GameObject musicToggleOnGraphic;
    [SerializeField] GameObject vibrationToggleOffGraphic;
    [SerializeField] GameObject vibrationToggleOnGraphic;
    [SerializeField] GameObject ingameButtons;
    private static bool Vibration
    {
        get => PlayerPrefs.GetInt("Vibration", 1) == 1;
        set
        {
            PlayerPrefs.SetInt("Vibration", value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    //private static bool MusicOn
    //{
    //    get => PlayerPrefs.GetInt(MUSIC_ON, 1) == 1;
    //    set
    //    {
    //        PlayerPrefs.SetInt(MUSIC_ON, value ? 1 : 0);
    //        PlayerPrefs.Save();
    //    }
    //}

    protected override void Start()
    {
        base.Start();
        soundToggle.isOn = MusicManager.soundTurn;
        musicToggle.isOn = MusicManager.musicTurn;
        vibrationToggle.isOn = Vibration;
        soundToggle.onValueChanged.AddListener(SetSoundToggle);
        musicToggle.onValueChanged.AddListener(SetMusicToggle);
        vibrationToggle.onValueChanged.AddListener(SetVibrationToggle);
        OnStart();
    }
    public override void Open(UnityAction action = null)
    {
        ingameButtons.SetActive(SceneManager.GetActiveScene().name == "Game");
        base.Open(action);
    }
    protected void OnStart()
    {
        SetSoundToggle(MusicManager.soundTurn);
        SetMusicToggle(MusicManager.musicTurn);
    }

    private void SetSoundToggle(bool value)
    {
        MusicManager.soundTurn = value;
        soundToggleOffGraphic.SetActive(!MusicManager.soundTurn);
        soundToggleOnGraphic.SetActive(MusicManager.soundTurn);
    }
    private void SetVibrationToggle(bool value)
    {
        Vibration = value;
        vibrationToggleOffGraphic.SetActive(!Vibration);
        vibrationToggleOnGraphic.SetActive(Vibration);
    }
    private void SetMusicToggle(bool value)
    {
        MusicManager.musicTurn = value;
        if(value)
        {
            MusicManager.Instance.ResumeBGMusic();
        }   
        else
        {
            MusicManager.Instance.PauseBGMusic();
        }
        musicToggleOffGraphic.SetActive(!MusicManager.musicTurn);
        musicToggleOnGraphic.SetActive(MusicManager.musicTurn);
//#if MOREMOUNTAINS_NICEVIBRATIONS
//            MoreMountains.NiceVibrations.MMVibrationManager.SetHapticsActive(value);
//#endif
    }
    public void OnClickHome()
    {
        FadeLoadScene.Fade("Home", Color.black, 2f, null);
        Close();
    }
}