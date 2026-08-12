using System;
using UnityEngine;
public class AttackRange : MonoBehaviour
{
    [SerializeField] Character character;
    public void OnTriggerEnter(Collider other)
    {
        //Cache collder
        if (other.CompareTag("Enemy"))
        {
            Character target = other.GetComponent<Character>();
            character.AddTarget(target);
        }
    }
}