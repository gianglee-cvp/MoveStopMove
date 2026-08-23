using UnityEngine;
public class CanvasMainMenu : UICanvas
{
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