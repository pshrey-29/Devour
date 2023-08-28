using UnityEngine;

/// <summary>
/// An example of a scene-specific manager grabbing resources from the resource system
/// Scene-specific managers are things like grid managers, unit managers, environment managers etc
/// </summary>
public class UnitManager : StaticInstance<UnitManager>
{

    // public void SpawnHeroes() {
    //     SpawnUnit(ExampleHeroType.Tarodev, new Vector3(1, 0, 0));
    // }

    GameObject SpawnUnit(ScriptableUnitBase t, Vector3 pos, Quaternion rot)
    {
        // var tarodevScriptable = ResourceSystem.Instance.GetExampleHero(t);

        GameObject spawned = Instantiate(t.prefab, pos, rot);// ,transform);

        // Apply possible modifications here such as potion boosts, team synergies, etc
        Stats stats = t.BaseStats;
        // stats.Health += 20;

        // spawned.SetStats(stats);
        spawned.GetComponent<UnitBase>().SetStats(stats);

        return spawned;
    }

    public void SpawnHero(ScriptableHero Hero, Vector3 pos, Vector2 slot)
    {
        if (!GameManager.Instance.heroSpawnSlots[(int)slot.x][(int)slot.y] &&
            LevelSystem.foodsInPlate[Hero] > 0)
        {
            GameObject spawned = SpawnUnit(Hero, pos, Quaternion.identity);
            spawned.GetComponent<HeroUnitBase>().Hero = Hero;
            spawned.GetComponent<HeroUnitBase>().SpawnSlot = slot;
            GameManager.Instance.heroSpawnSlots[(int)slot.x][(int)slot.y] = true;
            LevelSystem.foodsInPlate[Hero]--;
        }
    }

    public void SpawnEnemy(ScriptableEnemy enemy, Vector3 pos)
    {
        GameObject spawned = SpawnUnit(enemy, pos, Quaternion.Euler(0, 180, 0));
        spawned.GetComponent<EnemyUnitBase>().Enemy = enemy;
    }
}