using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public class EnemyUnitBase : UnitBase
{
    public ScriptableEnemy Enemy;
    protected event Action OnAttackTime;
    public event Action OnDestroyingEnemy;
    [SerializeField] protected float timeGapInAttack = 1f;
    protected Timer _timer;
    private HeroUnitBase currentTargetHero;
    protected bool _canAttack = false, _canMove = true, _isDying = false;
    protected Animator animator;
    [SerializeField] protected float speed = 20;
    [SerializeField] private float speedConstant = 0.01f, accelerationConstant = 0.05f;

    void Start()
    {
        _timer = new Timer(timeGapInAttack);
        animator = gameObject.GetComponent<Animator>();
        // health = Enemy.BaseStats.Health;
    }

    void Update()
    {
        CalculateFireTime();
        EnemyThrustForward(Stats.Speed);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        //Avoiding collision with enemy
        if (other.gameObject.CompareTag("Enemy"))
        {
            Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(),
                other.gameObject.GetComponent<Collider2D>(), true);
        }

        //Stoping movement after colliding with hero
        if (other.gameObject.CompareTag("Hero"))
        {
            _canMove = false;
            currentTargetHero = other.transform.GetComponent<HeroUnitBase>();
            _canAttack = true;
            OnAttackTime += Attack;
            currentTargetHero.OnDestroyingHero += MoveOnAfterDestroyingHero;
            animator.SetBool("IsAttacking", true);
        }

        if (other.gameObject.CompareTag("Finish"))
        {
            GameManager.Instance.ChangeState(GameState.Lose);
        }
    }

    protected void EnemyThrustForward(float speed)
    {
        Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
        Vector2 dir = new Vector2(-1, 0);
        if (_canMove)
        {
            if (rb.velocity.x > dir.x * speedConstant * speed)
            {
                rb.AddForce(dir * accelerationConstant* Time.deltaTime * speed, ForceMode2D.Force);
            }
        }
        else
        {
            rb.velocity = new Vector2(0, 0);
        }
    }

    private void CalculateFireTime()
    {
        if (GameManager.Instance.State == GameState.InGame)
        {
            _timer.UpdateTime(Time.deltaTime);

            if (_timer.finished)
            {
                Debug.Log("Attack");
                OnAttackTime?.Invoke();
                _timer.Reset();
            }
        }
    }

    private void Attack()
    {
        currentTargetHero.TakeDamage(Stats.AttackPower);
    }

    private void MoveOnAfterDestroyingHero()
    {
        _canAttack = false;
        _canMove = true;
        OnAttackTime -= Attack;
        animator.SetBool("IsAttacking", false);
        currentTargetHero.OnDestroyingHero -= MoveOnAfterDestroyingHero;
        currentTargetHero = null;
    }

    public override void TakeDamage(int dmg)
    {
        health -= dmg;
        Debug.Log(health);

        if (health <= 0 && !_isDying)
        {
            _isDying = true;
            _canMove = false;
            _canAttack = false;
            animator.SetBool("IsDying", true);
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            Destroy(gameObject, 1.33f);

            OnDestroyingEnemy?.Invoke();
            EnemyManager enemyManager = EnemyManager.Instance;
            enemyManager.livingSpawnedEnemyDictionary[Enemy]--;
            Debug.Log(Enemy.name + " " + enemyManager.livingSpawnedEnemyDictionary[Enemy]);
            if (enemyManager.livingSpawnedEnemyDictionary[Enemy] <= 0)
            {
                Debug.Log(Enemy.name + " removing");
                enemyManager.livingSpawnedEnemyDictionary.Remove(Enemy);
                Debug.Log(Enemy.name + " removed");
                Debug.Log(enemyManager.livingSpawnedEnemyDictionary.Count);
            }
        }
    }
}
