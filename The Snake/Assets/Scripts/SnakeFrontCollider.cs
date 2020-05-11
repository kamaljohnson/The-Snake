using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeFrontCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Wall":
                break;
            case "Food":
                Food.Eat(other.gameObject);
                break;
            case "Tail":
                break;
        }
    }
}
