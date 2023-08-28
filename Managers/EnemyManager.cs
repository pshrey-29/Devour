using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

public class EnemyManager : StaticInstance<EnemyManager>
{
    public static event Action OnSpawnTime;
    private ScriptableLevel _levelData;
    private Wave currentWave;
    private List<EnemyData> waveData;
    private List<ScriptableEnemy> currentEnemyList;
    // private ScriptableEnemy enemy;
    private Dictionary<ScriptableEnemy, int> WaveDataDictionary;
    public Dictionary<ScriptableEnemy, int> livingSpawnedEnemyDictionary;
    private UnitManager unitManager;
    private int WaveNumber = 0;
    private bool _canSpawn = false;
    [SerializeField] List<Transform> spawnSpots;
    [SerializeField] private float timeBeforeWave = 2f;
    [SerializeField] private float minTimeGapbwSpawn = 1f, maxTimeGapbwSpawn = 5f;


    // Start is called before the first frame update
    void Start()
    {
        _levelData = LevelSystem.levelData;
        unitManager = UnitManager.Instance;
        // enemy = ResourceSystem.Instance.EnemyDictionary["Wizard"];
        // unitManager.SpawnEnemy(enemy, ChooseLocation());
        StartCoroutine(StartGame());
    }

    // Update is called once per frame
    void Update()
    {
        // CheckWhetherCanSpawn();
    }

    private Vector3 ChooseLocation()
    {
        int random = UnityEngine.Random.Range(0, 100);
        int selected = random % 5;
        Vector3 location = spawnSpots[selected].position;
        return location;
    }

    private float ChooseTimeGap()
    {
        float random = UnityEngine.Random.Range(minTimeGapbwSpawn, maxTimeGapbwSpawn);
        return random;
    }

    private ScriptableEnemy ChooseEnemy()
    {
        int random = UnityEngine.Random.Range(0, 100) % currentEnemyList.Count;
        ScriptableEnemy randomEnemy = currentEnemyList[random];
        Debug.Log(randomEnemy);


        return randomEnemy;
    }

    IEnumerator SpawnEnemy()
    {
        ScriptableEnemy enemyToSpawn = ChooseEnemy();
        unitManager.SpawnEnemy(enemyToSpawn, ChooseLocation());
        // Debug.Log("EnemySpawned: " + enemyToSpawn.name);
        WaveDataDictionary[enemyToSpawn]--;

        //removing enemy from waveData if its count is zero
        if (WaveDataDictionary[enemyToSpawn] <= 0)
        {
            WaveDataDictionary.Remove(enemyToSpawn);
            currentEnemyList.Remove(enemyToSpawn);
        }

        yield return new WaitForSeconds(ChooseTimeGap());

    }

    IEnumerator StartWave(Wave wave)
    {
        Debug.Log("Wave Started");
        //Wave Start logic
        WaveNumber++;
        waveData = wave.WaveData;
        currentEnemyList = new List<ScriptableEnemy>();
        foreach (EnemyData enemyData in waveData)
        {
            currentEnemyList.Add(enemyData.Enemy);
        }

        // livingSpawnedEnemyDictionary = waveData.ToDictionary(r => r.Enemy, r => r.Count);
        // for (int i = 0; i < currentEnemyList.Count; i++)
        // {
        //     ScriptableEnemy enemy = currentEnemyList[i];
        //     if (livingSpawnedEnemyDictionary[enemy] <= 0)
        //     {
        //         livingSpawnedEnemyDictionary.Remove(enemy);
        //         // currentEnemyList.Remove(enemy);
        //     }
        // }

        WaveDataDictionary = waveData.ToDictionary(r => r.Enemy, r => r.Count);
        // Debug.Log(WaveDataDictionary.Count);
        int enemyInThisWaveData = currentEnemyList.Count;
        List<ScriptableEnemy> EnemyToDelete = new List<ScriptableEnemy>();
        for (int i = 0; i < currentEnemyList.Count; i++)
        {
            ScriptableEnemy enemy = currentEnemyList[i];
            if (WaveDataDictionary[enemy] <= 0)
            {
                WaveDataDictionary.Remove(enemy);
                EnemyToDelete.Add(enemy);
            }
        }

        foreach(ScriptableEnemy enemy in EnemyToDelete)
        {
                currentEnemyList.Remove(enemy);
        }


        // Debug.Log("Dictionary size" + livingSpawnedEnemyDictionary.Count);

        yield return new WaitForSeconds(timeBeforeWave);
        // Debug.Log(WaveDataDictionary.Count);


        //In wave logic
        while (WaveDataDictionary.Count > 0)
        {
            yield return SpawnEnemy();
        }


        //before Wave End Logic
        //checking if wave finished


        Debug.Log("Wave Ended");

        // if(livingSpawnedEnemyDictionary.Count <= 0)
        // {
        //     if(WaveNumber >= LevelSystem.Instance.levelData.Waves.Count)
        //     {

        //     }
        // }
    }

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(5);
        livingSpawnedEnemyDictionary = LevelSystem.EnemyDataofLevel;
        int dictionarySize = livingSpawnedEnemyDictionary.Count;
        foreach(ScriptableEnemy enemy in ResourceSystem.Instance.Enemies)
        {
            if(livingSpawnedEnemyDictionary[enemy] <= 0)
            {
                livingSpawnedEnemyDictionary.Remove(enemy);
            }
        }

        Debug.Log("Game Started");
        for (int i = 0; i < LevelSystem.levelData.Waves.Count; i++)
        {
            yield return StartWave(_levelData.Waves[i]);
            if (i < (_levelData.Waves.Count - 1))
            {
                yield return new WaitForSeconds(10);
                Debug.Log("NextWave");
            }
            else
            {
                while (livingSpawnedEnemyDictionary.Count > 0)
                {
                    Debug.Log("living type of enemy: " +livingSpawnedEnemyDictionary.Count);
                    yield return new WaitForSeconds(1);
                    Debug.Log("Waiting for end");
                }
            }
        }

        GameManager.Instance.ChangeState(GameState.Win);
    }

    private void FireSpawnEvent()
    {
        if (GameManager.Instance.State == GameState.InGame)
        {
            if (WaveDataDictionary.Count > 0)
            {
                OnSpawnTime?.Invoke();
                Debug.Log("Spawn Enemy");
            }
        }
    }

    // private void CheckWhetherCanSpawn()
    // {
    //     if(WaveDataDictionary.Count > 0)
    //     {
    //         _canSpawn = true;
    //     }
    //     else
    //     {
    //         _canSpawn = false;
    //     }
    // }


}
