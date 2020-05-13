using System.Collections;
using UnityEngine;

public class SpikeManager : MonoBehaviour
{
    public int initialCount;

    public int spawnDelay;
    
    public void Start()
    {
        Spawner.Spawn(SpawnObject.Spike, initialCount, spawnDelay);
    }
}
