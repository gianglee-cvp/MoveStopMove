using System;
using TMPro;
using UnityEngine;

public class CanvasLose : UICanvas
{
    [SerializeField] protected TextMeshProUGUI rankTMP;
    [SerializeField] protected TextMeshProUGUI nameTMP;
    [SerializeField] protected TextMeshProUGUI goldTMP;
    public void Init(int rank , string name ,  int gold)
    {
        rankTMP.text = rank.ToString();
        nameTMP.text = name;
        goldTMP.text = gold.ToString();
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
}
