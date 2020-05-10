using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Snake : MonoBehaviour
{
    public float stepSize;
    public float stepSpeed;

    public GameObject snakeTailPrefab;
    
    public GameObject snakeTailFiller;
    
    public static SnakeTail endTail;

    public bool grow;

    public int initialSize;
    public static int size;

    private float tempTimer = 1;
    
    public void Start()
    {
        size = initialSize;
        
        Movement.stepSize = stepSize;
        Movement.stepSpeed = stepSpeed;
    }

    private void Update()
    {

        tempTimer -= Time.deltaTime;
        if (tempTimer <= 0)
        {
            grow = true;
            tempTimer = 1;
        }
        
        if (grow)
        {
            Grow();
            grow = false;
        }
    }

    public static void Grow()
    {
        endTail.movement.createTailAtEnd = true;
    }
}
