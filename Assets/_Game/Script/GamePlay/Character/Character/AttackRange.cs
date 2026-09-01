using System;
using Unity.VisualScripting;
using UnityEngine;
public class AttackRange : MonoBehaviour
{
    [SerializeField] Character character;
    [SerializeField] GameObject attackRangeVisual;
    [SerializeField] SphereCollider attackCol; 

    public void OnTriggerEnter(Collider other)
    {
        Character ch = CacheComponent<Collider,Character>.Get(other);
        if(ch == null) return;
        character.AddTarget(ch);
    }
    public void UpdateRange(float range)
    {
        if(attackRangeVisual != null)
        {
            attackRangeVisual.transform.localScale = Vector3.one * 2 * range;
        }
        attackCol.radius = range;
    }
    public void SetActiveUI(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}
