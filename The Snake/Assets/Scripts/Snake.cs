using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Vector3 = UnityEngine.Vector3;

public class Snake : MonoBehaviour
{
    public float stepSize;
    public float stepSpeed;

    public GameObject snakeTailPrefab;
    public static GameObject tailPrefab; 
    
    public static SnakeTail endTail;

    public int initialSize;
    public static int size;

    public List<SnakeTail> initialSnakeBody;
    
    public static List<SnakeTail> snakeBody = new List<SnakeTail>();

    public new static Transform transform;

    public static bool grow;

    public Transform frontCollider;

    public static Snake snake;
    
    public void Start()
    {
        snake = this;
        
        tailPrefab = snakeTailPrefab;
        transform = GetComponent<Transform>();
        
        snakeBody = initialSnakeBody;
        
        size = initialSize;
        
        Movement.stepSize = stepSize;
        Movement.stepSpeed = stepSpeed;

    }

    public static void DestroyTail(GameObject tailObj)
    {
        var flag = false;
        var firstFillerLocationToDelete = new Vector3();
        var tempList = new List<SnakeTail>();
        foreach (var tail in snakeBody)
        {
            if (tailObj.name == tail.name)
            {
                flag = true;
                firstFillerLocationToDelete = tail.destination;
            }

            if (flag)
            {
                tempList.Add(tail);
            }
        }

        const float tolerance = 0.1f;
        var fillersToBeDestroyed = new List<GameObject>();

        foreach (var tail in tempList)
        {
            flag = false;
            for (var i = Movement.junctionFillers.Count - 1; i >= 0 ; i--)
            {
                var filler = Movement.junctionFillers[i];
                if (flag)
                {
                    fillersToBeDestroyed.Add(filler);
                    continue;
                }

                if (Vector3.Distance(filler.transform.localPosition, tail.transform.localPosition) <= tolerance)
                {
                    flag = true;
                    fillersToBeDestroyed.Add(filler);
                }
            }

            if (flag)
            {
                break;
            }
        }        
        
        foreach (var tail in tempList)
        {
            snakeBody.Remove(tail);
            Destroy(tail.gameObject);
            size--;
        }

        foreach (var filler in fillersToBeDestroyed)
        {
            Movement.junctionFillers.Remove(filler);
            Destroy(filler);
        }
        // destroying fillers
        
        fillersToBeDestroyed = new List<GameObject>();
        flag = false;

        for (var i = Movement.junctionFillers.Count - 1; i >= 0 ; i--)
        {
            var filler = Movement.junctionFillers[i];
            if (flag)
            {
                fillersToBeDestroyed.Add(filler);
                continue;
            }

            if (Vector3.Distance(filler.transform.localPosition, firstFillerLocationToDelete) <= tolerance)
            {
                flag = true;
                fillersToBeDestroyed.Add(filler);
            }
        }

        // distroying the edge case fillers
        foreach (var filler in fillersToBeDestroyed)
        {
            Movement.junctionFillers.Remove(filler);
            Destroy(filler);
        }

    }
    
    public static void Grow()
    {
        grow = true;
    }
}
