using UnityEngine;

public class SpikeManager : MonoBehaviour
{
    public int initialCount;

    public void Start()
    {
        Spawner.Spawn(SpawnObject.Spike, initialCount);
    }
}
