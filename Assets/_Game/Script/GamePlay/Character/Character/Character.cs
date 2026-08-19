using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
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
    [SerializeField] protected CharacterLevel characterLevel;
    [SerializeField] protected Transform spawnEffectPoint;
    [SerializeField] protected CapsuleCollider physicColider;
    
    //character state
    [SerializeField] protected bool isMoving = false;
    [SerializeField] protected bool isAttacking = false;
    [SerializeField] protected bool isAttackable = true;
    [SerializeField] protected bool isDead = false;
    public bool IsDead{get => isDead;}


    protected Coroutine attackCO;
    protected float offsetRange = 0f; 
    
    //TODO đổi thành protected
    public List<Character> listTarget = new List<Character>();
    [SerializeField] protected Character currentTarget;
    public float Range
    {
        get => characterLevel.Range;
    }
    public Vector3 Scale => characterLevel.Scale;
    public float Size => characterLevel.Size;
    public int Level => characterLevel.Level;
    public int Exp => characterLevel.Exp;
    public float catchDistance => 0.5f * characterLevel.Size;
    public virtual void OnInit()
    {
        characterVisual.OnInit();
        listTarget.Clear();
        LevelUp(0);
        currentTarget = null;
        isDead = false;
        
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
            bool targetOutRange = Helper.CheckDistanceOutRange(pos,target.pos,characterLevel.Range + offsetRange);
            if (target.IsDead || targetOutRange)
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
        return physicColider.radius * characterLevel.Size;
    }
    public virtual void OnDead()
    {
        ParticleEffect vfx = SimplePool.Spawn<ParticleEffect>(PoolType.ParticleEffect,spawnEffectPoint.position,Quaternion.identity,spawnEffectPoint);
        vfx.Play(characterVisual.GetCurrentColorType());
        isDead = true;
        CancelAttack();
        ChangeAnim(CharacterAnimType.Dead);
    }
    public void RotateToTarget(Vector3 des)
    {
        Vector3 direction = des - transform.position;
        direction.y = 0f;
        TF.rotation = Quaternion.LookRotation(direction);
    }
    public virtual void LevelUp(int level)
    {
        characterLevel.PowerUp(level);
        // characterVisual.transform.localScale = characterLevel.Scale;

        // physicColider.radius = characterLevel.Size * Constant.CH_PHYSIC_COLLIDER_RADIUS;
        // physicColider.height = Constant.CH_PHYSIC_COLLIDER_HEIGHT * characterLevel.Size; 
        // physicColider.center = characterLevel.Size  * Constant.CH_PHYSIC_COLLIDER_CENTER; 
    }
    public virtual void CollectExp(int exp)
    {
        // characterLevel.IncreaseExp(exp);
        // Debug.Log("log1");
        int newLevel = characterLevel.CalculateExp(exp);
        if (newLevel != characterLevel.Level)
        {
            LevelUp(newLevel);
        }
    }
}
