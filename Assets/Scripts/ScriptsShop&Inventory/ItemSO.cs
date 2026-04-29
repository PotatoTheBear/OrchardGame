using UnityEngine;

[CreateAssetMenu(fileName = "new item")]
public class ItemSO : ScriptableObject
{
    public string itemName;
    [TextArea]public string itemDescription;
    public Sprite icon;
}
