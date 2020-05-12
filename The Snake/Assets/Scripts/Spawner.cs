using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum SpawnObject
{
    Food,
    Spike
}

public class Spawner : MonoBehaviour
{

    public GameObject food;
    public GameObject spike;
    private static Spawner _spawner;
    
    private readonly List<int> _spawnObjectCounter = new List<int>
    {
        0,        // Food initial count 
        0        // Spike initial count 
    };
    
    public Vector2 groundSize;

    public void Awake()
    {
        _spawner = this;
    }

    public void Update()
    {
        for (var i = 0; i < _spawnObjectCounter.Count; i++)
        {
            if (_spawnObjectCounter[i] > 0)
            {
                Spawn((SpawnObject) i);
            }
        }
    }

    private void Spawn(SpawnObject spawnObject)
    {
        var randX = Mathf.RoundToInt(Random.Range(0, _spawner.groundSize.x));
        var randZ = Mathf.RoundToInt(Random.Range(0, _spawner.groundSize.y));

        randX *= 2;
        randZ *= 2;
        
        var spawnPointer = transform.position + new Vector3(randX, 5, randZ);

        if (!Physics.Raycast(spawnPointer, Vector3.down, out var hit, 10f)) return;
        if (!hit.collider.CompareTag("Ground")) return;

        GameObject obj; 
        switch(spawnObject)
        {
            case SpawnObject.Food:
                obj = Instantiate(_spawner.food, transform);
                break;
            case SpawnObject.Spike:
                obj = Instantiate(_spawner.spike, transform);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(spawnObject), spawnObject, null);
        }
        
        obj.transform.localPosition = new Vector3(randX, 0, randZ);

        _spawnObjectCounter[(int)spawnObject]--;
    }
    
    public static void Spawn(SpawnObject spawnObject, int count = 1)
    {
        _spawner._spawnObjectCounter[(int)spawnObject] += count;
    }

}
