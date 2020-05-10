using System;
using UnityEngine;

public class SnakeTail : MonoBehaviour
{

    public Movement movement;

    public void Start()
    {
        movement.movementType = MovementType.Follow;
        if (movement.isEndTail)
        {
            Snake.endTail = this;
        }
    }
}
