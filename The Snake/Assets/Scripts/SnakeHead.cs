using System;
using UnityEngine;

public class SnakeHead : MonoBehaviour
{
    public Movement movement;

    public PlayerInput input;

    public Health health;
    
    private void Update()
    {
        HandleMovement();

        if (health.health == 0)
        {
            Movement.canMove = false;
        }

        if (health.hit)
        {
            movement.Deviate();
            health.hit = false;
        }
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
        else if (other.CompareTag("Wall"))
        {
            health.GetHit();
        }
    }
    
}