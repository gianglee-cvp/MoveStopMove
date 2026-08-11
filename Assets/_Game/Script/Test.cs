using UnityEngine;
using UnityEngine.InputSystem;
public class Test : MonoBehaviour
{
    public Character character;
    public void Awake()
    {
        InputManager.Instance.OnInit();
        character.OnInit();
    }

}