using System;
using Unity.VisualScripting;
using UnityEngine;
public class AttackRange : MonoBehaviour
{
    [SerializeField] Character character;

    public void OnTriggerEnter(Collider other)
    {
        Character ch = CacheComponent<Collider,Character>.Get(other);
        if(ch == null) return;
        character.AddTarget(ch);
    }
    public void OnTriggerExit(Collider other)
    {
        Character ch = CacheComponent<Collider,Character>.Get(other);
        if(ch == null) return;
        character.RemoveTarget(ch);
    }
}
