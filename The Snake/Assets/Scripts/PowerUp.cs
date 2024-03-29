﻿using System;
using System.Collections.Generic;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Serialization;

public enum PowerUpType
{
    PopulationGrowth,
    ExtraCannon,
    SpeedIncrease
}

public class PowerUp : MonoBehaviour
{

    public PowerUpType type;
    public GameObject adIcon;
    public TMP_Text powerUpCostText;

    public ulong powerUpCost;

    public float powerUpMultiplier;

    private bool _adSupport;
    private int adShowCount = 3;    
    private int _adCounter;

    public static bool adAvailable;

    public static PowerUp currentRewardedVideoRequestedPowerUp;
    
    private void Start()
    {
        LoadPriceOfPowerUpFromPref();
        UpdateUi();
    }

    public void RequestPowerUpActivation()
    {        
        LoadPriceOfPowerUpFromPref();
        bool purchaseStatus;
        
        if (_adSupport)
        {
            adAvailable = false;
            currentRewardedVideoRequestedPowerUp = this;
            RewardedAdsManager.ShowRewardedVideo();
            return;
        }

        purchaseStatus = CashManager.MakePurchase(powerUpCost);
        
        if (purchaseStatus)
        {
            ActivatePowerUp();
        }
    }

    public void ActivatePowerUp()
    {
        switch (type)
        {
            case PowerUpType.PopulationGrowth:
                NotificationManager.Notify(NotificationType.PopulationPowerUp);
                break;
            case PowerUpType.ExtraCannon:
                NotificationManager.Notify(NotificationType.ExtraCannonPowerUp);
                break;
            case PowerUpType.SpeedIncrease:
                NotificationManager.Notify(NotificationType.SpeedPowerUp);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        UpdatePowerUpCost();
    }

    private void LoadPriceOfPowerUpFromPref()
    {
        if (PlayerPrefs.HasKey("PowerUp" + GetName(type)))
        {
            powerUpCost = Convert.ToUInt64(PlayerPrefs.GetString("PowerUp" + GetName(type)));
        }
        else
        {
            SetPowerUpCostToPref();
        }    
    }

    private void SetPowerUpCostToPref()
    {
        PlayerPrefs.SetString("PowerUp" + GetName(type), "" + powerUpCost);
    }

    private void UpdatePowerUpCost()
    {
        powerUpCost = (ulong) (powerUpCost * powerUpMultiplier);
        SetPowerUpCostToPref();

        if (CashManager.GetCash() < powerUpCost && !_adSupport)
            _adSupport = true;
        else
        {
            _adSupport = false;
        }
        
        _adCounter++;
        if (_adCounter >= adShowCount || _adSupport)
        {
            _adSupport = true;
            _adCounter = 0;
        }
        
        UpdateUi();
    }

    private void UpdateUi()
    {
        powerUpCostText.text = CashManager.GetCashDisplay(powerUpCost);
        
        if (_adSupport && adAvailable)
        {
            adIcon.gameObject.SetActive(true);
            powerUpCostText.gameObject.SetActive(false);
        }
        else
        {
            _adSupport = false;
            powerUpCostText.gameObject.SetActive(true);
            adIcon.gameObject.SetActive(false);
        }
    }

    public static void UpdateFreePowerUpStatus(bool rewardedVideoWatched)
    {
        if (rewardedVideoWatched)
        {
            currentRewardedVideoRequestedPowerUp.ActivatePowerUp();
            currentRewardedVideoRequestedPowerUp = null;
        }
    }
    
    private static string GetName(PowerUpType type)
    {
        switch (type)
        {
            case PowerUpType.PopulationGrowth:
                return "PopulationGrowth";
            case PowerUpType.ExtraCannon:
                return "ExtraCannon";
            case PowerUpType.SpeedIncrease:
                return "SpeedIncrease";
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public static void Reset()
    {
        foreach (var powerUp in FindObjectsOfType<PowerUp>())
        {
            powerUp._adSupport = false;
            powerUp.UpdateUi();
        }
    }
}

