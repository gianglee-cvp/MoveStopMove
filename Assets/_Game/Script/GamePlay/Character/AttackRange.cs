using System;
using UnityEngine;
public class AttackRange : MonoBehaviour
{
    [SerializeField] Character character;
    public void OnTriggerEnter(Collider other)
    {
        Character ch = CacheComponent<Collider,Character>.Get(other);
        if(ch == null) return;
        Debug.Log("trigger");
        character.AddTarget(ch);
    }
}