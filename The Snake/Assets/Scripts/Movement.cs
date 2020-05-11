﻿using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Serialization;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class Movement : MonoBehaviour
{
    public static float stepSpeed;
    public static float stepSize;

    public PlayerInput input;

    public GameObject junctionFillerPrefab;

    private static bool _snapped = true;
    private static Directions _nextDirection = Directions.Forward;

    public List<GameObject> junctionFillers = new List<GameObject>(); 
    
    private void Update()
    {
        HandleInput();
        
        if (!_snapped)
        {
            Move();
        }
        else
        {
            UpdateDestinations();
        }
    }

    private void Move()
    {
        const float tolerance = 0.1f;
        var _snapped = false;

        foreach (var tail in Snake.snakeBody)
        {
            var tailTransform = tail.transform;
            var tailDestination = tail.destination; 
            tailTransform.localPosition = Vector3.MoveTowards(tailTransform.localPosition,
                tailDestination, stepSpeed * Time.deltaTime);

            if (Vector3.Distance(tailTransform.localPosition, tailDestination) < tolerance)
            {
                _snapped = true;
            }
        }

        if (_snapped)
        {
            foreach (var tail in Snake.snakeBody)
            {
                var tailTransform = tail.transform;
                var tailDestination = tail.destination;
                tailTransform.localPosition = tailDestination;
            }

            foreach (var filler in junctionFillers)
            {
                if (filler.transform.localPosition != Snake.snakeBody[Snake.size - 1].transform.localPosition) continue;
                
                var fillerObj = filler;
                junctionFillers.Remove(fillerObj);
                Destroy(filler);
                break;
            }
            
            Movement._snapped = true;
        }
    }
    
    private void UpdateDestinations()
    {
        var head = Snake.snakeBody[0];

        for (var i = Snake.size - 1; i > 0; i--)
        {
            var nextTail = Snake.snakeBody[i - 1];
            Snake.snakeBody[i].destination = nextTail.destination;
        }

        head.destination = GetHeadDestination(head.destination);

        CheckDirectionChange();
        
        _snapped = false;
    }

    private void CheckDirectionChange()
    {
        const float tolerance = 0.1f;

        var head = Snake.snakeBody[0];
        var tail1 = Snake.snakeBody[1];

        var headDirectionVector = head.destination - head.transform.localPosition;
        var tail1DirectionVector = tail1.destination - tail1.transform.localPosition;

        if (Vector3.Distance(headDirectionVector, tail1DirectionVector) > tolerance)
        {
            var junctionFillerObj = Instantiate(junctionFillerPrefab, Snake.transform);
            junctionFillerObj.transform.localPosition = head.transform.localPosition;
            junctionFillerObj.name = "JF";
            junctionFillers.Add(junctionFillerObj);
        }
    }
    
    private void HandleInput()
    {
        var nextDirection = input.HandleInput();
        if (nextDirection != Directions.None)
        {
            _nextDirection = nextDirection;
        }
    }


    private static Vector3 GetHeadDestination(Vector3 currentPosition)
    {
        return currentPosition + DirectionVector(_nextDirection) * stepSize;
    }

    private static Vector3 DirectionVector(Directions direction)
    {
        switch (direction)
        {
            case Directions.Right:
                return Vector3.right;
            case Directions.Left:
                return Vector3.left;
            case Directions.Forward:
                return Vector3.forward;
            case Directions.Back:
                return Vector3.back;
            case Directions.None:
                return Vector3.zero;
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }
    }
}

public enum Directions
{
    Right,
    Left,
    Forward,
    Back,
    None
}
