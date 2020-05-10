﻿using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public TMP_Text levelText;

    private static int currentLevel = 1;
    
    private void Start()
    {
        GetLevelFromPref();
        
        UpdateUi();
    }

    public static int GetCurrentLevel()
    {
        return currentLevel;
    }
    
    public void IncrementLevel()
    {
        currentLevel ++;
        SetLevelToPref();
        
        GameProgressManager.UpdateProgressPerCash();
        UpdateUi();
    }

    public static void GetLevelFromPref()
    {
        if (PlayerPrefs.HasKey("CurrentLevel"))
        {
            currentLevel = PlayerPrefs.GetInt("CurrentLevel");
        }
        else
        {
            SetLevelToPref();
        }
    }

    public static void SetLevelToPref()
    {
        PlayerPrefs.SetInt("CurrentLevel", currentLevel);
    }

    private void UpdateUi()
    {
        levelText.text = "Level " + currentLevel;
    }
}