using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectileUnitBase : UnitBase
{
    [SerializeField] private float speed = 8f;
    public ScriptableHero Hero;

    // Start is called before the first frame update
    void Start()
    {
        HeroThrustForward(speed);
    }

    private void Update()
    {
        DestroyAutomatically();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Enemy"))
        {
            Debug.Log("Damage Enemy");
            other.GetComponent<EnemyUnitBase>().TakeDamage(Hero.BaseStats.AttackPower);
        }
    }

    private void HeroThrustForward(float speed)
    {
        Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
        Vector2 dir = new Vector2(1,0);
        // rb.AddForce(dir * speed, ForceMode2D.Force);
        rb.velocity = dir * speed;
    }

    public virtual void Despawn()
    {
        Destroy(gameObject);
    }

    private void DestroyAutomatically()
    {
        if(transform.position.x >= 9.5)
        {
            Destroy(gameObject);
        }
    }
}
