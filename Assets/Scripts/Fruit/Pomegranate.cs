using UnityEngine;

public class Pomegranate : Fruit
{
    [SerializeField] protected float ammoValue;

    public override void Interact()
    {
        base.Interact();
        // Give ammo here
    }
}
