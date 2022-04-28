using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using PathCreation.Examples;

public class EnemySpawner : MonoBehaviour
{
    [Header("Referances")]
    [SerializeField] private List<PathCreator> paths;
    [SerializeField] private Wave _Wave;
    private GameControl gameControl;
    private UIControl uiControl;

    [Header("States")]
    [SerializeField] private bool isAllWavesEnd;
    public bool isWaveSpawning;
    public bool isWaveEnded;
    public bool lastWave;
    public bool firstStart;

    [Header("Config")]
    public int aliveEnemyCount;
    public int currentWaveIndex;
    public int unBuildedTowers;

    #region Unity Methods
    private void Start()
    {
        firstStart = true;
        gameControl = GameObject.FindWithTag("Settings").GetComponent<GameControl>();
        uiControl = GameObject.FindWithTag("Settings").GetComponent<UIControl>();
        AddPaths();
    }

    private void Update()
    {
        if (gameControl.isGameFinished || !gameControl.isGameStarted) return;
        CheckTowers();

        if(firstStart) return;
        if(isWaveEnded){
            CheckEnemies();
        }

        if(!isWaveSpawning){
            isWaveSpawning = true;
            StartCoroutine(StartWave());
        }
    }

    private void CheckTowers(){
        unBuildedTowers = 0;
        var allTowers = FindObjectsOfType<Tower>();
        foreach(Tower tower in allTowers){
            if(!tower.isBuilded){
                unBuildedTowers++;
            }
        }

        if(unBuildedTowers != 0) return;
        firstStart = false;
    }
    #endregion

    #region EnemySpawner Methods
    private void CheckEnemies(){
        aliveEnemyCount = FindObjectsOfType<Enemy>().Length;
        if(aliveEnemyCount == 0){
            if(lastWave){
                StartCoroutine(GameOver());
                return;
            }
            isWaveSpawning = false;
            isWaveEnded = false;
        }
    }
    private IEnumerator StartWave(){
        yield return new WaitForSeconds(_Wave.waves[currentWaveIndex].waveWaitTime);
        for (int i = 0; i < _Wave.waves[currentWaveIndex].enemyCount; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(_Wave.waves[currentWaveIndex].timeBetweenSpawns);
        }

        if(currentWaveIndex == _Wave.waves.Count - 1){
            lastWave = true;
        }
        currentWaveIndex++;
        isWaveEnded = true;
    }

    private IEnumerator GameOver(){
        yield return new WaitForSeconds(1.5f);
        gameControl.isGameFinished = true;
        uiControl.Win();
    }

    private void AddPaths(){
        foreach(Transform child in GameObject.FindWithTag("Paths").transform){
            paths.Add(child.GetComponent<PathCreator>());
        }
    }

    private void SpawnEnemy()
    {
        var enemyPrefab = _Wave.waves[currentWaveIndex].enemyType;
        var enemy =  Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        var path = paths[Random.Range(0, paths.Count)];
        enemy.GetComponent<PathFollower>().pathCreator = path;
    }
    #endregion
}