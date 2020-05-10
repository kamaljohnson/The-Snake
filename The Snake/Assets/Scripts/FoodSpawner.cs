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
        
        var randX = Mathf.RoundToInt(Random.Range(0, _spawner.groundSize.x));
        var randZ = Mathf.RoundToInt(Random.Range(0, _spawner.groundSize.y));

        randX *= 2;
        randZ *= 2;
        
        _spawnPointer = transform.position + new Vector3(randX, 5, randZ);

        if (!Physics.Raycast(_spawnPointer, Vector3.down, out var hit, 10f)) return;
        if (!hit.collider.CompareTag("Ground")) return;
        
        var foodObj = Instantiate(_spawner.food, transform);
        Debug.Log(randX + " " + randZ);
        foodObj.transform.localPosition = new Vector3(randX, 0, randZ);
        
        spawnFood = false;
    }
    
    public static void SpawnFood()
    {
        _spawner.spawnFood = true;
    }

}
