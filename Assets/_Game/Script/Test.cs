using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Test : MonoBehaviour
{
    public Character character;
    public List<Character> list = new List<Character>();
    public PoolControl poolControl;
    public BotManager botManager;
    public void Awake()
    {
        poolControl.OnInit();
        InputManager.Instance.OnInit();
        botManager.Init();
        // character.OnInit();
        foreach(var ch in list)
        {
            ch.OnInit();
        }
    }

}