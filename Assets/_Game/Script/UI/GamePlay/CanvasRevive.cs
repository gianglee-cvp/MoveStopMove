using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class CanvasRevive : UICanvas
{
    [SerializeField] protected Button btnRevive;
    [SerializeField] protected Button btnSkip;
    [SerializeField] protected Image imgRevive;
    [SerializeField] protected TextMeshProUGUI textRevive;
    protected bool isCountDown;
    [SerializeField] protected float rotateSpeed;

    public override void Setup()
    {
        base.Setup();
    }
    public override void Open()
    {
        base.Open();
        isCountDown = true;
        UIManager.Instance.GetUI<CanvasGamePlay>().SetActiveTut(false);
        UIManager.Instance.GetUI<CanvasGamePlay>().PlaySettingsAnimation(false);
        StartCoroutine(Countdown());
        StartCoroutine(Rotate());
    }
    public void SkipButton()
    {
        StopCountdown();
        Close(0);
        UIManager.Instance.OpenUI<CanvasLose>();
    }
    public void SaveMeButton()
    {
        const int reviveCost = 500;
        if (DataManager.Instance.TrySpendGold(reviveCost))
        {
            StopCountdown();
            Close(0);
            LevelManager.Instance.GetPlayer().SaveMe();
            UIManager.Instance.GetUI<CanvasGamePlay>().ActiveJoystick();
        }
        else
        {
            Debug.Log("dont enough gold");
        }
    }
    public IEnumerator Countdown()
    {
        for (int i = 5; i >= 0; i--)
        {
            textRevive.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }
        isCountDown = false;
        SkipButton();
    }
    protected IEnumerator Rotate()
    {
        float timer = 0f;

        while (timer < 7f)
        {
            imgRevive.transform.Rotate(0f,0f,rotateSpeed * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }
    }
    public void StopCountdown()
    {
        StopAllCoroutines();
    }
    public override void CloseDirectly()
    {
        base.CloseDirectly();
        StopCountdown();
    }
}
