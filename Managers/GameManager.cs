using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Nice, easy to understand enum-based game manager. For larger and more complex games, look into
/// state machines. But this will serve just fine for most games.
/// </summary>
public class GameManager : StaticInstance<GameManager>
{
    public static event Action<GameState> OnBeforeStateChanged;
    public static event Action<GameState> OnAfterStateChanged;
    public static event Action OnFireTime;
    public List<List<bool>> heroSpawnSlots = new List<List<bool>>();
    public ScriptableHero currentsSelectedHero;

    [SerializeField] private float timeGapInFire = 10f;
    private Timer _timer;
    private bool _canFire = true;

    public GameState State { get; private set; }

    // Kick the game off with the first state
    void Start()
    {

        _timer = new Timer(timeGapInFire);
        InitializeSpawnSlots();
        ChangeState(GameState.Starting);
    }
    void Update()
    {
        CalculateFireTime();
    }

    public void ChangeState(GameState newState)
    {
        OnBeforeStateChanged?.Invoke(newState);


        Debug.Log($"New state: {newState}");

        State = newState;
        switch (newState)
        {
            case GameState.Starting:
                HandleStarting();
                break;
            case GameState.InGame:
                HandleInGame();
                break;
            case GameState.Win:
                break;
            case GameState.Lose:
                break;
            case GameState.Pause:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnAfterStateChanged?.Invoke(newState);
    }

    private void HandleStarting()
    {
        // Do some start setup, could be environment, cinematics etc
        // Eventually call ChangeState again with your next state

        ChangeState(GameState.InGame);
    }



    private void HandleInGame()
    {
        //looping with timer for continous firing



    }

    private void CalculateFireTime()
    {
        if (State == GameState.InGame)
        {
            _timer.UpdateTime(Time.deltaTime);

            if (_timer.finished)
            {
                OnFireTime?.Invoke();
                Debug.Log("Fire");
                _timer.Reset();
            }
        }
    }

    private void InitializeSpawnSlots()
    {
        for (int i = 0; i < 10; i++)
        {
            List<bool> row = new List<bool>();
            for (int j = 0; j < 5; j++)
            {
                row.Add(false);
            }
            heroSpawnSlots.Add(row);
        }

    }
}

/// <summary>
/// This is obviously an example and I have no idea what kind of game you're making.
/// You can use a similar manager for controlling your menu states or dynamic-cinematics, etc
/// </summary>
[Serializable]
public enum GameState
{
    Starting = 0,
    InGame = 1,
    Win = 2,
    Lose = 3,
    Pause = 4,
}