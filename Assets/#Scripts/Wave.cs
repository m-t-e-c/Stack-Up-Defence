using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Wave")]
public class Wave : ScriptableObject
{
    public List<WaveInfo> waves = new List<WaveInfo>();
}


[Serializable]
public struct WaveInfo{
    public GameObject enemyType;
    public int enemyCount;
    public float timeBetweenSpawns;
    public float waveWaitTime;
}