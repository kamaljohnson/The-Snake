using System;
using UnityEngine;
using UnityEngine.Serialization;

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

    public bool isTail;
    
    private void Start()
    {
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
        
        if (nextDirection != _currentDirection && !isTail)
        {
            FillFrontGap();
        }
        
        _currentDirection = nextDirection;
        
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
    }

    private Directions GetMovementDirection(Vector3 newDestination)
    {
        const float tolerance = 0.1f;
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

        transform.position = Vector3.MoveTowards(transform.position, _destination, stepSpeed * Time.deltaTime);
            
        if (Vector3.Distance(_destination, transform.position) <= 0.01f)
        {
            transform.position = _destination;
            _destinationIsReached = true;
        }
    }

    private void FillFrontGap()
    {
        tailGapFillerObj = Instantiate(Snake.snakeTailFillerObj, transform.position, Quaternion.identity);
        tailGapFillerActive = true;
    }
}

public enum Directions
{
    Right,
    Left,
    Forward,
    Back
}
