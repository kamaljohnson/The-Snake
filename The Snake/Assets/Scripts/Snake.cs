using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Snake : MonoBehaviour
{
    public float stepSize;
    public float stepSpeed;

    
    public GameObject snakeTailFiller;
    public static GameObject snakeTailFillerObj;
    
    public SnakeTail endTail;
    
    public void Start()
    {
        endTail.movement.isTail = true;
        snakeTailFillerObj = snakeTailFiller;
        
        Movement.stepSize = stepSize;
        Movement.stepSpeed = stepSpeed;
    }

    public static void Grow()
    {
        
    }
}
