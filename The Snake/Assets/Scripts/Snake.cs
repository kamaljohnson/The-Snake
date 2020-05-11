using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class Snake : MonoBehaviour
{
    public float stepSize;
    public float stepSpeed;

    public GameObject snakeTailPrefab;
    
    public static SnakeTail endTail;

    public int initialSize;
    public static int size;

    public List<SnakeTail> initialSnakeBody;
    
    public static List<SnakeTail> snakeBody = new List<SnakeTail>();

    public new static Transform transform;
    
    public void Start()
    {
        transform = GetComponent<Transform>();
        
        snakeBody = initialSnakeBody;
        
        size = initialSize;
        
        Movement.stepSize = stepSize;
        Movement.stepSpeed = stepSpeed;

    }
    
    public static void Grow()
    {
        
    }
}
