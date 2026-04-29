using System;
using UnityEngine;

public class Enemy : Creature
{
    public static Enemy Instance { get; private set; }

    private IMovement movement;

    private void Start()
    {
        Instance = this;

        movement = new EntityMovement();
    }

    void Update()
    {
        movement.Move(this);
        Death();
    }
}
