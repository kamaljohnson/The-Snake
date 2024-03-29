﻿using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Serialization;

public enum GameState
{
    AtMenu,
    Playing,
    GameOver,
    GameWon
}

public class GameManager : MonoBehaviour
{

    public static GameState gameState;

    public static bool CanPlay;
    
    private static GameManager _gameManager;
    
    public GameObject bottomPowerUpUi;

    public GameObject transitionUi;
    public GameObject gameWonUi;
    public GameObject gameBoard;
    
    private static int _adCounter;
    
    private static bool _notifiedAllMonstersEffected;
    
    public void Start()
    {
        _gameManager = this;
        Screen.orientation = ScreenOrientation.Portrait;
        _adCounter = 0;
        gameState = GameState.AtMenu;
        GoToMenu();
    }

    public void GoToMenu()
    {
        StartCoroutine(TriggerTransitionAnimation());
        _gameManager.ShowMenu();

        Reset();

        _gameManager.gameWonUi.SetActive(false);
        
        gameState = GameState.AtMenu;
        StartCoroutine(TriggerCanPlay());
    }
    
    public static void StartGame()
    {
        gameState = GameState.Playing;
        _gameManager.HideMenu();
    }

    public static void GameOver()
    {
        CanPlay = false;
        ShowAdIfCounter();
        // UnityVideoAds.ShowAd();
        _gameManager.DelayedGoToMenu();
    }

    public void DelayedGoToMenu()
    {
        StartCoroutine(TriggerGoToMenu());
    }
    
    public static void GameWon()
    {        
        ShowAdIfCounter();
        gameState = GameState.GameWon;
        _gameManager.gameWonUi.SetActive(true);
        CanPlay = false;
    }

    private static void ShowAdIfCounter()
    {
        _adCounter++;
        if (_adCounter < 4) return;
        _adCounter = 0;
        Debug.Log("request show ad");
        UnityVideoAds.ShowAd();
    }
    
    public static void ReportPersonDead()
    {
        _gameManager.CheckGameEnding();
    }

    public static void ReportCannonBallUsed()
    {
        _gameManager.CheckGameEnding();
    }

    private void CheckGameEnding()
    {
        if(gameState != GameState.Playing) return;
        
        if (GameProgressManager.GetCurrentProgressState() == GameProgressState.Complete)
        {
            GameWon();
        }
        else
        {
            GameOver();
        }

    }

    private void ShowMenu()
    {
        _gameManager.gameBoard.GetComponent<Animator>().Play("GameBoardAnimateUp", -1, 0f);
        bottomPowerUpUi.GetComponent<Animator>().Play("BottomPowerUpAnimateIn", -1, 0);
    }

    private void HideMenu()
    {
        _gameManager.gameBoard.GetComponent<Animator>().Play("GameBoardAnimateDown", -1, 0f);
        bottomPowerUpUi.GetComponent<Animator>().Play("BottomPowerUpAnimateOut", -1, 0);
    }

    IEnumerator TriggerGoToMenu()
    {
        yield return new WaitForSeconds(1f);
        GoToMenu();        
    }
    
    IEnumerator TriggerCanPlay()
    {
        yield return new WaitForSeconds(1f);
        CanPlay = true;
    }
    
    IEnumerator TriggerTransitionAnimation()
    {
        transitionUi.SetActive(true);
        yield return new WaitForSeconds(1f);
        transitionUi.SetActive(false);
    }
    
    private void Reset()
    {
        GameProgressManager.Reset();
        PowerUp.Reset();

        _notifiedAllMonstersEffected = false;
    }
}
