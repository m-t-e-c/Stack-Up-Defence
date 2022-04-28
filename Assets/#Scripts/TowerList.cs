using System.Collections.Generic;
using UnityEngine;

public class TowerList : MonoBehaviour
{
    public List<GameObject> Towers;

    public GameObject GetTower(TowerType type1, TowerType type2)
    {
        if(type1 != type2){
          return null;
        }   

        switch (type1)
        {
            case TowerType.LEVEL0: return Towers[1].gameObject;
            case TowerType.LEVEL1: return Towers[2].gameObject;
            case TowerType.LEVEL2: return Towers[3].gameObject;
            case TowerType.LEVEL3: return Towers[4].gameObject;
            case TowerType.LEVEL4: return Towers[5].gameObject;
            case TowerType.LEVEL5: return Towers[6].gameObject;
            default: return null;
        }
    }

    public GameObject RandomTower(){
        return Towers[Random.Range(0,2)];
    }
}
