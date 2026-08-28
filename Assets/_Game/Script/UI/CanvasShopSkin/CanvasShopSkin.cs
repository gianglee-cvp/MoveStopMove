using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum CardType
{
    Hair = 0,
    Pant = 1,
    Shield = 2, 
    Init = 3
}
public class CanvasShopSkin : UICanvas
{
    [SerializeField] protected List<ScrollView> listCard;
    [SerializeField] protected List<Button> listButtonCard;
    [SerializeField] protected TextMeshProUGUI desText;
    [SerializeField] protected TextMeshProUGUI priceText;
    protected CardType currentCard = CardType.Init;
    protected ItemData selectedItem;
    protected ScrollView selectedScrollView;
    [SerializeField] protected List<Button> buttonsSelect = new List<Button>();
    void Awake()
    {
        selectedItem = null;
        selectedScrollView = null;
        for(int i = 0 ; i < listCard.Count; i++)
        {
            listCard[i].OnInit();
        }
    }
    public override void Setup()
    {
        base.Setup();
        SelectCard(0);
        LevelManager.Instance.ChangeAnimPlayer(CharacterAnimType.Dance);
    }
    public void SelectCard(int cardType)
    {
        if(cardType == (int)currentCard) return;
        for(int i = 0 ; i < listButtonCard.Count ; i++)
        {
            SetAlpha(listButtonCard[i] , 1f);
            listCard[i].Hide();
        }
        SetAlpha(listButtonCard[cardType] , 0f);
        listCard[cardType].SelectInitialItem();
        listCard[cardType].Show();
    }
    public void SetAlpha(Button button , float al)
    {
        Color color = button.image.color;
        color.a = al;
        button.image.color = color;
    }
    public void TryCloth(int index , SkinType type, string des , int price, ScrollView owner)
    {
        selectedItem = DataManager.Instance.GetItemData<ItemData>(type, index);
        selectedScrollView = owner;
        LevelManager.Instance.PlayerTrySkin(index, type);
        desText.text = des;
        priceText.text = price.ToString();
    }
    public void CloseButton()
    {
        Close(0);
        GameManager.Instance.ChangeGameState(Enum_GameState.MainMenu);
        UIManager.Instance.OpenUI<CanvasMainMenu>();
        LevelManager.Instance.ReloadCloth();
    }
    public void BuyButton()
    {
        ItemData targetItem = selectedItem;
        if (targetItem == null) return;
        int oldEquippedIndex = DataManager.Instance.GetEquippedID(targetItem.Type);
        if (!DataManager.Instance.BuyItem(targetItem)) return;

        LevelManager.Instance.ReloadCloth();
        selectedItem = targetItem;
        if (selectedScrollView != null)
        {
            selectedScrollView.RefreshItemState(oldEquippedIndex);
            selectedScrollView.RefreshItemState(targetItem.Index);
        }
        ChangeButtonBuy(Enum_ShopState.equipped);

    }
    public void ChangeButtonBuy(Enum_ShopState type)
    {
        for(int i = 0 ; i < buttonsSelect.Count ; i++)
        {
            buttonsSelect[i].gameObject.SetActive(false);
        }
        buttonsSelect[(int)type].gameObject.SetActive(true);
    }
    public void ButtonSelect()
    {
        if (selectedScrollView.SelectItem(selectedItem.Index))
        {
            ChangeButtonBuy(Enum_ShopState.equipped);
        }
    }
    public override void CloseDirectly()
    {
        base.CloseDirectly();
        LevelManager.Instance.ChangeAnimPlayer(CharacterAnimType.Idle);
    }
    public void SetDesText(String s)
    {
        desText.text = s;
    }
}
