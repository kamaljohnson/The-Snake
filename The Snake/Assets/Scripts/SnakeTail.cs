using System;
using UnityEngine;

public class SnakeTail : MonoBehaviour
{

    public Vector3 destination;

    public new Transform transform;

    private void Start()
    {
        destination = GetComponent<Transform>().localPosition;
        
        transform = GetComponent<Transform>();
    }
}
