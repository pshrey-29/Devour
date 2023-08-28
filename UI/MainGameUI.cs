using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainGameUI : MonoBehaviour
{
    [SerializeField]
    private GameObject InGameUI, GamePauseUI, GameWinUI, GameLoseUI, GameEndUI;

    [SerializeField]
    private Button Pausebutton, Speedbutton;
    private int timeSpeed;


    private void Awake() => GameManager.OnBeforeStateChanged += OnStateChanged;

    private void OnDestroy() => GameManager.OnBeforeStateChanged -= OnStateChanged;


    // Start is called before the first frame update
    void Start()
    {
        timeSpeed = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            GameManager.Instance.ChangeState(GameState.Pause);
        }
    }

    private void OnStateChanged(GameState newState)
    {
        if (newState == GameState.Win)
        {
            if (LevelSystem.Level < 5)
            {
                PauseGameAndShowUI(GameWinUI);
            }
            else
            {
                PauseGameAndShowUI(GameEndUI);
            }
        }
        if (newState == GameState.Lose) PauseGameAndShowUI(GameLoseUI);
        if (newState == GameState.Pause) PauseGameAndShowUI(GamePauseUI);
        if (newState == GameState.InGame) BackToGame();
    }

    public void Pause()
    {
        GameManager.Instance.ChangeState(GameState.Pause);
    }

    // public void SpeedChange()
    // {
    //     switch (timeSpeed)
    //     {
    //         case 1:
    //             Time.timeScale = 2;
    //             timeSpeed = 2;
    //             break;

    //         case 2:
    //             Time.timeScale = 1;
    //             timeSpeed = 1;
    //             break;

    //         default:
    //             Time.timeScale = 1;
    //             timeSpeed = 1;
    //             break;
    //     }
    // }

    public void Resume()
    {
        GameManager.Instance.ChangeState(GameState.InGame);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("PreGame");
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        LevelSystem.Level = 0;
        SceneManager.LoadScene("MainMenu");
    }

    public void NextLevel()
    {
        Time.timeScale = 1;
        LevelSystem.Level++;
        SceneManager.LoadScene("PreGame");
    }

    private void BackToGame()
    {
        GamePauseUI.SetActive(false);
        Time.timeScale = 1;
        EnableMainGameUIButtons();
    }

    private void PauseGameAndShowUI(GameObject UI)
    {
        Time.timeScale = 0;
        UI.SetActive(true);
        DisableMainGameUIButtons();
    }

    private void DisableMainGameUIButtons()
    {
        Speedbutton.interactable = false;
        Pausebutton.interactable = false;
    }

    private void EnableMainGameUIButtons()
    {
        Speedbutton.interactable = true;
        Pausebutton.interactable = true;
    }
}
