using System;
using UnityEngine;

/// <summary>
/// Keeping all relevant information about a unit on a scriptable means we can gather and show
/// info on the menu screen, without instantiating the unit prefab.
/// </summary>
public abstract class ScriptableUnitBase : ScriptableObject {
    public Faction Faction;
    public new string name;
    public GameObject prefab;

    [SerializeField] private Stats _stats;
    public Stats BaseStats => _stats;

    // Used in game
    // public HeroUnitBase Prefab;
    
    // Used in menus
    public string Description;
    public Sprite MenuSprite;
}

/// <summary>
/// Keeping base stats as a struct on the scriptable keeps it flexible and easily editable.
/// We can pass this struct to the spawned prefab unit and alter them depending on conditions.
/// </summary>
[Serializable]
public struct Stats {
    public int Health;
    public int AttackPower;

    //for enemy
    public int Speed;
}

[Serializable]
public enum Faction {
    Heroes = 0,
    Enemies = 1
}