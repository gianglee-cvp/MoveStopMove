using UnityEngine;
using UnityEngine.InputSystem;
public class Test : MonoBehaviour
{
    public Character character;
    public PoolControl poolControl;
    public void Awake()
    {
        Debug.Log("1");
        poolControl.OnInit();
        InputManager.Instance.OnInit();
        character.OnInit();
    }

}