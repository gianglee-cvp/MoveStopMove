using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Enum_GameState
{
    Loading,
    MainMenu,
    ShopWeapon,
    ShopSkin,
    Play
}
public class GameManager : Singleton<GameManager>
{
    [SerializeField] protected PoolControl poolControl;
    [SerializeField] private bool isLoaded;
    [SerializeField] private float loadingFadeDuration = 0.25f;
    protected Enum_GameState  currentGameState;

    public bool IsLoaded => isLoaded;

    public void Awake()
    {
        isLoaded = false;
        currentGameState = Enum_GameState.Loading;
        UIManager.Instance.OnInit();
        UIManager.Instance.OpenUI<CanvasLoading>();
        StartCoroutine(BootstrapGame());
    }

    private IEnumerator BootstrapGame()
    {
        //TODO xem logic bien nay can khong 

        yield return null;

        CameraFollow.Instance.OnInit();
        poolControl.OnInit();
        DataManager.Instance.OnInit();
        InputManager.Instance.OnInit();
        LevelManager.Instance.OnInit();

        isLoaded = true;

        CanvasLoading loadingCanvas = UIManager.Instance.GetUI<CanvasLoading>();
        float fadeTime = loadingCanvas != null ? loadingCanvas.FadeDuration : loadingFadeDuration;
        yield return new WaitForSeconds(fadeTime);
        UIManager.Instance.OpenUI<CanvasMainMenu>();
        currentGameState = Enum_GameState.MainMenu;
    }
    public void PlayGame()
    {
        ChangeGameState(Enum_GameState.Play);
        UIManager.Instance.CloseAllUI();
        UIManager.Instance.OpenUI<CanvasGamePlay>();
        LevelManager.Instance.StartGame();
    }
    public void ChangeGameState(Enum_GameState state)
    {
        currentGameState = state;
        CameraFollow.Instance.ChangeByState(state , false);
    }
    public bool IsGameState(Enum_GameState state)
    {
        return currentGameState == state;
    }
    public void ReturnToMainMenu()
    {
        LevelManager.Instance.EndGame();
        LevelManager.Instance.OnInit();
        UIManager.Instance.CloseAllUI();
        UIManager.Instance.OpenUI<CanvasMainMenu>();
        ChangeGameState(Enum_GameState.MainMenu);
    }
}
