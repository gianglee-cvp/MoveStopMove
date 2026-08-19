using UnityEngine;
public class CanvasGamePlay : UICanvas
{
    [SerializeField] protected TargetContainer targetContainer;
    public void RegisterTarget(Bot bot)
    {
        targetContainer.RegisterTarget(bot);
    }
    public void UnregisterTarget(Bot bot)
    {
        targetContainer.UnregisterTarget(bot);
    }
}