using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "Level")]
public class ScriptableLevel : ScriptableObject
{
    public int level;
    public int plateSize;
    public List<Wave> Waves;
    // [SerializeField] private EnemyCount _stats;
    // public EnemyCount BaseStats => _stats; 
}

[Serializable]
public struct EnemyData
{
    public ScriptableEnemy Enemy;
    public int Count;
}

[Serializable]
public struct Wave
{
    public List<EnemyData> WaveData;
}
