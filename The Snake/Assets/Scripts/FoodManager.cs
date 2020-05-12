using System;
using UnityEngine;
using UnityEngine.Serialization;

public class FoodManager : MonoBehaviour
{
    public int initialCount;

    public void Start()
    {
        Spawner.Spawn(SpawnObject.Food, initialCount);
    }
}
