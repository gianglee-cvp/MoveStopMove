using UnityEngine;
using UnityEngine.InputSystem;
public class Test : MonoBehaviour
{
    public Character character;
    public void Awake()
    {
        Debug.Log("1");
        InputManager.Instance.OnInit();
        character.OnInit();
    }

}