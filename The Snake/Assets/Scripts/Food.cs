using System.Runtime.CompilerServices;
using UnityEngine;

public class Food : MonoBehaviour
{
    public static void Eat(GameObject food)
    {
        Snake.Grow();
        
        Destroy(food);
    }

    private void OnDestroy()
    {
        FoodSpawner.SpawnFood();
    }
}
