using System.Collections.Generic;
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
    protected CardType currentCard = CardType.Init;
    public override void Setup()
    {
        base.Setup();
        for(int i = 0 ; i < listCard.Count; i++)
        {
            listCard[i].OnInit();
        }
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
        listCard[cardType].Show();
    }
    public void SetAlpha(Button button , float al)
    {
        Color color = button.image.color;
        color.a = al;
        button.image.color = color;
    }
    public void TryCloth(int index , SkinType type)
    {
        LevelManager.Instance.PlayerTrySkin(index, type);
    }
    public void CloseButton()
    {
        Close(0);
        GameManager.Instance.ChangeGameState(Enum_GameState.MainMenu);
        UIManager.Instance.OpenUI<CanvasMainMenu>();
        LevelManager.Instance.ReloadCloth();
    }
    public override void CloseDirectly()
    {
        base.CloseDirectly();
        LevelManager.Instance.ChangeAnimPlayer(CharacterAnimType.Idle);
    }
}