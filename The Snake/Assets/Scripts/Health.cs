using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int health;

    public bool hit;
    
    public void GetHit()
    {
        hit = true;
        health--;
        if (health == 0)
        {
            Die();    
        }
    }

    private void Die()
    {
        Handheld.Vibrate();
    }
}
