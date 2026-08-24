using UnityEngine;
public class CanvasMainMenu : UICanvas
{
    public override void Setup()
    {
        base.Setup();
    }
    public void ButtonPlay()
    {
        GameManager.Instance.PlayGame();
    }
    public void ShopSkinButton()
    {
        //TODO lam transition di ra ngoai 
        Close(0);
        GameManager.Instance.ChangeGameState(Enum_GameState.ShopSkin);
        UIManager.Instance.OpenUI<CanvasShopSkin>();
    }
}
