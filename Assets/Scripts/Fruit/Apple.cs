using UnityEngine;

public class Apple : Fruit
{
    [SerializeField] protected float healAmount;

    public override void Interact()
    {
        // Heal
        Debug.Log("Old HP: " + Player.Instance.GetHP());
        Player.Instance.Heal(healAmount);
        Debug.Log("New HP: " + Player.Instance.GetHP()); 
        base.Interact();
    }
}
