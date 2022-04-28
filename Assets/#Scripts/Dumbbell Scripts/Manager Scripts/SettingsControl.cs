using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using GameAnalyticsSDK;
using ElephantSDK;

public class SettingsControl : MonoBehaviour
{
    [Header("===Setting Variables===")]
    [Range(0,144)] public int frameRate = 60;
    [Range(0,1)] public float startTimeScale = 1f;
    public int totalLevelCount = 30;

    [Header("===Control Type===")]
    public bool joystick = false;
    public bool dragDrop = false;
    public bool swerve = false;
    [Header("===Drag'n Drop Variables===")]
    public float dragDropYOffset;
    public LayerMask draggableLayer, groundLayer;
    [Header("===Swerve Variables===")]
    public bool planeSwerve = false;
    public float swerveSideways, swerveForward, clampValue;
    
    [Header("===Audio Settings===")]
    public bool audioEnabled = false;
    public AudioSource[] audioSources;
    [Range(0,1)] public float audioVolume;

    //Post Processing
    PostProcessLayer ppLayer;
    PostProcessVolume ppVolume;
    bool highQuality = true;
    int y;

    [HideInInspector] public UIControl uIControl;
    [HideInInspector] public GameControl game;
    [HideInInspector] public JoystickControl joystickControl;
    [HideInInspector] public DragDropControl dragDropControl;
    [HideInInspector] public SwerveControl swerveControl;

    public void InitializeSettings()
    {
        Application.targetFrameRate = frameRate;
        Time.timeScale = startTimeScale;  
        game.mainCam = Camera.main.gameObject;
        ppVolume = game.mainCam.GetComponent<PostProcessVolume>();
        ppLayer = game.mainCam.GetComponent<PostProcessLayer>();

        JoystickCheck();
        DragDropCheck();
        SwerveCheck();
        
        AnalyticsTracker("start");
        PopulateLevels();

        if(!audioEnabled) return;
        audioSources = GameObject.FindObjectsOfType<AudioSource>();
        InitializeAudio();
    }

    public void AnalyticsTracker(string y)
    {
        int x = PlayerPrefs.GetInt("unlockedLevel", 1);
        if(game.levelNumber > x) PlayerPrefs.SetInt("unlockedLevel", game.levelNumber);
        if(y == "start")
        {
            Elephant.LevelStarted(game.levelNumber);
            GameAnalytics.Initialize();
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "Level " + game.levelNumber, game.levelTime.ToString("F0"));
            return;
        }
        if(y == "lose")
        {
            Elephant.LevelFailed(game.levelNumber);
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, "Level " + game.levelNumber, game.levelTime.ToString("F0"));
        }
        if(y == "win")
        {
            Elephant.LevelCompleted(game.levelNumber);
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "Level " + game.levelNumber, game.levelTime.ToString("F0"));
            game.levelNumber++;
            PlayerPrefs.SetInt("currentLevelNumber", game.levelNumber);
            PlayerPrefs.Save();
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
#region  Button Functions
    public void InitializeAudio()
    {
        audioVolume = PlayerPrefs.GetFloat("audioVolume", 0.5f);
        foreach(AudioSource x in audioSources) x.volume = audioVolume;
    }

    public void CheckAudioLevel()
    {
        foreach(AudioSource x in audioSources) x.volume = audioVolume;        
    }

    public void SelectLevel(int x)
    {
        if(x > game.levels.Length && game.levels.Length > 0) y = x % game.levels.Length;
        PlayerPrefs.SetInt("currentLevelNumber", y);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ToggleGraphicsQuality()
    {
        highQuality =! highQuality;
        ppVolume.enabled = highQuality;
        ppLayer.enabled = highQuality;
        uIControl.qualityText.text = highQuality ? "HIGH" : "LOW";
    }
    void PopulateLevels()
    {
        uIControl.buttonPrefab.gameObject.SetActive(false);
        var text = uIControl.buttonPrefab.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        var unlockedLevel = PlayerPrefs.GetInt("unlockedLevel", 1);
        for (var i = 1; i <= totalLevelCount; i++) {
            text.text = i.ToString();
            var buttonInstance = Instantiate(uIControl.buttonPrefab, uIControl.buttonPrefab.transform.parent, true);
            buttonInstance.name = i.ToString();
            buttonInstance.interactable = i <= unlockedLevel;
            buttonInstance.gameObject.SetActive(true);
        }
    }
#endregion  Button Functions
#region Input Types
    void JoystickCheck()
    {
        if(!joystick) return;
        game.joystick = GameObject.FindGameObjectWithTag("Joystick").GetComponent<Joystick>();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        joystickControl = playerObj.GetComponent<JoystickControl>();
        if(playerObj == null || joystickControl == null) return;
        joystickControl.InitializeReferences();
    }
    void DragDropCheck()
    {
        if(!dragDrop) return;
        dragDropControl = GetComponent<DragDropControl>();
        dragDropControl.enabled = true;
        dragDropControl.InitializeReferences();
    }
    void SwerveCheck()
    {
        if(!swerve) return;
        swerveControl = GetComponent<SwerveControl>();
        swerveControl.enabled = true;
        swerveControl.InitializeReferences();
    }
#endregion Input Types
}


