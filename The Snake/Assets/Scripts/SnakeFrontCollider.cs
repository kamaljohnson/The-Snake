using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeFrontCollider : MonoBehaviour
{
    public static bool needsDeviation;
    
    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Wall":
                needsDeviation = true;
                Wall.Hit();
                Health.Hit();
                break;
            case "Food":
                Food.Eat(other.gameObject);
                break;
            case "Tail":
                Health.Hit();
                if (Health.health.healthLevel > 0)
                {
                    Snake.DestroyTail(other.gameObject);
                }
                break;
        }
    }
}
