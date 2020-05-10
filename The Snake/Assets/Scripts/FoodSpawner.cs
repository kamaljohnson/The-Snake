using UnityEngine;
using System.Collections;

public class FoodSpawner : MonoBehaviour
{

    public GameObject food;
    private static FoodSpawner _spawner;
    
    private static Vector3 _spawnPointer;

    public Vector2 groundSize;

    private bool spawnFood;
    
    public void Start()
    {
        _spawner = this;
        
        SpawnFood();
    }

    public void Update()
    {
        if (!spawnFood) return;
        
        var randX = Random.Range(_spawner.groundSize.x, -_spawner.groundSize.x);
        var randZ = Random.Range(_spawner.groundSize.y, -_spawner.groundSize.y);

        _spawnPointer = transform.position + new Vector3(randX, 5, randZ);

        if (!Physics.Raycast(transform.position + _spawnPointer, Vector3.down, out var hit, 10f)) return;
        if (!hit.collider.CompareTag("Ground")) return;
        
        var foodObj = Instantiate(_spawner.food, transform);
        foodObj.transform.localPosition = new Vector3(randX + 1f, 0, randZ + 1f);
        
        spawnFood = false;
    }
    
    public static void SpawnFood()
    {
        _spawner.spawnFood = true;
    }

}
