using System;
using UnityEngine;

public class SnakeHead : MonoBehaviour
{
    public Movement movement;
    
    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        HandleInput();
    }
    
    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            movement.nextDirection = Directions.Forward;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            movement.nextDirection = Directions.Back;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            movement.nextDirection = Directions.Right;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            movement.nextDirection = Directions.Left;
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