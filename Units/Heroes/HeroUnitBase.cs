using UnityEngine;
using System;

public abstract class HeroUnitBase : UnitBase
{
    // private bool _canMove;
    [SerializeField] protected float maxShots, shotsFired = 0;
    public ScriptableHero Hero;
    public Vector2 SpawnSlot;
    protected event Action OnFireTime;
    public event Action OnDestroyingHero;

    [SerializeField] protected float timeGapInFire = 1f;
    protected Timer _timer;
    private bool _canFire = true;
    protected Sprite profileSprite;


    // private void Awake() => GameManager.OnBeforeStateChanged += OnStateChanged;

    // private void OnDestroy() => GameManager.OnBeforeStateChanged -= OnStateChanged;
       

    void Start()
    {
        _timer = new Timer(timeGapInFire);
    }

    void Update()
    {
        CalculateFireTime();
        DestroyAutomatically();
    }

    // private void OnStateChanged(GameState newState) {
    //     // if (newState == GameState.InGame) _canMove = true;
    // }


    // private void OnMouseDown() {
    //     // Only allow interaction when it's the hero turn
    //     if (ExampleGameManager.Instance.State != GameState.HeroTurn) return;

    //     // Don't move if we've already moved
    //     if (!_canMove) return;

    //     // Show movement/attack options

    //     // Eventually either deselect or ExecuteMove(). You could split ExecuteMove into multiple functions
    //     // like Move() / Attack() / Dance()

    //     Debug.Log("Unit clicked");
    // }

    // public virtual void ExecuteMove() {
    // //     // Override this to do some hero-specific logic, then call this base method to clean up the turn

    // //     _canMove = false;
    // }


    private void CalculateFireTime()
    {
        if (GameManager.Instance.State == GameState.InGame)
        {
            _timer.UpdateTime(Time.deltaTime);

            if (_timer.finished)
            {
                Debug.Log("Fire");
                OnFireTime?.Invoke();
                Debug.Log("Fire");
                _timer.Reset();
            }
        }
    }

    private void DestroyAutomatically()
    {
        if (shotsFired >= maxShots)
        {
            GameManager.Instance.heroSpawnSlots[(int)SpawnSlot.x][(int)SpawnSlot.y] = false;
            Destroy(gameObject);
            Debug.Log("Destroyed after shotLimit Reached");
        }
    }

    public override void TakeDamage(int dmg)
    {
        base.TakeDamage(dmg);
        if(health <= 0)     OnDestroyingHero?.Invoke();
    }


}