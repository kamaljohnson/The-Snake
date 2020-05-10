using System;
using UnityEngine;

public class Food : MonoBehaviour
{
    public static void Eat()
    {
        Snake.Grow();
    }

    private void OnDestroy()
    {
        FoodSpawner.SpawnFood();
    }
}
