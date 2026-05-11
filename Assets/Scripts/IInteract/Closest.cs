using UnityEngine;

public class Closest : IInteract
{
    public void Interact()
    {
        var closestFruit = Interactable.GetClosest(Player.Instance.transform.position, Player.Instance.minInteractDistance);
        if (closestFruit != null)
            closestFruit.Interact();
    }
}
