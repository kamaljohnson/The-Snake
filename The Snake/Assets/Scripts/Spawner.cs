using System;
using System.Collections;
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
        if(GameManager.gameState != GameState.Playing) return;
        
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
        var spawnPointer = GetRandomPointOnGround();

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
        
        obj.transform.position = new Vector3(spawnPointer.x, transform.position.y, spawnPointer.z);

        _spawnObjectCounter[(int)spawnObject]--;
    }

    public static Vector3 GetRandomPointOnGround()
    {
        
        var randX = Mathf.RoundToInt(Random.Range(0, _spawner.groundSize.x));
        var randZ = Mathf.RoundToInt(Random.Range(0, _spawner.groundSize.y));

        randX *= 2;
        randZ *= 2;
        
        var spawnPointer = _spawner.transform.position + new Vector3(randX, 5, randZ);
        return spawnPointer;
    }
    
    public static void Spawn(SpawnObject spawnObject, int count = 1, int delay = 0)
    {
        if (delay == 0)
        {
            _spawner._spawnObjectCounter[(int)spawnObject] += count;
        }
        else
        {
            _spawner.InitiateSpawnWithDelay(spawnObject, count, delay);
        }
    }

    private void InitiateSpawnWithDelay(SpawnObject spawnObject, int count, int delay)
    {
        StartCoroutine(SpawnWithDelay(spawnObject, count, delay));
    }
    
    private IEnumerator SpawnWithDelay(SpawnObject spawnObject, int count, float delay)
    {
        yield return new WaitForSeconds(delay);
        _spawner._spawnObjectCounter[(int) spawnObject]++;
        count--;
        if (count > 0)
        {
            StartCoroutine(SpawnWithDelay(spawnObject, count, delay));
        }
    }
    
}
