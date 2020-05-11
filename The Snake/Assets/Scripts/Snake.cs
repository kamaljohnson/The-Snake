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

    public int initialSize;
    public static int size;

    public void Start()
    {
        size = initialSize;
        
        Movement.stepSize = stepSize;
        Movement.stepSpeed = stepSpeed;
    }

    public static void Grow()
    {
    }
}
