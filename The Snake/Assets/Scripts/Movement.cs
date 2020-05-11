using System;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Movement : MonoBehaviour
{
    public static float stepSpeed;
    public static float stepSize;

    public PlayerInput input;

    private static bool _snapped = true;
    private static Directions _nextDirection = Directions.Forward;

    private void Update()
    {
        HandleInput();
        
        if (!_snapped)
        {
            Move();
        }
        else
        {
            UpdateMovement();
        }
    }

    private void Move()
    {
        const float tolarance = 0.1f;
        var _snapped = false;

        foreach (var tail in Snake.snakeBody)
        {
            var tailTransform = tail.transform;
            var tailDestination = tail.destination; 
            tailTransform.localPosition = Vector3.MoveTowards(tailTransform.localPosition,
                tailDestination, stepSpeed * Time.deltaTime);

            if (Vector3.Distance(tailTransform.localPosition, tailDestination) < tolarance)
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

            Movement._snapped = true;
        }
    }
    
    private void UpdateMovement()
    {
        var head = Snake.snakeBody[0];

        for (var i = Snake.size - 1; i > 0; i--)
        {
            var nextTail = Snake.snakeBody[i - 1];
            Snake.snakeBody[i].destination = nextTail.destination;
        }

        head.destination = GetHeadDestination(head.destination);
        _snapped = false;
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
