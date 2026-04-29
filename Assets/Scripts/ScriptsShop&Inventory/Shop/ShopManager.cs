using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using System;
public class ShopManager : MonoBehaviour
{
    public static event Action<ShopManager, bool> OnShopStateChanged;

    [SerializeField] private List<ShopItems> shopItems;

    [SerializeField] private ShopSlot[] shopSlots;

    [SerializeField] private ShopInfo shopInfo;

    private void Start()
    {
        PopulateShopItems();
        OnShopStateChanged?.Invoke(this, true);

        CoinsManager.coins = 10;
        Debug.Log(CoinsManager.coins);
    }
    public void PopulateShopItems()
    {
        for (int i = 0; i < shopItems.Count && i < shopSlots.Length; i++)
        {
            ShopItems shopItem = shopItems[i];
            shopSlots[i].Initialize(shopItem.itemSO, shopItem.price);
            shopSlots[i].gameObject.SetActive(true);
        }

        for (int i = shopItems.Count; i < shopSlots.Length; i++)
        {
            shopSlots[i].gameObject.SetActive(false);
        }
    }

    public void TryBuyItem(ItemSO itemSO, int price)
    {
        if (itemSO != null && CoinsManager.coins >= price)
        {
            CoinsManager.coins -= price;
            Debug.Log("Kan kopen");
            Debug.Log(CoinsManager.coins);
        }
    }

    public void ItemDescriptionText(ItemSO itemSO)
    {
        shopInfo.ShowItemInfo(itemSO);
    }

    public void ClearDescriptionText()
    {
        shopInfo.itemDescriptionText.text = "";
    }
}

[System.Serializable]
public class ShopItems
{
    public ItemSO itemSO;
    public int price;
}
