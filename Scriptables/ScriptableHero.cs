using System;
using UnityEngine;

/// <summary>
/// Create a scriptable hero 
/// </summary>
[CreateAssetMenu(fileName = "New Hero", menuName = "Hero")]
public class ScriptableHero : ScriptableUnitBase {
    public GameObject projectilePrefab;
    public int calorieValue;
    public int maxShots;
    public string Type;
    public FoodType foodType;
    public int propertyValue;
    public int initialFoodCount{get;private set;}
 
}

[Serializable]
public enum FoodType {
    Sugar = 0,
    Fat = 1
}

