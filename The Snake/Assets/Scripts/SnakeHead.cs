using System;
using UnityEngine;

public class SnakeHead : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Food"))
        {
            Food.Eat();
            Destroy(other.gameObject);
        } 
    }
    
}