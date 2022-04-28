using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Levels/Levels", order = 1)]
public class Level : ScriptableObject 
{
    public GameObject levelPrefab;
    public GameObject currentLevel;
    public string levelType;
    public void Load()
    {
        if(levelPrefab != null) currentLevel = Instantiate(levelPrefab) as GameObject;
    }
}
