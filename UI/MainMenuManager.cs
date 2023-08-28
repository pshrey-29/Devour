using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    GameObject MainMenuUI, LevelsUI, InfoUI, CreditsUI, InfoPanel, FoodPanel, EnemyPanel;
    [SerializeField]
    GameObject InfoButton, FoodButton, EnemyButton;
    [SerializeField] Transform FoodContainer, FoodTemplate, FoodStatsPanel;
    [SerializeField] Transform EnemyContainer, EnemyTemplate, EnemyStatsPanel;


    // Start is called before the first frame update
    void Start()
    {
        CreateFoods();
        CreateEnemies();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play()
    {
        ShowHideUI(LevelsUI,MainMenuUI);
    }

    public void InfoMain()
    {
        ShowHideUI(InfoUI, MainMenuUI);
    }

    public void Credits()
    {
        ShowHideUI(CreditsUI, MainMenuUI);
    }

    public void Quit(){
        Application.Quit();
    }

    public void BackFromLevels()
    {
        ShowHideUI(MainMenuUI, LevelsUI);
    }

    public void BackFromInfo()
    {
        Debug.Log("run");
        ShowHideUI(MainMenuUI, InfoUI);
    }

    public void BackFromCredits()
    {
        ShowHideUI(MainMenuUI, CreditsUI);
    }

    public void SetLevel()
    {
        Transform buttonTransform = EventSystem.current.currentSelectedGameObject.transform;
        LevelSystem.Level = int.Parse(buttonTransform.name);
        SceneManager.LoadScene("PreGame");
    }

    public void ShowInfo()
    {
        ShowPanelandDarkButton(InfoPanel,InfoButton);
    }

    public void ShowFood()
    {
        ShowPanelandDarkButton(FoodPanel,FoodButton);
    }

    public void ShowEnemy()
    {
        ShowPanelandDarkButton(EnemyPanel,EnemyButton);
    }

    private void ShowPanelandDarkButton(GameObject ShowUI, GameObject button)
    {
        DeselectAllInfoButtons();
        ShowUI.SetActive(true);
        button.GetComponent<Image>().color = new Color(100f/255f,100f/255f,100f/255f,1);
    }

    private void ShowHideUI(GameObject ShowUI, GameObject HideUI)
    {
        ShowUI.SetActive(true);
        HideUI.SetActive(false);
    }

    private void DeselectAllInfoButtons()
    {
        InfoButton.GetComponent<Image>().color = Color.white;
        FoodButton.GetComponent<Image>().color = Color.white;
        EnemyButton.GetComponent<Image>().color = Color.white;
        InfoPanel.SetActive(false);
        FoodPanel.SetActive(false);
        EnemyPanel.SetActive(false);
    }

    private void CreateFoods()
    {
        FoodTemplate.gameObject.SetActive(false);
        List<ScriptableHero> Heroes = ResourceSystem.Instance.Heroes;
        List<Transform> FoodTransformList = new List<Transform>();

        foreach (ScriptableHero Hero in Heroes)
        {
            Transform foodTransform = CreateFoodTransform(FoodTemplate, FoodContainer, FoodTransformList);
            CreateFoodData(Hero, foodTransform);
        }
    }

    private void CreateEnemies()
    {
        EnemyTemplate.gameObject.SetActive(false);
        List<ScriptableEnemy> Enemies = ResourceSystem.Instance.Enemies;
        List<Transform> EnemyTransformList = new List<Transform>();

        foreach (ScriptableEnemy enemy in Enemies)
        {
            Transform enemyTransform = CreateEnemyTransform(EnemyTemplate, EnemyContainer, EnemyTransformList);
            CreateFoodData(enemy, enemyTransform);
        }
    }

    private Transform CreateFoodTransform(Transform template, Transform container, List<Transform> transformList)
    {
        float templateWidth = 150f;
        Transform entryTransform = Instantiate(template, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        int numberToStartNewLine = 5;
        if (transformList.Count < numberToStartNewLine)
        {
            entryRectTransform.anchoredPosition =
                new Vector2(templateWidth * transformList.Count, 0);
        }
        else
        {
            entryRectTransform.anchoredPosition =
                new Vector2(templateWidth * (transformList.Count - numberToStartNewLine), -250);
        }
        entryTransform.gameObject.SetActive(true);

        transformList.Add(entryTransform);
        return entryTransform;
    }

    private Transform CreateEnemyTransform(Transform template, Transform container, List<Transform> transformList)
    {
        float templateWidth = 250f;
        Transform entryTransform = Instantiate(template, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        int numberToStartNewLine = 3;
        if (transformList.Count < numberToStartNewLine)
        {
            entryRectTransform.anchoredPosition =
                new Vector2(templateWidth * transformList.Count, 0);
        }
        else
        {
            entryRectTransform.anchoredPosition =
                new Vector2(templateWidth * (transformList.Count - numberToStartNewLine), -250);
        }
        entryTransform.gameObject.SetActive(true);

        transformList.Add(entryTransform);
        return entryTransform;
    }

    private void CreateFoodData(ScriptableHero Hero, Transform foodTransform)
    {
        foodTransform.GetComponent<Image>().sprite = Hero.MenuSprite;
        foodTransform.GetComponent<Button>().onClick.AddListener(() => { ShowHeroStats(Hero); });
    }

    private void CreateFoodData(ScriptableEnemy enemy, Transform foodTransform)
    {
        foodTransform.GetComponent<Animator>().runtimeAnimatorController = enemy.animator;
        foodTransform.GetComponent<Button>().onClick.AddListener(() => { ShowEnemyStats(enemy); });
    }

    private void ShowEnemyStats(ScriptableEnemy enemy)
    {
        EnemyStatsPanel.Find("Name").GetComponent<Text>().text = enemy.name.ToUpper();
        EnemyStatsPanel.Find("Damage").GetComponent<Text>().text = enemy.BaseStats.AttackPower.ToString();
        EnemyStatsPanel.Find("Speed").GetComponent<Text>().text = enemy.BaseStats.Speed.ToString();
        EnemyStatsPanel.Find("Health").GetComponent<Text>().text = enemy.BaseStats.Health.ToString();
        EnemyStatsPanel.gameObject.SetActive(true);
    }

    private void ShowHeroStats(ScriptableHero hero)
    {
        FoodStatsPanel.Find("Name").GetComponent<Text>().text = hero.name.ToUpper();
        FoodStatsPanel.Find("Calorie").GetComponent<Text>().text = hero.calorieValue.ToString();
        FoodStatsPanel.Find("Damage").GetComponent<Text>().text = hero.BaseStats.AttackPower.ToString();
        FoodStatsPanel.Find("Shots").GetComponent<Text>().text = hero.maxShots.ToString();
        FoodStatsPanel.Find("Health").GetComponent<Text>().text = hero.BaseStats.Health.ToString();
        FoodStatsPanel.Find("Type").GetComponent<Text>().text = hero.Type.ToUpper();
        FoodStatsPanel.gameObject.SetActive(true);
    }
}
