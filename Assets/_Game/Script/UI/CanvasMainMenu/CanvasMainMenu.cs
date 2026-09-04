using TMPro;
using Unity.VisualScripting;
using UnityEngine;
public class CanvasMainMenu : UICanvas
{
    [SerializeField] TextMeshProUGUI goldText;
    public override void Setup()
    {
        base.Setup();
        goldText.text =  DataManager.Instance.Gold.ToString();
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
        LevelManager.Instance.GetPlayer().ChangeAnim(CharacterAnimType.Dance);
    }
    public void ShopWeaponButton()
    {
        Close(0);
        GameManager.Instance.ChangeGameState(Enum_GameState.ShopWeapon);
        UIManager.Instance.OpenUI<CanvasShopWeapon>();
        DataManager.Instance.SaveGame();
    }
}
