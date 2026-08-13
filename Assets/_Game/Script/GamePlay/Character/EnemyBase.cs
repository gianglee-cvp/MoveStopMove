using UnityEngine;
using NavMesh;
public class EnemyBase : Character
{
    protected NavMeshAI agent;
    //TODO co the doi thanh code khong dung gamobject
    [SerializeField] GameObject indicator;
    void Update()
    {
        SetTarget();
    }
    public void ShowTargetIndicator()
    {
        if (!indicator.activeSelf)
        {
            indicator.SetActive(true);
        }
    }
    public void HideTargetIndicator()
    {
        if (indicator.activeSelf)
        {
            indicator.SetActive(false);
        }
    }
}   