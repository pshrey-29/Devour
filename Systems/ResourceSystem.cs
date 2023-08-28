
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// One repository for all scriptable objects. Create your query methods here to keep your business logic clean.
/// I make this a MonoBehaviour as sometimes I add some debug/development references in the editor.
/// If you don't feel free to make this a standard class
/// </summary>
public class ResourceSystem : StaticInstance<ResourceSystem> {
    public List<ScriptableHero> Heroes { get; private set; }
    public List<ScriptableEnemy> Enemies { get; private set; }
    public List<ScriptableLevel> Levels { get; private set; }

    // private Dictionary<ExampleHeroType, ScriptableHero> _ExampleHeroesDict;
    public Dictionary<string, ScriptableHero> HeroDictionary;
    public Dictionary<string, ScriptableEnemy> EnemyDictionary;
    public Dictionary<int, ScriptableLevel> LevelDictionary;

    protected override void Awake() {
        base.Awake();
        AssembleResources();
    }

    private void AssembleResources() {
        Heroes = Resources.LoadAll<ScriptableHero>("Heroes").ToList();
        Enemies = Resources.LoadAll<ScriptableEnemy>("Enemies").ToList();
        Levels = Resources.LoadAll<ScriptableLevel>("Levels").ToList();
        // _ExampleHeroesDict = ExampleHeroes.ToDictionary(r => r.HeroType, r => r);
        HeroDictionary = Heroes.ToDictionary(r => r.name, r => r);
        EnemyDictionary = Enemies.ToDictionary(r => r.name, r => r);
        LevelDictionary = Levels.ToDictionary(r => r.level, r => r);
    }

    // public ScriptableExampleHero GetExampleHero(ExampleHeroType t) => _ExampleHeroesDict[t];
    // public ScriptableExampleHero GetRandomHero() => ExampleHeroes[Random.Range(0, ExampleHeroes.Count)];
}   