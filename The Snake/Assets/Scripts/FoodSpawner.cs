using UnityEngine;
using System.Collections;

public class FoodSpawner : MonoBehaviour
{

    public GameObject food;
    private static FoodSpawner _spawner;
    
    private static Vector3 _spawnPointer;

    public int initialFoodCount;
    private int _foodSpawnCount;
    
    public Vector2 groundSize;

    public void Start()
    {
        _spawner = this;
        SpawnFood(initialFoodCount);
    }

    public void Update()
    {
        if (_foodSpawnCount == 0) return;
        
        var randX = Mathf.RoundToInt(Random.Range(0, _spawner.groundSize.x));
        var randZ = Mathf.RoundToInt(Random.Range(0, _spawner.groundSize.y));

        randX *= 2;
        randZ *= 2;
        
        _spawnPointer = transform.position + new Vector3(randX, 5, randZ);

        if (!Physics.Raycast(_spawnPointer, Vector3.down, out var hit, 10f)) return;
        if (!hit.collider.CompareTag("Ground")) return;
        
        var foodObj = Instantiate(_spawner.food, transform);
        foodObj.transform.localPosition = new Vector3(randX, 0, randZ);

        _foodSpawnCount--;
    }

    public static void SpawnFood(int count = 1)
    {
        _spawner._foodSpawnCount += count;
    }

}
