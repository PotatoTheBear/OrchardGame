using UnityEngine;
using TMPro;

public class ShopInfo : MonoBehaviour
{
    public TMP_Text itemDescriptionText;

    public void ShowItemInfo(ItemSO itemSO)
    {
        itemDescriptionText.text = itemSO.itemDescription;
    }
}
