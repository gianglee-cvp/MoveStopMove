using TMPro;
using UnityEngine;

public class CanvasLose : UICanvas
{
    [SerializeField] protected TextMeshProUGUI rankTMP;
    [SerializeField] protected TextMeshProUGUI nameTMP;
    [SerializeField] protected TextMeshProUGUI goldTMP;
    Coroutine countCoroutine;
    public void Init(int rank , string name ,  int gold , int goldAfterBoost)
    {
        rankTMP.text = rank.ToString();
        nameTMP.text = name;
        if(countCoroutine != null)
        {
            StopCoroutine(countCoroutine);
            countCoroutine = null;
        }
        countCoroutine = StartCoroutine(Helper.Count(gold , goldAfterBoost , 1f , SetGoldText ));
        Debug.Log("gold" + gold + goldAfterBoost);
    }
    public void SetGoldText(int value)
    {
        goldTMP.text = value.ToString();
    }
    public override void Setup()
    {
        base.Setup();
    }

    public override void Open()
    {
        base.Open();
    }

    public void MainMenuButton()
    {
        Close(0);
        GameManager.Instance.ReturnToMainMenu();
    }
    public override void CloseDirectly()
    {
        base.CloseDirectly();
        StopCoroutine(countCoroutine);
        countCoroutine = null;
    }

}
