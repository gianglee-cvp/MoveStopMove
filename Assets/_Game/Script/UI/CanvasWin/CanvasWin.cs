using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CanvasWin : UICanvas
{
    [Header("Unlock Item Display")]
    [SerializeField] private Image unlockPanel;
    [SerializeField] private Image unlockIcon;
    [SerializeField] private float rotateSpeed;
    // [SerializeField] private TextMeshProUGUI unlockNameTMP;
    // [SerializeField] private TextMeshProUGUI unlockTypeTMP;

    public override void Setup()
    {
        base.Setup();
        ShowUnlockItem();
    }
    public override void Open()
    {
        base.Open();
        StartCoroutine(Rotate());
    }

    private void ShowUnlockItem()
    {
        ItemData unlockedItem = DataManager.Instance.UnlockRandomItem();

        if (unlockedItem == null)
        {
            if (unlockPanel != null) unlockPanel.gameObject.SetActive(false);
            return;
        }

        if (unlockPanel != null)   unlockPanel.gameObject.SetActive(true);
        if (unlockIcon != null)    unlockIcon.sprite = unlockedItem.Icon;
        // if (unlockNameTMP != null) unlockNameTMP.text = unlockedItem.ItemName;
        // if (unlockTypeTMP != null) unlockTypeTMP.text = unlockedItem.Type.ToString();
    }

    public void MainMenuButton()
    {
        Close(0);
        GameManager.Instance.ReturnToMainMenu();
    }
    protected IEnumerator Rotate()
    {
        float timer = 0f;

        while (timer < 999f)
        {
            unlockPanel.transform.Rotate(0f,0f,rotateSpeed * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }
    }
    public override void CloseDirectly()
    {
        base.CloseDirectly();
        StopAllCoroutines();
    }
}
