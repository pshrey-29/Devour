using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileBase : UnitBase
{
    [SerializeField] private float speed = 8f;

    // Start is called before the first frame update
    void Start()
    {
        HeroThrustForward(speed);
    }

    private void Update()
    {
        // DestroyAutomatically();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Hero"))
        {
            Debug.Log("Damage Hero");
            other.GetComponent<HeroUnitBase>().TakeDamage(
                ResourceSystem.Instance.EnemyDictionary["Skeleton King"].BaseStats.AttackPower
            );
            Destroy(gameObject);
        }
        if(other.CompareTag("Finish"))
        {
            Destroy(gameObject);
        }
    }

    private void HeroThrustForward(float speed)
    {
        Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
        Vector2 dir = new Vector2(-1,0);
        // rb.AddForce(dir * speed, ForceMode2D.Force);
        rb.velocity = dir * speed;
    }
}
