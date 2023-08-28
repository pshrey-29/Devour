using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PreGameManager : MonoBehaviour
{
    public static event Action OnPreGameStart;
    public static event Action OnPreGameEnd;    public Transform foodContainer, enemyContainer;
    public Transform foodTemplate, enemyTemplate;
    private List<Transform> FoodTransformList, EnemyTransformList;
    private List<ScriptableHero> Heroes;
    private List<ScriptableEnemy> Enemies;
    private List<ScriptableLevel> Levels;
    // private List<EnemyCount> EnemiesList;
    [SerializeField] private Text plateSizeText;
    private ScriptableLevel _levelData;
    private int plateSpace = 0;
    private Dictionary<ScriptableEnemy, int> EnemyDataofLevel;
    [SerializeField] GameObject EatingUI, GameUI;
    [SerializeField] AudioSource Music, EatingAudio;



    private void Awake()
    {
        OnPreGameEnd += LoadNextScene;
    }

    private void OnDestroy()
    {
        OnPreGameEnd -= LoadNextScene;
    }


    // Start is called before the first frame update
    void Start()
    {
        EnemyDataofLevel = ResourceSystem.Instance.Enemies.ToDictionary(r => r, r => 0);
        _levelData = ResourceSystem.Instance.LevelDictionary[LevelSystem.Level];
        CountEnemies(_levelData);
        //Setting up persistent static data;
        LevelSystem.levelData = _levelData;
        LevelSystem.EnemyDataofLevel = EnemyDataofLevel;
        OnPreGameStart?.Invoke();
        ShowLevelData();
        CreateFoods();
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void CreateFoods()
    {
        foodTemplate.gameObject.SetActive(false);
        Heroes = ResourceSystem.Instance.Heroes;
        FoodTransformList = new List<Transform>();
        LevelSystem.foodsInPlate = Heroes.ToDictionary(r => r, r => 0);

        foreach (ScriptableHero Hero in Heroes)
        {
            Transform foodTransform = CreateFoodTransform(foodTemplate, foodContainer, FoodTransformList);
            CreateFoodData(Hero, foodTransform);
        }

        ShouldAddHeroButtonbeActive(plateSpace);
    }

    private Transform CreateFoodTransform(Transform template, Transform container, List<Transform> transformList)
    {
        float templateHeight = 100f;
        Transform entryTransform = Instantiate(template, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        if (transformList.Count < 5)
        {
            entryRectTransform.anchoredPosition =
                new Vector2(0, -templateHeight * transformList.Count);
        }
        else
        {
            entryRectTransform.anchoredPosition =
                new Vector2(550, -templateHeight * (transformList.Count - 5));
        }
        entryTransform.gameObject.SetActive(true);

        transformList.Add(entryTransform);
        return entryTransform;
    }

    private Transform CreateEnemyTransform(Transform template, Transform container, List<Transform> transformList)
    {
        float templateHeight = 120f;
        Transform entryTransform = Instantiate(template, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        transformList.Add(entryTransform);
        return entryTransform;
    }

    private void CreateFoodData(ScriptableHero Hero, Transform foodTransform)
    {
        foodTransform.GetComponent<Image>().sprite = Hero.MenuSprite;
        foodTransform.Find("Food Name").GetComponent<Text>().text = Hero.name;
        foodTransform.Find("Food number").GetComponent<Text>()
            .text = LevelSystem.foodsInPlate[Hero].ToString();
        foodTransform.GetComponent<Button>().onClick.AddListener(() => { AddFoodToPlate(Hero); });
        foodTransform.Find("Remove Food").GetComponent<Button>()
            .onClick.AddListener(() => { RemoveFromPlate(Hero); });
    }

    private void AddFoodToPlate(ScriptableHero Hero)
    {
        Transform buttonTransform = EventSystem.current.currentSelectedGameObject.transform;
        string foodName = buttonTransform.Find("Food Name").GetComponent<Text>().text;
        Debug.Log(foodName);

        //calculating remaining space in plate
        int tempSpace = plateSpace;
        plateSpace -= Hero.calorieValue;

        if (plateSpace < 0)
        {
            plateSpace = tempSpace;
            return;
        }
        plateSizeText.text = plateSpace.ToString();
        LevelSystem.foodsInPlate[Hero]++;
        buttonTransform.Find("Food number").GetComponent<Text>()
            .text = LevelSystem.foodsInPlate[Hero].ToString();

        buttonTransform.Find("Remove Food").GetComponent<Button>().interactable = true;

        //checking if any hero cannot be added anymore
        ShouldAddHeroButtonbeActive(plateSpace);

    }

    private void RemoveFromPlate(ScriptableHero Hero)
    {
        Transform buttonTransform = EventSystem.current.currentSelectedGameObject.transform;
        string foodName = buttonTransform.parent.Find("Food Name").GetComponent<Text>().text;
        Debug.Log(foodName);

        //Checking for food count in plate
        if (LevelSystem.foodsInPlate[Hero] <= 0) return;
        LevelSystem.foodsInPlate[Hero]--;
        //calculating remaining space in plate
        plateSpace += Hero.calorieValue;
        plateSizeText.text = plateSpace.ToString();

        buttonTransform.parent.Find("Food number").GetComponent<Text>()
            .text = LevelSystem.foodsInPlate[Hero].ToString();


        //remove button should be active or not

        if (LevelSystem.foodsInPlate[Hero] <= 0)
        {
            buttonTransform.GetComponent<Button>().interactable = false;
        }

        //checking if any hero can be added now
        ShouldAddHeroButtonbeActive(plateSpace);

    }

    private void ShouldAddHeroButtonbeActive(float inputPlateSpace)
    {
        foreach (Transform food in FoodTransformList)
        {
            string name = food.Find("Food Name").GetComponent<Text>().text;
            if (inputPlateSpace >= ResourceSystem.Instance.HeroDictionary[name].calorieValue)
            {
                food.GetComponent<Button>().interactable = true;
            }
            else
            {
                food.GetComponent<Button>().interactable = false;
            }
        }
    }

    private void ShowLevelData()
    {
        enemyTemplate.gameObject.SetActive(false);
        Levels = ResourceSystem.Instance.Levels;
        EnemyTransformList = new List<Transform>();
        Enemies = ResourceSystem.Instance.Enemies;
        // EnemyDataofLevel = Enemies.ToDictionary(r => r, r => 0);

        plateSpace = _levelData.plateSize;
        plateSizeText.text = plateSpace.ToString();
        foreach (ScriptableEnemy enemy in Enemies)
        {
            if (EnemyDataofLevel[enemy] > 0)
            {
                Transform enemyTransform = CreateEnemyTransform(enemyTemplate, enemyContainer, EnemyTransformList);
                CreateEnemyData(enemy, enemyTransform);
                Debug.Log("loop ran");
            }
        }
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



    // private bool SearchInEnemyList(List<EnemyCount> enemiesList, EnemyData enemyToSearch)
    // {
    //     bool isMatchFound = false;
    //     if (enemiesList.Count > 0)
    //     {
    //         Debug.Log(enemiesList.Count);
    //         foreach (EnemyCount existingEnemy in enemiesList)
    //         {
    //             if (enemyToSearch.Enemy == existingEnemy.Enemy)
    //             {
    //                 existingEnemy.Count += enemyToSearch.Count;
    //                 Debug.Log("Added to existing");
    //                 Debug.Log(existingEnemy.Count);
    //                 isMatchFound = true;
    //                 return isMatchFound;
    //             }
    //         }
    //     }

    //     return isMatchFound;
    // }

    private void CreateEnemyData(ScriptableEnemy enemy, Transform enemyTransform)
    {
        enemyTransform.Find("Enemy name").GetComponent<Text>().text = enemy.name;
        enemyTransform.Find("Enemy count").GetComponent<Text>().text = EnemyDataofLevel[enemy].ToString();
        enemyTransform.Find("Profile").GetComponent<Animator>().runtimeAnimatorController = 
            enemy.animator;
    }

    public void NextScene()
    {
        OnPreGameEnd?.Invoke();
    }

    private void LoadNextScene()
    {
        StartCoroutine(Eating());
    }

    IEnumerator Eating()
    {
        EatingUI.SetActive(true);
        GameUI.SetActive(false);
        Music.Stop();
        EatingAudio.Play();
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("MainScene");
    }

}
