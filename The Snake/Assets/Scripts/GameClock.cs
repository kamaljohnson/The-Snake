using UnityEngine;

public abstract class GameClock: MonoBehaviour
{
    private float timer = 0f;
    public static float tickPeriod = 1f; 
    
    public void Update()
    {
        timer += Time.deltaTime;
     
        if (timer >= tickPeriod)
        {
            Tick();
            timer = 0;
        }
    }

    protected abstract void Tick();
}
