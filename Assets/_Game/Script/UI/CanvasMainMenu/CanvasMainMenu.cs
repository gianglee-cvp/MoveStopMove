using UnityEngine;
public class CanvasMainMenu : UICanvas
{
    public void ButtonPlay()
    {
        GameManager.Instance.PlayGame();
    }
}