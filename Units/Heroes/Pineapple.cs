using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pineapple : HeroUnitBase
{
    protected event Action OnAttackTime;

    [SerializeField] private float timeGapInAttack = 1f;
    private Timer _timer;
    private List<EnemyUnitBase> currentTargetEnemies;
    private bool _canAttack = false;
    // Start is called before the first frame update
    void Start()
    {
        currentTargetEnemies = new List<EnemyUnitBase>();
        GameManager.OnFireTime += AttackEnemies;
    }

    private void OnDestroy()
    {
        GameManager.OnFireTime -= AttackEnemies;
    }

    // Update is called once per frame
    void Update()
    {
        // CalculateFireTime();
        CheckWhetherCanAttack();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Enemy"))
        {
            EnemyUnitBase enemy = other.transform.GetComponent<EnemyUnitBase>();
            currentTargetEnemies.Add(enemy);
            enemy.OnDestroyingEnemy += () =>
            {
                int numberofEnemies = currentTargetEnemies.Count;
                currentTargetEnemies.Remove(currentTargetEnemies[numberofEnemies - 1]);
                Debug.Log(currentTargetEnemies.Count);
            };
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

    private void AttackEnemies()
    {
        if (_canAttack)
        {
            for (int i = 0; i < currentTargetEnemies.Count; i++)
            {
                currentTargetEnemies[i].TakeDamage(Stats.AttackPower);
            }
            Debug.Log("Attacked Enemies");
        }
    }

    private void CheckWhetherCanAttack()
    {
        if (currentTargetEnemies.Count > 0)
        {
            _canAttack = true;
        }
        else
        {
            _canAttack = false;
        }
    }
}