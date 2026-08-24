using UnityEngine;
public class CanvasGamePlay : UICanvas
{
    [SerializeField] protected TargetContainer targetContainer;
    public void RegisterTarget(Character character)
    {
        targetContainer.RegisterTarget(character);
    }
    public void UnregisterTarget(Character character)
    {
        targetContainer.UnregisterTarget(character);
    }
}
