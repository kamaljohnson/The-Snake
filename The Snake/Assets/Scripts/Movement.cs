using System;
using UnityEngine;

public enum MovementType
{
    Controlled,
    Follow
}

public class Movement : MonoBehaviour
{
    public static float stepSpeed;
    public static float stepSize;
    
    private Directions _currentDirection;
    public Directions nextDirection;

    private Vector3 _destination;
    private bool _destinationIsReached;

    public MovementType movementType;

    public Transform followTransform;

    public bool tailGapFillerActive;
    private GameObject tailGapFillerObj;

    public bool isEndTail;

    public bool createTailAtEnd;
    public bool justCreated;

    private Snake _snake;
    
    private void Start()
    {
        _destinationIsReached = false;
        _snake = FindObjectOfType<Snake>();

        _currentDirection = Directions.None;
        _destination = transform.position;
    }

    private void Update()
    {
        if (_destinationIsReached)
        {
            UpdateDirection();
        }
        
        Move();
    }
    
    private void UpdateDirection()
    {
        _destinationIsReached = false;
        switch (movementType)
        {
            case MovementType.Controlled:
                break;
            case MovementType.Follow:
                var newDestination = followTransform.position;
                nextDirection = GetMovementDirection(newDestination);
                break;
        }
        
        if (tailGapFillerActive)
        {
            tailGapFillerActive = false;
            Destroy(tailGapFillerObj);
        }
        
        if (nextDirection != _currentDirection && !isEndTail)
        {
            FillFrontGap();
        }

        if (CheckDirectionChange())
        {
            _currentDirection = nextDirection;
        }

        
        switch (_currentDirection)
        {
            case Directions.Right:
                _destination += Vector3.right * stepSize;
                break;
            case Directions.Left:
                _destination -= Vector3.right * stepSize;
                break;
            case Directions.Forward:
                _destination += Vector3.forward * stepSize;
                break;
            case Directions.Back:
                _destination -= Vector3.forward * stepSize;
                break;
        }
        
        if (createTailAtEnd)
        {
            CreateTailAtEnd();
        }

    }

    private bool CheckDirectionChange()
    {
        if (movementType == MovementType.Follow)
            return true;
        
        switch (_currentDirection)
        {
            case Directions.Right:
                return nextDirection != Directions.Left;
            case Directions.Left:
                return nextDirection != Directions.Right;
            case Directions.Forward:
                return nextDirection != Directions.Back;
            case Directions.Back:
                return nextDirection != Directions.Forward;
        }

        return true;
    }
    
    private Directions GetMovementDirection(Vector3 newDestination)
    {
        var tolerance = stepSize;
        var directionVector = newDestination - _destination;
        
        if (Vector3.Distance(directionVector, Vector3.right * stepSize) < tolerance)
        {
            return Directions.Right;
        }
        if (Vector3.Distance(directionVector, -Vector3.right * stepSize) < tolerance)
        {
            return Directions.Left;
        }
        if (Vector3.Distance(directionVector, Vector3.forward * stepSize) < tolerance)
        {
            return Directions.Forward;
        }
        
        return Directions.Back;
        
    }
    
    private void Move()
    {
        if (justCreated)
        {
            if (Vector3.Distance(followTransform.position, transform.position) < stepSize)
            {
                return;
            }

            justCreated = false;
            _destinationIsReached = true;
            return;
        }
        
        transform.position = Vector3.MoveTowards(transform.position, _destination, stepSpeed * Time.deltaTime);
            
        if (Vector3.Distance(_destination, transform.position) <= 0.01f)
        {
            transform.position = _destination;
            _destinationIsReached = true;
        }
    }

    private void FillFrontGap()
    {
        tailGapFillerObj = Instantiate(FindObjectOfType<Snake>().snakeTailFiller, transform.position, Quaternion.identity);
        tailGapFillerActive = true;
    }

    private void CreateTailAtEnd()
    {
        var endTailObj = Instantiate(_snake.snakeTailPrefab, _snake.transform);
        endTailObj.transform.position = transform.position;
        
        Snake.endTail.movement.isEndTail = false;
        
        var endTail = endTailObj.GetComponent<SnakeTail>();
        endTail.movement.SetFollowTransform(transform);
        endTail.movement.justCreated = true;
        endTail.movement.isEndTail = true;
        
        createTailAtEnd = false;
        Snake.endTail = endTail;

        gameObject.name = (Snake.size-1).ToString();
        endTailObj.name = "New End";
        Snake.size++;
    }

    private void SetFollowTransform(Transform followTransform)
    {
        this.followTransform = followTransform;
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
