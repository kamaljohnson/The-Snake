﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

public class Movement : MonoBehaviour
{
    public static float stepSpeed;
    public static float stepSize;

    public PlayerInput input;

    public GameObject junctionFillerPrefab;

    private static bool _snapped = true;
    private static Directions _nextDirection = Directions.Forward;
    private static Directions _currentDirection = Directions.Forward;

    public static List<GameObject> junctionFillers = new List<GameObject>();

    public static Movement movement;

    private void Start()
    {
        movement = this;
    }

    private void Update()
    {
        if (!Health.IsAlive() || GameManager.gameState != GameState.Playing) return;
        
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
        var snapped = false;

        if (SnakeFrontCollider.needsDeviation)
        {
            foreach (var tail in Snake.snakeBody)
            {
                var tailLocation = tail.transform.localPosition;
                var tailDestination = tail.destination;
                var directionVector = Vector3.Normalize(tailDestination - tailLocation);
                var loc = tailDestination - directionVector * stepSize;
                var x = Mathf.RoundToInt(loc.x);
                var y = Mathf.RoundToInt(loc.y);
                var z = Mathf.RoundToInt(loc.z);
                tail.transform.localPosition = new Vector3(x, y, z);
            }
            _nextDirection = GetDeviatedDirection();
            _snapped = true;
            return;
        }

        for (var i = 0; i < Snake.snakeBody.Count; i++)
        {
            var tail = Snake.snakeBody[i];
            var tailTransform = tail.transform;
            var tailDestination = tail.destination;
            tailTransform.localPosition = Vector3.MoveTowards(tailTransform.localPosition,
                tailDestination, stepSpeed * Time.deltaTime);

            if (i >= Snake.size - 1 && Snake.size != 1) continue;
            if (Vector3.Distance(tailTransform.localPosition, tailDestination) < tolerance)
            {
                Debug.Log("here");
                snapped = true;
            }
        }

        if (snapped)
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
            
            _snapped = true;
        }
    }

    private Directions GetDeviatedDirection()
    {
        SnakeFrontCollider.needsDeviation = false;
        RaycastHit hit;
        
        foreach (var tail in Snake.snakeBody)
        {
            tail.destination = tail.transform.localPosition;
        }

        var headLocation = Snake.snakeBody[0].transform.position;
        
        if (_currentDirection == Directions.Right || _currentDirection == Directions.Left)
        {
            if (Physics.Raycast(headLocation, Vector3.forward, out hit, stepSize))
            {
                if (!hit.transform.CompareTag("Wall"))
                {
                    return Directions.Forward;
                }
            }
            else
            {
                return Directions.Forward;
            }

            if (Physics.Raycast(headLocation, Vector3.back, out hit, stepSize))
            {
                if (!hit.transform.CompareTag("Wall"))
                {
                    return Directions.Back;
                }
            }
            else
            {
                return Directions.Back;
            }
        }
        
        if (Physics.Raycast(headLocation, Vector3.right, out hit, stepSize))
        {
            if (!hit.transform.CompareTag("Wall"))
            {
                return Directions.Right;
            }
        }
        else
        {
            return Directions.Right;
        }

        if (Physics.Raycast(headLocation, Vector3.left, out hit, stepSize))
        {
            if (!hit.transform.CompareTag("Wall"))
            {
                return Directions.Left;
            }
        }
        else
        {
            return Directions.Left;
        }

        return Directions.None;
    }
    
    private void UpdateDestinations()
    {
        _currentDirection = _nextDirection;
        var head = Snake.snakeBody[0];

        for (var i = Snake.size - 1; i > 0; i--)
        {
            var nextTail = Snake.snakeBody[i - 1];
            Snake.snakeBody[i].destination = nextTail.destination;
        }

        head.destination = GetHeadDestination(head.destination);

        Snake.snake.frontCollider.transform.localPosition = head.destination;
        
        CheckDirectionChange();
        
        _snapped = false;
        if (Snake.grow)
        {
            Grow();
            Snake.grow = false;
        }
    }

    private void Grow()
    {
        var endTailObj = Instantiate(Snake.tailPrefab, Snake.transform);
        endTailObj.transform.localPosition = Snake.snakeBody[Snake.size - 1].transform.localPosition;
        var tail = endTailObj.GetComponent<SnakeTail>();

        tail.transform = endTailObj.transform;
        tail.destination = tail.transform.localPosition;
        Snake.snakeBody.Add(tail);
        Snake.size++;
        Snake.snakeBody[Snake.size - 2].gameObject.name = (Snake.size - 2).ToString();
        endTailObj.name = "End";
    }
    
    private void CheckDirectionChange()
    {
        const float tolerance = 0.1f;

        if (Snake.size <= 1)
        {
            return;
        }
        
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
            switch (_currentDirection)
            {
                case Directions.Right:
                    if(nextDirection == Directions.Left)
                        return;
                    break;
                case Directions.Left:
                    if(nextDirection == Directions.Right)
                        return;
                    break;
                case Directions.Forward:
                    if(nextDirection == Directions.Back)
                        return;
                    break;
                case Directions.Back:
                    if(nextDirection == Directions.Forward)
                        return;
                    break;
            }
            
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

    public static void RecalculateFillers()
    {
        foreach (var filler in junctionFillers)
        {
            Destroy(filler);       
        }
        junctionFillers = new List<GameObject>();

        for (var i = 0; i < Snake.snakeBody.Count - 2; i++)
        {
            var destination0 = Snake.snakeBody[i].destination;
            var destination1 = Snake.snakeBody[i + 1].destination;
            var destination2 = Snake.snakeBody[i + 2].destination;

            var dir0 = Vector3.Normalize(destination0 - destination1);
            var dir1 = Vector3.Normalize(destination1 - destination2);

            const float tolerance = 0.1f;
            
            if (Vector3.Distance(dir0, dir1) > tolerance)
            {
                Debug.Log("filler created");
                var filler = Instantiate(movement.junctionFillerPrefab, Snake.transform);
                filler.transform.localPosition = destination1;
                junctionFillers.Add(filler);
            }
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
