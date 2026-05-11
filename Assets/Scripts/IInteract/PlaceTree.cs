using UnityEngine;

public class PlaceTree : IInteract
{
    public void Interact()
    {
        // Place Tree
        PlayerBuildTree.Instance.Place();
    }
}
