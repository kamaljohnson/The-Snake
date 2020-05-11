using UnityEngine;

public class Food : MonoBehaviour
{
    public static void Eat()
    {
        Handheld.Vibrate();
        Snake.Grow();
    }

    private void OnDestroy()
    {
        FoodSpawner.SpawnFood();
    }
}
