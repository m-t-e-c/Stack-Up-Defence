using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UIControl : MonoBehaviour
{
    [Header("===UI Panels===")]
    public GameObject tutorial;
    public GameObject winPanel;
    public GameObject losePanel;
    public GameObject pausePanel;
    public GameObject inGamePanel;

    [Header("===Level Texts===")]
    public TextMeshProUGUI[] levelVariables;
    public TextMeshPro remainingEnemy;
    public TextMeshPro baseHealth;

    [Header("===UI Elements===")]
    public TextMeshProUGUI qualityText;
    public Slider bgmVolume;
    public Slider sfxSlider;
    public Button buttonPrefab;
    public Button speedUp1Button;
    public Button speedUp1_5Button;
    public Button speedUp2Button;
    public Button speedUp4Button;

    public GameObject speedContainer;
    
    // Script References
    [HideInInspector] public SettingsControl settings;
    [HideInInspector] public GameControl game;
    [HideInInspector] public GameObject winParticles;

    private Base _base;

    private int enemyCount;

    private void Start() {
        _base = GameObject.FindWithTag("Base").GetComponent<Base>();
        InvokeRepeating("PlaceTexts",0.1f,0.2f);
        speedUp1Button.interactable = false;
    }

    private void PlaceTexts() {
        baseHealth.SetText(_base.Health.ToString());
        remainingEnemy.SetText(enemyCount.ToString());
    }

    private void OnEnemySpawned(){
       enemyCount++;
    }

    private void OnEnemyKilled(){
       enemyCount--;
    }

    private void OnEnable() {
        Enemy.OnEnemySpawned += OnEnemySpawned;
        Enemy.OnEnemyKilled += OnEnemyKilled;
    }

    private void OnDisable() {
        Enemy.OnEnemySpawned -= OnEnemySpawned;
        Enemy.OnEnemyKilled -= OnEnemyKilled; 
    }

    public void SpeedUp1x(){
        speedUp1Button.interactable = false;
        speedUp1_5Button.interactable = true;
        speedUp2Button.interactable = true;
        speedUp4Button.interactable = true;
        Time.timeScale = 1f;
    }

    public void SpeedUp1_5x(){
        speedUp1Button.interactable = true;
        speedUp1_5Button.interactable = false;
        speedUp2Button.interactable = true;
        speedUp4Button.interactable = true;
        Time.timeScale = 1.5f;
    }

    public void SpeedUp2x(){
        speedUp1Button.interactable = true;
        speedUp1_5Button.interactable = true;
        speedUp2Button.interactable = false;
        speedUp4Button.interactable = true;
        Time.timeScale = 2f;
    }

    public void SpeedUp4x(){
        speedUp1Button.interactable = true;
        speedUp1_5Button.interactable = true;
        speedUp2Button.interactable = true;
        speedUp4Button.interactable = false;
        Time.timeScale = 4f;
    }

#region Scene Initializers
    public void InitializeTexts()
    { 
        winParticles = Camera.main.transform.GetChild(0).gameObject;
        winParticles.SetActive(false);
        foreach(TextMeshProUGUI x in levelVariables)
        {
            x.text = "Level " + game.levelNumber.ToString();
        }
    }
#endregion Scene Initializers
#region Game State
    public void Win()
    {
        game.isGameFinished = true;
        winPanel.SetActive(true);
        winParticles.SetActive(true);
        inGamePanel.SetActive(false);
    }
    public void Lose()
    {
        game.isGameFinished = true;
        losePanel.SetActive(true);
        inGamePanel.SetActive(false);
    }
#endregion Game State
#region Button
    public void Restart()
    {
        settings.AnalyticsTracker("lose");
    }
    public void NextLevel()
    {
        settings.AnalyticsTracker("win");
    }
    public void LevelSelect()
    {
        int x;
        int.TryParse(EventSystem.current.currentSelectedGameObject.name, out x);
        settings.SelectLevel(x);
    }
    public void TogglePausePanel(GameObject y)
    {
        bool x = inGamePanel.activeInHierarchy;
        x =! x;
        inGamePanel.SetActive(x);
        y.SetActive(!x);
        if(!x) Time.timeScale = 0;
        else Time.timeScale = settings.startTimeScale;
    }
    public void PrivacyPolicy()
    {
        Application.OpenURL("http://privacy.dumbbellgames.com");
    }
    public void ToggleQuality()
    {
        settings.ToggleGraphicsQuality();
    }
#endregion Button
}
