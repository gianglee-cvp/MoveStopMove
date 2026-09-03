using TMPro;
using UnityEngine;
public class CanvasGamePlay : UICanvas
{
    [SerializeField] protected TargetContainer targetContainer;
    [SerializeField] protected TouchZone touchZone;
    [SerializeField] protected TextMeshProUGUI rankTMP;
    private int lastRank = -1;
    public void RegisterTarget(Character character)
    {
        targetContainer.RegisterTarget(character);
    }
    public void UnregisterTarget(Character character)
    {
        targetContainer.UnregisterTarget(character);
    }
    public override void Setup()
    {
        base.Setup();
        ActiveJoystick();
    }
    void Update()
    {
        if(!GameManager.Instance.IsGameState(Enum_GameState.Play)) return;
        UpdateRank();
    }

    public void SetActive(bool isActive)
    {
        touchZone.gameObject.SetActive(isActive);
        targetContainer.SetActiveTarget(isActive);
    }
    public void ReleaseJoystick()
    {
        touchZone.ReleaseJoystick();
    }
    public void ActiveJoystick()
    {
        touchZone.ActiveJoyStick();
    }
    public void UpdateRank()
    {
        int cnt = BotManager.Instance.BotActiveCount() + 1;
        if (cnt == lastRank) return;
        lastRank = cnt;
        rankTMP.text = cnt.ToString();

        if (cnt == 1)
        {
            Close(0);
            UIManager.Instance.OpenUI<CanvasWin>();
        }
    }
}
