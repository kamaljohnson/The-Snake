using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int healthLevel;

    public static Health health;

    private void Start()
    {
        health = this;
    }

    public static void Hit()
    {
        Handheld.Vibrate();
        health.healthLevel--;
        if (health.healthLevel <= 0)
        {
           health.Die(); 
        }
    }

    private void Die()
    {
        Debug.Log("snake died");        
    }

    public static bool IsAlive()
    {
        return health.healthLevel > 0;
    }
}
