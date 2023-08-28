using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelSystem : StaticInstance<LevelSystem>
{
    public static int Level = 4;
    public static ScriptableLevel levelData;
    List<ScriptableHero> Heroes;
    public static Dictionary<ScriptableHero,int> foodsInPlate;
    public static Dictionary<ScriptableEnemy, int> EnemyDataofLevel;

    private void Start()
    {
        // levelData = ResourceSystem.Instance.LevelDictionary[Level];
        // EnemyDataofLevel = ResourceSystem.Instance.Enemies.ToDictionary(r => r, r => 0);
        // CountEnemies(levelData);
        // Heroes = ResourceSystem.Instance.Heroes;
        // CreateDefaultPlate();
    }

    // public static ScriptableLevel GetLeveldData()
    // {
    //     ScriptableLevel levelData = ResourceSystem.Instance.LevelDictionary[Level];
    //     return levelData;
    // }

    // private void CreateDefaultPlate()
    // {
    //     foodsInPlate = Heroes.ToDictionary(r => r, r => 0);
    //     foodsInPlate[Heroes[0]] += 3;
    //     foodsInPlate[Heroes[1]] += 2;
    //     foodsInPlate[Heroes[2]] += 5;
    //     foodsInPlate[Heroes[8]] += 4;
    //     foodsInPlate[Heroes[9]] += 3;

    // }

    // private void ShowPlate()
    // {
    //     foreach(ScriptableHero hero in Heroes)
    //     {
    //         Debug.Log(foodsInPlate[hero] + " ");
    //     }
    // }

    // public static void UpdateLevel(int newLevel)
    // {
    //     Level = newLevel;
    // }

    public static void GetEnemyDataOfLevel()
    {

    }

    private void CountEnemies(ScriptableLevel levelData)
    {
        foreach (Wave wave in levelData.Waves)
        {
            foreach (EnemyData enemy in wave.WaveData)
            {
                EnemyDataofLevel[enemy.Enemy] += enemy.Count;
            }
        }

        return;
    }
    
}
