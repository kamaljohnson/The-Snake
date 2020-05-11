using System;
using UnityEngine;

public class SnakeHead : MonoBehaviour
{
    public Movement movement;

    public PlayerInput input;
    
    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        var nextDirection = input.HandleInput();
        if (nextDirection != Directions.None)
        {
            movement.nextDirection = nextDirection;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Food"))
        {
            Food.Eat();
            Destroy(other.gameObject);
        }
    }
    
}