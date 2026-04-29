using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ShopSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ItemSO itemSO;
    public TMP_Text itemNameText;
    public TMP_Text priceText;
    public Image itemImage;
    

    [SerializeField] private ShopManager shopManager;

    private int price;
    public void Initialize(ItemSO newItemSO, int price)
    {
        // vul de slot met informatie //
        itemSO = newItemSO;
        itemImage.sprite = itemSO.icon;
        itemNameText.text = itemSO.itemName;
        this.price = price;
        priceText.text = price.ToString();
    }

    public void OnBuyClicked()
    {
        shopManager.TryBuyItem(itemSO, price);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        shopManager.ItemDescriptionText(itemSO);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        shopManager.ClearDescriptionText();
    }
}
