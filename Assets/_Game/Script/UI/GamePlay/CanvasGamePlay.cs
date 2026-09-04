using System;
using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class CanvasGamePlay : UICanvas
{
    [SerializeField] protected TargetContainer targetContainer;
    [SerializeField] protected TouchZone touchZone;
    [SerializeField] protected TextMeshProUGUI rankTMP;
    [SerializeField] protected List<ButtonSeting> buttonSetings;
    [SerializeField] protected Image imgTut;
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
        ResetSettingButtons();
        SetActiveTut(true);
    }
    public override void CloseDirectly()
    {
        base.CloseDirectly();
        ResetSettingButtons();
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
    protected bool isSettingOpen = false;
    protected bool isAnimating = false;

    public void ResetSettingButtons()
    {
        isSettingOpen = false;
        isAnimating = false;
        
        foreach (ButtonSeting but in buttonSetings)
        {
            if (but != null)
            {
                but.gameObject.SetActive(false);
            }
        }
    }

    public void SettingButton()
    {
        if (isAnimating) return;
        if (buttonSetings == null || buttonSetings.Count == 0) return;

        isAnimating = true;
        // isSettingOpen = !isSettingOpen;

        PlaySettingsAnimation(!isSettingOpen);
    }
    public void PlaySettingsAnimation(bool open)
    {
        
        int total = buttonSetings.Count;
        int completed = 0;

        Action checkComplete = () =>
        {
            completed++;

            if (completed >= total)
            {
                isAnimating = false;
                isSettingOpen = open;
            }
        };

        foreach (ButtonSeting but in buttonSetings)
        {
            if (but == null)
            {
                checkComplete();
                continue;
            }

            if (open)
                but.Open(checkComplete);
            else
                but.Close(checkComplete);
        }
    }

    public void ButtonSound()
    {
        Debug.Log("Button Sound");
    }

    public void ButtonRetry()
    {
        Debug.Log("Button Retry");
        ResetSettingButtons();
        if(LevelManager.Instance.GetPlayer().IsDead) return;
        
        LevelManager.Instance.Retry();
        GameManager.Instance.PlayGame();
    }

    public void ButtonHome()
    {
        Debug.Log("Button Home");
        ResetSettingButtons();
        GameManager.Instance.ReturnToMainMenu();
    }
    public void SetActiveTut(bool isActive)
    {
        imgTut.gameObject.SetActive(isActive);
    }
}
