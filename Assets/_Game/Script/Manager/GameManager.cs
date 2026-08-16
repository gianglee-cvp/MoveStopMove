using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class GameManager : MonoBehaviour
{
    public List<Character> list = new List<Character>();
    public PoolControl poolControl;
    public void Awake()
    {
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
