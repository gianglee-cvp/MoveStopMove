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
        //TODO lam transition di ra ngoai khong dung close 
        Close(0);
        GameManager.Instance.ChangeGameState(Enum_GameState.ShopSkin);
        UIManager.Instance.OpenUI<CanvasShopSkin>();
    }
    public void ShopWeaponButton()
    {
        Close(0);
        GameManager.Instance.ChangeGameState(Enum_GameState.ShopWeapon);
        UIManager.Instance.OpenUI<CanvasShopWeapon>();
        DataManager.Instance.SaveGame();
    }
}
