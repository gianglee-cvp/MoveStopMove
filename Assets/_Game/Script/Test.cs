using UnityEngine;
using UnityEngine.InputSystem;
public class Test : MonoBehaviour
{
    public Character character;
    public PoolControl poolControl;
    public void Awake()
    {
        poolControl.OnInit();
        InputManager.Instance.OnInit();
        character.OnInit();
    }

}