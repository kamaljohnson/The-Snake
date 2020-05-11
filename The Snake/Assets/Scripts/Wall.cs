using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public Animator animator;

    public static Wall wall;

    private void Start()
    {
        wall = this;
    }

    public static void Hit()
    {
        wall.animator.Play("WallHitAnimation", -1, 0f);
    }
}
