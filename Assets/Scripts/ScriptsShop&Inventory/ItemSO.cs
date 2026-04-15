using UnityEngine;

[CreateAssetMenu(fileName = "new item")]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public string itemDescription;
    public Sprite icon;
}
