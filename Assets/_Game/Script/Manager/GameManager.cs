using System.Collections.Generic;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    public List<Character> list = new List<Character>();
    public PoolControl poolControl;
    public void Awake()
    {
        UIManager.Instance.OnInit();
        UIManager.Instance.OpenUI<CanvasGamePlay>();
        CameraFollow.Instance.OnInit();
        poolControl.OnInit();
        DataManager.Instance.OnInit();
        InputManager.Instance.OnInit();
        foreach(var ch in list)
        {
            ch.OnInit();
        }
        LevelManager.Instance.OnInit();
    }

}
