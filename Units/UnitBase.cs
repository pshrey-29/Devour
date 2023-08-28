using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This will share logic for any unit on the field. Could be friend or foe, controlled or not.
/// Things like taking damage, dying, animation triggers etc
/// </summary>
public class UnitBase : MonoBehaviour {
    public Stats Stats { get; private set; }
    protected int health;
    public virtual void SetStats(Stats stats) {
        Stats = stats;
        health = Stats.Health;
    } 

    public virtual void TakeDamage(int dmg) {
        health -= dmg;
        // Debug.Log(health);
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}