﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    public GameObject startButtonUi;
 
    public void OnStartButtonClick()
    {
        startButtonUi.SetActive(false);
        GameManager.StartGame();
    }
}