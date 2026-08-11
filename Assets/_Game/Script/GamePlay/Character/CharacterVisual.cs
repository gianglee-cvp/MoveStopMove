using System;
using System.Data.Common;
using Unity.VisualScripting;
using UnityEngine;
[Serializable]
public struct baseItem
{
    public WeaponBase currentWeapon;
    public BulletBase currentBullet;
}
public class CharacterVisual : MonoBehaviour
{
    protected CharacterAnimType currentAnim;
    [SerializeField] protected WeaponBase currentWeapon;
    [SerializeField] protected baseItem item;
    [SerializeField] protected Animator animator; 
    [SerializeField] protected Transform righHandTF;    
    [SerializeField] protected Transform shootPoint;
    [SerializeField] protected Character character;
    protected bool isAttacking => character.CheckAttack();

    public void OnInit()
    {
        currentWeapon.OnInit(righHandTF);
    }
    public void ChangeAnim(CharacterAnimType newAnim)
    {
        if(currentAnim != newAnim)
        {
            animator.ResetTrigger(currentAnim.ToString());
            currentAnim = newAnim;
            animator.SetTrigger(newAnim.ToString());
        }
    }
    public void ChangeAcessory()
    {
        
    }
    public void ChangeRotation(Vector3 move)
    {
        transform.rotation = Quaternion.LookRotation(move);
    }
    public void Attack()
    {
        ChangeAnim(CharacterAnimType.Attack);
    }
    public void InitThrow()
    {
        item.currentWeapon.OnDeactive();
        BulletBase bullet = Instantiate(item.currentBullet);
        bullet.Init(shootPoint);        
        item.currentBullet = bullet;
    }
    public void Throw(Vector3 des)
    {
        InitThrow();
        item.currentBullet.Throw(des);
    }
    public void ActiveWeapon()
    {
        item.currentWeapon.OnInit(righHandTF);
        // character.SetAttackState(false);
    }
}