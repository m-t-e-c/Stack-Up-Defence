using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    private TowerList towerList;
    [SerializeField] private float timeBetweenSpawns;
    public float waitTime;

    private void Start() {
        waitTime = 4.9f;
        towerList = GameObject.FindWithTag("TowerList").GetComponent<TowerList>();
    }

    private void Update() {
        waitTime += Time.deltaTime;
        if(waitTime >= timeBetweenSpawns){
            waitTime = 0;
            SpawnTower();
        }
    }

    private void SpawnTower(){
        var unBuildedTower = 0;
        var allTowers = FindObjectsOfType<Tower>();
        foreach(Tower tower in allTowers){
            if(!tower.isBuilded){
                unBuildedTower++;
            }
        }

        if(unBuildedTower > 0) return;
        Instantiate(towerList.RandomTower(),transform.position,Quaternion.identity);
    }
}
