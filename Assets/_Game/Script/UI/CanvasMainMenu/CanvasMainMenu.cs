using UnityEngine;
public class CanvasMainMenu : UICanvas
{
    public override void Setup()
    {
        base.Setup();
        CameraFollow.Instance.ChangeByState(Enum_GameState.MainMenu, true);
    }
    public void ButtonPlay()
    {
        GameManager.Instance.PlayGame();
    }
    public void ShopSkinButton()
    {
        //TODO lam transition di ra ngoai 
        Close(0);
        UIManager.Instance.OpenUI<CanvasShopSkin>();
    }
}