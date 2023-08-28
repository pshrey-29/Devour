using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleHitProjectile : ProjectileUnitBase {
    // [SerializeField] private AudioClip _someSound;

    // void Start() {
    //     // Example usage of a static system
    //     AudioSystem.Instance.PlaySound(_someSound);
    // }
    //

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.transform.CompareTag("Enemy"))
        {
            Debug.Log("Damage Enemy");
            other.GetComponent<EnemyUnitBase>().TakeDamage(Hero.BaseStats.AttackPower);
            Destroy(gameObject, 0.05f);
        }
    }
    
}