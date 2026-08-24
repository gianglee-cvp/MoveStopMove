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
    protected List<Enum_ShopState> shopStates; 
    public void OnInit()
    {
        DataManager.Instance.LoadGame(this);
        ItemData[] items = DataManager.Instance.GetListData(type);
        if (items.Length > shopStates.Count)
        {
            int missingCount = items.Length - shopStates.Count;
            for (int i = 0; i < missingCount; i++)
            {
                shopStates.Add(Enum_ShopState.buy);
            }
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
    public void CallBackTryCloth(int index , string desText , int price)
    {
        canvasHolder.TryCloth(index , type , desText , price, this);   
    }
    public void SelectInitialItem()
    {
        ItemData[] items = DataManager.Instance.GetListData(type);
        if (items == null || items.Length <= 1) return;

        int equippedIndex = DataManager.Instance.GetEquippedID(type);
        int initialIndex = equippedIndex > 0 ? equippedIndex : 1;

        ItemData item = items[initialIndex];
        if (item == null) return;

        canvasHolder.TryCloth(initialIndex, type, item.Des, item.Price, this);
    }
    public  void LoadGame(GameData data)
    {
        shopStates = data.GetShopStateByType(type);
    }
    //TODO them logic buy
    public  void SaveGame(ref GameData gameData)
    {
    }
    public void RefreshItemState(int index)
    {
        int cardIndex = index - 1;
        if (cardIndex < 0 || cardIndex >= cards.Count) return;

        cards[cardIndex].SetImgLock(shopStates[index] == Enum_ShopState.buy);
    }

}
