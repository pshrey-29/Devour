using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameEnvironmentManager : MonoBehaviour
{
    public Transform foodContainer, ReferenceSpawnPoint;
    public Transform foodTemplate;
    private List<Transform> FoodTransformList;
    private List<ScriptableHero> Heroes;
    // Start is called before the first frame update
    void Start()
    {
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

        foreach (ScriptableHero Hero in Heroes)
        {
            if (LevelSystem.foodsInPlate[Hero] > 0)
            {
                Transform foodTransform = CreateTransform(foodTemplate, foodContainer, FoodTransformList);
                CreateFoodData(Hero, foodTransform);
            }

        }

    }

    private Transform CreateTransform(Transform template, Transform container, List<Transform> transformList)
    {
        float templateHeight = 100f;
        Transform entryTransform = Instantiate(template, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, 25 - templateHeight * transformList.Count);
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
        foodTransform.GetComponent<Button>().onClick.AddListener(() => { SelectHeroToDeploy(Hero); });
    }

    private void SelectHeroToDeploy(ScriptableHero hero)
    {
        Transform buttonTransform = EventSystem.current.currentSelectedGameObject.transform;
        string foodName = buttonTransform.Find("Food Name").GetComponent<Text>().text;
        Debug.Log(foodName);
        if (LevelSystem.foodsInPlate[hero] > 0)
        {
            GameManager.Instance.currentsSelectedHero = hero;
            // LevelSystem.Instance.foodsInPlate[hero]--;
        }
        // ShouldAddHeroButtonbeActive();
    }

    public void Deploy()
    {
        Transform buttonTransform = EventSystem.current.currentSelectedGameObject.transform;
        if (GameManager.Instance.currentsSelectedHero != null)
        {
            ScriptableHero heroToDeploy = GameManager.Instance.currentsSelectedHero;
            if (LevelSystem.foodsInPlate[heroToDeploy] > 0)
            {
                // Debug.Log(buttonTransform.parent.position);
                Vector2 SpawnPoint = GetSpawnPoint(buttonTransform);
                Vector3 positionToSpawn = GetSpawnLocation(buttonTransform, SpawnPoint);
                Debug.Log(positionToSpawn);
                UnitManager.Instance.SpawnHero(heroToDeploy, positionToSpawn, SpawnPoint);
            }
        }


        UpdateFoodCountInPlate();
        ShouldAddHeroButtonbeActive();
    }

    private Vector3 GetSpawnLocation(Transform button, Vector2 SpawnPoint)
    {

        Vector3 refLocation = ReferenceSpawnPoint.position;
        // Vector3 scale = ReferenceSpawnPoint.localScale;
        Vector3 scale = button.parent.parent.GetComponent<RectTransform>().localScale;

        Vector3 location = refLocation + 
            new Vector3(SpawnPoint.x * 1.38f * scale.x, SpawnPoint.y * -1.54f * scale.y, 0);
        return location;
    }

    private Vector2 GetSpawnPoint(Transform button)
    {
        float positionY = button.parent.GetComponent<RectTransform>().localPosition.y;
        float positionX = button.GetComponent<RectTransform>().localPosition.x;
        // Vector3 scale = button.parent.parent.GetComponent<RectTransform>().localScale;

        float pointX = positionX / (150);
        float pointY = -positionY / (166.5f);

        Vector2 Point = new Vector2(pointX,pointY);

        Debug.Log(Point);

        return Point;
    }

    private void UpdateFoodCountInPlate()
    {
        foreach (Transform food in FoodTransformList)
        {
            string name = food.Find("Food Name").GetComponent<Text>().text;
            ScriptableHero hero = ResourceSystem.Instance.HeroDictionary[name];
            food.Find("Food number").GetComponent<Text>().text = LevelSystem
                .foodsInPlate[hero].ToString();
        }
    }

    private void ShouldAddHeroButtonbeActive()
    {
        foreach (Transform food in FoodTransformList)
        {
            string name = food.Find("Food Name").GetComponent<Text>().text;
            if (LevelSystem.foodsInPlate[ResourceSystem.Instance.HeroDictionary[name]] > 0)
            {
                food.GetComponent<Button>().interactable = true;
            }
            else
            {
                if(food.GetComponent<Button>().interactable)
                food.GetComponent<Button>().interactable = false;
                // GameManager.Instance.currentsSelectedHero = null;
                Debug.Log(name + " disabled");
            }
        }
    }
}
