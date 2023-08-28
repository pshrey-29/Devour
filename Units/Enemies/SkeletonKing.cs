using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SkeletonKing : MonoBehaviour
{
    private Timer _timerProjectile, _timerLaneChange;
    private event Action OnProjectileTime, OnLaneChange;

    private GameObject[] SpawnSpots;
    [SerializeField] GameObject prefab;


    // // Start is called before the first frame update
    void Start()
    {
        _timerProjectile = new Timer(3);
        _timerLaneChange = new Timer(10);
        SpawnSpots = GameObject.FindGameObjectsWithTag("Spawn Point");
        SpawnSpots[4] = SpawnSpots[5];
        Debug.Log(transform.position);
        transform.position = SpawnSpots[UnityEngine.Random.Range(0,20) % SpawnSpots.Length]
            .transform.position;
        Debug.Log(transform.position);
        OnProjectileTime += FireProjectile;
        OnLaneChange += ChangeLane;
    }

    // Update is called once per frame
    void Update()
    {
        CalculateFireTime(_timerProjectile,OnProjectileTime);
        CalculateFireTime(_timerLaneChange,OnLaneChange);
    }

    private void CalculateFireTime(Timer timer, Action eventFire)
    {
        if (GameManager.Instance.State == GameState.InGame)
        {
            timer.UpdateTime(Time.deltaTime);

            if (timer.finished)
            {
                Debug.Log("EventFire");
                eventFire?.Invoke();
                timer.Reset();
            }
        }
    }

    private void FireProjectile()
    {
            Vector3 relativePositionToSpawnAt = new Vector3(0, -0.2f, 0);
            float scale = transform.localScale.x;
            Vector3 positionToSpawnAt = transform.position + relativePositionToSpawnAt * scale;
            GameObject projectile = Instantiate(prefab, positionToSpawnAt, Quaternion.identity);
            projectile.transform.localScale *= scale;
    }

    private void ChangeLane()
    {
        float y = SpawnSpots[UnityEngine.Random.Range(0,20) % SpawnSpots.Length]
            .transform.position.y;
        Vector3 pos = new Vector3(transform.position.x, y, 0);
        transform.position = pos;
        Debug.Log(transform.position);

    }
}
