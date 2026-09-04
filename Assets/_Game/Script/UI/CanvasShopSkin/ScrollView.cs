using System;
using System.Collections.Generic;
using UnityEngine;
public class ScrollView : MonoBehaviour, IData
{
    [SerializeField] protected SkinType type;
    [SerializeField] protected List<CardItem> cards;
    [SerializeField] protected CardItem cardPrefab;
    [SerializeField] protected RectTransform cardHolder;
    [SerializeField] protected CanvasShopSkin canvasHolder;
    protected IReadOnlyList<Enum_ShopState> shopStates; 
    public void OnInit()
    {
        DataManager.Instance.LoadGame(this);
        ItemData[] items = DataManager.Instance.GetListData(type);
        if (items != null)
        {
            DataManager.Instance.EnsureShopStateCount(type, items.Length);
            shopStates = DataManager.Instance.GetShopStateByType(type);
        }
        for(int i = 1 ; i < items.Length ; i++)
        {
            CardItem card = SpawnCard();
            bool islock = shopStates[i] == 0;
            card.Init(islock,items[i].Icon , this, i , items[i].Des , items[i].Price);
            cards.Add(card);
        }
        gameObject.SetActive(false);
    }
    public CardItem SpawnCard()
    {
        return Instantiate(cardPrefab,cardHolder);

    }
    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    public void CallBackTryCloth(int index , string desText , int price , RectTransform item)
    {
        canvasHolder.TryCloth(index , type , desText , price, this , item);  
        canvasHolder.ChangeButtonBuy(shopStates[index]);
    }
    public void SelectInitialItem()
    {
        ItemData[] items = DataManager.Instance.GetListData(type);
        if (items == null || items.Length <= 1) return;

        int equippedIndex = DataManager.Instance.GetEquippedID(type);
        int initialIndex = equippedIndex > 0 ? equippedIndex : 1;

        ItemData item = items[initialIndex];
        if (item == null) return;

        canvasHolder.TryCloth(initialIndex, type, item.Des, item.Price, this , cards[ChangeIndexSOToIndexShop(initialIndex)].Rect);
        canvasHolder.ChangeButtonBuy(shopStates[initialIndex]);
        
    }
    public  void LoadGame(GameData data)
    {
        shopStates = data.GetShopStateByType(type);
    }
    //TODO them logic buy
    public  void SaveGame(GameData gameData)
    {
    }
    public void RefreshItemState(int index)
    {
        int cardIndex = ChangeIndexSOToIndexShop(index);
        if (cardIndex < 0 || cardIndex >= cards.Count) return;

        cards[cardIndex].SetImgLock(shopStates[index] == Enum_ShopState.buy);
    }
    public bool SelectItem(int index)
    {
        return DataManager.Instance.SelectItem(type, index);
    }
    public int ChangeIndexSOToIndexShop(int i)
    {
        return i-1;
    }
}
