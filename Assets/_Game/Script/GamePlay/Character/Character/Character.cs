using System.Collections;
using System.Collections.Generic;
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
    //attack
    protected Coroutine attackCO;
    //TODO đổi thành protected
    public List<Character> listTarget = new List<Character>();
    [SerializeField] protected Character currentTarget;

    public Vector3 Pos => TF.position;
    public float Range => characterLevel.Range; 
    public Vector3 Scale => characterLevel.Scale;
    public float Size => characterLevel.Size;
    public int Level => characterLevel.Level;
    public int Exp => characterLevel.Exp;
    public float capsuleRadius => physicColider.radius;
    #region State
    public virtual void OnInit()
    {
        characterVisual.OnInit();
        listTarget.Clear();
        isDead = false;
        LevelUp(0);
        currentTarget = null;
        characterVisual.ChangeAnim(CharacterAnimType.Idle);
        RefreshAttackableState();
    }
    public virtual void Idle()
    {
        characterVisual.ChangeAnim(CharacterAnimType.Idle);
        isAttacking = false;
        isMoving = false;
        RefreshAttackableState();
    }
    public virtual void OnDead()
    {
        ParticleEffect vfx = SimplePool.Spawn<ParticleEffect>(PoolType.ParticleEffectHit,spawnEffectPoint.position,Quaternion.identity,spawnEffectPoint);
        vfx.Play(characterVisual.GetCurrentColorType() , 1f);
        isDead = true;
        isAttackable = false;
        CancelAttack();
        characterVisual.ChangeAnim(CharacterAnimType.Dead);
    }
    #endregion
    #region Attack
    public virtual void Attack()
    {
        if(currentTarget == null)
        {
            Idle();
            return;
        } 
        RotateToTarget(currentTarget.Pos);
        isAttacking  = true;
        isAttackable = false;
        isMoving = false;
        characterVisual.ChangeAnim(CharacterAnimType.Attack);
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
        attackCO = null;
    }
    public virtual void CancelAttack()
    {
        if (attackCO != null)
        {
            StopCoroutine(attackCO);
            attackCO = null;
        }
        isAttacking = false;
        RefreshAttackableState();
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
            float offsetRange = target.capsuleRadius;
            bool targetOutRange = Helper.CheckDistanceOutRange(Pos,target.Pos,characterLevel.Range + offsetRange);
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
    public void RotateToTarget(Vector3 des)
    {
        Vector3 direction = des - transform.position;
        direction.y = 0f;
        TF.rotation = Quaternion.LookRotation(direction);
    }
    #endregion
    #region ChangeStat
    public virtual void LevelUp(int level)
    {
        if(isDead) return;
        characterLevel.PowerUp(level);
        if(level != 0)
        {
            SimplePool.Spawn<ParticleEffect>(PoolType.ParticleEffectLevelUp,spawnEffectPoint.position,Quaternion.identity,spawnEffectPoint).Play(characterVisual.color, 1f);
        }
    }
    public virtual void CollectExp(int exp)
    {
        int newLevel = characterLevel.CalculateExp(exp);
        if (newLevel != characterLevel.Level)
        {
            LevelUp(newLevel);
        }
    }
    public virtual void ResetBoosters()
    {
        characterLevel.ResetBoosters();
    }
    public virtual void ApplyRangeBooster(float rangeBonusPercent)
    {
        characterLevel.AddRangeBonus(rangeBonusPercent);
    }
    public virtual void ApplySpeedBooster(float moveSpeedBonusPercent)
    {
        characterLevel.AddMoveSpeedBonus(moveSpeedBonusPercent);
    }
    #endregion
    public ColorType GetColor()
    {
        return characterVisual.color;
    }
    public virtual void OnBulletDespawn(BulletBase bullet)
    {
        if (isDead) return;

        characterVisual.OnBulletDespawn(bullet);
        RefreshAttackableState();
    }
    public void OnWeaponStateChanged()
    {
        RefreshAttackableState();
    }
    protected void RefreshAttackableState()
    {
        isAttackable = !isDead && characterVisual.CheckWeaponActive ;
    }
}
