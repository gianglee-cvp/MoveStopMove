using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
public enum CharacterAnimType
{
    Idle = 0, 
    Run = 1,
    Attack = 2,
    Win = 3,
    Dance = 4,
    Dead = 5,
    Ulti = 6
}

public class Character : GameUnit
{
    public Vector3 pos
    {
        get => TF.position;
    }
    [SerializeField] protected CharacterVisual characterVisual;
    [SerializeField] protected float scale; // TODO thay doi khi cho vao level up


    [SerializeField] protected bool isMoving = false;
    [SerializeField] protected bool isAttacking = false;
    [SerializeField] protected bool isAttackable = true;

    protected Coroutine attackCO;
    [SerializeField] protected float range;
    protected float offsetRange = 0f;
    //TODO đổi thành protected
    public List<Character> listTarget = new List<Character>();
    [SerializeField] protected Character currentTarget;
    public float Range
    {
        get => range;
    }
    public virtual void OnInit()
    {
        characterVisual.OnInit();
        listTarget.Clear();
        currentTarget = null;
        ChangeAnim(CharacterAnimType.Idle);
    }
    public virtual void Idle()
    {
        ChangeAnim(CharacterAnimType.Idle);
        isAttacking = false;
        isAttackable  = true;
        isMoving = false;
    }
    public virtual void Attack()
    {
        if(currentTarget == null)
        {
            Idle();
            return;
        } 
        RotateToTarget(currentTarget.pos);
        isAttacking  = true;
        isAttackable = false;
        isMoving = false;
        ChangeAnim(CharacterAnimType.Attack);
        attackCO = StartCoroutine(Throw(TF.position)); 
    }
    protected IEnumerator Throw(Vector3 pos)
    {
        yield return new WaitForSeconds(Constant.THROW_DELAY_TIME);
        if (isAttacking)
        {
            characterVisual.Throw(pos);
        }
        isAttacking = false;
        yield return new WaitForSeconds(Constant.ATTACK_RECOVERY_TIME);
        Idle();
        characterVisual.ActiveWeapon();
        attackCO = null;
    }
    public virtual void CancelAttack()
    {
        if (attackCO != null)
        {
            StopCoroutine(attackCO);
            attackCO = null;
        }
        Idle();
    }

    public virtual void ChangeAnim(CharacterAnimType type)
    {
        characterVisual.ChangeAnim(type);
    }
    public virtual void AddTarget(Character ch)
    {
        listTarget.Add(ch);
    }
    public virtual Character SetTarget()
    {
        int cnt = listTarget.Count; 
        if(cnt == 0)
        {
            currentTarget = null;
            return currentTarget;
        } 
        Character finalTarget  = null;
        for(int i = cnt -1 ;  i >=0 ; i--)
        {
            Character target = listTarget[i];
            offsetRange = target.GetOffsetRange();
            bool targetOutRange = Helper.CheckDistanceOutRange(pos,target.pos,range + offsetRange);
            bool isTargetDead = !target.gameObject.activeSelf;
            if (isTargetDead || targetOutRange)
            {
                RemoveTarget(i);
            }
            else
            {
                finalTarget = listTarget[i];
            }
        }
        currentTarget = finalTarget;
        return finalTarget; 
        
    }
    public virtual void RemoveTarget(int index)
    {
        if (index < 0 || index >= listTarget.Count) return;
        listTarget.RemoveAt(index);
    }
    public virtual void RemoveTarget(Character target)
    {
        listTarget.Remove(target);
    }
    public virtual float GetOffsetRange()
    {
        //TODO cho vao cache;
        return GetComponent<CapsuleCollider>().radius * scale;
    }
    public virtual void OnDead()
    {
        ChangeAnim(CharacterAnimType.Dead);
    }
    public void RotateToTarget(Vector3 des)
    {
        Vector3 direction = des - transform.position;
        direction.y = 0f;
        TF.rotation = Quaternion.LookRotation(direction);
    }
}
