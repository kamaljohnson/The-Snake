using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileBlast : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tail"))
        {
            Snake.DestroyTail(other.gameObject);
        }
    }
}
