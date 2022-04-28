using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    [Header("===Levels===")]
    public Level[] levels;
    [HideInInspector] public int levelIndex;
    [HideInInspector] public int levelNumber;
    [HideInInspector] public GameObject currentLevel;
    [HideInInspector] public float levelTime = 0;
    
    //Ready references
    [HideInInspector] public GameObject mainCam;
    [HideInInspector] public bool isGameStarted = false;
    [HideInInspector] public bool isGameFinished = false;
    [HideInInspector] public Joystick joystick;
    [HideInInspector] public UIControl uIControl;
    [HideInInspector] public SettingsControl settings;

    public bool CheckTutorial()
    {
        if(levelNumber < 3 ) return true;
        else return false;
    }

#region Unity Main
    void Awake()
    {
        InitializeLevel();
        InitializeScripts();
        uIControl.InitializeTexts();
        settings.InitializeSettings();
    }
    void Update()
    {
        CheckGameStart();
        if(!isGameStarted) return;
        CheckInputType();
        if(!isGameFinished) levelTime += Time.deltaTime;
    }
#endregion Unity Main
#region Initializers
    void InitializeLevel()
    {
        levelNumber = PlayerPrefs.GetInt("currentLevelNumber", 1);
        if(levels.Length == 0) return; 
        levelIndex = (levelNumber % levels.Length);
        levels[levelIndex].Load(); 
        currentLevel = levels[levelIndex].currentLevel; 
    }
    void InitializeScripts()
    {
        uIControl = GetComponent<UIControl>();
        settings = GetComponent<SettingsControl>();
        settings.uIControl = uIControl;
        settings.game = this;
        uIControl.settings = settings;
        uIControl.game = this;
    }
#endregion Initializers
#region Game State
    void CheckGameStart()
    {
        if(Input.anyKey)
        {
            uIControl.tutorial.SetActive(false);
            uIControl.speedContainer.SetActive(true);
            isGameStarted = true;
        } 
    }
    void CheckInputType()
    {
        if(settings.joystick) settings.joystickControl.JoystickInput();
        if(settings.dragDrop) settings.dragDropControl.DragAndDrop();
        if(settings.swerve) settings.swerveControl.Swerve();
    }
#endregion Game State
}

