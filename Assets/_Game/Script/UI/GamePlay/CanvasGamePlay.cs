using UnityEngine;
public class CanvasGamePlay : UICanvas
{
    [SerializeField] protected TargetContainer targetContainer;
    [SerializeField] protected TouchZone touchZone;
    public void RegisterTarget(Character character)
    {
        targetContainer.RegisterTarget(character);
    }
    public void UnregisterTarget(Character character)
    {
        targetContainer.UnregisterTarget(character);
    }
    public void SetActive(bool isActive)
    {
        touchZone.gameObject.SetActive(isActive);
        targetContainer.SetActiveTarget(isActive);
    }
}
