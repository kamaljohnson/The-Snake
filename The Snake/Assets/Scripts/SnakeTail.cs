using System;
using UnityEngine;

public class SnakeTail : MonoBehaviour
{

    public Movement movement;

    public void SetFollowTransform(Transform followTransform)
    {
        movement.followTransform = followTransform;
    }
    
}
